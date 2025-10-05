using System.ComponentModel.DataAnnotations;

namespace QardX.Models
{
    public class Template
    {
        [Key]
        public int TemplateId { get; set; }

        [StringLength(100)]
        public string? TemplateName { get; set; }

        [StringLength(200)]
        public string? FilePath { get; set; }

        // Navigation property
        public virtual ICollection<VisitingCard> VisitingCards { get; set; } = new List<VisitingCard>();
    }
}