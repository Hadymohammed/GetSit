using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Net;
using System.Web;
namespace MVCTutorial.Controllers
{
    public class OTP : Controller
    {

        public ActionResult Index()
        {

            return View();
        }
        public JsonResult SendOTP()
        {
            int otpValue = new Random().Next(100000, 999999);
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
        }

        public JsonResult Json(string status, object allowGet)
        {
            throw new NotImplementedException();
        }

        public ActionResult EnterOTP()
        {
            return View();
        }

        public const string SessionKeyName = "123456";
        private string? recipient;

        public string? APIKey { get; private set; }

        [HttpPost]
        public JsonResult VerifyOTP(string otp, OTPData mydata)
        {
            bool result = false;

            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                HttpContext.Session.SetString(SessionKeyName, mydata.OTP);
            }
            var receivedOTP = HttpContext.Session.GetString(SessionKeyName);
            if (otp == receivedOTP)
            {
                result = true;
            }
            return Json(result);
        }
    }
}