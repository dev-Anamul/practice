using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

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
    public class IdentifiedPlacentaRemovalRepository : Repository<IdentifiedPlacentaRemoval>, IIdentifiedPlacentaRemovalRepository
    {
        /// <summary>
        /// Implementation of IIdentifiedPlacentaRemovalRepository interface.
        /// </summary>
        public IdentifiedPlacentaRemovalRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a IdentifiedPlacentaRemoval by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPlacentaRemovals.</param>
        /// <returns>Returns a IdentifiedPlacentaRemoval if the key is matched.</returns>
        public async Task<IdentifiedPlacentaRemoval> GetIdentifiedPlacentaRemovalByKey(Guid key)
        {
            try
            {
                var identifiedPlacentaRemoval = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedPlacentaRemoval != null)
                {
                    identifiedPlacentaRemoval.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedPlacentaRemoval.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedPlacentaRemoval.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedPlacentaRemoval.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedPlacentaRemoval.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedPlacentaRemoval.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                }
                return identifiedPlacentaRemoval;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPlacentaRemoval.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedPlacentaRemovals.</returns>
        public async Task<IEnumerable<IdentifiedPlacentaRemoval>> GetIdentifiedPlacentaRemovals()
        {
            try
            {
                return await QueryAsync(n => n.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a IdentifiedPlacentaRemoval by PlacentaRemovalId.
        /// </summary>
        /// <param name="placentaRemovalId"></param>
        /// <returns>Returns a IdentifiedPlacentaRemoval if the PlacentaRemovalId is matched.</returns>
        public async Task<IEnumerable<IdentifiedPlacentaRemoval>> GetIdentifiedPlacentaRemovalByPlacentaRemoval(Guid placentaRemovalId)
        {
            try
            {
                return await context.IdentifiedPlacentaRemovals.Where(p => p.IsDeleted == false && p.PlacentaRemovalId == placentaRemovalId).AsNoTracking()
               .Join(
                   context.Encounters.AsNoTracking(),
                 identifiedPlacentaRemoval => identifiedPlacentaRemoval.EncounterId,
                   encounter => encounter.Oid,
                   (identifiedPlacentaRemoval, encounter) => new IdentifiedPlacentaRemoval
                   {
                       EncounterId = identifiedPlacentaRemoval.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       ModifiedIn = identifiedPlacentaRemoval.ModifiedIn,
                       ModifiedBy = identifiedPlacentaRemoval.ModifiedBy,
                       IsSynced = identifiedPlacentaRemoval.IsSynced,
                       IsDeleted = identifiedPlacentaRemoval.IsDeleted,
                       CreatedIn = identifiedPlacentaRemoval.CreatedIn,
                       CreatedBy = identifiedPlacentaRemoval.CreatedBy,
                       DateCreated = identifiedPlacentaRemoval.DateCreated,
                       DateModified = identifiedPlacentaRemoval.DateCreated,
                       EncounterType = identifiedPlacentaRemoval.EncounterType,
                       InteractionId = identifiedPlacentaRemoval.InteractionId,
                       Placenta = identifiedPlacentaRemoval.Placenta,
                       PlacentaRemovalId = identifiedPlacentaRemoval.PlacentaRemovalId,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedPlacentaRemoval.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == identifiedPlacentaRemoval.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPlacentaRemoval by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedPlacentaRemoval by EncounterID.</returns>
        public async Task<IEnumerable<IdentifiedPlacentaRemoval>> GetIdentifiedPlacentaRemovalByEncounter(Guid encounterId)
        {
            try
            {
                return await context.IdentifiedPlacentaRemovals.Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
               .Join(
                   context.Encounters.AsNoTracking(),
                 identifiedPlacentaRemoval => identifiedPlacentaRemoval.EncounterId,
                   encounter => encounter.Oid,
                   (identifiedPlacentaRemoval, encounter) => new IdentifiedPlacentaRemoval
                   {
                       EncounterId = identifiedPlacentaRemoval.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       ModifiedIn = identifiedPlacentaRemoval.ModifiedIn,
                       ModifiedBy = identifiedPlacentaRemoval.ModifiedBy,
                       IsSynced = identifiedPlacentaRemoval.IsSynced,
                       IsDeleted = identifiedPlacentaRemoval.IsDeleted,
                       CreatedIn = identifiedPlacentaRemoval.CreatedIn,
                       CreatedBy = identifiedPlacentaRemoval.CreatedBy,
                       DateCreated = identifiedPlacentaRemoval.DateCreated,
                       DateModified = identifiedPlacentaRemoval.DateCreated,
                       EncounterType = identifiedPlacentaRemoval.EncounterType,
                       InteractionId = identifiedPlacentaRemoval.InteractionId,
                       Placenta = identifiedPlacentaRemoval.Placenta,
                       PlacentaRemovalId = identifiedPlacentaRemoval.PlacentaRemovalId,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedPlacentaRemoval.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == identifiedPlacentaRemoval.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}