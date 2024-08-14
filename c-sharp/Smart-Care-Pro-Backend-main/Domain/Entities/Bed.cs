using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Bed entity.
   /// </summary>
   public class Bed : BaseModel
   {
      /// <summary>
      /// Primary key of the table Bed.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Bed name of the Bed.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Ward .
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int WardId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("WardId")]
      [JsonIgnore]
      public virtual Ward Ward { get; set; }

      ///// <summary>
      ///// OPD visits of the client.
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Encounter> Encounters { get; set; }

      ///// <summary>
      ///// DepartmentId for client side.
      ///// </summary>
      [NotMapped]
      public int DepartmentId { get; set; }

      ///// <summary>
      ///// DepartmentId for client side.
      ///// </summary>
      [NotMapped]
      public int FirmId { get; set; }
   }
}