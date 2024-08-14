using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
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
    public class IfNotEmailInCorrectFormat : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isvalid = Regex.IsMatch(value.ToString(), @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$", RegexOptions.IgnoreCase);

            if (!isvalid)
                return new ValidationResult(MessageConstants.IfNotEmailAddress);
            else
                return ValidationResult.Success;
        }
    }
}