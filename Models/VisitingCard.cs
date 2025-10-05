using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardX.Models
{
    public class VisitingCard
    {
        [Key]
        public int CardId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();

        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? LinkedIn { get; set; }

        [StringLength(100)]
        public string? Company { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        // Phase 1 Features: Social Media Links
        [StringLength(200)]
        public string? Instagram { get; set; }

        [StringLength(200)]
        public string? Twitter { get; set; }

        [StringLength(200)]
        public string? Facebook { get; set; }

        [StringLength(200)]
        public string? Website { get; set; }

        // Logo/Photo Upload
        [StringLength(500)]
        public string? LogoPath { get; set; }

        // Professional Information
        [StringLength(100)]
        public string? JobTitle { get; set; }

        [StringLength(500)]
        public string? Skills { get; set; }

        [StringLength(200)]
        public string? Languages { get; set; }

        [StringLength(50)]
        public string? AvailabilityStatus { get; set; } // Available, Busy, Vacation

        // Template Customization
        [StringLength(20)]
        public string? PrimaryColor { get; set; }

        [StringLength(20)]
        public string? SecondaryColor { get; set; }

        [StringLength(50)]
        public string? FontFamily { get; set; }

        [StringLength(20)]
        public string? CardOrientation { get; set; } // Horizontal, Vertical, Square

        // Analytics
        public int ViewCount { get; set; } = 0;
        public DateTime? LastViewed { get; set; }

        [ForeignKey("Template")]
        public int TemplateId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Template Template { get; set; } = null!;
    }
}