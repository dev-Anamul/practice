using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
* Created by   : Stephan
* Date created : 12.09.2022
* Modified by  : Stephan
* Last modified: 12.08.2023
* Reviewed by  :
* Date reviewed:
*/
namespace Domain.Entities
{
   /// <summary>
   /// Vital entity.
   /// </summary>
   public class Vital : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Vitals.
      /// </summary>
      [Key]
      public Guid Oid { get; set; }

      /// <summary>
      /// Weight of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "decimal(18,2)")]
      [Display(Name = "Weight")]
      public decimal Weight { get; set; }

      /// <summary>
      /// Height of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "decimal(18,2)")]
      [Display(Name = "Height")]
      public decimal Height { get; set; }

      /// <summary>
      /// Calculated BMI score of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "BMI Score")]
      public string BMI { get; set; }

      /// <summary>
      /// Systolic blood pressure of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Systolic")]
      public int Systolic { get; set; }

      /// <summary>
      /// If client's systolic is unrecordable.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Systolic if unrecordable")]
      public Unrecordable SystolicIfUnrecordable { get; set; }

      /// <summary>
      /// Diastolic blood pressure of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Diastolic")]
      public int Diastolic { get; set; }

      /// <summary>
      /// If client's diastolic is unrecordable.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Diastolic if unrecordable")]
      public Unrecordable DiastolicIfUnrecordable { get; set; }

      /// <summary>
      /// Temperature of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "decimal(18,2)")]
      [Display(Name = "Temperature")]
      public decimal Temperature { get; set; }

      /// <summary>
      /// Pulse of the client.
      /// </summary>
      [Display(Name = "Pulse Rate")]
      public int? PulseRate { get; set; }

      /// <summary>
      ///  Respiratory rate of the client.
      /// </summary>
      [Display(Name = "Respiratory Rate")]
      public int? RespiratoryRate { get; set; }

      /// <summary>
      /// Oxygen saturation of the client.
      /// </summary>
      [Display(Name = "Oxygen saturation")]
      public int? OxygenSaturation { get; set; }

      /// <summary>
      /// MUAC of the client.
      /// </summary>
      [Display(Name = "MUAC")]
      public decimal? MUAC { get; set; }

      /// <summary>
      /// Calculated MUAC score of the client.
      /// </summary>
      [Display(Name = "MUAC Score")]
      public string MUACScore { get; set; }

      /// <summary>
      /// Abdominal circumference of the client.
      /// </summary>
      [Column(TypeName = "decimal(18,2)")]
      [Display(Name = "Abdominal circumference")]
      public decimal? AbdominalCircumference { get; set; }

      /// <summary>
      /// Head circumference of the client.
      /// </summary>
      [Display(Name = "Head circumference")]
      public decimal? HeadCircumference { get; set; }

      /// <summary>
      /// Calculated HC score of the client.
      /// </summary>
      [Display(Name = "HC score")]
      public string HCScore { get; set; }

      /// <summary>
      /// Calculated RBS of the client.
      /// </summary>
      [Display(Name = "Random Blood Sugar")]
      public string RandomBloodSugar { get; set; }

      /// <summary>
      /// Comments the reason.
      /// </summary>
      [StringLength(250)]
      [Display(Name = "Comment")]
      public string Comment { get; set; }

      /// <summary>
      /// Date of Vitals.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DataType(DataType.DateTime)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Vitals Date")]
      public DateTime VitalsDate { get; set; }

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