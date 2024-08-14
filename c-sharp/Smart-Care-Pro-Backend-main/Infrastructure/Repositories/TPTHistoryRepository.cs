using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Tariqul Islam
 * Date created : 05.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class TPTHistoryRepository : Repository<TPTHistory>, ITPTHistoryRepository
    {
        /// <summary>
        /// Implementation of ITBHistoryRepository interface.
        /// </summary>
        public TPTHistoryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a TPTHistory by key.
        /// </summary>
        /// <param name="key">Primary key of the table TPTHistory.</param>
        /// <returns>Returns a TPTHistory if the key is matched.</returns>
        public async Task<TPTHistory> GetTPTHistoryByKey(Guid key)
        {
            try
            {
                var tPTHistory = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (tPTHistory != null)
                    tPTHistory.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return tPTHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of TPTHistory.
        /// </summary>
        /// <returns>Returns a list of all TPTHistory.</returns>
        public async Task<IEnumerable<TPTHistory>> GetTPTHistories()
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
        /// The method is used to get a TPTHistory by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a TPTHistory if the ClientID is matched.</returns>
        public async Task<IEnumerable<TPTHistory>> GetTPTHistoryByClient(Guid clientId)
        {
            try
            {
                return await context.TPTHistories.Include(d => d.TakenTPTDrugs).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
    .Join(
        context.Encounters.AsNoTracking(),
        tPTHistory => tPTHistory.EncounterId,
        encounter => encounter.Oid,
        (tPTHistory, encounter) => new TPTHistory
        {
            EncounterId = tPTHistory.EncounterId,
            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
            ClientId = tPTHistory.ClientId,
            CreatedBy = tPTHistory.CreatedBy,
            CreatedIn = tPTHistory.CreatedIn,
            DateCreated = tPTHistory.DateCreated,
            DateModified = tPTHistory.DateModified,
            EncounterType = tPTHistory.EncounterType,
            InteractionId = tPTHistory.InteractionId,
            IsDeleted = tPTHistory.IsDeleted,
            IsOnTPT = tPTHistory.IsOnTPT,
            IsPatientEligible = tPTHistory.IsPatientEligible,
            IsSynced = tPTHistory.IsSynced,
            IsTakenTPTThreeYears = tPTHistory.IsTakenTPTThreeYears,
            ModifiedBy = tPTHistory.ModifiedBy,
            ModifiedIn = tPTHistory.ModifiedIn,
            ReasonNotStarted = tPTHistory.ReasonNotStarted,
            TPTEndDate = tPTHistory.TPTEndDate,
            TPTStartDate = tPTHistory.TPTStartDate,
            TakenTPTDrugs = tPTHistory.TakenTPTDrugs.ToList()

        }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<TPTHistory>> GetTPTHistoryByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var tPTHistoryAsQuerable = context.TPTHistories.Include(d => d.TakenTPTDrugs).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
  .Join(
      context.Encounters.AsNoTracking(),
      tPTHistory => tPTHistory.EncounterId,
      encounter => encounter.Oid,
      (tPTHistory, encounter) => new TPTHistory
      {
          EncounterId = tPTHistory.EncounterId,
          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
          ClientId = tPTHistory.ClientId,
          CreatedBy = tPTHistory.CreatedBy,
          CreatedIn = tPTHistory.CreatedIn,
          DateCreated = tPTHistory.DateCreated,
          DateModified = tPTHistory.DateModified,
          EncounterType = tPTHistory.EncounterType,
          InteractionId = tPTHistory.InteractionId,
          IsDeleted = tPTHistory.IsDeleted,
          IsOnTPT = tPTHistory.IsOnTPT,
          IsPatientEligible = tPTHistory.IsPatientEligible,
          IsSynced = tPTHistory.IsSynced,
          IsTakenTPTThreeYears = tPTHistory.IsTakenTPTThreeYears,
          ModifiedBy = tPTHistory.ModifiedBy,
          ModifiedIn = tPTHistory.ModifiedIn,
          ReasonNotStarted = tPTHistory.ReasonNotStarted,
          TPTEndDate = tPTHistory.TPTEndDate,
          TPTStartDate = tPTHistory.TPTStartDate,
          TakenTPTDrugs = tPTHistory.TakenTPTDrugs.ToList()

      }).AsQueryable();

                if (encounterType == null)
                    return await tPTHistoryAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await tPTHistoryAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetTPTHistoryByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.TPTHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.TPTHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get the list of TPTHistory by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all TPTHistory by EncounterID.</returns>
        public async Task<IEnumerable<TPTHistory>> GetTPTHistoryByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.TPTHistories.Include(d => d.TakenTPTDrugs).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
    .Join(
        context.Encounters.AsNoTracking(),
        tPTHistory => tPTHistory.EncounterId,
        encounter => encounter.Oid,
        (tPTHistory, encounter) => new TPTHistory
        {
            EncounterId = tPTHistory.EncounterId,
            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
            ClientId = tPTHistory.ClientId,
            CreatedBy = tPTHistory.CreatedBy,
            CreatedIn = tPTHistory.CreatedIn,
            DateCreated = tPTHistory.DateCreated,
            DateModified = tPTHistory.DateModified,
            EncounterType = tPTHistory.EncounterType,
            InteractionId = tPTHistory.InteractionId,
            IsDeleted = tPTHistory.IsDeleted,
            IsOnTPT = tPTHistory.IsOnTPT,
            IsPatientEligible = tPTHistory.IsPatientEligible,
            IsSynced = tPTHistory.IsSynced,
            IsTakenTPTThreeYears = tPTHistory.IsTakenTPTThreeYears,
            ModifiedBy = tPTHistory.ModifiedBy,
            ModifiedIn = tPTHistory.ModifiedIn,
            ReasonNotStarted = tPTHistory.ReasonNotStarted,
            TPTEndDate = tPTHistory.TPTEndDate,
            TPTStartDate = tPTHistory.TPTStartDate,
            TakenTPTDrugs = tPTHistory.TakenTPTDrugs.ToList()

        }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
