using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// LoginHistory entity.
   /// </summary>

   public class LoginHistory : BaseModel
   {
      //// <summary>
      /// Primary Key of the table LoginHistories.
      /// </summary>
      [Key]
      public Guid Oid { get; set; }

      /// <summary>
      /// Login date of the user.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Login date")]
      [IfFutureDate]
      public DateTime DateLogin { get; set; }

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
   }
}