using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

/*
 * Created by   : Lion
 * Date created : 27.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class DispenseDto
    {
        public Guid PrescriptionId { get; set; }
        public Guid ClientId { get; set; }
        public Guid EncounterId { get; set; }
        public Guid DispenserId { get; set; }

        /// <summary>
        /// Date of the Dispense.
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Dispense Date")]
        public DateTime? DispenseDate { get; set; }

        public List<DispensedItem> DispenseList { get; set; }
    }
}