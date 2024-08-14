using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
* Created by   : Stephan
* Date created : 18.02.2023
* Modified by  : 
* Last modified: 
* Reviewed by  :
* Date reviewed:
*/
namespace Domain.Entities
{
   /// <summary>
   /// Covid entity.
   /// </summary>
   public class Covid : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Covid.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Source of Alert of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Source of Alert")]
      public SourceOfAlert SourceOfAlert { get; set; }

      /// <summary>
      /// Notification Date of Covid of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DataType(DataType.Date)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Notification Date")]
      public DateTime NotificationDate { get; set; }

      /// <summary>
      /// Client has Other Covid Symptoms or not.
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Other Covid Symptom")]
      public string OtherCovidSymptom { get; set; }

      /// <summary>
      /// Other Exposer risk or not.
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Other Exposer Risk")]
      public string OtherExposureRisk { get; set; }

      /// <summary>
      /// Client's Is ICUAdmitted or not. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is ICUAdmitted")]
      public bool IsICUAdmitted { get; set; }

      /// <summary>
      /// ICU Admission Date of Covid of the client.
      /// </summary>
      [DataType(DataType.Date)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "ICU Admission Date")]
      public DateTime? ICUAdmissionDate { get; set; }

      /// <summary>
      /// Client's Is On Oxygen or not. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is OnOxygen")]
      public bool IsOnOxygen { get; set; }

      /// <summary>
      /// Client's Is Oxygen Saturation or not. 
      /// </summary>
      [Display(Name = "Oxygen Saturation")]
      public int OxygenSaturation { get; set; }

      /// <summary>
      /// Client's Received BP Support or not. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Received BP Support")]
      public bool ReceivedBPSupport { get; set; }

      /// <summary>
      /// Client's Received Ventilatory Support or not. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Received Ventilatory Support")]
      public bool ReceivedVentilatorySupport { get; set; }

      /// <summary>
      /// Date First Positive Date of Covid of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DataType(DataType.Date)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Date First Positive")]
      public DateTime DateFirstPositive { get; set; }

      /// <summary>
      /// Any International Travel or not. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Any International Travel")]
      public bool AnyInternationalTravel { get; set; }

      /// <summary>
      /// Travel Destination of client. 
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Travel Destination")]
      public string TravelDestination { get; set; }

      /// <summary>
      /// Client's Is Client Health Care Worker or not. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is Client Health Care Worker")]
      public bool IsClientHealthCareWorker { get; set; }

      /// <summary>
      /// Client's Covid Exposer or not. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Had Covid Exposer")]
      public bool HadCovidExposure { get; set; }

      /// <summary>
      /// Client's Mental Status On Admission. 
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Mental Status On Admission")]
      public string MentalStatusOnAdmission { get; set; }

      /// <summary>
      /// Client's Has Pneumonia or not. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Has Pneumonia")]
      public bool HasPneumonia { get; set; }

      /// <summary>
      /// Is Patient Hospitalized or not. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Has Pneumonia")]
      public bool IsPatientHospitalized { get; set; }

      /// <summary>
      /// Hospitalized Date of Covid of the client.
      /// </summary>
      [DataType(DataType.Date)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Date Hospitalized")]
      public DateTime? DateHospitalized { get; set; }

      // <summary>
      /// Client's Is ARDS or not. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Is ARDS")]
      public bool IsARDS { get; set; }

      /// <summary>
      /// Clients Other Comorbidities Conditions
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Other Comorbidities Conditions")]
      public string OtherComorbiditiesConditions { get; set; }

      /// <summary>
      /// Clients Other Respiratory Illness
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Other Respiratory Illness")]
      public string OtherRespiratoryIllness { get; set; }

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
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<CovidSymptomScreening> CovidSymptomScreenings { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<CovidComorbidity> CovidComorbidities { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<ExposureRisk> ExposureRisks { get; set; }

      /// <summary>
      /// List of Covid Symptom Screening of the client.
      /// </summary>
      [NotMapped]
      public int[] CovidSymptomScreeningList { get; set; }

      /// <summary>
      /// List of Covid Comobidity List of the client.
      /// </summary>
      [NotMapped]
      public CovidComorbidityCondition[] CovidComobidityList { get; set; }

      /// <summary>
      /// List of ExposureRisks of the client.
      /// </summary>
      [NotMapped]
      public ExposureRisks[] ExposureRisksList { get; set; }
   }
}