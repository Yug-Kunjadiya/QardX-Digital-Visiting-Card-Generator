using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QardX.Data;
using QardX.Models;
using QardX.Services;

namespace QardX.Controllers
{
    public class ContactController : Controller
    {
        private readonly QardXDbContext _context;
        private readonly IEmailService _emailService;

        public ContactController(QardXDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Submit(ContactForm contactForm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contactForm.SubmittedAt = DateTime.Now;
                    contactForm.IsRead = false;

                    _context.ContactForms.Add(contactForm);
                    await _context.SaveChangesAsync();

                    // Send notification email to card owner
                    await _emailService.SendContactFormNotificationAsync(contactForm);

                    return Json(new { success = true, message = "Your message has been sent successfully!" });
                }

                return Json(new { success = false, message = "Please fill in all required fields." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while sending your message." });
            }
        }

        public async Task<IActionResult> Embed(int cardId)
        {
            var card = await _context.VisitingCards
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CardId == cardId);

            if (card == null)
                return NotFound();

            return View(card);
        }

        [HttpGet]
        public IActionResult GetEmbedCode(int cardId)
        {
            var embedCode = $@"
                <div id='qardx-contact-form-{cardId}' style='max-width: 500px; font-family: Arial, sans-serif;'></div>
                <script>
                (function() {{
                    var form = document.createElement('div');
                    form.innerHTML = `
                        <style>
                            .qardx-form {{ background: #f8f9fa; padding: 20px; border-radius: 10px; border: 1px solid #dee2e6; }}
                            .qardx-form h4 {{ color: #3456a3; margin-bottom: 15px; }}
                            .qardx-form input, .qardx-form textarea {{ width: 100%; padding: 10px; margin-bottom: 15px; border: 1px solid #ccc; border-radius: 5px; box-sizing: border-box; }}
                            .qardx-form button {{ background: linear-gradient(135deg, #3456a3, #5478c4); color: white; padding: 12px 25px; border: none; border-radius: 5px; cursor: pointer; width: 100%; }}
                            .qardx-form button:hover {{ background: linear-gradient(135deg, #2a4a8f, #4a6bb3); }}
                            .qardx-message {{ padding: 10px; margin-top: 10px; border-radius: 5px; }}
                            .qardx-success {{ background: #d4edda; color: #155724; border: 1px solid #c3e6cb; }}
                            .qardx-error {{ background: #f8d7da; color: #721c24; border: 1px solid #f5c6cb; }}
                        </style>
                        <form class='qardx-form' onsubmit='submitQardXContact(event, {cardId})'>
                            <h4>Get in Touch</h4>
                            <input type='text' name='name' placeholder='Your Name *' required>
                            <input type='email' name='email' placeholder='Your Email *' required>
                            <input type='tel' name='phone' placeholder='Your Phone'>
                            <input type='text' name='company' placeholder='Your Company'>
                            <textarea name='message' rows='4' placeholder='Your Message *' required></textarea>
                            <button type='submit'>Send Message</button>
                            <div id='qardx-message-{cardId}' class='qardx-message' style='display: none;'></div>
                        </form>
                    `;
                    document.getElementById('qardx-contact-form-{cardId}').appendChild(form);
                    
                    window.submitQardXContact = function(event, cardId) {{
                        event.preventDefault();
                        var form = event.target;
                        var formData = new FormData(form);
                        var messageDiv = document.getElementById('qardx-message-' + cardId);
                        
                        var data = {{
                            cardId: cardId,
                            name: formData.get('name'),
                            email: formData.get('email'),
                            phone: formData.get('phone') || '',
                            company: formData.get('company') || '',
                            message: formData.get('message')
                        }};
                        
                        fetch('https://localhost:5000/Contact/Submit', {{
                            method: 'POST',
                            headers: {{ 'Content-Type': 'application/json' }},
                            body: JSON.stringify(data)
                        }})
                        .then(response => response.json())
                        .then(result => {{
                            messageDiv.style.display = 'block';
                            if (result.success) {{
                                messageDiv.className = 'qardx-message qardx-success';
                                messageDiv.textContent = result.message;
                                form.reset();
                            }} else {{
                                messageDiv.className = 'qardx-message qardx-error';
                                messageDiv.textContent = result.message;
                            }}
                        }})
                        .catch(error => {{
                            messageDiv.style.display = 'block';
                            messageDiv.className = 'qardx-message qardx-error';
                            messageDiv.textContent = 'An error occurred. Please try again.';
                        }});
                    }};
                }})();
                </script>";

            return Json(new { embedCode });
        }
    }
}