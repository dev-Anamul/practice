using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 17.04.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// VeginalPosition entity.
   /// </summary>
   public class VaginalPosition : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table VaginalPositions.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Position for table VaginalPositions.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Position Position { get; set; }

      /// <summary>
      /// Note for table VaginalPositions.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      public string Note { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Clients.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }
   }
}