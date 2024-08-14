using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by: Phoenix(1)
 * Date created: 17.09.2022
 * Modified by: Sphinx(1)
 * Last modified: 06.11.2022
 */

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IProvinceRepository interface.
    /// </summary>
    public class ProvinceRepository : Repository<Province>, IProvinceRepository
    {
        public ProvinceRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a province by name.
        /// </summary>
        /// <param name="provinceName">Name of a province.</param>
        /// <returns>Returns a province if the province name is matched.</returns>
        public async Task<Province> GetProvinceByName(string provinceName)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.Description.ToLower().Trim() == provinceName.ToLower().Trim() && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a province by key.
        /// </summary>
        /// <param name="key">Primary key of the table Provinces.</param>
        /// <returns>Returns a province if the key is matched.</returns>
        public async Task<Province> GetProvinceByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.Oid == key && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of provinces.
        /// </summary>
        /// <returns>Returns a list of all provinces.</returns>
        public async Task<IEnumerable<Province>> GetProvinces()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}