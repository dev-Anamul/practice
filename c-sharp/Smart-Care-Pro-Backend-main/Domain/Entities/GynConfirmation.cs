using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Tomas
 * Date created : 05.03.2023
 * Modified by  : Bella  
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains information of GynConfirmation entity in the database.
   /// </summary>
   public class GynConfirmation : BaseModel
   {
      /// <summary>
      /// Primary key of the table GynConfirmation.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// This field indicates the description of the GynConfirmation.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      public string Description { get; set; }

      /// <summary>
      /// IdentifiedPregnancyConfirmations of the GynConfirmation.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedPregnancyConfirmation> IdentifiedPregnancyConfirmations { get; set; }
   }
}