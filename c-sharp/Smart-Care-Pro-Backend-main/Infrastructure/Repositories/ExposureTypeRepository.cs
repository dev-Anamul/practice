using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Rezwana
 * Date created : 19.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IExposureTypeRepository interface.
    /// </summary>
    public class ExposureTypeRepository : Repository<ExposureType>, IExposureTypeRepository
    {
        public ExposureTypeRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an exposure type by type.
        /// </summary>
        /// <param name="exposureType">Type of an exposure type.</param>
        /// <returns>Returns an exposure type if the type is matched.</returns>
        public async Task<ExposureType> GetExposureTypeByType(string exposureType)
        {
            try
            {
                return await FirstOrDefaultAsync(e => e.Description.ToLower().Trim() == exposureType.ToLower().Trim() && e.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an exposure type by key.
        /// </summary>
        /// <param name="key">Primary key of the table ExposureTypes.</param>
        /// <returns>Returns an exposure type if the key is matched.</returns>
        public async Task<ExposureType> GetExposureTypeByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(e => e.Oid == key && e.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of exposure types.
        /// </summary>
        /// <returns>Returns a list of all exposure types.</returns>
        public async Task<IEnumerable<ExposureType>> GetExposureTypes()
        {
            try
            {
                return await QueryAsync(e => e.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}