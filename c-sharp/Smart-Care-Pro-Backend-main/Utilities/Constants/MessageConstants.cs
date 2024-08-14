/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Bella
 * Last modified: 01.01.2023
 * Reviewed by  : 
 * Reviewed Date:
 */
namespace Utilities.Constants
{
    /// <summary>
    /// Error message constants.
    /// </summary>
    public static class MessageConstants
    {
        //COMMON:
        public const string GenericError = "Something went wrong! Please try after sometime. If you are experiencing similar problem frequently, please report it to helpdesk.";

        public const string InvalidDOBError = "Date of birth cannot be a future date!";

        public const string DuplicateCellphoneError = "The cellphone number is associated with another user account!";

        public const string DuplicateClientNRCError = "The NRC number is associated with another client!";

        public const string DuplicateError = "Duplicate data found!";

        public const string DuplicateHospitalNoError = "The Hospital No is associated with another client!";

        public const string DuplicateServiceNoError = "The Service No is associated with another client!";

        public const string DuplicateManNumberError = "The Man Number is associated with another client!";

        public const string NoMatchFoundError = "No match found!";

        public const string NoPEPMatchFoundError = "No PEP match found!";

        public const string InvalidParameterError = "Invalid parameter(s)!";

        public const string IfNotAlphabet = "Alphabet only!";

        public const string IfNotInteger = "Numbers only!";

        public const string IfNotDecimal = "Decimal only!";

        public const string IfNotSelected = "Please select an option!";

        public const string IfNotEmailAddress = "Email address is not in correct formation!";

        public const string IfNotCountryCode = "Invalid country code!";

        public const string IfFutureDate = "This date should not be a future date!";

        public const string IfPastDate = "This date should not be a past date!";

        public const string IfOldDate = "This date should not be an old date!";

        public const string RequiredFieldError = "Required.";

        public const string CellPhoneValidation = "Phone number must needs to be 11 digits!";

        public const string RequiredFieldEmpty = "One or more required fields are blank! Review the form below to submit.";

        public const string InvalidRequest = "Invalid request! System cannot find anything to process.";

        public const string RecordExists = "Record already exists!";

        public const string RecordSavedSuccessfully = "Record saved successfully!";

        public const string RecordUpdatedSuccessfully = "Record updated successfully!";

        public const string RecordDeletedSuccessfully = "Record deleted successfully!";

        public const string FailedNUPNCreate = "Unsuccessfull NUPN Generate!";

        public const string RecoveryRequestDuplicate = "Already received your recovery request !";

        public const string NothingToSave = "No information has been supplied! There is nothing to save.";

        public const string DateOutOfrange = "Date out of range! Selection must be within 31 days.";

        public const string InvalidDateRange = "Invalid date range!";

        public const string SomethingWentWrong = "Something went wrong! Please review the form below.";

        public const string UnauthorizedAttemptOfRecordUpdateError = "Unauthorized attempt of updating record!";

        public const string UnAuthorizedFacility = "You are not authorized to login with this facility";

        public const string AlreadyReceivedFacility = "We already received your request for this facility";

        public const string AlreadyExistFacility = "You are currently an active user of this facility.";

        public const string NRC = "Invalid NRC!";

        public const string Country = "Country is required!";

        public const string CountryCode = "Numbers only!";

        public const string CountryCodeError = "The country code does not match!";

        public const string CellPhone = "Numbers only!";

        public const string DuplicateFound = "Duplicate Found!";

        public const string MadeFacilityAdministratorSuccessfully = "Facility Administrator has been made successfully!";

        public const string FacilityAdministratorTaken = "Facility already has a Facility Administrator!";

        //public const string CellPhoneOnlyElevenDigit = "Not more or less 11 digits..";

        //USER PROFILE REGISTRATION:
        public const string UsernameTaken = "Username already exists!";

        public const string PasswordLengthError = "Password should be at least 6 characters!";

        public const string UnderAgedUserError = "User must be equal or above 18 years old!";

        public const string DuplicateUserNRCError = "The NRC number is associated with another user account!";

        public const string ConfirmPasswordNotMatchedError = "Passwords do not match!";

        public const string DuplicateUserAccountError = "The email address is associated with another user account!";

        public const string UserRegisteredSuccessfully = "Your Smart Care Evo User Account Created Successfully.";

        public const string UserPasswordUpdatedSuccessfully = "User password updated successfully!";

        public const string UserRecordUpdatedSuccessfully = "User profile updated successfully!";

        public const string FacilityAccessRequestSuccessfully = "\r\nYour  Login Request to the Livingstone General Hospital has been\r\n\r\nSubmitted Successfully!\r\n\r\n\r\n\r\nYou will be able to Login to this Facility after Facility Administrator Approve your Request.";

        public const string PasswordMatchError = "The Passwords do not match!";

        public const string OldPasswordMatchError = "The Old Password does not match!";

        public const string OldAndNewPasswordMatchError = "The New Password should not match with current password!";

        public const string WrongPasswordError = "Wrong password!";

        public const string WrongCellphoneInputError = "The Cellphone number or the User Name doesn't match!";

        public const string WrongUsernameInputError = "The Username is wrong!";

        public const string WrongUsernameAndCellphoneInputError = "Please provide either your Username or your Cellphone!";

        public const string UserNotFound = "User not found";

        public const string CellPhoneAndCountryCodeRequired = "Please Provide both Country Code and Cell phone number";

        //USER LOGIN :
        public const string AccountInactive = "User account is inactive! Contact with administrator to activate the account.";

        public const string InvalidLogin = "Invalid username or password!";

        public const string RecoveryRequestExists = "You cannot Login until your Recovery Request Processed by Admin";

