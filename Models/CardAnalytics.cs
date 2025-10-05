using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QardX.Models
{
    public class CardAnalytics
    {
        [Key]
        public int AnalyticsId { get; set; }

        [ForeignKey("VisitingCard")]
        public int CardId { get; set; }

        public DateTime ViewedAt { get; set; } = DateTime.Now;

        [StringLength(45)]
        public string? IPAddress { get; set; }

        [StringLength(200)]
        public string? UserAgent { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? DeviceType { get; set; } // Mobile, Desktop, Tablet

        [StringLength(50)]
        public string? Browser { get; set; }

        // Navigation property
        public virtual VisitingCard VisitingCard { get; set; } = null!;
    }

    public class AnalyticsDashboardViewModel
    {
        public int TotalCards { get; set; }
        public int TotalViews { get; set; }
        public int UniqueViewers { get; set; }
        public int ViewsToday { get; set; }
        public int ViewsThisWeek { get; set; }
        public int ViewsThisMonth { get; set; }
        public List<CardAnalyticsViewModel> CardAnalytics { get; set; } = new();
        public List<ViewsByDate> ViewsTrend { get; set; } = new();
        public List<CountryBreakdown> TopCountries { get; set; } = new();
        public List<DeviceBreakdown> TopDevices { get; set; } = new();
        public List<BrowserBreakdown> TopBrowsers { get; set; } = new();

        // For card details view
        public string CardName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastViewed { get; set; }
        public List<CardView> RecentViews { get; set; } = new();

        // Aliases for view compatibility
        public List<CountryBreakdown> GeographicDistribution => TopCountries;
        public List<DeviceBreakdown> DeviceDistribution => TopDevices;
        public List<BrowserBreakdown> BrowserDistribution => TopBrowsers;
    }

    public class CardAnalyticsViewModel
    {
        public int CardId { get; set; }
        public string CardName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public DateTime? LastViewed { get; set; }
    }

    public class CountryBreakdown
    {
        public string Country { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class DeviceBreakdown
    {
        public string DeviceType { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class BrowserBreakdown
    {
        public string Browser { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class ViewsByDate
    {
        public DateTime Date { get; set; }
        public int Views { get; set; }
    }

    public class ViewsByCountry
    {
        public string Country { get; set; } = string.Empty;
        public int Views { get; set; }
        public double Percentage { get; set; }
    }

    public class ViewsByDevice
    {
        public string DeviceType { get; set; } = string.Empty;
        public int Views { get; set; }
        public double Percentage { get; set; }
    }

    public class ViewsByBrowser
    {
        public string Browser { get; set; } = string.Empty;
        public int Views { get; set; }
        public double Percentage { get; set; }
    }
}