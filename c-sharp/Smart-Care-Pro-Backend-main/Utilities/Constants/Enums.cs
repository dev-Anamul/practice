using System.ComponentModel.DataAnnotations;

/*
 * Created by   : Lion
 * Date created : 03.10.2022
 * Modified by  : Bella
 * Last modified: 27.03.2023
 * Reviewed by  : 
 * Reviewed Date:
 */
namespace Utilities.Constants
{
    /// <summary>
    /// Enums.
    /// </summary>
    /// </summary>
    public static class Enums
    {
        /// <summary>
        /// This enum is used to determine the type of the user.
        /// </summary>
        public enum UserType : byte
        {
            [Display(Name = "System Administrator")]
            SystemAdministrator = 1,

            [Display(Name = "Facility Administrator")]
            FacilityAdministrator = 2,

            Clinician = 3
        }

        /// <summary>
        /// This enum is used to determine the sex of the client/user.
        /// </summary>
        public enum Sex : byte
        {
            Male = 1,

            Female = 2
        }

        /// <summary>
        /// This enum is used to determine the RelationalType of the DFZ Client.
        /// </summary>
        public enum RelationType : byte
        {
            Father = 1,
            Mother = 2,
            Sister = 3,
            brother = 4,
            spouse = 5,
            Son = 6,
            daughter = 7
        }

        /// <summary>
        /// This enum is used to determine the marital status of the client.
        /// </summary>
        public enum MaritalStatus : byte
        {
            Single = 1,

            Married = 2,

            Widowed = 3,

            Divorced = 4
        }

        /// <summary>
        /// This enum is used when the data is unable to record.
        /// </summary>
        public enum Unrecordable : byte
        {
            TooHigh = 1,

            TooLow = 2,

            Unknown = 3
        }

        /// <summary>
        /// This enum is used to determine the source of the client.
        /// </summary>
        public enum ClientSource : byte
        {
            Urban = 1,

            Rural = 2
        }

        ///// <summary>
        ///// This enum is used to determine the last HIV test result of the client/partner.
        ///// </summary>
        //public enum PreviousHIVTestResult : byte
        //{
        //    Positive = 1,
        //    Negative = 2,
        //    Intermediate = 3,
        //    RefusedTestOrRetest = 4
        //}

        /// <summary>
        /// This enum is used to determine the ART status of the client.
        /// </summary>
        public enum ARTStatus : byte
        {
            [Display(Name = "Not on ART")]
            NotOnART = 1,

            OnART = 2,

            Unknown = 3
        }

        /// <summary>
        /// This enum is used to determine to select yes or no for a particular field.
        /// </summary>
        public enum YesNo : byte
        {
            Yes = 1,

            No = 2
        }

        /// <summary>
        /// This enum is used to determine to select yes or no or unknown for a particular field.
        /// </summary>
        public enum YesNoUnknown : byte
        {
            Yes = 1,

            No = 2,

            Unknown = 3
        }

        public enum YesNoOther : byte
        {
            Yes = 1,

            No = 2,

            Other = 3
        }

        /// <summary>
        /// This enum is used to determine the type of the HIV test of the client.
        /// </summary>
        public enum HIVTestType : byte
        {
            New = 1,

            Retest = 2,

            Confirmatory = 3
        }

        /// <summary>
        /// This enum is used to determine that how the client is tested.
        /// </summary>
        public enum TestedAs : byte
        {
            Individual = 1,

            Couple = 2,

            Family = 3,

            Other = 4
        }

        /// <summary>
        /// This enum is used to determine the type of HIV test of the client. 
        /// </summary>
        public enum HIVTest : byte
        {
            [Display(Name = "HIV Determine")]
            HIV_Determine = 1,

            [Display(Name = "HIV Bioline")]
            HIV_Bioline = 2,

            [Display(Name = "HIV DNA PCR")]
            HIV_DNA_PCR = 3
        }

        /// <summary>
        /// This enum is used to determine the last HIV test result of the client.
        /// </summary>
        public enum HIVTestResult : byte
        {
            Positive = 1,

            Negative = 2,

            Indeterminant = 3,

            Detectable = 4,

            [Display(Name = "Not detected")]
            Not_Detected = 5
        }

        /// <summary>
        /// This enum is used to determine the rtner HIV status of the client.
        /// </summary>
        public enum PartnerHIVStatus : byte
        {
            Positive = 1,

            Negative = 2,

            Indeterminant = 3,

            [Display(Name = "Refused test or result")]
            Refusedtestorresult = 4,

            [Display(Name = "Never tested")]
            NeverTested = 5,

            [Display(Name = "I Don't know")]
            Dont_know = 6
        }

        /// <summary>
        /// This enum is used to determine the type of the HIV of the client.
        /// </summary>
        public enum HIVTypes : byte
        {
            [Display(Name = "HIV-1")]
            HIV1 = 1,

            [Display(Name = "HIV-2")]
            HIV2 = 2,

            [Display(Name = "HIV-1 & HIV-2")]
            HIV1_HIV2 = 3
        }

        /// <summary>
        /// This enum is used to determine the modules of smart care Evo.
        /// </summary>
        public enum Modules : byte
        {
            Vitals = 1,

            HTS = 2
        }

        /// <summary>
        /// This enum is used to determine the religious denomination of the client.
        /// </summary>
        public enum ReligiousDenomination : byte
        {
            Christian = 1,

            Muslim = 2,

            Hindu = 3,

            Buddhist = 4,

            Jewish = 5,

            None = 6
        }

        /// <summary>
        /// This enum is used to determine the medical history of the client.
        /// </summary>
        public enum InformationType : byte
        {
            [Display(Name = "Past drug history")]
            PastDrugHistory = 1,

            [Display(Name = "Admission history")]
            AdmissionHistory = 2,

            [Display(Name = "Surgical history")]
            SurgicalHistory = 3,

            [Display(Name = "Family medical history")]
            FamilyMedicalHistory = 4,

            [Display(Name = "Alcoholand smoking habits")]
            AlcoholandSmokingHabits = 5,

            [Display(Name = "Sibling history")]
            SiblingHistory = 6
        }

        /// <summary>
        /// This enum is used to determine the of the OperationType of Surgery.
        /// </summary>
        public enum OperationType : byte
        {
            Emergency = 1,

            Elective = 2
        }

        /// <summary>
        /// This enum is used to determine the cacx result of the client.
        /// </summary>
        public enum CaCxResult : byte
        {
            Normal = 1,

            Abnormal = 2,

            [Display(Name = "Not Sure")]
            NotSure = 3
        }

        /// <summary>
        /// This enum is used to determine the severity of the allergy of the client.
        /// </summary>
        public enum Severity : byte
        {
            Mild = 1,

            Intermediate = 2,

            Severe = 3,

            Unknown = 4
        }

        /// <summary>
        /// This enum is used to determine the user's access to the modules. 
        /// </summary>
        public enum UserAccessModule : int
        {
            Clients = 1,

            Hts = 2,

            Vitals = 3,

            Users = 4,

            [Display(Name = "Facility Administration")]
            FacilityAdministration = 5,

            [Display(Name = "Nursing Plan")]
            NursingPlan = 6,

            Surgery = 7,

            [Display(Name = "ANC Service")]
            ANCService = 8,

            [Display(Name = "Medical Encounter")]
            MedicalEncounter = 9,

            PEP = 10,

            PrEP = 11,

            ART = 12,

            [Display(Name = "ART IHPAI")]
            ARTIHPAI = 12,

            [Display(Name = "ART FollowUp")]
            ARTFollowUp = 13,

            [Display(Name = "Under Five")]
            UnderFive = 14,

            [Display(Name = "TB Screening")]
            TBScreening = 15,

            [Display(Name = "TB FollowUp")]
            TBFollowUp = 16,

            [Display(Name = "ART Stable on Care")]
            ARTStableOnCare = 17,

            [Display(Name = "Pediatric IHPAI")]
            PediatricIHPAI = 18,

            [Display(Name = "Pediatric FollowUp")]
            PediatricFollowUp = 19,

            [Display(Name = "Pediatric Stable on Care")]
            PediatricStableOnCare = 20,

            [Display(Name = "ART Pediatric")]
            ARTPediatric = 21,

            VMMC = 22,

            [Display(Name = "Pre Transfusion Vital")]
            PreTransfusionVital = 23,

            [Display(Name = "Intra Transfusion on Vital")]
            IntraTransfusionVital = 24,

            [Display(Name = "ANC Labour and Delivery")]
            ANCLabourAndDelivery = 25,

            [Display(Name = "ANC Labour and Delivery PMTCT")]
            ANCLabourAndDeliveryPMTCT = 26,

            [Display(Name = "Postnatal Adult")]
            PostnatalAdult = 27,

            [Display(Name = "Postnatal Pediatric")]
            PostnatalPediatric = 28,

            [Display(Name = "Postnatal PMTCT")]
            PostnatalPMTCT = 29
        }

        public enum GeneralCondition : byte
        {
            Good = 1,

            Stable = 2,

            Critical = 3
        }

        public enum Pallor : byte
        {
            Nil = 1,

            Mild = 2,

            Moderate = 3,

            Severe = 4
        }

        public enum Grade : byte
        {
            Nil = 1,

            [Display(Name = "1+")]
            OnePlus = 2,

            [Display(Name = "2+")]
            TwoPlus = 3,

            [Display(Name = "3+")]
            ThreePlus = 4,

            [Display(Name = "4+")]
            FourPlus = 5
        }

        public enum PresentNotPresent : byte
        {
            Present = 1,

            [Display(Name = "Not present")]
            NotPresent = 2
        }

        public enum IsSuccessfulDelivery : byte
        {
            Yes = 1,
            No = 0
        }
        public enum TypeOfDelivery : byte
        {
            [Display(Name = "Vaginal delivery")]
            VaginalDelivery = 1,

            [Display(Name = "Cesarian section")]
            CesarianSection = 2,

            [Display(Name = "Vacuum extraction")]
            VacuumExtraction = 3
        }
        public enum EyeScore : byte
        {

            [Display(Name = "Spontaneous - open with blinking at baseline (4 points)")]
            Spontaneous = 4,

            [Display(Name = "To verbal stimuli, command, speech (3 points)")]
            Verbal = 3,

            [Display(Name = "To pain only (not applied to face) (2 points)")]
            Pain = 2,

            [Display(Name = "No response (1 point)")]
            Response = 1
        }

        public enum VerbalScore : byte
        {
            [Display(Name = "No response (1 point)")]
            Response = 1,

            [Display(Name = "Incomprehensible speech (2 points)")]
            Speech = 2,

            [Display(Name = "Inappropriate words (3 points)")]
            InappropriateWords = 3,

            [Display(Name = "Confused conversation, but able to answer questions (4 points)")]
            Confused = 4,

            [Display(Name = "Oriented (5 points)")]
            Oriented = 5

        }

        public enum MotorScore : byte
        {
            [Display(Name = "No response (1 point)")]
            Response = 1,

            [Display(Name = "Extension response in response to pain (2 point)")]
            ExtensionToPain = 2,

            [Display(Name = "Flexion in response to pain (decorticate posturing) (3 points)")]
            FlexionToPain = 3,

            [Display(Name = "Withdraws in response to pain (4 points)")]
            WithdrawToPain = 4,

            [Display(Name = "Purposeful movement to painful stimulus (5 points)")]
            PainfulStimulus = 5,

            [Display(Name = "Obeys commands for movement (6 points)")]
            Commands = 6
        }

        public enum DiagnosisType : byte
        {
            NTG = 1,

            ICD11 = 2
        }

        public enum ConditionType : byte
        {
            [Display(Name = "Chronic Condition")]
            ChronicCondition = 1,

            [Display(Name = "Non Chronic Condition")]
            NonChronicCondition = 2
        }
        public enum Certainty : byte
        {
            Confirmed = 1,

            Suspected = 2,

            [Display(Name = "Rule Out")]
            RuleOut = 3,

