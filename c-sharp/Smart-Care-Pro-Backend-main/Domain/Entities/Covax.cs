using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Tomas
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Covax Entity
   /// </summary>
   public class Covax : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Covax.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Name of CovaxNumber.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [Display(Name = "Batch Number")]
      public string CovaxNumber { get; set; }

      /// <summary>
      /// Name of WasCovaxOffered.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool WasCovaxOffered { get; set; }

      /// <summary>
      /// Client Get Vaccinated Today or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool DoesClientGetVaccinatedToday { get; set; }

      /// <summary>
      /// Name of Client Refused For Vaccination or not.
      /// </summary>
      public ReasonClientRefusedForVaccination? ReasonClientRefusedForVaccination { get; set; }

      /// <summary>
      /// Other Reason Client Refused For Vaccination.
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Other Reason Client Refused For Vaccination")]
      public string OtherReasonClientRefusedForVaccination { get; set; }

      /// <summary>
      /// Name of IsPregnantOrLactating.
      /// </summary>
      public bool IsPregnantOrLactating { get; set; }

      /// <summary>
      /// Name of HasCancer.
      /// </summary>
      public bool HasCancer { get; set; }

      /// <summary>
      /// Name of HasDiabetes.
      /// </summary>
      public bool HasDiabetes { get; set; }

      /// <summary>
      /// Name of HasHeartDisease.
      /// </summary>
      public bool HasHeartDisease { get; set; }

      /// <summary>
      /// Name of HasHyperTension.
      /// </summary>
      public bool HasHyperTension { get; set; }

      /// <summary>
      /// Name of HasHIV.
      /// </summary>
      public bool HasHIV { get; set; }

      /// <summary>
      /// Name of Other Comorbidities.
      /// </summary>
      public string OtherComorbidities { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Clients.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<CovaxRecord> CovaxRecords { get; set; }
   }
}