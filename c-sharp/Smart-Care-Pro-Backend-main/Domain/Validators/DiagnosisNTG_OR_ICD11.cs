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
    public class DiagnosisNTG_OR_ICD11 : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int DiagnoseTypeId = 0;
            if (value != null)
            {
                try
                {
                    DiagnoseTypeId = Convert.ToInt32(value);
                }
                catch
                {

                }
            }
            if (value == null || DiagnoseTypeId == 0)
            {
                if (validationContext.MemberName == "NTGId")
                {
                    Object instance = validationContext.ObjectInstance;
                    Type type = instance.GetType();
                    Object proprtyvalue = type.GetProperty("ICDId").GetValue(instance, null);

                    if (proprtyvalue == null)
                        return new ValidationResult(MessageConstants.DiagnosisTypeValidatorMessage);
                    else
                    {
                        if (Convert.ToInt32(proprtyvalue) > 0)
                            return ValidationResult.Success;
                        else
                            return new ValidationResult(MessageConstants.DiagnosisTypeValidatorMessage);
                    }
                }

                if (validationContext.MemberName == "ICDId")
                {
                    Object instance = validationContext.ObjectInstance;
                    Type type = instance.GetType();
                    Object proprtyvalue = type.GetProperty("NTGId").GetValue(instance, null);

                    if (proprtyvalue == null)
                        return new ValidationResult(MessageConstants.DiagnosisTypeValidatorMessage);
                    else
                    {
                        if (Convert.ToInt32(proprtyvalue) > 0)
                            return ValidationResult.Success;
                        else
                            return new ValidationResult(MessageConstants.DiagnosisTypeValidatorMessage);
                    }
                }

                return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }
    }
}