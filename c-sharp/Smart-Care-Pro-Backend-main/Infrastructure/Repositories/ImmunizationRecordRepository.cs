using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IImmunizationRecordRepository interface.
    /// </summary>
    public class ImmunizationRecordRepository : Repository<ImmunizationRecord>, IImmunizationRecordRepository
    {
        public ImmunizationRecordRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a Client by key.
        /// </summary>
        /// <param name="clientId">Primary key of the table Clients.</param>
        /// <returns>Returns a Client if the key is matched.</returns>
        public async Task<IEnumerable<ImmunizationRecord>> GetImmunizationRecordByClient(Guid clientId)
        {
            try
            {
                return await context.ImmunizationRecords.Include(p => p.VaccineDose).ThenInclude(x => x.Vaccine).ThenInclude(x => x.VaccineType).Include(a => a.AdverseEvents).Include(x => x.Client).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
   .Join(
       context.Encounters.AsNoTracking(),
       immunizationRecord => immunizationRecord.EncounterId,
       encounter => encounter.Oid,
       (immunizationRecord, encounter) => new ImmunizationRecord
       {
           EncounterId = immunizationRecord.EncounterId,
           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
           AdverseEvents = immunizationRecord.AdverseEvents.ToList(),
           BatchNumber = immunizationRecord.BatchNumber,
           ClientId = immunizationRecord.ClientId,
           Client = immunizationRecord.Client,
           CreatedBy = immunizationRecord.CreatedBy,
           CreatedIn = immunizationRecord.CreatedIn,
           DateCreated = immunizationRecord.DateCreated,
           DateGiven = immunizationRecord.DateGiven,
           DateModified = immunizationRecord.DateModified,
           DoseId = immunizationRecord.DoseId,
           VaccineDose = immunizationRecord.VaccineDose,
           InteractionId = immunizationRecord.InteractionId,
           EncounterType = immunizationRecord.EncounterType,
           IsDeleted = immunizationRecord.IsDeleted,
           IsSynced = immunizationRecord.IsSynced,
           ModifiedBy = immunizationRecord.ModifiedBy,
           ModifiedIn = immunizationRecord.ModifiedIn,
           VaccineTypes = immunizationRecord.VaccineTypes,
           ClinicianName = context.UserAccounts.Where(x => x.Oid == immunizationRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
           FacilityName = context.Facilities.Where(x => x.Oid == immunizationRecord.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

       }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<ImmunizationRecord>> GetImmunizationRecordByClientLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.ImmunizationRecords.Include(p => p.VaccineDose).ThenInclude(x => x.Vaccine).ThenInclude(x => x.VaccineType).Include(a => a.AdverseEvents).Include(x => x.Client).Where(p => p.IsDeleted == false && p.ClientId == clientId && p.DateCreated >= Last24Hours).AsNoTracking()
   .Join(
       context.Encounters.AsNoTracking(),
       immunizationRecord => immunizationRecord.EncounterId,
       encounter => encounter.Oid,
       (immunizationRecord, encounter) => new ImmunizationRecord
       {
           EncounterId = immunizationRecord.EncounterId,
           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
           AdverseEvents = immunizationRecord.AdverseEvents.ToList(),
           BatchNumber = immunizationRecord.BatchNumber,
           ClientId = immunizationRecord.ClientId,
           Client = immunizationRecord.Client,
           CreatedBy = immunizationRecord.CreatedBy,
           CreatedIn = immunizationRecord.CreatedIn,
           DateCreated = immunizationRecord.DateCreated,
           DateGiven = immunizationRecord.DateGiven,
           DateModified = immunizationRecord.DateModified,
           DoseId = immunizationRecord.DoseId,
           VaccineDose = immunizationRecord.VaccineDose,
           InteractionId = immunizationRecord.InteractionId,
           EncounterType = immunizationRecord.EncounterType,
           IsDeleted = immunizationRecord.IsDeleted,
           IsSynced = immunizationRecord.IsSynced,
           ModifiedBy = immunizationRecord.ModifiedBy,
           ModifiedIn = immunizationRecord.ModifiedIn,
           VaccineTypes = immunizationRecord.VaccineTypes,
           ClinicianName = context.UserAccounts.Where(x => x.Oid == immunizationRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
           FacilityName = context.Facilities.Where(x => x.Oid == immunizationRecord.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

       }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<ImmunizationRecord>> GetImmunizationRecordByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var immunizationRecordAsQuerable = context.ImmunizationRecords.Include(p => p.VaccineDose).ThenInclude(x => x.Vaccine).ThenInclude(x => x.VaccineType).Include(a => a.AdverseEvents).Include(x => x.Client).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
   .Join(
       context.Encounters.AsNoTracking(),
       immunizationRecord => immunizationRecord.EncounterId,
       encounter => encounter.Oid,
       (immunizationRecord, encounter) => new ImmunizationRecord
       {
           EncounterId = immunizationRecord.EncounterId,
           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
           AdverseEvents = immunizationRecord.AdverseEvents.ToList(),
           BatchNumber = immunizationRecord.BatchNumber,
           ClientId = immunizationRecord.ClientId,
           Client = immunizationRecord.Client,
           CreatedBy = immunizationRecord.CreatedBy,
           CreatedIn = immunizationRecord.CreatedIn,
           DateCreated = immunizationRecord.DateCreated,
           DateGiven = immunizationRecord.DateGiven,
           DateModified = immunizationRecord.DateModified,
           DoseId = immunizationRecord.DoseId,
           VaccineDose = immunizationRecord.VaccineDose,
           InteractionId = immunizationRecord.InteractionId,
           EncounterType = immunizationRecord.EncounterType,
           IsDeleted = immunizationRecord.IsDeleted,
           IsSynced = immunizationRecord.IsSynced,
           ModifiedBy = immunizationRecord.ModifiedBy,
           ModifiedIn = immunizationRecord.ModifiedIn,
           VaccineTypes = immunizationRecord.VaccineTypes,
           ClinicianName = context.UserAccounts.Where(x => x.Oid == immunizationRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
           FacilityName = context.Facilities.Where(x => x.Oid == immunizationRecord.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

       }).AsQueryable();

                if (encounterType == null)
                    return await immunizationRecordAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await immunizationRecordAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetImmunizationRecordByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.ImmunizationRecords.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.ImmunizationRecords.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }

        /// <summary>
        /// The method is used to get a ImmunizationRecord by key.
        /// </summary>
        /// <param name="key">Primary key of the table ImmunizationRecords.</param>
        /// <returns>Returns a ImmunizationRecord if the key is matched.</returns>
        public async Task<ImmunizationRecord> GetImmunizationRecordByKey(Guid key)
        {
            try
            {
                var immunizationRecord = await context.ImmunizationRecords.Where(p => p.InteractionId == key && p.IsDeleted == false).Include(x => x.VaccineDose).Include(x => x.VaccineDose.Vaccine).Include(x => x.VaccineDose.Vaccine.VaccineType).FirstOrDefaultAsync();

                if (immunizationRecord != null)
                {
                    immunizationRecord.ClinicianName = await context.UserAccounts.Where(x => x.Oid == immunizationRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    immunizationRecord.FacilityName = await context.Facilities.Where(x => x.Oid == immunizationRecord.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    immunizationRecord.EncounterDate = await context.Encounters.Where(x => x.Oid == immunizationRecord.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                }

                return immunizationRecord;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ImmunizationRecords.
        /// </summary>
        /// <returns>Returns a list of all ImmunizationRecords.</returns>
        public async Task<IEnumerable<ImmunizationRecord>> GetImmunizationRecords()
        {
            try
            {
                return await LoadListWithChildAsync<ImmunizationRecord>(u => u.IsDeleted == false, vd => vd.VaccineDose, v => v.VaccineDose.Vaccine, vt => vt.VaccineDose.Vaccine.VaccineType, c => c.Client);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ImmunizationRecords.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a list of all ImmunizationRecords.</returns>
        public async Task<IEnumerable<ImmunizationRecord>> GetImmunizationRecordByEncounter(Guid encounterId)
        {
            try
            {
                return await context.ImmunizationRecords.Include(p => p.VaccineDose).ThenInclude(x => x.Vaccine).ThenInclude(x => x.VaccineType).Include(a => a.AdverseEvents).Include(x => x.Client).Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
   .Join(
       context.Encounters.AsNoTracking(),
       immunizationRecord => immunizationRecord.EncounterId,
       encounter => encounter.Oid,
       (immunizationRecord, encounter) => new ImmunizationRecord
       {
           EncounterId = immunizationRecord.EncounterId,
           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
           AdverseEvents = immunizationRecord.AdverseEvents.ToList(),
           BatchNumber = immunizationRecord.BatchNumber,
           ClientId = immunizationRecord.ClientId,
           Client = immunizationRecord.Client,
           CreatedBy = immunizationRecord.CreatedBy,
           CreatedIn = immunizationRecord.CreatedIn,
           DateCreated = immunizationRecord.DateCreated,
           DateGiven = immunizationRecord.DateGiven,
           DateModified = immunizationRecord.DateModified,
           DoseId = immunizationRecord.DoseId,
           VaccineDose = immunizationRecord.VaccineDose,
           InteractionId = immunizationRecord.InteractionId,
           EncounterType = immunizationRecord.EncounterType,
           IsDeleted = immunizationRecord.IsDeleted,
           IsSynced = immunizationRecord.IsSynced,
           ModifiedBy = immunizationRecord.ModifiedBy,
           ModifiedIn = immunizationRecord.ModifiedIn,
           VaccineTypes = immunizationRecord.VaccineTypes,
           ClinicianName = context.UserAccounts.Where(x => x.Oid == immunizationRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
           FacilityName = context.Facilities.Where(x => x.Oid == immunizationRecord.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

       }).OrderByDescending(x => x.EncounterDate).ToListAsync();

                //return await QueryAsync(c => c.IsDeleted == false && c.EncounterId == encounterId);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}