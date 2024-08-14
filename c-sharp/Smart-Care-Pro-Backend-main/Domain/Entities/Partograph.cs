using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

/*
 * Created by   : Bella
 * Date created : 12.02.2023
 * Modified by  : Brian
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// The Partograph table holds  the Partograph detail of the patients
   /// </summary>
   public class Partograph : BaseModel
   {
      /// <summary>
      /// Primary key of the table Partograph.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Gravida mesurement of the patients.
      /// </summary>
      [Required(ErrorMessage = "Gravida is required!")]
      [Display(Name = "Gravida")]
      public byte Gravida { get; set; }

      /// <summary>
      /// Parity mesurement of the patients.
      /// </summary>
      [Required(ErrorMessage = "Parity is required!")]
      [Display(Name = "Parity")]
      public byte? Parity { get; set; }

      /// <summary>
      /// SB or NND System of a patients.
      /// </summary>       
      [StringLength(100)]
      [Display(Name = "SBOrNND")]
      public string SBOrNND { get; set; }

      /// <summary>
      /// Loss of pregnancy due to the premature exit of the products of conception
      /// </summary>
      [Display(Name = "Abortion")]
      public int? Abortion { get; set; }

      /// <summary>
      /// Estimated date of delivery of the patients.
      /// </summary>
      [Display(Name = "EDD")]
      public DateTime? EDD { get; set; }

      /// <summary>
      /// Mesurement of height.
      /// </summary>       
      [StringLength(100)]
      [Display(Name = "Borderline risk factors")]
      public string BorderlineRiskFactors { get; set; }

      /// <summary>
      /// Regular contractions started time.
      /// </summary>      
      [Display(Name = "Height")]
      [Column(TypeName = "decimal(18,2)")]
      public decimal? Height { get; set; }

      /// <summary>
      ///Regular contractions started time.
      /// </summary>
      [Required(ErrorMessage = "The Regular Contractions is required!")]
      [Display(Name = "Regular Contractions")]
      public DateTime RegularContractions { get; set; }

      [Required(ErrorMessage = "Partograph initiated date is required!")]
      [Display(Name = "Initiate Date")]
      public DateTime InitiateDate { get; set; }

      [Required(ErrorMessage = "Partograph initiated time is required!")]
      [StringLength(10)]
      [Display(Name = "Initiate Time")]
      public string InitiateTime { get; set; }

      /// <summary>
      ///During pregnancy to describe a rupture of the amniotic sac.
      /// </summary>       
      [Display(Name = "Membranes Ruptured")]
      public DateTime? MembranesRuptured { get; set; }

      /// <summary>
      /// Foreign Key, Primary Key of the Admissions. 
      /// </summary>
      public Guid AdmissionId { get; set; }

      /// <summary>
      /// Foreign Key, Primary Key of the Encounters. 
      /// </summary>
      public Guid EncounterId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("EncounterId")]
      [JsonIgnore]
      public virtual Encounter Encounters { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<FetalHeartRate> FetalHeartRates { get; set; }

      ///// <summary>
      ///// 
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Liquor> Liquors { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Moulding> Mouldings { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Cervix> Cervixes { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<DescentOfHead> DescentOfHeads { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Contraction> Contractions { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Oxytocin> Oxytocins { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Drop> Drops { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Medicine> Medicines { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<BloodPressure> BloodPressures { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Pulse> Pulses { get; set; }

      ///// <summary>
      ///// 
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Temperature> Temperatures { get; set; }

      ///// <summary>
      ///// 
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Protein> Proteins { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Acetone> Acetones { get; set; }

      ///// <summary>
      ///// 
      ///// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Volume> Volumes { get; set; }

      /// <summary>
      /// 
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<PartographDetail> PartographDetails { get; set; }
   }
}