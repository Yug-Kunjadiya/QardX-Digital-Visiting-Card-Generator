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
    public class BatchController : Controller
    {
        private readonly QardXDbContext _context;
        private readonly IBatchQRService _batchQRService;

        public BatchController(QardXDbContext context, IBatchQRService batchQRService)
        {
            _context = context;
            _batchQRService = batchQRService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var cards = await _context.VisitingCards
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return View(cards);
        }

        public async Task<IActionResult> QRCodes()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var cards = await _context.VisitingCards
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            var viewModel = new BatchQRViewModel
            {
                AvailableCards = cards,
                Options = new QRBatchOptions()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateBatch(BatchQRViewModel model)
        {
            if (!model.SelectedCardIds.Any())
            {
                TempData["Error"] = "Please select at least one card.";
                return RedirectToAction(nameof(QRCodes));
            }

            try
            {
                var results = await _batchQRService.GenerateBatchQRCodesAsync(model.SelectedCardIds, model.Options);
                
                // Store results in TempData for display
                TempData["BatchResults"] = System.Text.Json.JsonSerializer.Serialize(results);
                TempData["Success"] = $"Generated {results.Count(r => r.Success)} QR codes successfully.";
                
                if (results.Any(r => !r.Success))
                {
                    TempData["Warning"] = $"{results.Count(r => !r.Success)} QR codes failed to generate.";
                }

                return RedirectToAction(nameof(BatchResults));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error generating batch QR codes: {ex.Message}";
                return RedirectToAction(nameof(QRCodes));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportBatch(List<int> cardIds, QRBatchOptions options)
        {
            if (!cardIds.Any())
            {
                return BadRequest("No cards selected");
            }

            try
            {
                var zipData = await _batchQRService.ExportBatchQRCodesAsync(cardIds, options);
                var fileName = $"QRCodes_Batch_{DateTime.Now:yyyyMMdd_HHmmss}.zip";
                
                return File(zipData, "application/zip", fileName);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error exporting batch: {ex.Message}";
                return RedirectToAction(nameof(QRCodes));
            }
        }

        public IActionResult BatchResults()
        {
            var resultsJson = TempData["BatchResults"] as string;
            if (string.IsNullOrEmpty(resultsJson))
            {
                return RedirectToAction(nameof(QRCodes));
            }

            var results = System.Text.Json.JsonSerializer.Deserialize<List<BatchQRResult>>(resultsJson);
            return View(results);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadQR(int cardId, string format = "PNG", int size = 300)
        {
            var options = new QRCodeOptions
            {
                Format = format,
                Size = size
            };

            var result = await _batchQRService.GenerateQRCodeWithOptionsAsync(cardId, options);
            
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            var mimeType = format.ToLower() switch
            {
                "svg" => "image/svg+xml",
                "png" => "image/png",
                "jpeg" => "image/jpeg",
                _ => "application/octet-stream"
            };

            var fileName = $"QRCode_{result.CardName.Replace(" ", "_")}_{cardId}.{format.ToLower()}";
            return File(result.QRCodeData, mimeType, fileName);
        }
    }
}