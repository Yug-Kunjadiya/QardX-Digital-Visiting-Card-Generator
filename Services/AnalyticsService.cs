using Microsoft.EntityFrameworkCore;
using QardX.Data;
using QardX.Models;
using System.Net;

namespace QardX.Services
{
    public interface IAnalyticsService
    {
        Task TrackCardViewAsync(int cardId, HttpRequest request);
        Task<AnalyticsDashboardViewModel> GetCardAnalyticsAsync(int cardId);
        Task<AnalyticsDashboardViewModel> GetUserAnalyticsAsync(int userId);
    }

    public class AnalyticsService : IAnalyticsService
    {
        private readonly QardXDbContext _context;
        private readonly ILogger<AnalyticsService> _logger;

        public AnalyticsService(QardXDbContext context, ILogger<AnalyticsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task TrackCardViewAsync(int cardId, HttpRequest request)
        {
            try
            {
                var cardView = new CardView
                {
                    CardId = cardId,
                    ViewedAt = DateTime.Now,
                    ViewerIP = GetClientIPAddress(request),
                    UserAgent = request.Headers["User-Agent"].ToString(),
                    DeviceType = GetDeviceType(request.Headers["User-Agent"].ToString()),
                    Browser = GetBrowser(request.Headers["User-Agent"].ToString()),
                    IsUniqueView = true // You can implement unique view logic here
                };

                _context.CardViews.Add(cardView);

                // Update card view count
                var card = await _context.VisitingCards.FindAsync(cardId);
                if (card != null)
                {
                    card.ViewCount++;
                    card.LastViewed = DateTime.Now;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking card view for card {CardId}", cardId);
            }
        }

        public async Task<AnalyticsDashboardViewModel> GetCardAnalyticsAsync(int cardId)
        {
            var cardViews = await _context.CardViews
                .Where(cv => cv.CardId == cardId)
                .ToListAsync();

            var card = await _context.VisitingCards
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CardId == cardId);

            var now = DateTime.Now;
            var today = now.Date;
            var weekAgo = today.AddDays(-7);
            var monthAgo = today.AddMonths(-1);

            return new AnalyticsDashboardViewModel
            {
                TotalViews = cardViews.Count,
                UniqueViewers = cardViews.GroupBy(cv => cv.ViewerIP).Count(),
                ViewsToday = cardViews.Count(cv => cv.ViewedAt.Date == today),
                ViewsThisWeek = cardViews.Count(cv => cv.ViewedAt.Date >= weekAgo),
                ViewsThisMonth = cardViews.Count(cv => cv.ViewedAt.Date >= monthAgo),
                ViewsTrend = GetViewsTrend(cardViews, 30),
                TopCountries = GetTopCountriesBreakdown(cardViews),
                TopDevices = GetTopDevicesBreakdown(cardViews),
                TopBrowsers = GetTopBrowsersBreakdown(cardViews),
                CardName = card?.FullName ?? "Unknown",
                Company = card?.Company ?? "",
                CreatedAt = card?.CreatedAt ?? DateTime.Now,
                LastViewed = card?.LastViewed,
                RecentViews = cardViews.OrderByDescending(cv => cv.ViewedAt).Take(20).ToList()
            };
        }

        public async Task<AnalyticsDashboardViewModel> GetUserAnalyticsAsync(int userId)
        {
            var userCards = await _context.VisitingCards
                .Where(c => c.UserId == userId)
                .Include(c => c.User)
                .ToListAsync();

            var cardIds = userCards.Select(c => c.CardId).ToList();

            var cardViews = await _context.CardViews
                .Where(cv => cardIds.Contains(cv.CardId))
                .ToListAsync();

            var now = DateTime.Now;
            var today = now.Date;
            var weekAgo = today.AddDays(-7);
            var monthAgo = today.AddMonths(-1);

            return new AnalyticsDashboardViewModel
            {
                TotalCards = userCards.Count,
                TotalViews = cardViews.Count,
                UniqueViewers = cardViews.GroupBy(cv => cv.ViewerIP).Count(),
                ViewsToday = cardViews.Count(cv => cv.ViewedAt.Date == today),
                ViewsThisWeek = cardViews.Count(cv => cv.ViewedAt.Date >= weekAgo),
                ViewsThisMonth = cardViews.Count(cv => cv.ViewedAt.Date >= monthAgo),
                CardAnalytics = userCards.Select(card => new CardAnalyticsViewModel
                {
                    CardId = card.CardId,
                    CardName = card.User.FullName,
                    Company = card.Company ?? "",
                    ViewCount = cardViews.Count(cv => cv.CardId == card.CardId),
                    LastViewed = cardViews.Where(cv => cv.CardId == card.CardId)
                                        .OrderByDescending(cv => cv.ViewedAt)
                                        .FirstOrDefault()?.ViewedAt
                }).ToList(),
                ViewsTrend = GetViewsTrend(cardViews, 30),
                TopCountries = GetTopCountriesBreakdown(cardViews),
                TopDevices = GetTopDevicesBreakdown(cardViews),
                TopBrowsers = GetTopBrowsersBreakdown(cardViews)
            };
        }

        private string GetClientIPAddress(HttpRequest request)
        {
            return request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim()
                ?? request.Headers["X-Real-IP"].FirstOrDefault()
                ?? request.HttpContext.Connection.RemoteIpAddress?.ToString()
                ?? "Unknown";
        }

        private string GetDeviceType(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent)) return "Unknown";

