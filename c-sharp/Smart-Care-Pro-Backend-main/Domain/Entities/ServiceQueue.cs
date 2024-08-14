using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   public class ServiceQueue : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ServiceQueues table.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Purpose for service queue.
      /// </summary>
      [StringLength(200)]
      public string Purpose { get; set; }

      /// <summary>
      ///Urgency Type ?
      /// </summary> 
      [Display(Name = "Urgency Type")]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public UrgencyType UrgencyType { get; set; }

      /// <summary>
      ///Service queued Date.
      /// </summary>
      [Display(Name = "Date queued")]
      [Column(TypeName = "smalldatetime")]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public DateTime DateQueued { get; set; }

      /// <summary>
      ///Is service queue completed or not ?
      /// </summary>
      [Display(Name = "Is completed")]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool IsCompleted { get; set; }

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

      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int FacilityQueueId { get; set; }

      [ForeignKey("FacilityQueueId")]
      [JsonIgnore]
      public FacilityQueue FacilityQueue { set; get; }

      [NotMapped]
      public string ClinicianName { get; set; }
   }
}