﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 25.12.2022
 * Modified by  : Lion
 * Last modified: 08.02.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// DeathRecord entity.
   /// </summary>
   public class DeathRecord : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table DeathRecords.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// First name of the informant.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [Display(Name = "Informant's First Name")]
      public string InformantFirstName { get; set; }

      /// <summary>
      /// Surname of the informant.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [Display(Name = "Informant's Surname")]
      public string InformantSurname { get; set; }

      /// <summary>
      /// Nickname of the informant.
      /// </summary>
      [StringLength(60)]
      [Display(Name = "Informant's Nickname")]
      public string InformantNickname { get; set; }

      /// <summary>
      /// Informant's relationship with the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Informant's Relationship")]
      public InformantRelationship InformantRelationship { get; set; }

      /// <summary>
      /// Other relationship of informant with the client.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Informant's Other Relationship")]
      public string InformantOtherRelationship { get; set; }

      /// <summary>
      /// City of the informant.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Informant's City")]
      public string InformantCity { get; set; }

      /// <summary>
      /// Street No. of the informant.
      /// </summary>
      [StringLength(30)]
      [Display(Name = "Informant's StreetNo")]
      public string InformantStreetNo { get; set; }

      /// <summary>
      /// POBox of the informant.
      /// </summary>
      [StringLength(30)]
      [Display(Name = "Informant's POBox")]
      public string InformantPOBox { get; set; }

      /// <summary>
      /// Landmark of the informant .
      /// </summary>
      [StringLength(500)]
      [Display(Name = "Informant's Landmark")]
      public string InformantLandmark { get; set; }

      /// <summary>
      /// Landline country code of the informant.
      /// </summary>
      [StringLength(4)]
      [Display(Name = " Informant's Landline Country Code")]
      public string InformantLandlineCountryCode { get; set; }

      /// <summary>
      /// Landline number of the informant.
      /// </summary>
      [StringLength(20)]
      [Display(Name = "Informant's Landline")]
      public string InformantLandline { get; set; }

      /// <summary>
      /// Cellphone country code of the informant.
      /// </summary>
      [StringLength(4)]
      [Display(Name = "Informant's Cellphone Country Code")]
      public string InformantCellphoneCountryCode { get; set; }

      /// <summary>
      /// Cellphone number of the informant.
      /// </summary>
      [StringLength(20)]
      [Display(Name = "Informant's Cellphone")]
      public string InformantCellphone { get; set; }

      /// <summary>
      /// Date of death of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Date of Death")]
      public DateTime DateOfDeath { get; set; }

      /// <summary>
      /// Age of death of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(30)]
      [Display(Name = "Date of Death")]
      public string AgeOfDeceased { get; set; }

      /// <summary>
      /// Location of the client's death.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Where death occured")]
      public WhereDeathOccured WhereDeathOccured { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Clients.
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
      /// Foreign key. Primary key of the table Districts.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int DistrictOfDeath { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("DistrictOfDeath")]
      [JsonIgnore]
      public virtual District District { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<DeathCause> DeathCause { get; set; }

      //[NotMapped]
      //public List<DeathCause> DeathCauses { get; set; }

      [NotMapped]
      public int ICPC2ID { get; set; }

      [NotMapped]
      public int? ICD11ID { get; set; }

      [NotMapped]
      public int CauseType { get; set; } = 1;
   }
}