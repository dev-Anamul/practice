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
    public class SystemExaminationDto
    {
        public List<SystemExamination> SystemExaminations
        {
            get; set;
        }
        public Guid EncounterId { get; set; }
        public Guid ClientID { get; set; }
        public Enums.EncounterType EncounterType { get; set; }
        public int? CreatedIn { get; set; }
        public DateTime? DateCreated { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}