using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 08.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// DotCalender entity.
   /// </summary>
   public class DotCalendar : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table DotCalenders.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Calender Date of the Dot calender.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Calendar Date")]
      public DateTime CalendarDate { get; set; }

      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Feedback")]
      public Feedback Feedback { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Dots.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.Town)]
      public Guid DotId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DotId")]
      [JsonIgnore]
      public virtual Dot Dot { get; set; }
   }

   

}