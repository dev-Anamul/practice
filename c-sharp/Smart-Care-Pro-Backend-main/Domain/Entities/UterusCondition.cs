using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 30.04.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// UterusCondition entity.
   /// </summary>
   public class UterusCondition : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table UterusConditions.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Condition of uterus for UterusConditions.
      /// </summary>
      [Display(Name = "Condition of uterus")]
      public ConditionOfUterus? ConditionOfUterus { get; set; }

      /// <summary>
      /// Other conditions of uterus for UterusConditions.
      /// </summary>
      [Display(Name = "Other")]
      public string Other { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table UterusConditions.
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
      /// List of  assessments of the client.
      /// </summary>
      [NotMapped]
      public ConditionOfUterus[] ConditionOfUterusList { get; set; }
   }
}