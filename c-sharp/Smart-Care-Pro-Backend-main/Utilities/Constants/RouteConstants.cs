/*
 * Created by   : Stephan
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */

namespace Utilities.Constants
{
    /// <summary>
    /// RouteConstants.
    /// </summary>
    public static class RouteConstants
    {
        public const string BaseRoute = "carepro-api";

        // MODULE
        #region UserAccess
        public const string CreateUserAccess = "user-access";

        public const string ReadUserAccesses = "user-accesses";

        public const string ReadUserAccessByKey = "user-access/key/{key}";

        public const string UpdateUserAccess = "user-access/{key}";

        public const string DeleteUserAccess = "user-access/{key}";
        #endregion

        #region UserAccount
        public const string CreateUserAccount = "user-account";

        public const string ReadUserAccounts = "user-accounts";

        public const string ReadUserAccountByKey = "user-account/key/{key}";

        public const string ReadUserAccountByFirstname = "user-account/firstname/{firstName}";

        public const string ReadUserAccountBySurname = "user-account/surname/{surName}";

        public const string ReadUserAccountByCellphone = "user-account/cellphone/{cellphone}";

        public const string ReadUserAccountByUserAccountBasicInfo = "user-account/userAccount-basicInfo";

        public const string UpdateUserAccount = "user-account/{key}";

        public const string DeleteUserAccount = "user-account/{key}";

        public const string UserLogin = "user-account/login";

        public const string GetUserAccessByUserName = "user-account/User-access-by-username/{userName}";

        public const string ReadUserAccessByUserName = "user-account/user-access-by-username/{userName}";

        public const string VerifyPassword = "user-account/verify-password";

        public const string UpdatePassword = "user-account/update-password";

        public const string CheckUserName = "user-account/user-check/{userName}";

        public const string CheckUserNRC = "user-account/user-check-by-nrc";

        public const string CheckUserMobile = "user-account/user-check-by-cell";

        public const string ChangedPassword = "user-account/change-password";

        public const string RecoveryRequest = "user-account/recovery-request";

        public const string UserAccountReadByKey = "user-account/key/";

        public const string UserAccountUpdate = "user-account/";
        #endregion

        #region LoginHistory
        public const string CreateLoginHistory = "login-history";

        public const string ReadLoginHistories = "login-histories";

        public const string ReadLoginHistoryByKey = "login-history/key/{key}";

        public const string UpdateLoginHistory = "login-history/{key}";

        public const string DeleteLoginHistory = "login-history/{key}";
        #endregion

        #region RecoveryRequest
        public const string CreateRecoveryRequest = "recovery-request";

        public const string ReadRecoveryRequests = "recovery-requests";

        public const string ReadRecoveryRequestByKey = "recovery-request/key/{key}";

        public const string ReadRecoveryRequestByDate = "recovery-request/by-date";

        public const string UpdateRecoveryRequest = "recovery-request/{key}";

        public const string DeleteRecoveryRequest = "recovery-request/key";
        #endregion

        #region FacilityAccess
        public const string CreateFacilityAccess = "facility-access";

        public const string MakeAdmin = "facility-access/make-admin/{userAccountId}";

        public const string ReadFacilityAccess = "facility-accesses";

        public const string ReadFacilityAccessForAdmin = "facility-accesses-admin";

        public const string ReadfacilityAccessByKey = "facility-access/key/{key}";

        public const string ReadFacilityAccessWithModulePermissionsByKey = "facility-access-with-module-access/key/{key}";

        public const string UpdateFacilityAccessByUserAccountID = "facility-access/{userAccountId}";

        public const string RevokeLoginByUserAccountID = "facility-access-revoke-login/{userAccountId}";

        public const string ApproveFacilityAccess = "approve-facility-access/{key}";

        public const string LoginRecoveryFacilityAccess = "login-recovery-facility-access";

        public const string RejectFacilityAccess = "reject-facility-access/{key}";

        public const string ReadFacilityAccessByUserAccountId = "facility-access-by-useraccount/{useraccountId}";

        public const string ReadFacilityAccessByFacilityID = "facility-access/facility-access-by-facility/{facilityId}";

        public const string ReadFacilityAccessLoginRequestByFacilityID = "facility-access/facility-access-by-facility/login-request/{facilityId}";

        public const string ReadFacilityAccessApprovedRequestByFacilityID = "facility-access/facility-access-by-facility/approved-request/{facilityId}";

        public const string ReadFacilityAccessRecoveryRequestByFacilityID = "facility-access/facility-access-by-facility/recovery-request/{facilityId}";
        #endregion

        #region ModuleAccess
        public const string CreateModuleAccess = "module-access";

        public const string ReadModuleAccessByFacility = "module-access/module-access-by-facility-access/{facilityAccessId}";
        #endregion

        //CLIENT MODULE
        #region Country
        public const string CreateCountry = "country";

        public const string ReadCountries = "countries";

        public const string ReadCountryByKey = "country/key/{key}";

        public const string UpdateCountry = "country/{key}";

        public const string DeleteCountry = "country/{key}";
        #endregion

        #region Province
        public const string CreateProvince = "province";

        public const string ReadProvinces = "provinces";

        public const string ReadProvincesByUserID = "provinces/userid/{userId}";

        public const string ReadProvinceByKey = "province/key/{key}";

        public const string UpdateProvince = "province/{key}";

        public const string DeleteProvince = "province/{key}";
        #endregion

        #region District
        public const string CreateDistrict = "district";

        public const string ReadDistricts = "districts";

        public const string ReadDistrictByKey = "district/key/{key}";

        public const string UpdateDistrict = "district/{key}";

        public const string DeleteDistrict = "district/{key}";

        public const string DistrictByProvince = "district/district-by-province/{provinceId}";

        public const string DistrictByFacility = "district/district-By-facility/{facilityId}";
        #endregion

        #region Town
        public const string CreateTown = "town";

        public const string ReadTowns = "towns";

        public const string ReadTownByKey = "town/key/{key}";

        public const string UpdateTown = "town/{key}";

        public const string DeleteTown = "town/{key}";

        public const string TownByDistrict = "town/town-by-district/{districtId}";

        public const string ReadTownByDistrict = "town/town-by-district/";
        #endregion

        #region Facility
        public const string CreateFacility = "facility";

        public const string ReadFacilities = "facilities";

        public const string ReadRequestFacilityData = "request-facility-data/{userId}";

        public const string ReadSelectFacilityData = "select-facility-data/{userId}";

        public const string ReadFacilitiesWithPagging = "facilities-with-pagging";

        public const string ReadFacilityByKey = "facility/key/{key}";

        public const string ReadActiveFacility = "active-facilities";

        public const string ReadfacilityByDistrict = "facility/facility-by-district/{districtId}";

        public const string ReadActivefacilityByDistrict = "active/facility-by-district/{districtId}";

        public const string ReadFacilityByFacilityName = "facility/facilityname/{facilityName}";

        public const string UpdateFacility = "facility/{key}";

        public const string DeleteFacility = "facility/{key}";

        public const string facilityByFacilityType = "facility/facility-by-facility-type/{type}";
        #endregion

        #region HomeLanguage
        public const string CreateHomeLanguage = "home-language";

        public const string ReadHomeLanguages = "home-languages";

        public const string ReadHomeLanguageByKey = "home-language/key/{key}";

        public const string UpdateHomeLanguage = "home-language/{key}";

        public const string DeleteHomeLanguage = "home-language/{key}";
        #endregion

        #region EducationLevel
        public const string CreateEducationLevel = "education-level";

        public const string ReadEducationLevels = "education-levels";

        public const string ReadEducationLevelByKey = "education-level/key/{key}";

        public const string UpdateEducationLevel = "education-level/{key}";

        public const string DeleteEducationLevel = "education-level/{key}";
        #endregion

        #region Occupation
        public const string CreateOccupation = "occupation";

        public const string ReadOccupations = "occupations";

        public const string ReadOccupationByKey = "occupation/key/{key}";

        public const string UpdateOccupation = "occupation/{key}";

        public const string DeleteOccupation = "occupation/{key}";
        #endregion

        #region Client
        public const string CreateClient = "client";

        public const string CreateClientWithRelation = "client-with-relation";

        public const string CreateDFZClientRelation = "dfz-client-relation";

        public const string ReadClients = "clients";

        public const string ReadDFZClientsRelations = "dfz-clients-relations/key/{key}";

        public const string ReadDFZClientsByDependentId = "dfz-clients-dependency/dependentid/{id}";

        public const string ReadDFZClientsRelationById = "dfz-cliens-relation-by-id/key/{id}";

        public const string ReadClientByKey = "client/key/{key}";

        public const string ReadClientDetailsForTOPCardBykey = "client-details-for-topcard/key/{key}";

        public const string ReadClientDetailsForRightCardByKey = "client-details-for-rightcard/key/{key}";

        public const string ReadClientByNRC = "client/NRC/{NRC}";

        public const string ReadClientByHospitalNo = "client/hospitalno/{hospitalNo}";

        public const string ReadDeptendentClientByHospitalNo = "dependent-client/hospitalno/{hospitalNo}";

        public const string ReadDFZClientByServiceNo = "dfzclient/serviceno/{serviceNo}";

        public const string ReadClientByServiceNo = "client/serviceno/{serviceNo}";

        public const string ReadClientByNUPN = "client/NUPN/{NUPN}";

        public const string ReadClientByNUPNAndGender = "client/NUPN-Gender/{NUPN}";

        public const string GenerateClientNUPN = "client/NUPN-Generate/{facilityId}";

        public const string ReadClientByCellphone = "client/Cellphone";

        public const string ReadClientByClientBasicInfo = "client/clientbasicinfo";

        public const string ReadClientByClientNoDob = "client/clientnodob";

        public const string UpdateClient = "client/{key}";

        public const string UpdateClientMother = "client/link-mother/{key}";

        public const string UpdateDependentClient = "dependent-client/{key}";

        public const string DeleteClient = "client/{key}";

        public const string ClientReadByKey = "client/key/";

        public const string ReadClientDetailsForTOPCard = "client-details-for-topcard/key/";

        public const string ReadClientDetailsForRightCard = "client-details-for-rightcard/key/";

        public const string ClientUpdate = "client/";

        public const string RemoveDFZClientRelation = "client-remove-dependent/{key}";

        public const string ClientByFacility = "clients/{facilityId}";
        #endregion

        #region Caregiver
        public const string CreateCaregiver = "caregiver";

        public const string ReadCaregivers = "caregivers";

        public const string ReadCaregiverByKey = "caregiver/key/{key}";

        public const string UpdateCaregiver = "caregiver/{key}";

        public const string DeleteCaregiver = "caregiver/{key}";
        #endregion

        #region NextOfKin
        public const string CreateNextOfKin = "next-of-kin";

        public const string ReadNextOfKins = "next-of-kins";

        public const string ReadNextOfKinByKey = "next-of-kin/key/{key}";

        public const string UpdateNextOfKin = "next-of-kin/{key}";

        public const string DeleteNextOfKin = "next-of-kin/{key}";

        public const string ReadNextOfKinByClient = "next-of-kin/byclient/{clientId}";

        #endregion

        //VITAL MODULE
        #region MedicalEncounter
        public const string CreateEncounter = "encounter";

        public const string ReadEncounters = "encounters";

        public const string ReadEncounterByKey = "encounter/key/{key}";

        public const string ReadEncounterByClient = "encounter/client/{key}";

        public const string ReadEncounterListByClient = "encounters/client/{key}";

        public const string ReadAdmissionsByClient = "encounters/addmissions-by-client/{key}";

        public const string ReadIPDAdmissionByClient = "encounters/ipd-addmission-by-client/{key}";

        public const string UpdateEncounter = "encounter/{key}";

        public const string UpdateAdmission = "encounter/admission/{key}";

        public const string DeleteEncounter = "encounter/{key}";
        #endregion

        #region OPDVisit
        public const string CreatOPDVisit = "opd-visit";

        public const string CreatOPDVisitHistory = "opd-visit-history";

        public const string ReadeOPDVisits = "opd-visits";

        public const string ReadeOPDVisitsByDate = "opd-visits-by-date/{date}";

        public const string ReadOPDVisitByKey = "opd-visit/key/key";

        public const string ReadMedicalHistory = "medical-history-medical-encounter/{clientId}";

        public const string ReadOPDVisitByclientIdAndVisitDate = "opd-visit/clientId-visitdate";

        public const string UpdateOPDVisit = "opd-visit/{key}";

        public const string DeleteOPDVisit = "opd-visit/key";
        #endregion

        #region Interaction
        public const string CreateInteraction = "interaction";

        public const string ReadInteractions = "interactions";

        public const string ReadInteractionByKey = "interaction/key/{key}";

        public const string UpdateInteraction = "interaction/{key}";

        public const string DeleteInteraction = "interaction/{key}";
        #endregion

        #region Vital
        public const string CreateVital = "vital";

        public const string ReadVitals = "vitals/{clientId}";

        public const string ReadVitalByKey = "vital/key/{key}";

        public const string ReadVitalByClient = "vital/vital-by-client/{clientId}";

        public const string ReadLatestVitalByClient = "vital/Latest-vital-by-client/{clientId}";

        public const string ReadLatestVitalByClientAndEncounterType = "vital/Latest-vital-by-client-encountertype/{clientId}";

        public const string UpdateVital = "vital/{key}";

        public const string DeleteVital = "vital/{key}";
        #endregion

        //HTS MODULE
        #region HTSReferredTo
        public const string CreateHTSReferredTo = "hts-referred-to";

        public const string ReadHTSReferredTo = "readhts-referred-to";

        public const string ReadHTSReferredToByKey = "hts-referred-to/key/{key}";

        public const string ReadHTSReferredToByHTS = "hts-referred-to/hts-referred-to-by-hts/{HTSId}";

        public const string UpdateHTSReferredTo = "hts-referred-to/{key}";

        public const string DeleteHTSReferredTo = "hts-referred-to/{key}";
        #endregion

        #region HTSReferral
        public const string CreateHTSReferral = "hts-referral";

        public const string ReadHTSReferrals = "hts-referrals";

        public const string ReadHTSReferralByKey = "hts-referral/key/{key}";

        public const string UpdateHTSReferral = "hts-referral/{key}";

        public const string DeleteHTSReferral = "hts-referral/{key}";
        #endregion

        #region HIVNotTestingReason
        public const string CreateHIVNotTestingReason = "hiv-not-testing-reason";

        public const string ReadHIVNotTestingReasons = "hiv-not-testing-reasons";

        public const string ReadHIVNotTestingReasonByKey = "hiv-not-testing-reason/key/{key}";

        public const string UpdateHIVNotTestingReason = "hiv-not-testing-reason/{key}";

        public const string DeleteHIVNotTestingReason = "hiv-not-testing-reason/{key}";
        #endregion

        #region HIVTestingReason
        public const string CreateHIVTestingReason = "hiv-testing-reason";

        public const string ReadHIVTestingReasons = "hiv-testing-reasons";

        public const string ReadHIVTestingReasonByKey = "hiv-testing-reason/key/{key}";

        public const string UpdateHIVTestingReason = "hiv-testing-reason/{key}";

        public const string DeleteHIVTestingReason = "hiv-testing-reason/{key}";
        #endregion

        #region HIVRiskFactor
        public const string CreateHIVRiskFactor = "hiv-risk-factor";

        public const string ReadHIVRiskFactors = "hiv-risk-factors";

        public const string ReadHIVRiskFactorByKey = "hiv-risk-factor/key/{key}";

        public const string UpdateHIVRiskFactor = "hiv-risk-factor/{key}";

        public const string DeleteHIVRiskFactor = "hiv-risk-factor/{key}";
        #endregion

        #region RiskAssessment
        public const string CreateRiskAssessment = "risk-assessment";

        public const string ReadRiskAssessments = "risk-assessments";

        public const string ReadRiskAssessmentByKey = "risk-assessment/key/{key}";

        public const string ReadRiskAssessmentByHTS = "risk-assessment/risk-assessment-by-hts/{HTSId}";

        public const string UpdateRiskAssessment = "risk-assessment/{key}";

        public const string DeleteRiskAssessment = "risk-assessment/{key}";
        #endregion

        #region ServicePoint
        public const string CreateServicePoint = "service-point";

        public const string ReadServicePoints = "service-points";

        public const string ReadServicePointByKey = "service-point/key/{key}";

        public const string UpdateServicePoint = "service-point/{key}";

        public const string DeleteServicePoint = "service-point/{key}";

        public const string ReadServicePointByFacility = "service-point/facilityId/{facilityId}";

        #endregion

        #region ClientType
        public const string CreateClientType = "client-type";

        public const string ReadClientTypes = "client-types";

        public const string ReadClientTypeByKey = "client-type/key/{key}";

        public const string UpdateClientType = "client-type/{key}";

        public const string DeleteClientType = "client-type/{key}";
        #endregion

        #region VisitType
        public const string CreateVisitType = "visit-type";

        public const string ReadVisitTypes = "visit-types";

        public const string ReadVisitTypeByKey = "visit-type/key/{key}";

        public const string UpdateVisitType = "visit-type/{key}";

        public const string DeleteVisitType = "visit-type/{key}";
        #endregion

        #region HTS
        public const string CreateHTS = "hts";

        public const string ReadHTS = "readhts/{clientId}";

        public const string ReadHTSByKey = "hts/key/{key}";

        public const string ReadHTSByClient = "hts/hts-by-client/{clientId}";

        public const string ReadLatestHTSByClient = "hts/Latest-hts-by-client/{clientId}";

        public const string UpdateHTS = "hts/{key}";

        public const string DeleteHTS = "hts/{key}";
        #endregion

        //MEDICAL ENCOUNTER MODULE
        #region ChiefComplaint
        public const string CreateChiefComplaint = "chief-complaint";

        public const string ReadChiefComplaints = "chief-complaints";

        public const string ReadChiefComplaintByKey = "chief-complaint/key/{key}";

        public const string UpdateChiefComplaint = "chief-complaint/{key}";

        public const string ReadChiefComplaintByClient = "chief-complaint/by-Client/{clientId}";

        public const string DeleteChiefComplaint = "chief-complaint/{key}";

        //PEPChiefComplaint
        public const string CreatePEPChiefComplaint = "pep-chief-complaint";

        public const string ReadPEPChiefComplaints = "pep-chief-complaints";

        public const string ReadPEPChiefComplaintByKey = "pep-chief-complaint/key/{key}";

        public const string UpdatePEPChiefComplaint = "pep-chief-complaint/{key}";

        public const string ReadPEPChiefComplaintByClient = "pep-chief-complaint/by-Client/{clientId}";

        public const string DeletePEPChiefComplaint = "pep-chief-complaint/{key}";

        //PrEPChiefComplaint
        public const string CreatePrEPChiefComplaint = "prep-chief-complaint";

        public const string ReadPrEPChiefComplaints = "prep-chief-complaints";

        public const string ReadPrEPChiefComplaintByKey = "prep-chief-complaint/key/{key}";

        public const string UpdatePrEPChiefComplaint = "prep-chief-complaint/{key}";

        public const string RemovePrEPChiefComplaint = "prep-chief-complaint/remove/{interactionId}";

        public const string RemovePrEPKeyPopulation = "prep-key-population/remove/{encounterId}";

        public const string ReadPrEPChiefComplaintByClient = "prep-chief-complaint/by-client/{clientId}";

        public const string ReadANCServiceChiefComplaintByClient = "anc-chief-complaint/by-client/{clientId}";

        public const string DeletePrEPChiefComplaint = "prep-chief-complaint/{key}";

        //IPD Chief complaint
        public const string CreateIPDChiefComplaint = "ipd-chief-complaint";

        public const string UpdateIPDChiefComplaint = "ipd-chief-complaint/{key}";

        public const string ReadChiefComplaintByEncounter = "chief-complaint-by-encounter/{encounterId}";

        #endregion

        #region TBSymptom
        public const string ReadTBSymptoms = "tb-symptoms";
        #endregion

        #region ConstitutionalSymptoms
        public const string ReadConstitutionalSymptoms = "constitutional-symptoms";

        public const string CreateConstitutionalSymptom = "constitutional-symptom";

        public const string ReadConstitutionalSymptomByKey = "constitutional-symptom/key/{key}";

