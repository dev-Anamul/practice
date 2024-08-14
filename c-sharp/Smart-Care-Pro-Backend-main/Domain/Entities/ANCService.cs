using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 17.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ANCService entity.
   /// </summary>
   public class ANCService : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ANCServices.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Motherhood number for ANCServices.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Motherhood number")]
      public string MotherhoodNumber { get; set; }

      /// <summary>
      /// Gravida for ANCServices.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int Gravida { get; set; }

      /// <summary>
      /// Para for ANCServices.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int Parity { get; set; }

      /// <summary>
      /// Has current pregnency concluded or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Has current pregnency concluded")]
      public bool HasCurrentPregnancyConcluded { get; set; }

      /// <summary>
      /// Pregnency conclided reason if any.
      /// </summary>
      [Display(Name = "Pregnency conclided reason")]
      public PregnancyConcludedReason? PregnancyConcludedReason { get; set; }

      /// <summary>
      /// Other reason for ANCServices.
      /// </summary>
      [Display(Name = "Other reason")]
      public string OtherReason { get; set; }

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
   }
}