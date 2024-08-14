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
    public class IfInvalidCountryCode : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                if (validationContext.MemberName == "CellphoneCountryCode" || validationContext.MemberName == "OtherCellphoneCountryCode")
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
                        if (validationContext.MemberName == "OtherCellphoneCountryCode")
                        {
                            Object instanceOtherCellphoneCountryCode = validationContext.ObjectInstance;
                            Type typeOtherCellphoneCountryCode = instance.GetType();
                            Object OtherCellphoneCountryCodevalue = type.GetProperty("OtherCellphone").GetValue(instance, null);

                            if (OtherCellphoneCountryCodevalue == null)
                                return ValidationResult.Success;
                            else
                                return new ValidationResult(MessageConstants.OtherCellphoneCountryCodeRequired);
                        }

                        return new ValidationResult(MessageConstants.IfNotAlphabet);
                    }
                }

                if (validationContext.MemberName == "LandPhoneCountryCode")
                {
                    Object instanceOtherCellphone = validationContext.ObjectInstance;
                    Type typeOtherCellphone = instanceOtherCellphone.GetType();
                    Object OtherCellphonevalue = typeOtherCellphone.GetProperty("LandPhone").GetValue(instanceOtherCellphone, null);

                    if (OtherCellphonevalue == null)
                        return ValidationResult.Success;
                    else
                        return new ValidationResult(MessageConstants.OtherCellphoneCountryCodeRequired);
                }

                return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }
    }
}