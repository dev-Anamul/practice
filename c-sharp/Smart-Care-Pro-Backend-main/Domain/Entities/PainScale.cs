using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 06-02-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// PainScale entity.
   /// </summary>
   public class PainScale : BaseModel
   {
      /// <summary>
      /// Primary key of the table PainScale.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the pain.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(250)]
      [Display(Name = "Pain description")]
      public string Description { get; set; }

      ///// <summary>
      /////Pain Record Plan  of the client.
      ///// </summary
      [JsonIgnore]
      public virtual IEnumerable<PainRecord> PainRecords { get; set; }
   }
}