namespace QardX.Models
{
    public class QRBatchOptions
    {
        public int Size { get; set; } = 300;
        public string Format { get; set; } = "PNG";
        public bool IncludeLogo { get; set; } = false;
        public string BackgroundColor { get; set; } = "#FFFFFF";
        public string ForegroundColor { get; set; } = "#000000";
        public string ExportFormat { get; set; } = "ZIP"; // ZIP, PDF
    }

    public class QRCodeOptions
    {
        public int Size { get; set; } = 300;
        public string Format { get; set; } = "PNG";
        public bool IncludeLogo { get; set; } = false;
        public string BackgroundColor { get; set; } = "#FFFFFF";
        public string ForegroundColor { get; set; } = "#000000";
        public int Quality { get; set; } = 100;
    }

    public class BatchQRResult
    {
        public int CardId { get; set; }
        public string CardName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public byte[] QRCodeData { get; set; } = Array.Empty<byte>();
        public string Format { get; set; } = "PNG";
        public int Size { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class BatchQRViewModel
    {
        public List<VisitingCard> AvailableCards { get; set; } = new();
        public List<int> SelectedCardIds { get; set; } = new();
        public QRBatchOptions Options { get; set; } = new();
    }

    public class EmailServiceOptions
    {
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 587;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; } = true;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = "QardX";
    }

    public class EmailShareRequest
    {
        public int CardId { get; set; }
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IncludeQRCode { get; set; } = true;
        public bool IncludePDF { get; set; } = false;
    }

    public class ContactForm
    {
        public int ContactFormId { get; set; }
        public int CardId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        
        // Navigation property
        public VisitingCard Card { get; set; } = null!;
    }

    public class UserRole
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public string RoleName { get; set; } = string.Empty; // Admin, User, Premium
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        public DateTime? ExpiresAt { get; set; }
        
        // Navigation property
        public User User { get; set; } = null!;
    }

    public class ExportOptions
    {
        public string Format { get; set; } = "PDF"; // PDF, PNG, SVG, JPEG
        public int Quality { get; set; } = 100;
        public int Width { get; set; } = 1200;
        public int Height { get; set; } = 800;
        public bool IncludeQRCode { get; set; } = true;
        public bool IncludeLogo { get; set; } = true;
        public string BackgroundColor { get; set; } = "#FFFFFF";
    }

    public class CustomTemplate
    {
        public int CustomTemplateId { get; set; }
        public int UserId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string HtmlContent { get; set; } = string.Empty;
        public string? CssContent { get; set; }
        public string? PreviewImagePath { get; set; }
        public bool IsPublic { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation property
        public User User { get; set; } = null!;
    }

    public class EmailLog
    {
        public int EmailLogId { get; set; }
        public int CardId { get; set; }
        public string RecipientEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string EmailType { get; set; } = string.Empty; // 'CardShare', 'ContactNotification', 'BulkEmail'
        public DateTime SentAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Sent"; // 'Sent', 'Failed', 'Pending'
        public string? ErrorMessage { get; set; }
        
        // Navigation property
        public VisitingCard Card { get; set; } = null!;
    }

    public class BatchOperation
    {
        public int BatchOperationId { get; set; }
        public int UserId { get; set; }
        public string OperationType { get; set; } = string.Empty; // 'QRGeneration', 'BulkEmail', 'Export'
        public string CardIds { get; set; } = string.Empty; // JSON array of card IDs
        public string? Parameters { get; set; } // JSON parameters for the operation
        public string Status { get; set; } = "Processing"; // 'Processing', 'Completed', 'Failed'
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
        public string? ResultPath { get; set; } // Path to result file (e.g., ZIP file)
        
        // Navigation property
        public User User { get; set; } = null!;
    }
}