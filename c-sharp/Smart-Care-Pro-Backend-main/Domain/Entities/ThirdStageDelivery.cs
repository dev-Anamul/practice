using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

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
   /// ThirdStageDeliveries entity.
   /// </summary>
   public class ThirdStageDelivery : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ThirdStageDeliveries.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Active management if any.
      /// </summary>
      [Display(Name = "Active management")]
      public bool ActiveManagement { get; set; }

      /// <summary>
      /// is PPH yes or no.
      /// </summary>
      public bool PPH { get; set; }

      /// <summary>
      /// Blood loss ThirdStageDelivery.
      /// </summary>
      [Display(Name = "Blood loss")]
      public int? BloodLoss { get; set; }

      /// <summary>
      /// Is uterine atony or not.
      /// </summary>
      [Display(Name = "Is uterine atony")]
      public bool IsUterineAtony { get; set; }

      /// <summary>
      /// Is uterine inversion or not.
      /// </summary>
      [Display(Name = "Is uterine inversion")]
      public bool IsUterineInversion { get; set; }

      /// <summary>
      /// Is abnormal placemental or not.
      /// </summary>
      [Display(Name = "Is abnormal placemental")]
      public bool IsAbnormalPlacemental { get; set; }

      /// <summary>
      /// Is birth trauma or not.
      /// </summary>
      [Display(Name = "Is birth trauma")]
      public bool IsBirthTrauma { get; set; }

      /// <summary>
      /// Is coagulation disorder or not.
      /// </summary>
      [Display(Name = "Is coagulation disorder")]
      public bool IsCoagulationDisorder { get; set; }

      /// <summary>
      /// Is others reason or not.
      /// </summary>
      public bool Others { get; set; }

      /// <summary>
      /// Is multiple pregnancy or not.
      /// </summary>
      [Display(Name = "Is multiple pregnancy")]
      public bool IsMultiplePregnancy { get; set; }

      /// <summary>
      /// Is prolonged oxytocin use or not.
      /// </summary>
      [Display(Name = "Is prolonged oxytocin use")]
      public bool IsProlongedOxytocinUse { get; set; }

      /// <summary>
      /// Is uterine leiomoma use or not.
      /// </summary>
      [Display(Name = "Is uterine leiomoma")]
      public bool IsUterineLeiomyoma { get; set; }

      /// <summary>
      /// Is anaesthesia use or not.
      /// </summary>
      [Display(Name = "Is anaesthesia")]
      public bool IsAnesthesia { get; set; }

      /// <summary>
      /// Is latrogenic injury or not.
      /// </summary>
      [Display(Name = "Is latrogenic injury")]
      public bool IsLatrogenicInjury { get; set; }

      /// <summary>
      /// Is uterine rapture or not.
      /// </summary>
      [Display(Name = "Is uterine rapture")]
      public bool IsUterineRapture { get; set; }

      /// <summary>
      /// Is fetal macrosomia or not.
      /// </summary>
      [Display(Name = "Is fetal macrosomia")]
      public bool IsFetalMacrosomia { get; set; }

      /// <summary>
      /// Is malpresentation of fetus or not.
      /// </summary>
      [Display(Name = "Is malpresentation of fetus")]
      public bool IsMalpresentationOfFetus { get; set; }

      /// <summary>
      /// Is retained placenta or not.
      /// </summary>
      [Display(Name = "Is retained placenta")]
      public bool IsRetainedPlacenta { get; set; }

      /// <summary>
      /// Is abnormal placentation or not.
      /// </summary>
      [Display(Name = "Is abnormal placentation")]
      public bool IsAbnormalPlacentation { get; set; }

      /// <summary>
      /// Is uncontrolled cord contraction or not.
      /// </summary>
      [Display(Name = "Is uncontrolled cord contraction")]
      public bool IsUncontrolledCordContraction { get; set; }

      /// <summary>
      /// Is previous uterine inversion contraction or not.
      /// </summary>
      [Display(Name = "Is previous uterine inversion")]
      public bool IsPreviousUterineInversion { get; set; }

      /// <summary>
      /// Is von wllebrand or not.
      /// </summary>
      [Display(Name = "Is von willebrand")]
      public bool IsVonWillebrand { get; set; }

      /// <summary>
      /// Is hemophilia or not.
      /// </summary>
      [Display(Name = "Is hemophilia")]
      public bool IsHemophilia { get; set; }

      /// <summary>
      /// Is valementous cord insertion or not.
      /// </summary>
      [Display(Name = "Is valementous cord insertion")]
      public bool IsVelamentousCordInsertion { get; set; }

      /// <summary>
      /// Is retained product of conception.
      /// </summary>
      [Display(Name = "Is retained product of conception")]
      public bool IsRetainedProductOfConception { get; set; }

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
   }
}