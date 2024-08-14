using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 06.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class IdentifiedReasonRepository : Repository<IdentifiedReason>, IIdentifiedReasonRepository
    {
        /// <summary>
        /// Implementation of ITBHistoryRepository interface.
        /// </summary>
        public IdentifiedReasonRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a IdentifiedReason by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedReason.</param>
        /// <returns>Returns a IdentifiedReason if the key is matched.</returns>
        public async Task<IdentifiedReason> GetIdentifiedReasonByKey(Guid key)
        {
            try
            {
                var identifiedReason = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedReason != null)
                {
                    identifiedReason.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedReason.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedReason.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedReason.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedReason.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedReason.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return identifiedReason;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedReasons.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedReasons.</returns>
        public async Task<IEnumerable<IdentifiedReason>> GetIdentifiedReasons()
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
        /// The method is used to get a IdentifiedReason by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a IdentifiedReason if the ClientID is matched.</returns>
        public async Task<IEnumerable<IdentifiedReason>> GetIdentifiedReasonByClient(Guid clientId)
        {
            try
            {
                return await context.IdentifiedReasons.Include(p => p.TBSuspectingReason).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
      .Join(
          context.Encounters.AsNoTracking(),
          identifiedReason => identifiedReason.EncounterId,
          encounter => encounter.Oid,
          (identifiedReason, encounter) => new IdentifiedReason
          {
              EncounterId = identifiedReason.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              ClientId = identifiedReason.ClientId,
              CreatedBy = identifiedReason.CreatedBy,
              CreatedIn = identifiedReason.CreatedIn,
              DateCreated = identifiedReason.DateCreated,
              DateModified = identifiedReason.DateModified,
              EncounterType = identifiedReason.EncounterType,
              InteractionId = identifiedReason.InteractionId,
              IsDeleted = identifiedReason.IsDeleted,
              IsSynced = identifiedReason.IsSynced,
              ModifiedBy = identifiedReason.ModifiedBy,
              ModifiedIn = identifiedReason.ModifiedIn,
              TBSuspectingReason = identifiedReason.TBSuspectingReason,
              TBSuspectingReasonId = identifiedReason.TBSuspectingReasonId,
              ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedReason.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
              FacilityName = context.Facilities.Where(x => x.Oid == identifiedReason.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
          }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<IdentifiedReason>> GetIdentifiedReasonByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var identifiedReasonAsQuerable = context.IdentifiedReasons.Include(p => p.TBSuspectingReason).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         identifiedReason => identifiedReason.EncounterId,
         encounter => encounter.Oid,
         (identifiedReason, encounter) => new IdentifiedReason
         {
             EncounterId = identifiedReason.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             ClientId = identifiedReason.ClientId,
             CreatedBy = identifiedReason.CreatedBy,
             CreatedIn = identifiedReason.CreatedIn,
             DateCreated = identifiedReason.DateCreated,
             DateModified = identifiedReason.DateModified,
             EncounterType = identifiedReason.EncounterType,
             InteractionId = identifiedReason.InteractionId,
             IsDeleted = identifiedReason.IsDeleted,
             IsSynced = identifiedReason.IsSynced,
             ModifiedBy = identifiedReason.ModifiedBy,
             ModifiedIn = identifiedReason.ModifiedIn,
             TBSuspectingReason = identifiedReason.TBSuspectingReason,
             TBSuspectingReasonId = identifiedReason.TBSuspectingReasonId,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedReason.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == identifiedReason.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
         }).AsQueryable();

                if (encounterType == null)
                    return await identifiedReasonAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await identifiedReasonAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetIdentifiedReasonByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.IdentifiedReasons.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.IdentifiedReasons.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get a IdentifiedReason by EncounterId.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a IdentifiedReason if the EncounterId is matched.</returns>
        public async Task<IEnumerable<IdentifiedReason>> GetIdentifiedReasonByEncounterId(Guid encounterId)
        {
            try
            {
                return await context.IdentifiedReasons.Include(p => p.TBSuspectingReason).Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
      .Join(
          context.Encounters.AsNoTracking(),
          identifiedReason => identifiedReason.EncounterId,
          encounter => encounter.Oid,
          (identifiedReason, encounter) => new IdentifiedReason
          {
              EncounterId = identifiedReason.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              ClientId = identifiedReason.ClientId,
              CreatedBy = identifiedReason.CreatedBy,
              CreatedIn = identifiedReason.CreatedIn,
              DateCreated = identifiedReason.DateCreated,
              DateModified = identifiedReason.DateModified,
              EncounterType = identifiedReason.EncounterType,
              InteractionId = identifiedReason.InteractionId,
              IsDeleted = identifiedReason.IsDeleted,
              IsSynced = identifiedReason.IsSynced,
              ModifiedBy = identifiedReason.ModifiedBy,
              ModifiedIn = identifiedReason.ModifiedIn,
              TBSuspectingReason = identifiedReason.TBSuspectingReason,
              TBSuspectingReasonId = identifiedReason.TBSuspectingReasonId,
              ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedReason.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
              FacilityName = context.Facilities.Where(x => x.Oid == identifiedReason.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


          }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}