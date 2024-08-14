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
    public class MotherDetailsHistoryDto : BaseModel
    {
        public List<ThirdStageDelivery> ThirdStageDeliveries { get; set; }
        public List<PPHTreatment> PPHTreatments { get; set; }
        public List<MedicalTreatment> MedicalTreatments { get; set; }
        public List<UterusCondition> UterusConditions { get; set; }
        public List<PlacentaRemoval> PlacentaRemovals { get; set; }
        public List<PerineumIntact> PerineumIntacts { get; set; }
        public List<IdentifiedPerineumIntact> IdentifiedPerineumIntacts { get; set; }
    }
}
