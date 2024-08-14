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
    public interface IPeriuneumIntactRepository : IRepository<PerineumIntact>
    {
        /// <summary>
        /// The method is used to get a PeriuneumIntact by key.
        /// </summary>
        /// <param name="key">Primary key of the table PeriuneumIntacts.</param>
        /// <returns>Returns a PeriuneumIntact if the key is matched.</returns>
        public Task<PerineumIntact> GetPeriuneumIntactByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of PeriuneumIntacts.
        /// </summary>
        /// <returns>Returns a list of all PeriuneumIntacts.</returns>
        public Task<IEnumerable<PerineumIntact>> GetPeriuneumIntacts();


        /// <summary>
        /// The method is used to get the list of PeriuneumIntact by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all PeriuneumIntact by DeliveryId.</returns>
        public Task<IEnumerable<PerineumIntact>> GetPeriuneumIntactByDelivery(Guid DeliveryId);

        /// <summary>
        /// The method is used to get the list of PeriuneumIntact by EncounterId.
        /// </summary>
        /// <returns>Returns a list of all PeriuneumIntact by EncounterId.</returns>
        public Task<IEnumerable<PerineumIntact>> GetPeriuneumIntactByEncounter(Guid EncounterId);
    }
}