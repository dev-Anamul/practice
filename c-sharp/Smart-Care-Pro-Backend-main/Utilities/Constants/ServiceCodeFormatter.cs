using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 23.10.2022
 * Last modified: Brian
 * Modified by  : 14.01.2023
 * Reviewed by  :
 * Date reviewed: 
 */
namespace Utilities.Constants
{
    public class ServiceCodeFormatter
    {
        public static string FormatServiceCode(StatusCode statusCode, EncounterType encounterType)
        {
            // Convert StatusCode to a string and format it
            string formattedStatusCode = ((int)statusCode).ToString("D4");

            // Convert EncounterType to an integer and format it to be a 4-digit string
            string formattedEncounterType = ((int)encounterType).ToString("D4");

            // Combine the formatted values with a hyphen
            return formattedStatusCode + "-" + formattedEncounterType;
        }
    }
}
