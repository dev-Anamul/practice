using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
* Created by   : Stephan
* Date created : 24.12.2022
* Modified by  : Lion
* Last modified: 18.01.2023
* Reviewed by  :
* Date reviewed:
*/
namespace Domain.Entities
{
   /// <summary>
   /// ImmunizationRecord entity.
   /// </summary>
   public class ImmunizationRecord : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ImmunizationRecords.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Date of vaccine is given to the client.
      /// </summary>        
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Date Given")]
      public DateTime DateGiven { get; set; }

      /// <summary>
      /// Other vaccine name.
      /// </summary>
      [StringLength(100)]
      [Display(Name = "Batch Number")]
      public string BatchNumber { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table VaccineDoses.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int DoseId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DoseId")]
      [JsonIgnore]
      public virtual VaccineDose VaccineDose { get; set; }

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

      /// <summary>
      /// Types of vaccine.
      /// </summary>
      [NotMapped]
      public string VaccineTypes { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<AdverseEvent> AdverseEvents { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<CovaxRecord> CovaxRecords { get; set; }

      /// <summary>
      /// List of immunization record.
      /// </summary>
      [NotMapped]
      public List<ImmunizationRecord> ImmunizationRecordList { get; set; }
   }
}