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
    public class IfNotInteger : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                if (validationContext.MemberName == "Cellphone" || validationContext.MemberName == "OtherCellphone")
                {
                    Object instance = validationContext.ObjectInstance;
                    Type type = instance.GetType();
                    Object proprtyvalue = type.GetProperty("NoCellphone").GetValue(instance, null);

                    if (Convert.ToBoolean(proprtyvalue) == true)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        if (validationContext.MemberName == "OtherCellphone")
                        {
                            Object instanceOtherCellphoneCountryCode = validationContext.ObjectInstance;
                            Type typeOtherCellphoneCountryCode = instance.GetType();
                            Object OtherCellphoneCountryCodevalue = type.GetProperty("OtherCellphoneCountryCode").GetValue(instance, null);

                            if (OtherCellphoneCountryCodevalue == null)
                                return ValidationResult.Success;
                            else
                                return new ValidationResult(MessageConstants.OtherCellphoneRequired);
                        }

                        return new ValidationResult(MessageConstants.IfNotAlphabet);
                    }
                }

                if (validationContext.MemberName == "LandPhone")
                {
                    Object instanceOtherCellphoneCountryCode = validationContext.ObjectInstance;
                    Type typeOtherCellphoneCountryCode = instanceOtherCellphoneCountryCode.GetType();
                    Object OtherCellphoneCountryCodevalue = typeOtherCellphoneCountryCode.GetProperty("LandPhoneCountryCode").GetValue(instanceOtherCellphoneCountryCode, null);

                    if (OtherCellphoneCountryCodevalue == null)
                        return ValidationResult.Success;
                    else
                        return new ValidationResult(MessageConstants.OtherCellphoneCountryCodeRequired);
                }

                return ValidationResult.Success;
            }

            bool isvalid = Regex.IsMatch(value.ToString(), @"^[0-9]+$", RegexOptions.IgnoreCase);

            if (!isvalid)
                return new ValidationResult(MessageConstants.IfNotInteger);
            else
                return ValidationResult.Success;
        }
    }
}