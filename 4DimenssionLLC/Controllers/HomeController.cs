using _4DimenssionLLC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;

namespace _4DimenssionLLC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
		[HttpPost]
		public IActionResult SendEmail(string recipient,  string body)
		{
			try
			{				
			
				using (var message = new MailMessage())
				{
					message.From = new MailAddress("admin@4dsps.com");
					message.To.Add("info@dimensionsllc.com");
					message.Subject = "";
					message.Body = body;

					using (var client = new SmtpClient("email-smtp.us-east-1.amazonaws.com"))
					{
						//client.UseDefaultCredentials = false;
						client.Credentials = new System.Net.NetworkCredential("AKIAZAN35G64ZK35DH7B", "BJr/bO8pFdND+Ef0LAxkjfFIbdA3Ms72/fU+G9nSLS0F");
						client.EnableSsl = true;
						client.Port = 587;
						//EnableSsl = true,
						client.DeliveryMethod = SmtpDeliveryMethod.Network;
						client.UseDefaultCredentials = false;
						client.Send(message);
					}
				}

				return Ok("Email sent successfully!");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Failed to send email: {ex.Message}");
			}
		}
		public IActionResult PrivacyPolicy()
        {
            return View();
        }
		public IActionResult CompanyPolicy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}