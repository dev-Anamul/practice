using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Bithy
 * Date created : 07-02-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IPainRecord Repository interface.
    /// </summary>
    public class PainRecordRepository : Repository<PainRecord>, IPainRecordRepository
    {
        public PainRecordRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a PainRecord by key.
        /// </summary>
        /// <param name="key">Primary key of the table PainRecords.</param>
        /// <returns>Returns a PainRecord if the key is matched.</returns>
        public async Task<PainRecord> GetPainRecordByKey(Guid key)
        {
            try
            {
                var painRecord = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (painRecord != null)
                    painRecord.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return painRecord;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PainRecords.
        /// </summary>
        /// <returns>Returns a list of all PainRecords.</returns>
        public async Task<IEnumerable<PainRecord>> GetPainRecords()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a PainRecord by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a PainRecord if the ClientID is matched.</returns>
        public async Task<IEnumerable<PainRecord>> GetPainRecordByClient(Guid ClientID)
        {
            try
            {
                return await context.PainRecords.Include(p => p.PainScale).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == ClientID)
       .Join(
           context.Encounters.AsNoTracking(),
           painRecord => painRecord.EncounterId,
           encounter => encounter.Oid,
           (painRecord, encounter) => new PainRecord
           {
               EncounterId = painRecord.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               InteractionId = painRecord.InteractionId,
               EncounterType = painRecord.EncounterType,
               DateCreated = painRecord.DateCreated,
               DateModified = painRecord.DateModified,
               CreatedIn = painRecord.CreatedIn,
               CreatedBy = painRecord.CreatedBy,
               ClientId = painRecord.ClientId,
               IsDeleted = painRecord.IsDeleted,
               IsSynced = painRecord.IsSynced,
               ModifiedBy = painRecord.ModifiedBy,
               ModifiedIn = painRecord.ModifiedIn,
               PainScale = painRecord.PainScale,
               PainScaleId = painRecord.PainScaleId,
               PainScales = painRecord.PainScales,

           }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get a PainRecord by EncounterID.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a PainRecord if the EncounterID is matched.</returns>
        public async Task<IEnumerable<PainRecord>> GetPainRecordByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.PainRecords.Include(p => p.PainScale).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
       .Join(
           context.Encounters.AsNoTracking(),
           painRecord => painRecord.EncounterId,
           encounter => encounter.Oid,
           (painRecord, encounter) => new PainRecord
           {
               EncounterId = painRecord.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               InteractionId = painRecord.InteractionId,
               EncounterType = painRecord.EncounterType,
               DateCreated = painRecord.DateCreated,
               DateModified = painRecord.DateModified,
               CreatedIn = painRecord.CreatedIn,
               CreatedBy = painRecord.CreatedBy,
               ClientId = painRecord.ClientId,
               IsDeleted = painRecord.IsDeleted,
               IsSynced = painRecord.IsSynced,
               ModifiedBy = painRecord.ModifiedBy,
               ModifiedIn = painRecord.ModifiedIn,
               PainScale = painRecord.PainScale,
               PainScaleId = painRecord.PainScaleId,
               PainScales = painRecord.PainScales,

           }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a OPDVisit by key.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisit.</param>
        /// <returns>Returns a OPDVisit if the key is matched.</returns>
        public async Task<IEnumerable<PainRecord>> GetPainRecordByOpdVisit(Guid OPDVisitID)
        {
            try
            {
                return await context.PainRecords.Include(p => p.PainScale).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == OPDVisitID)
       .Join(
           context.Encounters.AsNoTracking(),
           painRecord => painRecord.EncounterId,
           encounter => encounter.Oid,
           (painRecord, encounter) => new PainRecord
           {
               EncounterId = painRecord.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               InteractionId = painRecord.InteractionId,
               EncounterType = painRecord.EncounterType,
               DateCreated = painRecord.DateCreated,
               DateModified = painRecord.DateModified,
               CreatedIn = painRecord.CreatedIn,
               CreatedBy = painRecord.CreatedBy,
               ClientId = painRecord.ClientId,
               IsDeleted = painRecord.IsDeleted,
               IsSynced = painRecord.IsSynced,
               ModifiedBy = painRecord.ModifiedBy,
               ModifiedIn = painRecord.ModifiedIn,
               PainScale = painRecord.PainScale,
               PainScaleId = painRecord.PainScaleId,
               PainScales = painRecord.PainScales,

           }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}