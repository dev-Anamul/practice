using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
 * Created by   : Bella
 * Date created : 12.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class PredictiveTestDto
    {
        public Guid Oid { get; set; }

        [StringLength(200)]
        [Display(Name = "Inter-incisor gap")]
        public string InterIncisorGap { get; set; }

        [StringLength(200)]
        [Display(Name = "Movement of head/neck")]
        public string MovementOfHeadNek { get; set; }

        [StringLength(200)]
        [Display(Name = "Thyromental distance")]
        public string ThyromentalDistance { get; set; }

        [StringLength(200)]
        [Display(Name = "Atlanto-occiptal flexion/extension")]
        public string AtlantoOcciptalFlexion { get; set; }

        [StringLength(200)]
        [Display(Name = "Mallampati class")]
        public string MallampatiClass { get; set; }

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
    }
}