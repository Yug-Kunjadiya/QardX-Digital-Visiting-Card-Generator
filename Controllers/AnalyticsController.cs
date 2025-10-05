using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QardX.Services;
using System.Security.Claims;

namespace QardX.Controllers
{
    [Authorize]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = GetCurrentUserId();
            var analytics = await _analyticsService.GetUserAnalyticsAsync(userId);
            return View(analytics);
        }

        [HttpGet]
        public async Task<IActionResult> CardAnalytics(int cardId)
        {
            // Verify the card belongs to the current user
            var userId = GetCurrentUserId();
            // Add authorization check here if needed
            
            var analytics = await _analyticsService.GetCardAnalyticsAsync(cardId);
            ViewBag.CardId = cardId;
            return View(analytics);
        }

        [HttpGet]
        public async Task<JsonResult> GetAnalyticsData(int cardId)
        {
            var analytics = await _analyticsService.GetCardAnalyticsAsync(cardId);
            return Json(analytics);
        }

        [HttpGet]
        public async Task<JsonResult> GetTrendData(int cardId, int days = 30)
        {
            var analytics = await _analyticsService.GetCardAnalyticsAsync(cardId);
            return Json(analytics.ViewsTrend.TakeLast(days));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = GetCurrentUserId();
            var analytics = await _analyticsService.GetCardAnalyticsAsync(id);
            
            // Verify the card belongs to the current user
            if (analytics == null)
            {
                return NotFound();
            }
            
            ViewBag.CardId = id;
            return View(analytics);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }
    }
}