        public const string UpdateConstitutionalSymptom = "constitutional-symptom/{key}";

        public const string DeleteConstitutionalSymptom = "constitutional-symptom/{key}";
        #endregion

        #region ConstitutionalSymptomType
        public const string ReadConstitutionalSymptomTypes = "constitutional-symptom-types";

        public const string ReadConstitutionalSymptomTypesByConstituationalSymptoms = "constitutional-symptom-types-by-symptom/{constitutionalSymptomId}";

 
        public const string CreateConstitutionalSymptomType = "constitutional-symptom-type";

        public const string ReadConstitutionalSymptomTypeByKey = "constitutional-symptom-type/key/{key}";

        public const string UpdateConstitutionalSymptomType = "constitutional-symptom-type/{key}";

        public const string DeleteConstitutionalSymptomType = "constitutional-symptom-type/{key}";
        #endregion

        #region IdentifiedConstitutionalSymptom
        public const string CreateIdentifiedConstitutionalSymptom = "identified-constitutional-symptom";

        public const string ReadIdentifiedConstitutionalSymptoms = "identified-constitutional-symptoms";

        public const string ReadIdentifiedConstitutionalSymptomByKey = "identified-constitutional-symptom/key/{key}";

        public const string UpdateIdentifiedConstitutionalSymptom = "identified-constitutional-symptom/{key}";

        public const string DeleteIdentifiedConstitutionalSymptom = "identified-constitutional-symptom/{key}";

        public const string RemoveIdentifiedConstitutionalSymptom = "identified-constitutional-symptom/remove/{key}";

        public const string ReadIdentifiedConstitutionalSymptomsByClient = "identified-constitutional-symptoms-by-client/{clientId}";
        public const string ReadIdentifiedTBConstitutionalSymptomsByClient = "identified-tb-constitutional-symptoms-by-client/{clientId}";

        public const string ReadIdentifiedConstitutionalSymptomsByEncounterId = "identified-constitutional-symptoms-encounter/{encounterId}";

        //PEP
        public const string CreatePEPIdentifiedConstitutionalSymptom = "pep-identified-constitutional-symptom";

        public const string ReadPEPIdentifiedConstitutionalSymptoms = "pep-identified-constitutional-symptoms";

        public const string ReadPEPIdentifiedConstitutionalSymptomByKey = "pep-identified-constitutional-symptom/key/{key}";

        public const string UpdatePEPIdentifiedConstitutionalSymptom = "pep-identified-constitutional-symptom/{key}";

        public const string DeletePEPIdentifiedConstitutionalSymptom = "pep-identified-constitutional-symptom/{key}";

        public const string RemovePEPIdentifiedConstitutionalSymptom = "pep-identified-constitutional-symptom/remove/{key}";

        public const string ReadPEPIdentifiedConstitutionalSymptomsByClient = "pep-identified-constitutional-symptoms-by-client/{clientId}";

        public const string ReadPEPIdentifiedConstitutionalSymptomsByEncounterId = "pep-identified-constitutional-symptoms-encounter/{encounterId}";

        #endregion

        #region PhysicalSystem
        public const string ReadPhysicalSystems = "physical-systems";

        public const string CreatePhysicalSystem = "physical-system";

        public const string ReadPhysicalSystemByKey = "physical-system/key/{key}";

        public const string UpdatePhysicalSystem = "physical-system/{key}";

        public const string DeletePhysicalSystem = "physical-system/{key}";
        #endregion

        #region SystemReview
        public const string CreateSystemReview = "system-review";

        public const string ReadSystemReviews = "system-reviews";

        public const string ReadSystemReviewByClient = "system-review/by-client/{clientId}";

        public const string ReadSystemReviewByKey = "system-review/key/{key}";

        public const string ReadSystemReviewByOPDVisit = "system-review-by-encounter/key/{encounterId}";

        public const string UpdateSystemReview = "system-review/{key}";

        public const string UpdateSystemReviewBySingleOjbect = "update-system-review/{key}";

        public const string DeleteSystemReview = "system-review/{key}";

        public const string RemoveSystemReview = "system-review/remove/{encounterId}";

        //PEPSystemReview
        public const string CreatePEPSystemReview = "pep-system-review";

        public const string ReadPEPSystemReviews = "pep-system-reviews";

        public const string ReadPEPSystemReviewByClient = "pep-system-review/by-client/{clientId}";

        public const string ReadPEPSystemReviewByKey = "pep-system-review/key/{key}";

        public const string ReadPEPSystemReviewByEncounter = "pep-system-review/by-encounter/{encounterId}";

        public const string UpdatePEPSystemReview = "pep-system-review/{key}";

        public const string DeletePEPSystemReview = "pep-system-review/{key}";

        public const string RemovePEPSystemReview = "pep-system-review/remove/{encounterId}";

        //PrEPSystemReview
        public const string CreatePrEPSystemReview = "prep-system-review";

        public const string ReadPrEPSystemReviews = "prep-system-reviews";

        public const string ReadPrEPSystemReviewByClient = "prep-system-review/by-client/{clientId}";

        public const string ReadPrEPSystemReviewByKey = "prep-system-review/key/{key}";

        public const string ReadPrEPSystemReviewByEncounter = "prep-system-review/by-encounter/{encounterId}";

        public const string UpdatePrEPSystemReview = "prep-system-review/{key}";

        public const string DeletePrEPSystemReview = "prep-system-review/{key}";

        public const string RemovePrEPSystemReview = "prep-system-review/remove/{encounterId}";
        #endregion

        #region Log
        public const string ReadAllLogs = "all-serilogs";

        #endregion

        #region Allergy
        public const string ReadAllergies = "allergies";

        public const string CreateAllergy = "allergy";

        public const string ReadAllergyByKey = "allergy/key/{key}";

        public const string UpdateAllergy = "allergy/{key}";

        public const string DeleteAllergy = "allergy/{key}";

        //PEPAllergy
        public const string ReadPEPAllergies = "pep-allergies";

        //PrEPAllergy
        public const string ReadPrEPAllergies = "pep-allergies";
        #endregion

        #region AllergicDrug
        public const string ReadAllergicDrug = "allergic-drugs";

        public const string CreateAllergicDrug = "allergic-drug";

        public const string ReadAllergicDrugByKey = "allergic-drug/key/{key}";

        public const string UpdateAllergicDrug = "allergic-drug/{key}";

        public const string DeleteAllergicDrug = "allergic-drug/{key}";
        #endregion

        #region BirthHistory
        public const string CreateBirthHistory = "birth-history";

        public const string ReadBirthHistories = "birth-histories";

        public const string ReadBirthHistoryByKey = "birth-history/key/{key}";

        public const string ReadBirthHistoryByClient = "birth-history/by-client/{clientId}";

        public const string ReadBirthHistoryByEncounterId = "birth-history/by-encounter/{encounterId}";

        public const string UpdateBirthHistory = "birth-history/{key}";

        public const string DeleteBirthHistory = "birth-history/{key}";
        #endregion

        #region VaccineType
        public const string CreateVaccineType = "vaccine-type";

        public const string ReadVaccineTypes = "vaccine-types";

        public const string ReadVaccineTypeByKey = "vaccine-type/key/{key}";

        public const string UpdateVaccineType = "vaccine-type/{key}";

        public const string DeleteVaccineType = "vaccine-type/{key}";
        #endregion

        #region Vaccine
        public const string CreateVaccine = "vaccine";

        public const string ReadVaccines = "vaccines";

        public const string ReadVaccineByKey = "vaccine/key/{key}";

        public const string UpdateVaccine = "vaccine/{key}";

        public const string DeleteVaccine = "vaccine/{key}";

        public const string VaccineNamesByVaccineType = "vaccine/by-vaccine-type/{vaccineTypeId}";
        #endregion

        #region CaCxScreeningMethod
        public const string CreateCaCxScreeningMethod = "cacx-screening-method";

        public const string ReadCaCxScreeningMethods = "cacx-screening-methods";

        public const string ReadCaCxScreeningMethodByKey = "cacx-screening-method/key/{key}";

        public const string UpdateCaCxScreeningMethod = "cacx-screening-method/{key}";

        public const string DeleteCaCxScreeningMethod = "cacx-screening-method/{key}";
        #endregion

        #region Contraceptive
        public const string CreateContraceptive = "contraceptive";

        public const string ReadContraceptives = "contraceptives";

        public const string ReadContraceptiveByKey = "contraceptive/key/{key}";

        public const string UpdateContraceptive = "contraceptive/{key}";

        public const string DeleteContraceptive = "contraceptive/{key}";
        #endregion

        #region ContraceptiveHistory
        public const string CreateContraceptiveHistory = "contraceptive-history";

        public const string ReadContraceptiveHistories = "contraceptive-histories";

        public const string ReadContraceptiveHistory = "contraceptive-history/key/{key}";

        public const string UpdateContraceptiveHistory = "contraceptive-history/{key}";

        public const string DeleteContraceptiveHistory = "contraceptive-history/{key}";
        #endregion

        #region IdentifiedTBSymptom
        public const string CreateIdentifiedTBSymptom = "identified-tb-symptom";

        public const string ReadIdentifiedTBSymptoms = "identified-tb-symptoms";

        public const string ReadIdentifiedTBSymptomByKey = "identified-tb-symptom/key/{key}";

        public const string ReadIdentifiedTBSymptomByEncounterId = "identified-tb-symptom-by-encounterId/{encounterId}";

        public const string ReadIdentifiedTBAndConstitutionalSymptomByEncounterId = "identified-tb-constitutional-symptom-by-encounterId/{encounterId}";

        public const string UpdateIdentifiedTBAndConstitutionalSymptom = "identified-tb-constitutional-symptom/{encounterId}";

        public const string IdentifiedTBAndConstitutionalSymptom = "identified-tb-constitutional-symptom";

        public const string UpdateIdentifiedTBSymptom = "identified-tb-symptom/{key}";

        public const string DeleteIdentifiedTBSymptom = "identified-tb-symptom/{key}";

        public const string RemoveIdentifiedTBSymptom = "identified-tb-symptom/remove/{key}";

        public const string ReadIdentifiedTBSymptomByClientId = "identified-tb-symptom-by-client/{clientId}";

        public const string CreatePEPIdentifiedTBSymptom = "pep-identified-tb-symptom";

        public const string ReadPEPIdentifiedTBSymptoms = "pep-identified-tb-symptoms";

        public const string ReadPEPIdentifiedTBSymptomByKey = "pep-identified-tb-symptom/key/{key}";

        public const string ReadPEPIdentifiedTBSymptomByEncounterId = "pep-identified-tb-symptom-by-encounter/{encounterId}";

        public const string UpdatePEPIdentifiedTBSymptom = "pep-identified-tb-symptom/{key}";

        public const string DeletePEPIdentifiedTBSymptom = "pep-identified-tb-symptom/{key}";

        public const string RemovePEPIdentifiedTBSymptom = "pep-identified-tb-symptom/remove/{key}";

        public const string ReadPEPIdentifiedTBSymptomByClientId = "pep-identified-tb-symptom-by-client/{clientId}";
        #endregion

        #region IdentifiedAllergy
        public const string CreateIdentifiedAllergy = "identified-allergy";

        public const string ReadIdentifiedAllergies = "identified-allergies";

        public const string ReadIdentifiedAllergyByKey = "identified-allergy/key/{key}";

        public const string UpdateIdentifiedAllergy = "identified-allergy/{key}";

        public const string DeleteIdentifiedAllergy = "identified-allergy/{key}";

        public const string ReadIdentifiedAllergyByClient = "identified-allergy-by-client/{clientId}";

        public const string ReadIdentifiedAllergyByEncounterId = "identified-allergy-encounter/{encounterId}";

        public const string RemoveIdentifiedAllergy = "identified-allergy/remove/{key}";

        //PEPIdentifiedAllergy
        public const string CreatePEPIdentifiedAllergy = "pep-identified-allergy";

        public const string ReadPEPIdentifiedAllergies = "pep-identified-allergies";

        public const string ReadPEPIdentifiedAllergyByKey = "pep-identified-allergy/key/{key}";

        public const string UpdatePEPIdentifiedAllergy = "pep-identified-allergy/{key}";

        public const string DeletePEPIdentifiedAllergy = "pep-identified-allergy/{key}";

        public const string ReadPEPIdentifiedAllergyByClient = "pep-identified-allergy/by-client/{clientId}";

        public const string ReadPEPIdentifiedAllergyByEncounter = "pep-identified-allergy/by-encounter/{encounterId}";

        public const string RemovePEPIdentifiedAllergy = "pep-identified-allergy/remove/{key}";

        //PrEPIdentifiedAllergy
        public const string CreatePrEPIdentifiedAllergy = "pep-identified-allergy";

        public const string ReadPrEPIdentifiedAllergies = "pep-identified-allergies";

        public const string ReadPrEPIdentifiedAllergyByKey = "pep-identified-allergy/key/{key}";

        public const string UpdatePrEPIdentifiedAllergy = "pep-identified-allergy/{key}";

        public const string DeletePrEPIdentifiedAllergy = "pep-identified-allergy/{key}";

        public const string ReadPrEPIdentifiedAllergyByClient = "pep-identified-allergy/by-client/{clientId}";

        public const string ReadPrEPIdentifiedAllergyByEncounter = "pep-identified-allergy/by-encounter/{encounterId}";

        public const string RemovePrEPIdentifiedAllergy = "pep-identified-allergy/remove/{key}";
        #endregion

        #region ImmunizationRecord
        public const string CreateImmunizationRecord = "immunization-record";

        public const string CreateVaccinationRecord = "vaccine-record";

        public const string ReadImmunizationRecords = "immunization-records";

        public const string ReadImmunizationRecordByKey = "immunization-record/key/{key}";

        public const string ReadImmunizationRecordByClient = "immunization-record/by-client/{clientId}";

        public const string UpdateImmunizationRecord = "immunization-record/{key}";

        public const string UpdateVaccinationRecord = "vaccine-record/{key}";

        public const string DeleteImmunizationRecord = "immunization-record/{key}";

        public const string RemoveImmunizationRecord = "immunization-record/remove/{encounterId}";

        public const string ReadVaccineTypesByImmunizationRecord = "immunization-record-vaccine-types/{vaccineTypeId}";

        public const string ReadImmunizationRecordByEncounterId = "immunization-record/by-encounter/{encounterId}";

        //PEP
        public const string RemovePEPImmunizationRecord = "pep-immunization-record/remove/{encounterId}";

        //PrEP
        public const string RemovePrEPImmunizationRecord = "prep-immunization-record/remove/{encounterId}";

        //Under5
        public const string RemoveUnderFiveImmunizationRecord = "under-five-immunization-record/remove/{encounterId}";

        #endregion

        #region GynObsHistory
        public const string CreateGynObsHistory = "gyn-obs-history";

        public const string ReadGynObsHistories = "gyn-obs-histories";

        public const string ReadGynObsHistoryByKey = "gyn-obs-history/key/{key}";

        public const string ReadGynObsHistoryByEncounterId = "gyn-obs-history-by-encounter/{encounterId}";

        public const string ReadGynObsHistoryByClientId = "gyn-obs-history-by-client/{clientId}";

        public const string ReadLatestGynObsHistoriesByClientId = "latest-gyn-obs-history-by-client/{clientId}";

        public const string UpdateGynObsHistory = "gyn-obs-history/{key}";

        public const string DeleteGynObsHistory = "gyn-obs-history/{key}";

        public const string RemoveGynObsHistory = "gyn-obs-history/remove/{encounterId}";

        //PEPGynObsHistory
        public const string CreatePEPGynObsHistory = "pep-gyn-obs-history";

        public const string ReadPEPGynObsHistories = "pep-gyn-obs-histories";

        public const string ReadPEPGynObsHistoryByKey = "pep-gyn-obs-history/key/{key}";

        public const string ReadPEPGynObsHistoryByEncounter = "pep-gyn-obs-history/by-encounter/{encounterId}";

        public const string ReadPEPGynObsHistoryByClient = "pep-gyn-obs-history/by-client/{clientId}";

        public const string UpdatePEPGynObsHistory = "pep-gyn-obs-history/{key}";

        public const string DeletePEPGynObsHistory = "pep-gyn-obs-history/{key}";

        public const string RemovePEPGynObsHistory = "pep-gyn-obs-history/remove/{key}";

        //PrEPGynObsHistory
        public const string CreatePrEPGynObsHistory = "prep-gyn-obs-history";

        public const string ReadPrEPGynObsHistories = "prep-gyn-obs-histories";

        public const string ReadPrEPGynObsHistoryByKey = "prep-gyn-obs-history/key/{key}";

        public const string ReadPrEPGynObsHistoryByEncounter = "prep-gyn-obs-history/by-encounter/{encounterId}";

        public const string ReadPrEPGynObsHistoryByClient = "prep-gyn-obs-history/by-client/{clientId}";

        public const string UpdatePrEPGynObsHistory = "prep-gyn-obs-history/{key}";

        public const string DeletePrEPGynObsHistory = "prep-gyn-obs-history/{key}";

        public const string RemovePrEPGynObsHistory = "prep-gyn-obs-history/remove/{key}";
        #endregion

        #region MedicalHistory
        public const string CreateMedicalHistory = "medical-history";

        public const string CreateMedicalHistoryForFamilyFoodHistory = "medical-history-for-family-food-history";

        public const string ReadMedicalHistories = "medical-histories";

        public const string ReadMedicalHistoriesByEncounterId = "medical-histories-by-visit/{encounterId}";

        public const string ReadMedicalHistoriesByClientId = "medical-histories-by-client/{clientId}";

        public const string ReadPastMedicalHistoryByClient = "past-medical-histories-by-client/{clientId}";

        public const string ReadFamilyFoodHistoryByClient = "family-food-histories-by-client/{clientId}";

        public const string ReadMedicalHistoriesByClientIdForFamilyFoodHistories = "medical-histories-by-client-for-family-food-history/{clientId}";

        public const string ReadMedicalHistoryByKey = "medical-history/key/{key}";

        public const string UpdateMedicalHistory = "medical-history/{key}";

        public const string DeleteMedicalHistory = "medical-history/{key}/{encounterType}";

        //PEPMedicalHistory
        public const string CreatePEPMedicalHistory = "pep-medical-history";

        public const string ReadPEPMedicalHistories = "pep-medical-histories";

        public const string ReadPEPMedicalHistoryByEncounter = "pep-medical-history/by-encounter/{encounterId}";

        public const string ReadPEPMedicalHistoryByClient = "pep-medical-history/by-client/{clientId}";

        public const string ReadPEPMedicalHistoryByKey = "pep-medical-history/key/{key}";

        public const string UpdatePEPMedicalHistory = "pep-medical-history/{key}";

        //PrEPMedicalHistory
        public const string CreatePrEPMedicalHistory = "prep-medical-history";

        public const string ReadPrEPMedicalHistories = "prep-medical-histories";

        public const string ReadPrEPMedicalHistoryByEncounter = "prep-medical-history/by-encounter/{encounterId}";

        public const string ReadPrEPMedicalHistoryByClient = "prep-medical-history/by-client/{clientId}";

        public const string ReadPrEPMedicalHistoryByKey = "prep-medical-history/key/{key}";

        public const string UpdatePrEPMedicalHistory = "prep-medical-history/{key}";

        public const string DeletePrEPMedicalHistory = "prep-medical-history/{key}";

        public const string RemovePrEPMedicalFamilySocialHistory = "prep-medical-history/remove-family-social/{encounterId}";

        public const string RemovePrEPMedicalPastMedicalHistory = "prep-medical-history/remove-past-medical/{encounterId}";
        #endregion

        #region ChildsDevelopmentHistory
        public const string CreateChildsDevelopmentHistory = "childs-dev-history";

        public const string ReadChildsDevelopmentHistories = "childs-dev-histories";

        public const string ReadChildsDevelopmentHistoryByKey = "childs-dev-history/key/{key}";

        public const string ReadChildsDevelopmentHistoryByClient = "childs-dev-history/by-client/{clientId}";

        public const string UpdateChildsDevelopmentHistory = "childs-dev-history/{key}";

        public const string DeleteChildsDevelopmentHistory = "childs-dev-history/{key}";
        #endregion

