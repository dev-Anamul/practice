using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 24.12.2022
 * Modified by  : Tomas
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contraceptive entity.
   /// </summary>
   public class Contraceptive : BaseModel
   {
      /// <summary>
      /// Primary key of the table Contraceptives.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Name of the contraceptive.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// Contraceptive histories of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<ContraceptiveHistory> ContraceptiveHistories { get; set; }
   }
}