using Domain.Entities;

/*
 * Created by: Lion
 * Date created: 12.09.2022
 * Modified by: Bella
 * Last modified: 06.11.2022
 */

namespace Infrastructure.Contracts
{
    public interface ICaregiverRepository : IRepository<Caregiver>
    {
        /// <summary>
        /// The method is used to get a caregiver by name.
        /// </summary>
        /// <param name="caregiverName">Name of a caregiver.</param>
        /// <returns>Returns a caregiver if the name is matched.</returns>
        public Task<Caregiver> GetCaregiverByName(string caregiverName);

        /// <summary>
        /// The method is used to get a caregiver by key.
        /// </summary>
        /// <param name="key">Primary key of the table Caregivers.</param>
        /// <returns>Returns a caregiver if the key is matched.</returns>
        public Task<Caregiver> GetCaregiverByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of caregivers.
        /// </summary>
        /// <returns>Returns a list of all caregivers.</returns>
        public Task<IEnumerable<Caregiver>> GetCaregivers();
    }
}