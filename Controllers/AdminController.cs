using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardX.Data;
using QardX.Models;
using QardX.Services;

namespace QardX.Controllers
{
    public class AdminController : Controller
    {
        private readonly QardXDbContext _context;
        private readonly IAnalyticsService _analyticsService;
        private const string AdminPassword = "admin2025"; // Set your admin password here
        private const string AdminSessionKey = "IsAdminAuthenticated";

        public AdminController(QardXDbContext context, IAnalyticsService analyticsService)
        {
            _context = context;
            _analyticsService = analyticsService;
        }

        [HttpPost]
        public IActionResult VerifyPassword([FromBody] AdminPasswordRequest request)
        {
            if (request.Password == AdminPassword)
            {
                HttpContext.Session.SetString(AdminSessionKey, "true");
                return Json(new { success = true });
            }
            
            return Json(new { success = false });
        }

        private bool IsAdminAuthenticated()
        {
            return HttpContext.Session.GetString(AdminSessionKey) == "true";
        }

        private IActionResult CheckAdminAuth()
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }
            return null;
        }

        public async Task<IActionResult> Index()
        {
            var authCheck = CheckAdminAuth();
            if (authCheck != null) return authCheck;

            var stats = new AdminDashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalCards = await _context.VisitingCards.CountAsync(),
                TotalTemplates = await _context.Templates.CountAsync(),
                RecentCards = await _context.VisitingCards
                    .Include(c => c.User)
                    .Include(c => c.Template)
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(10)
                    .ToListAsync()
            };

            return View(stats);
        }

        public async Task<IActionResult> Templates()
        {
            var authCheck = CheckAdminAuth();
            if (authCheck != null) return authCheck;

            var templates = await _context.Templates.ToListAsync();
            return View(templates);
        }

        public async Task<IActionResult> Users()
        {
            var authCheck = CheckAdminAuth();
            if (authCheck != null) return authCheck;

            var users = await _context.Users
                .Include(u => u.VisitingCards)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Cards()
        {
            var authCheck = CheckAdminAuth();
            if (authCheck != null) return authCheck;

            var cards = await _context.VisitingCards
                .Include(c => c.User)
                .Include(c => c.Template)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(cards);
        }

        public async Task<IActionResult> ContactSubmissions()
        {
            var authCheck = CheckAdminAuth();
            if (authCheck != null) return authCheck;

            var contacts = await _context.ContactForms
                .Include(c => c.Card)
                .ThenInclude(c => c.User)
                .OrderByDescending(c => c.SubmittedAt)
                .ToListAsync();
            return View(contacts);
        }

        public async Task<IActionResult> Analytics()
        {
            var authCheck = CheckAdminAuth();
            if (authCheck != null) return authCheck;

            // Get overall analytics for all users/cards
            var totalViews = await _context.CardViews.CountAsync();
            var totalCards = await _context.VisitingCards.CountAsync();
            var totalUsers = await _context.Users.CountAsync();
            
            // Get recent views
            var recentViews = await _context.CardViews
                .Include(v => v.VisitingCard)
                .ThenInclude(c => c.User)
                .OrderByDescending(v => v.ViewedAt)
                .Take(10)
                .ToListAsync();

            var viewModel = new AdminAnalyticsViewModel
            {
                TotalViews = totalViews,
                TotalCards = totalCards,
                TotalUsers = totalUsers,
                RecentViews = recentViews
            };

            return View(viewModel);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(AdminSessionKey);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> GetContactMessage(int id)
        {
            var authCheck = CheckAdminAuth();
            if (authCheck != null) return Json(new { success = false, message = "Unauthorized" });

            try
            {
                var contact = await _context.ContactForms
                    .Include(c => c.Card)
                    .FirstOrDefaultAsync(c => c.ContactFormId == id);

                if (contact == null)
                    return Json(new { success = false, message = "Contact not found" });

                return Json(new { 
                    success = true, 
                    contact = new {
                        name = contact.Name,
                        email = contact.Email,
                        phone = contact.Phone,
                        company = contact.Company,
                        message = contact.Message,
                        submittedAt = contact.SubmittedAt,
                        isRead = contact.IsRead
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error loading contact" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkContactAsRead(int id)
        {
            var authCheck = CheckAdminAuth();
            if (authCheck != null) return Json(new { success = false, message = "Unauthorized" });

            try
            {
                var contact = await _context.ContactForms.FindAsync(id);
                if (contact == null)
                    return Json(new { success = false, message = "Contact not found" });

                contact.IsRead = true;
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error updating contact" });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var authCheck = CheckAdminAuth();
            if (authCheck != null) return Json(new { success = false, message = "Unauthorized" });

            try
            {
                var contact = await _context.ContactForms.FindAsync(id);
                if (contact == null)
                    return Json(new { success = false, message = "Contact not found" });

                _context.ContactForms.Remove(contact);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting contact" });
            }
        }
    }
}