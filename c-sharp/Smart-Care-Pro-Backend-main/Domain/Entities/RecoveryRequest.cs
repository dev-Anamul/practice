using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 20.09.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
    /// <summary>
    /// RecoveryRequest entity.
    /// </summary>
    public class RecoveryRequest : BaseModel
    {
        /// <summary>
        /// Primary key of the table RecoveryRequests.
        /// </summary>
        [Key]
        public Guid Oid { get; set; }

        /// <summary>
        /// Name of a user.
        /// </summary>
        [StringLength(20)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        /// <summary>
        /// Country code of the user.        
        /// </summary>
        [StringLength(4)]
        [Display(Name = "Country code")]
        [IfInvalidCountryCode]
        public string CountryCode { get; set; }

        /// <summary>
        /// Cellphone number of the user.
        /// </summary>
        [StringLength(11)]
        [Display(Name = "Cellphone")]
        [IfNotInteger]
        public string Cellphone { get; set; }

        // <summary>
        /// Date of recovery request of a user account.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Date requested")]
        [IfFutureDate]
        public DateTime DateRequested { get; set; }

        /// <summary>
        /// Recovery request is open or not.
        /// </summary>
        [Display(Name = "Is request open")]
        public bool IsRequestOpen { get; set; }
    }
}