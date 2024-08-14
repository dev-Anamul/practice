using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bithy
 * Date created : 06.04.2023
 * Modified by  :
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class TBServiceRepository : Repository<TBService>, ITBServiceRepository
    {
        public TBServiceRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get TBService by key.
        /// </summary>
        /// <param name="key">Primary key of the table TBServices.</param>
        /// <returns>Returns a TBService if the key is matched.</returns>
        public async Task<TBService> GetTBServiceByKey(Guid key)
        {
            try
            {
                var tBService = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (tBService != null)
                    tBService.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return tBService;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a TBService by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a TBService if the ClientID is matched.</returns>
        public async Task<IEnumerable<TBService>> GetTBServiceByClient(Guid clientID)
        {
            try
            {
                return await context.TBServices.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientID)
  .Join(
      context.Encounters.AsNoTracking(),
      tBService => tBService.EncounterId,
      encounter => encounter.Oid,
      (tBService, encounter) => new TBService
      {
          EncounterId = tBService.EncounterId,
          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
          ClientId = tBService.ClientId,
          CaseIdNumber = tBService.CaseIdNumber,
          CreatedBy = tBService.CreatedBy,
          CreatedIn = tBService.CreatedIn,
          DateCreated = tBService.DateCreated,
          DateDischarged = tBService.DateDischarged,
          DateModified = tBService.DateModified,
          EncounterType = tBService.EncounterType,
          InteractionId = tBService.InteractionId,
          IsDeleted = tBService.IsDeleted,
          IsExMiner = tBService.IsExMiner,
          IsHealthCareWorker = tBService.IsHealthCareWorker,
          IsInmate = tBService.IsInmate,
          IsMiner = tBService.IsMiner,
          IsSynced = tBService.IsSynced,
          ModifiedBy = tBService.ModifiedBy,
          ModifiedIn = tBService.ModifiedIn,
          OtherPatientCategory = tBService.OtherPatientCategory,
          TreatmentOutcome = tBService.TreatmentOutcome,
          TreatmentStarted = tBService.TreatmentStarted,

      }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<TBService>> GetTBServiceByClient(Guid clientID, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var tBServiceAsQuerable = context.TBServices.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientID)
  .Join(
      context.Encounters.AsNoTracking(),
      tBService => tBService.EncounterId,
      encounter => encounter.Oid,
      (tBService, encounter) => new TBService
      {
          EncounterId = tBService.EncounterId,
          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
          ClientId = tBService.ClientId,
          CaseIdNumber = tBService.CaseIdNumber,
          CreatedBy = tBService.CreatedBy,
          CreatedIn = tBService.CreatedIn,
          DateCreated = tBService.DateCreated,
          DateDischarged = tBService.DateDischarged,
          DateModified = tBService.DateModified,
          EncounterType = tBService.EncounterType,
          InteractionId = tBService.InteractionId,
          IsDeleted = tBService.IsDeleted,
          IsExMiner = tBService.IsExMiner,
          IsHealthCareWorker = tBService.IsHealthCareWorker,
          IsInmate = tBService.IsInmate,
          IsMiner = tBService.IsMiner,
          IsSynced = tBService.IsSynced,
          ModifiedBy = tBService.ModifiedBy,
          ModifiedIn = tBService.ModifiedIn,
          OtherPatientCategory = tBService.OtherPatientCategory,
          TreatmentOutcome = tBService.TreatmentOutcome,
          TreatmentStarted = tBService.TreatmentStarted,


      }).AsQueryable();

                if (encounterType == null)
                    return await tBServiceAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await tBServiceAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetTBServiceByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.TBServices.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.TBServices.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        public async Task<TBService> GetActiveTBServiceByClient(Guid ClientID)
        {
            try
            {
                var tBService = await FirstOrDefaultAsync(b => b.IsDeleted == false && b.ClientId == ClientID && b.DateDischarged == null);

                if (tBService != null)
                    tBService.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return tBService;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of TBServices.
        /// </summary>
        /// <returns>Returns a list of all TBServices.</returns>
        public async Task<IEnumerable<TBService>> GetTBServices()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TBService>> GetTBServiceByOpdVisit(Guid OPDVisitID)
        {
            try
            {
                return await context.TBServices.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == OPDVisitID)
       .Join(
           context.Encounters.AsNoTracking(),
           tBService => tBService.EncounterId,
           encounter => encounter.Oid,
           (tBService, encounter) => new TBService
           {
               EncounterId = tBService.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               ClientId = tBService.ClientId,
               CaseIdNumber = tBService.CaseIdNumber,
               CreatedBy = tBService.CreatedBy,
               CreatedIn = tBService.CreatedIn,
               DateCreated = tBService.DateCreated,
               DateDischarged = tBService.DateDischarged,
               DateModified = tBService.DateModified,
               EncounterType = tBService.EncounterType,
               InteractionId = tBService.InteractionId,
               IsDeleted = tBService.IsDeleted,
               IsExMiner = tBService.IsExMiner,
               IsHealthCareWorker = tBService.IsHealthCareWorker,
               IsInmate = tBService.IsInmate,
               IsMiner = tBService.IsMiner,
               IsSynced = tBService.IsSynced,
               ModifiedBy = tBService.ModifiedBy,
               ModifiedIn = tBService.ModifiedIn,
               OtherPatientCategory = tBService.OtherPatientCategory,
               TreatmentOutcome = tBService.TreatmentOutcome,
               TreatmentStarted = tBService.TreatmentStarted,


           }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}