            [Display(Name = "Ruled Out")]
            RuledOut = 4
        }

        public enum InteractionType : byte
        {
            OPD = 1,

            IPD = 2
        }

        public enum Piority : byte
        {
            Regular = 1,

            Urgent = 2,

            Emergency = 3
        }

        public enum Origin : byte
        {
            [Display(Name = "From within 12Km, within catchment area")]
            FromWithin12km = 1,

            [Display(Name = "From more than 12Km, within catchment area")]
            FromMoreThenWithin12km = 2,

            [Display(Name = "From within district, outside catchment area")]
            FromWithinDistrict = 3,

            [Display(Name = "From outside district")]
            FromOutSideDistrict = 4,

            [Display(Name = "From outside Zambia")]
            FromOutSideZambia = 5,

            Unknown = 6
        }

        public enum InformantRelationship : byte
        {
            Spouse = 1,

            [Display(Name = "Own child")]
            OwnChild = 2,

            [Display(Name = "Step child")]
            StepChild = 3,

            [Display(Name = "Adopted child")]
            AdoptedChild = 4,

            [Display(Name = "Grandchild")]
            GrandChild = 5,

            [Display(Name = "Brother/Sister")]
            BrotherSister = 6,

            Cousin = 7,

            [Display(Name = "Niece/Nephew")]
            NieceNephew = 8,

            [Display(Name = "Brother-in-law/Sister-in-law")]
            BrotherSisterInLaw = 9,

            [Display(Name = "Parent-in-law")]
            ParentInLaw = 10,

            [Display(Name = "Maid/Nanny/House Servant")]
            MaidnannyHouseServant = 11,

            Guardian = 12,

            [Display(Name = "Other Relationship")]
            OtherRelationship = 13
        }

        public enum Plans : byte
        {
            Start = 1,

            [Display(Name = "Don't Start")]
            DontStart = 2,

            Stop = 3
        }

        public enum GuardianRelationship : byte
        {
            Spouse = 1,

            [Display(Name = "Own child")]
            OwnChild = 2,

            [Display(Name = "Step child")]
            Stepchild = 3,

            [Display(Name = "Adopted child")]
            AdoptedChild = 4,

            [Display(Name = "Grand child")]
            GrandChild = 5,

            [Display(Name = "Brother/sister")]
            BrotherSister = 6,

            Cousin = 7,

            [Display(Name = "Niece/Nephew")]
            NieceNephew = 8,

            [Display(Name = "Brother/sister in law")]
            BrotherSisterInlaw = 9,

            [Display(Name = "Parent in law")]
            ParentInlaw = 10,

            [Display(Name = "Maid/nanny/house servant")]
            MaidNannyHouseServant = 11,

            Guardian = 12,

            [Display(Name = "Other Relationship")]
            OtherRelationship = 13
        }

        public enum WhereDeathOccured : byte
        {
            Other = 1,

            [Display(Name = "Other facility")]
            OtherFacility = 2,

            [Display(Name = "Clinic(in-patient")]
            Clinic = 3,

            Hospital = 4,

            Home = 5
        }

        public enum BirthOutcome : byte
        {
            Alive = 1,

            [Display(Name = "Still born")]
            StillBorn = 2,

            Died = 3
        }

        public enum OtherFeedingOption : byte
        {
            [Display(Name = "Exclusive breast feeding")]
            ExclisiveBreast = 1,

            [Display(Name = "Exclusive alternative infant formula")]
            ExclusiveAlternative = 2,

            [Display(Name = "Supplementary food(No Breastfeeding)")]
            SupplementaryFood = 3,

            [Display(Name = "Mixed Feeding food(breast milk and other foods)")]
            MixedFeeding = 4,

            [Display(Name = "Complimentary Feeding and continue breastfeeding upto 2 year or more)")]
            Complimentary = 5,

            [Display(Name = "Mixed based feed after 6 months in addition to other foods")]
            MixedBasedFeed = 6,

            Other = 7
        }

        public enum ReasonForMissing : byte
        {
            Forgot = 1,

            [Display(Name = "Side effect")]
            SideEffects = 2,

            [Display(Name = "Away from home")]
            AwayFromHome = 3,

            Illness = 4,

            [Display(Name = "Medicines finished")]
            MedicinesFinished = 5,

            Other = 6
        }

        public enum EncounterType : byte
        {
            [Display(Name = "Medical Encounter")]
            MedicalEncounter = 1,

            PEP = 2,

            PrEP = 3,

            ART = 4,

            [Display(Name = "ART IHPAI")]
            ARTIHPAI = 5,

            [Display(Name = "ART FollowUp")]
            ARTFollowUp = 6,

            [Display(Name = "Under Five")]
            UnderFive = 7,

            [Display(Name = "TB Screening")]
            TBScreening = 8,

            [Display(Name = "TB FollowUp")]
            TBFollowUp = 9,

            [Display(Name = "ART Stable On Care")]
            ARTStableOnCare = 10,

            [Display(Name = "Pediatric IHPAI")]
            PediatricIHPAI = 11,

            [Display(Name = "Pediatric Follow Up")]
            PediatricFollowUp = 12,

            [Display(Name = "Pediatric Stable On Care")]
            PediatricStableOnCare = 13,

            [Display(Name = "ART Pediatric")]
            ARTPediatric = 14,

            VMMC = 15,

            Surgery = 16,

            [Display(Name = "Nursing Care")]
            NursingPlan = 17,

            [Display(Name = "Vitals & Anthropometry")]
            Vital = 18,

            [Display(Name = "Pre Transfusion Vital")]
            PreTransfusionVital = 19,

            [Display(Name = "Intra Transfusion Vital")]
            IntraTransfusionVital = 20,

            [Display(Name = "ANC Service")]
            ANCService = 21,

            [Display(Name = "ANC Labour & Delivery")]
            ANCLabourAndDelivery = 22,

            [Display(Name = "ANC Labour & Delivery PMTCT")]
            ANCLabourAndDeliveryPMTCT = 23,

            [Display(Name = "Postnatal")]
            PostnatalAdult = 24,

            [Display(Name = "Postnatal")]
            PostnatalPediatric = 25,

            [Display(Name = "ANC Follow Up")]
            ANCFollowUp = 26,

            [Display(Name = "Postnatal PMTCT")]
            PostnatalPMTCT_Adult = 27,

            [Display(Name = "Postnatal PMTCT")]
            PostnatalPMTCT_Pediatric = 28,

            [Display(Name = "ANC Initial Already On ART")]
            ANC_Initial_Already_On_ART = 29,

            [Display(Name = "ANC 1st Time On ART")]
            ANC_1st_Time_On_ART = 30,

            [Display(Name = "ANC Follow Up PMTCT")]
            ANC_Follow_up_PMTCT = 31,

            [Display(Name = "ANC Labour & Delivery Summary")]
            ANCLabourAndDeliverySummary = 32,

            [Display(Name = "Family Planning")]
            FamilyPlanning = 33,

            Referral = 34,

            [Display(Name = "ANC Delivery & Discharge")]
            ANCDeliveryDischargeMother = 35,

            [Display(Name = "ANC Delivery & Discharge (Baby)")]
            ANCDeliveryDischargeBaby = 36,

            [Display(Name = "Medical Encounter IPD")]
            MedicalEncounterIPD = 37,

            HTS = 38,

            Covax = 39,

            Covid = 40,

            [Display(Name = "Birth records")]
            BirthRecords = 41,

            [Display(Name = "Death records")]
            DeathRecords = 42,

            Investigation = 43,

            Prescriptions = 44,

            Dispensations = 45,

            [Display(Name = "Adverse Event")]
            AdverseEvent = 46,

            Result = 47,

            [Display(Name = "Cervical Cancer")]
            CervicalCancer = 48

        }
        public enum EncounterTypePermissions : byte
        {
            [Display(Name = "Medical Encounter")]
            MedicalEncounter = 1,

            PEP = 2,

            PrEP = 3,

            [Display(Name = "ART Adult")]
            Art_Adult = 5,
            [Display(Name = "ART Pediatric")]
            ARTPediatric = 14,

            [Display(Name = "Under Five")]
            UnderFive = 7,
            Investigation = 43,
            [Display(Name = "TB Service")]
            TBService = 8,

            Referral = 34,

            VMMC = 15,
            Surgery = 16,

            [Display(Name = "Nursing Care")]
            NursingPlan = 17,

            [Display(Name = "Labour & Delivery")]
            ANCLabourAndDelivery = 22,
            [Display(Name = "Vitals & Anthropometry")]
            Vital = 18,

            [Display(Name = "Postnatal")]
            Postnatal = 25,

            Covax = 39,

            Covid = 40,

            [Display(Name = "ANC Service")]
            ANCService = 21,

            [Display(Name = "Pharmacy")]

            Prescriptions = 44,

            [Display(Name = "ANC Follow Up PMTCT")]
            ANC_Follow_up_PMTCT = 31,

            [Display(Name = "ANC Labour & Delivery Summary")]
            ANCLabourAndDeliverySummary = 32,

            [Display(Name = "Family Planning")]
            FamilyPlanning = 33,

            [Display(Name = "ANC Delivery & Discharge")]
            ANCDeliveryDischargeMother = 35,

            [Display(Name = "ANC Delivery & Discharge (Baby)")]
            ANCDeliveryDischargeBaby = 36,

            [Display(Name = "Medical Encounter IPD")]
            MedicalEncounterIPD = 37,

            HTS = 38,

            [Display(Name = "Birth records")]
            BirthRecords = 41,

            [Display(Name = "Death records")]
            DeathRecords = 42,

            [Display(Name = "Cervical Cancer")]
            CervicalCancer = 43

        }

        public enum YesNoNotApplicable : byte
        {
            Yes = 1,

            No = 2,

            [Display(Name = "Not applicable")]
            NotApplicable = 3
        }

        public enum Route : byte
        {
            Oral = 1,

            IV = 2,

            Other = 3
        }
        public enum ReasonClientRefusedForVaccination : byte
        {
            Unknown = 1,

            Sick = 2,

            Other = 3
        }

        public enum SourceOfAlert : byte
        {
            [Display(Name = "Call center")]
            CallCenter = 1,

            [Display(Name = "Health facility")]
            HealthFacility = 2,

            Community = 3,

            [Display(Name = "Media scanning")]
            MediaScanning = 4
        }
        public enum ExposureRisks : byte
        {
            [Display(Name = "Contact of confirmed case")]
            Contactofconfirmedcase = 1,

            [Display(Name = "Attended mass gathering")]
            Attendedmassgathering = 2,

            [Display(Name = "Aeaactively reporting cases")]
            Aeaactivelyreportingcases = 3,

            None = 4

        }

        public enum CovidComorbidityCondition : byte
        {
            Pregnancy = 1,

            Diabetes = 2,

            [Display(Name = "Cardiac disease")]
            Cardiacdisease = 3,

            Hypertension = 4,

            COPD = 5,

            [Display(Name = "Chronic Kidney Disease")]
            ChronicKidneyDisease = 6,

            [Display(Name = "Chronic Liver Disease")]
            ChronicLiverDisease = 7,

            [Display(Name = "Immune System Compromised")]
            ImmuneSystemCompromised = 8,

            [Display(Name = "Other respiratory illness")]
            Otherrespiratoryillness = 9,

            None = 10
        }

        public enum ResultType : byte
        {
            Descriptive = 1,
            Numeric = 2,
            Options = 3
        }

        public enum IsResultNormal : byte
        {
            Normal = 1,

            Abnormal = 2,

            [Display(Name = "Not Applicable")]
            NotApplicable = 3
        }

        public enum CauseType : byte
        {
            [Display(Name = "Main Cause of death")]
            MainCauseOfDeath = 1,

            [Display(Name = "Contributing cause of death")]
            ContributingCauseOfDeath = 2
        }

        public enum RegimenFor : byte
        {
            ARVs = 1,

            ATT = 2,

            PEP = 3,

            PrEP = 4,

            TPT = 5
        }

