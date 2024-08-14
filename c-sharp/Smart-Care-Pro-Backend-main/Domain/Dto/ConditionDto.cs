using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
    public class ConditionDto
    {
        public List<Condition> conditions { get; set; }
        public Guid EncounterId { get; set; }
        public Guid ClientId { get; set; }

        public int? CreatedIn { get; set; }

        public Guid? CreatedBy { get; set; }

        public Enums.EncounterType EncounterType { get; set; }
    }
}