using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    public class IdentifiedTBFindingRepository : Repository<IdentifiedTBFinding>, IIdentifiedTBFindingRepository
    {
        /// <summary>
        /// Implementation of ITBHistoryRepository interface.
        /// </summary>
        public IdentifiedTBFindingRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a IdentifiedTBFinding by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedTBFinding.</param>
        /// <returns>Returns a IdentifiedTBFinding if the key is matched.</returns>
        public async Task<IdentifiedTBFinding> GetIdentifiedTBFindingByKey(Guid key)
        {
            try
            {
                var identifiedTBFinding = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedTBFinding != null)
                {
                    identifiedTBFinding.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedTBFinding.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedTBFinding.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedTBFinding.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedTBFinding.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedTBFinding.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                }
                return identifiedTBFinding;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedTBFindings.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedTBFindings.</returns>
        public async Task<IEnumerable<IdentifiedTBFinding>> GetIdentifiedTBFindings()
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
        /// The method is used to get a IdentifiedTBFinding by TBFindingId.
        /// </summary>
        /// <param name="TBFindingId"></param>
        /// <returns>Returns a IdentifiedTBFinding if the TBFindingId is matched.</returns>
        public async Task<IEnumerable<IdentifiedTBFinding>> GetIdentifiedTBFindingByTBFinding(int TBFindingId)
        {
            try
            {
                return await QueryAsync(p => p.TBFindingId == TBFindingId && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a IdentifiedTBFinding by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a IdentifiedTBFinding if the ClientID is matched.</returns>
        public async Task<IEnumerable<IdentifiedTBFinding>> GetIdentifiedTBFindingByClient(Guid clientId)
        {
            try
            {
                return await context.IdentifiedTBFindings.Include(p => p.TBFinding).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
      .Join(
          context.Encounters.AsNoTracking(),
          identifiedTBFinding => identifiedTBFinding.EncounterId,
          encounter => encounter.Oid,
          (identifiedTBFinding, encounter) => new IdentifiedTBFinding
          {
              EncounterId = identifiedTBFinding.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              ClientId = identifiedTBFinding.ClientId,
              CreatedBy = identifiedTBFinding.CreatedBy,
              CreatedIn = identifiedTBFinding.CreatedIn,
              DateCreated = identifiedTBFinding.DateCreated,
              DateModified = identifiedTBFinding.DateModified,
              EncounterType = identifiedTBFinding.EncounterType,
              InteractionId = identifiedTBFinding.InteractionId,
              IsDeleted = identifiedTBFinding.IsDeleted,
              IsSynced = identifiedTBFinding.IsSynced,
              ModifiedBy = identifiedTBFinding.ModifiedBy,
              ModifiedIn = identifiedTBFinding.ModifiedIn,
              TBFinding = identifiedTBFinding.TBFinding,
              TBFindingId = identifiedTBFinding.TBFindingId,
              ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedTBFinding.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
              FacilityName = context.Facilities.Where(x => x.Oid == identifiedTBFinding.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


          }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<IdentifiedTBFinding>> GetIdentifiedTBFindingByClientLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.IdentifiedTBFindings.Include(p => p.TBFinding).Where(p => p.IsDeleted == false && p.DateCreated >= Last24Hours && p.ClientId == clientId).AsNoTracking()
      .Join(
          context.Encounters.AsNoTracking(),
          identifiedTBFinding => identifiedTBFinding.EncounterId,
          encounter => encounter.Oid,
          (identifiedTBFinding, encounter) => new IdentifiedTBFinding
          {
              EncounterId = identifiedTBFinding.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              ClientId = identifiedTBFinding.ClientId,
              CreatedBy = identifiedTBFinding.CreatedBy,
              CreatedIn = identifiedTBFinding.CreatedIn,
              DateCreated = identifiedTBFinding.DateCreated,
              DateModified = identifiedTBFinding.DateModified,
              EncounterType = identifiedTBFinding.EncounterType,
              InteractionId = identifiedTBFinding.InteractionId,
              IsDeleted = identifiedTBFinding.IsDeleted,
              IsSynced = identifiedTBFinding.IsSynced,
              ModifiedBy = identifiedTBFinding.ModifiedBy,
              ModifiedIn = identifiedTBFinding.ModifiedIn,
              TBFinding = identifiedTBFinding.TBFinding,
              TBFindingId = identifiedTBFinding.TBFindingId,
              ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedTBFinding.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
              FacilityName = context.Facilities.Where(x => x.Oid == identifiedTBFinding.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


          }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<IdentifiedTBFinding>> GetIdentifiedTBFindingByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var identifiedTBFindingAsQuerable = context.IdentifiedTBFindings.Include(p => p.TBFinding).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
         .Join(
             context.Encounters.AsNoTracking(),
             identifiedTBFinding => identifiedTBFinding.EncounterId,
             encounter => encounter.Oid,
             (identifiedTBFinding, encounter) => new IdentifiedTBFinding
             {
                 EncounterId = identifiedTBFinding.EncounterId,
                 EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                 ClientId = identifiedTBFinding.ClientId,
                 CreatedBy = identifiedTBFinding.CreatedBy,
                 CreatedIn = identifiedTBFinding.CreatedIn,
                 DateCreated = identifiedTBFinding.DateCreated,
                 DateModified = identifiedTBFinding.DateModified,
                 EncounterType = identifiedTBFinding.EncounterType,
                 InteractionId = identifiedTBFinding.InteractionId,
                 IsDeleted = identifiedTBFinding.IsDeleted,
                 IsSynced = identifiedTBFinding.IsSynced,
                 ModifiedBy = identifiedTBFinding.ModifiedBy,
                 ModifiedIn = identifiedTBFinding.ModifiedIn,
                 TBFinding = identifiedTBFinding.TBFinding,
                 TBFindingId = identifiedTBFinding.TBFindingId,
                 ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedTBFinding.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                 FacilityName = context.Facilities.Where(x => x.Oid == identifiedTBFinding.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


             }).AsQueryable();

                if (encounterType == null)
                    return await identifiedTBFindingAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await identifiedTBFindingAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetIdentifiedTBFindingByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.IdentifiedTBFindings.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.IdentifiedTBFindings.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get a IdentifiedTBFinding by EncounterId.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a IdentifiedTBFinding if the EncounterId is matched.</returns>
        public async Task<IEnumerable<IdentifiedTBFinding>> GetIdentifiedTBFindingByEncounterId(Guid encounterId)
        {
            try
            {
                return await context.IdentifiedTBFindings.Include(p => p.TBFinding).Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
      .Join(
          context.Encounters.AsNoTracking(),
          identifiedTBFinding => identifiedTBFinding.EncounterId,
          encounter => encounter.Oid,
          (identifiedTBFinding, encounter) => new IdentifiedTBFinding
          {
              EncounterId = identifiedTBFinding.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              ClientId = identifiedTBFinding.ClientId,
              CreatedBy = identifiedTBFinding.CreatedBy,
              CreatedIn = identifiedTBFinding.CreatedIn,
              DateCreated = identifiedTBFinding.DateCreated,
              DateModified = identifiedTBFinding.DateModified,
              EncounterType = identifiedTBFinding.EncounterType,
              InteractionId = identifiedTBFinding.InteractionId,
              IsDeleted = identifiedTBFinding.IsDeleted,
              IsSynced = identifiedTBFinding.IsSynced,
              ModifiedBy = identifiedTBFinding.ModifiedBy,
              ModifiedIn = identifiedTBFinding.ModifiedIn,
              TBFinding = identifiedTBFinding.TBFinding,
              TBFindingId = identifiedTBFinding.TBFindingId,
              ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedTBFinding.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
              FacilityName = context.Facilities.Where(x => x.Oid == identifiedTBFinding.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


          }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}