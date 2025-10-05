using Microsoft.EntityFrameworkCore;
using QardX.Data;
using QardX.Models;
using System.IO.Compression;

namespace QardX.Services
{
    public interface IBatchQRService
    {
        Task<List<BatchQRResult>> GenerateBatchQRCodesAsync(List<int> cardIds, QRBatchOptions options);
        Task<byte[]> ExportBatchQRCodesAsync(List<int> cardIds, QRBatchOptions options);
        Task<BatchQRResult> GenerateQRCodeWithOptionsAsync(int cardId, QRCodeOptions options);
    }

    public class BatchQRService : IBatchQRService
    {
        private readonly IQRCodeService _qrCodeService;
        private readonly QardXDbContext _context;
        private readonly ILogger<BatchQRService> _logger;

        public BatchQRService(IQRCodeService qrCodeService, QardXDbContext context, ILogger<BatchQRService> logger)
        {
            _qrCodeService = qrCodeService;
            _context = context;
            _logger = logger;
        }

        public async Task<List<BatchQRResult>> GenerateBatchQRCodesAsync(List<int> cardIds, QRBatchOptions options)
        {
            var results = new List<BatchQRResult>();

            foreach (var cardId in cardIds)
            {
                try
                {
                    var card = await _context.VisitingCards
                        .Include(c => c.User)
                        .FirstOrDefaultAsync(c => c.CardId == cardId);

                    if (card == null)
                    {
                        results.Add(new BatchQRResult
                        {
                            CardId = cardId,
                            Success = false,
                            ErrorMessage = "Card not found"
                        });
                        continue;
                    }

                    var qrOptions = new QRCodeOptions
                    {
                        Size = options.Size,
                        Format = options.Format,
                        IncludeLogo = options.IncludeLogo,
                        BackgroundColor = options.BackgroundColor,
                        ForegroundColor = options.ForegroundColor
                    };

                    var qrResult = await GenerateQRCodeWithOptionsAsync(cardId, qrOptions);
                    results.Add(qrResult);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating QR code for card {CardId}", cardId);
                    results.Add(new BatchQRResult
                    {
                        CardId = cardId,
                        Success = false,
                        ErrorMessage = ex.Message
                    });
                }
            }

            return results;
        }

        public async Task<BatchQRResult> GenerateQRCodeWithOptionsAsync(int cardId, QRCodeOptions options)
        {
            var card = await _context.VisitingCards
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CardId == cardId);

            if (card == null)
            {
                return new BatchQRResult
                {
                    CardId = cardId,
                    Success = false,
                    ErrorMessage = "Card not found"
                };
            }

            try
            {
                var publicUrl = $"https://localhost:5000/PublicView/View/{cardId}";
                byte[] qrCodeData;

                switch (options.Format.ToLower())
                {
                    case "png":
                        qrCodeData = _qrCodeService.GenerateQRCode(publicUrl);
                        break;
                    case "svg":
                        qrCodeData = _qrCodeService.GenerateQRCode(publicUrl);
                        break;
                    default:
                        qrCodeData = _qrCodeService.GenerateQRCode(publicUrl);
                        break;
                }

                return new BatchQRResult
                {
                    CardId = cardId,
                    CardName = card.User.FullName,
                    Company = card.Company ?? "",
                    Success = true,
                    QRCodeData = qrCodeData,
                    Format = options.Format,
                    Size = options.Size,
                    GeneratedAt = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating QR code for card {CardId}", cardId);
                return new BatchQRResult
                {
                    CardId = cardId,
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<byte[]> ExportBatchQRCodesAsync(List<int> cardIds, QRBatchOptions options)
        {
            var results = await GenerateBatchQRCodesAsync(cardIds, options);
            
            // Create a ZIP file containing all QR codes
            using var memoryStream = new MemoryStream();
            using (var archive = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
            {
                foreach (var result in results.Where(r => r.Success))
                {
                    var fileName = $"{result.CardName.Replace(" ", "_")}_{result.CardId}.{result.Format.ToLower()}";
                    var entry = archive.CreateEntry(fileName);
                    
                    using var entryStream = entry.Open();
                    await entryStream.WriteAsync(result.QRCodeData, 0, result.QRCodeData.Length);
                }

                // Add a summary file
                var summaryEntry = archive.CreateEntry("batch_summary.txt");
                using var summaryStream = summaryEntry.Open();
                using var writer = new StreamWriter(summaryStream);
                
                await writer.WriteLineAsync($"QR Code Batch Export Summary");
                await writer.WriteLineAsync($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                await writer.WriteLineAsync($"Total Cards: {cardIds.Count}");
                await writer.WriteLineAsync($"Successful: {results.Count(r => r.Success)}");
                await writer.WriteLineAsync($"Failed: {results.Count(r => !r.Success)}");
                await writer.WriteLineAsync();
                
                foreach (var result in results)
                {
                    var status = result.Success ? "✓" : "✗";
                    await writer.WriteLineAsync($"{status} {result.CardName} ({result.CardId}) - {(result.Success ? "Success" : result.ErrorMessage)}");
                }
            }

            return memoryStream.ToArray();
        }
    }
}