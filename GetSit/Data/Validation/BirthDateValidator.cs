using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.Validation
{
    public class BirthDateValidator:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt = DateTime.Parse(value.ToString());
            DateTime dt_now = DateTime.Now;

            DateTime dt_18 = dt.AddYears(18); //here add years, not subtract

            if (dt_18.Date >= dt_now.Date) //here you want to compare dt_now
            {
                return false;
            }
            return true;
        }
    }
}
