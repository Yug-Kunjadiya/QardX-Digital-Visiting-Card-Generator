using System.ComponentModel.DataAnnotations;

namespace QardX.Models
{
    public class VisitingCardViewModel
    {
        public int CardId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "LinkedIn Profile")]
        public string? LinkedIn { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Company")]
        public string Company { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Template")]
        public int TemplateId { get; set; }

        // Phase 1 Features: Social Media Links
        [StringLength(200)]
        [Display(Name = "Instagram Profile")]
        [Url(ErrorMessage = "Please enter a valid Instagram URL")]
        public string? Instagram { get; set; }

        [StringLength(200)]
        [Display(Name = "Twitter Profile")]
        [Url(ErrorMessage = "Please enter a valid Twitter URL")]
        public string? Twitter { get; set; }

        [StringLength(200)]
        [Display(Name = "Facebook Profile")]
        [Url(ErrorMessage = "Please enter a valid Facebook URL")]
        public string? Facebook { get; set; }

        [StringLength(200)]
        [Display(Name = "Website")]
        [Url(ErrorMessage = "Please enter a valid Website URL")]
        public string? Website { get; set; }

        // Professional Information
        [StringLength(100)]
        [Display(Name = "Job Title")]
        public string? JobTitle { get; set; }

        [StringLength(500)]
        [Display(Name = "Skills (comma-separated)")]
        public string? Skills { get; set; }

        [StringLength(200)]
        [Display(Name = "Languages Spoken")]
        public string? Languages { get; set; }

        [Display(Name = "Availability Status")]
        public string? AvailabilityStatus { get; set; }

        // Template Customization
        [Display(Name = "Primary Color")]
        public string? PrimaryColor { get; set; }

        [Display(Name = "Secondary Color")]
        public string? SecondaryColor { get; set; }

        [Display(Name = "Font Family")]
        public string? FontFamily { get; set; }

        [Display(Name = "Card Orientation")]
        public string? CardOrientation { get; set; }

        // Logo Upload
        [Display(Name = "Company Logo / Profile Photo")]
        public IFormFile? LogoFile { get; set; }

        public string? LogoPath { get; set; }

        // Analytics
        public int ViewCount { get; set; }
        public DateTime? LastViewed { get; set; }

        // For display purposes
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? TemplateName { get; set; }
        public byte[]? QRCodeImage { get; set; }
    }
}
