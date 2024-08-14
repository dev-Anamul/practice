using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 30.04.2023
 * Modified by  : Lion
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// NeonatalAbnormalites entity.
   /// </summary>
   public class NeonatalAbnormality : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table NeonatalAbnormalities.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Abnormalities of the table NeonatalAbnormality.
      /// </summary>
      public Abnormalities? Abnormalities { get; set; }

      /// <summary>
      /// Other information.
      /// </summary>
      [StringLength(90)]
      public string Other { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Neonatal.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid NeonatalId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("NeonatalId")]
      [JsonIgnore]
      public virtual NewBornDetail NewBornDetail { get; set; }

      /// <summary>
      /// List of  assessments of the client.
      /// </summary>
      [NotMapped]
      public Abnormalities[] AbnormalitiesList { get; set; }
   }
}