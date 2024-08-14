using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 08.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// OptedCircumcisionReasons entity.
   /// </summary>
   public class OptedCircumcisionReason : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table OptedCircumcisionReasons.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table CircumcisionReasons.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int CircumcisionReasonId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("CircumcisionReasonId")]
      [JsonIgnore]
      public virtual CircumcisionReason CircumcisionReason { get; set; }

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
   }
}