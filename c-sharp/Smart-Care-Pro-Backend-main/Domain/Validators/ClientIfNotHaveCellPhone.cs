﻿using System.ComponentModel.DataAnnotations;
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
    public class ClientIfNotHaveCellPhone : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty("HasCellphone").GetValue(instance, null);

            if (!Convert.ToBoolean(proprtyvalue) && value == null)
                return new ValidationResult(MessageConstants.CellphoneRequired);

            return ValidationResult.Success;
        }
    }
}