using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 17.04.2023
 * Modified by  : Lion
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// MotherDetail entity.
   /// </summary>
   public class MotherDetail : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ANCServices.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Pregnancy no table MotherDetails.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Pregnancy no")]
      public int PregnancyNo { get; set; }

      /// <summary>
      /// Date of delivary table MotherDetails.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Date of delivary")]
      public DateTime DateofDelivary { get; set; }

      /// <summary>
      /// Metarnal outcome for table MotherDetails.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Metarnal outcome")]
      public MetarnalOutcome MetarnalOutcome { get; set; }

      /// <summary>
      /// Pregnancy conclusion for table MotherDetails.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Pregnancy conclusion")]
      public PregnancyConclusion PregnancyConclusion { get; set; }

      /// <summary>
      /// Early termination reason for table MotherDetails.
      /// </summary>
      [Display(Name = "Early termination reason")]
      public EarlyTerminationReason? EarlyTerminationReason { get; set; }

      /// <summary>
      /// Pregnancy duration for table MotherDetails.
      /// </summary>
      [Display(Name = "Pregnancy duration")]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int PregnancyDuration { get; set; }

      /// <summary>
      /// Pregnancy duration for table MotherDetails.
      /// </summary>
      [Display(Name = "Metarnal complication")]
      [StringLength(90)]
      public string MetarnalComplication { get; set; }

      /// <summary>
      /// Delivery method for table MotherDetails.
      /// </summary>
      [Display(Name = "Delivery method")]
      public DeliveryMethod DeliveryMethod { get; set; }

      /// <summary>
      /// Pueperium outcome for table MotherDetails.
      /// </summary>
      [Display(Name = "Pueperium outcome")]
      public PueperiumOutcome? PueperiumOutcome { get; set; }

      /// <summary>
      /// Pueperium outcome for table MotherDetails.
      /// </summary>
      public string Notes { get; set; }

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

      [NotMapped]
      public List<MotherDetail> MotherDetails { get; set; }
   }
}