using Infrastructure.Contracts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

/*
 * Created by: Lion
 * Date created: 12.09.2022
 * Modified by: Bella
 * Last modified: 25.12.2022
 */
namespace Infrastructure
{
    /// <summary>
    /// Implementation of IUnitOfWork.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        protected readonly DataContext context;
        protected readonly IConfiguration configuration;
        private bool _disposed;

        public UnitOfWork(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        //CLIENT
        #region CountryRepository
        private ICountryRepository countryRepository;
        public ICountryRepository CountryRepository
        {
            get
            {
                if (countryRepository == null)
                    countryRepository = new CountryRepository(context);

                return countryRepository;
            }
        }
        #endregion

        #region ProvinceRepository
        private IProvinceRepository provinceRepository;
        public IProvinceRepository ProvinceRepository
        {
            get
            {
                if (provinceRepository == null)
                    provinceRepository = new ProvinceRepository(context);

                return provinceRepository;
            }
        }
        #endregion

        #region DistrictRepository 
        private IDistrictRepository districtRepository;
        public IDistrictRepository DistrictRepository
        {
            get
            {
                if (districtRepository == null)
                    districtRepository = new DistrictRepository(context);

                return districtRepository;
            }
        }
        #endregion

        #region Facility
        private IFacilityRepository facilityRepository;
        public IFacilityRepository FacilityRepository
        {
            get
            {
                if (facilityRepository == null)
                    facilityRepository = new FacilityRepository(context);

                return facilityRepository;
            }
        }
        #endregion

        #region Town
        private ITownRepository townRepository;
        public ITownRepository TownRepository
        {
            get
            {
                if (townRepository == null)
                    townRepository = new TownRepository(context);

                return townRepository;
            }
        }
        #endregion

        #region OccupationRepository
        private IOccupationRepository occupationRepository;
        public IOccupationRepository OccupationRepository
        {
            get
            {
                if (occupationRepository == null)
                    occupationRepository = new OccupationRepository(context);

                return occupationRepository;
            }
        }
        #endregion

        #region HomeLanguageRepository
        private IHomeLanguageRepository homeLanguageRepository;
        public IHomeLanguageRepository HomeLanguageRepository
        {
            get
            {
                if (homeLanguageRepository == null)
                    homeLanguageRepository = new HomeLanguageRepository(context);

                return homeLanguageRepository;
            }
        }
        #endregion

        #region EducationLevelRepository
        private IEducationLevelRepository educationLevelRepository;
        public IEducationLevelRepository EducationLevelRepository
        {
            get
            {
                if (educationLevelRepository == null)
                    educationLevelRepository = new EducationLevelRepository(context);

                return educationLevelRepository;
            }
        }
        #endregion

        #region ClientRepository
        private IClientRepository clientRepository;
        public IClientRepository ClientRepository
        {
            get
            {
                if (clientRepository == null)
                    clientRepository = new ClientRepository(context);

                return clientRepository;
            }
        }
        #endregion

        #region DFZClientRepository
        private IDFZClientRepository dfzClientRepository;
        public IDFZClientRepository DFZClientRepository
        {
            get
            {
                if (dfzClientRepository == null)
                    dfzClientRepository = new DFZClientRepository(context);

                return dfzClientRepository;
            }
        }
        #endregion

        #region DFZClientDependentRepository
        private IDFZClientDependentRepository dfzClientDependentRepository;
        public IDFZClientDependentRepository DFZClientDependentRepository
        {
            get
            {
                if (dfzClientDependentRepository == null)
                    dfzClientDependentRepository = new DFZClientDependentRepository(context);

                return dfzClientDependentRepository;
            }
        }
        #endregion

        #region NextOfKinRepository
        private INextOfKinRepository nextOfKinRepository;
        public INextOfKinRepository NextOfKinRepository
        {
            get
            {
                if (nextOfKinRepository == null)
                    nextOfKinRepository = new NextOfKinRepository(context);

                return nextOfKinRepository;
            }
        }
        #endregion

        #region CaregiverRepository
        private ICaregiverRepository caregiverRepository;
        public ICaregiverRepository CaregiverRepository
        {
            get
            {
                if (caregiverRepository == null)
                    caregiverRepository = new CaregiverRepository(context);

                return caregiverRepository;
            }
        }
        #endregion

        //INTERACTION
        #region InteractionRepository
        private IInteractionRepository interactionRepository;
        public IInteractionRepository InteractionRepository
        {
            get
            {
                if (interactionRepository == null)
                    interactionRepository = new InteractionRepository(context);

                return interactionRepository;
            }
        }
        #endregion InteractionRepository

        #region OPDVisitRepository
        private IEncounterRepository encounterRepository;
        public IEncounterRepository EncounterRepository
        {
            get
            {
                if (encounterRepository == null)
                    encounterRepository = new EncounterRepository(context);

                return encounterRepository;
            }
        }
        #endregion

        //USER ACCOUNT
        #region UserAccountRepository
        private IUserAccountRepository userAccountRepository;
        public IUserAccountRepository UserAccountRepository
        {
            get
            {
                if (userAccountRepository == null)
                    userAccountRepository = new UserAccountRepository(context);

                return userAccountRepository;
            }
        }
        #endregion UserAccountRepository

        #region LoginHistoryRepository
        private ILoginHistoryRepository loginHistoryRepository;
        public ILoginHistoryRepository LoginHistoryRepository
        {
            get
            {
                if (loginHistoryRepository == null)
                    loginHistoryRepository = new LoginHistoryRepository(context);

                return loginHistoryRepository;
            }
        }
        #endregion LoginHistoryRepository

        #region RecoveryRequestRepository
        private IRecoveryRequestRepository recoveryRequestRepository;
        public IRecoveryRequestRepository RecoveryRequestRepository
        {
            get
            {
                if (recoveryRequestRepository == null)
                    recoveryRequestRepository = new RecoveryRequestRepository(context);

                return recoveryRequestRepository;
            }
        }
        #endregion RecoveryRequestRepository

        #region FacilityAccessRepository
        private IFacilityAccessRepository facilityaccessRepository;
        public IFacilityAccessRepository FacilityAccessRepository
        {
            get
            {
                if (facilityaccessRepository == null)
                    facilityaccessRepository = new FacilityAccessRepository(context);

                return facilityaccessRepository;
            }
        }
        #endregion FacilityAccessRepository

        #region ModuleAccessRepository
        private IModuleAccessRepository moduleaccessRepository;
        public IModuleAccessRepository ModuleAccessRepository
        {
            get
            {
                if (moduleaccessRepository == null)
                    moduleaccessRepository = new ModuleAccessRepository(context);

                return moduleaccessRepository;
            }
        }
        #endregion

        //HTS

        #region ClientTypeRepository
        private IClientTypeRepository clientTypeRepository;
        public IClientTypeRepository ClientTypeRepository
        {
            get
            {
                if (clientTypeRepository == null)
                    clientTypeRepository = new ClientTypeRepository(context);

                return clientTypeRepository;
            }
        }
        #endregion

        #region VisitTypeRepository
        private IVisitTypeRepository visitTypeRepository;
        public IVisitTypeRepository VisitTypeRepository
        {
            get
            {
                if (visitTypeRepository == null)
                    visitTypeRepository = new VisitTypeRepository(context);

                return visitTypeRepository;
            }
        }
        #endregion

        #region ServicePointRepository
        private IServicePointRepository servicePointRepository;
        public IServicePointRepository ServicePointRepository
        {
            get
            {
                if (servicePointRepository == null)
                    servicePointRepository = new ServicePointRepository(context);

                return servicePointRepository;
            }
        }
        #endregion

        #region HTSRepository
        private IHTSRepository htsRepository;
        public IHTSRepository HTSRepository
        {
            get
            {
                if (htsRepository == null)
                    htsRepository = new HTSRepository(context);

                return htsRepository;
            }
        }
        #endregion

        #region HIVRiskFactorRepository
        private IHIVRiskFactorRepository hivRiskFactorRepository;
        public IHIVRiskFactorRepository HIVRiskFactorRepository
        {
            get
            {
                if (hivRiskFactorRepository == null)
                    hivRiskFactorRepository = new HIVRiskFactorRepository(context);

                return hivRiskFactorRepository;
            }
        }
        #endregion

        #region HIVTestingReasonRepository
        private IHIVTestingReasonRepository hivTestingReasonRepository;
        public IHIVTestingReasonRepository HIVTestingReasonRepository
        {
            get
            {
                if (hivTestingReasonRepository == null)
                    hivTestingReasonRepository = new HIVTestingReasonRepository(context);

                return hivTestingReasonRepository;
            }
        }
        #endregion

        #region HIVNotTestingReasonRepository
        private IHIVNotTestingReasonRepository hivNotTestingReasonRepository;
        public IHIVNotTestingReasonRepository HIVNotTestingReasonRepository
        {
            get
            {
                if (hivNotTestingReasonRepository == null)
                    hivNotTestingReasonRepository = new HIVNotTestingReasonRepository(context);

                return hivNotTestingReasonRepository;
            }
        }
        #endregion

        #region RiskAssessmentRepository
        private IRiskAssessmentRepository riskAssessmentRepository;
        public IRiskAssessmentRepository RiskAssessmentRepository
        {
            get
            {
                if (riskAssessmentRepository == null)
                    riskAssessmentRepository = new RiskAssessmentRepository(context);

                return riskAssessmentRepository;
            }
        }
        #endregion

        //VITAL

        #region VitalRepository
        private IVitalRepository vitalRepository;
        public IVitalRepository VitalRepository
        {
            get
            {
                if (vitalRepository == null)
                    vitalRepository = new VitalRepository(context);

                return vitalRepository;
            }
        }
        #endregion

        //MEDICAL ENCOUNTER

        #region ChildDevelopmentHistory
        private IChildsDevelopmentHistoryRepository childsDevelopmentHistory;
        public IChildsDevelopmentHistoryRepository ChildsDevelopmentHistoryRepository
        {
            get
            {
                if (childsDevelopmentHistory == null)
                    childsDevelopmentHistory = new ChildsDevelopmentHistoryRepository(context);

                return childsDevelopmentHistory;
            }
        }
        #endregion

        #region CheifComplaint
        private IChiefComplaintRepository chiefComplaintRepository;
        public IChiefComplaintRepository ChiefComplaintRepository
        {
            get
            {
                if (chiefComplaintRepository == null)
                    chiefComplaintRepository = new ChiefComplaintRepository(context);

                return chiefComplaintRepository;
            }
        }
        #endregion CheifComplaint

        #region TBSymptomRepository
        private ITBSymptomRepository tbSymptomRepository;
        public ITBSymptomRepository TBSymptomRepository
        {
            get
            {
                if (tbSymptomRepository == null)
                    tbSymptomRepository = new TBSymptomRepository(context);

                return tbSymptomRepository;
            }
        }
        #endregion

        #region ConstitutionalSymptomRepository
        private IConstitutionalSymptomRepository constitutionalSymptomRepository;
        public IConstitutionalSymptomRepository ConstitutionalSymptomRepository
        {
            get
            {
                if (constitutionalSymptomRepository == null)
                    constitutionalSymptomRepository = new ConstitutionalSymptomRepository(context);

                return constitutionalSymptomRepository;
            }
        }
        #endregion

        #region ConstitutionalSymptomTypeRepository
        private IConstitutionalSymptomTypeRepository constitutionalSymptomTypeRepository;
        public IConstitutionalSymptomTypeRepository ConstitutionalSymptomTypeRepository
        {
            get
            {
                if (constitutionalSymptomTypeRepository == null)
                    constitutionalSymptomTypeRepository = new ConstitutionalSymptomTypeRepository(context);

                return constitutionalSymptomTypeRepository;
            }
        }
        #endregion

        #region IdentifiedConstitutionalSymptomRepository
        private IIdentifiedConstitutionalSymptomRepository identifiedConstitutionalSymptomRepository;
        public IIdentifiedConstitutionalSymptomRepository IdentifiedConstitutionalSymptomRepository
        {
            get
            {
                if (identifiedConstitutionalSymptomRepository == null)
                    identifiedConstitutionalSymptomRepository = new IdentifiedConstitutionalSymptomRepository(context);

                return identifiedConstitutionalSymptomRepository;
            }
        }
        #endregion

        #region PhysicalSystemRepository
        private IPhysicalSystemRepository physicalSystemRepository;
        public IPhysicalSystemRepository PhysicalSystemRepository
        {
            get
            {
                if (physicalSystemRepository == null)
                    physicalSystemRepository = new PhysicalSystemRepository(context);

                return physicalSystemRepository;
            }
        }
        #endregion

        #region SystemReviewRepository
        private ISystemReviewRepository systemReviewRepository;
        public ISystemReviewRepository SystemReviewRepository
        {
            get
            {
                if (systemReviewRepository == null)
                    systemReviewRepository = new SystemReviewRepository(context);

                return systemReviewRepository;
            }
        }
        #endregion

        #region MedicalHistoryRepository
        private IMedicalHistoryRepository medicalHistoryRepository;
        public IMedicalHistoryRepository MedicalHistoryRepository
        {
            get
            {
                if (medicalHistoryRepository == null)
                    medicalHistoryRepository = new MedicalHistoryRepository(context);

                return medicalHistoryRepository;
            }
        }
        #endregion

        #region AllergyRepository
        private IAllergyRepository allergyRepository;
        public IAllergyRepository AllergyRepository
        {
            get
            {
                if (allergyRepository == null)
                    allergyRepository = new AllergyRepository(context);

                return allergyRepository;
            }
        }
        #endregion

        #region AllergicDrugRepository
        private IAllergicDrugRepository allergicDrugRepository;
        public IAllergicDrugRepository AllergicDrugRepository
        {
            get
            {
                if (allergicDrugRepository == null)
                    allergicDrugRepository = new AllergicDrugRepository(context);

                return allergicDrugRepository;
            }
        }
        #endregion

        #region BirthHistoryRepository
        private IBirthHistoryRepository birthHistoryRepository;
        public IBirthHistoryRepository BirthHistoryRepository
        {
            get
            {
                if (birthHistoryRepository == null)
                    birthHistoryRepository = new BirthHistoryRepository(context);

                return birthHistoryRepository;
            }
        }
        #endregion

        #region VaccineTypeRepository
        private IVaccineTypeRepository vaccineTypeRepository;
        public IVaccineTypeRepository VaccineTypeRepository
        {
            get
            {
                if (vaccineTypeRepository == null)
                    vaccineTypeRepository = new VaccineTypeRepository(context);

                return vaccineTypeRepository;
            }
        }
        #endregion

        #region VaccineRepository
        private IVaccineRepository vaccineRepository;
        public IVaccineRepository VaccineRepository
        {
            get
            {
                if (vaccineRepository == null)
                    vaccineRepository = new VaccineRepository(context);

                return vaccineRepository;
            }
        }
        #endregion

        #region CaCxScreeningMethodRepository
        private ICaCxScreeningMethodRepository caCxScreeningMethodRepository;
        public ICaCxScreeningMethodRepository CaCxScreeningMethodRepository
        {
            get
            {
                if (caCxScreeningMethodRepository == null)
                    caCxScreeningMethodRepository = new CaCxScreeningMethodRepository(context);

                return caCxScreeningMethodRepository;
            }
        }
        #endregion

        #region GynObsHistoryRepository
        private IGynObsHistoryRepository gynObsHistoryRepository;
        public IGynObsHistoryRepository GynObsHistoryRepository
        {
            get
            {
                if (gynObsHistoryRepository == null)
                    gynObsHistoryRepository = new GynObsHistoryRepository(context);

                return gynObsHistoryRepository;
            }
        }
        #endregion

        #region ContraceptiveRepository
        private IContraceptiveRepository contraceptiveRepository;
        public IContraceptiveRepository ContraceptiveRepository
        {
            get
            {
                if (contraceptiveRepository == null)
                    contraceptiveRepository = new ContraceptiveRepository(context);

                return contraceptiveRepository;
            }
        }
        #endregion

        #region ContraceptiveHistoryRepository
        private IContraceptiveHistoryRepository contraceptiveHistoryRepository;
        public IContraceptiveHistoryRepository ContraceptiveHistoryRepository
        {
            get
            {
                if (contraceptiveHistoryRepository == null)
                    contraceptiveHistoryRepository = new ContraceptiveHistoryRepository(context);

                return contraceptiveHistoryRepository;
            }
        }
        #endregion

        #region IdentifiedTBSymptomRepository
        private IIdentifiedTBSymptomRepository identifiedTBSymptomRepository;
        public IIdentifiedTBSymptomRepository IdentifiedTBSymptomRepository
        {
            get
            {
                if (identifiedTBSymptomRepository == null)
                    identifiedTBSymptomRepository = new IdentifiedTBSymptomRepository(context);

                return identifiedTBSymptomRepository;
            }
        }
        #endregion

        #region IIdentifiedAllergyRepository
        private IIdentifiedAllergyRepository identifiedAllergyRepository;
        public IIdentifiedAllergyRepository IdentifiedAllergyRepository
        {
            get
            {
                if (identifiedAllergyRepository == null)
                    identifiedAllergyRepository = new IdentifiedAllergyRepository(context);

                return identifiedAllergyRepository;
            }
        }
        #endregion

        #region ImmunizationRecordRepository
        private IImmunizationRecordRepository immunizationRecordRepository;
        public IImmunizationRecordRepository ImmunizationRecordRepository
        {
            get
            {
                if (immunizationRecordRepository == null)
                    immunizationRecordRepository = new ImmunizationRecordRepository(context);

                return immunizationRecordRepository;
            }
        }
        #endregion

        #region AssessmentRepository
        private IAssessmentRepository assessmentRepository;
        public IAssessmentRepository AssessmentRepository
        {
            get
            {
                if (assessmentRepository == null)
                    assessmentRepository = new AssessmentRepository(context);

                return assessmentRepository;
            }
        }
        #endregion

        #region TreatmentPlanRepository
        private ITreatmentPlanRepository treatmentPlanRepository;
        public ITreatmentPlanRepository TreatmentPlanRepository
        {
            get
            {
                if (treatmentPlanRepository == null)
                    treatmentPlanRepository = new TreatmentPlanRepository(context);

                return treatmentPlanRepository;
            }
        }
        #endregion

        #region SystemExaminationRepository
        private ISystemExaminationRepository systemExaminationRepository;
        public ISystemExaminationRepository SystemExaminationRepository
        {
            get
            {
                if (systemExaminationRepository == null)
                    systemExaminationRepository = new SystemExaminationRepository(context);

                return systemExaminationRepository;
            }
        }
        #endregion

        #region GlasgowComaScaleRepository
        private IGlasgowComaScaleRepository glasgowComaScaleRepository;
        public IGlasgowComaScaleRepository GlasgowComaScaleRepository
        {
            get
            {
                if (glasgowComaScaleRepository == null)
                    glasgowComaScaleRepository = new GlasgowComaScaleRepository(context);

                return glasgowComaScaleRepository;
            }
        }
        #endregion

        #region DiagnosisRepository
        private IDiagnosisRepository diagnosisRepository;
        public IDiagnosisRepository DiagnosisRepository
        {
            get
            {
                if (diagnosisRepository == null)
                    diagnosisRepository = new DiagnosisRepository(context);

                return diagnosisRepository;
            }
        }
        #endregion

        #region ICDDiagnosisRepository
        private IICDDiagnosisRepository icdDiagnosisRepository;
        public IICDDiagnosisRepository ICDDiagnosisRepository
        {
            get
            {
                if (icdDiagnosisRepository == null)
                    icdDiagnosisRepository = new ICDDiagnosisRepository(context);

                return icdDiagnosisRepository;
            }
        }
        #endregion

        #region ConditionRepository
        private IConditionRepository conditionRepository;
        public IConditionRepository ConditionRepository
        {
            get
            {
                if (conditionRepository == null)
                    conditionRepository = new ConditionRepository(context);

                return conditionRepository;
            }
        }
        #endregion

        #region NTGLevelOneDiagnosisRepository
        private INTGLevelOneDiagnosisRepository ntgLevelOneDiagnosisRepository;
        public INTGLevelOneDiagnosisRepository NTGLevelOneDiagnosisRepository
        {
            get
            {
                if (ntgLevelOneDiagnosisRepository == null)
                    ntgLevelOneDiagnosisRepository = new NTGLevelOneDiagnosisRepository(context);

                return ntgLevelOneDiagnosisRepository;
            }
        }
        #endregion

        #region NTGLevelTwoDiagnosisRepository
        private INTGLevelTwoDiagnosisRepository ntgLevelTwoDiagnosisRepository;
        public INTGLevelTwoDiagnosisRepository NTGLevelTwoDiagnosisRepository
        {
            get
            {
                if (ntgLevelTwoDiagnosisRepository == null)
                    ntgLevelTwoDiagnosisRepository = new NTGLevelTwoDiagnosisRepository(context);

                return ntgLevelTwoDiagnosisRepository;
            }
        }
        #endregion

        #region NTGLevelThreeDiagnosisRepository
        private INTGLevelThreeDiagnosisRepository ntgLevelThreeDiagnosisRepository;
        public INTGLevelThreeDiagnosisRepository NTGLevelThreeDiagnosisRepository
        {
            get
            {
                if (ntgLevelThreeDiagnosisRepository == null)
                    ntgLevelThreeDiagnosisRepository = new NTGLevelThreeDiagnosisRepository(context);

                return ntgLevelThreeDiagnosisRepository;
            }
        }
        #endregion


        #region GeneralDrugDefinationRepository
        private IGeneralDrugDefinationRepository generalDrugDefinationRepository;
        public IGeneralDrugDefinationRepository GeneralDrugDefinationRepository
        {
            get
            {
                if (generalDrugDefinationRepository == null)
                    generalDrugDefinationRepository = new GeneralDrugDefinationRepository(context);

                return generalDrugDefinationRepository;
            }
        }
        #endregion
        #region ExposureRepository
        private IExposureRepository exposureRepository;
        public IExposureRepository ExposureRepository
        {
            get
            {
                if (exposureRepository == null)
                    exposureRepository = new ExposureRepository(context);

                return exposureRepository;
            }
        }
        #endregion

        #region ExposureTypeRepository
        private IExposureTypeRepository exposureTypeRepository;
        public IExposureTypeRepository ExposureTypeRepository
        {
            get
            {
                if (exposureTypeRepository == null)
                    exposureTypeRepository = new ExposureTypeRepository(context);

                return exposureTypeRepository;
            }
        }
        #endregion

        #region PEPRiskRepository
        private IPEPRiskRepository pEPRiskRepository;
        public IPEPRiskRepository PEPRiskRepository
        {
            get
            {
                if (pEPRiskRepository == null)
                    pEPRiskRepository = new PEPRiskRepository(context);

                return pEPRiskRepository;
            }
        }
        #endregion

        #region PEPRiskStatusRepository
        private IPEPRiskStatusRepository pEPRiskStatusRepository;
        public IPEPRiskStatusRepository PEPRiskStatusRepository
        {
            get
            {
                if (pEPRiskStatusRepository == null)
                    pEPRiskStatusRepository = new PEPRiskStatusRepository(context);

                return pEPRiskStatusRepository;
            }
        }
        #endregion

        #region PEPPreventionHistoryRepository
        private IPEPPreventionHistoryRepository pEPPreventionHistoryRepository;
        public IPEPPreventionHistoryRepository PEPPreventionHistoryRepository
        {
            get
            {
                if (pEPPreventionHistoryRepository == null)
                    pEPPreventionHistoryRepository = new PEPPreventionHistoryRepository(context);

                return pEPPreventionHistoryRepository;
            }
        }
        #endregion

        #region PrEPRepository
        private IPrEPRepository prepRepository;
        public IPrEPRepository PrEPRepository
        {
            get
            {
                if (prepRepository == null)
                    prepRepository = new PrEPRepository(context);

                return prepRepository;
            }
        }
        #endregion

        #region PrEPStoppingReasonRepository
        private IPrEPStoppingReasonRepository prEPStoppingReasonRepository;
        public IPrEPStoppingReasonRepository PrEPStoppingReasonRepository
        {
            get
            {
                if (prEPStoppingReasonRepository == null)
                    prEPStoppingReasonRepository = new PrEPStoppingReasonRepository(context);

                return prEPStoppingReasonRepository;
            }
        }
        #endregion

        #region DrugAdherenceRepository
        private IDrugAdherenceRepository drugAdherenceRepository;
        public IDrugAdherenceRepository DrugAdherenceRepository
        {
            get
            {
                if (drugAdherenceRepository == null)
                    drugAdherenceRepository = new DrugAdherenceRepository(context);

                return drugAdherenceRepository;
            }
        }
        #endregion

        #region KeyPopulationRepository
        private IKeyPopulationRepository keyPopulationRepository;
        public IKeyPopulationRepository KeyPopulationRepository
        {
            get
            {
                if (keyPopulationRepository == null)
                    keyPopulationRepository = new KeyPopulationRepository(context);

                return keyPopulationRepository;
            }
        }
        #endregion

        #region KeyPopulationDemographicRepository
        private IKeyPopulationDemographicRepository keyPopulationDemographicRepository;
        public IKeyPopulationDemographicRepository KeyPopulationDemographicRepository
        {
            get
            {
                if (keyPopulationDemographicRepository == null)
                    keyPopulationDemographicRepository = new KeyPopulationDemographicRepository(context);

                return keyPopulationDemographicRepository;
            }
        }
        #endregion

        #region QuestionRepository
        private IQuestionRepository questionRepository;
        public IQuestionRepository QuestionRepository
        {
            get
            {
                if (questionRepository == null)
                    questionRepository = new QuestionRepository(context);

                return questionRepository;
            }
        }
        #endregion

        #region SurgeryRepository
        private ISurgeryRepository surgeryRepository;
        public ISurgeryRepository SurgeryRepository
        {
            get
            {
                if (surgeryRepository == null)
                    surgeryRepository = new SurgeryRepository(context);

                return surgeryRepository;
            }
        }
        #endregion

        #region HIVRiskScreeningRepository
        private IHIVRiskScreeningRepository hivRiskScreeningRepository;
        public IHIVRiskScreeningRepository HIVRiskScreeningRepository
        {
            get
            {
                if (hivRiskScreeningRepository == null)
                    hivRiskScreeningRepository = new HIVRiskScreeningRepository(context);

                return hivRiskScreeningRepository;
            }
        }
        #endregion

        //ADMISSION
        #region DepartmentRepository
        private IDepartmentRepository departmentRepository;
        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                if (departmentRepository == null)
                    departmentRepository = new DepartmentRepository(context);

                return departmentRepository;
            }
        }
        #endregion

        #region FirmRepository
        private IFirmRepository firmRepository;
        public IFirmRepository FirmRepository
        {
            get
            {
                if (firmRepository == null)
                    firmRepository = new FirmRepository(context);

                return firmRepository;
            }
        }
        #endregion

        #region WardRepository
        private IWardRepository wardRepository;
        public IWardRepository WardRepository
        {
            get
            {
                if (wardRepository == null)
                    wardRepository = new WardRepository(context);

                return wardRepository;
            }
        }
        #endregion

        #region BedRepository
        private IBedRepository bedRepository;
        public IBedRepository BedRepository
        {
            get
            {
                if (bedRepository == null)
                    bedRepository = new BedRepository(context);

                return bedRepository;
            }
        }
        #endregion

        //INVESTIGATION

        #region TestRepository
        private ITestRepository testRepository;
        public ITestRepository TestRepository
        {
            get
            {
                if (testRepository == null)
                    testRepository = new TestRepository(context);

                return testRepository;
            }
        }
        #endregion

        #region TestTypeRepository
        private ITestTypeRepository testTypeRepository;
        public ITestTypeRepository TestTypeRepository
        {
            get
            {
                if (testTypeRepository == null)
                    testTypeRepository = new TestTypeRepository(context);

                return testTypeRepository;
            }
        }
        #endregion



        #region TestSubtypeRepository
        private ITestSubtypeRepository testSubtypeRepository;
        public ITestSubtypeRepository TestSubtypeRepository
        {
            get
            {
                if (testSubtypeRepository == null)
                    testSubtypeRepository = new TestSubtypeRepository(context);

                return testSubtypeRepository;
            }
        }
        #endregion

        #region MeasuringUnitRepository
        private IMeasuringUnitRepository measuringUnitRepository;
        public IMeasuringUnitRepository MeasuringUnitRepository
        {
            get
            {
                if (measuringUnitRepository == null)
                    measuringUnitRepository = new MeasuringUnitRepository(context);

                return measuringUnitRepository;
            }
        }
        #endregion

        #region ResultOptionRepository
        private IResultOptionRepository resultOptionRepository;
        public IResultOptionRepository ResultOptionRepository
        {
            get
            {
                if (resultOptionRepository == null)
                    resultOptionRepository = new ResultOptionRepository(context);

                return resultOptionRepository;
            }
        }
        #endregion

        #region TestItemRepository
        private ITestItemRepository testItemRepository;
        public ITestItemRepository TestItemRepository
        {
            get
            {
                if (testItemRepository == null)
                    testItemRepository = new TestItemRepository(context);

                return testItemRepository;
            }
        }
        #endregion

        #region CompositeTestRepository
        private ICompositeTestRepository compositeTestRepository;
        public ICompositeTestRepository CompositeTestRepository
        {
            get
            {
                if (compositeTestRepository == null)
                    compositeTestRepository = new CompositeTestRepository(context);

                return compositeTestRepository;
            }
        }
        #endregion

        #region InvestigationRepository
        private IInvestigationRepository investigationRepository;
        public IInvestigationRepository InvestigationRepository
        {
            get
            {
                if (investigationRepository == null)
                    investigationRepository = new InvestigationRepository(context);

                return investigationRepository;
            }
        }
        #endregion

        #region ResultRepository
        private IResultRepository resultRepository;
        public IResultRepository ResultRepository
        {
            get
            {
                if (resultRepository == null)
                    resultRepository = new ResultRepository(context);

                return resultRepository;
            }
        }
        #endregion

        //BIRTH
        #region BirthRecordRepository
        private IBirthRecordRepository birthRecordRepository;
        public IBirthRecordRepository BirthRecordRepository
        {
            get
            {
                if (birthRecordRepository == null)
                    birthRecordRepository = new BirthRecordRepository(context);

                return birthRecordRepository;
            }
        }
        #endregion

        //NURSING CARE
        #region NursingPlanRepository
        private INursingPlanRepository nursingPlanRepository;
        public INursingPlanRepository NursingPlanRepository
        {
            get
            {
                if (nursingPlanRepository == null)
                    nursingPlanRepository = new NursingPlanRepository(context);

                return nursingPlanRepository;
            }
        }
        #endregion

        #region TurningChartRepository

        private ITurningChartRepository turningChartRepository;
        public ITurningChartRepository TurningChartRepository
        {
            get
            {
                if (turningChartRepository == null)
                    turningChartRepository = new TurningChartRepository(context);

                return turningChartRepository;
            }
        }
        #endregion

        #region FluidRepository

        private IFluidRepository fluidRepository;
        public IFluidRepository FluidRepository
        {
            get
            {
                if (fluidRepository == null)
                    fluidRepository = new FluidRepository(context);

                return fluidRepository;
            }
        }
        #endregion

        #region FluidInputRepository

        private IFluidInputRepository fluidInputRepository;
        public IFluidInputRepository FluidInputRepository
        {
            get
            {
                if (fluidInputRepository == null)
                    fluidInputRepository = new FluidInputRepository(context);

                return fluidInputRepository;
            }
        }
        #endregion

        #region FluidOutputRepository

        private IFluidOutputRepository fluidOutputRepository;
        public IFluidOutputRepository FluidOutputRepository
        {
            get
            {
                if (fluidOutputRepository == null)
                    fluidOutputRepository = new FluidOutputRepository(context);

                return fluidOutputRepository;
            }
        }
        #endregion

        //PAIN SCALE
        #region PainScaleRepository
        private IPainScaleRepository painScaleRepository;
        public IPainScaleRepository PainScaleRepository
        {
            get
            {
                if (painScaleRepository == null)
                    painScaleRepository = new PainScaleRepository(context);

                return painScaleRepository;
            }
        }
        #endregion

        #region PainRecordRepository
        private IPainRecordRepository painRecordRepository;
        public IPainRecordRepository PainRecordRepository
        {
            get
            {
                if (painRecordRepository == null)
                    painRecordRepository = new PainRecordRepository(context);

                return painRecordRepository;
            }
        }
        #endregion

        #region PastAntenataRepository
        private IPastAntenataRepository pastAntenataRepository;
        public IPastAntenataRepository PastAntenataRepository
        {
            get
            {
                if (pastAntenataRepository == null)
                    pastAntenataRepository = new PastAntenataRepository(context);

                return pastAntenataRepository;
            }
        }
        #endregion

        //COVAX
        #region CovaxRepository
        private ICovaxRepository covaxRepository;
        public ICovaxRepository CovaxRepository
        {
            get
            {
                if (covaxRepository == null)
                    covaxRepository = new CovaxRepository(context);

                return covaxRepository;
            }
        }
        #endregion

        #region CovaxRecordRepository
        private ICovaxRecordRepository covaxRecordRepository;
        public ICovaxRecordRepository CovaxRecordRepository
        {
            get
            {
                if (covaxRecordRepository == null)
                    covaxRecordRepository = new CovaxReportRepository(context);

                return covaxRecordRepository;
            }
        }
        #endregion

        #region VaccineDoseRepository
        private IVaccineDoseRepository vaccineDoseRepository;
        public IVaccineDoseRepository VaccineDoseRepository
        {
            get
            {
                if (vaccineDoseRepository == null)
                    vaccineDoseRepository = new VaccineDoseRepository(context);

                return vaccineDoseRepository;
            }
        }
        #endregion

        #region AdverseEventRepository
        private IAdverseEventRepository adverseEventRepository;
        public IAdverseEventRepository AdverseEventRepository
        {
            get
            {
                if (adverseEventRepository == null)
                    adverseEventRepository = new AdverseEventRepository(context);

                return adverseEventRepository;
            }
        }
        #endregion

        //COVID
        #region Covid
        private ICovidRepository covidRepository;
        public ICovidRepository CovidRepository
        {
            get
            {
                if (covidRepository == null)
                    covidRepository = new CovidRepository(context);

                return covidRepository;
            }
        }
        #endregion

        #region CovidComorbidityRepository
        private ICovidComorbidityRepository covidComorbidityRepository;
        public ICovidComorbidityRepository CovidComorbidityRepository
        {
            get
            {
                if (covidComorbidityRepository == null)
                    covidComorbidityRepository = new CovidComorbidityRepository(context);

                return covidComorbidityRepository;
            }
        }
        #endregion

        #region ExposerRiskRepository
        private IExposerRiskRepository exposerRiskRepository;
        public IExposerRiskRepository ExposerRiskRepository
        {
            get
            {
                if (exposerRiskRepository == null)
                    exposerRiskRepository = new ExposerRiskRepository(context);

                return exposerRiskRepository;
            }
        }
        #endregion

        #region CovidSymptomRepository
        private ICovidSymptomRepository covidSymptomRepository;
        public ICovidSymptomRepository CovidSymptomRepository
        {
            get
            {
                if (covidSymptomRepository == null)
                    covidSymptomRepository = new CovidSymptomRepository(context);

                return covidSymptomRepository;
            }
        }
        #endregion

        #region CovidsymptomScreeningRepository
        public ICovidSymptomScreeningRepository covidSymptomScreeningRepository;
        public ICovidSymptomScreeningRepository CovidSymptomScreeningRepository
        {
            get
            {
                if (covidSymptomScreeningRepository == null)
                    covidSymptomScreeningRepository = new CovidsymptomScreeningRepository(context);

                return covidSymptomScreeningRepository;
            }
        }
        #endregion

        //PARTOGRAPH

        #region FetalHeartRatesRepository

        private IFetalHeartRatesRepository fetalHeartRatesRepository;

        public IFetalHeartRatesRepository FetalHeartRatesRepository
        {
            get
            {
                if (fetalHeartRatesRepository == null)
                    fetalHeartRatesRepository = new FetalHeartRatesRepository(context);

                return fetalHeartRatesRepository;
            }
        }

        #endregion FetalHeartRatesRepository

        #region LiquorsRepository

        private ILiquorsRepository liquorsRepository;

        public ILiquorsRepository LiquorsRepository
        {
            get
            {
                if (liquorsRepository == null)
                    liquorsRepository = new LiquorsRepository(context);

                return liquorsRepository;
            }
        }

        #endregion LiquorsRepository

        #region MouldingsRepository

        private IMouldingsRepository mouldingsRepository;

        public IMouldingsRepository MouldingsRepository
        {
            get
            {
                if (mouldingsRepository == null)
                    mouldingsRepository = new MouldingsRepository(context);

                return mouldingsRepository;
            }
        }

        #endregion MouldingsRepository

        #region CervixRepository

        private ICervixRepository crvixRepository;

        public ICervixRepository CervixRepository
        {
            get
            {
                if (crvixRepository == null)
                    crvixRepository = new CervixRepository(context);

                return crvixRepository;
            }
        }

        #endregion CervixRepository

        #region DescentOfHeadsRepository

        private IDescentOfHeadsRepository descentOfHeadsRepository;

        public IDescentOfHeadsRepository DescentOfHeadsRepository
        {
            get
            {
                if (descentOfHeadsRepository == null)
                    descentOfHeadsRepository = new DescentOfHeadsRepository(context);

                return descentOfHeadsRepository;
            }
        }

        #endregion DescentOfHeadsRepository

        #region ContractionsRepository

        private IContractionsRepository contractionsRepository;

        public IContractionsRepository ContractionsRepository
        {
            get
            {
                if (contractionsRepository == null)
                    contractionsRepository = new ContractionsRepository(context);

                return contractionsRepository;
            }
        }

        #endregion ContractionsRepository

        #region OxytocinRepository

        private IOxytocinRepository oxytocinRepository;

        public IOxytocinRepository OxytocinRepository
        {
            get
            {
                if (oxytocinRepository == null)
                    oxytocinRepository = new OxytocinRepository(context);

                return oxytocinRepository;
            }
        }

        #endregion OxytocinRepository

        #region DropsRepository

        private IDropsRepository dropsRepository;

        public IDropsRepository DropsRepository
        {
            get
            {
                if (dropsRepository == null)
                    dropsRepository = new DropsRepository(context);

                return dropsRepository;
            }
        }

        #endregion DropsRepository

        #region MedicinesRepository

        private IMedicinesRepository medicinesRepository;

        public IMedicinesRepository MedicinesRepository
        {
            get
            {
                if (medicinesRepository == null)
                    medicinesRepository = new MedicinesRepository(context);

                return medicinesRepository;
            }
        }

        #endregion MedicinesRepository


        #region MedicineBrandRepository

        private IMedicineBrandRepository medicineBrandRepository;

        public IMedicineBrandRepository MedicineBrandRepository
        {
            get
            {
                if (medicineBrandRepository == null)
                    medicineBrandRepository = new MedicineBrandRepository(context);

                return medicineBrandRepository;
            }
        }
        #endregion MedicineBrandRepository


        #region MedicineManufactureRepository

        private IMedicineManufactureRepository medicineManufactureRepository;

        public IMedicineManufactureRepository MedicineManufactureRepository
        {
            get
            {
                if (medicineManufactureRepository == null)
                    medicineManufactureRepository = new MedicineManufactureRepository(context);

                return medicineManufactureRepository;
            }
        }
        #endregion MedicineManufactureRepository


        #region StoppingReasonRepository

        private IStoppingReasonRepository stoppingReasonRepository;

        public IStoppingReasonRepository StoppingReasonRepository
        {
            get
            {
                if (stoppingReasonRepository == null)
                    stoppingReasonRepository = new StoppingReasonRepository(context);

                return stoppingReasonRepository;
            }
        }
        #endregion StoppingReasonRepository



        #region RiskRepository

        private IRiskRepository riskRepository;

        public IRiskRepository RiskRepository
        {
            get
            {
                if (riskRepository == null)
                    riskRepository = new RiskRepository(context);

                return riskRepository;
            }
        }
        #endregion RiskRepository


        #region PulseRepository

        private IPulseRepository pulseRepository;

        public IPulseRepository PulseRepository
        {
            get
            {
                if (pulseRepository == null)
                    pulseRepository = new PulseRepository(context);

                return pulseRepository;
            }
        }

        #endregion PulseRepository

        #region BloodPressureRepository

        private IBloodPressureRepository bloodPressureRepository;

        public IBloodPressureRepository BloodPressureRepository
        {
            get
            {
                if (bloodPressureRepository == null)
                    bloodPressureRepository = new BloodPressureRepository(context);

                return bloodPressureRepository;
            }
        }

        #endregion BloodPressureRepository

        #region TemperaturesRepository

        private ITemperaturesRepository temperaturesRepository;

        public ITemperaturesRepository TemperaturesRepository
        {
            get
            {
                if (temperaturesRepository == null)
                    temperaturesRepository = new TemperaturesRepository(context);

                return temperaturesRepository;
            }
        }

        #endregion TemperaturesRepository

        #region ProteinsRepository

        private IProteinsRepository proteinsRepository;

        public IProteinsRepository ProteinsRepository
        {
            get
            {
                if (proteinsRepository == null)
                    proteinsRepository = new ProteinsRepository(context);

                return proteinsRepository;
            }
        }

        #endregion ProteinsRepository

        #region AcetonesRepository

        private IAcetonesRepository acetonesRepository;

        public IAcetonesRepository AcetonesRepository
        {
            get
            {
                if (acetonesRepository == null)
                    acetonesRepository = new AcetoneRepository(context);

                return acetonesRepository;
            }
        }

        #endregion AcetonesRepository

        #region VolumesRepository

        private IVolumesRepository volumesRepository;

        public IVolumesRepository VolumesRepository
        {
            get
            {
                if (volumesRepository == null)
                    volumesRepository = new VolumesRepository(context);

                return volumesRepository;
            }
        }
        #endregion VolumesRepository

        #region PartographDetailsRepository
        private IPartographDetailsRepository? partographDetailsRepository;
        public IPartographDetailsRepository PartographDetailsRepository
        {
            get
            {
                if (partographDetailsRepository == null)
                    partographDetailsRepository = new PartographDetailsRepository(context);

                return partographDetailsRepository;
            }
        }
        #endregion PartographDetailsRepository

        #region PartographRepository

        private IPartographRepository partographRepository;

        public IPartographRepository PartographRepository
        {
            get
            {
                if (partographRepository == null)
                    partographRepository = new PartographRepository(context);

                return partographRepository;
            }
        }

        #endregion PartographRepository

        #region BirthDetailRepository

        private IBirthDetailRepository birthDetailRepository;

        public IBirthDetailRepository BirthDetailRepository
        {
            get
            {
                if (birthDetailRepository == null)
                    birthDetailRepository = new BirthDetailRepository(context);

                return birthDetailRepository;
            }
        }
        #endregion

        //DEATH RECORD MODULE:

        #region DeathRecordRepository
        private IDeathRecordRepository deathRecordRepository;
        public IDeathRecordRepository DeathRecordRepository
        {
            get
            {
                if (deathRecordRepository == null)
                    deathRecordRepository = new DeathRecordRepository(context);

                return deathRecordRepository;
            }
        }
        #endregion

        #region DeathCauseRepository
        private IDeathCauseRepository deathCauseRepository;
        public IDeathCauseRepository DeathCauseRepository
        {
            get
            {
                if (deathCauseRepository == null)
                    deathCauseRepository = new DeathCauseRepository(context);

                return deathCauseRepository;
            }
        }
        #endregion

        #region AnatomicAxisRepository
        private IAnatomicAxisRepository anatomicAxisRepository;
        public IAnatomicAxisRepository AnatomicAxisRepository
        {
            get
            {
                if (anatomicAxisRepository == null)
                    anatomicAxisRepository = new AnatomicAxisRepository(context);

                return anatomicAxisRepository;
            }
        }
        #endregion

        #region PathologyAxisRepository
        private IPathologyAxisRepository pathologyAxisRepository;
        public IPathologyAxisRepository PathologyAxisRepository
        {
            get
            {
                if (pathologyAxisRepository == null)
                    pathologyAxisRepository = new PathologyAxisRepository(context);

                return pathologyAxisRepository;
            }
        }
        #endregion

        #region ICPC2DescriptionRepository
        private IICPC2DescriptionRepository iCPC2DescriptionRepository;
        public IICPC2DescriptionRepository ICPC2DescriptionRepository
        {
            get
            {
                if (iCPC2DescriptionRepository == null)
                    iCPC2DescriptionRepository = new ICPC2DescriptionRepository(context);

                return iCPC2DescriptionRepository;
            }
        }
        #endregion

        // PHARMACY
        #region DrugClassRepository
        private IDrugClassRepository drugClassRepository;
        public IDrugClassRepository DrugClassRepository
        {
            get
            {
                if (drugClassRepository == null)
                    drugClassRepository = new DrugClassRepository(context);

                return drugClassRepository;
            }
        }
        #endregion

        #region DrugSubclassRepository
        private IDrugSubclassRepository drugSubclassRepository;
        public IDrugSubclassRepository DrugSubclassRepository
        {
            get
            {
                if (drugSubclassRepository == null)
                    drugSubclassRepository = new DrugSubclassRepository(context);

                return drugSubclassRepository;
            }
        }
        #endregion

        #region GenericDrugRepository
        private IGenericDrugRepository genericDrugRepository;
        public IGenericDrugRepository GenericDrugRepository
        {
            get
            {
                if (genericDrugRepository == null)
                    genericDrugRepository = new GenericDrugRepository(context);

                return genericDrugRepository;
            }
        }
        #endregion

        #region Drug Route
        private IDrugRouteRepository drugRouteRepository;
        public IDrugRouteRepository DrugRouteRepository
        {
            get
            {
                if (drugRouteRepository == null)
                    drugRouteRepository = new DrugRouteRepository(context);

                return drugRouteRepository;
            }
        }
        #endregion

        #region Drug Route
        private IGynConfirmationRepository gynConfirmationRepository;
        public IGynConfirmationRepository GynConfirmationRepository
        {
            get
            {
                if (gynConfirmationRepository == null)
                    gynConfirmationRepository = new GynConfirmationRepository(context);

                return gynConfirmationRepository;
            }
        }
        #endregion

        #region Drug Definition
        private IDrugDefinitionRepository drugDefinitionRepository;
        public IDrugDefinitionRepository DrugDefinitionRepository
        {
            get
            {
                if (drugDefinitionRepository == null)
                    drugDefinitionRepository = new DrugDefinitionRepository(context);

                return drugDefinitionRepository;
            }
        }
        #endregion

        #region Drug Regimen
        private IDrugRegimenRepository drugRegimenRepository;
        public IDrugRegimenRepository DrugRegimenRepository
        {
            get
            {
                if (drugRegimenRepository == null)
                    drugRegimenRepository = new DrugRegimenRepository(context);

                return drugRegimenRepository;
            }
        }
        #endregion

        #region System Relevance
        private ISystemRelevanceRepository systemRelevanceRepository;

        public ISystemRelevanceRepository SystemRelevanceRepository
        {
            get
            {
                if (systemRelevanceRepository == null)
                    systemRelevanceRepository = new SystemRelevanceRepository(context);

                return systemRelevanceRepository;
            }
        }
        #endregion

        #region Prescription 
        private IPrescriptionRepository prescriptionRepository;

        public IPrescriptionRepository PrescriptionRepository
        {
            get
            {
                if (prescriptionRepository == null)
                    prescriptionRepository = new PrescriptionRepository(context);

                return prescriptionRepository;
            }
        }
        #endregion

        #region Medication 
        private IMedicationRepository medicationRepository;

        public IMedicationRepository MedicationRepository
        {
            get
            {
                if (medicationRepository == null)
                    medicationRepository = new MedicationRepository(context);

                return medicationRepository;
            }
        }
        #endregion

        #region Frequency Interval 
        private IFrequencyIntervalRepository frequencyIntervalRepository;

        public IFrequencyIntervalRepository FrequencyIntervalRepository
        {
            get
            {
                if (frequencyIntervalRepository == null)
                    frequencyIntervalRepository = new FrequencyIntervalRepository(context);

                return frequencyIntervalRepository;
            }
        }
        #endregion

        #region Drug Utility 
        private IDrugUtilityRepository drugUtilityRepository;

        public IDrugUtilityRepository DrugUtilityRepository
        {
            get
            {
                if (drugUtilityRepository == null)
                    drugUtilityRepository = new DrugUtilityRepository(context);

                return drugUtilityRepository;
            }
        }
        #endregion

        #region DrugDosageUnit
        private IDrugDosageUnitRepository drugDosageUnitRepository;

        public IDrugDosageUnitRepository DrugDosageUnitRepository
        {
            get
            {
                if (drugDosageUnitRepository == null)
                    drugDosageUnitRepository = new DrugDosageUnitRepository(context);

                return drugDosageUnitRepository;
            }
        }
        #endregion

        #region DrugFormulation
        private IDrugFormulationRepository drugFormulationRepository;

        public IDrugFormulationRepository DrugFormulationRepository
        {
            get
            {
                if (drugFormulationRepository == null)
                    drugFormulationRepository = new DrugFormulationRepository(context);

                return drugFormulationRepository;
            }
        }
        #endregion

        #region Special Drug 
        private ISpecialDrugRepository specialDrugRepository;

        public ISpecialDrugRepository SpecialDrugRepository
        {
            get
            {
                if (specialDrugRepository == null)
                    specialDrugRepository = new SpecialDrugRepository(context);

                return specialDrugRepository;
            }
        }
        #endregion

        #region Dispense 
        private IDispenseRepository dispenseRepository;

        public IDispenseRepository DispenseRepository
        {
            get
            {
                if (dispenseRepository == null)
                    dispenseRepository = new DispenseRepository(context);

                return dispenseRepository;
            }
        }
        #endregion

        #region PregnencyBooking
        private IPregnencyBookingRepository bookingRepository;
        public IPregnencyBookingRepository PregnencyBookingRepository
        {
            get
            {
                if (bookingRepository == null)
                    bookingRepository = new PregnencyBookingRepository(context);

                return bookingRepository;
            }
        }
        #endregion

        #region IdentifiedPregnencyConfirmationRepository
        private IIdentifiedPregnancyConfirmationRepository identifiedPregnancyConfirmationRepository;
        public IIdentifiedPregnancyConfirmationRepository IdentifiedPregnancyConfirmationRepository
        {
            get
            {
                if (identifiedPregnancyConfirmationRepository == null)
                    identifiedPregnancyConfirmationRepository = new IdentifiedPregnancyConfirmationRepository(context);

                return identifiedPregnancyConfirmationRepository;
            }
        }
        #endregion

        //UNDER FIVE MODULE:

        #region Nutrition
        private INutritionRepository nutritionRepository;

        public INutritionRepository NutritionRepository
        {
            get
            {
                if (nutritionRepository == null)
                    nutritionRepository = new NutritionRepository(context);

                return nutritionRepository;
            }
        }
        #endregion

        #region FeedingHistory
        private IFeedingHistoryRepository feedingHistoryRepository;

        public IFeedingHistoryRepository FeedingHistoryRepository
        {
            get
            {
                if (feedingHistoryRepository == null)
                    feedingHistoryRepository = new FeedingHistoryRepository(context);

                return feedingHistoryRepository;
            }
        }
        #endregion

        #region CounsellingService
        private ICounsellingServiceRepository counsellingServiceRepository;

        public ICounsellingServiceRepository CounsellingServiceRepository
        {
            get
            {
                if (counsellingServiceRepository == null)
                    counsellingServiceRepository = new CounsellingServiceRepository(context);

                return counsellingServiceRepository;
            }
        }
        #endregion

        // ART

        #region Attached Facility
        private IAttachedFacilityRepository attachedFacilityRepository;

        public IAttachedFacilityRepository AttachedFacilityRepository
        {
            get
            {
                if (attachedFacilityRepository == null)
                    attachedFacilityRepository = new AttachedFacilityRepository(context);

                return attachedFacilityRepository;
            }
        }
        #endregion

        #region ART Register
        private IARTRegisterRepository aRTRegisterRepository;

        public IARTRegisterRepository ARTRegisterRepository
        {
            get
            {
                if (aRTRegisterRepository == null)
                    aRTRegisterRepository = new ARTRegisterRepository(context);

                return aRTRegisterRepository;
            }
        }
        #endregion

        #region Family Member
        private IFamilyMemberRepository familyMemberRepository;

        public IFamilyMemberRepository FamilyMemberRepository
        {
            get
            {
                if (familyMemberRepository == null)
                    familyMemberRepository = new FamilyMemberRepository(context);

                return familyMemberRepository;
            }
        }
        #endregion

        #region Patient Status
        private IPatientStatusRepository patientStatusRepository;

        public IPatientStatusRepository PatientStatusRepository
        {
            get
            {
                if (patientStatusRepository == null)
                    patientStatusRepository = new PatientStatusRepository(context);

                return patientStatusRepository;
            }
        }
        #endregion

        #region ARTResponse
        private IARTResponseRepository artResponseRepository;

        public IARTResponseRepository ARTResponseRepository
        {
            get
            {
                if (artResponseRepository == null)
                    artResponseRepository = new ARTResponseRepository(context);

                return artResponseRepository;
            }
        }
        #endregion

        #region PriorARTExposer
        private IPriorARTExposerRepository priorARTExposerRepository;

        public IPriorARTExposerRepository PriorARTExposerRepository
        {
            get
            {
                if (priorARTExposerRepository == null)
                    priorARTExposerRepository = new PriorARTExposerRepository(context);

                return priorARTExposerRepository;
            }
        }
        #endregion

        #region TakenARTDrug
        private ITakenARTDrugRepository takenARTDrugRepository;

        public ITakenARTDrugRepository TakenARTDrugRepository
        {
            get
            {
                if (takenARTDrugRepository == null)
                    takenARTDrugRepository = new TakenARTDrugRepository(context);

                return takenARTDrugRepository;
            }
        }
        #endregion

        #region ARTDrug
        private IARTDrugRepository artDrugRepository;

        public IARTDrugRepository ARTDrugRepository
        {
            get
            {
                if (artDrugRepository == null)
                    artDrugRepository = new ARTDrugRepository(context);

                return artDrugRepository;
            }
        }
        #endregion

        #region ARTDrugClass
        private IARTDrugClassRepository artDrugClassRepository;

        public IARTDrugClassRepository ARTDrugClassRepository
        {
            get
            {
                if (artDrugClassRepository == null)
                    artDrugClassRepository = new ARTDrugClassRepository(context);

                return artDrugClassRepository;
            }
        }
        #endregion

        #region TPTDrug
        private ITPTDrugRepository tptDrugRepository;

        public ITPTDrugRepository TPTDrugRepository
        {
            get
            {
                if (tptDrugRepository == null)
                    tptDrugRepository = new TPTDrugRepository(context);

                return tptDrugRepository;
            }
        }
        #endregion

        #region TakenTPTDrug
        private ITakenTPTDrugRepository takenTPTDrugRepository;

        public ITakenTPTDrugRepository TakenTPTDrugRepository
        {
            get
            {
                if (takenTPTDrugRepository == null)
                    takenTPTDrugRepository = new TakenTPTDrugRepository(context);

                return takenTPTDrugRepository;
            }
        }
        #endregion

        #region UseTBIdentification
        private IUseTBIdentificationMethodRepository useTBIdentificationRepository;

        public IUseTBIdentificationMethodRepository UseTBIdentificationMethodRepository
        {
            get
            {
                if (useTBIdentificationRepository == null)
                    useTBIdentificationRepository = new UseTBIdentificationMethodRepository(context);

                return useTBIdentificationRepository;
            }
        }
        #endregion

        #region TBIdentificationMethod
        private ITBIdentificationMethodRepository tBIdentificationMethodRepository;

        public ITBIdentificationMethodRepository TBIdentificationMethodRepository
        {
            get
            {
                if (tBIdentificationMethodRepository == null)
                    tBIdentificationMethodRepository = new TBIdentificationMethodRepository(context);

                return tBIdentificationMethodRepository;
            }
        }
        #endregion

        #region ARTDrugAdherence
        private IARTDrugAdherenceRepository aRTDrugAdherenceRepository;

        public IARTDrugAdherenceRepository ARTDrugAdherenceRepository
        {
            get
            {
                if (aRTDrugAdherenceRepository == null)
                    aRTDrugAdherenceRepository = new ARTDrugAdherenceRepository(context);

                return aRTDrugAdherenceRepository;
            }
        }
        #endregion

        #region TBHistories
        private ITBHistoryRepository tBHistoryRepository;

        public ITBHistoryRepository TBHistoryRepository
        {
            get
            {
                if (tBHistoryRepository == null)
                    tBHistoryRepository = new TBHistoryRepository(context);

                return tBHistoryRepository;
            }
        }
        #endregion

        #region TPTHistories
        private ITPTHistoryRepository tPTHistoryRepository;

        public ITPTHistoryRepository TPTHistoryRepository
        {
            get
            {
                if (tPTHistoryRepository == null)
                    tPTHistoryRepository = new TPTHistoryRepository(context);

                return tPTHistoryRepository;
            }
        }
        #endregion

        #region TBDrug
        private ITBDrugRepository tBDrugRepository;

        public ITBDrugRepository TBDrugRepository
        {
            get
            {
                if (tBDrugRepository == null)
                    tBDrugRepository = new TBDrugRepository(context);

                return tBDrugRepository;
            }
        }
        #endregion

        #region TakenTBDrug
        private ITakenTBDrugRepository takenTBDrugRepository;

        public ITakenTBDrugRepository TakenTBDrugRepository
        {
            get
            {
                if (takenTBDrugRepository == null)
                    takenTBDrugRepository = new TakenTBDrugRepository(context);

                return takenTBDrugRepository;
            }
        }
        #endregion

        #region ServiceQueues
        private IServiceQueueRepository serviceQueueRepository;

        public IServiceQueueRepository ServiceQueueRepository
        {
            get
            {
                if (serviceQueueRepository == null)
                    serviceQueueRepository = new ServiceQueueRepository(context);

                return serviceQueueRepository;
            }
        }
        #endregion

        #region Dots
        private IDotRepository dotRepository;

        public IDotRepository DotRepository
        {
            get
            {
                if (dotRepository == null)
                    dotRepository = new DotRepository(context);

                return dotRepository;
            }
        }
        private IDotCalenderRepository dotCalenderRepository;

        public IDotCalenderRepository DotCalenderRepository
        {
            get
            {
                if (dotCalenderRepository == null)
                    dotCalenderRepository = new DotCalenderRepository(context);

                return dotCalenderRepository;
            }
        }
        #endregion

        #region IdentifiedTBFindings
        private IIdentifiedTBFindingRepository identifiedTBFindingRepository;

        public IIdentifiedTBFindingRepository IdentifiedTBFindingRepository
        {
            get
            {
                if (identifiedTBFindingRepository == null)
                    identifiedTBFindingRepository = new IdentifiedTBFindingRepository(context);

                return identifiedTBFindingRepository;
            }
        }
        #endregion

        #region TBFindings
        private ITBFindingRepository tBFindingRepository;

        public ITBFindingRepository TBFindingRepository
        {
            get
            {
                if (tBFindingRepository == null)
                    tBFindingRepository = new TBFindingRepository(context);

                return tBFindingRepository;
            }
        }
        #endregion

        #region TBSuspectingReasons
        private ITBSuspectingReasonRepository tBSuspectingReasonRepository;

        public ITBSuspectingReasonRepository TBSuspectingReasonRepository
        {
            get
            {
                if (tBSuspectingReasonRepository == null)
                    tBSuspectingReasonRepository = new TBSuspectingReasonRepository(context);

                return tBSuspectingReasonRepository;
            }
        }
        #endregion

        #region WHOClinicalStages
        private IWHOClinicalStageRepository wHOClinicalStageRepository;

        public IWHOClinicalStageRepository WHOClinicalStageRepository
        {
            get
            {
                if (wHOClinicalStageRepository == null)
                    wHOClinicalStageRepository = new WHOClinicalStageRepository(context);

                return wHOClinicalStageRepository;
            }
        }
        #endregion

        #region WHOStagesConditions
        private IWHOStagesConditionRepository wHOStagesConditionRepository;

        public IWHOStagesConditionRepository WHOStagesConditionRepository
        {
            get
            {
                if (wHOStagesConditionRepository == null)
                    wHOStagesConditionRepository = new WHOStagesConditionRepository(context);

                return wHOStagesConditionRepository;
            }
        }
        #endregion

        #region IdentifiedReasonRepository
        private IIdentifiedReasonRepository identifiedReasonRepository;

        public IIdentifiedReasonRepository IdentifiedReasonRepository
        {
            get
            {
                if (identifiedReasonRepository == null)
                    identifiedReasonRepository = new IdentifiedReasonRepository(context);

                return identifiedReasonRepository;
            }
        }
        #endregion

        #region VMMCServiceRepository
        private IVMMCServiceRepository vMMCServiceRepository;

        public IVMMCServiceRepository VMMCServiceRepository
        {
            get
            {
                if (vMMCServiceRepository == null)
                    vMMCServiceRepository = new VMMCServiceRepository(context);

                return vMMCServiceRepository;
            }
        }
        #endregion

        #region OptedCircumcisionReasonRepository
        private IOptedCircumcisionReasonRepository optedCircumcisionReasonRepository;

        public IOptedCircumcisionReasonRepository OptedCircumcisionReasonRepository
        {
            get
            {
                if (optedCircumcisionReasonRepository == null)
                    optedCircumcisionReasonRepository = new OptedCircumcisionReasonRepository(context);

                return optedCircumcisionReasonRepository;
            }
        }
        #endregion

        #region CircumcisionReasonRepository
        private ICircumcisionReasonRepository circumcisionReasonRepository;

        public ICircumcisionReasonRepository CircumcisionReasonRepository
        {
            get
            {
                if (circumcisionReasonRepository == null)
                    circumcisionReasonRepository = new CircumcisionReasonRepository(context);

                return circumcisionReasonRepository;
            }
        }
        #endregion

        #region VMMCCampaignRepository
        private IVMMCCampaignRepository vMMCCampaignRepository;

        public IVMMCCampaignRepository VMMCCampaignRepository
        {
            get
            {
                if (vMMCCampaignRepository == null)
                    vMMCCampaignRepository = new VMMCCampaignRepository(context);

                return vMMCCampaignRepository;
            }
        }
        #endregion

        #region IOptedVMMCCampaignRepository
        private IOptedVMMCCampaignRepository optedVMMCCampaignRepository;

        public IOptedVMMCCampaignRepository OptedVMMCCampaignRepository
        {
            get
            {
                if (optedVMMCCampaignRepository == null)
                    optedVMMCCampaignRepository = new OptedVMMCCampaignRepository(context);

                return optedVMMCCampaignRepository;
            }
        }
        #endregion

        #region TBServiceRepository
        private ITBServiceRepository tbServiceRepository;

        public ITBServiceRepository TBServiceRepository
        {
            get
            {
                if (tbServiceRepository == null)
                    tbServiceRepository = new TBServiceRepository(context);

                return tbServiceRepository;
            }
        }
        #endregion

        #region WHOConditionRepository
        private IWHOConditionRepository wHOConditionRepository;

        public IWHOConditionRepository WHOConditionRepository
        {
            get
            {
                if (wHOConditionRepository == null)
                    wHOConditionRepository = new WHOConditionRepository(context);

                return wHOConditionRepository;
            }
        }
        #endregion

        #region DSDAssesmentRepository
        private IDSDAssesmentRepository dSDAssesmentRepository;

        public IDSDAssesmentRepository DSDAssesmentRepository
        {
            get
            {
                if (dSDAssesmentRepository == null)
                    dSDAssesmentRepository = new DSDAssesmentRepository(context);

                return dSDAssesmentRepository;
            }
        }
        #endregion

        #region DisabilityRepository
        private IDisabilityRepository disabilityRepository;

        public IDisabilityRepository DisabilityRepository
        {
            get
            {
                if (disabilityRepository == null)
                    disabilityRepository = new DisabilityRepository(context);

                return disabilityRepository;
            }
        }
        #endregion

        #region ClientsDisabilityRepository
        private IClientsDisabilityRepository clientsDisabilityRepository;

        public IClientsDisabilityRepository ClientsDisabilityRepository
        {
            get
            {
                if (clientsDisabilityRepository == null)
                    clientsDisabilityRepository = new ClientsDisabilityRepository(context);

                return clientsDisabilityRepository;
            }
        }
        #endregion

        #region VisitPuposeRepository
        private IVisitPuposeRepository visitPuposeRepository;

        public IVisitPuposeRepository VisitPuposeRepository
        {
            get
            {
                if (visitPuposeRepository == null)
                    visitPuposeRepository = new VisitPuposeRepository(context);

                return visitPuposeRepository;
            }
        }
        #endregion

        #region ARTTreatmentPlanRepository
        private IARTTreatmentPlanRepository aRTTreatmentPlanRepository;

        public IARTTreatmentPlanRepository ARTTreatmentPlan
        {
            get
            {
                if (aRTTreatmentPlanRepository == null)
                    aRTTreatmentPlanRepository = new ARTTreatmentPlanRepository(context);

                return aRTTreatmentPlanRepository;
            }
        }
        #endregion

        #region PMTCT
        private IPMTCTRepository pmtctRepository;
        public IPMTCTRepository PMTCTRepository
        {
            get
            {
                if (pmtctRepository == null)
                    pmtctRepository = new PMTCTRepository(context);

                return pmtctRepository;
            }
        }
        #endregion

        #region ComplicationRepository
        private IComplicationRepository complicationRepository;
        public IComplicationRepository ComplicationRepository
        {
            get
            {
                if (complicationRepository == null)
                    complicationRepository = new ComplicationRepository(context);

                return complicationRepository;
            }
        }
        #endregion

        #region ComplicationTypeRepository
        private IComplicationTypeRepository complicationTypeRepository;
        public IComplicationTypeRepository ComplicationTypeRepository
        {
            get
            {
                if (complicationTypeRepository == null)
                    complicationTypeRepository = new ComplicationTypeRepository(context);

                return complicationTypeRepository;
            }
        }
        #endregion

        #region IdentifiedComplicationRepository
        private IIdentifiedComplicationRepository identifiedComplicationRepository;
        public IIdentifiedComplicationRepository IdentifiedComplicationRepository
        {
            get
            {
                if (identifiedComplicationRepository == null)
                    identifiedComplicationRepository = new IdentifiedComplicationRepository(context);

                return identifiedComplicationRepository;
            }
        }
        #endregion

        #region AnestheticPlanRepository
        private IAnestheticPlanRepository anestheticPlanRepository;
        public IAnestheticPlanRepository AnestheticPlanRepository
        {
            get
            {
                if (anestheticPlanRepository == null)
                    anestheticPlanRepository = new AnestheticPlanRepository(context);

                return anestheticPlanRepository;
            }
        }
        #endregion

        #region SkinPreparationRepository
        private ISkinPreparationRepository skinPreparationRepository;
        public ISkinPreparationRepository SkinPreparationRepository
        {
            get
            {
                if (skinPreparationRepository == null)
                    skinPreparationRepository = new SkinPreparationRepository(context);

                return skinPreparationRepository;
            }
        }
        #endregion

        #region DrugPickupScheduleRepository
        private IDrugPickupScheduleRepository drugPickupScheduleRepository;
        public IDrugPickupScheduleRepository DrugPickupScheduleRepository
        {
            get
            {
                if (drugPickupScheduleRepository == null)
                    drugPickupScheduleRepository = new DrugPickupScheduleRepository(context);

                return drugPickupScheduleRepository;
            }
        }
        #endregion

        #region ANCService
        private IANCServiceRepository aNCServiceRepository;
        public IANCServiceRepository ANCServiceRepository
        {
            get
            {
                if (aNCServiceRepository == null)
                    aNCServiceRepository = new ANCServiceRepository(context);

                return aNCServiceRepository;
            }
        }
        #endregion

        #region ANCScreening
        private IANCScreeningRepository aNCScreeningRepository;
        public IANCScreeningRepository ANCScreeningRepository
        {
            get
            {
                if (aNCScreeningRepository == null)
                    aNCScreeningRepository = new ANCScreeningRepository(context);

                return aNCScreeningRepository;
            }
        }
        #endregion

        #region MotherDetail
        private IMotherDetailRepository motherDetailRepository;
        public IMotherDetailRepository MotherDetailRepository
        {
            get
            {
                if (motherDetailRepository == null)
                    motherDetailRepository = new MotherDetailRepository(context);

                return motherDetailRepository;
            }
        }
        #endregion

        #region ChildDetail
        private IChildDetailRepository childDetailRepository;
        public IChildDetailRepository ChildDetailRepository
        {
            get
            {
                if (childDetailRepository == null)
                    childDetailRepository = new ChildDetailRepository(context);

                return childDetailRepository;
            }
        }
        #endregion

        #region BloodTransfusionHistory
        private IBloodTransfusionHistoryRepository bloodTransfusionHistoryRepository;
        public IBloodTransfusionHistoryRepository BloodTransfusionHistoryRepository
        {
            get
            {
                if (bloodTransfusionHistoryRepository == null)
                    bloodTransfusionHistoryRepository = new BloodTransfusionHistoryRepository(context);

                return bloodTransfusionHistoryRepository;
            }
        }
        #endregion

        #region VeginalPosition
        private IVaginalPositionRepository vaginalPositionRepository;
        public IVaginalPositionRepository VaginalPositionRepository
        {
            get
            {
                if (vaginalPositionRepository == null)
                    vaginalPositionRepository = new VaginalPositionRepository(context);

                return vaginalPositionRepository;
            }
        }
        #endregion

        #region PregnancyBooking
        private IPregnancyBookingRepository pregnancyBookingRepository;
        public IPregnancyBookingRepository PregnancyBookingRepository
        {
            get
            {
                if (pregnancyBookingRepository == null)
                    pregnancyBookingRepository = new PregnancyBookingRepository(context);

                return pregnancyBookingRepository;
            }
        }
        #endregion

        #region ScreeningAndPrevention
        private IScreeningAndPreventionRepository screeningAndPreventionRepository;
        public IScreeningAndPreventionRepository ScreeningAndPreventionRepository
        {
            get
            {
                if (screeningAndPreventionRepository == null)
                    screeningAndPreventionRepository = new ScreeningAndPreventionRepository(context);

                return screeningAndPreventionRepository;
            }
        }
        #endregion

        #region IdentifiedPriorSensitization
        private IIdentifiedPriorSensitizationRepository identifiedPriorSensitizationRepository;
        public IIdentifiedPriorSensitizationRepository IdentifiedPriorSensitizationRepository
        {
            get
            {
                if (identifiedPriorSensitizationRepository == null)
                    identifiedPriorSensitizationRepository = new IdentifiedPriorSensitizationRepository(context);

                return identifiedPriorSensitizationRepository;
            }
        }
        #endregion

        #region PastAntenatalVisit
        private IPastAntenatalVisitRepository pastAntenatalVisitRepository;
        public IPastAntenatalVisitRepository PastAntenatalVisitRepository
        {
            get
            {
                if (pastAntenatalVisitRepository == null)
                    pastAntenatalVisitRepository = new PastAntenatalVisitRepository(context);

                return pastAntenatalVisitRepository;
            }
        }
        #endregion

        #region PriorSensitization
        private IPriorSensitizationRepository priorSensitizationRepository;
        public IPriorSensitizationRepository PriorSensitizationRepository
        {
            get
            {
                if (priorSensitizationRepository == null)
                    priorSensitizationRepository = new PriorSensitizationRepository(context);

                return priorSensitizationRepository;
            }
        }
        #endregion

        #region ObstetricExamination
        private IObstetricExaminationRepository obstetricExaminationRepository;
        public IObstetricExaminationRepository ObstetricExaminationRepository
        {
            get
            {
                if (obstetricExaminationRepository == null)
                    obstetricExaminationRepository = new ObstetricExaminationRepository(context);

                return obstetricExaminationRepository;
            }
        }
        #endregion

        #region VisitDetail
        private IVisitDetailRepository visitDetailRepository;
        public IVisitDetailRepository VisitDetailRepository
        {
            get
            {
                if (visitDetailRepository == null)
                    visitDetailRepository = new VisitDetailRepository(context);

                return visitDetailRepository;
            }
        }
        #endregion

        #region PelvicAndVaginalExamination
        private IPelvicAndVaginalExaminationRepository pelvicAndVaginalExaminationRepository;
        public IPelvicAndVaginalExaminationRepository PelvicAndVaginalExaminationRepository
        {
            get
            {
                if (pelvicAndVaginalExaminationRepository == null)
                    pelvicAndVaginalExaminationRepository = new PelvicAndVaginalExaminationRepository(context);

                return pelvicAndVaginalExaminationRepository;
            }
        }
        #endregion

        #region IdentifiedEyesAssessment
        private IIdentifiedEyesAssessmentRepository identifiedEyesAssessmentRepository;
        public IIdentifiedEyesAssessmentRepository IdentifiedEyesAssessmentRepository
        {
            get
            {
                if (identifiedEyesAssessmentRepository == null)
                    identifiedEyesAssessmentRepository = new IdentifiedEyesAssessmentRepository(context);

                return identifiedEyesAssessmentRepository;
            }
        }
        #endregion

        #region IdentifiedCordStump
        private IIdentifiedCordStumpRepository identifiedCordStumpRepository;
        public IIdentifiedCordStumpRepository IdentifiedCordStumpRepository
        {
            get
            {
                if (identifiedCordStumpRepository == null)
                    identifiedCordStumpRepository = new IdentifiedCordStumpRepository(context);

                return identifiedCordStumpRepository;
            }
        }
        #endregion

        #region PreferredFeeding
        private IPreferredFeedingRepository preferredFeedingRepository;
        public IPreferredFeedingRepository PreferredFeedingRepository
        {
            get
            {
                if (preferredFeedingRepository == null)
                    preferredFeedingRepository = new PreferredFeedingRepository(context);

                return preferredFeedingRepository;
            }
        }
        #endregion

        #region IdentifiedPreferredFeeding
        private IIdentifiedPreferredFeedingRepository identifiedPreferredFeedingRepository;
        public IIdentifiedPreferredFeedingRepository IdentifiedPreferredFeedingRepository
        {
            get
            {
                if (identifiedPreferredFeedingRepository == null)
                    identifiedPreferredFeedingRepository = new IdentifiedPreferredFeedingRepository(context);

                return identifiedPreferredFeedingRepository;
            }
        }
        #endregion

        #region IdentifiedDeliveryIntervention
        private IIdentifiedDeliveryInterventionRepository identifiedDeliveryInterventionRepository;
        public IIdentifiedDeliveryInterventionRepository IdentifiedDeliveryInterventionRepository
        {
            get
            {
                if (identifiedDeliveryInterventionRepository == null)
                    identifiedDeliveryInterventionRepository = new IdentifiedDeliveryInterventionRepository(context);

                return identifiedDeliveryInterventionRepository;
            }
        }
        #endregion

        #region IdentifiedCurrentDeliveryComplication
        private IIdentifiedCurrentDeliveryComplicationRepository identifiedCurrentDeliveryComplicationRepository;
        public IIdentifiedCurrentDeliveryComplicationRepository IdentifiedCurrentDeliveryComplicationRepository
        {
            get
            {
                if (identifiedCurrentDeliveryComplicationRepository == null)
                    identifiedCurrentDeliveryComplicationRepository = new IdentifiedCurrentDeliveryComplicationRepository(context);

                return identifiedCurrentDeliveryComplicationRepository;
            }
        }
        #endregion

        #region ThirdStageDelivery
        private IThirdStageDeliveryRepository thirdStageDeliveryRepository;
        public IThirdStageDeliveryRepository ThirdStageDeliveryRepository
        {
            get
            {
                if (thirdStageDeliveryRepository == null)
                    thirdStageDeliveryRepository = new ThirdStageDeliveryRepository(context);

                return thirdStageDeliveryRepository;
            }
        }
        #endregion

        #region PPHTreatment
        private IPPHTreatmentRepository pPHTreatmentRepository;
        public IPPHTreatmentRepository PPHTreatmentRepository
        {
            get
            {
                if (pPHTreatmentRepository == null)
                    pPHTreatmentRepository = new PPHTreatmentRepository(context);

                return pPHTreatmentRepository;
            }
        }
        #endregion

        #region ReferralModuleRepository
        private IReferralModuleRepository referralModuleRepository;
        public IReferralModuleRepository ReferralModuleRepository
        {
            get
            {
                if (referralModuleRepository == null)
                    referralModuleRepository = new ReferralModuleRepository(context);

                return referralModuleRepository;
            }
        }
        #endregion

        #region IdentifiedReferralReasonRepository
        private IIdentifiedReferralReasonRepository identifiedReferralReasonRepository;
        public IIdentifiedReferralReasonRepository IdentifiedReferralReasonRepository
        {
            get
            {
                if (identifiedReferralReasonRepository == null)
                    identifiedReferralReasonRepository = new IdentifiedReferralReasonRepository(context);

                return identifiedReferralReasonRepository;
            }
        }
        #endregion

        #region ReasonOfReferralsRepository
        private IReasonsOfReferalRepository reasonsOfReferalRepository;
        public IReasonsOfReferalRepository ReasonsOfReferalRepository
        {
            get
            {
                if (reasonsOfReferalRepository == null)
                    reasonsOfReferalRepository = new ReasonsOfReferalRepository(context);

                return reasonsOfReferalRepository;
            }
        }
        #endregion

        #region MotherDeliverySummaryRepository
        private IMotherDeliverySummaryRepository motherDeliverySummaryRepository;
        public IMotherDeliverySummaryRepository MotherDeliverySummaryRepository
        {
            get
            {
                if (motherDeliverySummaryRepository == null)
                    motherDeliverySummaryRepository = new MotherDeliverySummaryRepository(context);

                return motherDeliverySummaryRepository;
            }
        }
        #endregion

        #region MedicalTreatment
        private IMedicalTreatmentRepository medicalTreatmentRepository;
        public IMedicalTreatmentRepository MedicalTreatmentRepository
        {
            get
            {
                if (medicalTreatmentRepository == null)
                    medicalTreatmentRepository = new MedicalTreatmentRepository(context);

                return medicalTreatmentRepository;
            }
        }
        #endregion

        #region UterusCondition
        private IUterusConditionRepository uterusConditionRepository;
        public IUterusConditionRepository UterusConditionRepository
        {
            get
            {
                if (uterusConditionRepository == null)
                    uterusConditionRepository = new UterusConditionRepository(context);

                return uterusConditionRepository;
            }
        }
        #endregion

        #region PlacentaRemoval
        private IPlacentaRemovalRepository placentaRemovalRepository;
        public IPlacentaRemovalRepository PlacentaRemovalRepository
        {
            get
            {
                if (placentaRemovalRepository == null)
                    placentaRemovalRepository = new PlacentaRemovalRepository(context);

                return placentaRemovalRepository;
            }
        }
        #endregion

        #region PeriuneumIntact
        private IPeriuneumIntactRepository periuneumIntactRepository;
        public IPeriuneumIntactRepository PeriuneumIntactRepository
        {
            get
            {
                if (periuneumIntactRepository == null)
                    periuneumIntactRepository = new PeriuneumIntactRepository(context);

                return periuneumIntactRepository;
            }
        }
        #endregion

        #region IdentifiedPeriuneumIntact
        private IIdentifiedPerineumIntactRepository identifiedPerineumIntactRepository;
        public IIdentifiedPerineumIntactRepository IdentifiedPerineumIntactRepository
        {
            get
            {
                if (identifiedPerineumIntactRepository == null)
                    identifiedPerineumIntactRepository = new IdentifiedPerineumIntactRepository(context);

                return identifiedPerineumIntactRepository;
            }
        }
        #endregion

        #region PresentingPart
        private IPresentingPartRepository presentingPartRepository;
        public IPresentingPartRepository PresentingPartRepository
        {
            get
            {
                if (presentingPartRepository == null)
                    presentingPartRepository = new PresentingPartRepository(context);

                return presentingPartRepository;
            }
        }
        #endregion

        #region Breech
        private IBreechRepository breechRepository;
        public IBreechRepository BreechRepository
        {
            get
            {
                if (breechRepository == null)
                    breechRepository = new BreechRepository(context);

                return breechRepository;
            }
        }
        #endregion

        #region ModeOfDelivery
        private IModeOfDeliveryRepository modeOfDeliveryRepository;
        public IModeOfDeliveryRepository ModeOfDeliveryRepository
        {
            get
            {
                if (modeOfDeliveryRepository == null)
                    modeOfDeliveryRepository = new ModeOfDeliveryRepository(context);

                return modeOfDeliveryRepository;
            }
        }
        #endregion

        #region NeonatalBirthOutcome
        private INeonatalBirthOutcomeRepository neonatalBirthOutcomeRepository;
        public INeonatalBirthOutcomeRepository NeonatalBirthOutcomeRepository
        {
            get
            {
                if (neonatalBirthOutcomeRepository == null)
                    neonatalBirthOutcomeRepository = new NeonatalBirthOutcomeRepository(context);

                return neonatalBirthOutcomeRepository;
            }
        }
        #endregion

        #region CauseOfStillBirth
        private ICauseOfStillBirthRepository causeOfStillBirthRepository;
        public ICauseOfStillBirthRepository CauseOfStillBirthRepository
        {
            get
            {
                if (causeOfStillBirthRepository == null)
                    causeOfStillBirthRepository = new CauseOfStillBirthRepository(context);

                return causeOfStillBirthRepository;
            }
        }
        #endregion

        #region FeedingMethod
        private IFeedingMethodRepository feedingMethodRepository;
        public IFeedingMethodRepository FeedingMethodRepository
        {
            get
            {
                if (feedingMethodRepository == null)
                    feedingMethodRepository = new FeedingMethodRepository(context);

                return feedingMethodRepository;
            }
        }
        #endregion

        #region DischargeMetric
        private IDischargeMetricRepository dischargeMetricRepository;
        public IDischargeMetricRepository DischargeMetricRepository
        {
            get
            {
                if (dischargeMetricRepository == null)
                    dischargeMetricRepository = new DischargeMetricRepository(context);

                return dischargeMetricRepository;
            }
        }
        #endregion

        #region IdentifiedPPHTreatment
        private IIdentifiedPPHTreatmentRepository identifiedPPHTreatmentRepository;
        public IIdentifiedPPHTreatmentRepository IdentifiedPPHTreatmentRepository
        {
            get
            {
                if (identifiedPPHTreatmentRepository == null)
                    identifiedPPHTreatmentRepository = new IdentifiedPPHTreatmentRepository(context);

                return identifiedPPHTreatmentRepository;
            }
        }
        #endregion

        #region IdentifiedPlacentaRemoval
        private IIdentifiedPlacentaRemovalRepository identifiedPlacentaRemovalRepository;
        public IIdentifiedPlacentaRemovalRepository IdentifiedPlacentaRemovalRepository
        {
            get
            {
                if (identifiedPlacentaRemovalRepository == null)
                    identifiedPlacentaRemovalRepository = new IdentifiedPlacentaRemovalRepository(context);

                return identifiedPlacentaRemovalRepository;
            }
        }
        #endregion

        #region CauseOfNeonatalDeath
        private ICauseOfNeonatalDeathRepository causeOfNeonatalDeathRepository;
        public ICauseOfNeonatalDeathRepository CauseOfNeonatalDeathRepository
        {
            get
            {
                if (causeOfNeonatalDeathRepository == null)
                    causeOfNeonatalDeathRepository = new CauseOfNeonatalDeathRepository(context);

                return causeOfNeonatalDeathRepository;
            }
        }
        #endregion

        #region NeonatalDeath
        private INeonatalDeathRepository neonatalDeathRepository;
        public INeonatalDeathRepository NeonatalDeathRepository
        {
            get
            {
                if (neonatalDeathRepository == null)
                    neonatalDeathRepository = new NeonatalDeathRepository(context);

                return neonatalDeathRepository;
            }
        }
        #endregion

        #region NeonatalAbnormality
        private INeonatalAbnormalityRepository neonatalAbnormalityRepository;
        public INeonatalAbnormalityRepository NeonatalAbnormalityRepository
        {
            get
            {
                if (neonatalAbnormalityRepository == null)
                    neonatalAbnormalityRepository = new NeonatalAbnormalityRepository(context);

                return neonatalAbnormalityRepository;
            }
        }
        #endregion

        #region NeonatalInjury
        private INeonatalInjuryRepository neonatalInjuryRepository;
        public INeonatalInjuryRepository NeonatalInjuryRepository
        {
            get
            {
                if (neonatalInjuryRepository == null)
                    neonatalInjuryRepository = new NeonatalInjuryRepository(context);

                return neonatalInjuryRepository;
            }
        }
        #endregion

        #region ApgarScore
        private IApgarScoreRepository apgarScoreRepository;
        public IApgarScoreRepository ApgarScoreRepository
        {
            get
            {
                if (apgarScoreRepository == null)
                    apgarScoreRepository = new ApgarScoreRepository(context);

                return apgarScoreRepository;
            }
        }
        #endregion

        #region NewBornDetail
        private INewBornDetailRepository newBornDetailRepository;
        public INewBornDetailRepository NewBornDetailRepository
        {
            get
            {
                if (newBornDetailRepository == null)
                    newBornDetailRepository = new NewBornDetailRepository(context);

                return newBornDetailRepository;
            }
        }
        #endregion

        #region FamilyPlanRegisterRepository
        private IFamilyPlanRegisterRepository familyPlanRegisterRepository;
        public IFamilyPlanRegisterRepository FamilyPlanRegisterRepository
        {
            get
            {
                if (familyPlanRegisterRepository == null)
                    familyPlanRegisterRepository = new FamilyPlanRegisterRepository(context);

                return familyPlanRegisterRepository;
            }
        }
        #endregion

        #region STIRiskRepository
        private ISTIRiskRepository stiRiskRepository;
        public ISTIRiskRepository STIRiskRepository
        {
            get
            {
                if (stiRiskRepository == null)
                    stiRiskRepository = new STIRiskRepository(context);

                return stiRiskRepository;
            }
        }
        #endregion

        #region PastMedicalConditonRepository
        private IPastMedicalConditonRepository pastMedicalConditonRepository;
        public IPastMedicalConditonRepository PastMedicalConditonRepository
        {
            get
            {
                if (pastMedicalConditonRepository == null)
                    pastMedicalConditonRepository = new PastMedicalConditonRepository(context);

                return pastMedicalConditonRepository;
            }
        }
        #endregion

        #region MedicalConditionRepository
        private IMedicalConditionRepository medicalConditionRepository;
        public IMedicalConditionRepository MedicalConditionRepository
        {
            get
            {
                if (medicalConditionRepository == null)
                    medicalConditionRepository = new MedicalConditionRepository(context);

                return medicalConditionRepository;
            }
        }
        #endregion

        #region FamilyPlan
        private IFamilyPlanRepository familyPlanRepository;
        public IFamilyPlanRepository FamilyPlanRepository
        {
            get
            {
                if (familyPlanRepository == null)
                    familyPlanRepository = new FamilyPlanRepository(context);

                return familyPlanRepository;
            }
        }
        #endregion

        #region QuickExaminationRepository
        private IQuickExaminationRepository quickExaminationRepository;
        public IQuickExaminationRepository QuickExaminationRepository
        {
            get
            {
                if (quickExaminationRepository == null)
                    quickExaminationRepository = new QuickExaminationRepository(context);

                return quickExaminationRepository;
            }
        }
        #endregion

        #region GuidedExaminationRepository
        private IGuidedExaminationRepository guidedExaminationRepository;
        public IGuidedExaminationRepository GuidedExaminationRepository
        {
            get
            {
                if (guidedExaminationRepository == null)
                    guidedExaminationRepository = new GuidedExaminationRepository(context);

                return guidedExaminationRepository;
            }
        }
        #endregion

        #region FamilyPlanningClass
        private IFamilyPlanningClassRepository familyPlanningClassRepository;
        public IFamilyPlanningClassRepository FamilyPlanningClassRepository
        {
            get
            {
                if (familyPlanningClassRepository == null)
                    familyPlanningClassRepository = new FamilyPlanningClassRepository(context);

                return familyPlanningClassRepository;
            }
        }
        #endregion

        #region FamilyPlanningSubClass
        private IFamilyPlanningSubClassRepository familyPlanningSubClassRepository;
        public IFamilyPlanningSubClassRepository FamilyPlanningSubClassRepository
        {
            get
            {
                if (familyPlanningSubClassRepository == null)
                    familyPlanningSubClassRepository = new FamilyPlanningSubClassRepository(context);

                return familyPlanningSubClassRepository;
            }
        }
        #endregion

        #region InsertionAndRemovalProcedure
        private IInsertionAndRemovalProcedureRepository insertionAndRemovalProcedureRepository;
        public IInsertionAndRemovalProcedureRepository InsertionAndRemovalProcedureRepository
        {
            get
            {
                if (insertionAndRemovalProcedureRepository == null)
                    insertionAndRemovalProcedureRepository = new InsertionAndRemovalProcedureRepository(context);

                return insertionAndRemovalProcedureRepository;
            }
        }
        #endregion

        #region FacilityQueue
        private IFacilityQueueRepository facilityQueueRepository;
        public IFacilityQueueRepository FacilityQueueRepository
        {
            get
            {
                if (facilityQueueRepository == null)
                    facilityQueueRepository = new FacilityQueueRepository(context);

                return facilityQueueRepository;
            }
        }
        #endregion

        #region Log Repository
        private ILogRepository logRepository;
        public ILogRepository LogRepository
        {
            get
            {
                if (logRepository == null)
                    logRepository = new LogRepository(configuration);

                return logRepository;
            }
        }
        #endregion

        //DFZ
        #region ArmedForceServiceRepository
        private IArmedForceServiceRepository armedForceServiceRepository;
        public IArmedForceServiceRepository ArmedForceServiceRepository
        {
            get
            {
                if (armedForceServiceRepository == null)
                    armedForceServiceRepository = new ArmedForceServiceRepository(context);

                return armedForceServiceRepository;
            }
        }
        #endregion

        #region patientTypeRepository
        private IPatientTypeRepository patientTypeRepository;
        public IPatientTypeRepository PatientTypeRepository
        {
            get
            {
                if (patientTypeRepository == null)
                    patientTypeRepository = new PatientTypeRepository(context);

                return patientTypeRepository;
            }
        }
        #endregion

        #region DFZRankRepository
        private IDFZRankRepository dFZRankRepository;
        public IDFZRankRepository DFZRankRepository
        {
            get
            {
                if (dFZRankRepository == null)
                    dFZRankRepository = new DFZRankRepository(context);

                return dFZRankRepository;
            }
        }
        #endregion
        #region IPreScreeningVisitRepository
        private IPreScreeningVisitRepository preScreeningVisitRepository;

        public IPreScreeningVisitRepository PreScreeningVisitRepository
        {
            get
            {
                if (preScreeningVisitRepository == null)
                    preScreeningVisitRepository = new PreScreeningVisitRepository(context);

                return preScreeningVisitRepository;
            }
        }
        #endregion
        #region InterCourseStatusRepository
        private IInterCourseStatusRepository interCourseStatusRepository;

        public IInterCourseStatusRepository InterCourseStatusRepository
        {
            get
            {
                if (interCourseStatusRepository == null)
                    interCourseStatusRepository = new InterCourseStatusRepository(context);

                return interCourseStatusRepository;
            }
        }
        #endregion


        #region ThermoAblationTreatmentMethodRepository
        private IThermoAblationTreatmentMethodRepository thermoAblationTreatmentMethodRepository;
        public IThermoAblationTreatmentMethodRepository ThermoAblationTreatmentMethodRepository
        {
            get
            {
                if (thermoAblationTreatmentMethodRepository == null)
                    thermoAblationTreatmentMethodRepository = new ThermoAblationTreatmentMethodRepository(context);

                return thermoAblationTreatmentMethodRepository;
            }
        }
        #endregion

        #region LeepsTreatmentMethodRepository
        private ILeepsTreatmentMethodRepository leepstreatmentmethodRepository;
        public ILeepsTreatmentMethodRepository LeepsTreatmentMethodRepository
        {
            get
            {
                if (leepstreatmentmethodRepository == null)
                    leepstreatmentmethodRepository = new LeepsTreatmentMethodRepository(context);

                return leepstreatmentmethodRepository;
            }
        }
        #endregion

        #region ScreeningRepository
        private IScreeningRepository screeningRepository;
        public IScreeningRepository ScreeningRepository
        {
            get
            {
                if (screeningRepository == null)
                    screeningRepository = new ScreeningRepository(context);

                return screeningRepository;
            }
        }
        #endregion 
        #region ThermoAblationRepository
        private IThermoAblationRepository thermoAblationRepository;
        public IThermoAblationRepository ThermoAblationRepository
        {
            get
            {
                if (thermoAblationRepository == null)
                    thermoAblationRepository = new ThermoAblationRepository(context);

                return thermoAblationRepository;
            }
        }

        #endregion
        #region LeepsRepository
        private ILeepsRepository leepsRepository;
        public ILeepsRepository LeepsRepository
        {
            get
            {
                if (leepsRepository == null)
                    leepsRepository = new LeepsRepository(context);

                return leepsRepository;
            }
        }

        #endregion
        #region CaCXPlanRepository
        private ICaCXPlanRepository caCXPlanRepository;
        public ICaCXPlanRepository CaCXPlanRepository
        {
            get
            {
                if (caCXPlanRepository == null)
                    caCXPlanRepository = new CaCXPlanRepository(context);

                return caCXPlanRepository;
            }
        }

        #endregion
        public IDbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await context.Database.BeginTransactionAsync();
        }

        protected void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }

            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}