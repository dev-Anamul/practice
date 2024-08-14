using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface INewBornDetailRepository : IRepository<NewBornDetail>
    {
        /// <summary>
        /// The method is used to get a NewBornDetail by key.
        /// </summary>
        /// <param name="key">Primary key of the table NewBornDetails.</param>
        /// <returns>Returns a NewBornDetail if the key is matched.</returns>
        public Task<NewBornDetail> GetNewBornDetailByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of NewBornDetails.
        /// </summary>
        /// <returns>Returns a list of all NewBornDetails.</returns>
        public Task<IEnumerable<NewBornDetail>> GetNewBornDetails();

        /// <summary>
        /// The method is used to get the list of NewBornDetail by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all NewBornDetail by DeliveryId.</returns>
        public Task<IEnumerable<NewBornDetail>> GetNewBornDetailByDelivery(Guid deliveryId);

        /// <summary>
        /// The method is used to get the list of NewBornDetail by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all NewBornDetail by EncounterID.</returns>
        public Task<IEnumerable<NewBornDetail>> GetNewBornDetailByEncounter(Guid encounterId);
    }
}