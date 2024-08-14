using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 25.12.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// BirthRecord entity.
   /// </summary>
   public class BirthRecord : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table BirthRecords.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Client's Birth record is given or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool IsBirthRecordGiven { get; set; }

      /// <summary>
      /// Client's under five card is given or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool IsUnderFiveCardGiven { get; set; }

      /// <summary>
      /// Under five card number of the client.
      /// </summary>
      [StringLength(20)]
      [Display(Name = "Under FiveCard Number")]
      public string UnderFiveCardNumber { get; set; }

      /// <summary>
      /// Origin of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Test result")]
      public Origin Origin { get; set; }

      /// <summary>
      /// First name of the informant.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [Display(Name = "Informant First Name")]
      public string InformantFirstName { get; set; }

      /// <summary>
      /// Surname of the informant.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [Display(Name = "Informant Surname")]
      public string InformantSurname { get; set; }

      /// <summary>
      /// Nickname of the informant.
      /// </summary>
      [StringLength(60)]
      [Display(Name = "Informant Nickname")]
      public string InformantNickname { get; set; }

      /// <summary>
      /// Informant's relationship with the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Informant Relationship")]
      public InformantRelationship InformantRelationship { get; set; }

      /// <summary>
      /// Other relationship of informant with the client.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Informant Other Relationship")]
      public string InformantOtherRelationship { get; set; }

      /// <summary>
      /// City of the informant.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Informant City")]
      public string InformantCity { get; set; }

      /// <summary>
      /// Street No. of the informant.
      /// </summary>
      [StringLength(30)]
      [Display(Name = "Informant Street No.")]
      public string InformantStreetNo { get; set; }

      /// <summary>
      /// POBox of the informant.
      /// </summary>
      [StringLength(30)]
      [Display(Name = "Informant POBox")]
      public string InformantPOBox { get; set; }

      /// <summary>
      /// Landmark of the informant.
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Informant Landmark")]
      public string InformantLandmark { get; set; }

      /// <summary>
      /// Landline country code of the informant.
      /// </summary>
      [StringLength(4)]
      [Display(Name = " Informant Landline Country Code")]
      public string InformantLandlineCountryCode { get; set; }

      /// <summary>
      /// Landline number of the informant.
      /// </summary>
      [StringLength(20)]
      [Display(Name = "Informant Landline")]
      public string InformantLandline { get; set; }

      /// <summary>
      /// Cellphone country code of the informant.
      /// </summary>
      [StringLength(4)]
      [Display(Name = "Informant Cellphone Country Code")]
      public string InformantCellphoneCountryCode { get; set; }

      /// <summary>
      /// Cellphone number of the informant.
      /// </summary>
      [StringLength(20)]
      [Display(Name = "Informant Cellphone")]
      public string InformantCellphone { get; set; }

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