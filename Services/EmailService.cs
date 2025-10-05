using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QardX.Data;
using QardX.Models;

namespace QardX.Services
{
    public interface IEmailService
    {
        Task<bool> SendCardByEmailAsync(EmailShareRequest request);
        Task<bool> SendBulkCardsAsync(List<EmailShareRequest> requests);
        Task<bool> SendContactFormNotificationAsync(ContactForm contactForm);
        Task<bool> TestEmailConfigurationAsync();
    }

    public class EmailService : IEmailService
    {
        private readonly EmailServiceOptions _emailOptions;
        private readonly IExportService _exportService;
        private readonly IQRCodeService _qrCodeService;
        private readonly QardXDbContext _context;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptions<EmailServiceOptions> emailOptions,
            IExportService exportService,
            IQRCodeService qrCodeService,
            QardXDbContext context,
            ILogger<EmailService> logger)
        {
            _emailOptions = emailOptions.Value;
            _exportService = exportService;
            _qrCodeService = qrCodeService;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> SendCardByEmailAsync(EmailShareRequest request)
        {
            try
            {
                var card = await _context.VisitingCards
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.CardId == request.CardId);

                if (card == null)
                {
                    _logger.LogError("Card not found: {CardId}", request.CardId);
                    return false;
                }

                using var smtpClient = CreateSmtpClient();
                using var message = new MailMessage();

                message.From = new MailAddress(_emailOptions.FromEmail, _emailOptions.FromName);
                message.To.Add(request.ToEmail);
                message.Subject = !string.IsNullOrEmpty(request.Subject) 
                    ? request.Subject 
                    : $"Digital Business Card - {card.User.FullName}";

                var htmlBody = await GenerateEmailHtmlAsync(card, request.Message);
                message.Body = htmlBody;
                message.IsBodyHtml = true;

                // Add QR Code attachment if requested
                if (request.IncludeQRCode)
                {
                    var publicUrl = $"https://localhost:5000/PublicView/View/{card.CardId}";
                    var qrCodeData = _qrCodeService.GenerateQRCode(publicUrl);
                    var qrAttachment = new Attachment(new MemoryStream(qrCodeData), $"QRCode_{card.User.FullName}.png", "image/png");
                    message.Attachments.Add(qrAttachment);
                }

                // Add PDF attachment if requested
                if (request.IncludePDF)
                {
                    var pdfData = await _exportService.ExportToPDFAsync(card.CardId);
                    var pdfAttachment = new Attachment(new MemoryStream(pdfData), $"BusinessCard_{card.User.FullName}.pdf", "application/pdf");
                    message.Attachments.Add(pdfAttachment);
                }

                await smtpClient.SendMailAsync(message);
                _logger.LogInformation("Email sent successfully to {Email} for card {CardId}", request.ToEmail, request.CardId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email} for card {CardId}", request.ToEmail, request.CardId);
                return false;
            }
        }

        public async Task<bool> SendBulkCardsAsync(List<EmailShareRequest> requests)
        {
            var results = new List<bool>();
            
            foreach (var request in requests)
            {
                var result = await SendCardByEmailAsync(request);
                results.Add(result);
                
                // Add delay between emails to avoid being flagged as spam
                await Task.Delay(1000);
            }

            var successCount = results.Count(r => r);
            _logger.LogInformation("Bulk email completed: {Success}/{Total} emails sent successfully", 
                successCount, requests.Count);

            return successCount > 0;
        }