        public enum TimeUnit : byte
        {
            [Display(Name = "Days")]
            Days = 1,

            [Display(Name = "Weeks")]
            Weeks = 2,

            [Display(Name = "Months")]
            Months = 3
        }

        public enum TypeOfEntry : byte
        {
            [Display(Name = "New patient")]
            NewPatient = 1,

            [Display(Name = "Transfer in")]
            TransferIn = 2,

            [Display(Name = "Silent transfer")]
            SilentTransfer = 3
        }

        public enum TypeOfEntryWithoutNew : byte
        {
            [Display(Name = "Transfer in")]
            TransferIn = 2,

            [Display(Name = "Silent transfer")]
            SilentTransfer = 3
        }

        public enum HIVResult : byte
        {
            Yes = 1,

            No = 2
        }

        public enum FamilyMemberType : byte
        {
            Father = 1,

            Mother = 2,

            Child = 3,

            [Display(Name = "Grand parent")]
            Grandparent = 4,

            Uncle = 5,

            Aunty = 6,

            Spouse = 7,

            Other = 8
        }

        public enum NextOfKinType : byte
        {
            Provider = 1,

            Supporter = 2,

            Guardian = 3,

            [Display(Name = "Patient-other")]
            PatientOther = 4,

            Neighbor = 5,

            Parent = 6,

            Other = 7
        }

        public enum PatientsStatus : byte
        {
            [Display(Name = "Patient is being sent to another clinic")]
            SentToAnotherClinic = 1,

            [Display(Name = "Patient is being made inactive")]
            Inactivated = 2,

            [Display(Name = " Patient has stopped ART")]
            StoppedART = 3,

            [Display(Name = "Patient is being reactivated")]
            Reactivated = 4,

            [Display(Name = "Patient has died")]
            Died = 5,

            [Display(Name = "Patient is being Transferred In")]
            TransferIn = 6
        }

        public enum ReferralReason : byte
        {
            [Display(Name = "HIV care")]
            HIVCare = 1,

            [Display(Name = "STI treatment")]
            STITreatment = 2,

            [Display(Name = "Patient care")]
            PatientCare = 3,

            [Display(Name = "Patient request")]
            PatientRequest = 4,

            [Display(Name = "Discharge from facility")]
            DischargeFromFacility = 5,

            [Display(Name = "Complicated care")]
            ComplicatedCare = 6,

            [Display(Name = "Transition to adult care")]
            TransitionToAdultCare = 7,

            [Display(Name = "Penile abnormalities")]
            PenileAbnormalities = 8,

            Other = 9
        }

        public enum ReasonForInactive : byte
        {
            [Display(Name = "Lost to follow up")]
            LostToFollowUp = 1,

            [Display(Name = "Patient is HIV negative")]
            PatientIsHIVNegative = 2,

            Other = 3,

            Refused = 4,
        }

        public enum ReasonForStoppingART : byte
        {
            [Display(Name = "Unable to tolerate ARV")]
            UnableToTolerateARV = 1,

            [Display(Name = "Patient or care giver requested")]
            PatientOrCareGiverRequested = 2,

            [Display(Name = "Client is non adherent to ART despite repeted counselling")]
            ClientIsNonAdherentToARTDespiteRepetedCounselling = 3,

            [Display(Name = "ARV non available")]
            ARVNonAvailable = 4,

            [Display(Name = "Patient does not have reliable care giver")]
            PatientDoesnotHaveReliableCareGiver = 5,

            [Display(Name = "Client has serious drug toxicity")]
            ClientHasSeriousDrugToxicity = 6,

            [Display(Name = "Patient has condition that precludes oral intake")]
            PatientHasConditionThatPrecludesOralIntake = 7,

            [Display(Name = "Patient is negative")]
            PatientIsNegative = 8,

            [Display(Name = "Religious belief")]
            ReligiousBelief = 9,

            Other = 10
        }

        public enum ReasonForReactivation : byte
        {
            [Display(Name = "Restarting ART after having been lost to follow up")]
            RestartingARTAfterHavingBeenLostToFollowUp = 1,

            [Display(Name = "Move back to catchment area")]
            MoveBackToCatchmentArea = 2,

            Other = 3
        }

        //public enum RegimenLine : byte
        //{
        //    FirstLine = 1,

        //    SecondLine = 2,

        //    C = 3
        //}

        public enum PregnancyRisk : byte
        {
            [Display(Name = "High Blood  Pressure")]
            HighBloodPressure = 1,

            [Display(Name = "Depression & Anxiety")]
            DepressionAnxiety = 2
        }

        public enum BreastFeedingRisk : byte
        {
            A = 1,

            B = 2,

            C = 3
        }

        //public enum MedicationFor : byte
        //{
        //    A = 1,

        //    B = 2,

        //    C = 3
        //}

        public enum CounsellingType : byte
        {
            EAC = 1,

            Nutrition = 2,

            Other = 3
        }

        public enum VisitPurposes : byte
        {
            [Display(Name = "Buddy pick up")]
            BuddyPickUp = 1,

            [Display(Name = "Patient present")]
            PatientPresent = 2
        }

        public enum ClinicalMonitoring : byte
        {
            [Display(Name = "New stage condition")]
            NewStageCondition = 1,

            [Display(Name = "Recurrent stage 3 condition")]
            RecurrentStage3Condition = 2,

            [Display(Name = "Recurrent stage 2 condition")]
            RecurrentStage2Condition = 3
        }

        public enum ImmunologicMonitoring
        {
            [Display(Name = "CD4 count below 350 cells/mm3")]
            CD4CountBelow350 = 1,

            [Display(Name = "CD4 count below 200 cells/mm3")]
            CD4CountBelow200 = 2
        }

        public enum VirologicMonitoring : byte
        {
            [Display(Name = "Viral load > 1000 cp/ml")]
            ViralLoadGreaterThen1000 = 1,

            [Display(Name = "Viral load 20-1000 cp/ml")]
            ViralLoad20To1000 = 2,

            [Display(Name = "Viral load < 20 cp/ml")]
            ViralLoadSmallerThen20 = 3,

            [Display(Name = "Target not detected (TND)")]
            TargetNotDetected = 4
        }

        public enum StableOnCareStatus : byte
        {
            [Display(Name = "Virally suppressed last 12 months")]
            VirallySuppressedLast12Months = 1,

            [Display(Name = "On ART > 1 Year")]
            OnARTGreaterThen1Year = 2,

            [Display(Name = "NO adverse drug events")]
            NOAdverseDrugEvents = 3,

            [Display(Name = "No Ols or pregnancy")]
            NoOlsOrPregnancy = 4
        }

        public enum CurrentlyHaveTB : byte
        {
            Yes = 1,

            No = 2,

            [Display(Name = "I don't Know")]
            IDontKnow = 3,
        }

        public enum HowTBDiagnosed : byte
        {
            Microscopy = 1,

            Xray = 2,

            [Display(Name = "Xpert MTB")]
            XpertMTB = 3,

            [Display(Name = "Xpert RIF")]
            XpertRIF = 4,

            [Display(Name = "Drug susceptibility test/culture")]
            DrugSusceptibility = 5,

            [Display(Name = "LF LAM")]
            LF_LAM = 6
        }

        public enum KindOfTB : byte
        {
            Pulmonary = 1,

            Extrapulmonary = 2
        }

        public enum WasATTCompleted : byte
        {
            Yes = 1,

            No = 2,

            [Display(Name = "I don't know")]
            IDontKnow = 3
        }

        public enum MonthOfTBCourse : byte
        {
            [Display(Name = "Sample month one")]
            SampleMonth1 = 1,

            [Display(Name = "Sample month two")]
            SampleMonth2 = 2
        }

        //public enum TBDrug : byte
        //{

        //}

        //public enum TPTDrugs : byte
        //{

        //}

        public enum RecencyType : byte
        {
            Recent = 1,

            [Display(Name = "Long Term")]
            LongTerm = 2,

            Unknown = 3
        }

        public enum ChildExposureStatus : byte
        {
            [Display(Name = "Child not exposed")]
            ChildNotExposed = 1,

            [Display(Name = "Child exposed")]
            ChildExposed = 2,

            [Display(Name = "Status unknown")]
            StatusUnknown = 3
        }

        public enum FeedingCode : byte
        {
            [Display(Name = "Exclusive breastfeeding (in the 1st 6 months, breastfeeding only, no water, on other fluids except medicines)")]
            ExclusiveBreastfeeding = 1,

            [Display(Name = "Exclusive alternative infant formula")]
            ExclusiveAlternativeFormula = 2,

            [Display(Name = "Animal milk")]
            AnimalMilk = 3,

            [Display(Name = "Mixed feeding(breast milk and other foods)")]
            MixedFeeding = 4,

            [Display(Name = "Continued breastfeeding after 6 months in addition to other foods")]
            ContinuedBreastfeeding = 5,

            [Display(Name = "Milk based feed after 6 months in addition to other foods")]
            MilkBasedFeed = 6,

            [Display(Name = "Complimentary feeding after 6 months")]
            Complimentaryfeeding = 7,

            [Display(Name = "Other foods")]
            Other = 8
        }

        public enum Status : byte
        {
            [Display(Name = "Mild malnourishment")]
            MildMalnourishment = 1,

            [Display(Name = "Moderate malnourishment")]
            ModerateMalnourishment = 2,

            [Display(Name = "Severe malnourishment")]
            SevereMalnourishment = 3,

            [Display(Name = "Normal nutrition")]
            NormalNutrition = 4,

            [Display(Name = "Mild obese")]
            MildObese = 5,

            [Display(Name = "Moderate obese")]
            ModerateObese = 6,

            [Display(Name = "Severe obese")]
            SevereObese = 7
        }

        public enum UnderWeight : byte
        {
            Normal = 1,

            [Display(Name = "Moderate underweight")]
            ModerateUnderweight = 2,

            [Display(Name = "Severe underweight")]
            SevereUnderweight = 3
        }

        public enum Obesity : byte
        {
            Normal = 1,

            [Display(Name = "Moderate wasting")]
            ModerateWasting = 2,

            [Display(Name = "Severe wasting")]
            SevereWasting = 3,

            [Display(Name = "Overweight/Obese")]
            OverweightOrObese = 4
        }

        public enum Stunting : byte
        {
            Normal = 1,

            [Display(Name = "Moderate stunting")]
            ModerateStunting = 2,

            [Display(Name = "Severe stunting")]
            SevereStunting = 3
        }

        public enum MalnutritionOutcome : byte
        {
            [Display(Name = "Not applicable")]
            NA = 1,

            Admitted = 2,

            Cured = 3,

            Defaulted = 4,

            [Display(Name = "Non-responder")]
            NonResponder = 5
        }

        public enum ExposureReason : byte
        {
            [Display(Name = "HIV PrEP")]
            HIVPrEP = 1,

            PEP = 2,

            PMTCT = 3,

            ART = 4
        }

        public enum DosesMissed : byte
        {
            [Display(Name = "0: Follow regular pharmacy schedule.")]
            FollowRegularPharmacySchedule = 1,

            [Display(Name = "1: Follow monthly pharmacy schedule.")]
            FollowMonthlyPharmacySchedule = 2,

            [Display(Name = ">=2: Follow 4 weeks of weekly appointment visits.")]
            FollowWeeklyAppointmentVisits = 3
        }

        public enum ReducePharmacyVisit : byte
        {
            [Display(Name = "Monthly")]
            Monthly = 1,

            [Display(Name = "Bi-monthly")]
            BiMonthly = 2,

            [Display(Name = "3 months")]
            ThreeMonth = 3
        }

        public enum ServiceCode : byte
        {
            [Display(Name = "Medical encounter (General)")]
            MedicalEncounter = 1,

            PEP = 2,

            PrEP = 3,

            ART = 4,

            [Display(Name = "ART IHPAI")]
            ARTIHPAI = 5,

            [Display(Name = "ART FollowUp")]
            ARTFollowUp = 6,

