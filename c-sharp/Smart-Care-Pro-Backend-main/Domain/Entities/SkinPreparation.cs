using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 13.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// SkinPreparation entity.
   /// </summary>
   public class SkinPreparation : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table SkinPreparations.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Type of anaesthesia.
      /// </summary>
      [Display(Name = "Type of anaesthesia")]
      public TypeofAnesthesia TypeofAnesthesia { get; set; }

      /// <summary>
      /// Whether Povidone Iodine is used or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is Povidone Iodine used?")]
      public bool IsPovidoneIodineUsed { get; set; }

      /// <summary>
      /// Regional type..
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Regional type")]
      public RegionalType RegionalType { get; set; }

      /// <summary>
      /// Local medicine type.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Local medicine type")]
      public LocalMedicineType LocalMedicineType { get; set; }

      /// <summary>
      /// Local medicine type.
      /// </summary>
      [Display(Name = "Level Of anesthesia")]
      public LevelOfAnesthesia LevelOfAnesthesia { get; set; }

      /// <summary>
      /// Medicine unit of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(20)]
      [Display(Name = "Medicine unit")]
      public string MedicineUnit { get; set; }

      /// <summary>
      /// Note.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Note")]
      public string Note { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table AnestheticPlans.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid AnestheticPlanId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("AnestheticPlanId")]
      [JsonIgnore]
      public virtual AnestheticPlan AnestheticPlan { get; set; }
   }
}