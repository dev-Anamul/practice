using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public class PrescriptionsDto
    {
        public Guid InteractionId { get; set; }
        public Guid PrescreptionID { get; set; }
        public Guid ClientID { get; set; }
        public Guid EncounterID { get; set; }
        public Guid PrescriberID { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public int? CreatedIn { get; set; }
        public int? ModifiedIn { get; set; }

        public DateTime PrescriptionDate { get; set; }
        public DateTime? DateModified { get; set; }
        /// <summary>
        ///Check Prescription Is Dispansed or not.
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Dispensation Date")]
        public DateTime? DispensationDate { get; set; }

        public int? DispensedDrugsQuantity { get; set; }

        public string DispensedDrugsBrand { get; set; }

        public string ReasonForReplacement { get; set; }

        public List<Medication> PrescriptionsList { get; set; }

    }
}