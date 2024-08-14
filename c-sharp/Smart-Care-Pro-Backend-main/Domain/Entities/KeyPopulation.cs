using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// KeyPopulation entity.
   /// </summary>
   public class KeyPopulation : BaseModel
   {
      /// <summary>
      /// Primary key of the table KeyPopulations.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Key populations.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      ///// <summary>
      ///// Key Population Demographics of the Client.
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<KeyPopulationDemographic> KeyPopulationDemographics { get; set; }
   }
}