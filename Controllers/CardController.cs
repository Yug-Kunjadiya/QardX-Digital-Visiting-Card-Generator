using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardX.Data;
using QardX.Models;
using QardX.Services;
using System.Security.Claims;

namespace QardX.Controllers
{
    [Authorize]
    public class CardController : Controller
    {
        private readonly QardXDbContext _context;
        private readonly IQRCodeService _qrCodeService;
        private readonly IFileUploadService _fileUploadService;

        public CardController(QardXDbContext context, IQRCodeService qrCodeService, IFileUploadService fileUploadService)
        {
            _context = context;
            _qrCodeService = qrCodeService;
            _fileUploadService = fileUploadService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var cards = await _context.VisitingCards
                .Include(c => c.User)
                .Include(c => c.Template)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(cards);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var templates = await _context.Templates.ToListAsync();
            ViewBag.Templates = templates;
            
            var user = await GetCurrentUser();
            
            // Split the user's full name into first and last name for initial population
            string firstName = "", lastName = "";
            if (!string.IsNullOrEmpty(user?.FullName))
            {
                var nameParts = user.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (nameParts.Length > 0)
                {
                    firstName = nameParts[0];
                    if (nameParts.Length > 1)
                    {
                        lastName = string.Join(" ", nameParts.Skip(1));
                    }
                }
            }
            
            var model = new VisitingCardViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                Email = user?.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VisitingCardViewModel model, IFormFile? logoFile)
        {
            if (ModelState.IsValid)
            {
                var userId = GetCurrentUserId();
                
                // Handle logo upload
                string? logoPath = null;
                if (logoFile != null)
                {
                    try
                    {
                        logoPath = await _fileUploadService.UploadFileAsync(logoFile, "logos");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("LogoFile", ex.Message);
                        var templates = await _context.Templates.ToListAsync();
                        ViewBag.Templates = templates;
                        return View(model);
                    }
                }
                
                var visitingCard = new VisitingCard
                {
                    UserId = userId,
                    FirstName = model.FirstName ?? string.Empty,
                    LastName = model.LastName ?? string.Empty,
                    Email = model.Email,
                    Phone = model.Phone,
                    LinkedIn = model.LinkedIn,
                    Company = model.Company,
                    Address = model.Address,
                    TemplateId = model.TemplateId,
                    CreatedAt = DateTime.Now,
                    // New fields
                    Instagram = model.Instagram,
                    Twitter = model.Twitter,
                    Facebook = model.Facebook,
                    Website = model.Website,
                    JobTitle = model.JobTitle,
                    Skills = model.Skills,
                    Languages = model.Languages,
                    AvailabilityStatus = model.AvailabilityStatus,
                    PrimaryColor = model.PrimaryColor,
                    SecondaryColor = model.SecondaryColor,
                    FontFamily = model.FontFamily,
                    CardOrientation = model.CardOrientation,
                    LogoPath = logoPath
                };

                _context.VisitingCards.Add(visitingCard);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Preview), new { id = visitingCard.CardId });
            }

            var templatesForError = await _context.Templates.ToListAsync();
            ViewBag.Templates = templatesForError;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetCurrentUserId();
            var card = await _context.VisitingCards
                .Include(c => c.User)
                .Include(c => c.Template)
                .FirstOrDefaultAsync(c => c.CardId == id && c.UserId == userId);

            if (card == null)
            {
                return NotFound();
            }

            var templates = await _context.Templates.ToListAsync();
            ViewBag.Templates = templates;

