using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class ChiefComplaintsDto 
    {
        public Guid EncounterId { get; set; }
        public List<ChiefComplaint> ChiefComplaints { get; set; }

        public List<Diagnosis> Diagnoses { get; set; }
    }
}
