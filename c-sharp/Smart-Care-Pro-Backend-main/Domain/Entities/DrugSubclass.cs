using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 05.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// DrugSubclass entity.
   /// </summary>
   public class DrugSubclass : BaseModel
   {
      /// <summary>
      /// Primary Key of the table DrugSubclass.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Description of the DrugSubclass.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Drug Subclass")]
      [StringLength(90)]
      public string Description { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table DrugClass.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int DrugClassId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DrugClassId")]
      [JsonIgnore]
      public virtual DrugClass DrugClass { get; set; }

      /// <summary>
      /// Generic Drugs of Drug Sub Classes.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<GenericDrug> GenericDrugs { get; set; }
   }
}