            var model = new VisitingCardViewModel
            {
                CardId = card.CardId,
                FirstName = card.FirstName,
                LastName = card.LastName,
                Email = card.Email ?? card.User.Email,
                Phone = card.Phone ?? string.Empty,
                LinkedIn = card.LinkedIn,
                Company = card.Company ?? string.Empty,
                Address = card.Address ?? string.Empty,
                TemplateId = card.TemplateId,
                // New fields
                Instagram = card.Instagram,
                Twitter = card.Twitter,
                Facebook = card.Facebook,
                Website = card.Website,
                JobTitle = card.JobTitle,
                Skills = card.Skills,
                Languages = card.Languages,
                AvailabilityStatus = card.AvailabilityStatus,
                PrimaryColor = card.PrimaryColor,
                SecondaryColor = card.SecondaryColor,
                FontFamily = card.FontFamily,
                CardOrientation = card.CardOrientation,
                LogoPath = card.LogoPath
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VisitingCardViewModel model, IFormFile? logoFile)
        {
            if (ModelState.IsValid)
            {
                var userId = GetCurrentUserId();
                var card = await _context.VisitingCards
                    .FirstOrDefaultAsync(c => c.CardId == model.CardId && c.UserId == userId);

                if (card == null)
                {
                    return NotFound();
                }

                // Handle logo upload
                if (logoFile != null)
                {
                    try
                    {
                        // Delete old logo if exists
                        if (!string.IsNullOrEmpty(card.LogoPath))
                        {
                            await _fileUploadService.DeleteFileAsync(card.LogoPath);
                        }
                        
                        card.LogoPath = await _fileUploadService.UploadFileAsync(logoFile, "logos");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("LogoFile", ex.Message);
                        var templates = await _context.Templates.ToListAsync();
                        ViewBag.Templates = templates;
                        return View(model);
                    }
                }

                card.FirstName = model.FirstName ?? string.Empty;
                card.LastName = model.LastName ?? string.Empty;
                card.Email = model.Email;
                card.Phone = model.Phone;
                card.LinkedIn = model.LinkedIn;
                card.Company = model.Company;
                card.Address = model.Address;
                card.TemplateId = model.TemplateId;
                // New fields
                card.Instagram = model.Instagram;
                card.Twitter = model.Twitter;
                card.Facebook = model.Facebook;
                card.Website = model.Website;
                card.JobTitle = model.JobTitle;
                card.Skills = model.Skills;
                card.Languages = model.Languages;
                card.AvailabilityStatus = model.AvailabilityStatus;
                card.PrimaryColor = model.PrimaryColor;
                card.SecondaryColor = model.SecondaryColor;
                card.FontFamily = model.FontFamily;
                card.CardOrientation = model.CardOrientation;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Preview), new { id = card.CardId });
            }

            var templatesForError = await _context.Templates.ToListAsync();
            ViewBag.Templates = templatesForError;
            return View(model);
        }

        public async Task<IActionResult> Preview(int id)
        {
            var userId = GetCurrentUserId();
            var card = await _context.VisitingCards
                .Include(c => c.User)
                .Include(c => c.Template)
                .FirstOrDefaultAsync(c => c.CardId == id && c.UserId == userId);

            if (card == null)
            {
                return NotFound();
            }

            // Generate QR Code with public URL
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var publicCardUrl = _qrCodeService.GeneratePublicCardUrl(card.CardId, baseUrl);
            var qrCodeImage = _qrCodeService.GenerateQRCode(publicCardUrl);

            var model = new VisitingCardViewModel
            {
                CardId = card.CardId,
                FirstName = card.FirstName,
                LastName = card.LastName,
                Email = card.Email ?? card.User.Email,
                Phone = card.Phone,
                LinkedIn = card.LinkedIn,
                Company = card.Company,
                Address = card.Address,
                TemplateId = card.TemplateId,
                // New fields
                Instagram = card.Instagram,
                Twitter = card.Twitter,
                Facebook = card.Facebook,
                Website = card.Website,
                JobTitle = card.JobTitle,
                Skills = card.Skills,
                Languages = card.Languages,
                AvailabilityStatus = card.AvailabilityStatus,
                PrimaryColor = card.PrimaryColor,
                SecondaryColor = card.SecondaryColor,
                FontFamily = card.FontFamily,
                CardOrientation = card.CardOrientation,
                LogoPath = card.LogoPath,
                // Template info
                TemplateName = card.Template.TemplateName,
                QRCodeImage = qrCodeImage
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetQRCode(int id)
        {
            var userId = GetCurrentUserId();
            var card = await _context.VisitingCards
                .Include(c => c.User)
                .Include(c => c.Template)
                .FirstOrDefaultAsync(c => c.CardId == id && c.UserId == userId);

            if (card == null)
            {
                return NotFound();
            }

            // Create a complete VisitingCardViewModel with all fields
            var cardViewModel = new VisitingCardViewModel
            {
                CardId = card.CardId,
                FirstName = card.FirstName,
                LastName = card.LastName,
                JobTitle = card.JobTitle,
                Phone = card.Phone,
                LinkedIn = card.LinkedIn,
                Instagram = card.Instagram,
                Twitter = card.Twitter,
                Facebook = card.Facebook,
                Website = card.Website,
                Skills = card.Skills,
                Languages = card.Languages,
                AvailabilityStatus = card.AvailabilityStatus,
                Company = card.Company,
                Address = card.Address,
                TemplateId = card.TemplateId,
                FullName = card.User.FullName,
                Email = card.Email ?? card.User.Email, // Use card email with fallback
                TemplateName = card.Template?.TemplateName,
                PrimaryColor = card.PrimaryColor,
                SecondaryColor = card.SecondaryColor,
                FontFamily = card.FontFamily,
                CardOrientation = card.CardOrientation,
                LogoPath = card.LogoPath
            };

            var vCardData = _qrCodeService.GenerateEnhancedVCardData(cardViewModel);
            var qrCodeImage = _qrCodeService.GenerateQRCode(vCardData);

            return File(qrCodeImage, "image/png");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            var card = await _context.VisitingCards
                .FirstOrDefaultAsync(c => c.CardId == id && c.UserId == userId);

            if (card == null)
            {
                return NotFound();
            }

            _context.VisitingCards.Remove(card);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        }

        private async Task<User?> GetCurrentUser()
        {
            var userId = GetCurrentUserId();
            return await _context.Users.FindAsync(userId);
        }
    }
}