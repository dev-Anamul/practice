using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

/*
 * Created by   : Bithy
 * Date created : 25.12.2022
 * Modified by  : Shakil
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ISystemReviewRepository interface.
    /// </summary>
    public class SystemReviewRepository : Repository<SystemReview>, ISystemReviewRepository
    {
        public SystemReviewRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a system review by key.
        /// </summary>
        /// <param name="key">Primary key of the table SystemReviews.</param>
        /// <returns>Returns a system review if the key is matched.</returns>
        public async Task<SystemReview> GetSystemReviewByKey(Guid key)
        {
            try
            {
                var systemReview = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (systemReview != null)
                    systemReview.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return systemReview;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SystemReview>> GetSystemReviewByPhysicalSystemId(int physicalSystemId)
        {
            try
            {
                var systemReview = await context.SystemReviews.Where(s => s.PhysicalSystemId == physicalSystemId && s.IsDeleted == false).ToListAsync();
                return systemReview;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a OPD visit by key.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisits.</param>
        /// <returns>Returns a OPD visit if the key is matched.</returns>
        public async Task<IEnumerable<SystemReview>> GetSystemReviewByOPDVisit(Guid OPDVisitID)
        {
            return await context.SystemReviews.Include(x => x.PhysicalSystem).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == OPDVisitID)
                    .Join(
                           context.Encounters.AsNoTracking(),
                           systemReview => systemReview.EncounterId,
                           encounter => encounter.Oid,
                           (systemReview, encounter) => new SystemReview
                           {
                               EncounterId = systemReview.EncounterId,
                               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                               PhysicalSystemId = systemReview.PhysicalSystemId,
                               PhysicalSystem = systemReview.PhysicalSystem,
                               Note = systemReview.Note,
                               ClientId = systemReview.ClientId,
                               CreatedBy = systemReview.CreatedBy,
                               DateModified = systemReview.DateModified,
                               DateCreated = systemReview.DateCreated,
                               CreatedIn = systemReview.CreatedIn,
                               EncounterType = systemReview.EncounterType,
                               ModifiedIn = systemReview.ModifiedIn,
                               InteractionId = systemReview.InteractionId,
                               IsDeleted = systemReview.IsDeleted,
                               IsSynced = systemReview.IsSynced,
                               ModifiedBy = systemReview.ModifiedBy,

                           }).OrderByDescending(x => x.EncounterDate).ToListAsync();
        }

        /// <summary>
        /// The method is used to get the list of system reviews.
        /// </summary>
        /// <returns>Returns a list of all system reviews.</returns>
        public async Task<IEnumerable<SystemReview>> GetSystemReviews()
        {
            try
            {
                return await LoadListWithChildAsync<SystemReview>(u => u.IsDeleted == false, p => p.PhysicalSystem);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Returns a clients if the key is matched.</returns>
        public async Task<IEnumerable<SystemReview>> GetSystemReviewByClient(Guid ClientID)
        {
            return await context.SystemReviews.Include(x => x.PhysicalSystem).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == ClientID)
                    .Join(
                           context.Encounters.AsNoTracking(),
                           systemReview => systemReview.EncounterId,
                           encounter => encounter.Oid,
                           (systemReview, encounter) => new SystemReview
                           {
                               EncounterId = systemReview.EncounterId,
                               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                               PhysicalSystemId = systemReview.PhysicalSystemId,
                               PhysicalSystem = systemReview.PhysicalSystem,
                               Note = systemReview.Note,
                               ClientId = systemReview.ClientId,
                               CreatedBy = systemReview.CreatedBy,
                               DateModified = systemReview.DateModified,
                               DateCreated = systemReview.DateCreated,
                               CreatedIn = systemReview.CreatedIn,
                               EncounterType = systemReview.EncounterType,
                               ModifiedIn = systemReview.ModifiedIn,
                               InteractionId = systemReview.InteractionId,
                               IsDeleted = systemReview.IsDeleted,
                               IsSynced = systemReview.IsSynced,
                               ModifiedBy = systemReview.ModifiedBy,

                           }).OrderByDescending(x => x.EncounterDate).ToListAsync();
        }
        public async Task<IEnumerable<SystemReview>> GetSystemReviewByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType)
        {
            var systemReviewAsQuerable = context.SystemReviews.Include(x => x.PhysicalSystem).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == ClientID)
                   .Join(
                          context.Encounters.AsNoTracking(),
                          systemReview => systemReview.EncounterId,
                          encounter => encounter.Oid,
                          (systemReview, encounter) => new SystemReview
                          {
                              EncounterId = systemReview.EncounterId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              PhysicalSystemId = systemReview.PhysicalSystemId,
                              PhysicalSystem = systemReview.PhysicalSystem,
                              Note = systemReview.Note,
                              ClientId = systemReview.ClientId,
                              CreatedBy = systemReview.CreatedBy,
                              DateModified = systemReview.DateModified,
                              DateCreated = systemReview.DateCreated,
                              CreatedIn = systemReview.CreatedIn,
                              EncounterType = systemReview.EncounterType,
                              ModifiedIn = systemReview.ModifiedIn,
                              InteractionId = systemReview.InteractionId,
                              IsDeleted = systemReview.IsDeleted,
                              IsSynced = systemReview.IsSynced,
                              ModifiedBy = systemReview.ModifiedBy,

                          }).AsQueryable();

            if (encounterType == null)
                return await systemReviewAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            else
                return await systemReviewAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
        }
        public int GetSystemReviewByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.SystemReviews.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.SystemReviews.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
    }
}