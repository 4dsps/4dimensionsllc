﻿using _4DimenssionLLC.Models;
using _4DimenssionLLC.services;
using _4DimenssionLLC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;

namespace _4DimenssionLLC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

		// Constructor with dependency injection
		public HomeController(
			ILogger<HomeController> logger,
			Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
		{
			_logger = logger;
			_hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult SendEmail(string recipient, string body)
		{
			try
			{
				using (var message = new MailMessage())
				{
					message.From = new MailAddress("admin@4dsps.com");
					message.To.Add(recipient); // Changed to use recipient parameter
					message.Subject = "";
					message.Body = body;

					using (var client = new SmtpClient("email-smtp.us-east-1.amazonaws.com"))
					{
						client.Credentials = new System.Net.NetworkCredential("AKIAZAN35G64ZK35DH7B", "BJr/bO8pFdND+Ef0LAxkjfFIbdA3Ms72/fU+G9nSLS0F");
						client.EnableSsl = true;
						client.Port = 587;
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

		[HttpPost]
		public async Task<BlazorResponseViewModel> AddContacts([FromForm] ContactFormViewModel pModel)
		{
			BlazorResponseViewModel response = new BlazorResponseViewModel();
			try
			{
				Int64 contactId = Convert.ToInt64(pModel.Id);
				List<ContactFormViewModel> fList = new List<ContactFormViewModel>
				{
					new ContactFormViewModel
					{
						Id = 1,
						FirstName = "John",
						LastName = "Doe",
						Dob = DateTime.Now,
						Contact = 1234567890,
						Contact2 = 1234567890,
						Email = "john.doe@example.com",
						Address = "123 Elm St",
						Message = "Test message",
						Reaction = 1
					}
				};

				// Save contact information
				SQLiteDbManager.SaveContactForm(fList.FirstOrDefault());
				response.data = contactId;
				response.status = true;
			}
			catch (Exception ex)
			{
				response.status = false;
				response.message = ex.Message;
			}
			return response;
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
