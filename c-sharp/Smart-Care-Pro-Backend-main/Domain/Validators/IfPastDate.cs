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
    public class IfPastDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime date = (DateTime)value;

                if (date.Date < DateTime.Now.Date)
                    return new ValidationResult(MessageConstants.IfPastDate);
            }

            return ValidationResult.Success;
        }
    }
}