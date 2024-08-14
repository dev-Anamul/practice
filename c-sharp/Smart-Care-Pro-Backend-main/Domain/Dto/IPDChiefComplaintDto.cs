using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 15.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
   public class IPDChiefComplaintDto : EncounterBaseModel
   {
      public Guid ChiefComplaintID { get; set; }

      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "History summary")]
      public string HistorySummary { get; set; }

      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Examination summary")]
      public string ExaminationSummary { get; set; }

      public Guid EncounterID { get; set; }

      public Guid ClientID { get; set; }

      public Guid InteractionID { get; set; }
   }
}