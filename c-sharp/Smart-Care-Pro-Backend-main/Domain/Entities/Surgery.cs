using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 06.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Surgery entity.
   /// </summary>
   public class Surgery : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Surgery.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// BookingDate of Surgery.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [DataType(DataType.Date)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Booking Date")]
      public DateTime BookingDate { get; set; }

      /// <summary>
      /// BookingTime of Surgery.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "Booking Time")]
      public TimeSpan BookingTime { get; set; }

      /// <summary>
      /// OperationDate of Surgery.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [DataType(DataType.Date)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Operation Date")]
      public DateTime OperationDate { get; set; }

      /// <summary>
      /// OperationTime of Surgery.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "Operation Time")]
      public TimeSpan OperationTime { get; set; }

      /// <summary>
      /// OperationType of Surgery.
      /// </summary>        
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Operation Type")]
      public OperationType OperationType { get; set; }

      /// <summary>
      /// Surgons of the Surgery.
      /// </summary> 
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(500)]
      [Display(Name = "Surgeons")]
      public string Surgeons { get; set; }

      /// <summary>
      /// BookingNote of the Surgery.
      /// </summary> 
      [StringLength(500)]
      [Display(Name = "Booking Note")]
      public string BookingNote { get; set; }

      /// <summary>
      /// TimePatientWheeledTheater of the Surgery.
      /// </summary> 
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "Time Patient Wheeled Theater")]
      public TimeSpan TimePatientWheeledTheater { get; set; }

      /// <summary>
      /// NursingPreOpPlan of the Surgery.
      /// </summary> 
      [Display(Name = "Nursing Pre. Op. Plan")]
      public string NursingPreOpPlan { get; set; }

      /// <summary>
      /// SurgicalCheckList of the Surgery.
      /// </summary> 
      [Display(Name = "Surgical Check List")]
      public string SurgicalCheckList { get; set; }

      /// <summary>
      /// Team of the Surgery.
      /// </summary> 
      [Display(Name = "Team")]
      public string Team { get; set; }

      /// <summary>
      /// OperationStartTime of Surgery.
      /// </summary>
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "Operation Start Time ")]
      public TimeSpan OperationStartTime { get; set; }

      /// <summary>
      /// OperationEndTime of Surgery.
      /// </summary>
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "Operation End Time ")]
      public TimeSpan OperationEndTime { get; set; }

      /// <summary>
      /// SurgeryIndication of the Surgery.
      /// </summary> 
      [StringLength(200)]
      [Display(Name = "Surgery Indication")]
      public string SurgeryIndication { get; set; }

      /// <summary>
      /// OperationName of the Surgery.
      /// </summary> 
      [StringLength(200)]
      [Display(Name = "Operation Name")]
      public string OperationName { get; set; }

      /// <summary>
      /// PostOpProcedure of the Surgery.
      /// </summary> 
      [Display(Name = "Post Op. Procedure")]
      public string PostOpProcedure { get; set; }

      /// <summary>
      /// Procedure type of the Surgery.
      /// </summary> 
      [Display(Name = "Procedure type")]
      public ProcedureType ProcedureType { get; set; }

      /// <summary>
      /// Device type of the Surgery.
      /// </summary> 
      [StringLength(30)]
      [Display(Name = "Procedure type")]
      public string DeviceType { get; set; }

      /// <summary>
      /// Suture type of the Surgery.
      /// </summary> 
      [Display(Name = "Suture type")]
      public SutureType SutureType { get; set; }

      /// <summary>
      /// Device type of the Surgery.
      /// </summary> 
      [StringLength(30)]
      [Display(Name = "Other")]
      public string Other { get; set; }

      /// <summary>
      /// Whether the surgery is VMMC or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is VMMC Surgery?")]
      public bool IsVMMCSurgery { get; set; }

      /// <summary>
      /// WardID of the Surgery.
      /// </summary> 
      public int? WardId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("WardId")]
      [JsonIgnore]
      public virtual Ward Ward { get; set; }

      /// <summary>
      /// ClientID of the Surgery.
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
      /// Diagnosis of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Diagnosis> Diagnoses { get; set; }

      /// <summary>
      /// Treatment plans of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<TreatmentPlan> TreatmentPlans { get; set; }

      /// <summary>
      /// Anesthetic Plans of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<AnestheticPlan> AnestheticPlans { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [NotMapped]
      public Guid[] DiagnosisList { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [NotMapped]
      public string TreatmentNote { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [NotMapped]
      public string DiagnosisName { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [NotMapped]
      public string BookingTimeStr { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [NotMapped]
      public string OperationTimeStr { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [NotMapped]
      public string TimePatientWheeledTheaterStr { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [NotMapped]
      public string OperationStartTimeStr { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [NotMapped]
      public string OperationEndTimeStr { get; set; }
   }
}