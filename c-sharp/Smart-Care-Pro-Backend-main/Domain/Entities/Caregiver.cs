using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Caregiver entity.
   /// </summary>
   public class Caregiver : BaseModel
   {
      /// <summary>
      /// Primary key of the table Caregivers.
      /// </summary>
      [Key]
      public Guid Oid { get; set; }

      /// <summary>
      /// First name of the caregiver.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [MaxLength(60), MinLength(2)]
      [Display(Name = "First name")]
      [IfNotAlphabet]
      public string FirstName { get; set; }

      /// <summary>
      /// Surname of the caregiver.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [MaxLength(60), MinLength(2)]
      [Display(Name = "Surname")]
      [IfNotAlphabet]
      public string Surname { get; set; }

      /// <summary>
      /// Caregiver's relationship with the client.
      /// </summary>
      [StringLength(30)]
      [Display(Name = "Relationship with client")]
      public string Relationship { get; set; }

      /// <summary>
      /// Country code of the caregiver's cellphone number.
      /// </summary>
      [StringLength(4)]
      [MaxLength(4), MinLength(2)]
      [Display(Name = "Country code")]
      [IfInvalidCountryCode]
      public string CountryCode { get; set; }

      /// <summary>
      /// Cellphone number of the caregiver.
      /// </summary>
      [StringLength(11)]
      [Display(Name = "Cellphone number")]
      [IfNotInteger]
      public string Cellphone { get; set; }

      /// <summary>
      /// Profession of the caregiver.
      /// </summary>
      [StringLength(90)]
      [Display(Name = "Profession")]
      public string Profession { get; set; }

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
   }
}