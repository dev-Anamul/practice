using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 15.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class SystemReviewDto
    {
        public List<SystemReview> systemReviews { get; set; }
        public Guid EncounterId { get; set; }
        public Guid ClientId { get; set; }
        /// <summary>
        /// Reference of the facility where the row is created.
        /// </summary>
        [Display(Name = "Created in")]
        public int? CreatedIn { get; set; }

        /// <summary>
        /// Creation date of the row.
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Date created")]
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// Reference of the user who has created the row.
        /// </summary>
        [Display(Name = "Created by")]
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Reference of the facility where the row is modified.
        /// </summary>
        [Display(Name = "Modified in")]
        public int? ModifiedIn { get; set; }

        /// <summary>
        /// Last modification date of the row.
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Date modified")]
        public DateTime? DateModified { get; set; }

        /// <summary>
        /// Reference of the user who has last modified the row.
        /// </summary>
        [Display(Name = "Modified by")]
        public Guid? ModifiedBy { get; set; }

        /// <summary>
        /// Status of the row. It indicates whether the row is deleted or not.
        /// </summary>
        [Display(Name = "Is row deleted?")]
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Synced status of the row. It indicates whether the row is synced or not.
        /// </summary>
        [Display(Name = "Is row synced?")]
        public bool? IsSynced { get; set; }

        public Enums.EncounterType EncounterType { get; set; }
    }
}