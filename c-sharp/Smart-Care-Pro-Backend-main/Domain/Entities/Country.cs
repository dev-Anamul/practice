using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Tomas
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Country entity.
   /// </summary>
   public class Country : BaseModel
   {
      /// <summary>
      /// Primary key of the table Countries.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Name of a country.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "Description")]
      [IfNotAlphabet]
      public string Description { get; set; }

      /// <summary>
      /// ISO Alpha-2 counrty code of a country.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(2)]
      [Display(Name = "ISO Alpha-2")]
      public string ISOCodeAlpha2 { get; set; }

      /// <summary>
      /// Country code of a country.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(4)]
      [MaxLength(4), MinLength(2)]
      [Display(Name = "Country code")]
      [IfInvalidCountryCode]
      public string CountryCode { get; set; }

      /// <summary>
      /// Citizens of a country.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Client> Clients { get; set; }
   }
}