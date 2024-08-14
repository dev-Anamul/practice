using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by: Phoenix(1)
 * Date created: 12.09.2022
 * Modified by: Sphinx(1)
 * Last modified: 06.11.2022
 */

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ICaregiverRepository interface.
    /// </summary>
    public class CaregiverRepository : Repository<Caregiver>, ICaregiverRepository
    {
        public CaregiverRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get caregiver by name.
        /// </summary>
        /// <param name="caregiverName">Name of a caregiver.</param>
        /// <returns>Returns a caregiver if the name is matched.</returns>
        public async Task<Caregiver> GetCaregiverByName(string caregiverName)
        {
            try
            {
                return await LoadWithChildAsync<Caregiver>(c => c.FirstName.ToLower().Trim() == caregiverName.ToLower().Trim() && c.IsDeleted == false, c => c.Client);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get caregiver by key.
        /// </summary>
        /// <param name="key">Primary key of the table Caregivers.</param>
        /// <returns>Returns a caregiver if the key is matched.</returns>
        public async Task<Caregiver> GetCaregiverByKey(Guid key)
        {
            try
            {
                return await LoadWithChildAsync<Caregiver>(c => c.Oid == key && c.IsDeleted == false, c => c.Client);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of caregivers.
        /// </summary>
        /// <returns>Returns a list of all caregivers.</returns>
        public async Task<IEnumerable<Caregiver>> GetCaregivers()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false, c => c.Client);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}