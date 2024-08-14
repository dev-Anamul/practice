using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 29.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// 
   /// </summary>
   public class CounsellingService : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table CounsellingServices.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Type of the Counselling.
      /// </summary>
      [Display(Name = "Counselling Type")]
      public CounsellingType? CounsellingType { get; set; }

      /// <summary>
      /// Type of the Other Counselling.
      /// </summary>
      [StringLength(60)]
      [Display(Name = "Other Counselling Type")]
      public string OtherCounsellingType { get; set; }

      /// <summary>
      /// Session Number Counselling Services.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Session Number")]
      [Range(1, 99999999, ErrorMessage = "Please input a number")]
      public int SessionNumber { get; set; }

      /// <summary>
      /// Session Date of Counselling Services.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Session Date")]
      public DateTime SessionDate { get; set; }

      /// <summary>
      /// Session Note of Counselling Services.
      /// </summary>
      [Display(Name = "Session Note")]
      public string SessionNote { get; set; }

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