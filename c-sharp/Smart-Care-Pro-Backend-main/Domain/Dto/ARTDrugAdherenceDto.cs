using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Utilities.Constants;

/*
 * Created by   : Bella
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class ARTDrugAdherenceDto
    {
        public List<ARTDrugAdherence> ARTDrugAdherences { get; set; }

        public Guid EncounterID { get; set; }

        public Guid ClientID { get; set; }

        public int? CreatedIn { get; set; }

        public DateTime? DateCreated { get; set; }

        public Guid? CreatedBy { get; set; }

        public Enums.EncounterType EncounterType { get; set; }
    }
}