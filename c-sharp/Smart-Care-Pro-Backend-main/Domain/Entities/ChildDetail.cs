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
   /// ChildDetail entity.
   /// </summary>
   public class ChildDetail : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ChildDetails.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Birth outcome for table ChildDetails.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Birth outcome")]
      public ChildDetailBirthOutcome BirthOutcome { get; set; }

      /// <summary>
      /// Birth weight for table ChildDetails.
      /// </summary>
      [Display(Name = "Birth weight")]
      [Column(TypeName = "decimal(18,2)")]
      public decimal? BirthWeight { get; set; }

      /// <summary>
      /// Child name for table ChildDetails.
      /// </summary>
      [Display(Name = "Child name")]
      [StringLength(60)]
      public string ChildName { get; set; }

      /// <summary>
      /// Child sex for table ChildDetails.
      /// </summary>
      [Display(Name = "Child sex")]
      public Sex? ChildSex { get; set; }

      /// <summary>
      /// Last TC date for table ChildDetails.
      /// </summary>
      [Display(Name = "Last TC date")]
      public DateTime? LastTCDate { get; set; }

      /// <summary>
      /// Last TC result for table ChildDetails.
      /// </summary>
      [Display(Name = "Last TC result")]
      public PositiveNegative? LastTCResult { get; set; }

      /// <summary>
      /// Child care number for table ChildDetails.
      /// </summary>
      [Display(Name = "Child care number")]
      [StringLength(60)]
      public string ChildCareNumber { get; set; }

      /// <summary>
      /// Date of child turns 18 months for table ChildDetails.
      /// </summary>
      [Display(Name = "Date of child turns 18 months")]
      public DateTime? DateOfChildTurns18Months { get; set; }

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
      /// ChildDetail for the client side.
      /// </summary>
      [NotMapped]
      public List<ChildDetail> ChildDetails { get; set; }
   }
}