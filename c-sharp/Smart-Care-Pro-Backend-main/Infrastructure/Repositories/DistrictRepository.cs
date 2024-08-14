using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

/*
 * Created by: Phoenix(1)
 * Date created: 12.09.2022
 * Modified by: Sphinx(1)
 * Last modified: 06.11.2022
 */

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IDistrictRepository interface.
    /// </summary>
    public class DistrictRepository : Repository<District>, IDistrictRepository
    {
        public DistrictRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a district by district name.
        /// </summary>
        /// <param name="districtName">Name of a district.</param>
        /// <returns>Returns a district if the district name is matched.</returns>
        public async Task<District> GetDistrictByName(string districtName)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.Description.ToLower().Trim() == districtName.ToLower().Trim() && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a district by key.
        /// </summary>
        /// <param name="key">Primary key of the table Districts.</param>
        /// <returns>Returns a district if the key is matched.</returns>
        public async Task<District> GetDistrictByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.Oid == key && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the districts by ProvinceID.
        /// </summary>
        /// <param name="ProvinceId">PovinceID of the table Districts.</param>
        /// <returns>Returns a district if the ProvinceID is matched.</returns>
        public async Task<IEnumerable<District>> GetDistrictByProvince(int ProvinceId)
        {
            try
            {
                var districts = await QueryAsync(d => d.IsDeleted == false && d.ProvinceId == ProvinceId,p=>p.Provinces);

                return districts.OrderBy(d => d.Description);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get the list of districts.
        /// </summary>
        /// <returns>Returns a list of all districts.</returns>
        public async Task<IEnumerable<District>> GetDistricts()
        {
            try
            {
                return await QueryAsync(d => d.IsDeleted == false,p => p.Provinces);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of districts withOut Including Province.
        /// </summary>
        /// <returns>Returns a list of all districts.</returns>
        public async Task<IEnumerable<District>> GetDistrictsWithOutProvince()
        {
            try
            {
                return await QueryAsync(d => d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}