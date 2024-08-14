using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IUterusConditionRepository : IRepository<UterusCondition>
    {
        /// <summary>
        /// The method is used to get a UterusCondition by key.
        /// </summary>
        /// <param name="key">Primary key of the table UterusConditions.</param>
        /// <returns>Returns a UterusCondition if the key is matched.</returns>
        public Task<UterusCondition> GetUterusConditionByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of UterusConditions.
        /// </summary>
        /// <returns>Returns a list of all UterusConditions.</returns>
        public Task<IEnumerable<UterusCondition>> GetUterusConditions();


        /// <summary>
        /// The method is used to get the list of UterusCondition by DeliveryId.
        /// </summary>
        /// <param name="DeliveryId">primary key of the table MotherDelivery.</param>
        /// <returns>Returns a list of all UterusCondition by DeliveryId.</returns>
        public Task<IEnumerable<UterusCondition>> GetUterusConditionByDelivery(Guid deliveryId);

        /// <summary>
        /// The method is used to get the list of UterusCondition by EncounterId.
        /// </summary>
        /// <param name="EncounterId">EncounterId of the table UterusConditions.</param>
        /// <returns>Returns a list of all UterusCondition by EncounterId.</returns>
        public Task<IEnumerable<UterusCondition>> GetUterusConditionByEncounter(Guid EncounterId);
    }
}