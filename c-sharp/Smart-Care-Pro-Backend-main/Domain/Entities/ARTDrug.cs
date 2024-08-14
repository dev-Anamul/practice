using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   ///ARTDrug entity.
   /// </summary>
   public class ARTDrug : BaseModel
   {
      /// <summary>
      /// Primary Key of the table ARTDrugs.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Drug name of the ART.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table ARTDrugClasses.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int ARTDrugClassId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ARTDrugClassId")]
      [JsonIgnore]
      public virtual ARTDrugClass ARTDrugClass { get; set; }

      /// <summary>
      /// TakenARTDrugs of the ART.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<TakenARTDrug> TakenARTDrugs { get; set; }
   }
}