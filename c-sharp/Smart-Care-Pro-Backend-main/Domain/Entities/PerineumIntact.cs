using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

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
   /// PerineumIntact entity.
   /// </summary>
   public class PerineumIntact : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table PeriuneumIntacts.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Is periuneum intact or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is periuneum intact")]
      public bool IsPerineumIntact { get; set; }

      /// <summary>
      /// Tear details of PeriuneumIntacts.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Tear details")]
      [StringLength(90)]
      public string TearDetails { get; set; }

      /// <summary>
      /// Mother delivery comment of PeriuneumIntacts.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Mother delivery comment")]
      [StringLength(250)]
      public string MotherDeliveryComment { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table PeriuneumIntacts.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid DeliveryId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DeliveryId")]
      [JsonIgnore]
      public virtual MotherDeliverySummary MotherDeliverySummary { get; set; }
   }
}