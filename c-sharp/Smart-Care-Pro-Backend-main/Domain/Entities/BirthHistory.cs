using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by    : Tomas
 * Date created  : 26.12.2022
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Domain.Entities
{
   public class BirthHistory : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table BirthHistories.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Weight of the newly born child.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "Smallint")]
      [Display(Name = "Birth Weight")]
      public int BirthWeight { get; set; }

      /// <summary>
      /// Height of the newly born child.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Birth Height")]
      public int? BirthHeight { get; set; }

      /// <summary>
      /// Tetanus at birth is given or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Birth Outcome")]
      public BirthOutcome BirthOutcome { get; set; }

      /// <summary>
      /// Height of the newly born child.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Head Circumference")]
      public int? HeadCircumference { get; set; }

      /// <summary>
      /// Height of the newly born child.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Chest Circumference")]
      public int? ChestCircumference { get; set; }

      /// <summary>
      /// Tetanus at birth is given or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "General Condition")]
      public GeneralCondition GeneralCondition { get; set; }

      /// <summary>
      /// Height of the newly born child.
      /// </summary>
      [Display(Name = "Breast Feeding Well")]
      public bool IsBreastFeedingWell { get; set; }

      /// <summary>
      /// Tetanus at birth is given or not.
      /// </summary>
      [Display(Name = "Other Feeding Option")]
      public OtherFeedingOption? OtherFeedingOption { get; set; }

      /// <summary>
      /// DeliveryTime of birth.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "Delivery Time")]
      public TimeSpan DeliveryTime { get; set; }

      /// <summary>
      /// Other reason of the client for not testing HIV.
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Vaccination Outside")]
      public string VaccinationOutside { get; set; }

      /// <summary>
      /// Tetanus at birth is given or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Tetanus at Birth")]
      public YesNoUnknown TetanusAtBirth { get; set; }

      /// <summary>
      /// Note of newly born child's birth history.
      /// </summary>
      [StringLength(1000)]
      [Display(Name = "Note")]
      public string Note { get; set; }

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