            [Display(Name = "Under Five")]
            UnderFive = 7,

            [Display(Name = "TB Screening")]
            TBScreening = 8,

            [Display(Name = "TB FollowUp")]
            TBFollowUp = 9,

            [Display(Name = "ART Stable On Care")]
            ARTStableOnCare = 10,

            [Display(Name = "Pediatric IHPAI")]
            PediatricIHPAI = 11,

            [Display(Name = "Pediatric Follow Up")]
            PediatricFollowUp = 12,

            [Display(Name = "Pediatric Stable On Care")]
            PediatricStableOnCare = 13,

            [Display(Name = "ART Pediatric")]
            ARTPediatric = 14,

            VMMC = 15,

            Surgery = 16,

            [Display(Name = "Nursing Care")]
            NursingPlan = 17,

            [Display(Name = "Vitals & Anthropometry")]
            Vital = 18,

            [Display(Name = "Pre Transfusion Vital")]
            PreTransfusionVital = 19,

            [Display(Name = "Intra Transfusion Vital")]
            IntraTransfusionVital = 20,

            [Display(Name = "ANC Service")]
            ANCService = 21,

            [Display(Name = "ANC Labour & Delivery")]
            ANCLabourAndDelivery = 22,

            [Display(Name = "ANC Labour & Delivery PMTCT")]
            ANCLabourAndDeliveryPMTCT = 23,

            [Display(Name = "Postnatal Adult")]
            PostnatalAdult = 24,

            [Display(Name = "Postnatal Pediatric")]
            PostnatalPediatric = 25,

            [Display(Name = "ANC Follow Up")]
            ANCFollowUp = 26,

            [Display(Name = "Postnatal PMTCT")]
            PostnatalPMTCT_Adult = 27,

            [Display(Name = "Postnatal PMTCT(Pediatric)")]
            PostnatalPMTCT_Pediatric = 28,

            [Display(Name = "ANC Initial Already On ART")]
            ANC_Initial_Already_On_ART = 29,

            [Display(Name = "ANC 1st Time On ART")]
            ANC_1st_Time_On_ART = 30,

            [Display(Name = "ANC Follow Up PMTCT")]
            ANC_Follow_up_PMTCT = 31,

            [Display(Name = "ANC Labour & Delivery Summary")]
            ANCLabourAndDeliverySummary = 32,

            [Display(Name = "Family Planning")]
            FamilyPlanning = 33,

            Referral = 34,

            [Display(Name = "ANC Delivery & Discharge")]
            ANCDeliveryDischargeMother = 35,

            [Display(Name = "ANC Delivery & Discharge (Baby)")]
            ANCDeliveryDischargeBaby = 36,

            [Display(Name = "Medical Encounter IPD")]
            MedicalEncounterIPD = 37,

            HTS = 38,

            Covax = 39,

            Covid = 40,

            [Display(Name = "Birth records")]
            BirthRecords = 41,

            [Display(Name = "Death records")]
            DeathRecords = 42,

            Investigation = 43,

            Prescriptions = 44,

            Dispensations = 45,

            [Display(Name = "Service Queue")]
            ServiceQueue = 97,

            [Display(Name = "Service Points")]
            ServicePoint = 98,
            [Display(Name = "Message Handle Death Record")]
            MeesageHandleDeathRecords = 99
        }
        public enum ServiceQueues : byte
        {
            [Display(Name = "Add Results")]
            Results = 1,
            ResultDetail = 2,
            Dispense = 3,
            DispenseDetail = 4,
        }
        public enum UrgencyType : byte
        {
            Emergency = 1,
            Urgent = 2,
            Standard = 3
        }
        public enum TreatmentOutcome : byte
        {
            Cured = 1,

            Died = 2,

            Completed = 3,

            [Display(Name = "Not evaluated")]
            NotEvaluated = 4,

            [Display(Name = "Rx failed")]
            RxFailed = 5,

            [Display(Name = "Lost to followUp")]
            LostToFollowUp = 6
        }

        public enum DotPlan : byte
        {
            Clinic = 1,

            Volunteer = 2,

            Relative = 3,

            [Display(Name = "No Dot Plan")]
            NoDotPlan = 4
        }

        public enum DiseaseSite : byte
        {
            PTB = 1,

            EPTB = 2,

            Both = 3
        }

        public enum TBType : byte
        {
            Susceptible = 1,

            MDR = 2,

            DR = 3
        }

        public enum SusceptiblePTType : byte
        {
            New = 1,

            [Display(Name = "Treatment after loss to follow up")]
            TreatmentAfterLossToFollowUp = 2,

            Relapse = 3,

            Failure = 4,

            [Display(Name = "Transfer in")]
            TransferIn = 5,

            [Display(Name = "Other previously treatment without know outcome status")]
            OtherPreviouslyTreatmentWithoutKnowOutcomeStatus = 6
        }

        public enum TBSusceptibleRegimen : byte
        {
            RHZE = 1,

            [Display(Name = "2RHZE/4RH")]
            TwoRHZEPer4RH = 2,

            [Display(Name = "2RHZE/10RH")]
            TwoRHZEPer10RH = 3,

            [Display(Name = "2RHZ/E/4RH")]
            TwoRHZPerEPer4RH = 4,

            [Display(Name = "2RHZ/E/10RH")]
            TwoRHZPerEPer10RH = 5,
        }

        public enum MDRDRRegimenGroup : byte
        {
            New = 1,

            Relapse = 2,

            [Display(Name = "Treatment after loss to follow up")]
            TreatmentAfterLossToFollowUp = 3,

            [Display(Name = "Treatment after failure of first treatment")]
            TreatmentAfterFailureOfFirstTreatment = 4,

            [Display(Name = "Treatment after failure of re-treatment")]
            TreatmentAfterFailureOfReTreatement = 5,

            [Display(Name = "Transfer in")]
            TransferIn = 6,

            [Display(Name = "Other previously treatment without know outcome status")]
            OtherPreviouslyTreatmentWithoutKnowOutcomeStatus = 7
        }

        public enum Phase : byte
        {
            [Display(Name = "Continue Initial Phase")]
            ContinueInitialPhase = 1,

            [Display(Name = "Start Continuation Phase")]
            StartContinuationPhase = 2
        }

        public enum MDRDRRegimen : byte
        {
            [Display(Name = "4-6 Km-Mfx-Cfz-Eto-HHD/5Mfx-Cfz-E-Z")]
            FourToSixKmMfxEtoHHD5MfxCfzEZ = 1,

            [Display(Name = "8Z-km-Lfx-Eto-Cs/12Z-Lfx-Eto-Cs")]
            EightZKmLfxEtoCs12ZLfxEtoCs = 2,

            [Display(Name = "Other regimen")]
            OtherRegimen = 3
        }

        public enum ArtPlan : byte
        {
            [Display(Name = "Start ART")]
            StartART = 1,

            [Display(Name = "Refer to next level of care")]
            ReferToNextLevelOfCare = 5
        }

        public enum ArtPlanForFollowupAndStableOnCare : byte
        {
            [Display(Name = "Continue ART")]
            ContinueART = 2,

            [Display(Name = "Modify ART")]
            ModifyART = 3,

            [Display(Name = "Stop ART")]
            StopART = 4,

            [Display(Name = "Switch to next level of ART")]
            SwitchToNextLevelofART = 6
        }

        public enum TPTPlan : byte
        {
            [Display(Name = "Provide TPT")]
            ProvideTPT = 1,

            [Display(Name = "Continue TPT")]
            ContinueTPT = 2,

            [Display(Name = "Discontinue TPT")]
            DiscontinueTPT = 3
        }

        public enum CTXPlan : byte
        {
            [Display(Name = "Provide CTX")]
            ProvideCTX = 1,

            [Display(Name = "Continue CTX")]
            ContinueCTX = 2,

            [Display(Name = "Discontinue CTX")]
            DiscontinueCTX = 3
        }

        public enum EACPlan : byte
        {
            [Display(Name = "Provide EAC")]
            ProvideEAC = 1,

            [Display(Name = "Continue EAC")]
            ContinueEAC = 2,

            [Display(Name = "Discontinue EAC")]
            DiscontinueEAC = 3
        }

        public enum DSDPlan : byte
        {
            [Display(Name = "Start in DSD community based")]
            StartinDSDCommunityBased = 1,

            [Display(Name = "Start in DSD family based")]
            StartInDSDFamilyBased = 2,

            [Display(Name = "Continue in DSD")]
            ContinueInDSD = 3,

            [Display(Name = "Transfer to main stream")]
            TransfertoMainstream = 4
        }

        public enum FluconazolePlan : byte
        {
            [Display(Name = "Provide fluconazole preemptive therapy")]
            ProvideFluconazolePreEmptiveTherapy = 1,

            [Display(Name = "Continue fluconazole preemptive therapy")]
            ContinueFluconazolePreEmptiveTherapy = 2,

            [Display(Name = "Discontinue fluconazole preemptive therapy")]
            DiscontinueFluconazolePreEmptiveTherapy = 3
        }

        public enum AdvancedHIVCare : byte
        {
            [Display(Name = "Does patient have advanced HIV disease")]
            DoesPatientHaveAdvancedHIVDisease = 1,

            [Display(Name = "CrAg Test Done")]
            CrAgTestDone = 2,

            [Display(Name = "LFLAM Test Done")]
            LFLAMTestDone = 3,

            CXR = 4
        }

        public enum Feedback : byte
        {
            [Display(Name = "Taken with Presence")]
            TakenWithPresence = 1,

            [Display(Name = "Taken without Presence")]
            TakenWithoutPresence = 2,

            [Display(Name = "Not Taken")]
            NotTaken = 3
        }

        public enum PresentedHIVStatus : byte
        {
            [Display(Name = "+Ve")]
            Positive = 1,

            [Display(Name = "-Ve")]
            Negative = 2,

            [Display(Name = "Exposed infant")]
            ExposedInfant = 3,

            Unknown = 4
        }

        public enum PositiveNegative : byte
        {
            [Display(Name = "+Ve")]
            Positive = 1,

            [Display(Name = "-Ve")]
            Negative = 2,
        }

        public enum MandibleSize : byte
        {
            Normal = 1,

            Receding = 2
        }

        public enum TongueSize : byte
        {
            Normal = 1,

            Large = 2
        }

        public enum WhenMotherTakenARV : byte
        {
            Antenatal = 1,

            Intrapartum = 2,

            Postpartum = 3
        }

        public enum Duration : byte
        {
            Minutes = 1,

            Hours = 2,

            Days = 3,

            Weeks = 4,

            Months = 5,

            Years = 6,
        }

        public enum HowLongChildTakenARV : byte
        {
            [Display(Name = "Throughout breast feeding")]
            ThroughoutBreastfeeding = 1,

            [Display(Name = "6 Weeks")]
            SixWeeks = 2,

            Other = 3
        }

        public enum StatusCode : int
        {
            [Display(Name = "Adverse Event")]
            AdverseEvent = 0001,

            [Display(Name = "ANC Screening")]
            ANCScreening = 0002,

            [Display(Name = "ANC Service")]
            ANCService = 0003,

            [Display(Name = "Anesthetic Plan")]
            AnestheticPlan = 0004,

            [Display(Name = "Apgar Score")]
            ApgarScore = 0005,

            [Display(Name = "ART Drug Adherence")]
            ARTDrugAdherence = 0006,

            [Display(Name = "ART Response")]
            ARTResponse = 0007,

            [Display(Name = "ART Treatment Plan")]
            ARTTreatmentPlan = 0008,

            Assessment = 0009,

            [Display(Name = "Attached Facility")]
            AttachedFacility = 0010,

            [Display(Name = "Birth Detail")]
            BirthDetail = 0011,

            [Display(Name = "Birth History")]
            BirthHistory = 0012,

            [Display(Name = "Birth Record")]
            BirthRecord = 0013,

            [Display(Name = "Blood Pressure")]
            BloodPressure = 0014,

            [Display(Name = "Blood Transfusion History")]
            BloodTransfusionHistory = 0015,

