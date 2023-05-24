using GetSit.Data.enums;
using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.Validation
{
    sealed public class UserRoleValidator:ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (Enum.IsDefined(typeof(UserRole), value))
                return true;
            return false;
        }
    }
}
