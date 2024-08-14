using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class IdentifiedPreferredFeedingRepository : Repository<IdentifiedPreferredFeeding>, IIdentifiedPreferredFeedingRepository
    {
        /// <summary>
        /// Implementation of IIdentifiedPreferredFeedingRepository interface.
        /// </summary>
        public IdentifiedPreferredFeedingRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a IdentifiedPreferredFeeding by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPreferredFeedings.</param>
        /// <returns>Returns a IdentifiedPreferredFeeding if the key is matched.</returns>
        public async Task<IdentifiedPreferredFeeding> GetIdentifiedPreferredFeedingByKey(Guid key)
        {
            try
            {
                var identifiedPreferredFeeding = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedPreferredFeeding != null)
                {
                    identifiedPreferredFeeding.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedPreferredFeeding.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedPreferredFeeding.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedPreferredFeeding.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedPreferredFeeding.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedPreferredFeeding.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return identifiedPreferredFeeding;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPreferredFeeding.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedPreferredFeedings.</returns>
        public async Task<IEnumerable<IdentifiedPreferredFeeding>> GetIdentifiedPreferredFeedings()
        {
            try
            {
                return await LoadListWithChildAsync<IdentifiedPreferredFeeding>(s => s.IsDeleted == false, p => p.PreferredFeeding);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPreferredFeeding by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a list of all IdentifiedPreferredFeeding by ClientID.</returns>
        public async Task<IEnumerable<IdentifiedPreferredFeeding>> GetIdentifiedPreferredFeedingByClient(Guid clientId)
        {
            try
            {
                return await context.IdentifiedPreferredFeedings.Include(x => x.PreferredFeeding).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         identifiedPreferredFeeding => identifiedPreferredFeeding.EncounterId,
         encounter => encounter.Oid,
         (identifiedPreferredFeeding, encounter) => new IdentifiedPreferredFeeding
         {
             EncounterId = identifiedPreferredFeeding.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             ClientId = identifiedPreferredFeeding.ClientId,
             EncounterType = identifiedPreferredFeeding.EncounterType,
             PreferredFeeding = identifiedPreferredFeeding.PreferredFeeding,
             PreferredFeedingId = identifiedPreferredFeeding.PreferredFeedingId,
             CreatedBy = identifiedPreferredFeeding.CreatedBy,
             CreatedIn = identifiedPreferredFeeding.CreatedIn,
             DateCreated = identifiedPreferredFeeding.DateCreated,
             DateModified = identifiedPreferredFeeding.DateModified,
             InteractionId = identifiedPreferredFeeding.InteractionId,
             IsDeleted = identifiedPreferredFeeding.IsDeleted,
             IsSynced = identifiedPreferredFeeding.IsSynced,
             ModifiedBy = identifiedPreferredFeeding.ModifiedBy,
             ModifiedIn = identifiedPreferredFeeding.ModifiedIn,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedPreferredFeeding.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == identifiedPreferredFeeding.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

         }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<IdentifiedPreferredFeeding>> GetIdentifiedPreferredFeedingByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var identifiedPreferredFeedingAsQuerable = context.IdentifiedPreferredFeedings.Include(x => x.PreferredFeeding).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         identifiedPreferredFeeding => identifiedPreferredFeeding.EncounterId,
         encounter => encounter.Oid,
         (identifiedPreferredFeeding, encounter) => new IdentifiedPreferredFeeding
         {
             EncounterId = identifiedPreferredFeeding.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             ClientId = identifiedPreferredFeeding.ClientId,
             EncounterType = identifiedPreferredFeeding.EncounterType,
             PreferredFeeding = identifiedPreferredFeeding.PreferredFeeding,
             PreferredFeedingId = identifiedPreferredFeeding.PreferredFeedingId,
             CreatedBy = identifiedPreferredFeeding.CreatedBy,
             CreatedIn = identifiedPreferredFeeding.CreatedIn,
             DateCreated = identifiedPreferredFeeding.DateCreated,
             DateModified = identifiedPreferredFeeding.DateModified,
             InteractionId = identifiedPreferredFeeding.InteractionId,
             IsDeleted = identifiedPreferredFeeding.IsDeleted,
             IsSynced = identifiedPreferredFeeding.IsSynced,
             ModifiedBy = identifiedPreferredFeeding.ModifiedBy,
             ModifiedIn = identifiedPreferredFeeding.ModifiedIn,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedPreferredFeeding.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == identifiedPreferredFeeding.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

         }).AsQueryable();

                if (encounterType == null)
                    return await identifiedPreferredFeedingAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await identifiedPreferredFeedingAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetIdentifiedPreferredFeedingByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.IdentifiedPreferredFeedings.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.IdentifiedPreferredFeedings.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPreferredFeeding by EncounterID.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a list of all IdentifiedPreferredFeeding by EncounterID.</returns>
        public async Task<IEnumerable<IdentifiedPreferredFeeding>> GetIdentifiedPreferredFeedingByEncounter(Guid encounterId)
        {
            try
            {
                return await context.IdentifiedPreferredFeedings.Include(x => x.PreferredFeeding).Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         identifiedPreferredFeeding => identifiedPreferredFeeding.EncounterId,
         encounter => encounter.Oid,
         (identifiedPreferredFeeding, encounter) => new IdentifiedPreferredFeeding
         {
             EncounterId = identifiedPreferredFeeding.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             ClientId = identifiedPreferredFeeding.ClientId,
             EncounterType = identifiedPreferredFeeding.EncounterType,
             PreferredFeeding = identifiedPreferredFeeding.PreferredFeeding,
             PreferredFeedingId = identifiedPreferredFeeding.PreferredFeedingId,
             CreatedBy = identifiedPreferredFeeding.CreatedBy,
             CreatedIn = identifiedPreferredFeeding.CreatedIn,
             DateCreated = identifiedPreferredFeeding.DateCreated,
             DateModified = identifiedPreferredFeeding.DateModified,
             InteractionId = identifiedPreferredFeeding.InteractionId,
             IsDeleted = identifiedPreferredFeeding.IsDeleted,
             IsSynced = identifiedPreferredFeeding.IsSynced,
             ModifiedBy = identifiedPreferredFeeding.ModifiedBy,
             ModifiedIn = identifiedPreferredFeeding.ModifiedIn,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedPreferredFeeding.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == identifiedPreferredFeeding.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

         }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPreferredFeeding by PreferredFeedingId.
        /// </summary>
        /// <param name="preferredFeedingId"></param>
        /// <returns>Returns a list of all IdentifiedPreferredFeeding by PreferredFeedingId.</returns>
        public async Task<IEnumerable<IdentifiedPreferredFeeding>> ReadIdentifiedPreferredFeedingByPreferredFeeding(int preferredFeedingId)
        {
            try
            {
                return await context.IdentifiedPreferredFeedings.Include(x => x.PreferredFeeding).Where(p => p.IsDeleted == false && p.PreferredFeedingId == preferredFeedingId).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         identifiedPreferredFeeding => identifiedPreferredFeeding.EncounterId,
         encounter => encounter.Oid,
         (identifiedPreferredFeeding, encounter) => new IdentifiedPreferredFeeding
         {
             EncounterId = identifiedPreferredFeeding.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             ClientId = identifiedPreferredFeeding.ClientId,
             EncounterType = identifiedPreferredFeeding.EncounterType,
             PreferredFeeding = identifiedPreferredFeeding.PreferredFeeding,
             PreferredFeedingId = identifiedPreferredFeeding.PreferredFeedingId,
             CreatedBy = identifiedPreferredFeeding.CreatedBy,
             CreatedIn = identifiedPreferredFeeding.CreatedIn,
             DateCreated = identifiedPreferredFeeding.DateCreated,
             DateModified = identifiedPreferredFeeding.DateModified,
             InteractionId = identifiedPreferredFeeding.InteractionId,
             IsDeleted = identifiedPreferredFeeding.IsDeleted,
             IsSynced = identifiedPreferredFeeding.IsSynced,
             ModifiedBy = identifiedPreferredFeeding.ModifiedBy,
             ModifiedIn = identifiedPreferredFeeding.ModifiedIn,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedPreferredFeeding.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == identifiedPreferredFeeding.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

         }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}