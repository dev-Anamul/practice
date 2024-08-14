using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Rezwana
 * Date created : 19.01.2023
 * Modified by  : Bithy
 * Last modified: 22.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IPEPPreventionHistoryRepository interface.
    /// </summary>
    public class PEPPreventionHistoryRepository : Repository<HIVPreventionHistory>, IPEPPreventionHistoryRepository
    {
        public PEPPreventionHistoryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a PEP prevention history by key.
        /// </summary>
        /// <param name="key">Primary key of the table PEPPreventionHistories.</param>
        /// <returns>Returns a PEP prevention history if the key is matched.</returns>
        public async Task<HIVPreventionHistory> GetPEPPreventionHistoryByKey(Guid key)
        {
            try
            {
                var hIVPreventionHistory = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (hIVPreventionHistory != null)
                    hIVPreventionHistory.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return hIVPreventionHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Client.</param>
        /// <returns>Returns a Client if the key is matched.</returns>
        public async Task<IEnumerable<HIVPreventionHistory>> GetPEPPreventionHistoryByClient(Guid ClientID)
        {
            return await context.HIVPreventionHistories.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == ClientID)
           .Join(
               context.Encounters.AsNoTracking(),
               hIVPreventionHistory => hIVPreventionHistory.EncounterId,
               encounter => encounter.Oid,
               (hIVPreventionHistory, encounter) => new HIVPreventionHistory
               {
                   EncounterId = hIVPreventionHistory.EncounterId,
                   EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                   ClientId = hIVPreventionHistory.ClientId,
                   CreatedIn = hIVPreventionHistory.CreatedIn,
                   DateCreated = hIVPreventionHistory.DateCreated,
                   CreatedBy = hIVPreventionHistory.CreatedBy,
                   DateModified = hIVPreventionHistory.DateModified,
                   EncounterType = hIVPreventionHistory.EncounterType,
                   InteractionId = hIVPreventionHistory.InteractionId,
                   IsCircumcised = hIVPreventionHistory.IsCircumcised,
                   IsCondomLubricantUsed = hIVPreventionHistory.IsCondomLubricantUsed,
                   IsDeleted = hIVPreventionHistory.IsDeleted,
                   IsPEPUsedBefore = hIVPreventionHistory.IsPEPUsedBefore,
                   IsPrEPUsedBefore = hIVPreventionHistory.IsPrEPUsedBefore,
                   IsSynced = hIVPreventionHistory.IsSynced,
                   ModifiedBy = hIVPreventionHistory.ModifiedBy,
                   ModifiedIn = hIVPreventionHistory.ModifiedIn

               }).OrderByDescending(x => x.EncounterDate).ToListAsync();
        }
        public async Task<IEnumerable<HIVPreventionHistory>> GetPEPPreventionHistoryByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            var hIVPreventionHistoryAsQuerable = context.HIVPreventionHistories.Include(x => x.Client).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
           .Join(
               context.Encounters.AsNoTracking(),
               hIVPreventionHistory => hIVPreventionHistory.EncounterId,
               encounter => encounter.Oid,
               (hIVPreventionHistory, encounter) => new HIVPreventionHistory
               {
                   EncounterId = hIVPreventionHistory.EncounterId,
                   EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                   ClientId = hIVPreventionHistory.ClientId,
                   Client=hIVPreventionHistory.Client,
                   CreatedIn = hIVPreventionHistory.CreatedIn,
                   DateCreated = hIVPreventionHistory.DateCreated,
                   CreatedBy = hIVPreventionHistory.CreatedBy,
                   DateModified = hIVPreventionHistory.DateModified,
                   EncounterType = hIVPreventionHistory.EncounterType,
                   InteractionId = hIVPreventionHistory.InteractionId,
                   IsCircumcised = hIVPreventionHistory.IsCircumcised,
                   IsCondomLubricantUsed = hIVPreventionHistory.IsCondomLubricantUsed,
                   IsDeleted = hIVPreventionHistory.IsDeleted,
                   IsPEPUsedBefore = hIVPreventionHistory.IsPEPUsedBefore,
                   IsPrEPUsedBefore = hIVPreventionHistory.IsPrEPUsedBefore,
                   IsSynced = hIVPreventionHistory.IsSynced,
                   ModifiedBy = hIVPreventionHistory.ModifiedBy,
                   ModifiedIn = hIVPreventionHistory.ModifiedIn

               }).AsQueryable();

            if (encounterType == null)
                return await hIVPreventionHistoryAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            else
                return await hIVPreventionHistoryAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
       
        }
        public int GetPEPPreventionHistoryByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.HIVPreventionHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.HIVPreventionHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }

        /// <summary>
        /// The method is used to get a Client by EncounterId.
        /// </summary>
        /// <param name="EncounterId">Primary key of the table Encounter.</param>
        /// <returns>Returns a HIVPreventionHistory if the EncounterId is matched.</returns>
        public async Task<IEnumerable<HIVPreventionHistory>> GetPEPPreventionHistoryByEncounterId(Guid EncounterId)
        {
          return await context.HIVPreventionHistories.Include(x=>x.Client).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterId)
           .Join(
               context.Encounters.AsNoTracking(),
               hIVPreventionHistory => hIVPreventionHistory.EncounterId,
               encounter => encounter.Oid,
               (hIVPreventionHistory, encounter) => new HIVPreventionHistory
               {
                   EncounterId = hIVPreventionHistory.EncounterId,
                   EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                   ClientId = hIVPreventionHistory.ClientId,
                   Client = hIVPreventionHistory.Client,
                   CreatedIn = hIVPreventionHistory.CreatedIn,
                   DateCreated = hIVPreventionHistory.DateCreated,
                   CreatedBy = hIVPreventionHistory.CreatedBy,
                   DateModified = hIVPreventionHistory.DateModified,
                   EncounterType = hIVPreventionHistory.EncounterType,
                   InteractionId = hIVPreventionHistory.InteractionId,
                   IsCircumcised = hIVPreventionHistory.IsCircumcised,
                   IsCondomLubricantUsed = hIVPreventionHistory.IsCondomLubricantUsed,
                   IsDeleted = hIVPreventionHistory.IsDeleted,
                   IsPEPUsedBefore = hIVPreventionHistory.IsPEPUsedBefore,
                   IsPrEPUsedBefore = hIVPreventionHistory.IsPrEPUsedBefore,
                   IsSynced = hIVPreventionHistory.IsSynced,
                   ModifiedBy = hIVPreventionHistory.ModifiedBy,
                   ModifiedIn = hIVPreventionHistory.ModifiedIn,
                

               }).OrderByDescending(x=>x.EncounterDate).ToListAsync();

         }

        /// <summary>
        /// The method is used to get the list of PEP prevention histories.
        /// </summary>
        /// <returns>Returns a list of all PEP prevention histories.</returns>
        public async Task<IEnumerable<HIVPreventionHistory>> GetPEPPreventionHistories()
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
    }
}