        #region Assessment
        public const string CreateAssessment = "assessment";

        public const string ReadAssessments = "assessments";

        public const string ReadAssessmentByClient = "assessment/by-client/{clientId}";

        public const string ReadAssessmentByKey = "assessment/key/{key}";

        public const string UpdateAssessment = "assessment/{key}";

        public const string DeleteAssessment = "assessment/{key}";
        #endregion

        #region TreatmentPlan
        public const string CreateTreatmentPlan = "treatment-plan";

        public const string ReadTreatmentPlans = "treatment-plans";

        public const string ReadTreatmentPlanByKey = "treatment-plan/key/{key}";

        public const string ReadTreatmentPlanByClient = "treatment-plan/{clientId}";

        public const string ReadLastEncounterTreatmentPlanByClient = "treatment-plan/last-encounter/{clientId}";

        public const string ReadLatestTreatmentPlanByClient = "latest-treatment-plan/{clientId}";
        public const string ReadLatestTreatmentPlanByClientForFluid = "latest-treatment-plan/for-fluid/{clientId}";

        public const string ReadTreatmentPlanBySurgeryId = "treatment-plan/surgery/{surgeryId}";

        public const string ReadCompleteTreatmentPlanByEncounterId = "complete-treatment-plan/{encounterId}";

        public const string UpdateTreatmentPlan = "treatment-plan/{key}";

        public const string DeleteTreatmentPlan = "treatment-plan/{key}";

        //PEP TreatmentPlan
        public const string CreatePEPTreatmentPlan = "pep-treatment-plan";

        public const string ReadPEPTreatmentPlans = "pep-treatment-plans";

        public const string ReadPEPTreatmentPlanByKey = "pep-treatment-plan/key/{key}";

        public const string ReadPEPTreatmentPlanByClient = "pep-treatment-plan-by-client/{clientId}";

        public const string ReadPEPCompleteTreatmentPlanByEncounter = "pep-complete-treatment-plan/by-encounter/{encounterId}";

        public const string ReadPEPCompleteTreatmentPlanByEncounterId = "pep-complete-treatmentplan/{encounterId}";

        public const string ReadPrEPCompleteTreatmentPlanByEncounterId = "prep-complete-treatmentplan/{encounterId}";

        public const string UpdatePEPTreatmentPlan = "pep-treatment-plan/{key}";

        public const string DeletePEPTreatmentPlan = "pep-treatment-plan/{key}";

        //PrEPTreatmentPlan
        public const string CreatePrEPTreatmentPlan = "prep-treatment-plan";

        public const string ReadPrEPTreatmentPlans = "prep-treatment-plans";

        public const string ReadPrEPTreatmentPlanByKey = "prep-treatment-plan/key/{key}";

        public const string ReadPrEPTreatmentPlanByClient = "prep-treatment-plan-by-client/{clientId}";

        public const string ReadPrEPCompleteTreatmentPlanByEncounter = "prep-complete-treatment-plan/by-encounter/{encounterId}";

        public const string UpdatePrEPTreatmentPlan = "prep-treatment-plan/{key}";

        public const string DeletePrEPTreatmentPlan = "prep-treatment-plan/{key}";

        //IPD TreatmentPlan
        public const string CreateIPDTreatmentPlan = "ipd-treatment-plan";

        public const string UpdateIPDTreatmentPlan = "ipd-treatment-plan/{key}";

        public const string DeleteIPDTreatmentPlan = "ipd-treatment-plan/{key}";

        public const string ReadCompleteIPDHistoryDtoByEncounterId = "ipd-complete-treatment-plan/{encounterId}";

        //Nursing Plan
        public const string CreateNursingTreatmentPlan = "nursing-treatment-plan";

        public const string ReadNursingTreatmentPlans = "nursing-treatment-plans";

        public const string ReadNursingTreatmentPlanByKey = "nursing-treatment-plan/key/{key}";

        public const string ReadNursingTreatmentPlanByClient = "nursing-treatment-plan/{clientId}";

        public const string ReadLatestNursingTreatmentPlanByClient = "latest-nursing-treatment-plan/{clientId}";

        public const string ReadCompleteNursingTreatmentPlanByEncounter = "complete-nursing-treatment-plan/{encounterId}";

        public const string UpdateNursingTreatmentPlan = "nursing-treatment-plan/{key}";

        public const string DeleteNursingTreatmentPlan = "nursing-treatment-plan/{key}";
        #endregion

        #region SystemExamination
        public const string CreateSystemExamination = "system-examination";

        public const string ReadSystemExaminations = "system-examinations";

        public const string ReadSystemExaminationByKey = "system-examination/key/{key}";

        public const string ReadSystemExaminationByClient = "system-examination/by-client/{clientId}";

        public const string UpdateSystemExamination = "system-examination/{key}";

        public const string DeleteSystemExamination = "system-examination/{key}";

        public const string RemoveSystemExamination = "system-examination/remove/{encounterId}";

        public const string RemoveARTSystemExamination = "art-system-examination/remove/{encounterId}";

        public const string ReadSystemExaminationByOPDVisit = "system-examination-by-encounter/{encounterId}";

        //PEPSystemExamination
        public const string CreatePEPSystemExamination = "pep-system-examination";

        public const string ReadPEPSystemExaminations = "pep-system-examinations";

        public const string ReadPEPSystemExaminationByKey = "pep-system-examination/key/{key}";

        public const string ReadPEPSystemExaminationByClient = "pep-system-examination/by-client/{clientId}";

        public const string UpdatePEPSystemExamination = "pep-system-examination/{key}";

        public const string DeletePEPSystemExamination = "pep-system-examination/{key}";

        public const string RemovePEPSystemExamination = "pep-system-examination/remove/{encounterId}";

        public const string ReadPEPSystemExaminationByEncounter = "pep-system-examination/by-encounter/{encounterId}";

        //PrEPSystemExamination
        public const string CreatePrEPSystemExamination = "prep-system-examination";

        public const string ReadPrEPSystemExaminations = "prep-system-examinations";

        public const string ReadPrEPSystemExaminationByKey = "prep-system-examination/key/{key}";

        public const string ReadPrEPSystemExaminationByClient = "prep-system-examination/by-client/{clientId}";

        public const string UpdatePrEPSystemExamination = "prep-system-examination/{key}";

        public const string DeletePrEPSystemExamination = "prep-system-examination/{key}";

        public const string RemovePrEPSystemExamination = "prep-system-examination/remove/{encounterId}";

        public const string ReadPrEPSystemExaminationByEncounter = "prep-system-examination/by-encounter/{encounterId}";

        //Under 5
        public const string CreateUnderFiveSystemExamination = "under-five-system-examination";

        public const string ReadUnderFiveSystemExaminations = "under-five-system-examinations";

        public const string ReadUnderFiveSystemExaminationByKey = "under-five-system-examination/key/{key}";

        public const string ReadUnderFiveSystemExaminationByClient = "under-five-system-examination/by-client/{clientId}";

        public const string UpdateUnderFiveSystemExamination = "under-five-system-examination/{key}";

        public const string DeleteUnderFiveSystemExamination = "under-five-system-examination/{key}";

        public const string RemoveUnderFiveSystemExamination = "under-five-system-examination/remove/{encounterId}";

        public const string ReadUnderFiveSystemExaminationByEncounter = "under-five-system-examination/by-encounter/{encounterId}";
        #endregion

        #region GlasgowComaScale
        public const string CreateGlasgowComaScale = "glasgow-coma-scale";

        public const string ReadGlasgowComaScales = "glasgow-coma-scales";

        public const string ReadGlasgowComaScaleByKey = "glasgow-coma-scale/key/{key}";

        public const string ReadGlasgowComaScaleByClient = "glasgow-coma-scale/by-client/{clientId}";

        public const string UpdateGlasgowComaScale = "glasgow-coma-scale/{key}";

        public const string DeleteGlasgowComaScale = "glasgow-coma-scale/{key}";

        //PEPGlasgowComaScale
        public const string CreatePEPGlasgowComaScale = "pep-glasgow-coma-scale";

        public const string ReadPEPGlasgowComaScales = "pep-glasgow-coma-scales";

        public const string ReadPEPGlasgowComaScaleByKey = "pep-glasgow-coma-scale/key/{key}";

        public const string ReadPEPGlasgowComaScaleByClient = "pep-glasgow-coma-scale/by-client/{clientId}";

        public const string UpdatePEPGlasgowComaScale = "pep-glasgow-coma-scale/{key}";

        public const string DeletePEPGlasgowComaScale = "pep-glasgow-coma-scale/{key}";

        //PrEPGlasgowComaScale
        public const string CreatePrEPGlasgowComaScale = "prep-glasgow-coma-scale";

        public const string ReadPrEPGlasgowComaScales = "prep-glasgow-coma-scales";

        public const string ReadPrEPGlasgowComaScaleByKey = "prep-glasgow-coma-scale/key/{key}";

        public const string ReadPrEPGlasgowComaScaleByClient = "prep-glasgow-coma-scale/by-client/{clientId}";

        public const string UpdatePrEPGlasgowComaScale = "prep-glasgow-coma-scale/{key}";

        public const string DeletePrEPGlasgowComaScale = "prep-glasgow-coma-scale/{key}";
        #endregion

        #region Diagnosis
        public const string CreateDiagnosis = "diagnosis";

        public const string ReadDiagnoses = "diagnoses";

        public const string ReadDiagnosesBYClient = "diagnoses-diagnoses-by-client/{clientId}";

        public const string ReadDiagnosesLatestBYClient = "diagnoses-diagnoses-latest-by-client/{clientId}";

        public const string ReadDiagnosesLastBYClient = "diagnoses-diagnoses-last-by-client/{clientId}";

        public const string ReadDiagnosesByOPDVisit = "diagnoses-diagnoses-by-encounter/{encounterId}";

        public const string ReadDiagnosesByLastencounterId = "diagnoses-diagnoses-by-encounter/{encounterId}";

        public const string ReadDiagnosisByKey = "diagnosis/key/{key}";

        public const string ReadDiagnosisByClient = "diagnosis/latest-diagnosis-by-client/{clientId}";

        public const string ReadLastEncounterDiagnosisByClient = "diagnosis/last-encounter/{clientId}";

        public const string ReadDiagnosisBySurgeryId = "diagnosis/surgery/key/{key}";

        public const string LoadNTGTreeDiagnosis = "diagnosis/loadNTGTree";

        public const string LoadNTGLevel1Diagnosis = "diagnosis/load-ntg-level-1";

        public const string LoadNTGLevel2Diagnosis = "diagnosis/load-ntg-level-2";

        public const string LoadNTGLevel3Diagnosis = "diagnosis/load-ntg-level-3";

        public const string LoadDiagnosisCodesTreeDiagnosis = "diagnosis/loadDiagnosisCodesTree";

        public const string UpdateDiagnosis = "diagnosis/{key}";

        public const string DeteleDiagnosis = "diagnosis/{key}";

        public const string RemoveDiagnosis = "diagnosis/remove/{encounterId}";

        //PEP Diagnosis
        public const string CreatePEPDiagnosis = "pep-diagnosis";

        public const string ReadPEPDiagnoses = "pep-diagnoses";

        public const string ReadPEPDiagnosisByClient = "pep-diagnoses/by-client/{clientId}";

        public const string ReadPEPLatestThreeDiagnosesByClient = "pep-latest-three-diagnoses/by-client/{clientId}";

        public const string ReadPEPDiagnosisByEncounter = "pep-diagnosis/by-encounter/{encounterId}";

        public const string ReadPEPDiagnosisByKey = "pep-diagnosis/key/{key}";

        public const string UpdatePEPDiagnosis = "pep-diagnosis/{key}";

        public const string DetelePEPDiagnosis = "pep-diagnosis/{key}";

        public const string RemovePEPDiagnosis = "pep-diagnosis/remove/{key}";

        //PrEP Diagnosis
        public const string CreatePrEPDiagnosis = "prep-diagnosis";

        public const string ReadPrEPDiagnoses = "prep-diagnoses";

        public const string ReadPrEPDiagnosisByClient = "prep-diagnosis/by-client/{clientId}";

        public const string ReadPrEPLatestThreeDiagnosesByClient = "prep-latest-three-diagnoses/by-client/{clientId}";

        public const string ReadPrEPDiagnosisByEncounter = "prep-diagnoses/by-encounter/{encounterId}";

        public const string ReadPrEPDiagnosisByKey = "prep-diagnosis/key/{key}";

        public const string UpdatePrEPDiagnosis = "prep-diagnosis/{key}";

        public const string DetelePrEPDiagnosis = "prep-diagnosis/{key}";

        public const string RemovePrEPDiagnosis = "prep-diagnosis/remove/{key}";

        //IPD Diagnosis
        public const string CreateIPDDiagnosis = "ipd-diagnosis";

        public const string UpdateIPDDiagnosis = "ipd-diagnosis/{key}";

        public const string RemoveIPDDiagnosis = "ipd-diagnosis/remove/{key}";

        //Under5
        public const string RemoveUnderFiveDiagnosis = "under-five/remove/{key}";
        #endregion

        #region Condition
        public const string CreateCondition = "condition";

        public const string ReadConditions = "conditions";

        public const string ReadConditionByKey = "condition/key/{key}";

        public const string ReadConditionByClient = "condition/by-client/{clientId}";

        public const string LoadNTGTreeCondition = "condition/LoadNTGTree";

        public const string LoadDiagnosisCodesTreeCondition = "condition/LoadDiagnosisCodesTree";

        public const string ReadConditionByEncounterId = "condition/by-encounter/{encounterId}";

        public const string UpdateCondition = "condition/{key}";

        public const string DeteleCondition = "condition/{key}";

        public const string RemoveCondition = "condition/remove-condition/{encounterId}";

        //PEPCondition
        public const string CreatePEPCondition = "pep-condition";

        public const string ReadPEPConditions = "pep-conditions";

        public const string ReadPEPConditionByKey = "pep-condition/key/{key}";

        public const string ReadPEPConditionByClient = "pep-condition/by-client/{clientId}";

        public const string ReadPEPConditionByEncounter = "pep-condition/by-encounter/{encounterId}";

        public const string UpdatePEPCondition = "pep-condition/{key}";

        public const string DetelePEPCondition = "pep-condition/{key}";

        //PrEPCondition
        public const string CreatePrEPCondition = "prep-condition";

        public const string ReadPrEPConditions = "prep-conditions";

        public const string ReadPrEPConditionByKey = "prep-condition/key/{key}";

        public const string ReadPrEPConditionByClient = "prep-condition/by-client/{clientId}";

        public const string ReadPrEPConditionByEncounter = "prep-condition/by-encounter/{encounterId}";

        public const string UpdatePrEPCondition = "prep-condition/{key}";

        public const string DetelePrEPCondition = "prep-condition/{key}";
        #endregion

        #region ICDDiagnosis
        public const string CreateICDDiagnosis = "icd-diagnosis";

        public const string ReadICDDiagnoses = "icd-diagnoses";

        public const string ReadICDDiagnosesSearch = "icd-diagnoses-search";

        public const string ReadICDDiagnosisByKey = "icd-diagnosis/key/{key}";

        public const string ReadICDDiagnosisByICP2 = "icd-diagnosis/icp/{key}";

        public const string UpdateICDDiagnosis = "icd-diagnosis/{key}";

        public const string DeteleICDDiagnosis = "icd-diagnosis/{key}";

        //PEPICDDiagnosis
        public const string CreatePEPICDDiagnosis = "pep-icd-diagnosis";

        public const string ReadPEPICDDiagnoses = "pep-icd-diagnoses";

        public const string ReadPEPICDDiagnosisByKey = "pep-icd-diagnosis/key/{key}";

        public const string UpdatePEPICDDiagnosis = "pep-icd-diagnosis/{key}";

        public const string DetelePEPICDDiagnosis = "pep-icd-diagnosis/{key}";

        //PrEPICDDiagnosis
        public const string CreatePrEPICDDiagnosis = "prep-icd-diagnosis";

        public const string ReadPrEPICDDiagnoses = "prep-icd-diagnoses";

        public const string ReadPrEPICDDiagnosisByKey = "prep-icd-diagnosis/key/{key}";

        public const string UpdatePrEPICDDiagnosis = "prep-icd-diagnosis/{key}";

        public const string DetelePrEPICDDiagnosis = "prep-icd-diagnosis/{key}";
        #endregion

        #region NTGLevelOneDiagnosis
        public const string CreateNTGLevelOneDiagnosis = "ntg-level-one-diagnosis";

        public const string ReadNTGLevelOneDiagnoses = "ntg-level-one-diagnoses";

        public const string ReadNTGLevelOneDiagnosisByKey = "ntg-level-one-diagnosis/key/{key}";

        public const string UpdateNTGLevelOneDiagnosis = "ntg-level-one-diagnosis/{key}";

        public const string DeteleNTGLevelOneDiagnosis = "ntg-level-one-diagnosis/{key}";

        //PEPNTGLevelOneDiagnosis
        public const string CreatePEPNTGLevelOneDiagnosis = "pep-ntg-level-one-diagnosis";

        public const string ReadPEPNTGLevelOneDiagnoses = "pep-ntg-level-one-diagnoses";

        public const string ReadPEPNTGLevelOneDiagnosisByKey = "pep-ntg-level-one-diagnosis/key/{key}";

        public const string UpdatePEPNTGLevelOneDiagnosis = "pep-ntg-level-one-diagnosis/{key}";

        public const string DetelePEPNTGLevelOneDiagnosis = "pep-ntg-level-one-diagnosis/{key}";

        //PrEPNTGLevelOneDiagnosis
        public const string CreatePrEPNTGLevelOneDiagnosis = "prep-ntg-level-one-diagnosis";

        public const string ReadPrEPNTGLevelOneDiagnoses = "prep-ntg-level-one-diagnoses";

        public const string ReadPrEPNTGLevelOneDiagnosisByKey = "prep-ntg-level-one-diagnosis/key/{key}";

        public const string UpdatePrEPNTGLevelOneDiagnosis = "prep-ntg-level-one-diagnosis/{key}";

        public const string DetelePrEPNTGLevelOneDiagnosis = "prep-ntg-level-one-diagnosis/{key}";
        #endregion

        #region NTGLevelTwoDiagnosis
        public const string CreateNTGLevelTwoDiagnosis = "ntg-level-two-diagnosis";

        public const string ReadNTGLevelTwoDiagnoses = "ntg-level-two-diagnoses";

        public const string ReadNTGLevelTwoDiagnosisByKey = "ntg-level-two-diagnosis/key/{key}";

        public const string ReadNTGLevelTwoDiagnosisByNTGLevelOneDiagnosis = "ntg-level-two-diagnosis/by-ntg-level-one-diagnosis/{ntgLevelOneId}";

        public const string UpdateNTGLevelTwoDiagnosis = "ntg-level-two-diagnosis/{key}";

        public const string DeteleNTGLevelTwoDiagnosis = "ntg-level-two-diagnosis/{key}";
        #endregion

        #region NTGLevelThreeDiagnosis
        public const string CreateNTGLevelThreeDiagnosis = "ntg-level-three-diagnosis";

        public const string ReadNTGLevelThreeDiagnoses = "ntg-level-three-diagnoses";

        public const string ReadNTGLevelThreeDiagnosisByKey = "ntg-level-three-diagnosis/key/{key}";

        public const string ReadNTGLevelThreeDiagnosisByNTGLevelTwoDiagnosis = "ntg-level-three-diagnosis/by-ntg-level-two-diagnosis/{ntgLevelTwoId}";

        public const string UpdateNTGLevelThreeDiagnosis = "ntg-level-three-diagnosis/{key}";

        public const string DeteleNTGLevelThreeDiagnosis = "ntg-level-three-diagnosis/{key}";
        #endregion

        #region Exposure
        public const string CreateExposure = "exposure";

        public const string ReadExposures = "exposures";

        public const string ReadExposureByKey = "exposure/key/{key}";

        public const string UpdateExposure = "exposure/{key}";

        public const string DeleteExposure = "exposure/{key}";
        #endregion

        #region ExposureType
        public const string CreateExposureType = "exposure-type";

        public const string ReadExposureTypes = "exposure-types";

