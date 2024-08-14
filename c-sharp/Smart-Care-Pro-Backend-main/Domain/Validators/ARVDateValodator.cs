using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Domain.Validators
{
    public class ARVDateValodator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (validationContext.MemberName == "ARVStartDateForMother")
            {
                Object instance = validationContext.ObjectInstance;
                Type type = instance.GetType();
                Object proprtyvalue = type.GetProperty("ARVEndDateForMother").GetValue(instance, null);

                if (proprtyvalue == null)
                {
                    return ValidationResult.Success;
                }
                else if (value != null && proprtyvalue != null)
                {
                    if (Convert.ToDateTime(proprtyvalue) < Convert.ToDateTime(value))
                    {
                        return new ValidationResult("ARV EndDate For Mother cannot be less then start date");
                    }
                }
                else
                {
                    return ValidationResult.Success;

                }
            }
            if (validationContext.MemberName == "ARVStartDateForChild")
            {
                Object instance = validationContext.ObjectInstance;
                Type type = instance.GetType();
                Object proprtyvalue = type.GetProperty("ARVEndDateForChild").GetValue(instance, null);

                if (proprtyvalue == null)
                {
                    return ValidationResult.Success;
                }
                else if (value != null && proprtyvalue != null)
                {
                    if (Convert.ToDateTime(proprtyvalue) < Convert.ToDateTime(value))
                    {
                        return new ValidationResult("ARV EndDate For Mother cannot be less then start date");
                    }
                }
                else
                {
                    return ValidationResult.Success;

                }
            } 

            return ValidationResult.Success;
             
        }
    }
}