            userAgent = userAgent.ToLower();
            if (userAgent.Contains("mobile") || userAgent.Contains("android") || userAgent.Contains("iphone"))
                return "Mobile";
            if (userAgent.Contains("tablet") || userAgent.Contains("ipad"))
                return "Tablet";
            return "Desktop";
        }

        private string GetBrowser(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent)) return "Unknown";

            userAgent = userAgent.ToLower();
            if (userAgent.Contains("chrome")) return "Chrome";
            if (userAgent.Contains("firefox")) return "Firefox";
            if (userAgent.Contains("safari")) return "Safari";
            if (userAgent.Contains("edge")) return "Edge";
            if (userAgent.Contains("opera")) return "Opera";
            return "Other";
        }

        private List<ViewsByDate> GetViewsTrend(List<CardView> cardViews, int days)
        {
            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddDays(-days);

            return Enumerable.Range(0, days + 1)
                .Select(i => startDate.AddDays(i))
                .Select(date => new ViewsByDate
                {
                    Date = date,
                    Views = cardViews.Count(cv => cv.ViewedAt.Date == date)
                })
                .ToList();
        }

        private List<CountryBreakdown> GetTopCountriesBreakdown(List<CardView> cardViews)
        {
            var total = cardViews.Count;
            if (total == 0) return new List<CountryBreakdown>();

            return cardViews
                .GroupBy(cv => cv.Country ?? "Unknown")
                .Select(g => new CountryBreakdown
                {
                    Country = g.Key,
                    Count = g.Count(),
                    Percentage = (double)g.Count() / total * 100
                })
                .OrderByDescending(c => c.Count)
                .Take(10)
                .ToList();
        }

        private List<DeviceBreakdown> GetTopDevicesBreakdown(List<CardView> cardViews)
        {
            var total = cardViews.Count;
            if (total == 0) return new List<DeviceBreakdown>();

            return cardViews
                .GroupBy(cv => cv.DeviceType ?? "Unknown")
                .Select(g => new DeviceBreakdown
                {
                    DeviceType = g.Key,
                    Count = g.Count(),
                    Percentage = (double)g.Count() / total * 100
                })
                .OrderByDescending(d => d.Count)
                .ToList();
        }

        private List<BrowserBreakdown> GetTopBrowsersBreakdown(List<CardView> cardViews)
        {
            var total = cardViews.Count;
            if (total == 0) return new List<BrowserBreakdown>();

            return cardViews
                .GroupBy(cv => cv.Browser ?? "Unknown")
                .Select(g => new BrowserBreakdown
                {
                    Browser = g.Key,
                    Count = g.Count(),
                    Percentage = (double)g.Count() / total * 100
                })
                .OrderByDescending(b => b.Count)
                .ToList();
        }
    }
}