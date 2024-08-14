﻿using Domain.Entities;
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
    public class ClientIfOutsideZambia : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var client = (Client)validationContext.ObjectInstance;

            if (client.IsZambianBorn == true)
                return ValidationResult.Success;

            var birthpalce = value as string;

            return string.IsNullOrWhiteSpace(birthpalce)
                ? new ValidationResult(MessageConstants.RequiredFieldError)
                : ValidationResult.Success;
        }
    }
}