using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 12.09.2022
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains details of the HIVTestingReason entity in the database.
   /// </summary>
   public class HIVTestingReason : BaseModel
   {
      /// <summary>
      /// Primary key of the table HIVTestingReasons.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Reasons for testing HIV.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "HIV testing reason")]
      public string Description { get; set; }

      /// <summary>
      /// HTS of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<HTS> HTS { get; set; }
   }
}