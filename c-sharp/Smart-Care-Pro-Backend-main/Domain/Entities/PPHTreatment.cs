using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 30.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// PPHTreatments entity.
   /// </summary>
   public class PPHTreatment : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table PPHTreatments.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// PPH sugery by for PPHTreatment.
      /// </summary>
      [Display(Name = "PPH sugery")]
      public DeliveredBy? PPHSugery { get; set; }

      /// <summary>
      /// Fluids given by for PPHTreatment.
      /// </summary>
      [Display(Name = "Fluids given")]
      public DeliveredBy? FluidsGiven { get; set; }

      /// <summary>
      /// Fluid amount for PPHTreatment.
      /// </summary>
      [Display(Name = "Fluid amount")]
      public int FluidAmount { get; set; }

      /// <summary>
      /// Blood type by for PPHTreatment.
      /// </summary>
      [Display(Name = "Blood type")]
      public DeliveredBy BloodType { get; set; }

      /// <summary>
      /// Blood amount for PPHTreatment.
      /// </summary>
      [Display(Name = "Blood amount")]
      public int BloodAmount { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table PPHTreatments.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid DeliveryId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DeliveryId")]
      [JsonIgnore]
      public virtual MotherDeliverySummary MotherDeliverySummary { get; set; }

      /// <summary>
      /// IdentifiedPPHTreatment of the PPHTreatment.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedPPHTreatment> IdentifiedPPHTreatments { get; set; }

      /// <summary>
      /// List of  assessments of the client.
      /// </summary>
      [NotMapped]
      public TreatmentsOfPPH[] TreatmentsOfPPHList { get; set; }
   }
}