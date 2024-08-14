using Domain.Entities;
using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Constants;

/*
 * Created by: Lion
 * Date created: 28.09.2022
 * Modified by: Bella
 * Last modified: 06.11.2022 
 */

namespace Domain.Dto
{
    public class RecoveryRequestDto
    {
        /// <summary>
        /// Primary key of the table RecoveryRequests.
        /// </summary>
        [Key]
        public Guid Oid { get; set; }

        /// <summary>
        /// Country code of the user.        
        /// </summary>
        //[Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Country code")]
        public string CountryCode { get; set; }
        /// <summary>
        /// The row is assigned to the cellphone number of a user.
        /// </summary>
        /// 
        //[Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(20)]
        [Display(Name = "Cellphone")]
        //[IfNotInteger]
        public string Cellphone { get; set; } = null!;

        /// <summary>
        /// The row is assigned to the username of a user.
        /// </summary>
        /// 
        //[Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(30)]
        [Display(Name = "Username")]
        public string Username { get; set; }
    }
}
