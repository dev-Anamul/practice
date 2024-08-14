using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class RightCardDto
    {
        public List<Diagnosis> diagnoses { get; set; }
        public List<TreatmentPlan> treatmentPlans { get; set; }
        public List<InvestigationDto> investigations { get; set; }
        public MedicationDto medicationDto { get; set; }

    }
}