        public const string ReadExposureTypeByKey = "exposure-type/key/{key}";

        public const string UpdateExposureType = "exposure-type/{key}";

        public const string DeleteExposureType = "exposure-type/{key}";
        #endregion

        #region PEPRisk
        public const string CreatePEPRisk = "pep-risk";

        public const string ReadPEPRisks = "pep-risks";

        public const string ReadPEPRiskByKey = "pep-risk/key/{key}";

        public const string UpdatePEPRisk = "pep-risk/{key}";

        public const string DeletePEPRisk = "pep-risk/{key}";
        #endregion

        #region PEPRiskStatus
        public const string CreatePEPRiskStatus = "pep-risk-status";

        public const string ReadPEPRiskStatuses = "pep-risk-statuses";

        public const string ReadPEPRiskStautsByEncounterId = "pep-risk-statuses/key/{key}";

        public const string ReadPEPRiskStatusByKey = "pep-risk-status/key/{key}";

        public const string ReadPEPRiskStatusByClient = "pep-risk-status-by-client/key/{clientId}";

        public const string UpdatePEPRiskStatus = "pep-risk-status/{key}";

        public const string DeletePEPRiskStatus = "pep-risk-status/{key}";
        #endregion

        #region PEPPreventionHistory
        public const string CreatePEPPreventionHistory = "pep-prevention-history";

        public const string ReadPEPPreventionHistories = "pep-prevention-histories";

        public const string ReadPEPPreventionHistoryByKey = "pep-prevention-history/key/{key}";

        public const string ReadPEPPreventionHistoryByClient = "pep-prevention-history/by-client/{clientId}";

        public const string UpdatePEPPreventionHistory = "pep-prevention-history/{key}";

        public const string DeletePEPPreventionHistory = "pep-prevention-history/{key}";
        #endregion

        //INVESTIGATION
        #region Test
        public const string CreateTest = "test";

        public const string ReadTests = "tests";

        public const string ReadTestByKey = "test/key/{key}";

        public const string UpdateTest = "test/{key}";

        public const string DeleteTest = "test/{key}";

        public const string TestBySubTestType = "test/test-by-subtest/{testsubId}";

        public const string ReadTestBySubtypes = "test/test-type-by-subtest/{key}";
        #endregion

        #region TestType
        public const string CreateTestType = "test-type";

        public const string ReadTestTypes = "test-types";

        public const string ReadTestTypeByKey = "test-type/key/{key}";

        public const string UpdateTestType = "test-type/{key}";

        public const string DeleteTestType = "test-type/{key}";
        #endregion

        #region TestSubtype
        public const string CreateTestSubtype = "test-subtype";

        public const string ReadTestSubtypes = "test-subtypes";

        public const string ReadTestSubtypeByKey = "test-subtype/key/{key}";

        public const string UpdateTestSubtype = "test-subtype/{key}";

        public const string DeleteTestSubtype = "test-subtype/{key}";

        public const string SubTypesByTestType = "test-subtype/by-test-type/{testTypeId}";

        public const string ReadSubTestTypesByTest = "test-subtype/by-test/{key}";
        #endregion

        #region Investigation
        public const string CreateInvestigation = "investigation";

        public const string CreateInvestigationByTest = "investigation/compositetest";

        public const string ReadInvestigations = "investigations";

        public const string ReadInvestigationByBatch = "investigations-batch/key/{key}";

        public const string ReadInvestigationByKey = "investigation/key/{key}";

        public const string ReadInvestigationsByClient = "investigation-by-client/key/{clientId}";

        public const string ReadInvestigationsByEncounterId = "investigation-by-encounter/key/{encounterId}";

        public const string ReadInvestigationsDashBoard = "investigation/investigation-dashboard/{facilityId}";

        public const string UpdateInvestigationSampleCollection = "investigation/sample-collection/{key}";

        public const string UpdateInvestigation = "investigation/{key}";

        public const string DeleteInvestigation = "investigation/{key}";

        public const string ReadInvestigationByclientId = "investigation/latest-investigation-by-client/{clientId}";
        #endregion

        #region MeasuringUnit

        public const string CreateMeasuringUnit = "measuring-unit";

        public const string ReadMeasuringUnits = "measuring-units";

        public const string ReadMeasuringUnitByKey = "measuring-unit/key/{key}";

        public const string ReadMeasuringUnitByTest = "measuring-unit/test/{testId}";

        public const string UpdateMeasuringUnit = "measuring-unit/{key}";

        public const string DeleteMeasuringUnit = "measuring-unit/{key}";
        #endregion

        #region ResultOption

        public const string CreateResultOption = "result-option";

        public const string ReadResultOptions = "result-options";

        public const string ReadResultOptionByKey = "result-option/key/{key}";

        public const string ReadResultOptionByTest = "result-option/test/{testId}";

        public const string UpdateResultOption = "result-option/{key}";

        public const string DeleteResultOption = "result-option/{key}";
        #endregion

        #region TestItem

        public const string CreateTestItem = "test-item";

        public const string ReadTestItems = "test-items";

        public const string ReadTestItemByKey = "test-item/key/{key}";

        public const string UpdateTestItem = "test-item/{key}";

        public const string DeleteTestItem = "test-item/{key}";
        #endregion

        #region CompositeTest

        public const string CreateCompositeTest = "composite-test";

        public const string ReadCompositeTests = "composite-tests";

        public const string ReadCompositeTestByKey = "composite-test/key/{key}";

        public const string UpdateCompositeTest = "composite-test/{key}";

        public const string DeleteCompositeTest = "composite-test/{key}";
        #endregion

        #region Result
        public const string CreateResult = "result";

        public const string ReadResults = "results";

        public const string ReadResultByKey = "result/key/{key}";

        public const string UpdateResult = "result/{key}";

        public const string DeleteResult = "result/{key}";

        public const string ReadInvestigationByClient = "result/by-client/{key}";

        public const string ReadLatestResultByClient = "result/latest-result-by-client/{clientId}";

        #endregion

        #region PrEP
        public const string CreatePrEP = "prep";

        public const string ReadPrEPs = "preps";

        public const string ReadPrEPByKey = "prep/key/{key}";

        public const string ReadPrEPByClient = "prep/by-client/{clientId}";

        public const string UpdatePrEP = "prep/{key}";

        public const string DeletePrEP = "prep/{key}";
        #endregion

        #region PEP
        public const string CreatePEP = "pep";

        public const string ReadPEPs = "peps";

        public const string ReadPEPByKey = "pep/key/{key}";

        public const string ReadPEPByClient = "pep/by-client/{clientId}";

        public const string UpdatePEP = "pep/{key}";

        public const string DeletePEP = "pep/{key}";
        #endregion

        #region KeyPopulationDemographic
        public const string CreateKeyPopulationDemographic = "key-population-demographic";

        public const string ReadKeyPopulationDemographics = "key-population-demographics";

        public const string ReadKeyPopulationDemographicByKey = "key-population-demographic/key/{key}";

        public const string ReadKeyPopulationDemographicByClient = "key-population-demographic/by-client/{clientId}";

        public const string ReadKeyPopulationDemographicByEncounter = "key-population-demographic/by-encounter/{encounterId}";

        public const string UpdateKeyPopulationDemographic = "key-population-demographic/{key}";

        public const string DeleteKeyPopulationDemographic = "key-population-demographic/{key}";
        #endregion

        #region KeyPopulation
        public const string CreateKeyPopulation = "key-population";

        public const string ReadKeyPopulations = "key-populations";

        public const string ReadKeyPopulationByKey = "key-population/key/{key}";

        public const string UpdateKeyPopulation = "key-population/{key}";

        public const string DeleteKeyPopulation = "key-population/{key}";
        #endregion

        #region DrugAdherence
        public const string CreateDrugAdherence = "drug-adherence";

        public const string ReadDrugAdherences = "drug-adherences";

        public const string ReadDrugAdherenceByKey = "drug-adherence/key/{key}";

        public const string ReadDrugAdherenceByClient = "drug-adherence/by-client/{clientId}";

        public const string UpdateDrugAdherence = "drug-adherence/{key}";

        public const string DeleteDrugAdherence = "drug-adherence/{key}";
        #endregion

        #region Question
        public const string CreateQuestion = "question";

        public const string ReadQuestions = "questions";

        public const string ReadQuestionsForForm = "questions-for-form";

        public const string ReadQuestionByKey = "question/key/{key}";

        public const string UpdateQuestion = "question/{key}";

        public const string DeleteQuestion = "question/{key}";
        #endregion

        #region HIVRiskScreening
        public const string CreateHIVRiskScreening = "hiv-risk-screening";

        public const string ReadHIVRiskScreenings = "hiv-risk-screenings";

        public const string ReadHIVRiskScreeningByKey = "hiv-risk-screening/key/{key}";

        public const string ReadHIVRiskScreeningByClient = "hiv-risk-screening/by-client/{clientId}";

        public const string ReadHIVRiskScreeningByEncounter = "hiv-risk-screening/by-encounter/{encounterId}";

        public const string UpdateHIVRiskScreening = "hiv-risk-screening/{key}";

        public const string DeleteHIVRiskScreening = "hiv-risk-screening/{key}";
        #endregion

        #region PrEPStoppingReason
        public const string CreatePrEPStoppingReason = "prep-stopping-reason";

        public const string ReadPrEPStoppingReasons = "prep-stopping-reasons";

        public const string ReadPrEPStoppingReasonByKey = "prep-stopping-reason/key/{key}";

        public const string ReadPrEPStoppingReasonByClient = "prep-stopping-reason/by-client/{clientId}";

        public const string UpdatePrEPStoppingReason = "prep-stopping-reason/{key}";

        public const string DeletePrEPStoppingReason = "prep-stopping-reason/{key}";
        #endregion

        //ADMISSION MODULE:
        #region Department
        public const string CreateDepartment = "department";

        public const string ReadDepartments = "departments/key/{key}";

        public const string ReadDepartmentByKey = "department/key/{key}";

        public const string UpdateDepartment = "department/{key}";

        public const string DeleteDepartment = "department/{key}";

        #endregion

        #region Firm
        public const string CreateFirm = "firm";

        public const string ReadFirms = "firms";

        public const string ReadFirmsByFacilityId = "firms/facility/{facilityId}";

        public const string ReadFirmsByDepartmentId = "firms/department/{departmentId}";

        public const string ReadFirmByKey = "firm/key/{key}";

        public const string UpdateFirm = "firm/{key}";

        public const string FirmByDepartment = "firm/firm-by-department/{departmentId}";

        public const string DeleteFirm = "firm/{key}";

        #endregion

        #region Ward
        public const string CreateWard = "ward";

        public const string ReadWards = "wards";

        public const string ReadWardsByFacilityId = "wards/facility/{facilityId}";

        public const string ReadWardByKey = "ward/key/{key}";

        public const string ReadWardByFirm = "ward/ward-by-firm/{firmId}";

        public const string UpdateWard = "ward/{key}";

        public const string DeleteWard = "ward/{key}";

        #endregion

        #region Bed
        public const string CreateBed = "bed";

        public const string ReadBeds = "beds";

        public const string ReadBedsByFacilityId = "beds/facility/{facilityId}";

        public const string ReadBedByKey = "bed/key/{key}";

        public const string UpdateBed = "bed/{key}";

        public const string ReadBedByWard = "bed/bed-by-ward/{wardId}";

        public const string ReadBedByWardForDropdown = "bed/bed-by-ward-for-dropdown/{wardId}";

        public const string DeleteBed = "bed/{key}";

        #endregion

        //SURGERY MODULE:
        #region Surgery
        public const string CreateSurgery = "surgery";

        public const string ReadSurgerys = "surgerys";

        public const string ReadSurgeryByKey = "surgery/key/{key}";

        public const string ReadSurgeryByClientId = "surgery/by-client/{clientId}";

        public const string UpdateSurgery = "surgery/{key}";

        public const string UpdateSurgeryPreOp = "surgery/pre-op/{key}";

        public const string UpdateSurgeryIntraOp = "surgery/intra-op/{key}";

        public const string UpdateSurgeryPostOp = "surgery/post-op/{key}";

        #endregion

        //BIRTH RECORD MODULE
        #region BirthRecord
        public const string CreateBirthRecord = "birth-record";

        public const string ReadBirthRecords = "birth-records";

        public const string ReadBirthRecordByKey = "birth-record/key/{key}";

        public const string ReadBirthRecordByClient = "birth-record/by-client/{clientId}";

        public const string UpdateBirthRecord = "birth-record/{key}";

        public const string DeleteBirthRecord = "birth-record/{key}";
        #endregion

        //DEATH RECORD MODULE
        #region DeathRecord
        public const string CreateDeathRecord = "death-record";

        public const string ReadDeathRecords = "death-records";

        public const string ReadDeathRecordByKey = "death-record/key/{key}";

        public const string ReadDeathRecordByClient = "death-record/by-client/{clientId}";

        public const string UpdateDeathRecord = "death-record/{key}";

        public const string DeleteDeathRecord = "death-record/{key}";
        #endregion

        #region AnatomicAxis
        public const string CreateAnatomicAxis = "anatomic-axis";

        public const string ReadAnatomicAxes = "anatomic-axes";

        public const string ReadAnatomicAxisByKey = "anatomic-axis/key/{key}";

        public const string UpdateAnatomicAxis = "anatomic-axis/{key}";

        public const string DeleteAnatomicAxis = "anatomic-axis/{key}";
        #endregion

        #region DeathCause
        public const string CreateDeathCause = "death-cause";

        public const string ReadDeathCauses = "death-causes";

        public const string ReadDeathCauseByKey = "death-cause/key/{key}";

        public const string ReadDeathCauseByDeathRecord = "death-cause/by-death-record/{deathRecordId}";

        public const string UpdateDeathCause = "death-cause/{key}";

        public const string DeleteDeathCause = "death-cause/{key}";
        #endregion

        #region PathologyAxis
        public const string CreatePathologyAxis = "pathology-axis";

        public const string ReadPathologyAxes = "pathology-axes";

        public const string ReadPathologyAxisByKey = "pathology-axis/key/{key}";

        public const string UpdatePathologyAxis = "pathology-axis/{key}";

        public const string DeletePathologyAxis = "pathology-axis/{key}";
        #endregion

        #region ICPC2Description
        public const string CreateICPC2Description = "ICPC2-description";

        public const string ReadICPC2Descriptions = "ICPC2-descriptions";

        public const string ReadICPC2DescriptionByKey = "ICPC2-description/key/{key}";

        public const string ReadICPC2DescriptionByAnatomicAxis = "ICPC2-description/by-anatomicaxis/{anatomicAxisId}";

        public const string ReadICPC2DescriptionByPathologyAxis = "ICPC2-description/by-pathology-axis/{pathologyAxisId}/{anatomicAxisId}";

        public const string UpdateICPC2Description = "ICPC2-description/{key}";

        public const string DeleteICPC2Description = "ICPC2-description/{key}";
        #endregion

        // Pain Scale
        #region PainScale
        public const string CreatePainScale = "pain-scale";

        public const string ReadPainScales = "pain-scales";

        public const string ReadPainScaleByKey = "pain-scale/key/{key}";

        public const string UpdatePainScale = "pain-scale/{key}";

        public const string DeletePainScale = "pain-scale/{key}";
        #endregion

        #region PainRecord
        public const string CreatePainRecord = "pain-record";

        public const string ReadPainRecords = "pain-records";

        public const string ReadPainRecordByKey = "pain-record/key/{key}";

        public const string ReadPainRecordByClient = "pain-record/by-client/{clientId}";

        public const string ReadPainRecordByEncounter = "pain-record/by-encounter/{encounterId}";

        public const string UpdatePainRecord = "pain-record/{key}";

        public const string DeletePainRecord = "pain-record/{key}";

        public const string RemovePainRecord = "pain-record/remove/{key}";
        #endregion

        //NURSING PLAN MODULE
        #region NursingPlan

        public const string CreateNursingPlan = "nursing-plan";

        public const string ReadNursingPlans = "nursing-plans";

        public const string ReadNursingPlanByKey = "nursing-plan/key/{key}";

        public const string ReadNursingPlanByClient = "nursing-plan/by-client/{clientId}";

        public const string UpdateNursingPlan = "nursing-plan/{key}";

        public const string DeleteNursingPlan = "nursing-plan/{key}";
        #endregion

        #region TurningChart

        public const string CreateTurningChart = "turning-chart";

        public const string ReadTurningCharts = "turning-charts";

        public const string ReadTurningChartByKey = "turning-chart/key/{key}";

        public const string ReadTurningChartByClient = "turning-chart/by-client/{clientId}";

        public const string UpdateTurningChart = "turning-chart/{key}";

        public const string DeleteTurningChart = "turning-chart/{key}";

        #endregion

        #region Fluid

        public const string CreateFluid = "fluid";

        public const string ReadFluids = "fluids";

        public const string ReadFluidByKey = "fluid/key/{key}";

        public const string ReadFluidByClient = "fluid/by-client/{clientId}";

        public const string UpdateFluid = "fluid/{key}";

        public const string DeleteFluid = "fluid/{key}";

        #endregion

        #region FluidInput

        public const string CreateFluidInput = "fluid-input";

        public const string ReadFluidInputs = "fluid-inputs";

        public const string ReadFluidInputByKey = "fluid-input/key/{key}";

        public const string ReadFluidInputByEncounter = "fluid-input/by-encounter/{encounterId}";

        public const string ReadFluidInputByFluid = "fluid-input/by-client/{fluidId}";

        public const string UpdateFluidInput = "fluid-input/{key}";

        public const string DeleteFluidInput = "fluid-input/{key}";

        #endregion

        #region FluidOutput

        public const string CreateFluidOutput = "fluid-output";

        public const string ReadFluidOutputs = "fluid-outputs";

        public const string ReadFluidOutputByKey = "fluid-output/key/{key}";

        public const string ReadFluidOutputByEncounter = "fluid-output/by-encounter/{encounterId}";

        public const string ReadFluidOutputByFluid = "fluid-output/by-client/{fluidId}";

        public const string DeleteFluOutput = "fluid-output/{key}";

        #endregion

        //COVAX MODULE
        #region Covax

        public const string CreateCovax = "covax";

        public const string ReadCovaxes = "covaxes";

        public const string ReadCovaxByKey = "covax/key/{key}";

        public const string ReadCovaxByClient = "covax/by-client/{clientId}";

        public const string UpdateCovax = "covax/{key}";

        public const string DeleteCovax = "covax/{key}";

        #endregion

        #region VaccineDose
        public const string CreateVaccineDose = "vaccine-dose";

        public const string ReadVaccineDoses = "vaccines-doses";

        public const string ReadVaccineDoseByKey = "vaccine-dose/key/{key}";

        public const string UpdateVaccineDose = "vaccine-dose/{key}";

        public const string DeleteVaccineDose = "vaccine-dose/{key}";

        public const string VaccineDosesByVaccineName = "vaccine-dose/by-vaccinename/{vaccineId}";
        #endregion

        #region AdverseEvent
        public const string CreateAdverseEvent = "adverse-event";

        public const string ReadAdverseEvents = "adverse-events";

        public const string ReadAdverseEventByKey = "adverse-event/key/{key}";

        public const string ReadAdverseEventByEncounter = "adverse-event/by-encounter/{encounterId}";

        public const string ReadAdverseEventByImmunization = "adverse-event/by-immunization/{immunizationId}";

        public const string UpdateAdverseEvent = "adverse-event/{key}";

        public const string DeleteAdverseEvent = "adverse-event/{key}";
        #endregion

        //PARTOGRAPH MODULE
        #region Partograph

        public const string CreatePartograph = "partograph";

        public const string ReadPartographs = "partographs";

        public const string ReadPartographByKey = "partograph/key/{key}";

        public const string ReadPartographByEncounterId = "partograph/by-encounter/{encounterId}";

        public const string ReadPartographByClient = "partograph/by-client/{clientId}";

        public const string UpdatePartograph = "partograph/{key}";

        public const string DeletePartograph = "partograph/{key}";
        #endregion

        #region PartographDetails

        public const string CreatePartographDetail = "partograph-detail";

        public const string ReadPartographDetails = "partographs-details";

        public const string ReadPartographDetailByKey = "partograph-detail/key/{key}";

