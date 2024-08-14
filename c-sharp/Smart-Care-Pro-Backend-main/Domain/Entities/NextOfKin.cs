using Domain.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Tomas
 * Last modified: 15.03.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// NextOfKin entity.
   /// </summary>
   public class NextOfKin : BaseModel
   {
      /// <summary>
      /// Primary key of the table NextOfKins.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// First name of the kin.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [MaxLength(60), MinLength(2)]
      [Display(Name = "First name")]
      [IfNotAlphabet]
      public string FirstName { get; set; }

      /// <summary>
      /// Surname of the kin.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [MaxLength(60), MinLength(2)]
      [IfNotAlphabet]
      public string Surname { get; set; }

      /// <summary>
      /// Next Of Kin Type.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Next of kin type")]
      public NextOfKinType NextOfKinType { get; set; }

      /// <summary>
      /// Other Kin of client.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "If other type")]
      public string OtherNextOfKinType { get; set; }

      /// <summary>
      /// House Number of Kin of client.
      /// </summary>
      [StringLength(20)]
      [Display(Name = "House/plot number")]
      public string HouseNumber { get; set; }

      /// <summary>
      /// House Number of Other Kin of client.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Street name")]
      public string StreetName { get; set; }

      /// <summary>
      /// Township of Other Kin of client.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Township/compund")]
      public string Township { get; set; }

      /// <summary>
      /// Cheif Name of Kin of client.
      /// </summary>
      [StringLength(120)]
      [Display(Name = "Chief/Headman")]
      public string ChiefName { get; set; }

      /// <summary>
      /// Cellphone Country Code of the kin.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(4)]
      [Display(Name = "Country Code")]
      public string CellphoneCountryCode { get; set; }

      /// <summary>
      /// Cellhone number of the kin.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(20)]
      [MaxLength(10, ErrorMessage = MessageConstants.MaxCellPhoneValidation), MinLength(9, ErrorMessage = MessageConstants.MinCellPhoneValidation)]
      [Display(Name = "Phone number")]
      public string Cellphone { get; set; }

      /// <summary>
      /// Other Cellphone Country Code of the kin.
      /// </summary>
      [StringLength(4)]
      [Display(Name = "Other Phone Country Code")]
      public string OtherCellphoneCountryCode { get; set; }

      /// <summary>
      /// Cellhone number of the kin.
      /// </summary>
      [StringLength(20)]
      [MaxLength(10, ErrorMessage = MessageConstants.MaxCellPhoneValidation), MinLength(9, ErrorMessage = MessageConstants.MinCellPhoneValidation)]
      [Display(Name = "Other Phone number")]
      public string OtherCellphone { get; set; }

      /// <summary>
      /// Email Address of Other Kin of client.
      /// </summary>
      [StringLength(60)]
      [EmailAddress]
      [Display(Name = "Email address")]
      public string EmailAddress { get; set; }

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