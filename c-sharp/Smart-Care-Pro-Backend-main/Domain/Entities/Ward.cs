using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 29.01.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Ward entity.
   /// </summary>
   public class Ward : BaseModel
   {
      /// <summary>
      /// Primary key of the table Wards.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Ward name of the Ward.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Firm .
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int FirmId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("FirmId")]
      [JsonIgnore]
      public virtual Firm Firm { get; set; }

      [NotMapped]
      public int DepartmentId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Bed> Beds { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Surgery> Surgeries { get; set; }
   }
}