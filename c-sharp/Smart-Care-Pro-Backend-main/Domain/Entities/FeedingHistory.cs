using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 27.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Contains information of feeding histories of a client.
   /// </summary>
   public class FeedingHistory : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table FeedingHistories.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Feeding code of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Feeding code")]
      public FeedingCode FeedingCode { get; set; }

      /// <summary>
      /// Other feeding code of the client.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Other feeding code")]
      public string OtherFeedingCode { get; set; }

      /// <summary>
      /// Comments of the client.
      /// </summary>
      [Display(Name = "Comments")]
      public string Comments { get; set; }

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