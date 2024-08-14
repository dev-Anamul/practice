using System.ComponentModel.DataAnnotations;
using Utilities.Constants;


/*
 * Created by   : Lion
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
   /// <summary>
   /// Login Dto.
   /// </summary>
   public class LoginDto
   {
      /// <summary>
      /// The row is assigned to the username of a user.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(30)]
      [Display(Name = "Username")]
      public string Username { get; set; }

      /// <summary>
      /// The row is assigned to the password of a user account.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string Password { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Facilities.
      /// </summary>
      public int FacilityId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Provinces.
      /// </summary>
      public int ProvinceId { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Districts.
      /// </summary>
      public int DistrictId { get; set; }
   }
}