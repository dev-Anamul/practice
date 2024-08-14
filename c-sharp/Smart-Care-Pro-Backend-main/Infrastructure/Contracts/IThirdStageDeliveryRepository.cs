using Domain.Entities;

/*
 * Created by    : Stephan
 * Date created  : 07.02.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Contracts
{
    public interface IThirdStageDeliveryRepository : IRepository<ThirdStageDelivery>
    {
        /// <summary>
        /// The method is used to get a ThirdStageDelivery by key.
        /// </summary>
        /// <param name="key">Primary key of the table ThirdStageDeliverys.</param>
        /// <returns>Returns a ThirdStageDelivery if the key is matched.</returns>
        public Task<ThirdStageDelivery> GetThirdStageDeliveryByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of ThirdStageDeliverys.
        /// </summary>
        /// <returns>Returns a list of all ThirdStageDeliverys.</returns>
        public Task<IEnumerable<ThirdStageDelivery>> GetThirdStageDeliveries();


        /// <summary>
        /// The method is used to get the list of ThirdStageDelivery by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all ThirdStageDelivery by DeliveryId.</returns>
        public Task<IEnumerable<ThirdStageDelivery>> GetThirdStageDeliveryByDelivery(Guid DeliveryId);

    }
}