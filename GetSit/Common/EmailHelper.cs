using System;
using System.Diagnostics.Tracing;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Web.Helpers;


namespace GetSit.Common
{
    public class EmailHelper
    {
        private const string SenderEmail= "getsit594@gmail.com";
        private const string SenderName= "GetSit";
        private const string SenderPassword = "esyqrxcqijyqnpwf";
        static private bool SendEmail(string ReciverEmail, string messege,string subject,string ReciverName)
        {
            try
            {
                #region sendingProcess
                var fromAddress = new MailAddress(SenderEmail, SenderName);
                var toAddress = new MailAddress(ReciverEmail,ReciverName);
                string body = messege;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, SenderPassword)
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
        static public bool SendEmailAddStaff(string email, string Url, string SpaceName)
        {
                string body = $"Sent from GetSit , here is your invetation link to be a staff member at ${SpaceName} , do not share it with anyone  {Url} . ";
                const string subject = "Your invetation";
                const string reciverName = "New Staff";
            return SendEmail(email, body, subject, reciverName);
        }
        static public bool SendEmailAddAdmin(string email, string Url)
        {
            string body = $"Sent from GetSit , here is your invetation link to be an admin at GetSit , do not share it with anyone  {Url} . ";
            const string subject = "Your invetation";
            const string reciverName = "New Admin";
            return SendEmail(email, body, subject, reciverName);
        }
        static public bool SendSpaceRejection(string email, string Messege,string SpaceName)
        {
            const string subject = "Space Rejection";
            string reciverName = $"{SpaceName} Manager";
            return SendEmail(email, Messege, subject, reciverName);
        }
        static public bool SendHallRejection(string email, string Messege, string SpaceName)
        {
            const string subject = "Hall Rejection";
            string reciverName = $"{SpaceName} Manager";
            return SendEmail(email, Messege, subject, reciverName);
        }
        static public bool SendOTP(string email,string OTP)
        {
            string body = $"Sent from GetSit , Here is your OTP is {OTP} . Don't share it with anyone. ";
            const string subject = "Your OTP";
            const string reciverName = "New User";
            return SendEmail(email,body, subject, reciverName);
        }
    }
}
