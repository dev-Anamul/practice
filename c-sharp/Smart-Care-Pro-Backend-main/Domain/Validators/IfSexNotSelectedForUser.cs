using Domain.Dto;
using Domain.Entities;
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
    public class IfSexNotSelectedForUser : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance.GetType().Name == "UserAccountDto")
            {
                var user = (UserAccountDto)validationContext.ObjectInstance;

                if (user.Sex == 0)
                    return new ValidationResult(MessageConstants.RequiredFieldError);
            }
            else if (validationContext.ObjectInstance.GetType().Name == "UserAccountEditDto")
            {
                var user = (UserAccountDto)validationContext.ObjectInstance;

                if (user.Sex == 0)
                    return new ValidationResult(MessageConstants.RequiredFieldError);
            }
            else
            {
                var user = (UserAccount)validationContext.ObjectInstance;

                if (user.Sex == 0)
                    return new ValidationResult(MessageConstants.RequiredFieldError);
            }

            return ValidationResult.Success;
        }
    }
}