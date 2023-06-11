using GetSit.Data.enums;
using GetSit.Data.Security;
using GetSit.Data.ViewModels;
using Newtonsoft.Json;

namespace GetSit.Common
{
    static public class SessoinHelper
    {
        public const string UserKey = "UserModel"; 
        public const string ForgetPasswordKey = "ForgetPasswordModel"; 
        public const string RegisterKey = "RegisterVM"; 
        static public bool saveObject(HttpContext context,string key,Object obj)
        {
            try
            {
                context.Session.SetString(key, JsonConvert.SerializeObject(obj));
                return true;
            }catch(Exception err)
            {
                return false;
            }
        }
        static public T? getObject<T>(HttpContext context, string key)
        {
            try
            {
            var stringObj = context.Session.GetString(key);
            var obj = JsonConvert.DeserializeObject<T>(stringObj);
                return obj;
            }catch(Exception err)
            {
                return default(T);
            }
        }
        static public T getUser<T>(HttpContext context) where T : IAbstractUser
        {
            var stringUser = context.Session.GetString("UserModel");
            var user = JsonConvert.DeserializeObject<T>(stringUser);
            return user;
        }

    }
}