            Cervix = 0016,

            [Display(Name = "Chief Complaint")]
            ChiefComplaint = 0017,

            [Display(Name = "Child Detail")]
            ChildDetail = 0018,

            [Display(Name = "Childs Development History")]
            ChildsDevelopmentHistory = 0019,

            [Display(Name = "Clients Disability")]
            ClientsDisability = 0020,

            [Display(Name = "Complication")]
            Complication = 0021,

            Condition = 0022,

            [Display(Name = "Contraceptive History")]
            ContraceptiveHistory = 0023,

            Contraction = 0024,

            [Display(Name = "Counselling Service")]
            CounsellingService = 0025,

            Covax = 0026,

            [Display(Name = "Covax Record")]
            CovaxRecord = 0027,

            Covid = 0028,

            [Display(Name = "Covid Comorbidity")]
            CovidComorbidity = 0029,

            [Display(Name = "Covid Symptom Screening")]
            CovidSymptomScreening = 0030,

            [Display(Name = "Death Cause")]
            DeathCause = 0031,

            Diagnosis = 0032,

            [Display(Name = "Descent of Head")]
            DescentOfHead = 0033,

            [Display(Name = "Discharge Metric")]
            DischargeMetric = 0034,

            Dispense = 0035,

            [Display(Name = "Dispensed Item")]
            DispensedItem = 0036,

            Dot = 0037,

            [Display(Name = "Dot Calendar")]
            DotCalendar = 0038,

            Drop = 0039,

            [Display(Name = "Drug Adherence")]
            DrugAdherence = 0040,

            [Display(Name = "Drug PickUp Schedule")]
            DrugPickUpSchedule = 0041,

            [Display(Name = "DSD Assessment")]
            DSDAssessment = 0042,

            [Display(Name = "Exposure Risk")]
            ExposureRisk = 0043,

            [Display(Name = "Family Member")]
            FamilyMember = 0044,

            [Display(Name = "Family Plan")]
            FamilyPlan = 0045,

            [Display(Name = "Family Plan Register")]
            FamilyPlanRegister = 0046,

            [Display(Name = "Feeding History")]
            FeedingHistory = 0047,

            [Display(Name = "Feeding Method")]
            FeedingMethod = 0048,

            [Display(Name = "Fetal Heart Rate")]
            FetalHeartRate = 0049,

            Fluid = 0050,

            [Display(Name = "Fluid Intake")]
            FluidIntake = 0051,

            [Display(Name = "Fluid Output")]
            FluidOutput = 0052,

            [Display(Name = "Guided Examination")]
            GuidedExamination = 0053,

            [Display(Name = "Gyn Obs History")]
            GynObsHistory = 0054,

            [Display(Name = "HIV Prevention History")]
            HIVPreventionHistory = 0055,

            [Display(Name = "HIV Risk Screening")]
            HIVRiskScreening = 0056,

            HTS = 0057,

            [Display(Name = "Identified Allergy")]
            IdentifiedAllergy = 0058,

            [Display(Name = "Identified Complication")]
            IdentifiedComplication = 0059,

            [Display(Name = "Identified Constitutional Symptom")]
            IdentifiedConstitutionalSymptom = 0060,

            [Display(Name = "Identified Cord Stump")]
            IdentifiedCordStump = 0061,

            [Display(Name = "Identified Current Delivery Complication")]
            IdentifiedCurrentDeliveryComplication = 0062,

            [Display(Name = "Identified Delivery Intervention")]
            IdentifiedDeliveryIntervention = 0063,

            [Display(Name = "Identified Eyes Assessment")]
            IdentifiedEyesAssessment = 0064,

            [Display(Name = "Identified Perineum Intact")]
            IdentifiedPerineumIntact = 0065,

            [Display(Name = "Identified Placenta Removal")]
            IdentifiedPlacentaRemoval = 0066,

            [Display(Name = "Identified PPH Treatment")]
            IdentifiedPPHTreatment = 0067,

            [Display(Name = "Identified Preferred Feeding")]
            IdentifiedPreferredFeeding = 0068,

            [Display(Name = "Identified Pregnancy Confirmation")]
            IdentifiedPregnancyConfirmation = 0069,

            [Display(Name = "Identified Prior Sensitization")]
            IdentifiedPriorSensitization = 0070,

            [Display(Name = "Identified Reason")]
            IdentifiedReason = 0071,

            [Display(Name = "Identified Referral Reason")]
            IdentifiedReferralReason = 0072,

            [Display(Name = "Identified TB Finding")]
            IdentifiedTBFinding = 0073,

            [Display(Name = "Identified TB Symptom")]
            IdentifiedTBSymptom = 0074,

            [Display(Name = "Immunization Record")]
            ImmunizationRecord = 0075,

            [Display(Name = "Insertion And Removal Procedure")]
            InsertionAndRemovalProcedure = 0076,

            Investigation = 0077,

            [Display(Name = "Key Population Demographic")]
            KeyPopulationDemographic = 0078,

            [Display(Name = "Medical Condition")]
            MedicalCondition = 0079,

            [Display(Name = "Medical History")]
            MedicalHistory = 0080,

            [Display(Name = "Medical Treatment")]
            MedicalTreatment = 0081,

            Medication = 0082,

            Medicine = 0083,

            [Display(Name = "Mother Delivery Summary")]
            MotherDeliverySummary = 0084,

            [Display(Name = "Mother Detail")]
            MotherDetail = 0085,

            [Display(Name = "Neonatal Abnormality")]
            NeonatalAbnormality = 0086,

            [Display(Name = "Neonatal Death")]
            NeonatalDeath = 0087,

            [Display(Name = "Neonatal Injury")]
            NeonatalInjury = 0088,

            [Display(Name = "New Born Detail")]
            NewBornDetail = 0089,

            [Display(Name = "Next of Kin")]
            NextOfKin = 0090,

            [Display(Name = "Nursing Plan")]
            NursingPlan = 0091,

            

            [Display(Name = "Obstetric Examination")]
            ObstetricExamination = 0092,

            [Display(Name = "Opted Circumcision Reason")]
            OptedCircumcisionReason = 0093,

            [Display(Name = "Opted VMMC Campaign")]
            OptedVMMCCampaign = 0094,

            [Display(Name = "Pain Record")]
            PainRecord = 0095,

            Partograph = 0096,

            [Display(Name = "Partograph Detail")]
            PartographDetail = 0097,

            [Display(Name = "Past Antenatal Visit")]
            PastAntenatalVisit = 0098,

            [Display(Name = "Pelvic And Vaginal Examination")]
            PelvicAndVaginalExamination = 0099,

            [Display(Name = "Perineum Intact")]
            PerineumIntact = 0100,

            [Display(Name = "Placenta Removal")]
            PlacentaRemoval = 0101,

            Plan = 0102,

            [Display(Name = "PMTCT")]
            PMTCT = 0103,

            [Display(Name = "PPH Treatment")]
            PPHTreatment = 0104,

            [Display(Name = "Pregnancy Booking")]
            PregnancyBooking = 0105,

            [Display(Name = "Prescription")]
            Prescription = 0106,

            [Display(Name = "Prior ART Exposure")]
            PriorARTExposure = 0107,

            [Display(Name = "Referral")]
            ReferralModule = 0108,

            [Display(Name = "Quick Examination")]
            QuickExamination = 0109,

            Result = 0110,

            [Display(Name = "Risk Status")]
            RiskStatus = 0111,

            [Display(Name = "Screening And Prevention")]
            ScreeningAndPrevention = 0112,

            [Display(Name = "Service Queue")]
            ServiceQueue = 0113,

            [Display(Name = "Skin Preparation")]
            SkinPreparation = 0114,

            Surgery = 0115,

            [Display(Name = "System Examination")]
            SystemExamination = 0116,

            [Display(Name = "System Review")]
            SystemReview = 0117,

            [Display(Name = "Taken ART Drug")]
            TakenARTDrug = 0118,

            [Display(Name = "Taken TB Drug")]
            TakenTBDrug = 0119,

            [Display(Name = "Taken TPT Drug")]
            TakenTPTDrug = 0120,

            [Display(Name = "TB History")]
            TBHistory = 0121,

            [Display(Name = "TB Service")]
            TBService = 0122,

            Temperature = 0123,

            [Display(Name = "Third Stage Delivery")]
            ThirdStageDelivery = 0124,

            [Display(Name = "TPT History")]
            TPTHistory = 0125,

            [Display(Name = "Treatment Plan")]
            TreatmentPlan = 0126,

            [Display(Name = "Turning Chart")]
            TurningChart = 0127,

            [Display(Name = "Used TB Identification Method")]
            UsedTBIdentificationMethod = 0128,

            [Display(Name = "Uterus Condition")]
            UterusCondition = 0129,

            [Display(Name = "Vaginal Position")]
            VaginalPosition = 0130,

            [Display(Name = "Visit Detail")]
            VisitDetail = 0131,

            [Display(Name = "Visit Purpose")]
            VisitPurpose = 0132,

            Vital = 0133,

            [Display(Name = "WHO Condition")]
            WHOCondition = 0134,

            [Display(Name = "Death Record")]
            DeathRecord = 0135,

            [Display(Name = "Glasgow Coma Scale")]
            GlasgowComaScale = 0136,

            [Display(Name = "Pre-Screening Visit")]
            PreScreeningVisit = 0137,

            [Display(Name = "Thermo Ablation")]
            ThermoAblation = 0138,

            [Display(Name = "Screenings")]
            Screenings = 0139,

            Leep = 0140,

            [Display(Name = "CA-CXPlan")]
            CACXPlan = 0141,

            [Display(Name = "ART Service")]
            ARTService = 0142,

            Nutrition = 00143

        }

        public enum RegionalType : byte
        {
            Regional = 1,

            General = 2
        }

        public enum LocalMedicineType : byte
        {
            Spinal = 1,

            Epidural = 2
        }

        public enum TypeofAnesthesia : byte
        {
            Premedication = 1,

            Induction = 2,

            Maintenance = 3,

            Analgesia = 4,

            Other = 5
        }

        public enum ProcedureType : byte
        {
            [Display(Name = "Dorsal slit")]
            DorsalSlit = 1,

            Device = 2,

            Other = 3
        }

        public enum SutureType : byte
        {
            [Display(Name = "3.0 Chromic catgut")]
            Chromic = 1,

            [Display(Name = "3.0 Vicryl")]
            Vicryl = 2,

            Other = 3
        }

        public enum LevelOfAnesthesia : byte
        {
            Cervical = 1,

            Thoracic = 2,

            Lumbar = 3
        }

        public enum PregnancyConcludedReason : byte
        {
            [Display(Name = "Maternal death")]
            MaternalDeath = 1,

            [Display(Name = "Early termination")]
            EarlyTermination = 2,

            [Display(Name = "Elective abortion")]
            ElectiveAbortion = 3,

            [Display(Name = "Spontaneous complete abortion")]
            SpontaneousCompleteAbortion = 4,

            [Display(Name = "Spontaneous Incomplete Abortion")]
            SpontaneousIncompleteAbortion = 5,

            [Display(Name = "Spontaneous missed abortion")]
            SpontaneousMissedAbortion = 6,

            [Display(Name = "Spontaneous septic abortion")]
            SpontaneousSepticAbortion = 7,

            [Display(Name = "Spontaneous threatened abortion")]
            SpontaneousThreatenedAbortion = 8,

            [Display(Name = "Spontaneous inevitable abortion")]
            SpontaneousInevitableAbortion = 9,

            [Display(Name = "Delivered at other facilities")]
            DeliveredAtOtherFacilites = 10,

            Others = 11
        }

        public enum SyphilisTestType : byte
        {
            [Display(Name = "Syphilis RDT")]
            SyphilisRDT = 1,

            RPR = 2
        }

        public enum BreastFeedingChoice : byte
        {
            EBF = 1,

            ERF = 2
        }

        public enum BreastFeedingType : byte
        {
            EBF = 1,

