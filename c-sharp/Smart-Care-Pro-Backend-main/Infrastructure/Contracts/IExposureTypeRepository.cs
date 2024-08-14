using Domain.Entities;

/*
 * Created by    : Stephan
 * Date created  : 18.02.2023
 * Modified by   : Stephan
 * Last modified : 05.06.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Contracts
{
    public interface IExposureTypeRepository : IRepository<ExposureType>
    {
        /// <summary>
        /// The method is used to get an exposure type by type.
        /// </summary>
        /// <param name="exposureType">Type of an exposure type.</param>
        /// <returns>Returns an exposure type if the type is matched.</returns>
        public Task<ExposureType> GetExposureTypeByType(string exposureType);

        /// <summary>
        /// The method is used to get an exposure type by key.
        /// </summary>
        /// <param name="key">Primary key of the table ExposureTypes.</param>
        /// <returns>Returns an exposure type if the key is matched.</returns>
        public Task<ExposureType> GetExposureTypeByKey(int key);

        /// <summary>
        /// The method is used to get the list of exposure types.
        /// </summary>
        /// <returns>Returns a list of all exposure types.</returns>
        public Task<IEnumerable<ExposureType>> GetExposureTypes();

        
    }
}