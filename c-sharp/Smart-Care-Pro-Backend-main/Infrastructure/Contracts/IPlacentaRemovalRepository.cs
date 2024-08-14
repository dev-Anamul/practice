using Domain.Entities;

/*
 * Created by   : Biplob Roy
 * Date created : 02.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPlacentaRemovalRepository : IRepository<PlacentaRemoval>
    {
        /// <summary>
        /// The method is used to get a PlacentaRemoval by key.
        /// </summary>
        /// <param name="key">Primary key of the table PlacentaRemovals.</param>
        /// <returns>Returns a PlacentaRemoval if the key is matched.</returns>
        public Task<PlacentaRemoval> GetPlacentaRemovalByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of PlacentaRemovals.
        /// </summary>
        /// <returns>Returns a list of all PlacentaRemovals.</returns>
        public Task<IEnumerable<PlacentaRemoval>> GetPlacentaRemovals();


        /// <summary>
        /// The method is used to get the list of PlacentaRemoval by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all PlacentaRemoval by DeliveryId.</returns>
        public Task<IEnumerable<PlacentaRemoval>> GetPlacentaRemovalByDelivery(Guid DeliveryId);

        /// <summary>
        /// The method is used to get the list of PlacentaRemoval by EncounterId.
        /// </summary>
        /// <returns>Returns a list of all PlacentaRemoval by EncounterId.</returns>
        public Task<IEnumerable<PlacentaRemoval>> GetPlacentaRemovalByEncounter(Guid EncounterId);
    }
}