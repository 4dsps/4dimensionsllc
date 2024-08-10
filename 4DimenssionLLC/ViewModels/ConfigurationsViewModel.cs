using System;

namespace _4DimenssionLLC.ViewModels
{
    public  class ConfigurationsViewModel
    {
        public static string EmailBody { get; set; } = String.Empty;
        public static string EmailSubject { get; set; } = "DECS Login";
        public static string ProductAknowledgementEmailBody { get; set; } = String.Empty;
        public static string EmailProductSubject { get; set; } = "Product Upload Notification";
        public static string SMTPServer { get; set; } = "smtp-ionos.com";
        public static  Int32 SMTPPort { get; set; }
        public static bool SSLEnabled { get; set; } = true;

        public static string SMTPUser { get; set; } = "";
        public static string SMTPPwd { get; set; } = "";
        public static string EmailGroup { get; set; } = "";       
    }
   
}
