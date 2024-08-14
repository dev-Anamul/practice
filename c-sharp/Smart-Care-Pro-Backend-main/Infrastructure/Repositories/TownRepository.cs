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
    /// Implementation of ITownRepository interface.
    /// </summary>
    public class TownRepository : Repository<Town>, ITownRepository
    {

        public TownRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a town by name.
        /// </summary>
        /// <param name="townName">Name of a town.</param>
        /// <returns>Returns a town if the town name is matched.</returns>
        public async Task<Town> GetTownByName(string townName)
        {
            try
            {
                return await FirstOrDefaultAsync(t => t.Description.ToLower().Trim() == townName.ToLower().Trim() && t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a town by key.
        /// </summary>
        /// <param name="key">Primary key of the table Towns.</param>
        /// <returns>Returns a town if the key is matched.</returns>
        public async Task<Town> GetTownByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(t => t.Oid == key && t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a town by DistrictID.
        /// </summary>
        /// <param name="DistrictID">DistrictID of the table Towns.</param>
        /// <returns>Returns a town if the DistrictID is matched.</returns>
        public async Task<IEnumerable<Town>> GetTownByDistrict(int DistrictID)
        {
            try
            {
                //return await QueryAsync(d => d.IsDeleted == false && d.DistrictID == DistrictID);
                return await LoadListWithChildAsync<Town>(d => d.IsDeleted == false && d.DistrictId == DistrictID, d => d.District,p=>p.District.Provinces);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of towns.
        /// </summary>
        /// <returns>Returns a list of all towns.</returns>
        public async Task<IEnumerable<Town>> GetTowns()
        {
            try
            {
                return await QueryAsync(t => t.IsDeleted == false,d=>d.District,p=>p.District.Provinces);
            }
            catch (Exception)
            {
                throw;
            }
        }        
    }
}