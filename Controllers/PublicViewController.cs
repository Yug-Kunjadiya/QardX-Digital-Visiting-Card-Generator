using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardX.Data;
using QardX.Models;
using QardX.Models.ViewModels;
using QardX.Services;

namespace QardX.Controllers
{
    public class PublicViewController : Controller
    {
        private readonly QardXDbContext _context;
        private readonly IAnalyticsService _analyticsService;

        public PublicViewController(QardXDbContext context, IAnalyticsService analyticsService)
        {
            _context = context;
            _analyticsService = analyticsService;
        }

        [HttpGet("card/{id:int}")]
        public async Task<IActionResult> ViewCard(int id)
        {
            var card = await _context.VisitingCards
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CardId == id);

            if (card == null)
            {
                return NotFound("Card not found");
            }

            // Track the view for analytics
            await _analyticsService.TrackCardViewAsync(id, Request);

            var viewModel = new PublicCardViewModel
            {
                Id = card.CardId,
                FullName = card.User.FullName,
                JobTitle = card.JobTitle ?? "",
                Company = card.Company ?? "",
                Email = card.User.Email,
                Phone = card.Phone ?? "",
                Website = card.Website ?? "",
                Address = card.Address ?? "",
                TemplateId = card.TemplateId,
                CreatedAt = card.CreatedAt
            };

            return View(viewModel);
        }

        [HttpGet("vcard/{id:int}")]
        public async Task<IActionResult> DownloadVCard(int id)
        {
            var card = await _context.VisitingCards
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CardId == id);

            if (card == null)
            {
                return NotFound();
            }

            var vCardContent = GenerateVCardContent(card);
            var fileName = $"{card.User.FullName.Replace(" ", "_")}.vcf";

            return File(System.Text.Encoding.UTF8.GetBytes(vCardContent), 
                       "text/vcard", fileName);
        }

        private string GenerateVCardContent(VisitingCard card)
        {
            var vCard = "BEGIN:VCARD\n";
            vCard += "VERSION:3.0\n";
            vCard += $"FN:{card.User.FullName}\n";
            vCard += $"ORG:{card.Company}\n";
            vCard += $"EMAIL:{card.User.Email}\n";
            vCard += $"TEL:{card.Phone}\n";
                
            if (!string.IsNullOrEmpty(card.Address))
                vCard += $"ADR:;;{card.Address};;;;\n";
                
            vCard += "END:VCARD\n";

            return vCard;
        }
    }
}