        public const string ReadPartographDetailByAdmissionKey = "partograph-detail/by-client/{admissionId}";

        public const string ReadPartographDetailByPartographKey = "partograph-detail/by-client/{partographId}";

        public const string ReadPartographDetailByPartograph = "partograph-detail/by-client/partographId/{partographId}";

        public const string UpdatePartographDetail = "partograph-detail/{key}";

        #endregion

        #region FetalHeartRate
        public const string CreateFetalHeartRate = "fetal-heart-rate";

        public const string ReadFetalHeartRates = "fetal-heart-rates";
        #endregion

        #region Liquor
        public const string CreateLiquor = "liquor";
        #endregion

        #region Moulding
        public const string CreateMoulding = "moulding";
        #endregion

        #region Cervix
        public const string CreateCervix = "cervix";

        public const string ReadCervixs = "cervixs";
        #endregion

        #region DescentOfHead
        public const string CreateDescentOfHead = "descent-of-head";

        public const string ReadDescentOfHeads = "descent-of-heads";
        #endregion

        #region Contraction
        public const string CreateContraction = "contraction";

        public const string ReadContractions = "contractions";
        #endregion

        #region Oxytocin
        public const string CreateOxytocin = "oxytocin";

        public const string ReadOxytocins = "oxytocins";
        #endregion

        #region Drop
        public const string CreateDrop = "drop";

        public const string ReadDrops = "drops";
        #endregion

        #region Medicine
        public const string CreateMedicine = "medicine";
        #endregion

        #region MedicineBrand

        public const string CreateMedicineBrand = "medicine-brand";

        public const string ReadMedicineBrand = "medicine-brands";

        public const string ReadMedicineBrandByKey = "medicine-brand/key/{key}";

        public const string UpdateMedicineBrand = "medicine-brand/{key}";

        public const string DeleteMedicineBrand = "medicine-brand/{key}";
        #endregion

        #region MedicineManufacturer

        public const string CreateMedicineManufacturer = "medicine-manufacturer";

        public const string ReadMedicineManufacturer = "medicine-manufacturers";

        public const string ReadMedicineManufacturerByKey = "medicine-manufacturer/key/{key}";

        public const string UpdateMedicineManufacturer = "medicine-manufacturer/{key}";

        public const string DeleteMedicineManufacturer = "medicine-manufacturer/{key}";
        #endregion

        #region StoppingReason

        public const string CreateStoppingReason = "stopping-reason";

        public const string ReadStoppingReason = "stopping-reasons";

        public const string ReadStoppingReasonByKey = "stopping-reason/key/{key}";

        public const string UpdateStoppingReason = "stopping-reason/{key}";

        public const string DeleteStoppingReason = "stopping-reason/{key}";
        #endregion

        #region Risks

        public const string CreateRisk = "risk";

        public const string ReadRisk = "risks";

        public const string ReadRiskByKey = "risk/key/{key}";

        public const string UpdateRisk = "risk/{key}";

        public const string DeleteRisk = "risk/{key}";


        #endregion


        #region BloodPressure
        public const string CreateBloodPressure = "blood-pressure";

        public const string ReadBloodPressure = "blood-pressures";
        #endregion

        #region Pulse
        public const string CreatePulse = "pulse";

        public const string ReadPulses = "pulses";
        #endregion

        #region Temperature
        public const string CreateTemperature = "temperature";

        public const string ReadTemperatures = "temperatures";
        #endregion

        #region Protein
        public const string CreateProtein = "protein";
        #endregion

        #region Volume

        public const string CreateVolume = "volume";

        #endregion

        #region Acentone

        public const string CreateAcentone = "acentone";

        #endregion

        #region BirthDetail
        public const string CreateBirthDetail = "birth-detail";

        public const string ReadBirthDetails = "birth-details";

        public const string ReadBirthDetailByEncounter = "birth-details/by-encounter/{encounterId}";

        public const string ReadBirthDetailByKey = "birth-detail/key/{key}";

        public const string UpdateBirthDetail = "birth-detail/{key}";

        #endregion

        //COVID MODULE
        #region Covid

        public const string CreateCovid = "covid";

        public const string ReadCovids = "covids";

        public const string ReadCovidByKey = "covid/key/{key}";

        public const string ReadCovidByClient = "covid/by-client/{clientId}";

        public const string UpdateCovid = "covid/{key}";

        public const string DeleteCovid = "covid/{key}";
        #endregion

        #region Comobidity

        public const string CreateComobidity = "comobidity";

        public const string ReadComobidities = "comobidities";

        public const string ReadComobidityByKey = "comobidity/key/{key}";

        public const string ReadComobidityByClient = "comobidity/by-client/{clientId}";

        public const string UpdateComobidity = "comobidity/{key}";

        public const string DeleteComobidity = "comobidity/{key}";
        #endregion

        //GynConfirmation MODULE
        #region GynConfirmation

        public const string ReadGynConfirmations = "gyn-confirmations";

        public const string CreateGynConfirmation = "gyn-confirmation";

        public const string ReadGynConfirmationByKey = "gyn-confirmation/key/{key}";

        public const string UpdateGynConfirmation = "gyn-confirmation/{key}";

        public const string DeleteGynConfirmation = "gyn-confirmation/{key}";
        #endregion

        #region CovidSymptom

        public const string CreateCovidSymptom = "covid-symptom";

        public const string ReadCovidSymptoms = "covid-symptoms";

        public const string ReadCovidSymptomByKey = "covid-symptom/key/{key}";

        public const string UpdateCovidSymptom = "covid-symptom/{key}";

        public const string DeleteCovidSymptom = "covid-symptom/{key}";

        #endregion

        #region CovidsymptomScreening

        public const string CreateCovidsymptomScreening = "covid-symptom-screening";

        public const string ReadCovidsymptomScreenings = "covid-symptom-screenings";

        public const string ReadCovidsymptomScreeningByKey = "covid-symptom-screening/key/{key}";

        public const string UpdateCovidsymptomScreening = "covid-symptom-screening/{key}";

        public const string DeleteCovidsymptomScreening = "covid-symptom-screening/{key}";

        #endregion

        //Pharmacy
        #region GenericDrug
        public const string CreateGenericMedicine = "generic-medicine";

        public const string ReadGenericMedicines = "generic-medicines";

        public const string SearchGenericMedicines = "search-generic-medicines";

        public const string ReadGenericMedicineByKey = "generic-medicine/key/{key}";

        public const string ReadGenericMedicineByInteractionId = "generic-medicine/{interactionid}";

        public const string UpdateGenericMedicine = "generic-medicine/{key}";

        public const string DeleteGenericMedicine = "generic-medicine/{key}";

        public const string GenericMedicineByDrugUtilityid = "generic-medicine/generic-medicine-by-drug-utility/{drugUtilityId}";

        public const string GenericMedicineByDrugSubClassid = "generic-medicine/generic-medicine-by-drug-subclass/{drugSubclassId}";

        //  public const string ReadGeneralMedicationByPrescriptionId = "general-medication/key/{prescriptionId}";
        public const string ReadGeneralMedicationByPrescriptionId = "general-medication/prescriptionId/{prescriptionId}";

        public const string ReadGeneralMedicationByInteractionId = "general-medication/interaction/{interactionId}";

        public const string ReadGeneralMedicationDtoByPrescriptionId = "general-medicationdto/key/{prescriptionId}";

        #endregion

        #region DrugClass

        public const string CreateDrugClass = "drug-class";

        public const string ReadDrugClasses = "drug-classes";

        public const string ReadDrugClassByKey = "drug-class/key/{key}";

        public const string UpdateDrugClass = "drug-class/{key}";

        public const string DeleteDrugClass = "drug-class/{key}";

        #endregion

        #region SystemRelevance

        public const string CreateSystemRelevance = "system-relevance";

        public const string ReadSystemRelevance = "system-relevance";

        public const string ReadSystemRelevanceByKey = "system-relevance/key/{key}";

        public const string UpdateSystemRelevance = "system-relevance/{key}";

        public const string DeleteSystemRelevance = "system-relevance/{key}";
        #endregion

        #region DrugSubclass

        public const string CreateDrugSubclass = "drug-subclass";

        public const string ReadDrugSubclasses = "drug-subclasses";

        public const string ReadDrugSubclassByKey = "drug-subclass/key/{key}";

        public const string UpdateDrugSubclass = "drug-subclass/{key}";

        public const string RouteOfAdministration = "Route-administration";

        public const string ReadDrugSubClassByClass = "drug-subclass/by-class/{drugclassId}";

        public const string DeleteDrugSubclass = "drug-subclass/{key}";

        #endregion

        #region Drug Route
        public const string ReadDrugRoutes = "drug-routes";

        public const string CreateDrugRoute = "drug-route";

        public const string ReadDrugRouteByKey = "drug-route/key/{key}";

        public const string UpdateDrugRoute = "drug-route/{key}";

        public const string DeleteDrugRoute = "drug-route/{key}";
        #endregion

        #region GeneralDrugDefination
        public const string ReadGeneralDrugDefination = "general-drugdefinations";

        public const string CreateGeneralDrugDefination = "general-drugdefination";

        public const string ReadGeneralDrugDefinationByKey = "general-drugdefination/key/{key}";

        public const string UpdateGeneralDrugDefination = "general-drugdefination/{key}";

        public const string DeleteGeneralDrugDefination = "general-drugdefination/{key}";
        #endregion

        #region DrugUtility
        public const string ReadDrugUtilities = "drug-utilities";

        public const string ReadDrugUtilityByKey = "drug-utility/key/{key}";

        public const string CreateDrugUtility = "drug-utility";

        public const string UpdateDrugUtility = "drug-utility/{key}";

        public const string DeleteDrugUtility = "drug-utility/{key}";
        #endregion

        #region DrugDosage
        public const string ReadDrugDosages = "drug-dosages";

        public const string ReadDrugDosageByKey = "drug-dosage/key/{key}";

        public const string CreateDrugDosage = "drug-dosage";

        public const string UpdateDrugDosage = "drug-dosage/{key}";

        public const string DeleteDrugDosage = "drug-dosage/{key}";
        #endregion

        #region Prescription
        public const string CreatePrescription = "prescription";

        public const string MultiplePrescription = "multiple-prescription";

        public const string ReadPrescriptionsByEncounter = "prescription/by-encounter/{encounterId}";

        public const string ReadPrescriptions = "prescriptions";

        public const string ReadPrescriptionByKey = "prescription/key/{key}";

        public const string UpdatePrescription = "prescription/{key}";

        public const string ReadPrescriptionByClientId = "prescription/by-client/{clientId}";

        public const string ReadLatestPrescriptionByClientId = "Latest-prescription/by-client/{clientId}";

        public const string ReadPrescriptionForDispenseByClientId = "prescription-for-dispense/by-client/{clientId}";

        public const string ReadPrescriptionsByDate = "prescriptions/by-date";

        public const string ReadPharmacyDashBoard = "prescription/pharmacy-dashboard/{facilityId}";

        public const string CreateDispense = "dispense";
        #endregion

        #region General Medications
        public const string CreateGeneralMedication = "general-medication";

        public const string ReadGeneralMedications = "general-medications";

        public const string ReadGeneralMedicationByKey = "general-medication/key/{key}";

        public const string ReadDispenseDetailPrescriptionByKey = "general-dispense-detail/key/{key}";

        public const string UpdateGeneralMedication = "general-medication/{key}";

        public const string DeleteGeneralMedication = "general-medication/{key}";
        #endregion

        //SpecialMedication
        #region Special Medications
        public const string CreateSpecialMedication = "special-medication";

        public const string ReadSpecialMedications = "special-medications";

        public const string ReadSpecialMedicationByKey = "special-medication/key/{key}";

        public const string UpdateSpecialMedication = "special-medication/{key}";
        #endregion

        #region Frequency Interval
        public const string ReadFrequencyIntervals = "frequency-intervals";

        public const string ReadFrequencyIntervalByTimeIntervals = "frequency-interval/by-time-intervals";

        public const string ReadFrequencyIntervalByFrequency = "frequency-interval-by-frequency";

        public const string ReadFrequencyIntervalByKey = "frequency-interval/key/{key}";

        public const string CreateFrequencyInterval = "frequency-interval";

        public const string UpdateFrequencyInterval = "frequency-interval/{key}";

        public const string DeleteFrequencyInterval = "frequency-interval/{key}";
        #endregion

        #region Drug Formulation 
        public const string ReadDrugFormulations = "drug-formulations";

        public const string ReadDrugFormulationByKey = "drug-formulation/key/{key}";

        public const string CreateDrugFormulation = "drug-formulation";

        public const string UpdateDrugFormulation = "drug-formulation/{key}";

        public const string DeleteDrugFormulation = "drug-formulation/{key}";
        #endregion

        #region Special Drug
        public const string ReadSpecialDrugs = "special-drugs";

        public const string ReadSpecialDrugByKey = "special-drug/key/{key}";

        public const string ReadSpecialDrugByRegimenId = "special-drugs/by-regimen/{regimenId}";

        public const string CreateSpecialDrug = "special-drug";

        public const string UpdateSpecialDrug = "special-drug/{key}";

        public const string DeleteSpecialDrug = "special-drug/{key}";
        #endregion

        #region Dispance
        public const string CreateDispance = "dispance";
        #endregion

        #region Drug Regimen
        public const string DrugRegimens = "drug-Regimens";

        public const string ReadDrugRegimensByRegimen = "drug-Regimens/bydrug-regimen/{regimenfor}";

        public const string ReadDrugRegimenByKey = "drug-Regimen/key/{key}";

        public const string CreateDrugRegimen = "drug-Regimen";

        public const string UpdateDrugRegimen = "drug-Regimen/{key}";

        public const string DeleteDrugRegimen = "drug-Regimen/{key}";
        #endregion

        #region Drug Definition
        public const string DrugDefinition = "drug-Definition";
        #endregion

        //ART
        #region ART        
        public const string CreateAttachedFacility = "attached-facility";

        public const string ReadAttachedFacility = "attached-facility";

        public const string ReadAttachedFacilityByKey = "attached-facility/key/{key}";

        public const string UpdateAttachedFacility = "attached-facility/{key}";

        public const string ReadAttachedFacilityByClient = "attached-facility/by-client/{clientId}";

        #endregion

        #region ARTRegister
        public const string CreateARTRegister = "art-register";

        public const string ReadARTRegister = "art-registers";

        public const string ReadARTRegisterByKey = "art-register/key/{key}";

        public const string UpdateARTRegister = "art-register/{key}";

        public const string ReadARTRegisterByClient = "art-register/by-client/{clientId}";

        public const string ReadARTNumberByClient = "art-registernumber/by-client/{clientId}";

        public const string ReadGeneratedARTNumber = "art-generatednumber/by-facility/{facilityId}";
        #endregion

        #region Family Member
        public const string CreateFamilyMember = "family-member";

        public const string ReadFamilyMembers = "family-members";

        public const string ReadFamilyMemberByKey = "family-member/key/{key}";

        public const string UpdateFamilyMember = "family-member/{key}";

        public const string ReadFamilyMemberByClient = "family-member/by-client/{clientId}";
        #endregion

        #region Patient Status
        public const string CreatePatientStatus = "patient-status";

        public const string ReadPatientStatuses = "patient-statuses";

        public const string ReadPatientStatusByKey = "patient-status/key/{key}";

        public const string UpdatePatientStatus = "patient-status/{key}";

        public const string ReadPatientStatusByArtRegisterId = "patient-status/by-art-register/{artRegisterId}";
        #endregion

        #region Pregnency Booking
        public const string CreatePregnencyBooking = "pregnency-booking";

        public const string ReadPregnencyBookings = "pregnency-bookings";

        public const string ReadPregnencyBookingByKey = "pregnency-booking/key/{key}";

        public const string UpdatePregnencyBooking = "pregnency-booking/{key}";
        #endregion

        #region PriorARTExposer
        public const string CreatePriorARTExposer = "prior-art-exposer";

        public const string ReadPriorARTExposers = "prior-art-exposers";

        public const string ReadPriorARTExposerByKey = "prior-art-exposer/key/{key}";

        public const string ReadPriorARTExposerByClient = "prior-art-exposer/by-client/{clientId}";

        public const string UpdatePriorARTExposer = "prior-art-exposer/{key}";

        public const string DeletePriorARTExposer = "prior-art-exposer/{key}";
        #endregion

        #region UseTBIdentificationMehtods
        public const string CreateUseTBIdentificationMehtod = "use-tb-identification";

        public const string ReadUseTBIdentificationMehtods = "use-tb-identifications";

        public const string ReadUseTBIdentificationMehtodByKey = "use-tb-identification/key/{key}";

        public const string ReadUseTBIdentificationMehtodByEncounter = "use-tb-identification/by-encounter/{encounterId}";

        public const string UpdateUseTBIdentificationMehtod = "use-tb-identification/{key}";

        public const string DeleteUseTBIdentificationMehtod = "use-tb-identification/{key}";
        #endregion

        #region TBIdentificationMethod
        public const string CreateTBIdentificationMethod = "tb-identification-method";

        public const string ReadTBIdentificationMethods = "tb-identification-methods";

        public const string ReadTBIdentificationMethodByKey = "tb-identification-method/key/{key}";

        public const string UpdateTBIdentificationMethodMehtod = "tb-identification-method/{key}";

        public const string DeleteTBIdentificationMethodMehtod = "tb-identification-method/{key}";
        #endregion

        #region ARTDrugClass
        public const string ReadARTDrugClass = "art-drug-class";

        public const string CreateARTDrugClass = "art-drug-class";

        public const string ReadARTDrugClassByKey = "art-drug-class/key/{key}";

        public const string UpdateARTDrugClass = "art-drug-class/{key}";

        public const string DeleteARTDrugClass = "art-drug-class/{key}";
        #endregion

        #region ARTDrug
        public const string CreateARTDrug = "art-drug";

        public const string ReadARTDrugs = "art-drugs";

        public const string ReadARTDrugByARTDrugClass = "art-drug-by-art-drug-class/{artDrugClassId}";

        public const string ReadARTDrugByKey = "art-drug/key/{key}";

        public const string UpdateARTDrug = "art-drug/{key}";

        public const string DeleteARTDrug = "art-drug/{key}";
        #endregion

        #region TakenARTDrug

        public const string CreateTakenARTDrug = "taken-art-drug";

        public const string ReadTakenARTDrugs = "taken-art-drugs";

        public const string ReadTakenARTDrugByKey = "taken-art-drug/key/{key}";

        public const string UpdateTakenARTDrug = "taken-art-drug/{key}";

        public const string DeleteTakenARTDrug = "taken-art-drug/{key}";

        #endregion

        #region TPTDrug
        public const string CreateTPTDrug = "tpt-drug";

        public const string ReadTPTDrugs = "tpt-drugs";

        public const string ReadTPTDrugByKey = "tpt-drug/key/{key}";

        public const string UpdateTPTDrug = "tpt-drug/{key}";

        public const string DeleteTPTDrug = "tpt-drug/{key}";
        #endregion

        #region TakenTPTDrug

        public const string CreateTakenTPTDrug = "taken-tpt-drug";

        public const string ReadTakenTPTDrugs = "taken-tpt-drugs";

        public const string ReadTakenTPTDrugByKey = "taken-tpt-drug/key/{key}";

        public const string UpdateTakenTPTDrug = "taken-tpt-drug/{key}";

        public const string DeleteTakenTPTDrug = "taken-tpt-drug/{key}";

        #endregion

        #region TBHistory
        public const string CreateTBHistory = "tb-history";

        public const string ReadTBHistories = "tb-histories";

        public const string ReadTBHistoryByKey = "tb-history/key/{key}";

        public const string ReadTBHistoryByClient = "tb-history/by-client/{clientId}";

        public const string UpdateTBHistory = "tb-history/{key}";

        public const string DeleteTBHistory = "tb-history/{key}";
        #endregion

        #region TPTHistory
        public const string CreateTPTHistory = "tpt-history";

        public const string ReadTPTHistories = "tpt-histories";

        public const string ReadTPTHistoryByKey = "tpt-history/key/{key}";

        public const string ReadTPTHistoryByClient = "tpt-history/by-client/{clientId}";

