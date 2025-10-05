using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardX.Data;
using QardX.Models;

namespace QardX.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly QardXDbContext _context;

        public AdminController(QardXDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
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
            var templates = await _context.Templates.ToListAsync();
            return View(templates);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _context.Users
                .Include(u => u.VisitingCards)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Cards()
        {
            var cards = await _context.VisitingCards
                .Include(c => c.User)
                .Include(c => c.Template)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(cards);
        }

        public async Task<IActionResult> ContactSubmissions()
        {
            var contacts = await _context.ContactForms
                .Include(c => c.Card)
                .ThenInclude(c => c.User)
                .OrderByDescending(c => c.SubmittedAt)
                .ToListAsync();
            return View(contacts);
        }

        [HttpGet]
        public async Task<IActionResult> GetContactMessage(int id)
        {
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