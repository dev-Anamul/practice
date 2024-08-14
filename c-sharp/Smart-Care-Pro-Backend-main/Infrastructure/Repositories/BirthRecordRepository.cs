using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Domain.Dto;

/*
 * Created by    : Stephan
 * Date created  : 29.01.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IBirthRecordRepository interface.
    /// </summary>
    public class BirthRecordRepository : Repository<BirthRecord>, IBirthRecordRepository
    {
        public BirthRecordRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthRecords.</param>
        /// <returns>Returns a birth record if the key is matched.</returns>
        public async Task<BirthRecord> GetBirthRecordByKey(Guid key)
        {
            try
            {
                BirthRecord birthRecord = await context.BirthRecords.AsNoTracking().FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);
               
                if (birthRecord != null)
                {
                    birthRecord.EncounterDate = await context.Encounters.Where(x => x.Oid == birthRecord.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                    birthRecord.ClinicianName = await context.UserAccounts.Where(x => x.Oid == birthRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    birthRecord.FacilityName = await context.Facilities.Where(x => x.Oid == birthRecord.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                }

                return birthRecord;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public async Task<IEnumerable<BirthRecord>> GetBirthRecords()
        {
            try
            {
                return await context.BirthRecords.AsNoTracking().Where(x => x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          birthRecord => birthRecord.EncounterId,
                          encounter => encounter.Oid,
                          (birthRecord, encounter) => new BirthRecord
                          {
                              InteractionId = birthRecord.InteractionId,
                              IsBirthRecordGiven = birthRecord.IsBirthRecordGiven,
                              IsUnderFiveCardGiven = birthRecord.IsUnderFiveCardGiven,
                              UnderFiveCardNumber = birthRecord.UnderFiveCardNumber,
                              Origin = birthRecord.Origin,
                              InformantFirstName = birthRecord.InformantFirstName,
                              InformantSurname = birthRecord.InformantSurname,
                              InformantNickname = birthRecord.InformantNickname,
                              InformantRelationship = birthRecord.InformantRelationship,
                              InformantOtherRelationship = birthRecord.InformantOtherRelationship,
                              InformantCity = birthRecord.InformantCity,
                              InformantStreetNo = birthRecord.InformantStreetNo,
                              InformantPOBox = birthRecord.InformantPOBox,
                              InformantLandmark = birthRecord.InformantLandmark,
                              InformantLandlineCountryCode = birthRecord.InformantLandlineCountryCode,
                              InformantLandline = birthRecord.InformantLandline,
                              InformantCellphoneCountryCode = birthRecord.InformantCellphoneCountryCode,
                              InformantCellphone = birthRecord.InformantCellphone,

                              // Include other properties as needed

                              EncounterId = birthRecord.EncounterId,
                              EncounterType = birthRecord.EncounterType,
                              CreatedIn = birthRecord.CreatedIn,
                              DateCreated = birthRecord.DateCreated,
                              CreatedBy = birthRecord.CreatedBy,
                              ModifiedIn = birthRecord.ModifiedIn,
                              DateModified = birthRecord.DateModified,
                              ModifiedBy = birthRecord.ModifiedBy,
                              IsDeleted = birthRecord.IsDeleted,
                              IsSynced = birthRecord.IsSynced,
                              ClientId = birthRecord.ClientId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                           ClinicianName = context.UserAccounts.Where(x => x.Oid == birthRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == birthRecord.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                          }
                      )

                      .OrderByDescending(b => b.EncounterDate)
                      .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a birth record if the ClientID is matched.</returns>
        public async Task<IEnumerable<BirthRecord>> GetBirthRecordByClient(Guid clientId)
        {
            try
            {
                return await context.BirthRecords.AsNoTracking().Where(x => x.ClientId == clientId && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          birthRecord => birthRecord.EncounterId,
                          encounter => encounter.Oid,
                          (birthRecord, encounter) => new BirthRecord
                          {
                              InteractionId = birthRecord.InteractionId,
                              IsBirthRecordGiven = birthRecord.IsBirthRecordGiven,
                              IsUnderFiveCardGiven = birthRecord.IsUnderFiveCardGiven,
                              UnderFiveCardNumber = birthRecord.UnderFiveCardNumber,
                              Origin = birthRecord.Origin,
                              InformantFirstName = birthRecord.InformantFirstName,
                              InformantSurname = birthRecord.InformantSurname,
                              InformantNickname = birthRecord.InformantNickname,
                              InformantRelationship = birthRecord.InformantRelationship,
                              InformantOtherRelationship = birthRecord.InformantOtherRelationship,
                              InformantCity = birthRecord.InformantCity,
                              InformantStreetNo = birthRecord.InformantStreetNo,
                              InformantPOBox = birthRecord.InformantPOBox,
                              InformantLandmark = birthRecord.InformantLandmark,
                              InformantLandlineCountryCode = birthRecord.InformantLandlineCountryCode,
                              InformantLandline = birthRecord.InformantLandline,
                              InformantCellphoneCountryCode = birthRecord.InformantCellphoneCountryCode,
                              InformantCellphone = birthRecord.InformantCellphone,

                              // Include other properties as needed

                              EncounterId = birthRecord.EncounterId,
                              EncounterType = birthRecord.EncounterType,
                              CreatedIn = birthRecord.CreatedIn,
                              DateCreated = birthRecord.DateCreated,
                              CreatedBy = birthRecord.CreatedBy,
                              ModifiedIn = birthRecord.ModifiedIn,
                              DateModified = birthRecord.DateModified,
                              ModifiedBy = birthRecord.ModifiedBy,
                              IsDeleted = birthRecord.IsDeleted,
                              IsSynced = birthRecord.IsSynced,
                              ClientId = birthRecord.ClientId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                             ClinicianName = context.UserAccounts.Where(x => x.Oid == birthRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == birthRecord.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                          }
                      )
                      .OrderByDescending(b => b.EncounterDate)
                      .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get a birth record by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a birth record if the OPD EncounterID is matched.</returns>
        public async Task<IEnumerable<BirthRecord>> GetBirthRecordByEncounter(Guid encounterId)
        {
            try
            {
                return await context.BirthRecords.AsNoTracking().Where(x => x.EncounterId == encounterId && x.IsDeleted == false)
                    .Join(
                          context.Encounters.AsNoTracking(),
                          birthRecord => birthRecord.EncounterId,
                          encounter => encounter.Oid,
                          (birthRecord, encounter) => new BirthRecord
                          {
                              InteractionId = birthRecord.InteractionId,
                              IsBirthRecordGiven = birthRecord.IsBirthRecordGiven,
                              IsUnderFiveCardGiven = birthRecord.IsUnderFiveCardGiven,
                              UnderFiveCardNumber = birthRecord.UnderFiveCardNumber,
                              Origin = birthRecord.Origin,
                              InformantFirstName = birthRecord.InformantFirstName,
                              InformantSurname = birthRecord.InformantSurname,
                              InformantNickname = birthRecord.InformantNickname,
                              InformantRelationship = birthRecord.InformantRelationship,
                              InformantOtherRelationship = birthRecord.InformantOtherRelationship,
                              InformantCity = birthRecord.InformantCity,
                              InformantStreetNo = birthRecord.InformantStreetNo,
                              InformantPOBox = birthRecord.InformantPOBox,
                              InformantLandmark = birthRecord.InformantLandmark,
                              InformantLandlineCountryCode = birthRecord.InformantLandlineCountryCode,
                              InformantLandline = birthRecord.InformantLandline,
                              InformantCellphoneCountryCode = birthRecord.InformantCellphoneCountryCode,
                              InformantCellphone = birthRecord.InformantCellphone,

                              // Include other properties as needed

                              EncounterId = birthRecord.EncounterId,
                              EncounterType = birthRecord.EncounterType,
                              CreatedIn = birthRecord.CreatedIn,
                              DateCreated = birthRecord.DateCreated,
                              CreatedBy = birthRecord.CreatedBy,
                              ModifiedIn = birthRecord.ModifiedIn,
                              DateModified = birthRecord.DateModified,
                              ModifiedBy = birthRecord.ModifiedBy,
                              IsDeleted = birthRecord.IsDeleted,
                              IsSynced = birthRecord.IsSynced,
                              ClientId = birthRecord.ClientId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                             ClinicianName = context.UserAccounts.Where(x => x.Oid == birthRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == birthRecord.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                          }
                      )

                      .OrderByDescending(b => b.EncounterDate)
                      .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}