        public const string UpdateTPTHistory = "tpt-history/{key}";

        public const string DeleteTPTHistory = "tpt-history/{key}";
        #endregion

        #region TBDrug
        public const string CreateTBDrug = "tb-drug";

        public const string ReadTBDrugs = "tb-drugs";

        public const string ReadTBDrugByKey = "tb-drug/key/{key}";

        public const string UpdateTBDrug = "tb-drug/{key}";

        public const string DeleteTBDrug = "tb-drug/{key}";
        #endregion

        #region TakenTBDrug
        public const string CreateTakenTBDrug = "taken-tb-drug";

        public const string ReadTakenTBDrugs = "taken-tb-drugs";

        public const string ReadTakenTBDrugByKey = "taken-tb-drug/key/{key}";

        public const string UpdateTakenTBDrug = "taken-tb-drug/{key}";

        public const string DeleteTakenTBDrug = "taken-tb-drug/{key}";
        #endregion

        #region ARTDrugAdherence
        public const string CreateARTDrugAdherence = "art-drug-dherence";

        public const string ReadARTDrugAdherences = "art-drug-dherences";

        public const string ReadARTDrugAdherenceByKey = "art-drug-dherence/key/{key}";

        public const string ReadARTDrugAdherenceByClient = "art-drug-adherence/by-client/{clientId}";

        public const string UpdateARTDrugAdherence = "art-drug-dherence/{key}";

        public const string DeleteARTDrugAdherence = "art-drug-dherence/{key}";
        #endregion

        #region ARTResponse
        public const string CreateARTResponse = "art-response";

        public const string ReadARTResponses = "art-responses";

        public const string ReadARTResponseByKey = "art-response/key/{key}";

        public const string ReadARTResponseByClient = "art-response/by-client/{clientId}";

        public const string UpdateARTResponse = "art-response/{key}";

        public const string DeleteARTResponse = "art-response/{key}";
        #endregion

        #region ClientsDisabilities
        public const string CreateClientsDisability = "clients-disability";

        public const string ReadClientsDisabilities = "clients-disabilities";

        public const string ReadClientsDisabilityByKey = "clients-disability/key/{key}";

        public const string ReadClientsDisabilityByClient = "clients-disability/by-client/{clientId}";

        public const string UpdateClientsDisability = "clients-disability/{key}";

        public const string DeleteClientsDisability = "clients-disability/{key}";
        #endregion

        #region ART treatment plans
        public const string CreateARTTreatmentPlan = "art-treatmentplan";

        public const string ReadARTTreatmentPlan = "art-treatmentplans";

        public const string ReadARTTreatmentPlanByKey = "art-treatmentplan/key/{key}";

        public const string ReadARTTreatmentPlanByClient = "art-treatmentplan/by-client/{clientId}";

        public const string UpdateARTTreatmentPlan = "art-treatmentplan/{key}";

        public const string DeleteARTTreatmentPlan = "art-treatmentplan/{key}";
        #endregion

        #region Disabilities

        public const string ReadDisabilities = "disabilities";

        public const string CreateDisability = "disability";

        public const string ReadDisabilityByKey = "disability/key/{key}";

        public const string UpdateDisability = "disability/{key}";

        public const string DeleteDisability = "disability/{key}";
        #endregion

        #region DSDAssesments
        public const string CreateDSDAssesment = "dsd-assesment";

        public const string ReadDSDAssesments = "dsd-assesments";

        public const string ReadDSDAssesmentByKey = "dsd-assesment/key/{key}";

        public const string ReadDSDAssesmentByClient = "dsd-assesment/by-client/{clientId}";

        public const string UpdateDSDAssesment = "dsd-assesment/{key}";

        public const string DeleteDSDAssesment = "dsd-assesment/{key}";
        #endregion

        //UNDER FIVE MODULE:
        #region Nutrition
        public const string CreateNutrition = "nutrition";

        public const string ReadNutritions = "nutritions";

        public const string ReadNutritionByKey = "nutrition/key/{key}";

        public const string ReadNutritionByClient = "nutrition/by-client/{clientId}";

        public const string UpdateNutrition = "nutrition/{key}";

        public const string DeleteNutrition = "nutrition/{key}";
        #endregion

        #region FeedingHistory
        public const string CreateFeedingHistory = "feeding-history";

        public const string ReadFeedingHistories = "feeding-histories";

        public const string ReadFeedingHistoryByKey = "feeding-history/key/{key}";

        public const string ReadFeedingHistoryByClient = "feeding-history/by-client/{clientId}";

        public const string UpdateFeedingHistory = "feeding-history/{key}";

        public const string DeleteFeedingHistory = "feeding-history/{key}";
        #endregion

        #region CounsellingService
        public const string CreateCounsellingService = "counselling-service";

        public const string ReadCounsellingServices = "counselling-services";

        public const string ReadCounsellingServiceByKey = "counselling-service/key/{key}";

        public const string ReadCounsellingServiceByClient = "counselling-service/by-client/{clientId}";

        public const string UpdateCounsellingService = "counselling-service/{key}";

        public const string DeleteCounsellingService = "counselling-service/{key}";
        #endregion

        #region ServiceQueue
        public const string CreateServiceQueue = "service-queue";

        public const string ReadServiceQueues = "service-queues";

        public const string ReadLatestServiceQueues = "latest-service-queues/{servicePointId}";

        //  public const string ReadLatestServiceQueuesByClient = "latest-service-queues/{servicePointId}/{id}";

        public const string ReadServiceQueueByKey = "service-queue/key/{key}";

        public const string ReadServiceQueueByClient = "service-queue/by-client/{clientId}";

        public const string ReadServiceQueueByEncounterId = "service-queue/by-encounter/{encounterId}";

        public const string UpdateServiceQueue = "service-queue/{key}";
        public const string UpdateServiceQueueByClientId = "service-queue/{clientId}/{facilityId}/{ServiceCode}";

        public const string DeleteServiceQueue = "service-queue/{key}";
        #endregion

        #region TBSuspectingReason

        public const string ReadTBSuspectingReasons = "tb-suspecting-reasons";

        #endregion

        #region WHOClinicalStage
        public const string CreateWHOClinicalStage = "who-clinical-stage";

        public const string ReadWHOClinicalStages = "who-clinical-stages";

        public const string ReadWHOClinicalStageByKey = "who-clinical-stage/key/{key}";

        public const string UpdateWHOClinicalStage = "who-clinical-stage/{key}";

        public const string DeleteWHOClinicalStage = "who-clinical-stage/{key}";
        #endregion

        #region TBFinding
        public const string CreateTBFinding = "tB-finding";

        public const string ReadTBFindings = "tB-findings";

        public const string ReadTBFindingByKey = "tB-finding/key/{key}";

        public const string UpdateTBFinding = "tB-finding/{key}";

        public const string DeleteTBFinding = "tB-finding/{key}";
        #endregion

        #region IdentifiedTBFinding
        public const string CreateIdentifiedTBFinding = "identified-tbfinding";

        public const string ReadIdentifiedTBFindings = "identified-tbfindings";

        public const string ReadIdentifiedTBFindingByKey = "identified-tbfinding/key/{key}";

        public const string ReadIdentifiedTBFindingByClient = "identified-tbfinding/by-client/{clientId}";

        public const string ReadIdentifiedTBFindingTBFindingId = "identified-tbfinding/by-tbfinding/{tbfindingId}";

        public const string ReadIdentifiedTBFindingByEncounterId = "identified-tbfinding/by-encounter/{encounterId}";

        public const string UpdateIdentifiedTBFinding = "update-identified-tbfinding/{key}";

        public const string DeleteIdentifiedTBFinding = "identified-tbfinding/{key}";

        public const string RemoveIdentifiedTBFinding = "identified-tbfinding/remove/{encounterId}";
        #endregion

        #region WHOStagesCondition
        public const string CreateWHOStagesCondition = "who-stages-condition";

        public const string ReadWHOStagesConditions = "who-stages-conditions";

        public const string ReadWHOStagesConditionByKey = "who-stages-condition/key/{key}";

        public const string UpdateWHOStagesCondition = "who-stages-condition/{key}";

        public const string DeleteWHOStagesCondition = "who-stages-condition/{key}";
        #endregion

        #region IdentifiedReason
        public const string CreateIdentifiedReason = "identified-reason";

        public const string ReadIdentifiedReasons = "identified-reasons";

        public const string ReadIdentifiedReasonByKey = "identified-reason/key/{key}";

        public const string ReadIdentifiedReasonByClient = "identified-reason/by-client/{clientId}";

        public const string ReadIdentifiedReasonByEncounterId = "identified-reason/by-encounter/{encounterId}";

        public const string UpdateIdentifiedReason = "update-identified-reason/{key}";

        public const string RemoveIdentifiedReason = "identified-reason/remove/{key}";
        #endregion

        #region Dot
        public const string CreateDot = "dot";

        public const string ReadDots = "dots";

        public const string ReadDotByKey = "dot/key/{key}";

        public const string ReadActiveDotByClient = "dot/client/{clientId}";

        public const string ReadDotByTBService = "dot/{tbserviceid}";

        public const string UpdateDot = "dot/{key}";

        public const string DeleteDot = "dot/{key}";

        public const string CreateDotCalender = "dotcalender";

        public const string AddMonthDotCalender = "AddMonth";
        #endregion

        #region VMMCService
        public const string CreateVMMCService = "vmmc-service";

        public const string CreateAirwayAssessment = "airway-assessment";

        public const string CreatePredictiveTest = "predictive-test";

        public const string CreateAssessmentPlan = "assessment-plan";

        public const string ReadVMMCServices = "vmmc-services";

        public const string ReadVMMCServiceByKey = "vmmc-service/key/{key}";

        public const string ReadVMMCServiceByClient = "vmmc-service/by-client/{clientId}";

        public const string UpdateVMMCService = "vmmc-service/{key}";

        public const string DeleteVMMCService = "vmmc-service/{key}";
        #endregion

        #region OptedCircumcisionReason
        public const string CreateOptedCircumcisionReason = "opted-circumcision-reason";

        public const string ReadOptedCircumcisionReasons = "opted-circumcision-reasons";

        public const string ReadOptedCircumcisionReasonByKey = "opted-circumcision-reason/key/{key}";

        public const string ReadOptedCircumcisionReasonByCircumcisionReason = "opted-circumcision-reason/by-circumcision-reason/{circumcisionReasonId}";

        public const string ReadOptedCircumcisionReasonByVMMCService = "opted-circumcision-reason/byvmmc-service/{vmmcServiceId}";

        public const string UpdateOptedCircumcisionReason = "opted-circumcision-reason/{key}";

        public const string DeleteOptedCircumcisionReason = "opted-circumcision-reason/{key}";
        #endregion

        #region CircumcisionReason
        public const string CreateCircumcisionReason = "circumcision-reason";

        public const string ReadCircumcisionReasons = "circumcision-reasons";

        public const string ReadCircumcisionReasonByKey = "circumcision-reason/key/{key}";

        public const string UpdateCircumcisionReason = "circumcision-reason/{key}";

        public const string DeleteCircumcisionReason = "circumcision-reason/{key}";
        #endregion

        #region VMMCCampaign
        public const string CreateVMMCCampaign = "vmmc-campaign";

        public const string ReadVMMCCampaigns = "vmmc-campaigns";

        public const string ReadVMMCCampaignByKey = "vmmc-campaign/key/{key}";

        public const string UpdateVMMCCampaign = "vmmc-campaign/{key}";

        public const string DeleteVMMCCampaign = "vmmc-campaign/{key}";
        #endregion

        #region OptedVMMCCampaign
        public const string CreateOptedVMMCCampaign = "opted-vmmc-campaign";

        public const string ReadOptedVMMCCampaigns = "opted-vmmc-campaigns";

        public const string ReadOptedVMMCCampaignByKey = "opted-vmmc-campaign/key/{key}";

        public const string ReadOptedVMMCCampaignByVMMCCampaign = "opted-vmmc-campaign/by-vmmc-campaign/{vmmcCampaignId}";

        public const string ReadOptedVMMCCampaignByVMMCService = "opted-vmmc-campaign/by-vmmc-service/{vmmcServiceId}";

        public const string UpdateOptedVMMCCampaign = "opted-vmmc-campaign/{key}";

        public const string DeleteOptedVMMCCampaign = "opted-vmmc-campaign/{key}";
        #endregion

        #region TBService
        public const string CreateTBServices = "tb-service";

        public const string ReadTBServices = "tb-services";

        public const string ReadTBServiceByKey = "tb-service/key/{key}";

        public const string ReadTBServiceByClient = "tb-service/by-client/{clientId}";

        public const string ReadActiveTBServiceByClient = "tb-service/active/by-client/{clientId}";

        public const string ReadTBPlanByEncounter = "tb-service/by-encounter/{encounterId}";

        public const string UpdateTBService = "tb-service/{key}";

        public const string DeleteTBService = "tb-service/{key}";
        #endregion

        #region WHOCondition

        public const string CreateWHOConditions = "who-condition";

        public const string ReadWHOConditions = "who-conditions";

        public const string ReadWHOConditionByKey = "who-condition/key/{key}";

        public const string ReadWHOConditionByClient = "who-condition/by-client/{clientId}";

        public const string UpdateWHOCondition = "who-condition/{key}";

        public const string DeleteWHOCondition = "who-condition/{key}";
        #endregion

        #region Visit Purpose
        public const string CreateVisitPurpose = "visit-purpose";

        public const string ReadVisitPurposes = "visit-purposes";

        public const string ReadVisitPurposeByKey = "visit-purpose/key/{key}";

        public const string ReadVisitPurposeByClient = "visit-purpose/by-client/{clientId}";

        public const string ReadLatestVisitPurposesByClientId = "latest-visit-purpose-by-client/{clientId}";

        public const string UpdateVisitPurpose = "visit-purpose/{key}";

        public const string DeleteVisitPurpose = "visit-purpose/{key}";
        #endregion

        #region PMTCT
        public const string CreatePMTCT = "pmtct";

        public const string ReadPMTCTs = "pmtcts";

        public const string ReadPMTCTByKey = "pmtct/key/{key}";

        public const string ReadPMTCTByClient = "pmtct/by-client/{clientId}";

        public const string ReadPMTCTByEncounter = "pmtct/by-encounter/{encounterId}";

        public const string UpdatePMTCT = "pmtct/{key}";

        public const string DeletePMTCT = "pmtct/{key}";
        #endregion

        #region Complication
        public const string CreateComplication = "complication";

        public const string ReadComplications = "complications";

        public const string ReadComplicationByKey = "complication/key/{key}";

        public const string ReadComplicationByEncounter = "complication/by-encounter/{encounterId}";

        public const string ReadComplicationByVMMCService = "complication/by-vmmc-service/{vmmcServiceId}";

        public const string UpdateComplication = "complication/{key}";

        public const string DeleteComplication = "complication/{key}";

        public const string RemoveComplication = "complication/remove/{encounterId}";
        #endregion

        #region ComplicationType
        public const string CreateComplicationType = "complication-type";

        public const string ReadComplicationTypes = "complication-types";

        public const string ReadComplicationTypeByKey = "complication-type/key/{key}";

        public const string UpdateComplicationType = "complication-type/{key}";

        public const string DeleteComplicationType = "complication-type/{key}";
        #endregion

        #region IdentifiedComplication
        public const string CreateIdentifiedComplication = "identified-complication";

        public const string ReadIdentifiedComplications = "identified-complications";

        public const string ReadIdentifiedComplicationByKey = "identified-complication/key/{key}";

        public const string ReadIdentifiedComplicationByComplication = "identified-complication/by-complication/{complicationId}";

        public const string ReadIdentifiedComplicationByComplicationType = "identified-complication/by-complication-type/{complicationTypeId}";

        public const string ReadIdentifiedComplicationByEncounter = "identified-complication/by-encounter/{encounterId}";

        public const string UpdateIdentifiedComplication = "identified-complication/{key}";

        public const string DeleteIdentifiedComplication = "identified-complication/{key}";

        public const string RemoveIdentifiedComplication = "identified-complication/remove/{encounterId}";
        #endregion

        #region AnestheticPlan
        public const string CreateAnestheticPlan = "anesthetic-plan";

        public const string ReadAnestheticPlans = "anesthetic-plans";

        public const string ReadAnestheticPlanByKey = "anesthetic-plan/key/{key}";

        public const string ReadAnestheticPlanByEncounter = "anesthetic-plan/by-encounter/{encounterId}";

        public const string ReadAnestheticPlanBySurgery = "anesthetic-plan/by-surgery/{surgeryId}";

        public const string ReadAnestheticPlanListBySurgery = "anesthetic-plans/by-surgery/{surgeryId}";

        public const string UpdateAnestheticPlan = "anesthetic-plan/{key}";

        public const string UpdateIntraOpAnestheticPlan = "anesthetic-plan/intra-op/{key}";

        public const string DeleteAnestheticPlan = "anesthetic-plan/{key}";
        #endregion

        #region SkinPreparation
        public const string CreateSkinPreparation = "skin-preparation";

        public const string ReadSkinPreparations = "skin-preparations";

        public const string ReadSkinPreparationByKey = "skin-preparation/key/{key}";

        public const string ReadSkinPreparationByEncounter = "skin-preparation/by-encounter/{encounterId}";

        public const string ReadSkinPreparationByAnestheticPlan = "skin-preparation/by-anesthetic-plan/{anestheticPlanId}";

        public const string ReadSkinPreparationListByAnestheticPlan = "skin-preparations/by-anesthetic-plan/{anestheticPlanId}";

        public const string UpdateSkinPreparation = "skin-preparation/{key}";

        public const string DeleteSkinPreparation = "skin-preparation/{key}";
        #endregion

        #region ANCService
        public const string CreateANCService = "anc-service";

        public const string ReadANCServices = "anc-services";

        public const string ReadANCServiceByKey = "anc-service/key/{key}";

        public const string ReadANCServiceByClient = "anc-service/by-client/{clientId}";

        public const string ReadLatestANCServiceByClient = "latest-anc-service/by-client/{clientId}";

        public const string ReadANCServiceByEncounter = "anc-service/by-encounter/{encounterId}";

        public const string UpdateANCService = "anc-service/{key}";

        public const string DeleteANCService = "anc-service/{key}";
        #endregion

        #region ANCScreening
        public const string CreateANCScreening = "anc-screening";

        public const string ReadANCScreenings = "anc-screenings";

        public const string ReadANCScreeningByKey = "anc-screening/key/{key}";

        public const string ReadANCScreeningByClient = "anc-screening/by-client/{clientId}";

        public const string ReadANCScreeningByEncounter = "anc-screening/by-encounter/{encounterId}";

        public const string UpdateANCScreening = "anc-screening/{key}";

        public const string DeleteANCScreening = "anc-screening/{key}";
        #endregion

        #region MotherDetail
        public const string CreateMotherDetail = "mother-detail";

        public const string ReadMotherDetails = "mother-details";

        public const string ReadMotherDetailByKey = "mother-detail/key/{key}";

        public const string ReadMotherDetailByClient = "mother-detail/by-client/{clientId}";

        public const string ReadMotherDetailByEncounter = "mother-detail/by-encounter/{encounterId}";

        public const string UpdateMotherDetail = "mother-detail/{key}";

        public const string DeleteMotherDetail = "mother-detail/{key}";
        #endregion

        #region ChildDetail
        public const string CreateChildDetail = "child-detail";

        public const string ReadChildDetails = "child-details";

        public const string ReadChildDetailByKey = "child-detail/key/{key}";

        public const string ReadChildDetailByClient = "child-detail/by-client/{clientId}";

        public const string ReadChildDetailByEncounter = "child-detail/by-encounter/{encounterId}";

        public const string UpdateChildDetail = "child-detail/{key}";

        public const string DeleteChildDetail = "child-detail/{key}";
        #endregion

        #region PastAntenaltal
        public const string CreatePastAntenaltal = "past-antenaltal";

        public const string ReadPastAntenaltals = "past-antenaltals";

        #endregion

        #region BloodTransfusionHistory
        public const string CreateBloodTransfusionHistory = "blood-transfusion-history";

        public const string ReadBloodTransfusionHistorys = "blood-transfusion-histories";

