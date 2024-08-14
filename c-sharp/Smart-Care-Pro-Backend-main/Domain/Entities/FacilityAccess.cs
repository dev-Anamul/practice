using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 12.12.2022
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains information of the access rights to different facilities of a user account.
   /// </summary>
   public class FacilityAccess : BaseModel
   {
      /// <summary>
      /// Primary key of the table FacilityAccesses.
      /// </summary>
      [Key]
      public Guid Oid { get; set; }

      /// <summary>
      /// The field stores the date when a user sends a request to get access to a facility.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Date requested")]
      public DateTime DateRequested { get; set; }

      /// <summary>
      /// The field stores the date when a user gets the approval of the request to get access to a facility.
      /// </summary>
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Date approved")]
      public DateTime? DateApproved { get; set; }

      /// <summary>
      /// Indicates whether a facility access is approved or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is approved")]
      public bool IsApproved { get; set; }

      /// <summary>
      /// The field indicates whether a facility access is ignored or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is ignored")]
      public bool IsIgnored { get; set; }

      /// <summary>
      /// The field indicates whether the password of a user account is forgot or not.
      /// </summary>
      [Display(Name = "Forgot password")]
      public bool ForgotPassword { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table UserAccounts.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid UserAccountId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("UserAccountId")]
      [JsonIgnore]
      public virtual UserAccount UserAccount { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Facilities.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int FacilityId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("FacilityId")]
      [JsonIgnore]
      public virtual Facility Facility { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<ModuleAccess> ModuleAccesses { get; set; }
   }
}