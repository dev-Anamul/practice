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
    public class CompletePrEPPlanDto : BaseModel
    {
        public List<ChiefComplaint> chiefComplaints { get; set; }
        
        public List<SystemReview> systemReviews { get; set; }
        
        public List<MedicalHistory> medicalHistories { get; set; }
        
        public List<DrugAdherence> drugAdherence { get; set; }
        
        public List<HIVPreventionHistory> preventionHistory { get; set; }
        
        public List<Condition> conditions { get; set; }
        
        public List<ImmunizationRecord> immunizationRecords { get; set; }
        
        public List<GynObsHistory> gynObsHistories { get; set; }
        
        public List<BirthHistory> birthHistories { get; set; }
        
        public List<ChildsDevelopmentHistory> childsDevelopmentHistories { get; set; }
        
        public List<Assessment> assessments { get; set; }
        
        public List<SystemExamination> systemExaminations { get; set; }

        public List<Plan> PrEP { get; set; }
    }
}