using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardX.Models
{
    public class CardView
    {
        [Key]
        public int ViewId { get; set; }

        [ForeignKey("VisitingCard")]
        public int CardId { get; set; }

        [StringLength(45)]
        public string? ViewerIP { get; set; }

        [StringLength(500)]
        public string? UserAgent { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Region { get; set; }

        [StringLength(50)]
        public string? DeviceType { get; set; }

        [StringLength(100)]
        public string? Browser { get; set; }

        [StringLength(100)]
        public string? OperatingSystem { get; set; }

        [StringLength(500)]
        public string? ReferrerUrl { get; set; }

        [StringLength(200)]
        public string? ReferrerDomain { get; set; }

        [StringLength(100)]
        public string? SessionId { get; set; }

        public bool IsUniqueView { get; set; } = true;

        public int? ViewDuration { get; set; }

        public DateTime ViewedAt { get; set; } = DateTime.Now;

        // Navigation property
        public virtual VisitingCard VisitingCard { get; set; } = null!;
    }
}