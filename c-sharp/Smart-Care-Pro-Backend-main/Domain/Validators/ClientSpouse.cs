using System.ComponentModel.DataAnnotations;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 12.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Validators
{
    public class ClientSpouse : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                Object instance = validationContext.ObjectInstance;
                Type type = instance.GetType();
                Object proprtyvalue = type.GetProperty("MaritalStatus").GetValue(instance, null);

                if (Convert.ToInt32(proprtyvalue) == 1 || Convert.ToInt32(proprtyvalue) == 3 || Convert.ToInt32(proprtyvalue) == 4)
                    return ValidationResult.Success;

                return new ValidationResult(MessageConstants.SpouseName);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}