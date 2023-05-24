using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Data.Validation
{
    public class PhoneValidator:ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var number = value?.ToString();
            if(number == null)return false;

            char[] valids = { '1', '2', '0', '5' };
            if (number[0].Equals('0') && number[1].Equals('1') && valids.Contains(number[2]) && number.Length == 11)
            {
                return true;
            }
            
            return false;
        }
    }
}
