using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 13.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Complication entity.
   /// </summary>
   public class Complication : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Complications.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Other complications of the client.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Other complications")]
      public string OtherComplications { get; set; }

      /// <summary>
      /// Severity of complications of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Severity")]
      public Severity Severity { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table VMMCServices.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid VMMCServiceId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("VMMCServiceId")]
      [JsonIgnore]
      public virtual VMMCService VMMCService { get; set; }

      /// <summary>
      /// Identify complications of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedComplication> IdentifiedComplications { get; set; }

      /// <summary>
      /// Identified complications to the client.
      /// </summary>
      [NotMapped]
      public int[] IdentifiedComplicationList { get; set; }
   }
}