using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 10.04.2023
 * Modified by  :
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
   public class CompleteTBServiceDto : BaseModel
   {
      public List<TBService> tbServices { get; set; }

      public List<ChiefComplaint> chiefComplaints { get; set; }

      public List<IdentifiedTBSymptom> identifiedTBSymptoms { get; set; }

      public List<IdentifiedConstitutionalSymptom> identifiedConstitutionalSymptoms { get; set; }

      public List<SystemReview> systemReviews { get; set; }

      public List<MedicalHistory> medicalHistories { get; set; }

      public List<TBHistory> tbHistories { get; set; }

      public List<Condition> conditions { get; set; }

      public List<IdentifiedAllergy> allergies { get; set; }

      public List<Assessment> assessments { get; set; }

      public List<GlasgowComaScale> glasgowComaScales { get; set; }

      public List<IdentifiedTBFinding> identifiedTBFindings { get; set; }

      public List<WHOCondition> whoConditions { get; set; }

      public List<IdentifiedReason> identifiedReasons { get; set; }

      public List<Diagnosis> diagnoses { get; set; }

      public List<TreatmentPlan> treatmentPlans { get; set; }

      public List<Dot> dots { get; set; }

      public List<FamilyMember> familyMembers { get; set; }
   }
}