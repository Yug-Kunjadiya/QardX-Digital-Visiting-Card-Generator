using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QardX.Models;
using SystemDrawing = System.Drawing;
using SystemDrawingImaging = System.Drawing.Imaging;

namespace QardX.Services
{
    public interface IExportService
    {
        byte[] ExportToPDF(VisitingCardViewModel card, byte[] qrCodeImage);
        byte[] ExportToPNG(VisitingCardViewModel card, byte[] qrCodeImage);
        Task<byte[]> ExportToPDFAsync(int cardId);
        Task<byte[]> ExportToPNGAsync(int cardId);
        Task<byte[]> ExportWithOptionsAsync(int cardId, ExportOptions options);
        Task<string> ExportToSVGAsync(int cardId, ExportOptions options);
        Task<byte[]> ExportToJPEGAsync(int cardId, ExportOptions options);
    }

    public class ExportService : IExportService
    {
        public ExportService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] ExportToPDF(VisitingCardViewModel card, byte[] qrCodeImage)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(50);

                    page.Content().Column(column =>
                    {
                        // Header
                        column.Item().AlignCenter().Text($"Digital Visiting Card - {card.TemplateName ?? "Professional"}")
                            .FontSize(16).Bold().FontFamily("Arial");

                        column.Item().PaddingVertical(20);

                        // Card Content
                        column.Item().Row(row =>
                        {
                            // Left side - Contact Info
                            row.RelativeItem(3).Column(leftColumn =>
                            {
                                // Name (FirstName + LastName or FullName)
                                var displayName = !string.IsNullOrEmpty(card.FirstName) && !string.IsNullOrEmpty(card.LastName) 
                                    ? $"{card.FirstName} {card.LastName}" 
                                    : card.FullName ?? "";
                                leftColumn.Item().Text(displayName)
                                    .FontSize(24).Bold()
                                    .FontFamily("Arial")
                                    .FontColor(GetTemplateColor(card.TemplateName));

                                // Job Title
                                if (!string.IsNullOrEmpty(card.JobTitle))
                                {
                                    leftColumn.Item().Text(card.JobTitle)
                                        .FontSize(14).Italic()
                                        .FontFamily("Arial");
                                }

                                // Company
                                if (!string.IsNullOrEmpty(card.Company))
                                {
                                    leftColumn.Item().Text(card.Company)
                                        .FontSize(16).Italic()
                                        .FontFamily("Arial");
                                }

                                leftColumn.Item().PaddingVertical(10);

                                // Contact Information
                                leftColumn.Item().Text($"✉ Email: {card.Email ?? ""}")
                                    .FontSize(12);

                                if (!string.IsNullOrEmpty(card.Phone))
                                {
                                    leftColumn.Item().Text($"📞 Phone: {card.Phone}")
                                        .FontSize(12);
                                }

                                if (!string.IsNullOrEmpty(card.LinkedIn))
                                {
                                    leftColumn.Item().Text($"🔗 LinkedIn: {card.LinkedIn}")
                                        .FontSize(12);
                                }

                                if (!string.IsNullOrEmpty(card.Instagram))
                                {
                                    leftColumn.Item().Text($"📷 Instagram: {card.Instagram}")
                                        .FontSize(12);
                                }

                                if (!string.IsNullOrEmpty(card.Twitter))
                                {
                                    leftColumn.Item().Text($"🐦 Twitter: {card.Twitter}")
                                        .FontSize(12);
                                }

                                if (!string.IsNullOrEmpty(card.Facebook))
                                {
                                    leftColumn.Item().Text($"📘 Facebook: {card.Facebook}")
                                        .FontSize(12);
                                }

                                if (!string.IsNullOrEmpty(card.Website))
                                {
                                    leftColumn.Item().Text($"🌐 Website: {card.Website}")
                                        .FontSize(12);
                                }

                                if (!string.IsNullOrEmpty(card.Address))
                                {
                                    leftColumn.Item().Text($"📍 Address: {card.Address}")
                                        .FontSize(12);
                                }

                                // Professional Information
                                if (!string.IsNullOrEmpty(card.Skills))
                                {
                                    leftColumn.Item().PaddingTop(5).Text($"🛠 Skills: {card.Skills}")
                                        .FontSize(10);
                                }

                                if (!string.IsNullOrEmpty(card.Languages))
                                {
                                    leftColumn.Item().Text($"🗣 Languages: {card.Languages}")
                                        .FontSize(10);
                                }

                                if (!string.IsNullOrEmpty(card.AvailabilityStatus))
                                {
                                    leftColumn.Item().Text($"✅ Status: {card.AvailabilityStatus}")
                                        .FontSize(10);
                                }
                            });

                            // Right side - QR Code
                            row.RelativeItem(1).AlignCenter().AlignMiddle().Column(rightColumn =>
                            {
                                rightColumn.Item().Image(qrCodeImage).FitArea();
                                rightColumn.Item().PaddingTop(10).AlignCenter().Text("Scan to save contact")
                                    .FontSize(10);
                            });
                        });

                        // Footer
                        column.Item().PaddingTop(30).AlignCenter().Text("Generated by QardX - Digital Visiting Card Generator")
                            .FontSize(8);
                    });
                });
            }).GeneratePdf();
        }

        public byte[] ExportToPNG(VisitingCardViewModel card, byte[] qrCodeImage)
        {
            int width = 800;
            int height = 500;

            using var bitmap = new SystemDrawing.Bitmap(width, height);
            using var graphics = SystemDrawing.Graphics.FromImage(bitmap);
            graphics.Clear(GetBackgroundColor(card.TemplateName));

            // Set high quality rendering
            graphics.TextRenderingHint = SystemDrawing.Text.TextRenderingHint.AntiAlias;
            graphics.SmoothingMode = SystemDrawing.Drawing2D.SmoothingMode.AntiAlias;

            // Define fonts
            var nameFont = new SystemDrawing.Font("Arial", 28, SystemDrawing.FontStyle.Bold);
            var companyFont = new SystemDrawing.Font("Arial", 18, SystemDrawing.FontStyle.Italic);
            var contactFont = new SystemDrawing.Font("Arial", 12);

            // Define colors based on template
            var textColor = GetTextColor(card.TemplateName);
            var accentColor = GetAccentColor(card.TemplateName);

            // Draw content
            int x = 50;
            int y = 50;

            // Name (FirstName + LastName or FullName)
            var displayName = !string.IsNullOrEmpty(card.FirstName) && !string.IsNullOrEmpty(card.LastName) 
                ? $"{card.FirstName} {card.LastName}" 
                : card.FullName ?? "";
            graphics.DrawString(displayName, nameFont, new SystemDrawing.SolidBrush(textColor), x, y);
            y += 40;

            // Job Title
            if (!string.IsNullOrEmpty(card.JobTitle))
            {
                graphics.DrawString(card.JobTitle, companyFont, new SystemDrawing.SolidBrush(accentColor), x, y);
                y += 30;
            }

            // Company
            if (!string.IsNullOrEmpty(card.Company))
            {
                graphics.DrawString(card.Company, companyFont, new SystemDrawing.SolidBrush(accentColor), x, y);
                y += 30;
            }

            y += 20; // Extra spacing

            // Contact info
            graphics.DrawString($"✉ Email: {card.Email ?? ""}", contactFont, new SystemDrawing.SolidBrush(textColor), x, y);
            y += 25;

            if (!string.IsNullOrEmpty(card.Phone))
            {
                graphics.DrawString($"📞 Phone: {card.Phone}", contactFont, new SystemDrawing.SolidBrush(textColor), x, y);
                y += 25;
            }

            if (!string.IsNullOrEmpty(card.LinkedIn))
            {
                graphics.DrawString($"🔗 LinkedIn: {card.LinkedIn}", contactFont, new SystemDrawing.SolidBrush(textColor), x, y);
                y += 25;
            }

            if (!string.IsNullOrEmpty(card.Instagram))
            {
                graphics.DrawString($"📷 Instagram: {card.Instagram}", contactFont, new SystemDrawing.SolidBrush(textColor), x, y);
                y += 25;
            }

            if (!string.IsNullOrEmpty(card.Twitter))
            {
                graphics.DrawString($"🐦 Twitter: {card.Twitter}", contactFont, new SystemDrawing.SolidBrush(textColor), x, y);
                y += 25;
            }

            if (!string.IsNullOrEmpty(card.Facebook))
            {
                graphics.DrawString($"📘 Facebook: {card.Facebook}", contactFont, new SystemDrawing.SolidBrush(textColor), x, y);
                y += 25;
            }

            if (!string.IsNullOrEmpty(card.Website))
            {
                graphics.DrawString($"🌐 Website: {card.Website}", contactFont, new SystemDrawing.SolidBrush(textColor), x, y);
                y += 25;
            }

            if (!string.IsNullOrEmpty(card.Address))
            {
                graphics.DrawString($"📍 Address: {card.Address}", contactFont, new SystemDrawing.SolidBrush(textColor), x, y);
                y += 25;
            }

            // Professional Information (smaller font)
            var smallFont = new SystemDrawing.Font("Arial", 10);
            y += 10; // Extra spacing

            if (!string.IsNullOrEmpty(card.Skills))
            {
                graphics.DrawString($"🛠 Skills: {card.Skills}", smallFont, new SystemDrawing.SolidBrush(textColor), x, y);
                y += 20;
            }

            if (!string.IsNullOrEmpty(card.Languages))
            {
                graphics.DrawString($"🗣 Languages: {card.Languages}", smallFont, new SystemDrawing.SolidBrush(textColor), x, y);
                y += 20;
            }

            if (!string.IsNullOrEmpty(card.AvailabilityStatus))
            {
                graphics.DrawString($"✅ Status: {card.AvailabilityStatus}", smallFont, new SystemDrawing.SolidBrush(textColor), x, y);
            }

            // QR Code
            if (qrCodeImage != null && qrCodeImage.Length > 0)
            {
                using var qrStream = new MemoryStream(qrCodeImage);
                using var qrImg = SystemDrawing.Image.FromStream(qrStream);
                graphics.DrawImage(qrImg, width - 150, 50, 100, 100);
            }

            // Save to byte array
            using var stream = new MemoryStream();
            bitmap.Save(stream, SystemDrawingImaging.ImageFormat.Png);
            return stream.ToArray();
        }

        private string GetTemplateColor(string? templateName)
        {
            return templateName switch
            {
                "Classic Blue" => Colors.Blue.Medium,
                "Minimal White" => Colors.Black,
                "Modern Dark" => Colors.Yellow.Medium,
                _ => Colors.Black
            };
        }

        private SystemDrawing.Color GetBackgroundColor(string? templateName)
        {
            return templateName switch
            {
                "Classic Blue" => SystemDrawing.Color.FromArgb(52, 84, 150),
                "Minimal White" => SystemDrawing.Color.White,
                "Modern Dark" => SystemDrawing.Color.FromArgb(33, 37, 41),
                _ => SystemDrawing.Color.White
            };
        }

        private SystemDrawing.Color GetTextColor(string? templateName)
        {
            return templateName switch
            {
                "Classic Blue" => SystemDrawing.Color.White,
                "Minimal White" => SystemDrawing.Color.Black,
                "Modern Dark" => SystemDrawing.Color.White,
                _ => SystemDrawing.Color.Black
            };
        }

        private SystemDrawing.Color GetAccentColor(string? templateName)
        {
            return templateName switch
            {
                "Classic Blue" => SystemDrawing.Color.LightBlue,
                "Minimal White" => SystemDrawing.Color.Gray,
                "Modern Dark" => SystemDrawing.Color.Gold,
                _ => SystemDrawing.Color.Gray
            };
        }

        // New advanced export methods
        public async Task<byte[]> ExportToPDFAsync(int cardId)
        {
            var card = await GetCardWithDetailsAsync(cardId);
            if (card == null) throw new ArgumentException("Card not found");

            var qrCode = GenerateQRCodeForCard(cardId);
            return ExportToPDF(card, qrCode);
        }

        public async Task<byte[]> ExportToPNGAsync(int cardId)
        {
            var card = await GetCardWithDetailsAsync(cardId);
            if (card == null) throw new ArgumentException("Card not found");

            var qrCode = GenerateQRCodeForCard(cardId);
            return ExportToPNG(card, qrCode);
        }

        public async Task<byte[]> ExportWithOptionsAsync(int cardId, ExportOptions options)
        {
            return options.Format.ToUpper() switch
            {
                "PDF" => await ExportToPDFAsync(cardId),
                "PNG" => await ExportToPNGAsync(cardId),
                "JPEG" => await ExportToJPEGAsync(cardId, options),
                "SVG" => System.Text.Encoding.UTF8.GetBytes(await ExportToSVGAsync(cardId, options)),
                _ => throw new ArgumentException($"Unsupported format: {options.Format}")
            };
        }

        public async Task<string> ExportToSVGAsync(int cardId, ExportOptions options)
        {
            var card = await GetCardWithDetailsAsync(cardId);
            if (card == null) throw new ArgumentException("Card not found");

            var svg = $@"
                <svg width='{options.Width}' height='{options.Height}' xmlns='http://www.w3.org/2000/svg'>
                    <rect width='100%' height='100%' fill='{options.BackgroundColor}'/>
                    <g transform='translate(50, 50)'>
                        <text x='0' y='40' font-family='Arial, sans-serif' font-size='32' font-weight='bold' fill='#333'>
                            {card.FullName}
                        </text>
                        <text x='0' y='80' font-family='Arial, sans-serif' font-size='18' fill='#666'>
                            {card.JobTitle}
                        </text>
                        <text x='0' y='110' font-family='Arial, sans-serif' font-size='16' fill='#666'>
                            {card.Company}
                        </text>
                        <text x='0' y='150' font-family='Arial, sans-serif' font-size='14' fill='#555'>
                            📧 {card.Email}
                        </text>
                        <text x='0' y='180' font-family='Arial, sans-serif' font-size='14' fill='#555'>
                            📞 {card.Phone}
                        </text>
                        {(options.IncludeQRCode 
                            ? $"<rect x='{options.Width - 200}' y='20' width='150' height='150' fill='#f0f0f0' stroke='#ddd'/><text x='{options.Width - 125}' y='105' text-anchor='middle' font-size='12' fill='#999'>QR Code</text>" 
                            : "")}
                    </g>
                </svg>";

            return svg;
        }

        public async Task<byte[]> ExportToJPEGAsync(int cardId, ExportOptions options)
        {
            var card = await GetCardWithDetailsAsync(cardId);
            if (card == null) throw new ArgumentException("Card not found");

            using var bitmap = new SystemDrawing.Bitmap(options.Width, options.Height);
            using var graphics = SystemDrawing.Graphics.FromImage(bitmap);
            
            // Set high quality rendering
            graphics.TextRenderingHint = SystemDrawing.Text.TextRenderingHint.AntiAlias;
            graphics.SmoothingMode = SystemDrawing.Drawing2D.SmoothingMode.AntiAlias;
            
            // Background
            var bgColor = SystemDrawing.ColorTranslator.FromHtml(options.BackgroundColor);
            graphics.Clear(bgColor);
            
            // Card content
            var font = new SystemDrawing.Font("Arial", 24, SystemDrawing.FontStyle.Bold);
            var brush = new SystemDrawing.SolidBrush(SystemDrawing.Color.Black);
            
            graphics.DrawString(card.FullName, font, brush, 50, 50);
            
            using var stream = new MemoryStream();
            var encoder = SystemDrawingImaging.ImageCodecInfo.GetImageEncoders()
                .First(x => x.FormatID == SystemDrawingImaging.ImageFormat.Jpeg.Guid);
            
            var encoderParams = new SystemDrawingImaging.EncoderParameters(1);
            encoderParams.Param[0] = new SystemDrawingImaging.EncoderParameter(
                SystemDrawingImaging.Encoder.Quality, (long)options.Quality);
            
            bitmap.Save(stream, encoder, encoderParams);
            return stream.ToArray();
        }

        private async Task<VisitingCardViewModel?> GetCardWithDetailsAsync(int cardId)
        {
            // This would need to be injected via constructor or passed as parameter
            // For now, returning a placeholder - in real implementation, inject DbContext
            return new VisitingCardViewModel
            {
                CardId = cardId,
                FullName = "Sample User",
                Email = "sample@example.com",
                JobTitle = "Software Developer",
                Company = "Tech Company",
                Phone = "+1234567890"
            };
        }

        private byte[] GenerateQRCodeForCard(int cardId)
        {
            // This would use the QRCodeService
            // Placeholder implementation
            return new byte[0];
        }
    }
}