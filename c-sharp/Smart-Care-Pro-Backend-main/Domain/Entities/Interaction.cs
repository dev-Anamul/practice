using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
* Created by   : Stephan
* Date created : 24.12.2022
* Modified by  : Lion
* Last modified: 24.12.2022
* Reviewed by  :
* Date reviewed:
*/
namespace Domain.Entities
{
   /// <summary>
   /// Interactions entity.
   /// </summary>
   public class Interaction : BaseModel
   {
      /// <summary>
      /// Primary key of the table Interactions.
      /// </summary>
      [Key]
      public Guid Oid { get; set; }

      /// <summary>
      /// Service code of the modules.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Service Code")]
      [StringLength(30)]
      public string ServiceCode { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table OPDVisits.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid EncounterId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("EncounterId")]
      [JsonIgnore]
      public virtual Encounter Encounter { get; set; }
   }
}