using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by   : Lion
 * Date created : 29.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class DiagnosisDto
    {
        public Guid EncounterId { get; set; }
        public List<Diagnosis> DiagnosisList { get; set; }
    }
}
