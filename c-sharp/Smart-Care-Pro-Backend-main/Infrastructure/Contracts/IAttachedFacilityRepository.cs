using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 15-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IAttachedFacilityRepository : IRepository<AttachedFacility>
    {
        /// <summary>
        /// The method is used to get a AttachedFacility by key.
        /// </summary>
        /// <param name="key">Primary key of the table AttachedFacility.</param>
        /// <returns>Returns a AttachedFacility if the key is matched.</returns>
        public Task<AttachedFacility> GetAttachedFacilityByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of AttachedFacility  .
        /// </summary>
        /// <returns>Returns a list of all AttachedFacility.</returns>
        public Task<IEnumerable<AttachedFacility>> GetAttachedFacility();

        /// <summary>
        /// The method is used to get the list of AttachedFacility by client.
        /// </summary>
        /// <returns>Returns a list of all AttachedFacility by client.</returns>
        public Task<IEnumerable<AttachedFacility>> GetAttachedFacilityByClient(Guid clientId);

        /// <summary>
        /// The method is used to get a AttachedFacility by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a AttachedFacility if the Encounter is matched.</returns>
        public Task<IEnumerable<AttachedFacility>> GetAttachedFacilityByEncounterId(Guid encounterId);
    }
}