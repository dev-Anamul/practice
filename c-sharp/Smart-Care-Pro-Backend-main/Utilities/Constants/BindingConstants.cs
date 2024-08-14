/*
 * Created by   : Lion
 * Date created : 20.12.2022
 * Modified by  : Bella
 * Last modified: 01.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Utilities.Constants
{
   public static class BindingConstants
   {
      //USER MODULE
      public const string UserAccountCreate = "FirstName, Surname, DOB, Sex, Designation, NRC,  NoNRC, ContactAddress, CountryCode, Cellphone, Username, Password, ConfirmPassword";

      public const string UserAccountEdit = "Oid, FirstName, Surname, DOB, Sex, Designation, CountryCode, Cellphone, NRC, NoNRC, ContactAddress";

      //CLIENT MODULE
      public const string ClientCreate = "Firstname, Surname, Sex, DOB, IsDOBEstimated, NRC, NoNRC, RegistrationDate, FathersFirstName, FatehersSurname, FathersNRC, IsFatherDeceased, MothersFirstName, MothersSurname, MothersNRC, IsMotherDeceased, GuardiansFirstName, GuardiansSurname, GuardiansNRC, MaritalStatus, SpousesLegalName, SpousesSurname, CellphoneCountryCode, Cellphone, OtherCellphoneCountryCode, OtherCellphone, NoCellphone, LandPhoneCountryCode, LandPhone, Email, HouseholdNumber, Road, Area, Landmarks, IsZambianBorn, BirthPlace, Religion, DistrictID, HomeLanguageID, EducationLevelID, OccupationID, CountryID, TownID";

      public const string ClientEdit = "Oid, Firstname, Surname, Sex, DOB, IsDOBEstimated, NRC, NoNRC, RegistrationDate, FathersFirstName, FatehersSurname, FathersNRC, IsFatherDeceased, MothersFirstName, MothersSurname, MothersNRC, IsMotherDeceased, GuardiansFirstName, GuardiansSurname, GuardiansNRC, MaritalStatus, SpousesLegalName, SpousesSurname, CellphoneCountryCode, Cellphone, OtherCellphoneCountryCode, OtherCellphone, NoCellphone, LandPhoneCountryCode, LandPhone, Email, HouseholdNumber, Road, Area, Landmarks, IsZambianBorn, BirthPlace, Religion, DistrictID, HomeLanguageID, EducationLevelID, OccupationID, CountryID, TownID";

      //VITAL MODULE
      public const string VitalCreate = "Weight, Height, BMI, Systolic, SystolicIfUnrecordable, Diastolic, DiastolicIfUnrecordable, Temperature, PulseRate, RespiratoryRate, OxygenSaturation, MUAC, MUACScore, AbdominalCircumference, HeadCircumference, HCScore, ClientID";

      public const string VitalEdit = "Oid, Weight, Height, BMI, Systolic, SystolicIfUnrecordable, Diastolic, DiastolicIfUnrecordable, Temperature, PulseRate, RespiratoryRate, OxygenSaturation, MUAC, MUACScore, AbdominalCircumference, HeadCircumference, HCScore, ClientID";

      //HTS MODULE
      public const string HTSCreate = "ClientTypeID, VisitTypeID, ServicePointID, ClientSource, HIVTestingReasonID, LastTested, LastTestResult, ARTStatus, PartnerLastTested, PartnerHIVStatus,PartnerLastTestDate, HasCounselled, HasConsented, HIVNotTestingReasonID, OtherReason, TestModality, TestedAs, TestSerialNo, TestType, TestResult, HIVType, IsDNAPCRSampleCollected, SampleCollectionDate, IsResultReceived, RetestDate, ConsentForSMS, EncounterID, VisitID, RiskAssessmentList, HTSReferredToList";

      public const string HTSEdit = "InteractionId,ClientTypeID, VisitTypeID, ServicePointID, ClientSource, HIVTestingReasonID, LastTested, LastTestResult, ARTStatus, PartnerLastTested, PartnerHIVStatus,PartnerLastTestDate, HasCounselled, HasConsented, HIVNotTestingReasonID, OtherReason, TestModality, TestedAs, TestSerialNo, TestType, TestResult, HIVType, IsDNAPCRSampleCollected, SampleCollectionDate, IsResultReceived, RetestDate, ConsentForSMS, EncounterID, VisitID, RiskAssessmentList, HTSReferredToList";

      //MEDICAL ENCOUNTER MODULE
      public const string ChiefComplaintCreate = "ChiefComplaints, HistoryOfChiefComplaint, HIVStatus, HistorySummary, ExaminationSummary, LastHIVTestDate, " +
            "TestingLocation, PotentialHIVExposureDate, RecencyType, RecencyTestDate, ChildExposureStatus, IsChildGivenARV, IsMotherGivenARV";

      public const string ChiefComplaintEdit = "InteractionId, ChiefComplaints, HistoryOfChiefComplaint, HIVStatus, HistorySummary, " +
            "ExaminationSummary, LastHIVTestDate, TestingLocation, PotentialHIVExposureDate, RecencyType, RecencyTestDate, ChildExposureStatus, IsChildGivenARV, IsMotherGivenARV";

      public const string AllergicDrugCreate = "DrugType";

      public const string AllergicDrugEdit = "Oid, DrugType";

      public const string AllergyCreate = "DrugType";

      public const string AllergyEdit = "Oid, DrugType";

      public const string BirthHistoryCreate = "BirthWeight, BirthHeight, BirthOutcome, HeadCircumference, ChestCircumference, GeneralCondition, IsBreastFeedingWell," +
            "OtherFeedingOption, DeliveryTime, VaccinationOutside, Note, TetanusAtBirth";

      public const string BirthHistoryEdit = "InteractionId, BirthWeight, BirthHeight, BirthOutcome, HeadCircumference, ChestCircumference, GeneralCondition, IsBreastFeedingWell," +
            "OtherFeedingOption, DeliveryTime, VaccinationOutside, Note, TetanusAtBirth";

      public const string CaCxScreeningMethodCreate = "ScreeningMethod";

      public const string CaCxScreeningMethodEdit = "Oid, ScreeningMethod";

      public const string ChildsDevelopmentHistoryCreate = "FeedingHistory, SocialSmile, HeadHolding, TurnTowardSoundOrigin, GraspToy, FollowObjectsWithEyes, " +
            "RollsOver, Babbles, TakesObjectsToMouth, RepeatsSyllables, MovesObjects, PlaysPeekaBoo, RespondsToOwnName, TakesStepsWithSupport, PicksUpSmallObjects, " +
            "ImitatesSimpleGestures, SaysTwoToThreeWords, Sitting, Standing, Walking, Talking, DrinksFromCup, SaysSevenToTenWords, PointsToBodyParts, StartsToRun, " +
            "PointsPictureOnRequest, Sings, BuildTowerWithBox, JumpsAndRun, BeginsToDressAndUndress, GroupsSimilarObjects, PlaysWithOtherChildren, SaysFirstNameAndShortStory";

      public const string ChildsDevelopmentHistorytEdit = "InteractionId, FeedingHistory, SocialSmile, HeadHolding, TurnTowardSoundOrigin, GraspToy, FollowObjectsWithEyes, " +
            "RollsOver, Babbles, TakesObjectsToMouth, RepeatsSyllables, MovesObjects, PlaysPeekaBoo, RespondsToOwnName, TakesStepsWithSupport, PicksUpSmallObjects, " +
            "ImitatesSimpleGestures, SaysTwoToThreeWords, Sitting, Standing, Walking, Talking, DrinksFromCup, SaysSevenToTenWords, PointsToBodyParts, StartsToRun, " +
            "PointsPictureOnRequest, Sings, BuildTowerWithBox, JumpsAndRun, BeginsToDressAndUndress, GroupsSimilarObjects, PlaysWithOtherChildren, SaysFirstNameAndShortStory";

        public const string ConstitutionalSymptomCreate = "Symptom";

      public const string ConstitutionalSymptomEdit = "Oid, Symptom";

      public const string ConstitutionalSymptomTypeCreate = "SymptomType";

      public const string ConstitutionalSymptomTypeEdit = "Oid, SymptomType";

      public const string ContraceptiveCreate = "ContraceptiveName";

      public const string ContraceptiveEdit = "Oid, ContraceptiveName";

      public const string ContraceptiveHistoryCreate = "ContraceptiveID, InteractionId";

      public const string ContraceptiveHistoryEdit = "Oid, ContraceptiveID, InteractionId";

      public const string GynObsHistoryCreate = "MenstrualHistory, LNMP, IsPregnant, ObstetricsHistoryNote, GestationalAge, EDD, IsCaCxScreened, CaCxLastScreened, CaCxResult, CaCxScreeningMethodID, ClientID";

      public const string GynObsHistoryEdit = "InteractionId, MenstrualHistory, LNMP, IsPregnant, ObstetricsHistoryNote, GestationalAge, EDD, IsCaCxScreened, CaCxLastScreened, CaCxResult, CaCxScreeningMethodID, ClientID";

      public const string MedicalHistoryCreate = "History, InformationType, ClientID";

      public const string MedicalHistoryEdit = "InteractionId, DrugType, History, InformationType, ClientID";

      public const string IdentifiedAllergyCreate = "Severity, AllergyID, AllergicDrugID, ClientID";

      public const string IdentifiedAllergyEdit = "InteractionId, Severity, AllergyID, AllergicDrugID, ClientID";

      public const string IdentifiedConstitutionalSymptomCreate = "ConstitutionalSymptomTypeID, ClientID";

      public const string IdentifiedConstitutionalSymptomEdit = "InteractionId, ConstitutionalSymptomTypeID, ClientID";

      public const string IdentifiedTBSymptomCreate = "TBSymptomID, ClientID";

      public const string IdentifiedTBSymptomEdit = "InteractionId, TBSymptomID, ClientID";

      //public const string ImmunizationRecordCreate = "OtherVaccineName, DateGiven, VaccineID, ClientID";
      public const string ImmunizationRecordCreate = "ImmunizationRecordList";

      public const string ImmunizationRecordEdit = "InteractionId, OtherVaccineName, DateGiven, VaccineID, ClientID";

      public const string OPDVisitCreate = "OPDVisitDate, ClientID";

      public const string OPDVisitEdit = "Oid, OPDVisitDate, ClientID";

      public const string PhysicalSystemCreate = "SystemName";

      public const string PhysicalSystemEdit = "Oid, SystemName";

      public const string SystemReviewCreate = "Note, PhysicalSystemID, ClientID";

      public const string SystemReviewEdit = "InteractionId, Note, PhysicalSystemID, ClientID";

      public const string TBSymptomCreate = "Symptom";

      public const string TBSymptomEdit = "Oid, Symptom";

      public const string VaccineCreate = "VaccineName, VaccineTypeID";

      public const string VaccineEdit = "Oid, VaccineName, VaccineTypeID";

      public const string VaccineTypeCreate = "VaccineTypes";

      public const string VaccineTypeEdit = "Oid, VaccineTypes";

      public const string TreatmentPlanCreate = "TreatmentPlans";

      public const string TreatmentPlanEdit = "InteractionId,TreatmentPlans";

      public const string DiagnosisCreate = "DiagnosisType, NTGId, ICDId, ClientId";

      public const string DiagnosisEdit = "InteractionId, DiagnosisType, NTGId, ICDId, ClientId";

      public const string ConditionCreate = "ConditionType, DateDiagnosed, DateResolved, IsOngoing, Certainty, Comments, NTGID, ICDID, ClientID";

      public const string ConditionEdit = "InteractionId, ConditionType, DateDiagnosed, DateResolved, IsOngoing, Certainty, Comments, NTGID, ICDID, ClientID";

      //PEP
      public const string ChiefComplaintPEPCreate = "ChiefComplaints, HistoryOfChiefComplaint, HIVStatus, LastHIVTestDate, TestingLocation, PotentialHIVExposureDate";

      public const string ChiefComplaintPEPEdit = "InteractionId, ChiefComplaints, HistoryOfChiefComplaint, HIVStatus, LastHIVTestDate, TestingLocation, PotentialHIVExposureDate";

      public const string GynObsHistoryPEPCreate = "MenstrualHistory, LNMP, IsPregnant, ObstetricsHistoryNote, GestationalAge, EDD, IsCaCxScreened, CaCxLastScreened, CaCxResult, CaCxScreeningMethodID, ClientID";

      public const string GynObsHistoryPEPEdit = "InteractionId, MenstrualHistory, LNMP, IsPregnant, ObstetricsHistoryNote, GestationalAge, EDD, IsCaCxScreened, CaCxLastScreened, CaCxResult, CaCxScreeningMethodID, ClientID";

      public const string MedicalHistoryPEPCreate = "History, InformationType, ClientID";

      public const string MedicalHistoryPEPEdit = "InteractionId, History, InformationType, ClientID";

      public const string IdentifiedConstitutionalSymptomPEPCreate = "ConstitutionalSymptomTypeID, ClientID";

      public const string IdentifiedConstitutionalSymptomPEPEdit = "InteractionId, ConstitutionalSymptomTypeID, ClientID";

      public const string IdentifiedTBSymptomPEPCreate = "TBSymptomID, ClientID";

      public const string IdentifiedTBSymptomPEPEdit = "InteractionId, TBSymptomID, ClientID";

      public const string ExposureCreate = "ExposureTypeID, ChiefComplaintsID";

      public const string ExposureEdit = "Oid, ExposureTypeID, ChiefComplaintsID";

      public const string ExposureTypeCreate = "ExposureType";

      public const string ExposureTypeEdit = "Oid, ExposureTypeID";

      public const string SystemReviewPEPCreate = "Note, PhysicalSystemID, ClientID";

      public const string SystemReviewPEPEdit = "InteractionId, Note, PhysicalSystemID, ClientID";

      public const string SystemExaminationPEPCreate = "Note, PhysicalSystemID, ClientID";

      public const string SystemExaminationPEPEdit = "InteractionId, Note, PhysicalSystemID, ClientID";

      public const string GlasGowComaScalePEPCreate = "EyeScore, VerbalScore, MotorScore, GlasgowComaScore, Result, ClientID";

      public const string GlasGowComaScalePEPEdit = "InteractionId, EyeScore, VerbalScore, MotorScore, GlasgowComaScore, Result, ClientID";

      public const string PEPRiskCreate = "Risk";

      public const string PEPRiskEdit = "Oid, Risk";

      public const string DiagnosisPEPCreate = "DiagnosisType, NTGID, ICDID, ClientID";

      public const string DiagnosisPEPEdit = "InteractionId, DiagnosisType, NTGID, ICDID, ClientID";

      public const string ConditionPEPCreate = "ConditionType, DateDiagnosed, DateResolved, IsOngoing, Certainty, Comments, NTGID, ICDID, ClientID";

      public const string ConditionPEPEdit = "InteractionId, ConditionType, DateDiagnosed, DateResolved, IsOngoing, Certainty, Comments, NTGID, ICDID, ClientID";

      public const string PEPPreventionHistoryPEPCreate = "IsPEPUsed, IsPrEPUsed, IsCondomLubUsed, VMMC, ClientID";

      public const string PEPPreventionHistoryPEPEdit = "InteractionId, IsPEPUsed, IsPrEPUsed, IsCondomLubUsed, VMMC, ClientID";

      //MEDICAL INVESTIGATION
      public const string InvestigationCreate = "Quantity, SampleQuantity, Priority, ImagingTestDetails, AdditionalComment, InvestigationBatchID, TestID, IsResultReceived";

      public const string InvestigationEdit = "Oid, Quantity, SampleQuantity, Priority, ImagingTestDetails, AdditionalComment, InvestigationBatchID, TestID, IsResultReceived";

      public const string ResultCreate = "ResultDate, Result, CommentOnResult, AdditionalNote, InvestigationID";

      public const string ResultEdit = "Oid, ResultDate, Result, CommentOnResult, AdditionalNote, InvestigationID";

      //IPD
      public const string IPDChiefComplaintCreate = "HistorySummary, ExaminationSummary";

      public const string IPDChiefComplaintEdit = "InteractionId, HistorySummary, ExaminationSummary";

        //ART
        public const string AttachedFacilityCreate = "InteractionId, TypeOfEntry,AttachedFacilityId,SourceFacilityId"; 
        public const string AttachedFacilityUpdate = "InteractionId, CreatedBy,CreatedIn,DateCreated,TypeOfEntry, SourceFacilityId";

        public const string NextOfKinsCreate = "Firstname, Surname, NextOfKinType, OtherNextOfKinType, HouseNumber, StreetName, Township, CheifName, CellphoneCountryCode, CellPhone, OtherCellphoneCountryCode, OtherCellPhone, EmailAddress";

        public const string NextOfKinsUpdate = "InteractionId,CreatedBy,CreatedIn,DateCreated, Firstname, Surname, NextOfKinType, OtherNextOfKinType, HouseNumber, StreetName, Township, CheifName, CellphoneCountryCode, CellPhone, OtherCellphoneCountryCode, OtherCellPhone, EmailAddress";

    }
}