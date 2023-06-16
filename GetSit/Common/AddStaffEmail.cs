using GetSit.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MVCTutorial.Controllers;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Mail;
using static System.Net.WebRequestMethods;


namespace GetSit.Common
{
    public class AddStaffEmail
    {
        public const string SessionKey = "OTP";
        private string? recipient;
        public string? APIKey { get; private set; }
        static public bool SendEmailAddStaff(string email, string Url)
        {
            try
            {
                #region sendingProcess
                var fromAddress = new MailAddress("getsit594@gmail.com", "GetSit");
                var toAddress = new MailAddress(email, "New Staff");
                const string fromPassword = "esyqrxcqijyqnpwf";
                const string subject = "Your invetation";
                string body = $"Sent from GetSit , here is your invetation link , do not share it with anyone  {Url} . ";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
                #endregion
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }
    }
}