        public async Task<bool> SendContactFormNotificationAsync(ContactForm contactForm)
        {
            try
            {
                var card = await _context.VisitingCards
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.CardId == contactForm.CardId);

                if (card == null) return false;

                using var smtpClient = CreateSmtpClient();
                using var message = new MailMessage();

                message.From = new MailAddress(_emailOptions.FromEmail, _emailOptions.FromName);
                message.To.Add(card.User.Email);
                message.Subject = $"New Contact Form Submission - {contactForm.Name}";

                var htmlBody = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <div style='background: linear-gradient(135deg, #3456a3, #5478c4); color: white; padding: 20px; text-align: center;'>
                            <h2>New Contact Form Submission</h2>
                        </div>
                        <div style='padding: 20px; background: #f8f9fa;'>
                            <h3>Contact Details:</h3>
                            <p><strong>Name:</strong> {contactForm.Name}</p>
                            <p><strong>Email:</strong> {contactForm.Email}</p>
                            <p><strong>Phone:</strong> {contactForm.Phone}</p>
                            <p><strong>Company:</strong> {contactForm.Company}</p>
                            <p><strong>Submitted:</strong> {contactForm.SubmittedAt:yyyy-MM-dd HH:mm:ss}</p>
                            
                            <h3>Message:</h3>
                            <div style='background: white; padding: 15px; border-left: 4px solid #3456a3; margin: 10px 0;'>
                                {contactForm.Message.Replace("\n", "<br>")}
                            </div>
                        </div>
                        <div style='background: #e9ecef; padding: 10px; text-align: center; font-size: 12px; color: #6c757d;'>
                            This notification was sent from your QardX digital business card.
                        </div>
                    </body>
                    </html>";

                message.Body = htmlBody;
                message.IsBodyHtml = true;

                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send contact form notification for card {CardId}", contactForm.CardId);
                return false;
            }
        }

        public async Task<bool> TestEmailConfigurationAsync()
        {
            try
            {
                using var smtpClient = CreateSmtpClient();
                using var message = new MailMessage();

                message.From = new MailAddress(_emailOptions.FromEmail, _emailOptions.FromName);
                message.To.Add(_emailOptions.FromEmail);
                message.Subject = "QardX Email Configuration Test";
                message.Body = "This is a test email to verify your QardX email configuration is working correctly.";

                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email configuration test failed");
                return false;
            }
        }

        private SmtpClient CreateSmtpClient()
        {
            var smtpClient = new SmtpClient(_emailOptions.SmtpHost, _emailOptions.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailOptions.Username, _emailOptions.Password),
                EnableSsl = _emailOptions.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            return smtpClient;
        }

        private async Task<string> GenerateEmailHtmlAsync(VisitingCard card, string? customMessage = null)
        {
            var publicUrl = $"https://localhost:5000/PublicView/View/{card.CardId}";
            
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background: linear-gradient(135deg, #3456a3, #5478c4); color: white; padding: 20px; text-align: center;'>
                        <h2>Digital Business Card</h2>
                        <p>You've received a digital business card!</p>
                    </div>
                    
                    <div style='padding: 20px; background: #f8f9fa;'>
                        <div style='background: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1);'>
                            <h3 style='color: #3456a3; margin-top: 0;'>{card.User.FullName}</h3>
                            {(!string.IsNullOrEmpty(card.JobTitle) ? $"<p style='color: #6c757d; margin: 5px 0;'>{card.JobTitle}</p>" : "")}
                            {(!string.IsNullOrEmpty(card.Company) ? $"<p style='color: #6c757d; margin: 5px 0;'>{card.Company}</p>" : "")}
                            
                            <div style='margin: 15px 0;'>
                                <p><strong>📧 Email:</strong> {card.User.Email}</p>
                                {(!string.IsNullOrEmpty(card.Phone) ? $"<p><strong>📞 Phone:</strong> {card.Phone}</p>" : "")}
                                {(!string.IsNullOrEmpty(card.Website) ? $"<p><strong>🌐 Website:</strong> <a href='{card.Website}'>{card.Website}</a></p>" : "")}
                            </div>
                        </div>
                        
                        {(!string.IsNullOrEmpty(customMessage) ? $@"
                        <div style='background: white; padding: 15px; margin: 15px 0; border-left: 4px solid #3456a3;'>
                            <h4>Personal Message:</h4>
                            <p>{customMessage.Replace("\n", "<br>")}</p>
                        </div>" : "")}
                        
                        <div style='text-align: center; margin: 20px 0;'>
                            <a href='{publicUrl}' style='background: linear-gradient(135deg, #3456a3, #5478c4); color: white; padding: 12px 25px; text-decoration: none; border-radius: 6px; font-weight: bold;'>
                                View Full Card
                            </a>
                        </div>
                    </div>
                    
                    <div style='background: #e9ecef; padding: 10px; text-align: center; font-size: 12px; color: #6c757d;'>
                        Powered by QardX - Digital Business Cards
                    </div>
                </body>
                </html>";
        }
    }
}