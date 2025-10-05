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
    public class ExportController : Controller
    {
        private readonly QardXDbContext _context;
        private readonly IQRCodeService _qrCodeService;
        private readonly IExportService _exportService;

        public ExportController(QardXDbContext context, IQRCodeService qrCodeService, IExportService exportService)
        {
            _context = context;
            _qrCodeService = qrCodeService;
            _exportService = exportService;
        }

        [HttpGet("Export/PDF/{id}")]
        public async Task<IActionResult> ExportPDF(int id)
        {
            var card = await GetVisitingCard(id);
            if (card == null)
            {
                return NotFound();
            }

            var qrCodeImage = GenerateQRCode(card);
            var pdfBytes = _exportService.ExportToPDF(card, qrCodeImage);

            var fileName = $"VisitingCard_{card.FullName?.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

        [HttpGet("Export/PNG/{id}")]
        public async Task<IActionResult> ExportPNG(int id)
        {
            var card = await GetVisitingCard(id);
            if (card == null)
            {
                return NotFound();
            }

            var qrCodeImage = GenerateQRCode(card);
            var pngBytes = _exportService.ExportToPNG(card, qrCodeImage);

            var fileName = $"VisitingCard_{card.FullName?.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}.png";
            return File(pngBytes, "image/png", fileName);
        }

        private async Task<VisitingCardViewModel?> GetVisitingCard(int id)
        {
            var userId = GetCurrentUserId();
            var card = await _context.VisitingCards
                .Include(c => c.User)
                .Include(c => c.Template)
                .FirstOrDefaultAsync(c => c.CardId == id && c.UserId == userId);

            if (card == null)
            {
                return null;
            }

            return new VisitingCardViewModel
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
                Email = card.Email ?? card.User.Email, // Use card email with fallback to user email
                TemplateName = card.Template?.TemplateName,
                PrimaryColor = card.PrimaryColor,
                SecondaryColor = card.SecondaryColor,
                FontFamily = card.FontFamily,
                CardOrientation = card.CardOrientation,
                LogoPath = card.LogoPath
            };
        }

        private byte[] GenerateQRCode(VisitingCardViewModel card)
        {
            // Use enhanced VCard generation for better contact compatibility
            var vCardData = _qrCodeService.GenerateEnhancedVCardData(card);
            return _qrCodeService.GenerateQRCode(vCardData);
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        }
    }
}