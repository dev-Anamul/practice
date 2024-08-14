using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 16.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IVaccineTypeRepository : IRepository<VaccineType>
    {
        /// <summary>
        /// The method is used to get a vaccine type by name.
        /// </summary>
        /// <param name="vaccineType">Name of a vaccine type.</param>
        /// <returns>Returns a vaccine type if the name is matched.</returns>
        public Task<VaccineType> GetVaccineTypeByName(string vaccineType);

        /// <summary>
        /// The method is used to get a vaccine type by key.
        /// </summary>
        /// <param name="key">Primary key of the table VaccineTypes.</param>
        /// <returns>Returns a vaccine type if the key is matched.</returns>
        public Task<VaccineType> GetVaccineTypeByKey(int key);

        /// <summary>
        /// The method is used to get the list of vaccine types.
        /// </summary>
        /// <returns>Returns a list of all vaccine types.</returns>
        public Task<IEnumerable<VaccineType>> GetVaccineTypes();
    }
}