        public const string IncorrectOldPassword = "Current password do not match!";

        public const string InvalidUserType = "Access denied!";

        public const string InvalidFacility = "Invalid facility!";

        public const string InvalidFacilityLogin = "Please Contact to Administrator. You don't have any Facility";

        //CLIENT PROFILE:
        public const string NUPN = "Invalid NUPN!";

        public const string HomeLanguage = "Required!";

        public const string Occupation = "Occupation is required!";

        public const string EducationLevel = "Education level is required!";

        public const string Town = "Town is required!";

        public const string MaritalStatusRequired = "Marital status is required!";

        public const string RelationshipRequired = "Relationship is required!";

        public const string ClientRegisteredSuccessfully = "Client Registered Successfully!";

        public const string ClientUpdatedSucessfulle = "Client data updated successfully!";

        public const string ClientAlreadyExist = "Client already registered!";

        public const string ClientFirstName = "First name is required";

        public const string ClientSurname = "Surname is required!";

        public const string ClientSex = "Sex is not selected!";

        public const string ClientNRC = "NRC is required!";

        public const string ClientArea = "Area is required!";

        public const string PasswordChangedSuccessfully = "Password Changed Successfully.";

        public const string OtherCellphoneRequired = "Other cellphone number is required!";

        public const string CellphoneRequired = "Cellphone number is required!";

        public const string OtherCellphoneCountryCodeRequired = "Other cellphone country code is required!";

        public const string SpouseName = "Spouse name is Required!";

        public const string DuplicateCountry = "Duplicate country found!";

        public const string DuplicateDistrict = "Duplicate district found!";

        public const string DuplicateEducationLevel = "Duplicate education level found!";

        public const string DuplicateFacility = "Duplicate facility found!";

        public const string DuplicateTown = "Duplicate town found!";

        public const string DuplicateProvince = "Duplicate province found!";

        public const string CannotDeleteDistrict = "Duplicate province found!";

        public const string RecordSaved = "Record saved successfully!";

        public const string DistrictOfBirth = "District of birth is required!";

        // HTS
        public const string HTSCreatedSuccessfully = "HTS Created Successfully.";

        public const string ServicePointCreatedSuccessfully = "Service Point Created Successfully.";

        public const string ServiceQueueDuplicate = "Patient is already in this service queue.";

        public const string RequiredFacilityServicePoint = "Facility service point is required!";

        public const string DiagnosisCreatedSuccessfully = "Diagnosis Saved Successfully.";

        public const string DiagnosisUpdatedSuccessfully = "Diagnosis Updated Successfully.";

        public const string DiagnosisTypeValidatorMessage = "Please select a Diagnosis Type.";

        // Admission
        public const string DepartmentCreatedSuccessfully = "Department Saved Successfully.";

        public const string DepartmentUpdatedSuccessfully = "Department Updated Successfully.";

        public const string FirmCreatedSuccessfully = "Firm Saved Successfully.";

        public const string AdmissionCreatedSuccessfully = "Admission Saved Successfully.";

        public const string UpdateAdmissionSuccessfully = "Admission Updated Successfully.";

        public const string DischargeCreatedSuccessfully = "Discharge Saved Successfully.";

        public const string DischargeDate = "Discharge date cannot be past date then Admission date.";

        public const string BedCreatedSuccessfully = "Bed Saved Successfully.";

        public const string BedAlreadyTaken = "Bed already taken please.";

        public const string WardCreatedSuccessfully = "Ward Saved Successfully.";

        public const string FirmUpdatedSuccessfully = "Firm Updated Successfully.";

        // Surgery
        public const string SurgeryCreatedSuccessfully = "Record Saved Successfully.";

        public const string SurgeryUpdatedSuccessfully = "Record Updated Successfully.";

        public const string SurgeryOperationDateError = "The Operation Date should not be a past date.";

        public const string SurgeryOperationEndTimeError = "The Operation End Time should not be a past time.";

        public const string SurgeryOperationDateAndBookingDateError = "The Operation date should not be a past date from the booking date.";

        // Pharmacy
        public const string DrugClasses = "Drug Class is required!";

        public const string DrugSubClasses = "Drug SubClass is required!";

        public const string DrugFormulations = "Drug Formulation is required!";

        public const string DrugRegimens = "Drug Regimen is required!";

        public const string PhysicalSystem = "Physical System is required!";

        public const string DrugDefinitions = "Drug Definition is required!";

        public const string DrugDosageUnites = "Drug Dosage Unite is required!";

        public const string DrugUtilities = "Drug Utilities is required!";

        public const string GenericDrugs = "Generic Drugs is required!";

        public const string DrugRoute = "Drug Route is required!";

        public const string GeneralMedication = "General Medication is required!";

        public const string FrequencyInterval = "Frequency Interval is required!";

        public const string PrescriptionCreatedSuccessfully = "Prescription Saved Successfully.";

        public const string GeneralMedicationUpdatedSuccessfully = "General Medication Updated Successfully.";

        //

        public const string TypeOfEntry = "Type Of Facility Entry is not selected!";

        public const string AddMonth = "One month is added";

        public const string InvestigationResults = "Investigation Results Successfully Added";

        public const string AddCondtions = "Please add minimum one record";

        public const string PreTransfusionVital = "Please add Pre Transfusion Vital before Intra Transfusion Vital";

        public const string PreTransfusionVitalDateValidation = "The Intra-transfusion vitals date should not be a past date from the Pre-transfusion date.";

        public const string MaxCellPhoneValidation = "Cell Phone number must needs to be 11 digits";

        public const string MinCellPhoneValidation = "Cell Phone number must needs to be 9 digits";

        public const string UpdateMessage = "Successfully Updated";
    }
}