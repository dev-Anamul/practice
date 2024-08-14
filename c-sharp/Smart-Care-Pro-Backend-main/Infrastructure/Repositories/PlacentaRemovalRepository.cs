using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Biplob Roy
 * Date created : 02.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class PlacentaRemovalRepository : Repository<PlacentaRemoval>, IPlacentaRemovalRepository
    {
        public PlacentaRemovalRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// The method is used to a get PlacentaRemoval by key.
        /// </summary>
        /// <param name="key">Primary key of the table PlacentaRemovals.</param>
        /// <returns>Returns a PlacentaRemoval if the key is matched.</returns>
        public async Task<PlacentaRemoval> GetPlacentaRemovalByKey(Guid key)
        {
            try
            {
                var placentaRemoval = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (placentaRemoval != null)
                    placentaRemoval.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return placentaRemoval;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PlacentaRemovals.
        /// </summary>
        /// <returns>Returns a list of all Medical treatments.</returns>
        public async Task<IEnumerable<PlacentaRemoval>> GetPlacentaRemovals()
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

        /// <summary>
        /// The method is used to get the list of PlacentaRemoval by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all PlacentaRemoval by EncounterID.</returns>
        public async Task<IEnumerable<PlacentaRemoval>> GetPlacentaRemovalByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.PlacentaRemovals.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
            .Join(
                context.Encounters.AsNoTracking(),
                placentaRemoval => placentaRemoval.EncounterId,
                encounter => encounter.Oid,
                (placentaRemoval, encounter) => new PlacentaRemoval
                {
                    EncounterId = placentaRemoval.EncounterId,
                    EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                    CreatedIn = placentaRemoval.CreatedIn,
                    CreatedBy = placentaRemoval.CreatedBy,
                    DateCreated = placentaRemoval.DateCreated,
                    DateModified = placentaRemoval.DateModified,
                    DeliveryId = placentaRemoval.DeliveryId,
                    IsDeleted = placentaRemoval.IsDeleted,
                    InteractionId = placentaRemoval.InteractionId,
                    Other = placentaRemoval.Other,
                    EncounterType = placentaRemoval.EncounterType,
                    IsPlacentaRemovalCompleted = placentaRemoval.IsPlacentaRemovalCompleted,
                    IsSynced = placentaRemoval.IsSynced,
                    ModifiedBy = placentaRemoval.ModifiedBy,
                    ModifiedIn = placentaRemoval.ModifiedIn,

                }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PlacentaRemoval by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all PlacentaRemoval by DeliveryId.</returns>
        public async Task<IEnumerable<PlacentaRemoval>> GetPlacentaRemovalByDelivery(Guid DeliveryId)
        {
            try
            {
                return await context.PlacentaRemovals.Include(x=>x.IdentifiedPlacentaRemovals).AsNoTracking().Where(p => p.IsDeleted == false && p.DeliveryId == DeliveryId)
            .Join(
                context.Encounters.AsNoTracking(),
                placentaRemoval => placentaRemoval.EncounterId,
                encounter => encounter.Oid,
                (placentaRemoval, encounter) => new PlacentaRemoval
                {
                    EncounterId = placentaRemoval.EncounterId,
                    EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                    CreatedIn = placentaRemoval.CreatedIn,
                    CreatedBy = placentaRemoval.CreatedBy,
                    DateCreated = placentaRemoval.DateCreated,
                    DateModified = placentaRemoval.DateModified,
                    DeliveryId = placentaRemoval.DeliveryId,
                    IsDeleted = placentaRemoval.IsDeleted,
                    InteractionId = placentaRemoval.InteractionId,
                    Other = placentaRemoval.Other,
                    EncounterType = placentaRemoval.EncounterType,
                    IsPlacentaRemovalCompleted = placentaRemoval.IsPlacentaRemovalCompleted,
                    IsSynced = placentaRemoval.IsSynced,
                    ModifiedBy = placentaRemoval.ModifiedBy,
                    ModifiedIn = placentaRemoval.ModifiedIn,
                    IdentifiedPlacentaRemovals = placentaRemoval.IdentifiedPlacentaRemovals.ToList()

                }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}