            [Display(Name = "Mixed Feeding")]
            MixedFeeding = 2
        }

        public enum MetarnalOutcome : byte
        {
            Good = 1,

            Fair = 2,

            Ill = 3,

            Critical = 4
        }

        public enum PregnancyConclusion : byte
        {
            [Display(Name = "Early termination")]
            EarlyTermination = 1,

            Ectopic = 2,

            [Display(Name = "Full term")]
            FullTerm = 3,

            [Display(Name = "Molar pregnancy")]
            MolarPregnancy = 4,

            [Display(Name = "Pseudo pregnancy")]
            PseudoPregnancy = 5
        }

        public enum EarlyTerminationReason : byte
        {
            [Display(Name = "Elective abortion")]
            ElectiveAbortion = 1,

            [Display(Name = "Spontaneous complete abortion")]
            SpontaneousCompleteAbortion = 2,

            [Display(Name = "Spontaneous incomplete abortion")]
            SpontaneousIncompleteAbortion = 3,

            [Display(Name = "Spontaneous missed abortion")]
            SpontaneousMissedAbortion = 4,

            [Display(Name = "Spontaneous septic abortion")]
            SpontaneousSepticAbortion = 5,

            [Display(Name = "Spontaneous threatened abortion")]
            SpontaneousThreatenedAbortion = 6,

            [Display(Name = "Spontaneous inevitable abortion")]
            SpontaneousInevitableAbortion = 7
        }

        public enum DeliveryMethod : byte
        {
            [Display(Name = "Early termination")]
            EarlyTermination = 1,

            SVD = 2,

            BRE = 3,

            VAC = 4,

            FOR = 5,

            [Display(Name = "Caesarean section")]
            CeaserianSection = 6
        }

        public enum PueperiumOutcome : byte
        {
            Normal = 1,

            Anaemia = 2,

            Fever = 3,

            Hypertension = 4,

            Infection = 5
        }

        public enum ChildDetailBirthOutcome : byte
        {
            [Display(Name = "Alive and healthy")]
            AliveAndHealthy = 1,

            [Display(Name = "Alive but chronically ill")]
            AliveButChronicallyIll = 2,

            [Display(Name = "Still born")]
            StillBorn = 3,

            [Display(Name = "Died shortly after birth")]
            DiedShortlyAfterBirth = 4,

            [Display(Name = "Died under 5")]
            DiedUnderFive = 5,

            [Display(Name = "Died after 5")]
            DiedAfterFive = 6
        }

        public enum BloodGroup : byte
        {
            [Display(Name = "A +ive")]
            APositive = 1,

            [Display(Name = "A -ive")]
            ANegative = 2,

            [Display(Name = "B +ive")]
            BPositive = 3,

            [Display(Name = "B -ive")]
            BNegative = 4,

            [Display(Name = "AB +ive")]
            ABPositive = 5,

            [Display(Name = "AB -ive")]
            ABNegative = 6,

            [Display(Name = "O +ive")]
            OPositive = 7,

            [Display(Name = "O -ive")]
            ONegative = 8
        }

        public enum RHSensitivity : byte
        {
            [Display(Name = "Rhesus sensitized")]
            RhesusSensitized = 1,

            [Display(Name = "Rhesus non sensitized")]
            RhesusNonSensitized = 2
        }

        public enum Scoring : byte
        {
            [Display(Name = "+ve")]
            Positive = 1,

            [Display(Name = "-ve")]
            Negative = 2,

            Indeterminant = 3
        }

        public enum HIVStatus : byte
        {
            [Display(Name = "+ve")]
            Positive = 1,

            [Display(Name = "-ve")]
            Negative = 2,

            Indeterminant = 3
        }

        public enum Albumin : byte
        {
            Nil = 1,

            Trace = 2,

            [Display(Name = "+")]
            Positive = 3,

            [Display(Name = "++")]
            TwoPositive = 4,

            [Display(Name = "+++")]
            ThreePositive = 5,

            [Display(Name = "++++")]
            FourPositive = 6,
        }

        public enum Presentation : byte
        {
            Cephalic = 1,

            Breech = 2,

            Undefined = 3
        }

        public enum Engaged : byte
        {
            Engaged = 1,

            [Display(Name = "Not engaged")]
            NotEngaged = 2,
        }

        public enum Lie : byte
        {
            Transverse = 1,

            Longitudinal = 2,

            Oblique = 3,

            Undefined = 4
        }

        public enum FetalHeart : byte
        {
            FMF = 1,

            Heard = 2,

            [Display(Name = "Not heard")]
            NotHeard = 3
        }

        public enum Contractions : byte
        {
            Regular = 1,

            Irregular = 2
        }

        public enum Position : byte
        {
            A = 1,

            B = 2,

            C = 3
        }

        public enum MalariaDose : byte
        {
            [Display(Name = "ITN issued")]
            ITNIssued = 1,

            [Display(Name = "ITN used")]
            ITNUsed = 2,

            [Display(Name = "SP DOT given")]
            SPDOTGiven = 3
        }

        public enum AmeniaDose : byte
        {
            Screened = 1,

            [Display(Name = "Iron given")]
            IronGiven = 2,

            [Display(Name = "FeSO4 given")]
            FeSO4Given = 3
        }

        public enum TetanusDose : byte
        {
            [Display(Name = "Pregnancy fully TT protected")]
            PregnancyFullyTTProtected = 1,

            [Display(Name = "TT dose given")]
            TTDoseGiven = 2,

            //[Display(Name = "Indicate dose no")]
            //IndicateDoseNo = 3
        }

        public enum SyphilisDose : byte
        {
            [Display(Name = "RPR test done")]
            RPRTestDone = 1,

            [Display(Name = "Test positive")]
            TestPositive = 2,

            [Display(Name = "Benz Pen given")]
            BenzPenGiven = 3

        }

        public enum HepatitisBDose : byte
        {
            [Display(Name = "Hepatitis B test done")]
            HepBTesDone = 1,

            [Display(Name = "HepB test positive")]
            TestPositive = 2,

            [Display(Name = "Treated for HepB")]
            Treated = 3

        }

        public enum PregnancyConfirmationWay : byte
        {
            Examination = 1,

            [Display(Name = "Pregnancy test")]
            PregnancyTest = 2,

            Ultrasound = 3,

            [Display(Name = "Fetal movements")]
            FetalMovements = 4
        }

        public enum VisitDetailsType : byte
        {
            [Display(Name = "Within 48hrs")]
            Within48hrs = 1,

            [Display(Name = "2-6 days")]
            TwoToSixdays = 2,

            [Display(Name = "7-42 days")]
            SevenTo42days = 3,

            [Display(Name = "After 42 days")]
            After42days = 4,

            [Display(Name = "14 weeks")]
            Fourteenweeks = 5,

            [Display(Name = "6 months")]
            SixMonths = 6,

            [Display(Name = "9 months")]
            NineMonths = 7,

            [Display(Name = "12 months")]
            TwelveMonths = 8,

            [Display(Name = "18 months")]
            EighteenMonths = 9,

            Other = 10
        }

        public enum Feeding : byte
        {
            Well = 1,

            Poor = 2,

            [Display(Name = "Not feeding")]
            NotFeeding = 3
        }

        public enum Vulva : byte
        {
            Normal = 1,

            Abnormal = 2
        }

        public enum Lochia : byte
        {
            Lubra = 1,

            Serosa = 2,

            Alba = 3,

            Abnormal = 4
        }

        public enum Perineum : byte
        {
            Intact = 1,

            Tear = 2
        }

        public enum EyesCondition : byte
        {
            Dischargeb = 1,

            Jaundice = 2,

            Cataracts = 3,

            Other = 4,

            Normal = 5
        }

        public enum CordStumpCondition : byte
        {
            Infected = 1,

            Granuloma = 2,

            Other = 3,

            Normal = 4
        }

        public enum TBScreening : byte
        {
            [Display(Name = "Suspected TB")]
            SuspectedTB = 1,

            [Display(Name = "Known TB")]
            KnownTB = 2,

            [Display(Name = "TB Diagnosed")]
            TBDiagnosed = 3,

            [Display(Name = "No TB")]
            NoTB = 4
        }

        public enum BirthType : byte
        {
            Single = 1,

            Twins = 2,

            Triplets = 3,

            More = 4
        }

        public enum DeliveredType : byte
        {
            [Display(Name = "Delivered by clinician")]
            DeliveredByClinician = 1,

            [Display(Name = "Delivered by TBA")]
            DeliveredByTBA = 2
        }

        public enum DeliveryLocation : byte
        {
            Home = 1,

            [Display(Name = "Health facility")]
            HealthFacility = 2
        }

        public enum DeliveredBy : byte
        {
            Doctor = 1,

            Midwife = 2,

            [Display(Name = "Enrolled midwife")]
            EnrolledMidwife = 3,

            Nurse = 4,

            [Display(Name = "Registered nurse")]
            RegisterredNurse = 5,

            [Display(Name = "Enrolled nurse")]
            EnrolledNurse = 6,

            Licentiate = 7
        }

        public enum Treatments : byte
        {
            [Display(Name = "Dextrose 5 percent")]
            DextroseFivePercent = 1,

            [Display(Name = "Ringers lactate")]
            RingersLactate = 2,

            [Display(Name = "Normal Saline")]
            NormalSaline = 3,

            [Display(Name = "Dextrose normal saline")]
            DextroseNormalSaline = 4,

            Methergine = 5,

            [Display(Name = "Dextran 70")]
            Dextran70 = 6
        }

        public enum ConditionOfUterus : byte
        {
            [Display(Name = "Not Assessed")]
            NotAssessed = 1,

            [Display(Name = "Normal: contracted")]
            Normalcontracted = 2,

            [Display(Name = "Abnormal: Boggy")]
            AbnormalBoggy = 3,

            [Display(Name = "Abnormal other")]
            AbnormalOther = 4
        }

        public enum TearDegree : byte
        {
            [Display(Name = "1st")]
            First = 1,

            [Display(Name = "2nd")]
            Second = 2,

            [Display(Name = "3rd")]
            Third = 3,

            [Display(Name = "4th")]
            Fourth = 4
        }

        public enum Methods : byte
        {
            Formula = 1,

            [Display(Name = "Breast feeding")]
            Breastfeeding = 2,

            [Display(Name = "Breastfed in 1hr")]
            Breastfed1hr = 3,

            [Display(Name = "Breastfeeding well?")]
            BreastfeedingWell = 4
        }

        public enum PerinatalProblems : byte
        {
            [Display(Name = "Congenital problems")]
            CongenitalProblems = 1,

            [Display(Name = "Pre/post maturity")]
            PrePostMaturity = 2,

            [Display(Name = "Low apgar score")]
            LowApgarScore = 3
        }

        public enum TreatmentsOfPPH : byte
        {
            [Display(Name = "Bimanual compression")]
            BimanualCompression = 1,

            Medication = 2,

            [Display(Name = "Blood transfusion")]
            BloodTransfusion = 3,

            Fluids = 4,

            Surgery = 5
        }

        public enum Placenta : byte
        {
            [Display(Name = "Manual removal")]
            ManualRemoval = 1,

            [Display(Name = "Controlled cord traction")]
            ControlledCordTraction = 2,

            [Display(Name = "Uterine massage")]
            UterineMassage = 3,

            [Display(Name = "Provision of oxytocin")]
            ProvisionOfOxytocin = 4
        }

        public enum Abnormalities : byte
        {
            [Display(Name = "CNS: Spina bifida")]
            CNSSpinalBifidia = 1,

            [Display(Name = "CNS: Anencephaly")]
            CNSAnencephaly = 2,

            [Display(Name = "Cardiac: PDA")]
            CardiacPDA = 3,

            [Display(Name = "Septal: ASD")]
            SeptalASD = 4,

            [Display(Name = "Septal: VSD")]
            SeptalVSD = 5,

            Exomphalos = 6,

