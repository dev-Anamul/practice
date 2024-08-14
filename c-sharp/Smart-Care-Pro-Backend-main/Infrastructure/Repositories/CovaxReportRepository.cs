using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Shakil
 * Date created : 16.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class CovaxReportRepository : Repository<CovaxRecord>, ICovaxRecordRepository
    {
        public CovaxReportRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get covax report by key.
        /// </summary>
        /// <param name="key">Primary key of the table CovaxRecordes.</param>
        /// <returns>Returns a covax report if the key is matched.</returns>
        public async Task<CovaxRecord> GetCovaxRecordByKey(Guid key)
        {
            try
            {
                var covaxRecord = await context.CovaxRecords.AsNoTracking().FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);

                if (covaxRecord != null)
                {
                    covaxRecord.ClinicianName = await context.UserAccounts.Where(x => x.Oid == covaxRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    covaxRecord.FacilityName = await context.Facilities.Where(x => x.Oid == covaxRecord.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    covaxRecord.EncounterDate = await context.Encounters.Where(x => x.Oid == covaxRecord.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return covaxRecord;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of covax reports.
        /// </summary>
        /// <returns>Returns a list of all covax reports.</returns>
        public async Task<IEnumerable<CovaxRecord>> GetCovaxRecordes()
        {
            try
            {
                return await context.CovaxRecords.AsNoTracking().Where(x => x.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        covaxRecord => covaxRecord.EncounterId,
                        encounter => encounter.Oid,
                        (covaxRecord, encounter) => new CovaxRecord
                        {
                            InteractionId = covaxRecord.InteractionId,
                            CovaxId = covaxRecord.CovaxId,
                            Covax = covaxRecord.Covax,
                            ImmunizationId = covaxRecord.ImmunizationId,
                            ImmunizationRecord = covaxRecord.ImmunizationRecord,

                            // Properties from EncounterBaseModel
                            EncounterId = covaxRecord.EncounterId,
                            EncounterType = covaxRecord.EncounterType,
                            CreatedIn = covaxRecord.CreatedIn,
                            DateCreated = covaxRecord.DateCreated,
                            CreatedBy = covaxRecord.CreatedBy,
                            ModifiedIn = covaxRecord.ModifiedIn,
                            DateModified = covaxRecord.DateModified,
                            ModifiedBy = covaxRecord.ModifiedBy,
                            IsDeleted = covaxRecord.IsDeleted,
                            IsSynced = covaxRecord.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == covaxRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == covaxRecord.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                        }
                    )
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get the list of covax report.
        /// </summary>
        /// <returns>Returns a list of all covax reports.</returns>
        public async Task<IEnumerable<CovaxRecord>> GetCovaxRecordByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.CovaxRecords.AsNoTracking().Where(x => x.IsDeleted == false && x.EncounterId == EncounterID)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        covaxRecord => covaxRecord.EncounterId,
                        encounter => encounter.Oid,
                        (covaxRecord, encounter) => new CovaxRecord
                        {
                            InteractionId = covaxRecord.InteractionId,
                            CovaxId = covaxRecord.CovaxId,
                            Covax = covaxRecord.Covax,
                            ImmunizationId = covaxRecord.ImmunizationId,
                            ImmunizationRecord = covaxRecord.ImmunizationRecord,

                            // Properties from EncounterBaseModel
                            EncounterId = covaxRecord.EncounterId,
                            EncounterType = covaxRecord.EncounterType,
                            CreatedIn = covaxRecord.CreatedIn,
                            DateCreated = covaxRecord.DateCreated,
                            CreatedBy = covaxRecord.CreatedBy,
                            ModifiedIn = covaxRecord.ModifiedIn,
                            DateModified = covaxRecord.DateModified,
                            ModifiedBy = covaxRecord.ModifiedBy,
                            IsDeleted = covaxRecord.IsDeleted,
                            IsSynced = covaxRecord.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == covaxRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == covaxRecord.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                        }
                    )
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}