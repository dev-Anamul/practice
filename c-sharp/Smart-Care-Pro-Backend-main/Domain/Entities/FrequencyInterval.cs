using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 12.03.2023
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Represents FrequencyInterval entity in the database.
   /// </summary>
   public class FrequencyInterval : BaseModel
   {
      /// <summary>
      /// Primary Key of the table FrequencyInterval.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// This field indicates the description of the Frequency Interval.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Time interval")]
      public string Description { get; set; }

      /// <summary>
      /// This field indicates the FrequencyType of the FrequencyInterval.
      /// </summary>
      public FrequencyType FrequencyType { get; set; }

      /// <summary>
      /// Medications of the FrequencyIntervals.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Medication> Medications { get; set; }
   }
}