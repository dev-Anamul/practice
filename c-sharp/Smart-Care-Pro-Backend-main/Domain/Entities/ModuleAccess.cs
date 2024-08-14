using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 18.12.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ModuleAccess entity.
   /// </summary>
   public class ModuleAccess : BaseModel
   {
      /// <summary>
      /// Primary Key of the table ModuleAccesses.
      /// </summary>
      [Key]
      public Guid Oid { get; set; }

      /// <summary>
      /// Code of the modules.
      /// </summary>
      public int? ModuleCode { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table FacilityAccesses.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid FacilityAccessId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("FacilityAccessId")]
      [JsonIgnore]
      public virtual FacilityAccess FacilityAccess { get; set; }
   }
}