using Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
    public class CompleteTreatMentPlanDto : EncounterBaseModel
    {
        public List<Nutrition> nutritions { get; set; }

        public List<ChiefComplaint> chiefComplaints { get; set; }

        public List<IdentifiedTBSymptom> identifiedTBSymptoms { get; set; }

        public List<IdentifiedConstitutionalSymptom> identifiedConstitutionalSymptoms { get; set; }

        public List<SystemReview> systemReviews { get; set; }

        public List<MedicalHistory> medicalHistories { get; set; }

        public List<Condition> conditions { get; set; }

        public List<IdentifiedAllergy> allergies { get; set; }

        public List<ImmunizationRecord> immunizationRecords { get; set; }

        public List<TreatmentPlan> treatmentPlans { get; set; }

        public List<Diagnosis> diagnoses { get; set; }

        public List<GlasgowComaScale> glasgowComaScales { get; set; }

        public List<GynObsHistory> gynObsHistories { get; set; }

        public List<BirthHistory> birthHistories { get; set; }

        public List<FeedingHistory> feedingHistories { get; set; }

        public List<ChildsDevelopmentHistory> childsDevelopmentHistories { get; set; }

        public List<Assessment> assessments { get; set; }

        public List<SystemExamination> systemExaminations { get; set; }

        public List<CounsellingService> counsellingServices { get; set; }

        public List<DrugAdherence> drugAdherence { get; set; }

        public List<HIVPreventionHistory> preventionHistory { get; set; }

        public List<Plan> PEP { get; set; }

        // TB Service
        public List<TBService> tbServices { get; set; }

        public List<TBFinding> tbFinding { get; set; }

        public List<TBHistory> tbHistories { get; set; }

        public List<IdentifiedTBFinding> identifiedTBFindings { get; set; }

        public List<WHOCondition> whoConditions { get; set; }

        public List<IdentifiedReason> identifiedReasons { get; set; }

        public List<Dot> dots { get; set; }

        public List<WHOStagesCondition> whoStagesConditions { get; set; }

        // VMMC
        public List<VMMCService> vmmcServices { get; set; }

        public List<PainRecord> painRecords { get; set; }

        public List<Complication> complications { get; set; }

        public List<CircumcisionReason> circumcisionReason { get; set; }

        public List<AnestheticPlan> anestheticPlans { get; set; }

        public List<SkinPreparation> skinPreparations { get; set; }

        // ART
        public List<ARTService> artServices { get; set; }

        public List<AttachedFacility> attachedFacilities { get; set; }

        public List<NextOfKin> nextOfKins { get; set; }

        public List<PatientStatus> patientStatuses { get; set; }

        public List<DSDAssessment> dsdAssesments { get; set; }

        public List<VisitPurpose> visitPurposes { get; set; }

        public List<ARTResponse> artResponses { get; set; }

        public List<ARTDrugAdherence> artDrugAdherences { get; set; }

        // ART-IHPAI
        public List<PriorARTExposure> priorARTExposers { get; set; }

        public List<TPTHistory> tptHistories { get; set; }

        public List<ARTTreatmentPlan> artTreatmentPlans { get; set; }

        public List<ClientsDisability> clientsDisabilities { get; set; }

        //ANC
        public List<PMTCT> pmtcts { get; set; }

        public List<FamilyMember> familyMembers { get; set; }

        public List<ReferralModule> referralModules { get; set; }

        public List<ANCScreening> ancScreenings { get; set; }

        public List<BloodTransfusionHistory> BloodTransfusionHistories { get; set; }

        public List<Surgery> Surgeries { get; set; }

        public List<ANCService> ancServices { get; set; }

        public List<PastAntenatalVisit> pastAntenatalVisits { get; set; }

        public List<PregnancyBooking> pregnancyBookings { get; set; }

        public List<MotherDetail> motherDetails { get; set; }

        public List<ChildDetail> childDetails { get; set; }

        public List<ObstetricExamination> ObstericExaminations { get; set; }

        public List<ScreeningAndPrevention> screeningAndPreventions { get; set; }

        public List<PelvicAndVaginalExamination> pelvicVaginalExaminations { get; set; }

        public List<IdentifiedPregnancyConfirmation> identifiedPregnancyConfirmations { get; set; }

        public List<IdentifiedPriorSensitization> identifiedPriorSensitizations { get; set; }

        public List<GynConfirmation> gynConfirmations { get; set; }

        public List<VaginalPosition> veginalPositions { get; set; }

        public List<DischargeMetric> dischargeMetrics { get; set; }

        // PNC
        public List<VisitDetail> visitDetails { get; set; }

        public List<IdentifiedEyesAssessment> identifiedEyesAssessments { get; set; }

        public List<IdentifiedCordStump> identifiedCordStumps { get; set; }

        public List<IdentifiedPreferredFeeding> preferredFeedings { get; set; }

        // LABOUR AND DELIVERY

        public List<IdentifiedReferralReason> reasonOfReferrals { get; set; }

        public List<MotherDeliverySummary> motherDeliverySummaries { get; set; }

        public List<PPHTreatment> pPHTreatments { get; set; }

        public List<MedicalTreatment> medicalTreatments { get; set; }

        public List<ApgarScore> apgarScores { get; set; }

        public List<NewBornDetail> newBornDetails { get; set; }

        // ANC DELIVERY DISCHARGE BABY
        public List<FeedingMethod> feedingMethods { get; set; }

        // FAMILY PLANNING
        public List<GuidedExamination> guidedExaminations { get; set; }

        public List<QuickExamination> quickExaminations { get; set; }

        public List<MedicalCondition> medicalConditions { get; set; }

        public List<FamilyPlan> familyPlans { get; set; }

        public List<FamilyPlanRegister> familyPlanRegisters { get; set; }

        public List<InsertionAndRemovalProcedure> insertionAndRemovalProcedures { get; set; }

        [Display(Name = "Created in")]
        public int? CreatedIn { get; set; }


        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Date created")]
        public DateTime? DateCreated { get; set; }


        [Display(Name = "Created by")]
        public Guid? CreatedBy { get; set; }

    }
}