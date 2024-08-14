using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

/*
 * Created by   : Stephan
 * Date created : 15.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class IdentifiedSymptomDto : EncounterBaseModel
    {
        public int[] TBSymptomList { get; set; }

        public int[] ConstitutionalSymptomTypeList { get; set; }

        public Guid ClientID { get; set; }

        public int? CreatedIn { get; set; }

        public DateTime? DateCreated { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}