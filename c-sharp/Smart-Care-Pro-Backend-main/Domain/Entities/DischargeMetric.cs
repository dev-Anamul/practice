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
   /// FeedingMethods entity.
   /// </summary>
   public class DischargeMetric : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table DischargeMetrics.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Apgar score of the table DischargeMetrics.
      /// </summary>
      [Display(Name = "Apgar score")]
      public int? ApgarScore { get; set; }

      /// <summary>
      /// Body length of the table DischargeMetrics.
      /// </summary>
      [Display(Name = "Body length")]
      public int? BodyLength { get; set; }

      /// <summary>
      /// Head circumference of the table DischargeMetrics.
      /// </summary>
      [Display(Name = "Head circumference")]
      public int HeadCircumference { get; set; }

      /// <summary>
      /// Chest circumference of the table DischargeMetrics.
      /// </summary>
      [Display(Name = "Chest circumference")]
      public int ChestCircumference { get; set; }

      /// <summary>
      /// Perinatal Problems of the table DischargeMetrics.
      /// </summary>
      [Display(Name = "Perinatal problems")]
      public PerinatalProblems? PerinatalProblems { get; set; }

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