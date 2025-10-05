namespace QardX.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalCards { get; set; }
        public int TotalTemplates { get; set; }
        public List<VisitingCard> RecentCards { get; set; } = new();
    }
}