        public const string ReadBloodTransfusionHistoryByKey = "blood-transfusion-history/key/{key}";

        public const string ReadBloodTransfusionHistoryByClient = "blood-transfusion-history/by-client/{clientId}";

        public const string ReadBloodTransfusionHistoryByEncounter = "blood-transfusion-history/by-encounter/{encounterId}";

        public const string UpdateBloodTransfusionHistory = "blood-transfusion-history/{key}";

        public const string DeleteBloodTransfusionHistory = "blood-transfusion-history/{key}";
        #endregion

        #region PregnancyBooking
        public const string CreatePregnancyBooking = "pregnancy-booking";

        public const string ReadPregnancyBookings = "pregnancy-bookings";

        public const string ReadPregnancyBookingByKey = "pregnancy-booking/key/{key}";

        public const string ReadPregnancyBookingByClient = "pregnancy-booking/by-client/{clientId}";

        public const string ReadPregnancyBookingByEncounter = "pregnancy-booking/by-encounter/{encounterId}";

        public const string UpdatePregnancyBooking = "pregnancy-booking/{key}";

        public const string DeletePregnancyBooking = "pregnancy-booking/{key}";
        #endregion

        #region VaginalPosition
        public const string CreateVaginalPosition = "vaginal-position";

        public const string ReadVaginalPositions = "vaginal-positions";

        public const string ReadVaginalPositionByKey = "vaginal-position/key/{key}";

        public const string ReadVaginalPositionByClient = "vaginal-position/by-client/{clientId}";

        public const string ReadVaginalPositionByEncounter = "vaginal-position/by-encounter/{encounterId}";

        public const string UpdateVaginalPosition = "vaginal-position/{key}";

        public const string DeleteVaginalPosition = "vaginal-position/{key}";
        #endregion

        #region ScreeningAndPrevention
        public const string CreateScreeningAndPrevention = "screening-and-prevention";

        public const string ReadScreeningAndPreventions = "screening-and-preventions";

        public const string ReadScreeningAndPreventionByKey = "screening-and-prevention/key/{key}";

        public const string ReadScreeningAndPreventionByClient = "screening-and-prevention/by-client/{clientId}";

        public const string ReadScreeningAndPreventionByEncounter = "screening-and-prevention/by-encounter/{encounterId}";

        public const string UpdateScreeningAndPrevention = "screening-and-prevention/{key}";

        public const string DeleteScreeningAndPrevention = "screening-and-prevention/{key}";

        public const string RemoveScreeningAndPrevention = "screening-and-prevention/remove/{encounterId}";
        #endregion

        #region IdentifiedPregnancyConfirmation
        public const string CreateIdentifiedPregnancyConfirmation = "identified-pregnancy-confirmation";

        public const string ReadIdentifiedPregnancyConfirmations = "identified-pregnancy-confirmations";

        public const string ReadIdentifiedPregnancyConfirmationByKey = "identified-pregnancy-confirmation/key/{key}";

        public const string ReadIdentifiedPregnancyConfirmationByEncounter = "identified-pregnancy-confirmation/by-encounter/{encounterId}";

        public const string UpdateIdentifiedPregnancyConfirmation = "identified-pregnancy-confirmation/{key}";

        public const string DeleteIdentifiedPregnancyConfirmation = "identified-pregnancy-confirmation/{key}";
        #endregion

        #region IdentifiedPriorSensitization
        public const string CreateIdentifiedPriorSensitization = "identified-prior-sensitization";

        public const string ReadIdentifiedPriorSensitizations = "identified-prior-sensitizations";

        public const string ReadIdentifiedPriorSensitizationByKey = "identified-prior-sensitization/key/{key}";

        public const string ReadIdentifiedPriorSensitizationByClient = "identified-prior-sensitization/by-client/{clientId}";

        public const string ReadIdentifiedPriorSensitizationByEncounter = "identified-prior-sensitization/by-encounter/{encounterId}";

        public const string ReadIdentifiedPriorSensitizationByBloodTransfusion = "identified-prior-sensitization/by-blood-transfusion/{bloodTransfusionId}";

        public const string UpdateIdentifiedPriorSensitization = "identified-prior-sensitization/{key}";

        public const string DeleteIdentifiedPriorSensitization = "identified-prior-sensitization/{key}";
        #endregion

        //PNC
        #region PastAntenatalVisit
        public const string CreatePastAntenatalVisit = "past-antenatal-visit";

        public const string ReadPastAntenatalVisits = "past-antenatal-visits";

        public const string ReadPastAntenatalVisitByKey = "past-antenatal-visit/key/{key}";

        public const string ReadPastAntenatalVisitByClient = "past-antenatal-visit/by-client/{clientId}";

        public const string ReadPastAntenatalVisitByEncounter = "past-antenatal-visit/by-encounter/{encounterId}";

        public const string UpdatePastAntenatalVisit = "past-antenatal-visit/{key}";

        public const string DeletePastAntenatalVisit = "past-antenatal-visit/{key}";
        #endregion

        #region PriorSensitization
        public const string ReadPriorSensitizations = "prior-sensitizations";

        public const string ReadPriorSensitizationByKey = "prior-sensitization/key/{key}";

        public const string CreatePriorSensitization = "prior-sensitization";

        public const string UpdatePriorSensitization = "prior-sensitization/{key}";

        public const string DeletePriorSensitization = "prior-sensitization/{key}";
        #endregion

        #region ObstetricExamination
        public const string CreateObstetricExamination = "obstetric-examination";

        public const string ReadObstetricExaminations = "obstetric-examinations";

        public const string ReadObstetricExaminationByKey = "obstetric-examination/key/{key}";

        public const string ReadObstetricExaminationByClient = "obstetric-examination/by-client/{clientId}";

        public const string ReadObstetricExaminationByEncounter = "obstetric-examination/by-encounter/{encounterId}";

        public const string UpdateObstetricExamination = "obstetric-examination/{key}";

        public const string DeleteObstetricExamination = "obstetric-examination/{key}";
        #endregion

        #region VisitDetail
        public const string CreateVisitDetail = "visit-detail";

        public const string ReadVisitDetails = "visit-details";

        public const string ReadVisitDetailByKey = "visit-detail/key/{key}";

        public const string ReadVisitDetailByClient = "visit-detail/by-client/{clientId}";

        public const string ReadVisitDetailByEncounter = "visit-detail/by-encounter/{encounterId}";

        public const string UpdateVisitDetail = "visit-detail/{key}";

        public const string DeleteVisitDetail = "visit-detail/{key}";
        #endregion

        #region PelvicAndVaginalExamination
        public const string CreatePelvicAndVaginalExamination = "pelvic-and-vaginal-examination";

        public const string ReadPelvicAndVaginalExaminations = "pelvic-and-vaginal-examinations";

        public const string ReadPelvicAndVaginalExaminationByKey = "pelvic-and-vaginal-examination/key/{key}";

        public const string ReadPelvicAndVaginalExaminationByClient = "pelvic-and-vaginal-examination/by-client/{clientId}";

        public const string ReadPelvicAndVaginalExaminationByEncounter = "pelvic-and-vaginal-examination/by-encounter/{encounterId}";

        public const string UpdatePelvicAndVaginalExamination = "pelvic-and-vaginal-examination/{key}";

        public const string DeletePelvicAndVaginalExamination = "pelvic-and-vaginal-examination/{key}";
        #endregion

        #region IdentifiedEyesAssessment
        public const string CreateIdentifiedEyesAssessment = "identified-eyes-assessment";

        public const string ReadIdentifiedEyesAssessments = "identified-eyes-assessments";

        public const string ReadIdentifiedEyesAssessmentByKey = "identified-eyes-assessment/key/{key}";

        public const string ReadIdentifiedEyesAssessmentByClient = "identified-eyes-assessment/by-client/{clientId}";

        public const string ReadIdentifiedEyesAssessmentByEncounter = "identified-eyes-assessment/by-encounter/{encounterId}";

        public const string ReadIdentifiedEyesAssessmentByAssessment = "identified-eyes-assessment/by-assessment/{assessmentId}";

        public const string UpdateIdentifiedEyesAssessment = "identified-eyes-assessment/{key}";

        public const string DeleteIdentifiedEyesAssessment = "identified-eyes-assessment/{key}";
        #endregion

        #region IdentifiedCordStump
        public const string CreateIdentifiedCordStump = "identified-cord-stump";

        public const string ReadIdentifiedCordStumps = "identified-cord-stumps";

        public const string ReadIdentifiedCordStumpByKey = "identified-cord-stump/key/{key}";

        public const string ReadIdentifiedCordStumpByClient = "identified-cord-stump/by-client/{clientId}";

        public const string ReadIdentifiedCordStumpByEncounter = "identified-cord-stump/by-encounter/{encounterId}";

        public const string ReadIdentifiedCordStumpByAssessment = "identified-cord-stump/by-assessment/{assessmentId}";

        public const string UpdateIdentifiedCordStump = "identified-cord-stump/{key}";

        public const string DeleteIdentifiedCordStump = "identified-cord-stump/{key}";
        #endregion

        #region PreferredFeeding
        public const string CreatePreferredFeeding = "preferred-feeding";

        public const string ReadPreferredFeedings = "preferred-feedings";

        public const string ReadPreferredFeedingByKey = "preferred-feeding/key/{key}";

        public const string UpdatePreferredFeeding = "preferred-feeding/{key}";

        public const string DeletePreferredFeeding = "preferred-feeding/{key}";
        #endregion

        #region IdentifiedPreferredFeeding
        public const string CreateIdentifiedPreferredFeeding = "identified-preferred-feeding";

        public const string ReadIdentifiedPreferredFeedings = "identified-preferred-feedings";

        public const string ReadIdentifiedPreferredFeedingByKey = "identified-preferred-feeding/key/{key}";

        public const string ReadIdentifiedPreferredFeedingByClient = "identified-preferred-feeding/by-client/{clientId}";

        public const string ReadIdentifiedPreferredFeedingByEncounter = "identified-preferred-feeding/by-encounter/{encounterId}";

        public const string ReadIdentifiedPreferredFeedingByPreferredFeeding = "identified-preferred-feeding/by-preferred-feeding/{preferredFeedingId}";

        public const string UpdateIdentifiedPreferredFeeding = "identified-preferred-feeding/{key}";

        public const string DeleteIdentifiedPreferredFeeding = "identified-preferred-feeding/{key}";
        #endregion

        #region IdentifiedDeliveryIntervention
        public const string CreateIdentifiedDeliveryIntervention = "identified-delivery-intervention";

        public const string ReadIdentifiedDeliveryInterventions = "identified-delivery-interventions";

        public const string ReadIdentifiedDeliveryInterventionByKey = "identified-delivery-intervention/key/{key}";

        public const string ReadIdentifiedDeliveryInterventionByDelivery = "identified-delivery-intervention/by-delivery/{deliveryId}";

        public const string UpdateIdentifiedDeliveryIntervention = "identified-delivery-intervention/{key}";

        public const string DeleteIdentifiedDeliveryIntervention = "identified-delivery-intervention/{key}";
        #endregion

        #region IdentifiedCurrentDeliveryComplication
        public const string CreateIdentifiedCurrentDeliveryComplication = "identified-current-delivery-complication";

        public const string ReadIdentifiedCurrentDeliveryComplications = "identified-current-delivery-complications";

        public const string ReadIdentifiedCurrentDeliveryComplicationByKey = "identified-current-delivery-complication/key/{key}";

        public const string ReadIdentifiedCurrentDeliveryComplicationByDelivery = "identified-current-delivery-complication/by-delivery/{deliveryId}";

        public const string UpdateIdentifiedCurrentDeliveryComplication = "identified-current-delivery-complication/{key}";

        public const string DeleteIdentifiedCurrentDeliveryComplication = "identified-current-delivery-complication/{key}";
        #endregion

        #region ThirdStageDelivery
        public const string CreateThirdStageDelivery = "third-stage-delivery";

        public const string ReadThirdStageDeliveries = "third-stage-deliveries";

        public const string ReadThirdStageDeliveryByKey = "third-stage-delivery/key/{key}";

        public const string ReadThirdStageDeliveryByDelivery = "third-stage-delivery/by-delivery/{deliveryId}";

        public const string UpdateThirdStageDelivery = "third-stage-delivery/{key}";

        public const string DeleteThirdStageDelivery = "third-stage-delivery/{key}";
        #endregion

        #region PPHTreatment
        public const string CreatePPHTreatment = "pph-treatment";

        public const string ReadPPHTreatments = "pph-treatments";

        public const string ReadPPHTreatmentByKey = "pph-treatment/key/{key}";

        public const string ReadPPHTreatmentByDelivery = "pph-treatment/by-delivery/{deliveryId}";

        public const string UpdatePPHTreatment = "pph-treatment/{key}";

        public const string DeletePPHTreatment = "pph-treatment/{key}";
        #endregion

        //Labour & Delivery
        #region ReferralModules
        public const string CreateReferralModule = "referral-module";

        public const string ReadReferralModules = "referral-modules";

        public const string ReadReferralModuleByKey = "referral-module/key/{key}";

        public const string ReadReferralModuleByClient = "referral-module/by-client/{clientId}";

        public const string ReadReferralModuleByEncounter = "referral-module/by-encounter/{encounterId}";

        public const string UpdateReferralModule = "referral-module/{key}";

        public const string DeleteReferralModule = "referral-module/{key}";
        #endregion

        #region IdentifiedReferral
        public const string CreateIdentifiedReferral = "identified-referral";

        public const string ReadIdentifiedReferrals = "identified-referrals";

        public const string ReadIdentifiedReferralByKey = "identified-referral/key/{key}";

        public const string ReadIdentifiedReferralByEncounter = "identified-referral/by-encounter/{encounterId}";

        public const string UpdateIdentifiedReferral = "identified-referral/{key}";

        public const string DeleteIdentifiedReferral = "identified-referral/{key}";
        #endregion

        #region ReasonsOfReferral
        public const string CreateReasonsOfReferral = "reason-of-referral";

        public const string ReadReasonsOfReferrals = "reason-of-referrals";

        public const string ReadReasonsOfReferralByKey = "reason-of-referral/key/{key}";

        public const string UpdateReasonsOfReferral = "reason-of-referral/{key}";

        public const string DeleteReasonsOfReferral = "reason-of-referral/{key}";
        #endregion

        #region MotherDeliverySummary
        public const string CreateMotherDeliverySummary = "mother-delivery-summary";

        public const string ReadMotherDeliverySummaries = "mother-delivery-summaries";

        public const string ReadMotherDeliverySummaryByClient = "mother-delivery-summary/by-client/{clientId}";

        public const string ReadMotherDeliverySummaryByKey = "mother-delivery-summary/key/{key}";

        public const string ReadMotherDeliverySummaryByEncounter = "mother-delivery-summary/by-encounter/{encounterId}";

        public const string ReadMotherDeliverySummaryHistory = "mother-delivery-summary-history/by-delivery/{deliveryId}";
        //public const string ReadMotherDeliverySummaryHistory = "mother-delivery-summary-history/{deliveryId}";

        public const string UpdateMotherDeliverySummary = "mother-delivery-summary/{key}";

        public const string DeleteMotherDeliverySummary = "mother-delivery-summary/{key}";
        #endregion

        #region MedicalTreatment
        public const string CreateMedicalTreatment = "medical-treatment";

        public const string ReadMedicalTreatments = "medical-treatments";

        public const string ReadMedicalTreatmentByKey = "medical-treatment/key/{key}";

        public const string ReadMedicalTreatmentByDelivery = "medical-treatment/by-delivery/{deliveryId}";

        public const string ReadMedicalTreatmentByEncounter = "medical-treatment/by-encounter/{encounterId}";

        public const string UpdateMedicalTreatment = "medical-treatment/update/{key}";

        public const string DeleteMedicalTreatment = "medical-treatment/{key}";
        #endregion

        #region UterusCondition
        public const string CreateUterusCondition = "uterus-condition";

        public const string ReadUterusConditions = "uterus-conditions";

        public const string ReadUterusConditionByKey = "uterus-condition/key/{key}";

        public const string ReadUterusConditionByDelivery = "uterus-condition/by-delivery/{deliveryId}";

        public const string ReadUterusConditionByEncounter = "uterus-condition/by-encounter/{encounterId}";

        public const string UpdateUterusCondition = "uterus-condition/{key}";

        public const string DeleteUterusCondition = "uterus-condition/{key}";
        #endregion

        #region PlacentaRemoval
        public const string CreatePlacentaRemoval = "placenta-removal";

        public const string ReadPlacentaRemovals = "placenta-removals";

        public const string ReadPlacentaRemovalByKey = "placenta-removal/key/{key}";

        public const string ReadPlacentaRemovalByDelivery = "placenta-removal/by-delivery/{deliveryId}";

        public const string ReadPlacentaRemovalByEncounter = "placenta-removal/by-encounter/{encounterId}";

        public const string UpdatePlacentaRemoval = "placenta-removal/update/{key}";

        public const string DeletePlacentaRemoval = "placenta-removal/delete/{key}";
        #endregion

        #region PeriuneumIntact
        public const string CreatePeriuneumIntact = "periuneum-intact";

        public const string ReadPeriuneumIntacts = "periuneum-intacts";

        public const string ReadPeriuneumIntactByKey = "periuneum-intact/key/{key}";

        public const string ReadPeriuneumIntactByDelivery = "periuneum-intact/by-delivery/{deliveryId}";

        public const string ReadPeriuneumIntactByEncounter = "periuneum-intact/by-encounter/{encounterId}";

        public const string UpdatePeriuneumIntact = "periuneum-intact/update/{key}";

        public const string DeletePeriuneumIntact = "periuneum-intact/{key}";
        #endregion

        #region IdentifiedPeriuneumIntact
        public const string CreateIdentifiedPerineumIntact = "identified-perineum-intact";

        public const string ReadIdentifiedPerineumIntacts = "identified-perineum-intacts";

        public const string ReadIdentifiedPerineumIntactByKey = "identified-perineum-intact/key/{key}";

        public const string ReadIdentifiedPerineumIntactByDeliveryId = "identified-perineum-intact/by-delivery/{deliveryId}";

        public const string ReadIdentifiedPerineumIntactByEncounter = "identified-perineum-intact/by-encounter/{encounterId}";

        public const string UpdateIdentifiedPerineumIntact = "identified-perineum-intact/update/{key}";

        public const string DeleteIdentifiedPerineumIntact = "identified-perineum-intact/{key}";
        #endregion

        #region PresentingPart
        public const string CreatePresentingPart = "presenting-part";

        public const string ReadPresentingParts = "presenting-parts";

        public const string ReadPresentingPartByKey = "presenting-parts/key/{key}";

        public const string UpdatePresentingPart = "presenting-part/{key}";

        public const string DeletePresentingPart = "presenting-part/{key}";
        #endregion

        #region Breech
        public const string CreateBreech = "breech";

        public const string ReadBreeches = "breeches";

        public const string ReadBreechByKey = "breech/key/{key}";

        public const string UpdateBreech = "breech/{key}";

        public const string DeleteBreech = "breech/{key}";
        #endregion

        #region ModeOfDelivery
        public const string CreateModeOfDelivery = "mode-of-delivery";

        public const string ReadModeOfDeliveries = "mode-of-deliveries";

        public const string ReadModeOfDeliveryByKey = "mode-of-delivery/key/{key}";

        public const string UpdateModeOfDelivery = "mode-of-delivery/{key}";

        public const string DeleteModeOfDelivery = "mode-of-delivery/{key}";
        #endregion

        #region NeonatalBirthOutcome
        public const string CreateNeonatalBirthOutcome = "neonatal-birth-outcome";

        public const string ReadNeonatalBirthOutcomes = "neonatal-birth-outcomes";

        public const string ReadNeonatalBirthOutcomeByKey = "neonatal-birth-outcome/key/{key}";

        public const string UpdateNeonatalBirthOutcome = "neonatal-birth-outcome/{key}";

        public const string DeleteNeonatalBirthOutcome = "neonatal-birth-outcome/{key}";
        #endregion

        #region CauseOfStillBirth
        public const string CreateCauseOfStillBirth = "cause-of-still-birth";

        public const string ReadCauseOfStillBirths = "cause-of-still-births";

