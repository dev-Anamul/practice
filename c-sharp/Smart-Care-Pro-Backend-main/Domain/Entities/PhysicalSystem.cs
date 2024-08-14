using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 24.12.2022
 * Modified by  : Brian
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// PhysicalSystem entity.
   /// </summary>
   public class PhysicalSystem : BaseModel
   {
      /// <summary>
      /// Primary key of the table PhysicalSystems.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Name of a physical system.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "System")]
      public string Description { get; set; }

      /// <summary>
      /// Navigation Property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<SystemReview> SystemReviews { get; set; }

      ///// <summary>
      ///// Navigation Property.
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<SystemExamination> SystemExaminations { get; set; }

      // <summary>
      /// Navigation Property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<SystemRelevance> SystemRelevances { get; set; }
   }
}