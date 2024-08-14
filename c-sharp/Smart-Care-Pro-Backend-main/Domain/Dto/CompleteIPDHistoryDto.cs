using Domain.Entities;

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
    public class CompleteIPDHistoryDto : BaseModel
    {
        public List<ChiefComplaint> chiefComplaint { get; set; }

        public List<Diagnosis> diagnosis { get; set; }

        public List<TreatmentPlan> treatmentPlan { get; set;}   
    }
}