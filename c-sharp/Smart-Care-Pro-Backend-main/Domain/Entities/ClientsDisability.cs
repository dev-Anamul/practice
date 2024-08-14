using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   public class ClientsDisability : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table WHOStagesConditions.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Other Disability of the Client.
      /// </summary>
      [StringLength(90)]
      public string OtherDisability { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Disabilities.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int DisabilityId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DisabilityId")]
      [JsonIgnore]
      public virtual Disability Disability { get; set; }

      /// <summary>
      /// Foreign Key of Client.
      /// </summary>
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }

      /// <summary>
      /// Disability list of the Client.
      /// </summary>
      //public virtual IEnumerable<Disability> Disabilities { get; set; }
      [NotMapped]
      public int[] DisabilityList { get; set; }
   }
}