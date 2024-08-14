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
namespace Domain.Validations
{
    public class IfYesNoNotSelected : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            byte val = (byte)value;

            if (val == 0)
                return new ValidationResult(MessageConstants.RequiredFieldError);

            return ValidationResult.Success;
        }
    }
}