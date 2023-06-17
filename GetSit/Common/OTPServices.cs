using GetSit.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MVCTutorial.Controllers;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using static System.Net.WebRequestMethods;

namespace GetSit.Common
{
    public class OTPServices
    {
        public const string SessionKey = "OTP";
        private string? recipient;
        public string? APIKey { get; private set; }
        static public bool SendPhoneOTP(HttpContext httpContext,string phone)
        {
            try { 
            int otpValue = new Random().Next(100000, 999999);
            httpContext.Session.SetString(SessionKey,otpValue.ToString());
                #region sendingProcess
                const string accountSid = "AC341704b0e6aac63c0cce380809b4c370";
                const string authToken = "e7d7ce3a1fff7842291aa952625b1af1";

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: "Your Otp value is " + otpValue.ToString(),
                    from: new Twilio.Types.PhoneNumber("+14067097984"),
                    to: new Twilio.Types.PhoneNumber("+2"+phone)
                );
                #endregion
                return true;
            }catch(Exception err)
            {
                return false;
            }
        }
        static public bool SendEmailOTP(HttpContext httpContext,string email)
        {
            try
            {
                int otpValue = new Random().Next(100000, 999999);
                httpContext.Session.SetString(SessionKey, otpValue.ToString());
                #region sendingProcess
                var fromAddress = new MailAddress("getsit594@gmail.com", "GetSit");
                var toAddress = new MailAddress(email, "New User");
                const string fromPassword = "esyqrxcqijyqnpwf";
                const string subject = "Your OTP";
                string body = $"Sent from GetSit , Here is your OTP is {otpValue} . ";
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
        static public bool VerifyOTP(HttpContext httpContext, OTPVM receivedOTP)
        {
            bool result = false;
            /*var resceivedStringOTP = string.Join("", receivedOTP.OTP);*/
            var chars = new char[6];
            chars[0] = receivedOTP.op0; chars[1] = receivedOTP.op1;chars[2] = receivedOTP.op2;chars[3] = receivedOTP.op3;chars[4] = receivedOTP.op4;chars[5] =receivedOTP.op5;
            var resceivedStringOTP = string.Join("",chars);

            var ValidOTP = httpContext.Session.GetString(SessionKey);
            if (string.IsNullOrEmpty(ValidOTP))
            {
                return false;
            }
            //var stringReceviedOTP = string.Join("", receivedOTP.OTP);

            if (resceivedStringOTP == ValidOTP)
            {
                return true;
            }
            return false;
        }
    }
}
