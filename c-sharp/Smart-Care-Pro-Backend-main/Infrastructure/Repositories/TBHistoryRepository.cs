using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Infrastructure.Repositories
{
    public class TBHistoryRepository : Repository<TBHistory>, ITBHistoryRepository
    {
        /// <summary>
        /// Implementation of ITBHistoryRepository interface.
        /// </summary>
        public TBHistoryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a TBHistory by key.
        /// </summary>
        /// <param name="key">Primary key of the table TBHistory.</param>
        /// <returns>Returns a TBHistory if the key is matched.</returns>
        public async Task<TBHistory> GetTBHistoryByKey(Guid key)
        {
            try
            {
                var tBHistory = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (tBHistory != null)
                    tBHistory.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return tBHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of TBHistories.
        /// </summary>
        /// <returns>Returns a list of all TBHistories.</returns>
        public async Task<IEnumerable<TBHistory>> GetTBHistories()
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
        /// The method is used to get a TBHistory by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a TBHistory if the ClientID is matched.</returns>
        public async Task<IEnumerable<TBHistory>> GetTBHistoryByClient(Guid clientId)
        {
            try
            {
                return await context.TBHistories.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
      .Join(
          context.Encounters.AsNoTracking(),
          tBHistory => tBHistory.EncounterId,
          encounter => encounter.Oid,
          (tBHistory, encounter) => new TBHistory
          {
              EncounterId = tBHistory.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              ATTNotCompletedReason = tBHistory.ATTNotCompletedReason,
              ClientId = tBHistory.ClientId,
              WasATTCompleted = tBHistory.WasATTCompleted,
              MonthOfTBCourse = tBHistory.MonthOfTBCourse,
              ModifiedIn = tBHistory.ModifiedIn,
              IsOnTraditionalMedication = tBHistory.IsOnTraditionalMedication,
              ModifiedBy = tBHistory.ModifiedBy,
              CreatedBy = tBHistory.CreatedBy,
              CreatedIn = tBHistory.CreatedIn,
              CurrentlyHaveTB = tBHistory.CurrentlyHaveTB,
              DateCreated = tBHistory.DateCreated,
              DateModified = tBHistory.DateModified,
              EncounterType = tBHistory.EncounterType,
              InteractionId = tBHistory.InteractionId,
              IsDeleted = tBHistory.IsDeleted,
              IsOnAntiTBMedication = tBHistory.IsOnAntiTBMedication,
              IsSynced = tBHistory.IsSynced,
              KindOfTB = tBHistory.KindOfTB,



          }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<TBHistory>> GetTBHistoryByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var tBHistoryAsQuerable = context.TBHistories.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
  .Join(
      context.Encounters.AsNoTracking(),
      tBHistory => tBHistory.EncounterId,
      encounter => encounter.Oid,
      (tBHistory, encounter) => new TBHistory
      {
          EncounterId = tBHistory.EncounterId,
          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
          ATTNotCompletedReason = tBHistory.ATTNotCompletedReason,
          ClientId = clientId,
          WasATTCompleted = tBHistory.WasATTCompleted,
          MonthOfTBCourse = tBHistory.MonthOfTBCourse,
          ModifiedIn = tBHistory.ModifiedIn,
          IsOnTraditionalMedication = tBHistory.IsOnTraditionalMedication,
          ModifiedBy = tBHistory.ModifiedBy,
          CreatedBy = tBHistory.CreatedBy,
          CreatedIn = tBHistory.CreatedIn,
          CurrentlyHaveTB = tBHistory.CurrentlyHaveTB,
          DateCreated = tBHistory.DateCreated,
          DateModified = tBHistory.DateModified,
          EncounterType = tBHistory.EncounterType,
          InteractionId = tBHistory.InteractionId,
          IsDeleted = tBHistory.IsDeleted,
          IsOnAntiTBMedication = tBHistory.IsOnAntiTBMedication,
          IsSynced = tBHistory.IsSynced,
          KindOfTB = tBHistory.KindOfTB,



      }).AsQueryable();

                if (encounterType == null)
                    return await tBHistoryAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await tBHistoryAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetTBHistoryByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.TBHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.TBHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }

        /// <summary>
        /// The method is used to get the list of TBHistory by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all TBHistory by EncounterID.</returns>
        public async Task<IEnumerable<TBHistory>> GetTBHistoryByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.TBHistories.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
      .Join(
          context.Encounters.AsNoTracking(),
          tBHistory => tBHistory.EncounterId,
          encounter => encounter.Oid,
          (tBHistory, encounter) => new TBHistory
          {
              EncounterId = tBHistory.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              ATTNotCompletedReason = tBHistory.ATTNotCompletedReason,
              ClientId = tBHistory.ClientId,
              WasATTCompleted = tBHistory.WasATTCompleted,
              MonthOfTBCourse = tBHistory.MonthOfTBCourse,
              ModifiedIn = tBHistory.ModifiedIn,
              IsOnTraditionalMedication = tBHistory.IsOnTraditionalMedication,
              ModifiedBy = tBHistory.ModifiedBy,
              CreatedBy = tBHistory.CreatedBy,
              CreatedIn = tBHistory.CreatedIn,
              CurrentlyHaveTB = tBHistory.CurrentlyHaveTB,
              DateCreated = tBHistory.DateCreated,
              DateModified = tBHistory.DateModified,
              EncounterType = tBHistory.EncounterType,
              InteractionId = tBHistory.InteractionId,
              IsDeleted = tBHistory.IsDeleted,
              IsOnAntiTBMedication = tBHistory.IsOnAntiTBMedication,
              IsSynced = tBHistory.IsSynced,
              KindOfTB = tBHistory.KindOfTB,



          }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}