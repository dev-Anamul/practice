using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class IdentifiedAllergiesDto : EncounterBaseModel
    {

        /// <summary>
        /// Primary key of the table IdentifiedAllergies.
        /// </summary>
        [Key]
        public Guid InteractionID { get; set; }

        /// <summary>
        /// Severity of the Identified Allergy.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Severity")]
        public Severity Severity { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Allergy.
        /// </summary> 
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public int AllergyID { get; set; }
        
        /// <summary>
        /// Foreign key. Primary key of the table AllergicDrug.
        /// </summary> 
        public int? AllergicDrugID { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table Clients.
        /// </summary>
        public Guid ClientID { get; set; }

    }
}