        public const string ReadCauseOfStillBirthByKey = "cause-of-still-birth/key/{key}";

        public const string UpdateCauseOfStillBirth = "cause-of-still-birth/{key}";

        public const string DeleteCauseOfStillBirth = "cause-of-still-birth/{key}";
        #endregion

        #region FeedingMethod
        public const string CreateFeedingMethod = "feeding-method";

        public const string ReadFeedingMethods = "feeding-methods";

        public const string ReadFeedingMethodByKey = "feeding-method/key/{key}";

        public const string ReadFeedingMethodByClient = "feeding-method/by-client/{clientId}";

        public const string ReadFeedingMethodByEncounter = "feeding-method/by-encounter/{encounterId}";

        public const string UpdateFeedingMethod = "feeding-method/{key}";

        public const string DeleteFeedingMethod = "feeding-method/{key}";

        public const string RemoveFeedingMethod = "feeding-method/remove/{key}";
        #endregion

        #region DischargeMetric
        public const string CreateDischargeMetric = "discharge-metric";

        public const string ReadDischargeMetrics = "discharge-metrics";

        public const string ReadDischargeMetricByKey = "discharge-metric/key/{key}";

        public const string ReadDischargeMetricByClient = "discharge-metric/by-client/{clientId}";

        public const string ReadDischargeMetricByEncounter = "discharge-metric/by-encounter/{encounterId}";

        public const string UpdateDischargeMetric = "discharge-metric/{key}";

        public const string DeleteDischargeMetric = "discharge-metric/{key}";
        #endregion

        #region IdentifiedPPHTreatment
        public const string CreateIdentifiedPPHTreatment = "identified-pph-treatment";

        public const string ReadIdentifiedPPHTreatments = "identified-pph-treatments";

        public const string ReadIdentifiedPPHTreatmentByKey = "identified-pph-treatment/key/{key}";

        public const string ReadIdentifiedPPHTreatmentByPPHTreatments = "identified-pph-treatment/by-pphtreatment/{pphtreatmentsId}";

        public const string ReadIdentifiedPPHTreatmentByEncounter = "identified-pph-treatment/by-encounter/{encounterId}";

        public const string UpdateIdentifiedPPHTreatment = "identified-pph-treatment/{key}";

        public const string DeleteIdentifiedPPHTreatment = "identified-pph-treatment/{key}";
        #endregion

        #region IdentifiedPlacentaRemoval
        public const string CreateIdentifiedPlacentaRemoval = "identified-placenta-removal";

        public const string ReadIdentifiedPlacentaRemovals = "identified-placenta-removals";

        public const string ReadIdentifiedPlacentaRemovalByKey = "identified-placenta-removal/key/{key}";

        public const string ReadIdentifiedPlacentaRemovalByPlacentaRemoval = "identified-placenta-removal/by-placentaremoval/{placentaRemovalId}";

        public const string ReadIdentifiedPlacentaRemovalByEncounter = "identified-placenta-removal/by-encounter/{encounterId}";

        public const string UpdateIdentifiedPlacentaRemoval = "identified-placenta-removal/{key}";

        public const string DeleteIdentifiedPlacentaRemoval = "identified-placenta-removal/{key}";
        #endregion

        #region CauseOfNeonatalDeath
        public const string ReadCauseOfNeonatalDeaths = "cause-of-neonatal-deaths";

        public const string ReadCauseOfNeonatalDeathByKey = "cause-of-neonatal-death/key/{key}";

        public const string CreateCauseOfNeonatalDeath = "cause-of-neonatal-death";

        public const string UpdateCauseOfNeonatalDeath = "cause-of-neonatal-death/{key}";

        public const string DeleteCauseOfNeonatalDeath = "cause-of-neonatal-death/{key}";
        #endregion

        #region NeonatalDeath
        public const string CreateNeonatalDeath = "neonatal-death";

        public const string ReadNeonatalDeaths = "neonatal-deaths";

        public const string ReadNeonatalDeathByKey = "neonatal-death/key/{key}";

        public const string ReadNeonatalDeathByCauseOfNeonatalDeath = "neonatal-death/causeofneonataldeath/{causeofneonatalDeathId}";

        public const string ReadNeonatalDeathByNeonatal = "neonatal-death/neonatal/{neonatalId}";

        public const string ReadNeonatalDeathByEncounter = "neonatal-death/byencounter/{encounterId}";

        public const string UpdateNeonatalDeath = "neonatal-death/{key}";

        public const string DeleteNeonatalDeath = "neonatal-death/{key}";
        #endregion

        #region NeonatalAbnormalite
        public const string CreateNeonatalAbnormality = "neonatal-abnormality";

        public const string ReadNeonatalAbnormalities = "neonatal-abnormalities";

        public const string ReadNeonatalAbnormalityByKey = "neonatal-abnormality/key/{key}";

        public const string ReadNeonatalAbnormalityByNeonatal = "neonatal-abnormality/by-neonatal/{neonatalId}";

        public const string ReadNeonatalAbnormalityByEncounter = "neonatal-abnormality/by-encounter/{encounterId}";

        public const string UpdateNeonatalAbnormality = "neonatal-abnormality/{key}";

        public const string DeleteNeonatalAbnormality = "neonatal-abnormality/{key}";
        #endregion

        #region NeonatalInjury
        public const string CreateNeonatalInjury = "neonatal-injury";

        public const string ReadNeonatalInjuries = "neonatal-injuries";

        public const string ReadNeonatalInjuryByKey = "neonatal-injury/key/{key}";

        public const string ReadNeonatalInjuryByNeonatal = "neonatal-injury/by-neonatal/{neonatalId}";

        public const string ReadNeonatalInjuryByEncounter = "neonatal-injury/by-encounter/{encounterId}";

        public const string UpdateNeonatalInjury = "neonatal-injury/update/{key}";

        public const string DeleteNeonatalInjury = "neonatal-injury/delete/{key}";
        #endregion

        #region ApgarScore
        public const string CreateApgarScore = "apgar-score";

        public const string ReadApgarScores = "apgar-scores";

        public const string ReadApgarScoreByKey = "apgar-score/key/{key}";

        public const string ReadApgarScoreByNeonatal = "apgar-score/by-neonatal/{neonatalId}";

        public const string ReadApgarScoreByEncounter = "apgar-score/byencounter/{encounterId}";

        public const string UpdateApgarScore = "apgar-score/{key}";

        public const string DeleteApgarScore = "apgar-score/{key}";
        #endregion

        #region NewBornDetail
        public const string CreateNewBornDetail = "new-born-detail";

        public const string ReadNewBornDetails = "new-born-details";

        public const string ReadNewBornDetailByKey = "new-born-detail/key/{key}";

        public const string ReadNewBornDetailByDelivery = "new-born-detail/by-delivery/{deliveryId}";

        public const string ReadNewBornDetailByDeliveryHistory = "new-born-detail-histories/by-encounter/{encounterId}";

        public const string ReadNewBornDetailByEncounter = "new-born-detail/by-encounter/{encounterId}";

        public const string UpdateNewBornDetail = "new-born-detail/{key}";

        public const string DeleteNewBornDetail = "new-born-detail/{key}";
        #endregion

        #region FamilyPlanRegister
        public const string CreateFamilyPlanRegister = "family-plan-register";

        public const string ReadFamilyPlanRegisters = "family-plan-registers";

        public const string ReadFamilyPlanRegisterByClient = "family-plan-register/by-client/{clientId}";

        public const string ReadFamilyPlanRegisterByKey = "family-plan-register/key/{key}";

        public const string UpdateFamilyPlanRegister = "family-plan-register/{key}";

        public const string DeleteFamilyPlanRegister = "family-plan-register/{key}";
        #endregion

        // Family Plan
        #region QuickExamination
        public const string CreateQuickExamination = "quick-examination";

        public const string ReadQuickExaminations = "quick-examinations";

        public const string ReadQuickExaminationByKey = "quick-examination/key/{key}";

        public const string ReadQuickExaminationByClient = "quick-examination/by-client/{clientId}";

        public const string ReadQuickExaminationByEncounter = "quick-examination/by-encounter/{encounterId}";

        public const string UpdateQuickExamination = "quick-examination/{key}";

        public const string DeleteQuickExamination = "quick-examination/{key}";
        #endregion

        #region GuidedExamination
        public const string CreateGuidedExamination = "quided-examination";

        public const string ReadGuidedExaminations = "quided-examinations";

        public const string ReadGuidedExaminationByKey = "quided-examination/key/{key}";

        public const string ReadGuidedExaminationByClient = "quided-examination/by-client/{clientId}";

        public const string ReadGuidedExaminationByEncounter = "quided-examination/by-encounter/{encounterId}";

        public const string UpdateGuidedExamination = "quided-examination/{key}";

        public const string DeleteGuidedExamination = "quided-examination/{key}";
        #endregion

        #region STIRisk
        public const string ReadSTIRisks = "sti-risks";

        public const string ReadSTIRiskByKey = "sti-risk/key/{key}";

        public const string CreateSTIRisk = "sti-risk";

        public const string UpdateSTIRisk = "sti-risk/{key}";

        public const string DeleteSTIRisk = "sti-risk/{key}";
        #endregion

        #region PastMedicalConditon
        public const string ReadPastMedicalConditons = "past-medical-conditons";

        public const string ReadPastMedicalConditonByKey = "past-medical-conditon/key/{key}";

        public const string CreatePastMedicalConditon = "past-medical-conditon";

        public const string UpdatePastMedicalConditon = "past-medical-conditon/{key}";

        public const string DeletePastMedicalConditon = "past-medical-conditon/{key}";
        #endregion

        #region MedicalCondition
        public const string CreateMedicalCondition = "medical-condition";

        public const string ReadMedicalConditions = "medical-conditions";

        public const string ReadMedicalConditionByClient = "medical-condition/byclient/{clientId}";

        public const string ReadMedicalConditionByEncounter = "medical-condition/by-encounter/{encounterId}";

        public const string ReadMedicalConditionByPastMedicalConditon = "medical-condition/by-past-medical-conditon/{pastMedicalConditonId}";

        public const string ReadMedicalConditionBySTIRisk = "medical-condition/by-stirisk/{stiriskId}";

        public const string ReadMedicalConditionByKey = "medical-condition/key/{key}";

        public const string UpdateMedicalCondition = "medical-condition/{key}";

        public const string DeleteMedicalCondition = "medical-condition/{key}";
        #endregion

        #region FamilyPlan
        public const string CreateFamilyPlan = "family-plan";

        public const string ReadFamilyPlans = "family-plans";

        public const string ReadFamilyPlanByClient = "family-plan/by-client/{clientId}";

        public const string ReadFamilyPlanByEncounter = "family-plan/by-encounter/{encounterId}";

        public const string ReadFamilyPlanByKey = "family-plan/key/{key}";

        public const string UpdateFamilyPlan = "family-plan/{key}";

        public const string DeleteFamilyPlan = "family-plan/{key}";
        #endregion

        #region FamilyPlaningClass
        public const string ReadFamilyPlanningClasses = "family-planing-classes";

        public const string ReadFamilyPlanningClassByKey = "family-planing-class/key/{key}";

        public const string CreateFamilyPlanningClass = "family-planing-class";

        public const string UpdateFamilyPlanningClass = "family-planing-class/{key}";

        public const string DeleteFamilyPlanningClass = "family-planing-class/{key}";
        #endregion

        #region FamilyPlaningSubClass
        public const string ReadFamilyPlanningSubClasses = "family-planing-sub-classes";

        public const string ReadFamilyPlanningSubClassByKey = "family-planing-sub-class/key/{key}";

        public const string FamilyPlanningSubclassByClass = "family-planning/subclass-by-class/{classId}";

        public const string CreateFamilyPlanningSubClass = "family-planing-sub-classes";

        public const string UpdateFamilyPlanningSubClass = "family-planing-sub-classes/{key}";

        public const string DeleteFamilyPlanningSubClass = "family-planing-sub-classes/{key}";
        #endregion

        #region InsertionAndRemovalProcedure
        public const string CreateInsertionAndRemovalProcedure = "insertion-and-removal-procedure";

        public const string ReadInsertionAndRemovalProcedures = "insertion-and-removal-procedures";

        public const string ReadInsertionAndRemovalProcedureByClient = "insertion-and-removal-procedure/by-client/{clientId}";

        public const string ReadInsertionAndRemovalProcedureByEncounter = "insertion-and-removal-procedure/by-encounter/{encounterId}";

        public const string ReadInsertionAndRemovalProcedureByKey = "insertion-and-removal-procedure/key/{key}";

        public const string UpdateInsertionAndRemovalProcedure = "insertion-and-removal-procedure/{key}";

        public const string DeleteInsertionAndRemovalProcedure = "insertion-and-removal-procedure/{key}";
        #endregion

        #region FacilityQueue
        public const string CreateFacilityQueue = "facility-queue";

        public const string ReadFacilityQueues = "facility-queues";

        public const string ReadFacilityQueueByKey = "facility-queue/key/{key}";

        public const string ReadFacilityQueueByKeyAndText = "facility-queue/search/{key}";

        public const string ReadFacilityQueueByFacility = "facility-queue/{facilityId}";

        public const string ReadFacilityQueueByFacilityText = "facility-queue/search/{facilityId}";

        public const string ReadFacilityQueueByFacilityWithActivePatientCount = "service-point/facilityId/with-active-patientcount/{facilityId}";

        public const string UpdateFacilityQueue = "facility-queue/{key}";

        public const string DeleteFacilityQueue = "facility-queue/{key}";
        #endregion

        // Reporting
        #region Report
        public const string ReadARTReports = "art-reports/{Key}";

        public const string ReadOPDAttendance = "opd-Attendance/{Key}";

        public const string ReadMCH = "mch/{Key}";

        public const string ReadLateForPharmacy = "late-for-pharmacy";

        public const string ReadOpdRegister = "opd-register";

        public const string ReadNdRegister = "nd-register";

        public const string ReadARTRegisterReport = "art-register-reports";

        public const string ReadAPRegister = "ap-register";

        public const string ReadHIAOneA = "hia-one-a";

        public const string ReadHIAOneB = "hia-one-b";

        public const string ReadHIATwo = "hia-two";

        public const string ReadDiarrhea = "diarrhea/{Key}";

        public const string ReadCholera = "cholera/{Key}";
        #endregion

        // Sms
        #region SMS
        public const string SendSMS = "sendSMS";
        #endregion

        //DFZ
        #region ArmedForceService
        public const string CreateArmedForceService = "armed-force-service";

        public const string ReadArmedForceServiceses = "armed-force-serviceses";

        public const string ReadArmedForceServiceByKey = "armed-force-serviceses/key/{key}";

        public const string UpdateArmedForceService = "armed-force-service/{key}";

        public const string DeleteArmedForceService = "armed-force-service/{key}";
        #endregion

        #region PatientType
        public const string CreatePatientType = "patient-type";

        public const string ReadPatientTypes = "patient-types";

        public const string ReadPatientTypeByKey = "patient-type/key/{key}";

        public const string UpdatePatientType = "patient-type/{key}";

        public const string DeletePatientType = "patient-type/{key}";

        public const string ReadPatientTypeByArmedForceService = "patient-type-by-armedforce/{armedForceId}";
        #endregion

        #region DefenceRank
        public const string CreateDefenceRank = "defence-rank";

        public const string ReadDefenceRanks = "defence-ranks";

        public const string ReadDefenceRankByKey = "defence-rank/key/{key}";

        public const string UpdateDefenceRank = "defence-rank/{key}";

        public const string DeleteDefenceRank = "defence-rank/{key}";

        public const string ReadDefenceRankByPatientType = "defence-rank-by-patienttype/{key}";
        #endregion

        #region SCRS Authentication
        public const string SCRSToken = "scrs-authentication/generate-token";
        public const string SCRSTokenVerification = "scrs-authentication/token-verification";
        public const string SCRSLogin = "scrs-authentication/scrs-login";
        public const string SCRSCheckFacilityPermission = "scrs-authentication/check-facility-permission";
        #endregion

        //PreScreening MODULE
        #region PreScreening
        public const string CreatePreScreeningVisit = "pre-screening-visit";

        public const string ReadPreScreeningVisits = "pre-screening-visits";

        public const string ReadPreScreeningVisitByKey = "pre-screening-visit/key/{key}";

        public const string UpdatePreScreeningVisit = "pre-screening-visit/{key}";

        public const string ReadPreScreeningVisitByClient = "pre-screening-visit/by-client/{clientId}";

        public const string ReadPreScreeningVisitByEncounter = "pre-screening-visit/by-encounter/{encounterId}";

        public const string DeletePreScreeningVisit = "pre-screening-visit/{key}";
        #endregion

        //Screenings MODULE
        #region Screenings
        public const string CreateScreening = "screening";

        public const string ReadScreenings = "screenings";

        public const string ReadScreeningByKey = "screening/key/{key}";

        public const string UpdateScreening = "screening/{key}";

        public const string ReadScreeningsByClient = "screening/by-Client/{clientId}";
        public const string ReadScreeningByEncounter = "screening/by-Encounter/{encounterId}";

        public const string DeleteScreening = "screening/{key}";
        #endregion

        //ThermoAblation MODULE
        #region ThermoAblations
        public const string CreateThermoAblation = "thermoablation";

        public const string ReadThermoAblations = "thermoablations";

        public const string ReadThermoAblationByKey = "thermoablation/key/{key}";

        public const string UpdateThermoAblation = "thermoablation/{key}";

        public const string ReadThermoAblationsByClient = "thermoablations/by-client/{clientId}";

        public const string ReadThermoAblationsByEncounter = "thermoablations/by-encounter/{encounterId}";

        public const string DeleteThermoAblation = "thermoablation/{key}";
        #endregion 
        
        #region Leeps
        public const string CreateLeep = "leep";

        public const string ReadLeeps = "leeps";

        public const string ReadLeepByKey = "leep/key/{key}";

        public const string UpdateLeep = "leep/{key}";

        public const string ReadLeepsByClient = "leeps/by-client/{clientId}";

        public const string ReadLeepsByEncounter = "leeps/by-encounter/{encounterId}";

        public const string DeleteLeep = "leep/{key}";
        #endregion

        #region Intercoursestatus
        public const string CreateInterCourseStatus = "intercoursestatus";

        public const string ReadInterCourseStatus = "intercoursestatuses";

        public const string ReadInterCourseStatusByKey = "intercoursestatus/key/{key}";

        public const string UpdateInterCourseStatus = "intercoursestatus/{key}"; 

        public const string DeleteInterCourseStatus = "intercoursestatus/{key}";
        #endregion

        #region ThermoAblationTreatmentMethod
        public const string CreateThermoAblationTreatmentMethod = "thermoablationtreatmentmethod";

        public const string ReadThermoAblationTreatmentMethod = "thermoablationtreatmentmethods";

        public const string ReadThermoAblationTreatmentMethodByKey = "thermoablationtreatmentmethod/key/{key}";

        public const string UpdateThermoAblationTreatmentMethod = "thermoablationtreatmentmethod/{key}";

        public const string DeleteThermoAblationTreatmentMethod = "thermoablationtreatmentmethod/{key}";
        #endregion

        #region LeepsTreatmentMethod
        public const string CreateLeepsTreatmentMethod = "leepstreatmentmethod";

        public const string ReadLeepsTreatmentMethod = "leepstreatmentmethods";

        public const string ReadLeepsTreatmentMethodByKey = "leepstreatmentmethod/key/{key}";

        public const string UpdateLeepsTreatmentMethod = "leepstreatmentmethod/{key}";

        public const string DeleteLeepsTreatmentMethod = "leepstreatmentmethod/{key}";
        #endregion

        #region CA-CXPlans
        public const string CreateCACXPlan = "ca-cxplan";

        public const string ReadCACXPlan = "ca-cxplans";

        public const string ReadCACXPlanByKey = "ca-cxplan/key/{key}";

        public const string UpdateCACXPlan = "ca-cxplan/{key}";

        public const string ReadCACXPlanByClient = "ca-cxplan/by-client/{clientId}";

        public const string ReadCACXPlanByEncounter = "ca-cxplan/by-encounter/{encounterId}";

        public const string DeleteCACXPlan = "ca-cxplan/{key}";
        #endregion
    }
}