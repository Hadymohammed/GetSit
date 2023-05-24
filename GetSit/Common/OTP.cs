using GetSit.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MVCTutorial.Controllers;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace GetSit.Common
{
    public class OTP
    {
        HttpContext _httpContext;
        public const string SessionKey = "OTP";
        private string? recipient;
        public string? APIKey { get; private set; }
        public OTP(HttpContext context)
        {
            _httpContext = context;
        }

        public bool SendOTP()
        {
            int otpValue = new Random().Next(100000, 999999);
            _httpContext.Session.SetString(SessionKey,otpValue.ToString());
            #region sendingProcess
            /* 
            var status = "";
            try
            {

                string message = "Your OTP Number is " + otpValue + " ( Sent By : GetSit )";
                String encodedMessage = HttpUtility.UrlEncode(message);
                using (var webClient = new WebClient())
                {
                    byte[] response = webClient.UploadValues("", new NameValueCollection(){

                                         {"apikey" , APIKey},
                                         {"numbers" , recipient},
                                         {"message" , encodedMessage},
                                         {"sender" , ""}});

                    string result = System.Text.Encoding.UTF8.GetString(response);

                    JObject jsonObject = JObject.Parse(result);
                }

                return Json(encodedMessage);
            }
            catch (Exception e)
            {
                throw (e);
            }
            */
            #endregion
            return true;
        }
        public bool VerifyOTP(OTPVM receivedOTP)
        {
            bool result = false;
            var resceivedStringOTP = string.Join("", receivedOTP.OTP);

            var ValidOTP = _httpContext.Session.GetString(SessionKey);
            if (string.IsNullOrEmpty(ValidOTP))
            {
                return false;
            }
            var stringReceviedOTP = string.Join("", receivedOTP.OTP);

            if (stringReceviedOTP == ValidOTP)
            {
                result = true;
            }
            return false;
        }
    }
}
