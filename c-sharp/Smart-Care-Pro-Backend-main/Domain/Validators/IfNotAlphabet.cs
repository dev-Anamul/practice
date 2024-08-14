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
namespace Domain.Validations
{
    public class IfNotAlphabet : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                if (validationContext.MemberName == "SpousesLegalName" || validationContext.MemberName == "SpousesSurname")
                {
                    Object instance = validationContext.ObjectInstance;
                    Type type = instance.GetType();
                    Object proprtyvalue = type.GetProperty("MaritalStatus").GetValue(instance, null);

                    if (Convert.ToInt32(proprtyvalue) == 1)
                        return ValidationResult.Success;
                    else
                        return new ValidationResult(MessageConstants.IfNotAlphabet);
                }
            }

            bool isvalid = Regex.IsMatch(value.ToString(), @"^[a-zA-Z ]+$", RegexOptions.IgnoreCase);

            if (!isvalid)
                return new ValidationResult(MessageConstants.IfNotAlphabet);
            else
                return ValidationResult.Success;
        }
    }
}