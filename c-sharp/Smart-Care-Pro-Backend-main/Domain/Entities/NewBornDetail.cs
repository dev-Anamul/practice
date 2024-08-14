using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 12.08.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   public class NewBornDetail : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table NewBornDetails.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Date of delivery of the table NewBornDetails.
      /// </summary>
      [Display(Name = "Date of delivery")]
      public DateTime? DateOfDelivery { get; set; }

      /// <summary>
      /// Time of delivery of the table NewBornDetails.
      /// </summary>
      [DataType(DataType.Time)]
      [Column(TypeName = "time")]
      [Display(Name = "Time of delivery")]
      public TimeSpan? TimeOfDelivery { get; set; }

      /// <summary>
      /// Birth weight of the table NewBornDetails.
      /// </summary>
      [Display(Name = "Birth weight")]
      [Column(TypeName = "decimal(18,2)")]
      public decimal? BirthWeight { get; set; }

      /// <summary>
      /// Birth height of the table NewBornDetails.
      /// </summary>
      [Display(Name = "Birth height")]
      [Column(TypeName = "decimal(18,2)")]
      public decimal? BirthHeight { get; set; }

      /// <summary>
      /// Other information of the table NewBornDetails.
      /// </summary>
      [StringLength(90)]
      public string Other { get; set; }

      /// <summary>
      /// Delivered by of the table NewBornDetails.
      /// </summary>
      [Display(Name = "Delivered by")]
      [StringLength(90)]
      public string DeliveredBy { get; set; }

      /// <summary>
      /// Gender of the table NewBornDetails.
      /// </summary>
      public Sex? Gender { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table PresentingPart.
      /// </summary>
      public int? PresentingPartId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("PresentingPartId")]
      [JsonIgnore]
      public virtual PresentingPart PresentingPart { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Breech.
      /// </summary>
      public int? BreechId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("BreechId")]
      [JsonIgnore]
      public virtual Breech Breech { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table ModeOfDelivery.
      /// </summary>
      public int? ModeOfDeliveryId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ModeOfDeliveryId")]
      [JsonIgnore]
      public virtual ModeOfDelivery ModeOfDelivery { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table NeonatalBirthOutcome.
      /// </summary>
      public int? NeonatalBirthOutcomeId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("NeonatalBirthOutcomeId")]
      [JsonIgnore]
      public virtual NeonatalBirthOutcome NeonatalBirthOutcome { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table CauseOfStillBirth.
      /// </summary>
      public int? CauseOfStillbirthId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("CauseOfStillbirthId")]
      [JsonIgnore]
      public virtual CauseOfStillbirth CauseOfStillbirth { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table MotherDeliverySummaries.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid DeliveryId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DeliveryId")]
      [JsonIgnore]
      public virtual MotherDeliverySummary MotherDeliverySummary { get; set; }

      /// <summary>
      /// NeonatalDeath of the NewBornDetail.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<NeonatalDeath> NeonatalDeaths { get; set; }

      /// <summary>
      /// NeonatalAbnormalite of the NewBornDetail.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<NeonatalAbnormality> NeonatalAbnormalities { get; set; }

      /// <summary>
      /// NeonatalInjury of the NewBornDetail.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<NeonatalInjury> NeonatalInjuries { get; set; }

      /// <summary>
      /// ApgarScore of the NewBornDetail.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<ApgarScore> ApgarScores { get; set; }
   }
}