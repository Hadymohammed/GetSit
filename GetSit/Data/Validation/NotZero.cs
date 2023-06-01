using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.Validation
{
    public class NotZero: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value?.ToString() == "0")
                return false;
            return true;
        }
    }
}
