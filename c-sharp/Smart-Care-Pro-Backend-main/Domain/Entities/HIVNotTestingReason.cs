using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of the HIVNotTestingReason entity in the database.
   /// </summary>

   public class HIVNotTestingReason : BaseModel
   {
      /// <summary>
      /// Primary key of the table HIVNotTestingReasons.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// This field indicates the HIV not testing reason.
      /// </summary
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Reason for not testing HIV")]
      public string Description { get; set; }

      /// <summary>
      /// HTS of the client. 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<HTS> HTS { get; set; }
   }
}