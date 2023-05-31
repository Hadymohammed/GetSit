using GetSit.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MVCTutorial.Controllers;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace GetSit.Common
{
    public class OTPServices
    {
        public const string SessionKey = "OTP";
        private string? recipient;
        public string? APIKey { get; private set; }
       
        static public bool SendOTP(HttpContext httpContext)
        {
            try { 
            int otpValue = new Random().Next(100000, 999999);
            httpContext.Session.SetString(SessionKey,otpValue.ToString());
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
            }catch(Exception err)
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