            [Display(Name = "Talipes ( undefined, club foot)")]
            TalipesUndefinedClubFoot = 7,

            Polydactyly = 8,

            [Display(Name = "Cleft Palate")]
            CleftPalate = 9,

            [Display(Name = "Cleft Lip")]
            CleftLip = 10,

            [Display(Name = "False Teeth")]
            FalseTeeth = 11,

            [Display(Name = "Erb's Palsy")]
            ErbPalsy = 12,

            [Display(Name = "Inperforate Anus")]
            InperforateAnus = 13,

            [Display(Name = "Genital Abnormalities")]
            GenitalAbnormalities = 14
        }

        public enum Injuries : byte
        {
            [Display(Name = "Skull Fracture")]
            SkullFracture = 1,

            [Display(Name = "Fracture Clavicle")]
            FractureClavicle = 2,

            [Display(Name = "Fracture Upper Limbs")]
            FractureUpperLimbs = 3,

            [Display(Name = "Fracture Lower Limbs")]
            FractureLowerLimbs = 4,

            [Display(Name = "Birth Asphyxia")]
            BirthAsphyxia = 5,

            Septicemia = 6,

            [Display(Name = "Instrumental Delivery")]
            InstrumentalDelivery = 7,

            [Display(Name = "Other Injury")]
            OtherInjury = 8
        }

        public enum Interventions : byte
        {
            Antibiotics = 1,

            Augmentation = 2,

            Decapitation = 3,

            [Display(Name = "Induction of labor")]
            InductionofLabor = 4,

            [Display(Name = "Seizure prophylaxis")]
            SeizuireProphylaxis = 5,

            Symphysiotomy = 6,

            [Display(Name = "Vaginal wash")]
            VaginalWash = 7,

            [Display(Name = "Version and extraction")]
            VersionAndExtraction = 8
        }

        public enum DeliveryComplications : byte
        {
            [Display(Name = "Abnormal bleeding")]
            AbnormalBleeding = 1,

            [Display(Name = "Abnormal presentation")]
            AbnormalPresentation = 2,

            [Display(Name = "Cord prolapse")]
            CordProlapse = 3,

            [Display(Name = "Retained placenta")]
            RetainedPlacenta = 4,

            [Display(Name = "Maternal death")]
            MaternalDeath = 5,

            [Display(Name = "Fetal distress")]
            FetalDistress = 6,

            [Display(Name = "Prolonged labor")]
            ProlongedLabor = 7,

            [Display(Name = "Obstructed labor")]
            ObstructedLabor = 8,

            Eclampsia = 9,

            [Display(Name = "Pre-eclampsia")]
            PreEclampsia = 10
        }

        public enum ApgarTimeSpan : byte
        {
            [Display(Name = "1 MIN")]
            OneMIN = 1,

            [Display(Name = "5 MIN")]
            FiveMIN = 2,

            [Display(Name = "10 MIN")]
            TenMIN = 3
        }

        public enum Appearance : byte
        {
            [Display(Name = "Cyanotic/Pale all over (0 point)")]
            CyanoticPerPaleAllOver = 1,

            [Display(Name = "Peripheral cyanosis only (2 point)")]
            PeripheralCyanosisOnly = 2,

            [Display(Name = "Pink (2 points)")]
            Pink = 3
        }

        public enum Pulses : byte
        {
            [Display(Name = "Zero (0 point)")]
            Zero = 1,

            [Display(Name = "<100 (1 point)")]
            SmallerThenHundred = 2,

            [Display(Name = "100-140 (2 points)")]
            HumdredToHundredFourty = 3,
        }

        public enum Grimace : byte
        {
            [Display(Name = "No response to stimulation (0 point)")]
            NoResponsetoStimulation = 1,

            [Display(Name = "Grimace or weak cry when stimulated (1 point)")]
            GrimaceOrWeakCryWhenStimulated = 2,

            [Display(Name = "Cry when stimulated (2 points)")]
            CryWhenStimulated = 3
        }

        public enum Activity : byte
        {
            [Display(Name = "Floppy (0 point)")]
            Floppy = 1,

            [Display(Name = "Some flexion (1 point)")]
            SomeFlexion = 2,

            [Display(Name = "Wel flexed and resisting (2 points)")]
            WelFlexedAndResisting = 3
        }

        public enum Respiration : byte
        {
            [Display(Name = "Apneic (0 point)")]
            Apneic = 1,

            [Display(Name = "Slow irregular breathing (1 point)")]
            SlowIrregularBreathing = 2,

            [Display(Name = "Strong cry (2 points)")]
            StrongCry = 3
        }

        public enum Perineums : byte
        {
            Episiotomy = 1,

            Sutured = 2,

            [Display(Name = "Anterior Perineum Tear")]
            AnteriorPeriuneumTear = 3,

            [Display(Name = "Posterior Perineum Tear")]
            PosteriorPeriuneumTear = 4,

            [Display(Name = "Vaginal Tear")]
            VaginalTear = 5,

            [Display(Name = "Cervical Tear")]
            CervicalTear = 6,

            Other = 7
        }

        public enum PregnancyIntension : byte
        {
            [Display(Name = "Wants to become pregnant")]
            WantstoBecomePregnant = 1,

            [Display(Name = "Does not intend to get pregnant")]
            DoesNotIntendtoGetPregnant = 2,

            [Display(Name = "Unsure or undecided about pregnancy intention")]
            UnsureOrUndecidedAboutPregnancyIntention = 3
        }

        public enum ReasonForFollowUp : byte
        {
            [Display(Name = "Stop Contraception")]
            StopContraception = 1,

            [Display(Name = "Switch Method")]
            SwitchMethod = 2,

            [Display(Name = " Issues and Concerns/method failure")]
            IssueAndConcernsMethodFailure = 3,

            Continuation = 4,

            [Display(Name = "Implant removal")]
            ImplantRemoval = 5,

            [Display(Name = "IUD removal")]
            IUDRemoval = 6,

            Other = 7
        }

        public enum ReasonForStopping : byte
        {
            [Display(Name = "Desire for pregnancy")]
            SesireForPregnancy = 1,

            [Display(Name = "Reported failure")]
            ReportedFailure = 2,

            Other = 3
        }

        public enum PueperalCondition : byte
        {
            Normal = 1,

            Anemic = 2,

            Fever = 3,

            HyperTension = 4,

            Infected = 5
        }

        public enum PerineumCondition : byte
        {
            Intact = 1,

            Swollen = 2,

            Hematoma = 3,

            Infected = 4
        }

        public enum VaginalMuscleTone : byte
        {
            strong = 1,

            weak = 2
        }

        public enum CervixColour : byte
        {
            Pink = 1,

            Bluish = 2
        }

        public enum CervicalConsistency : byte
        {
            Soft = 1,

            Firm = 2,

            Other = 3
        }

        public enum UterusPosition : byte
        {
            Anteverted = 1,

            Retroverted = 2,
        }

        public enum NormalAbnormal : byte
        {
            Normal = 1,

            Abnormal = 2
        }

        public enum ReasonOfNotPregnant : byte
        {
            [Display(Name = "Last menstrual bleeding started within the past 7 days")]
            LastMenstrualBleedingPast7Days = 1,

            [Display(Name = "Abstained from sexual intercourse since last normal menses, delivery, miscarriage or abortion")]
            AbstainedIntercourseNormalMensesDeliveryMiscarriageAbortion = 2,

            [Display(Name = "Consistently and correctly using reliable contraceptive method")]
            ConsistentlyCorrectlyReliableContraceptive = 3,

            [Display(Name = "Gave birth in the last 4 weeks")]
            GaveBirthLast4Weeks = 4,

            [Display(Name = "Gave birth less than 6 months ago, fully or nearly fully breastfeeding, and amenorrhoeic")]
            GaveBirthLessThan6MonthsFullyNearlyBreastfeedingAmenorrhoeic = 5,

            [Display(Name = "Miscarriage or abortion in the past 7 days")]
            MiscarriageAbortionPast7Days = 6,

            [Display(Name = "Miscarriage or abortion in the past 12 days")]
            MiscarriageAbortionPast12Days = 7,

            [Display(Name = "Reasonably certain a woman is not pregnant")]
            ReasonablyCertainWomanNotPregnant = 8
        }

        public enum FPMethodPlan : byte
        {
            [Display(Name = "Stop using method")]
            StopUsingMethod = 1,

            [Display(Name = "Continue with method")]
            ContinueWithMethod = 2,

            [Display(Name = "Switch method")]
            SwitchMethod = 3
        }

        public enum FPMethodPlanRequest : byte
        {
            [Display(Name = "Copper-bearing intrauterine device (Cu-IUD)")]
            CopperBearingIntrauterineDeviceCuIUD = 1,

            [Display(Name = "Levonorgestrel intrauterine device (LNG-IUD)")]
            LevonorgestrelIntrauterineDeviceLNGIUD = 2,

            [Display(Name = "Etonogestrel (ETG) one-rod")]
            EtonogestrelETGOneRod = 3,

            [Display(Name = "Levonorgestrel (LNG) two-rod")]
            LevonorgestrelLNGTwoRod = 4,

            [Display(Name = "DMPA-IM")]
            DMPAIM = 5,

            [Display(Name = "DMPA-SC")]
            DMPASC = 6,

            [Display(Name = "Norethisterone enanthate (NET-EN)")]
            NorethisteroneEnanthateNETEN = 7,

            [Display(Name = "Progestogen-only pills (POPs)")]
            ProgestogenOnlyPillsPOPs = 8,

            [Display(Name = "Combined oral contraceptives (COCs)")]
            CombinedOralContraceptivesCOCs = 9,

            [Display(Name = "Combined contraceptive patch")]
            CombinedContraceptivePatch = 10,

            [Display(Name = "Combined contraceptive vaginal ring (CVR)")]
            CombinedContraceptiveVaginalRingCVR = 11,

            [Display(Name = "Progesterone-releasing vaginal ring (PVR)")]
            ProgesteroneReleasingVaginalRingPVR = 12,

            [Display(Name = "Lactational amenorrhea method (LAM)")]
            LactationalAmenorrheaMethodLAM = 13,

            [Display(Name = "Male condoms")]
            MaleCondoms = 14,

            [Display(Name = "Female condoms")]
            FemaleCondoms = 15,

            [Display(Name = "Emergency contraceptive pills (ECPs)")]
            EmergencyContraceptivePillsECPs = 16,

            [Display(Name = "Fertility awareness-based methods (FAB)")]
            FertilityAwarenessBasedMethodsFAB = 17,

            [Display(Name = "Male Sterilization")]
            MaleSterilization = 18,

            [Display(Name = "Female Sterilization")]
            FemaleSterilization = 19,

            Withdrawal = 20,

            None = 21
        }

        public enum SelectedFamilyPlan : byte
        {
            [Display(Name = "Copper-bearing intrauterine device (Cu-IUD)")]
            CU_IUD = 1,

            [Display(Name = "Levonorgestrel intrauterine device (LNG-IUD)")]
            LNG_IUD = 2,

            [Display(Name = "Etonogestrel (ETG) one-rod")]
            ETG = 3,

            [Display(Name = "Levonorgestrel (LNG) two-rod")]
            LNG = 4,

            [Display(Name = "DMPA-IM")]
            DMPA_IM = 5,

            [Display(Name = "DMPA-SC")]
            DMPA_SC = 6,

            [Display(Name = "Norethisterone enanthate (NET-EN)")]
            Noresthisterone = 7,

            [Display(Name = "Progestogen-only pills (POPs)")]
            POPs = 8,

            [Display(Name = "Combined oral contraceptives (COCs)")]
            COCs = 9,

            [Display(Name = "Combined contraceptive patch")]
            COCOP = 10,

            [Display(Name = "Combined contraceptive vaginal ring (CVR)")]
            CVR = 11,

            [Display(Name = "Progesterone-releasing vaginal ring (PVR)")]
            PVR = 12,

            [Display(Name = "Lactational amenorrhea method (LAM)")]
            LAM = 13,

