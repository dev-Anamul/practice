using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Bella
 * Last modified: 01.04.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    /// <summary>
    /// IUnitOfWork.
    /// </summary>
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        IDbContextTransaction BeginTransaction();

        Task<IDbContextTransaction> BeginTransactionAsync();

        //USER MODULE:
        IUserAccountRepository UserAccountRepository { get; }

        ILoginHistoryRepository LoginHistoryRepository { get; }

        IRecoveryRequestRepository RecoveryRequestRepository { get; }

        IFacilityAccessRepository FacilityAccessRepository { get; }

        IModuleAccessRepository ModuleAccessRepository { get; }

        //CLIENT MODULE:
        ICountryRepository CountryRepository { get; }

        IProvinceRepository ProvinceRepository { get; }

        IDistrictRepository DistrictRepository { get; }

        ITownRepository TownRepository { get; }

        IFacilityRepository FacilityRepository { get; }

        IOccupationRepository OccupationRepository { get; }

        IHomeLanguageRepository HomeLanguageRepository { get; }

        IEducationLevelRepository EducationLevelRepository { get; }

        IDFZClientDependentRepository DFZClientDependentRepository { get; }

        IDFZClientRepository DFZClientRepository { get; }

        IClientRepository ClientRepository { get; }

        INextOfKinRepository NextOfKinRepository { get; }

        ICaregiverRepository CaregiverRepository { get; }

        //VITALS MODULE:
        IEncounterRepository EncounterRepository { get; }

        IInteractionRepository InteractionRepository { get; }

        IVitalRepository VitalRepository { get; }

        //HTS MODULE:
        IHIVTestingReasonRepository HIVTestingReasonRepository { get; }

        IHIVNotTestingReasonRepository HIVNotTestingReasonRepository { get; }

        IHIVRiskFactorRepository HIVRiskFactorRepository { get; }

        IRiskAssessmentRepository RiskAssessmentRepository { get; }

        IClientTypeRepository ClientTypeRepository { get; }

        IVisitTypeRepository VisitTypeRepository { get; }

        IServicePointRepository ServicePointRepository { get; }

        IHTSRepository HTSRepository { get; }

        //MEDICAL ENCOUNTER MODULE:
        IChildsDevelopmentHistoryRepository ChildsDevelopmentHistoryRepository { get; }

        IChiefComplaintRepository ChiefComplaintRepository { get; }

        ITBSymptomRepository TBSymptomRepository { get; }

        IConstitutionalSymptomRepository ConstitutionalSymptomRepository { get; }

        IConstitutionalSymptomTypeRepository ConstitutionalSymptomTypeRepository { get; }

        IIdentifiedConstitutionalSymptomRepository IdentifiedConstitutionalSymptomRepository { get; }

        IPhysicalSystemRepository PhysicalSystemRepository { get; }

        ISystemReviewRepository SystemReviewRepository { get; }

        IMedicalHistoryRepository MedicalHistoryRepository { get; }

        IMedicineBrandRepository MedicineBrandRepository { get; }

        IMedicineManufactureRepository MedicineManufactureRepository { get; }

        IStoppingReasonRepository StoppingReasonRepository { get; }

        IRiskRepository RiskRepository { get; }

        IAllergyRepository AllergyRepository { get; }

        IAllergicDrugRepository AllergicDrugRepository { get; }

        IBirthHistoryRepository BirthHistoryRepository { get; }

        IVaccineTypeRepository VaccineTypeRepository { get; }

        IVaccineRepository VaccineRepository { get; }

        ICaCxScreeningMethodRepository CaCxScreeningMethodRepository { get; }

        IGynObsHistoryRepository GynObsHistoryRepository { get; }

        IContraceptiveRepository ContraceptiveRepository { get; }

        IContraceptiveHistoryRepository ContraceptiveHistoryRepository { get; }

        IIdentifiedAllergyRepository IdentifiedAllergyRepository { get; }

        IIdentifiedTBSymptomRepository IdentifiedTBSymptomRepository { get; }

        IImmunizationRecordRepository ImmunizationRecordRepository { get; }

        IAssessmentRepository AssessmentRepository { get; }

        ITreatmentPlanRepository TreatmentPlanRepository { get; }

        ISystemExaminationRepository SystemExaminationRepository { get; }

        IGlasgowComaScaleRepository GlasgowComaScaleRepository { get; }

        IDiagnosisRepository DiagnosisRepository { get; }

        IICDDiagnosisRepository ICDDiagnosisRepository { get; }

        IConditionRepository ConditionRepository { get; }

        INTGLevelOneDiagnosisRepository NTGLevelOneDiagnosisRepository { get; }

        INTGLevelTwoDiagnosisRepository NTGLevelTwoDiagnosisRepository { get; }

        INTGLevelThreeDiagnosisRepository NTGLevelThreeDiagnosisRepository { get; }

        IExposureRepository ExposureRepository { get; }

        IExposureTypeRepository ExposureTypeRepository { get; }

        IPEPRiskRepository PEPRiskRepository { get; }

        IPEPRiskStatusRepository PEPRiskStatusRepository { get; }

        IPEPPreventionHistoryRepository PEPPreventionHistoryRepository { get; }

        ITestRepository TestRepository { get; }

        IInvestigationRepository InvestigationRepository { get; }

        ITestTypeRepository TestTypeRepository { get; }

        ITestSubtypeRepository TestSubtypeRepository { get; }

        //ADMISSION MODULE:
        IDepartmentRepository DepartmentRepository { get; }

        IFirmRepository FirmRepository { get; }

        IWardRepository WardRepository { get; }

        IBedRepository BedRepository { get; }

        //BIRTH RECORD MODULE:
        IBirthRecordRepository BirthRecordRepository { get; }

        //PREP MODULE:
        IPrEPRepository PrEPRepository { get; }

        IPrEPStoppingReasonRepository PrEPStoppingReasonRepository { get; }

        IDrugAdherenceRepository DrugAdherenceRepository { get; }

        IKeyPopulationRepository KeyPopulationRepository { get; }

        IKeyPopulationDemographicRepository KeyPopulationDemographicRepository { get; }

        IQuestionRepository QuestionRepository { get; }

        IHIVRiskScreeningRepository HIVRiskScreeningRepository { get; }

        //SURGERY MODULE:
        ISurgeryRepository SurgeryRepository { get; }

        //PAIN SCALE MODULE:
        IPainScaleRepository PainScaleRepository { get; }

        IPainRecordRepository PainRecordRepository { get; }
        IPastAntenataRepository PastAntenataRepository { get; }

        //NURSING PLAN MODULE:
        INursingPlanRepository NursingPlanRepository { get; }

        ITurningChartRepository TurningChartRepository { get; }

        IFluidRepository FluidRepository { get; }

        IFluidInputRepository FluidInputRepository { get; }

        IFluidOutputRepository FluidOutputRepository { get; }

        //PARTOGRAPH MODULE:
        IPartographRepository PartographRepository { get; }

        IFetalHeartRatesRepository FetalHeartRatesRepository { get; }

        ILiquorsRepository LiquorsRepository { get; }

        IMouldingsRepository MouldingsRepository { get; }

        ICervixRepository CervixRepository { get; }

        IDescentOfHeadsRepository DescentOfHeadsRepository { get; }

        IContractionsRepository ContractionsRepository { get; }

        IOxytocinRepository OxytocinRepository { get; }

        IDropsRepository DropsRepository { get; }

        IMedicinesRepository MedicinesRepository { get; }

        IPulseRepository PulseRepository { get; }

        IBloodPressureRepository BloodPressureRepository { get; }

        ITemperaturesRepository TemperaturesRepository { get; }

        IProteinsRepository ProteinsRepository { get; }

        IAcetonesRepository AcetonesRepository { get; }

        IVolumesRepository VolumesRepository { get; }

        IPartographDetailsRepository PartographDetailsRepository { get; }

        IBirthDetailRepository BirthDetailRepository { get; }

        //COVAX MODULE:
        ICovaxRepository CovaxRepository { get; }

        IVaccineDoseRepository VaccineDoseRepository { get; }

        ICovaxRecordRepository CovaxRecordRepository { get; }

        IAdverseEventRepository AdverseEventRepository { get; }

        //COVID MODULE:
        ICovidRepository CovidRepository { get; }

        ICovidSymptomRepository CovidSymptomRepository { get; }

        ICovidSymptomScreeningRepository CovidSymptomScreeningRepository { get; }

        ICovidComorbidityRepository CovidComorbidityRepository { get; }

        IExposerRiskRepository ExposerRiskRepository { get; }

        //INVESTIGATION MODULE:
        IMeasuringUnitRepository MeasuringUnitRepository { get; }

        IResultOptionRepository ResultOptionRepository { get; }

        ITestItemRepository TestItemRepository { get; }

        ICompositeTestRepository CompositeTestRepository { get; }

        IResultRepository ResultRepository { get; }

        //DEATH RECORD MODULE:
        IDeathRecordRepository DeathRecordRepository { get; }

        IDeathCauseRepository DeathCauseRepository { get; }

        IAnatomicAxisRepository AnatomicAxisRepository { get; }

        IPathologyAxisRepository PathologyAxisRepository { get; }

        IICPC2DescriptionRepository ICPC2DescriptionRepository { get; }

        IDotRepository DotRepository { get; }
        IDotCalenderRepository DotCalenderRepository { get; }

        IIdentifiedTBFindingRepository IdentifiedTBFindingRepository { get; }

        ITBFindingRepository TBFindingRepository { get; }

        ITBSuspectingReasonRepository TBSuspectingReasonRepository { get; }

        IWHOClinicalStageRepository WHOClinicalStageRepository { get; }

        IWHOStagesConditionRepository WHOStagesConditionRepository { get; }

        IWHOConditionRepository WHOConditionRepository { get; }

        IIdentifiedReasonRepository IdentifiedReasonRepository { get; }

        ITBServiceRepository TBServiceRepository { get; }

        IDisabilityRepository DisabilityRepository { get; }

        IClientsDisabilityRepository ClientsDisabilityRepository { get; }

        IDSDAssesmentRepository DSDAssesmentRepository { get; }

        IPMTCTRepository PMTCTRepository { get; }

        //PNC MODULE:
        IVisitDetailRepository VisitDetailRepository { get; }

        IPelvicAndVaginalExaminationRepository PelvicAndVaginalExaminationRepository { get; }

        IIdentifiedEyesAssessmentRepository IdentifiedEyesAssessmentRepository { get; }

        IIdentifiedCordStumpRepository IdentifiedCordStumpRepository { get; }

        IPreferredFeedingRepository PreferredFeedingRepository { get; }

        IIdentifiedPreferredFeedingRepository IdentifiedPreferredFeedingRepository { get; }

        IIdentifiedDeliveryInterventionRepository IdentifiedDeliveryInterventionRepository { get; }

        IIdentifiedCurrentDeliveryComplicationRepository IdentifiedCurrentDeliveryComplicationRepository { get; }

        IThirdStageDeliveryRepository ThirdStageDeliveryRepository { get; }

        IPPHTreatmentRepository PPHTreatmentRepository { get; }

        IMedicalTreatmentRepository MedicalTreatmentRepository { get; }

        IUterusConditionRepository UterusConditionRepository { get; }

        IPlacentaRemovalRepository PlacentaRemovalRepository { get; }

        IPeriuneumIntactRepository PeriuneumIntactRepository { get; }

        IIdentifiedPerineumIntactRepository IdentifiedPerineumIntactRepository { get; }

        IPresentingPartRepository PresentingPartRepository { get; }

        IBreechRepository BreechRepository { get; }

        IModeOfDeliveryRepository ModeOfDeliveryRepository { get; }

        INeonatalBirthOutcomeRepository NeonatalBirthOutcomeRepository { get; }

        ICauseOfStillBirthRepository CauseOfStillBirthRepository { get; }

        IFeedingMethodRepository FeedingMethodRepository { get; }

        IDischargeMetricRepository DischargeMetricRepository { get; }

        IIdentifiedPPHTreatmentRepository IdentifiedPPHTreatmentRepository { get; }

        IIdentifiedPlacentaRemovalRepository IdentifiedPlacentaRemovalRepository { get; }

        ICauseOfNeonatalDeathRepository CauseOfNeonatalDeathRepository { get; }

        INeonatalDeathRepository NeonatalDeathRepository { get; }

        INeonatalAbnormalityRepository NeonatalAbnormalityRepository { get; }

        INeonatalInjuryRepository NeonatalInjuryRepository { get; }

        IApgarScoreRepository ApgarScoreRepository { get; }

        INewBornDetailRepository NewBornDetailRepository { get; }

        //Pharmacy MODULE:
        #region Pharmacy
        IDrugClassRepository DrugClassRepository { get; }

        IDrugSubclassRepository DrugSubclassRepository { get; }

        IGenericDrugRepository GenericDrugRepository { get; }

        IDrugDefinitionRepository DrugDefinitionRepository { get; }

        IDrugRouteRepository DrugRouteRepository { get; }

        IGeneralDrugDefinationRepository GeneralDrugDefinationRepository { get; }

        IDrugRegimenRepository DrugRegimenRepository { get; }
        IGynConfirmationRepository GynConfirmationRepository { get; }

        ISystemRelevanceRepository SystemRelevanceRepository { get; }

        IPrescriptionRepository PrescriptionRepository { get; }

        IMedicationRepository MedicationRepository { get; }

        IFrequencyIntervalRepository FrequencyIntervalRepository { get; }

        IDrugUtilityRepository DrugUtilityRepository { get; }

        IDrugDosageUnitRepository DrugDosageUnitRepository { get; }

        IDrugFormulationRepository DrugFormulationRepository { get; }

        ISpecialDrugRepository SpecialDrugRepository { get; }

        IDispenseRepository DispenseRepository { get; }

        IDrugPickupScheduleRepository DrugPickupScheduleRepository { get; }
        #endregion

        // ART:
        #region ART
        IAttachedFacilityRepository AttachedFacilityRepository { get; }

        IARTRegisterRepository ARTRegisterRepository { get; }

        IFamilyMemberRepository FamilyMemberRepository { get; }

        IPatientStatusRepository PatientStatusRepository { get; }
        IPregnencyBookingRepository PregnencyBookingRepository { get; }

        IPriorARTExposerRepository PriorARTExposerRepository { get; }

        IUseTBIdentificationMethodRepository UseTBIdentificationMethodRepository { get; }

        ITBIdentificationMethodRepository TBIdentificationMethodRepository { get; }

        ITakenARTDrugRepository TakenARTDrugRepository { get; }

        IARTDrugRepository ARTDrugRepository { get; }

        IARTDrugClassRepository ARTDrugClassRepository { get; }

        ITBHistoryRepository TBHistoryRepository { get; }

        ITPTHistoryRepository TPTHistoryRepository { get; }

        ITPTDrugRepository TPTDrugRepository { get; }

        ITakenTPTDrugRepository TakenTPTDrugRepository { get; }

        IARTDrugAdherenceRepository ARTDrugAdherenceRepository { get; }

        ITBDrugRepository TBDrugRepository { get; }

        ITakenTBDrugRepository TakenTBDrugRepository { get; }

        IVisitPuposeRepository VisitPuposeRepository { get; }

        IARTTreatmentPlanRepository ARTTreatmentPlan { get; }
        #endregion

        //UNDER FIVE MODULE:
        #region UnderFive
        INutritionRepository NutritionRepository { get; }

        ICounsellingServiceRepository CounsellingServiceRepository { get; }

        IFeedingHistoryRepository FeedingHistoryRepository { get; }

        IARTResponseRepository ARTResponseRepository { get; }
        #endregion

        #region ServiceQueue
        IServiceQueueRepository ServiceQueueRepository { get; }
        #endregion

        #region VMMC
        IVMMCServiceRepository VMMCServiceRepository { get; }

        IOptedCircumcisionReasonRepository OptedCircumcisionReasonRepository { get; }

        ICircumcisionReasonRepository CircumcisionReasonRepository { get; }

        IVMMCCampaignRepository VMMCCampaignRepository { get; }

        IOptedVMMCCampaignRepository OptedVMMCCampaignRepository { get; }

        IAnestheticPlanRepository AnestheticPlanRepository { get; }

        ISkinPreparationRepository SkinPreparationRepository { get; }

        IComplicationRepository ComplicationRepository { get; }

        IComplicationTypeRepository ComplicationTypeRepository { get; }

        IIdentifiedComplicationRepository IdentifiedComplicationRepository { get; }
        #endregion

        #region ANCService
        IANCServiceRepository ANCServiceRepository { get; }

        IANCScreeningRepository ANCScreeningRepository { get; }

        IMotherDetailRepository MotherDetailRepository { get; }

        IChildDetailRepository ChildDetailRepository { get; }

        IBloodTransfusionHistoryRepository BloodTransfusionHistoryRepository { get; }

        IPregnancyBookingRepository PregnancyBookingRepository { get; }

        IVaginalPositionRepository VaginalPositionRepository { get; }

        IScreeningAndPreventionRepository ScreeningAndPreventionRepository { get; }

        IIdentifiedPregnancyConfirmationRepository IdentifiedPregnancyConfirmationRepository { get; }

        IIdentifiedPriorSensitizationRepository IdentifiedPriorSensitizationRepository { get; }

        IPastAntenatalVisitRepository PastAntenatalVisitRepository { get; }

        IPriorSensitizationRepository PriorSensitizationRepository { get; }

        IObstetricExaminationRepository ObstetricExaminationRepository { get; }

        #endregion

        #region Family Planing
        IQuickExaminationRepository QuickExaminationRepository { get; }

        IGuidedExaminationRepository GuidedExaminationRepository { get; }
        #endregion

        // Labour & Delivery
        IReferralModuleRepository ReferralModuleRepository { get; }

        IIdentifiedReferralReasonRepository IdentifiedReferralReasonRepository { get; }

        IReasonsOfReferalRepository ReasonsOfReferalRepository { get; }

        IMotherDeliverySummaryRepository MotherDeliverySummaryRepository { get; }

        // Family Planning
        IFamilyPlanRegisterRepository FamilyPlanRegisterRepository { get; }

        ISTIRiskRepository STIRiskRepository { get; }

        IPastMedicalConditonRepository PastMedicalConditonRepository { get; }

        IMedicalConditionRepository MedicalConditionRepository { get; }

        IFamilyPlanRepository FamilyPlanRepository { get; }

        IFamilyPlanningClassRepository FamilyPlanningClassRepository { get; }

        IFamilyPlanningSubClassRepository FamilyPlanningSubClassRepository { get; }

        IInsertionAndRemovalProcedureRepository InsertionAndRemovalProcedureRepository { get; }

        ILogRepository LogRepository { get; }

        IFacilityQueueRepository FacilityQueueRepository { get; }


        #region DFZ
        IArmedForceServiceRepository ArmedForceServiceRepository { get; }

        IPatientTypeRepository PatientTypeRepository { get; }

        IDFZRankRepository DFZRankRepository { get; }

        #endregion

        #region PreScreeningVisit
        IPreScreeningVisitRepository PreScreeningVisitRepository { get; }
        #endregion
        #region Screening
        IScreeningRepository ScreeningRepository { get; }
        #endregion 

        #region ThermoAblation
        IThermoAblationRepository ThermoAblationRepository { get; }
        #endregion   

        #region leeps
        ILeepsRepository LeepsRepository { get; }
        #endregion

        #region InterCourseStatus
        IInterCourseStatusRepository InterCourseStatusRepository { get; }
        #endregion

        #region ThermoAblationTreatmentMethod
        IThermoAblationTreatmentMethodRepository ThermoAblationTreatmentMethodRepository { get; }
        #endregion

        #region LeepsTreatmentMethod
        ILeepsTreatmentMethodRepository LeepsTreatmentMethodRepository { get; }
        #endregion

        #region CaCXPlanRepository
        ICaCXPlanRepository CaCXPlanRepository { get; }
        #endregion
    }
}