using QardX.Models;

namespace QardX.Models
{
    public class AdminAnalyticsViewModel
    {
        public int TotalViews { get; set; }
        public int TotalCards { get; set; }
        public int TotalUsers { get; set; }
        public List<CardView> RecentViews { get; set; } = new();
    }
}