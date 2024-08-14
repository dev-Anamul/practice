using System.ComponentModel.DataAnnotations;

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
    public class ClientIfAgeGreater18 : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty("DOB").GetValue(instance, null);
            DateTime Bday = Convert.ToDateTime(proprtyvalue);
            DateTime now = DateTime.Today;
            int age = now.Year - Bday.Year;
            if (Bday > now.AddYears(-age))
                age--;

            if (age < 18)
            {
                bool validate = false;
                var MotherFirstName = type.GetProperty("MothersFirstName").GetValue(instance, null);
                var MotherSurName = type.GetProperty("MothersSurname").GetValue(instance, null);
                //var MotherNRC = type.GetProperty("MothersNRC").GetValue(instance, null);
                //var MotherNAPSA = type.GetProperty("MotherNAPSANumber").GetValue(instance, null);
                var MotherNationality = type.GetProperty("MotherNationality").GetValue(instance, null);

                var FatherFirstName = type.GetProperty("FathersFirstName").GetValue(instance, null);
                var FatherSurName = type.GetProperty("FathersSurname").GetValue(instance, null);
                //var FatherNRC = type.GetProperty("FathersNRC").GetValue(instance, null);
                //var FatherNAPSA = type.GetProperty("FatherNAPSANumber").GetValue(instance, null);
                var FatherNationality = type.GetProperty("FatherNationality").GetValue(instance, null);

                var GuardianFirstName = type.GetProperty("GuardiansFirstName").GetValue(instance, null);
                var GuardianSurName = type.GetProperty("GuardiansSurname").GetValue(instance, null);
                var GuardianNRC = type.GetProperty("GuardiansNRC").GetValue(instance, null);
                var GuardianNAPSA = type.GetProperty("GuardianNAPSANumber").GetValue(instance, null);
                var GuardianNationality = type.GetProperty("GuardianNationality").GetValue(instance, null);
                var GuardianRelationship = type.GetProperty("GuardianRelationship").GetValue(instance, null);

                if (MotherFirstName == null || MotherSurName == null || MotherNationality == null)
                    validate = false;
                else
                    return ValidationResult.Success;

                if (FatherFirstName == null || FatherSurName == null || FatherNationality == null)
                    validate = false;
                else
                    return ValidationResult.Success;

                if (GuardianFirstName == null || GuardianSurName == null || GuardianNationality == null || GuardianRelationship == null)
                    validate = false;
                else
                    return ValidationResult.Success;

                return new ValidationResult("Atleast Provide Mother Father Or Guardians complete detail because client is below 18");
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}