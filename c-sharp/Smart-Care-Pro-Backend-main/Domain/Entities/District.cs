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
   /// District entity.
   /// </summary>
   public class District : BaseModel
   {
      /// <summary>
      /// Primary key of the table Districts.
      /// </summary>
      [Key]
      public int Oid { get; set; }

      /// <summary>
      /// Name of a district.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(90)]
      [Display(Name = "District name")]
      public string Description { get; set; }

      /// <summary>
      /// District Code.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(4)]
      [Display(Name = "District Code")]
      public string DistrictCode { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Provinces. 
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int ProvinceId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ProvinceId")]
      [JsonIgnore]
      public virtual Province Provinces { get; set; }

      /// <summary>
      /// Facilities of a district.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Facility> Facilities { get; set; }

      /// <summary>
      /// Towns of a district.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Town> Towns { get; set; }

      /// <summary>
      /// Clients of a district.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Client> Clients { get; set; }

      /// <summary>
      /// Death record of the client.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<DeathRecord> DeathRecords { get; set; }
   }
}