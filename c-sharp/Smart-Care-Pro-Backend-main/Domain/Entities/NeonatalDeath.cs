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
   /// NeonatalDeaths entity.
   /// </summary>
   public class NeonatalDeath : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table NeonatalDeaths.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Time of death of the table NeonatalDeaths.
      /// </summary>        
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "Time of death")]
      public TimeSpan? TimeOfDeath { get; set; }

      /// <summary>
      /// Age at time of death of the table NeonatalDeaths.
      /// </summary>
      [Display(Name = "Age at time of death")]
      public int? AgeAtTimeOfDeath { get; set; }

      /// <summary>
      /// Time unit of the table NeonatalDeaths.
      /// </summary>
      [Display(Name = "Time unit")]
      public TimeUnit? TimeUnit { get; set; }

      /// <summary>
      /// Comments of the table NeonatalDeaths.
      /// </summary>
      [StringLength(250)]
      public string Comments { get; set; }

      /// <summary>
      /// Other information.
      /// </summary>
      [StringLength(90)]
      public string Other { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table CauseOfNeonatalDeath.
      /// </summary>
      public int? CauseOfNeonatalDeathId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("CauseOfNeonatalDeathId")]
      [JsonIgnore]
      public virtual CauseOfNeonatalDeath CauseOfNeonatalDeath { get; set; }

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
   }
}