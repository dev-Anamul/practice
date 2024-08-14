using Domain.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 11.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// PMTCT entity.
   /// </summary>
   public class PMTCT : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table PMTCT.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Has mother taken ARV or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Has mother taken ARVs ? ")]
      public YesNoUnknown HasMotherTakenARV { get; set; }

      /// <summary>
      /// Other(specify) ARV for mothar.
      /// </summary>
      [StringLength(200)]
      [Display(Name = "Other(specify)")]
      public string OtherARVForMother { get; set; }

      /// <summary>
      /// When mothar taken ARV.
      /// </summary>
      [Display(Name = "When taken?")]
      public WhenMotherTakenARV? WhenMotherTakenARV { get; set; }

      /// <summary>
      /// Mother's ARV Start date.
      /// </summary>
      /// 
      [ARVDateValodator]
      [Display(Name = "ARV Start Date For Mother")]
      public DateTime? ARVStartDateForMother { get; set; }

      /// <summary>
      /// Mother's ARV end date.
      /// </summary>
      [Display(Name = "ARV End Date For Mother")]
      public DateTime? ARVEndDateForMother { get; set; }

      /// <summary>
      /// How lon mother taken ARV.
      /// </summary>
      [Display(Name = "How long taken ? ")]
      public int? HowLongMotherTakenARV { get; set; }

      /// <summary>
      /// Duration unit for mother taken ARV.
      /// </summary>
      [Display(Name = "Duration unit for mother")]
      public Duration? DurationUnitForMother { get; set; }

      /// <summary>
      /// Duration unit for mother taken ARV.
      /// </summary>
      [StringLength(200)]
      [Display(Name = "What ARVs mother ahs taken?")]
      public string WhatARVMotherWasTaken { get; set; }

      /// <summary>
      /// Has child taken ARV.
      /// </summary>
      [Display(Name = "Has child taken ARVs ?")]
      public YesNoUnknown HasChildTakenARV { get; set; }

      /// <summary>
      /// Other ARV for child.
      /// </summary>
      [StringLength(200)]
      [Display(Name = "Other(specify)")]
      public string OtherARVForChild { get; set; }

      /// <summary>
      /// How long child taken ARV.
      /// </summary>
      [Display(Name = "How long taken ? ")]
      public HowLongChildTakenARV? HowLongChildTakenARV { get; set; }

      /// <summary>
      /// ARV start date for child.
      /// </summary>
      /// 
      [ARVDateValodator]
      [Display(Name = "START DATE CALENDAR")]
      public DateTime? ARVStartDateForChild { get; set; }

      /// <summary>
      /// ARV end date for child.
      /// </summary>
      [Display(Name = "END DATE CALENDAR")]
      public DateTime? ARVEndDateForChild { get; set; }

      /// <summary>
      /// What ARV child was taken.
      /// </summary>
      [StringLength(300)]
      [Display(Name = "What ARVs child ahs taken?")]
      public string WhatARVChildWasTaken { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Clients.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }
   }
}