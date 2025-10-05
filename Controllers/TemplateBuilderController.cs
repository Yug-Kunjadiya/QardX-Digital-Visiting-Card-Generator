using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardX.Data;
using QardX.Models;
using System.Security.Claims;

namespace QardX.Controllers
{
    [Authorize]
    public class TemplateBuilderController : Controller
    {
        private readonly QardXDbContext _context;

        public TemplateBuilderController(QardXDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Builder()
        {
            var model = new CustomTemplate
            {
                UserId = GetCurrentUserId(),
                TemplateName = "My Custom Template",
                HtmlContent = GetDefaultHtmlTemplate(),
                CssContent = GetDefaultCssStyles(),
                IsPublic = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] CustomTemplate template)
        {
            try
            {
                template.UserId = GetCurrentUserId();
                template.CreatedAt = DateTime.Now;
                template.UpdatedAt = DateTime.Now;
                template.IsPublic = false;

                _context.CustomTemplates.Add(template);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Template saved successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to save template: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Preview([FromBody] PreviewRequest request)
        {
            return Json(new { 
                success = true, 
                html = GeneratePreviewHtml(request.HtmlContent, request.CssContent, request.SampleData) 
            });
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        }

        private string GetDefaultHtmlTemplate()
        {
            return @"<div class=""card-template"">
    <div class=""header"">
        <h2 class=""name"">{{Name}}</h2>
        <p class=""title"">{{JobTitle}}</p>
        <p class=""company"">{{Company}}</p>
    </div>
    <div class=""content"">
        <div class=""contact-info"">
            <p><i class=""fas fa-envelope""></i> {{Email}}</p>
            <p><i class=""fas fa-phone""></i> {{Phone}}</p>
            <p><i class=""fas fa-globe""></i> {{Website}}</p>
        </div>
        <div class=""social-links"">
            <p><i class=""fab fa-linkedin""></i> {{LinkedIn}}</p>
            <p><i class=""fab fa-twitter""></i> {{Twitter}}</p>
            <p><i class=""fab fa-instagram""></i> {{Instagram}}</p>
        </div>
    </div>
</div>";
        }

        private string GetDefaultCssStyles()
        {
            return @".card-template {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
    padding: 30px;
    border-radius: 15px;
    font-family: 'Segoe UI', sans-serif;
    max-width: 400px;
    margin: 0 auto;
    box-shadow: 0 10px 30px rgba(0,0,0,0.3);
}

.header {
    text-align: center;
    margin-bottom: 25px;
    border-bottom: 2px solid rgba(255,255,255,0.3);
    padding-bottom: 20px;
}

.name {
    font-size: 24px;
    font-weight: bold;
    margin: 0 0 10px 0;
    text-shadow: 0 2px 4px rgba(0,0,0,0.3);
}

.title {
    font-size: 16px;
    margin: 5px 0;
    opacity: 0.9;
}

.company {
    font-size: 14px;
    margin: 5px 0;
    opacity: 0.8;
}

.content {
    display: flex;
    justify-content: space-between;
    gap: 20px;
}

.contact-info, .social-links {
    flex: 1;
}

.contact-info p, .social-links p {
    margin: 8px 0;
    font-size: 13px;
    display: flex;
    align-items: center;
    gap: 8px;
}

.contact-info i, .social-links i {
    width: 16px;
    text-align: center;
}";
        }

        private string GeneratePreviewHtml(string htmlContent, string cssContent, string sampleData)
        {
            // Replace placeholders with sample data
            var processedHtml = htmlContent
                .Replace("{{Name}}", "John Doe")
                .Replace("{{JobTitle}}", "Software Engineer")
                .Replace("{{Company}}", "Tech Corp")
                .Replace("{{Email}}", "john@techcorp.com")
                .Replace("{{Phone}}", "+1-555-0123")
                .Replace("{{Website}}", "www.techcorp.com")
                .Replace("{{LinkedIn}}", "linkedin.com/in/johndoe")
                .Replace("{{Twitter}}", "@johndoe")
                .Replace("{{Instagram}}", "@johndoe");

            return $"<style>{cssContent}</style>{processedHtml}";
        }
    }

    public class PreviewRequest
    {
        public string HtmlContent { get; set; } = "";
        public string CssContent { get; set; } = "";
        public string SampleData { get; set; } = "";
    }
}
