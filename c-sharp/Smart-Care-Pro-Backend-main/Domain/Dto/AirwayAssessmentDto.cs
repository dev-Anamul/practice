using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
   public class AirwayAssessmentDto : BaseModel
   {
      public Guid Oid { get; set; }

      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Has dentures")]
      public bool HasDentures { get; set; }

      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Has loose teeth")]
      public bool HasLooseTeeth { get; set; }

      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Has abnormalities of the neck")]
      public bool HasAbnormalitiesOfTheNeck { get; set; }

      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Tongue size")]
      public TongueSize TongueSize { get; set; }

      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Mandible size")]
      public MandibleSize MandibleSize { get; set; }
   }
}