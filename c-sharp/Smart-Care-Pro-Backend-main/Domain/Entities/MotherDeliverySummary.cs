using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 30.04.2023
 * Modified by  : 
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// MotherDeliverySummaries entity.
   /// </summary>
   public class MotherDeliverySummary : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ANCServices.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      ///Number of MotherDeliverySummary.
      /// </summary>
      public int? Number { get; set; }

      /// <summary>
      /// Birth type of MotherDeliverySummary.
      /// </summary>
      [Display(Name = "Birth type")]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public BirthType BirthType { get; set; }

      /// <summary>
      /// Gestational period of MotherDeliverySummary.
      /// </summary>
      [Display(Name = "Gestational period")]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int GestationalPeriod { get; set; }

      /// <summary>
      /// Delivered type of MotherDeliverySummary.
      /// </summary>
      [Display(Name = "Delivered type")]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public DeliveredType DeliveredType { get; set; }

      /// <summary>
      /// Labor duration of MotherDeliverySummary.
      /// </summary>
      [Display(Name = "Labor duration")]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int LaborDuration { get; set; }

      /// <summary>
      /// Delivery location of MotherDeliverySummary.
      /// </summary>
      [Display(Name = "Delivery location")]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public DeliveryLocation DeliveryLocation { get; set; }

      /// <summary>
      /// Duration of rupture of MotherDeliverySummary.
      /// </summary>
      [Display(Name = "Duration of rupture")]
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int DurationOfRupture { get; set; }

      /// <summary>
      ///Delivered by name of MotherDeliverySummary.
      /// </summary>
      [Display(Name = "Delivered by name")]
      [StringLength(90)]
      public string DeliveredByName { get; set; }

      /// <summary>
      ///Delivered by for MotherDeliverySummary.
      /// </summary>
      [Display(Name = "Delivered by")]
      public DeliveredBy? DeliveredBy { get; set; }

      /// <summary>
      ///Delivered by name of MotherDeliverySummary.
      /// </summary>
      [StringLength(90)]
      public string Other { get; set; }

      /// <summary>
      ///Vaginal washes of MotherDeliverySummary.
      /// </summary>
      [Display(Name = "Vaginal washes")]
      public int? VaginalWashes { get; set; }

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
      /// IdentifiedDeliveryIntervention of the MotherDeliverySummaries.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedDeliveryIntervention> IdentifiedDeliveryInterventions { get; set; }

      /// <summary>
      /// IdentifiedCurrentDeliveryComplication of the MotherDeliverySummaries.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedCurrentDeliveryComplication> IdentifiedCurrentDeliveryComplications { get; set; }

      /// <summary>
      /// ThirdStageDelivery of the MotherDeliverySummaries.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<ThirdStageDelivery> ThirdStageDeliveries { get; set; }

      ///// <summary>
      ///// PPHTreatment of the MotherDeliverySummaries.
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<PPHTreatment> PPHTreatments { get; set; }

      /// <summary>
      /// MedicalTreatment of the MotherDeliverySummaries.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<MedicalTreatment> MedicalTreatments { get; set; }

      /// <summary>
      /// UterusCondition of the MotherDeliverySummaries.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<UterusCondition> UterusConditions { get; set; }

      ///// <summary>
      ///// PlacentaRemoval of the MotherDeliverySummaries.
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<PlacentaRemoval> PlacentaRemovals { get; set; }

      ///// <summary>
      ///// PeriuneumIntact of the MotherDeliverySummaries.
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<PerineumIntact> PerineumIntacts { get; set; }

      ///// <summary>
      ///// IdentifiedPeriuneumIntact of the MotherDeliverySummaries.
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<IdentifiedPerineumIntact> IdentifiedPerineumIntacts { get; set; }

      ///// <summary>
      ///// IdentifiedPeriuneumIntact of the MotherDeliverySummaries.
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<NewBornDetail> NewBornDetails { get; set; }

      /// <summary>
      /// List of  assessments of the client.
      /// </summary>
      [NotMapped]
      public Interventions[] InterventionsList { get; set; }

      /// <summary>
      /// List of  assessments of the client.
      /// </summary>
      [NotMapped]
      public DeliveryComplications[] DeliveryComplicationsList { get; set; }

      /// <summary>
      /// List of  assessments of the client.
      /// </summary>
      [NotMapped]
      public string OtherIntervention { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [NotMapped]
      public string OtherDeliveryComplication { get; set; }
   }
}