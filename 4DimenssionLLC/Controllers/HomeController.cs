using _4DimenssionLLC.Models;
using _4DimenssionLLC.services;
using _4DimenssionLLC.ViewModels;
using com.blazor.decs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;

namespace _4DimenssionLLC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
		// Inject SQLiteDbManager and ILogger
		public HomeController( ILogger<HomeController> logger)
        {
            _logger = logger;
        }
		public HomeController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration configuration)
		{

			_hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));

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
        public async Task<BlazorResponseViewModel> AddContacts([FromForm] ContactFormViewModel pModel)
        {
            BlazorResponseViewModel response = new BlazorResponseViewModel();
            try
            {
                Int64 contactId = Convert.ToInt64(pModel.Id);

                // var files = Request.Form.Files;          
                List<ContactFormViewModel> fList = new List<ContactFormViewModel>();
                string applicationPath = _hostingEnvironment.ContentRootPath;
                string fileAbsolutePath = string.Empty;
                int fileCounter = 0;
                if (pModel.Id >= 0 && HttpContext.Request.Form != null)
                {
                    if (HttpContext.Request.Form.Files.Count() > 0)
                    {
                        byte[] buffer = new byte[16 * 1024];
                        //  string id = files.get;
                        foreach (IFormFile file in Request.Form.Files)
                        {
                            fileCounter++;
                            //fileCounter = string.IsNullOrWhiteSpace(file.Name) ? fileCounter++ : Convert.ToInt32(file.Name.Trim().Substring(file.Name.Length - 1));
                            string filename = Guid.NewGuid().ToString() + "_" + Path.GetExtension(file.FileName);
                            //string ext = string.IsNullOrWhiteSpace(Path.GetExtension(filename)) ? ".mp4" : Path.GetExtension(filename);
                            fileAbsolutePath = applicationPath + DECSConstant.UPLOAD_WEB_ROOT_UPLOADFOLDER + filename;
                            using (Stream readStream = file.OpenReadStream())
                            {
                                using (FileStream f = new FileStream(fileAbsolutePath, FileMode.Create))
                                {
                                    int readBytes;
                                    while ((readBytes = readStream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        await f.WriteAsync(buffer, 0, readBytes);
                                    }
                                    f.Flush();
                                    f.Close();
                                }
								fList.Add(new ContactFormViewModel { Id = 1, FirstName = "John", LastName = "Doe", Dob = DateTime.Now, Contact = 1234567890, Contact2 = 0, Email = "john.doe@example.com", Address = "123 Elm St", Message = "Test message", Reaction = 1 });
							}
						}
                    }
                    if (fList.Any())
                    {
                        pModel.MediaContents = fList.ToList();
                    }
                    //pModel.UploadSource = vModel.UploadSource == null ? (int)UTIL.UTIL.POST_SOURCE.WEB : vModel.UploadSource;
                    contactId = SQLiteDbManager.SaveContactForm(pModel);

                    // Send Email

                    //try
                    //{
                    //    if (productId > 0 && !string.IsNullOrWhiteSpace(ConfigurationsViewModel.EmailGroup))
                    //    {
                    //        EmailSender emailSender = new EmailSender();
                    //        await emailSender.SubmitProductEmailNotificationAsync(ConfigurationsViewModel.EmailGroup, pModel.name);
                    //    }
                    //}
                    //catch
                    //{

                    //}
                    response.data = contactId;
                    // response.message = string.Format(DECSConstant.INSERTED_SUCCESS, (string.IsNullOrWhiteSpace(pModel.name)? pModel.description: pModel.name), System.DateTime.Now.ToString("MM/dd/yyy hh:mm:ss"));
                }
                
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