            [Display(Name = "Male condoms")]
            Male_Condom = 14,

            [Display(Name = "Female condoms")]
            Female_Condom = 15,

            [Display(Name = "Emergency contraceptive pills (ECPs)")]
            ECPs = 16,

            [Display(Name = "Fertility awareness-based methods (FAB)")]
            FAB = 17,

            [Display(Name = "Male sterilization")]
            MaleSTR = 18,

            [Display(Name = "Female sterilization")]
            FemaleSTR = 19,

            [Display(Name = "Withdrawal")]
            Withdrawal = 20,

            [Display(Name = "No method")]
            NoMethod = 21
        }

        public enum ClassifyFPMethod : byte
        {
            [Display(Name = "A condition for which there is no restriction for the use of the contraceptive method")]
            Condition_For_NoRestriction = 1,

            [Display(Name = "A condition where the advantages of using the method generally outweigh the theoretical or proven risks")]
            Condition_For_Advantages_Method = 2,

            [Display(Name = "A condition where the theoretical or proven risks usually outweigh the advantages of using the method")]
            Condition_For_Theoretical_Risk = 3,

            [Display(Name = "A condition which represents an unacceptable health risk if the contraceptive method is used")]
            Condition_For_Represents_Unacceptable = 4
        }

        public enum FamilyPlans : byte
        {
            Accept = 1,

            Caution = 2,

            Delay = 3,

            Special = 4
        }

        public enum FPProvidedPlace : byte
        {
            [Display(Name = "Provided on site")]
            ProvidedOnSite = 1,

            [Display(Name = "Referral")]
            Referral = 2,

            [Display(Name = "Home/self-administered")]
            HomeSelfAdministered = 3
        }

        public enum ReasonForNoPlan : byte
        {
            [Display(Name = "Chooses not to use")]
            ChoosesNotUse = 1,

            [Display(Name = "Out of stock – Method")]
            OutOfStockMethod = 2,

            [Display(Name = "Out of stock – Equipment or supplies")]
            OutOfStockEquipmentSupplies = 3,

            [Display(Name = "IUD insertion attempted but not done")]
            IUDInsertionAttemptedNotDone = 4,

            [Display(Name = "Client requires referral")]
            ClientRequiresReferral = 5
        }

        public enum ClientNotReceivePreferredOption : byte
        {
            [Display(Name = "Out of stock – Method")]
            OutOfStockMethod = 1,

            [Display(Name = "Out of stock – Equipment /supplies")]
            OutOfStockEquipmentSupplies = 2,

            [Display(Name = "Health worker skill mismatch")]
            HealthWorkerSkillMismatch = 3,

            [Display(Name = "Client requires referral")]
            ClientRequiresReferral = 4
        }

        public enum BackupMethodUsed : byte
        {
            [Display(Name = "Male condom")]
            MaleCondom = 1,

            [Display(Name = "Female condom")]
            FemaleCondom = 2,

            Abstinence = 3,

            [Display(Name = "Emergency contraceptive pills (ECPs)")]
            EmergencyContraceptivePillsECPs = 4
        }

        public enum ReferredBy : byte
        {
            [Display(Name = "External Facility")]
            ExternalFacility = 1,

            [Display(Name = "Community Based Agent")]
            CommunityBasedAgent = 2,

            [Display(Name = "Other Clinical Department")]
            OtherClinicalDepartment = 3,

            Spouse = 4,

            Friend = 5
        }

        public enum ClientStaysWith : byte
        {
            Parents = 1,

            Siblings = 2,

            [Display(Name = "Extended Family")]
            ExtendedFamily = 3,

            Partner = 4,

            [Display(Name = "Friend(s)")]
            Friend = 5,

            [Display(Name = "No one")]
            NoOne = 6
        }

        public enum CommunicationPreference : byte
        {
            SMS = 1,

            [Display(Name = "Voice Call")]
            VoiceCall = 2,

            [Display(Name = "Client's email")]
            ClientMail = 3
        }

        public enum AlternativeContact : byte
        {
            [Display(Name = "Next of kin")]
            NextOfKin = 1,

            Parent = 2,

            Guardian = 3,

            Neighbor = 4,

            Sibling = 5
        }

        public enum PatientType : byte
        {
            Referral = 1,

            [Display(Name = "Self Referred")]
            SelfReferred = 2,

            Scheme = 3,

            Exempt = 4
        }

        public enum ReferralType : byte
        {
            Internal = 1,

            External = 2
        }

        public enum ReasonForVisit : byte
        {
            [Display(Name = "New acceptor")]
            New = 1,

            [Display(Name = "Subsequent visit")]
            FollowUp = 2,

            [Display(Name = "Restart")]
            Restarted = 3,

            [Display(Name = "Emergency Contraception")]
            EmergencyContraception = 4,

            [Display(Name = "Discontinue")]
            StoppingFamilyPlanning = 5
        }

        public enum EpisiotomyCondition : byte
        {
            Intact = 1,

            Swollen = 2,

            Hematoma = 3,

            Infected = 4,

            Other = 5
        }

        public enum ClientPreferences : byte
        {
            [Display(Name = "Highly Effective")]
            HighlyEffective = 1,

            [Display(Name = "STI Prevention")]
            STIPrevention = 2,

            [Display(Name = "No Hormones")]
            NoHormones = 3,

            [Display(Name = "Regular Bleeding")]
            RegularBleeding = 4,

            Privacy = 5,

            [Display(Name = "Client Controlled")]
            ClientControlled = 6
        }

        public enum FrequencyType : byte
        {
            TimeInterval = 1,

            [Display(Name = "Frequency Interval")]
            FrequencyInterval = 2
        }

        public enum TestResult : byte
        {
            Reactive = 1,

            [Display(Name = "Non Reactive")]
            NonReactive = 2
        }

        /// <summary>
        /// This enum is used to determine the location  of the facility/client.
        /// </summary>
        public enum Location : byte
        {
            Urban = 1,

            Rural = 2,

            [Display(Name = "Peri-Urban")]
            PeriUrban = 3
        }

        /// <summary>
        /// This enum is used to determine the facility type of the client.
        /// </summary>
        public enum FacilityType : byte
        {
            [Display(Name = "1st Level Hospital")]
            FirstLevelHospital = 1,

            [Display(Name = "2nd Level Hospital")]
            SecondLevelHospital = 2,

            [Display(Name = "3rd Level Hospital")]
            ThirdLevelHospital = 3,

            [Display(Name = "Dental Clinic")]
            DentalClinic = 4,

            [Display(Name = "Diagnostic Centre")]
            DiagnosticCentre = 5,

            [Display(Name = "Eye Clinic")]
            EyeClinic = 6,

            [Display(Name = "Fertility Clinic")]
            FertilityClinic = 7,

            [Display(Name = "First-Aid Stations")]
            FirstAidStations = 8,

            [Display(Name = "General Clinic")]
            GeneralClinic = 9,

            [Display(Name = "Health Centre")]
            HealthCentre = 10,

            [Display(Name = "Health Post")]
            HealthPost = 11,

            Hospice = 12,

            [Display(Name = "Mini Hospital")]
            MiniHospital = 13,

            [Display(Name = "Mobile Clinic")]
            MobileClinic = 14,

            [Display(Name = "Optic Clinics")]
            OpticClinics = 15,

            [Display(Name = "Rehabilitation Centre")]
            RehabilitationCentre = 16,

            [Display(Name = "Specimen Collection Centre")]
            SpecimenCollectionCentre = 17,

            Others = 18
        }

        /// <summary>
        /// This enum is used to determine the facility type of the client.
        /// </summary>
        public enum Ownership : byte
        {
            [Display(Name = "Faith Based")]
            FaithBased = 1,

            [Display(Name = "Ministry Of Health")]
            MinistryOfHealth = 2,

            [Display(Name = "Correctional Service")]
            CorrectionalService = 3,

            Private = 4,

            [Display(Name = "Zambia National Service")]
            ZambiaNationalService = 5,

            [Display(Name = "Zambia Defence Force")]
            ZambiaDefenceForce = 6,

            [Display(Name = "Police Service")]
            PoliceService = 7
        }

        /// <summary>
        /// This enum is used to determine the Client Type.
        /// </summary>
        public enum ArmedForcesClientType : byte
        {
            Defense = 1,

            [Display(Name = "No Defense")]
            NoDefense = 2
        }

        /// <summary>
        /// This enum is used to determine the Armed Forcces Service.
        /// </summary>
        public enum ArmedForcesService : byte
        {
            Army = 1,

            ZAF = 2,

            ZNS = 3,

            [Display(Name = "Ministry of Defence")]
            MinistryOfDefence = 4
        }

        /// <summary>
        /// This enum is used to determine the Patient Type of Armed Forces Client.
        /// </summary>
        public enum ArmedForcesPatientType : byte
        {
            [Display(Name = "Army Officer")]
            ArmyOfficer = 1,

            [Display(Name = "Army Soldier")]
            ArmySoldier = 2,

            [Display(Name = "Army Civilian")]
            ArmyCivilian = 3,

            [Display(Name = "Army Civilian Dpdt")]
            ArmyCivilianDpdt = 4,

            [Display(Name = "Army Officer Dpdt")]
            ArmyOfficerDpdt = 5,

            [Display(Name = "Army Soldier Dpdt")]
            ArmySoldierDpdt = 6
        }

        /// <summary>
        /// This enum is used to determine the Rank of Armed Forces Client.
        /// </summary>
        public enum ArmedForcesClientRank : byte
        {
            [Display(Name = "Officer Cadet")]
            Cdt = 1,

            [Display(Name = "Second Lieutenant(2Lt)")]
            TwoLt = 2,

            [Display(Name = "Lieutenant(Lt)")]
            Lt = 3,

            [Display(Name = "Captain(Capt)")]
            Capt = 4,

            [Display(Name = "Major(Maj)")]
            Major = 5,

            [Display(Name = "Lieutenant Colonel(Lt. Col)")]
            LtCol = 6,

            [Display(Name = "Colonel(Col)")]
            Colonel = 7,

            [Display(Name = "Brigadier General(Brig. Gen)")]
            BrigGen = 8,

            [Display(Name = "Major General(Maj. Gen)")]
            MajGen = 9,

            [Display(Name = "Lieutenant General(Lt. Gen)")]
            LtGen = 10,

            [Display(Name = "General(Gen)")]
            General = 11
        }

        /// <summary>
        /// This enum is used to determine the Armed Forcces Service.
        /// </summary>
        public enum PreScreeningVisitType : byte
        {
            Enrollment = 1,

            [Display(Name = "Re-Screening After Previous Negative")]
            ReScreeningAfterPreviousNegative = 2,

            [Display(Name = "Post-Treatment FollowUp")]
            PostTreatmentFollowUp = 3,

        }

        public enum MenstrualBloodFlow : byte
        {
            Heavy = 1,

            Moderate = 2,

            Scanty = 3
        }

        public enum MenstrualCycleRegularity : byte
        {
            Regular = 1,

            Irregular = 2
        }

        public enum PapSmearTestResult : byte
        {
            Normal = 1,

            Abnormal = 2
        }

        public enum PapSmearGrade : byte
        {
            [Display(Name = "Low Grade")]
            LowGrade = 1,

            [Display(Name ="High Grade")]
            HighGrade = 2,

            Other = 3
        }

        public enum HPVTestType : byte
        {
            Self = 1,

            Provider = 2
        }

        public enum HPVTestResult : byte
        {
            Positive = 1,

            Negative = 2,

            [Display(Name ="Not Done")]
            NotDone= 3
        }

        public enum TestResultsType : byte
        {
            Negative = 1,

            [Display(Name ="Result Uncertain")]
            ResultUncertain = 2,

            Positive = 3,

            [Display(Name ="Suspected Cancer")]
            SuspectedCancer = 4
        }

        public enum TransferZoneStateType : byte
        {
            a = 1,

            Tpye1 =2,

            Type2 = 3,

            Type3 = 4
        }
    }
}