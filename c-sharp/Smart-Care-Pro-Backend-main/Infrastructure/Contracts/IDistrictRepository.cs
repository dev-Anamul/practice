using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */

namespace Infrastructure.Contracts
{
    public interface IDistrictRepository : IRepository<District>
    {
        /// <summary>
        /// The method is used to get a district by district name.
        /// </summary>
        /// <param name="districtName">Name of a district.</param>
        /// <returns>Returns a facility if the facility name is matched.</returns>
        public Task<District> GetDistrictByName(string districtName);

        /// <summary>
        /// The method is used to get a district by key.
        /// </summary>
        /// <param name="key">Primary key of the table Districts.</param>
        /// <returns>Returns a district if the key is matched.</returns>
        public Task<District> GetDistrictByKey(int key);

        /// <summary>
        /// The method is used to get the districts by ProvinceID.
        /// </summary>
        /// <param name="ProvinceId">PovinceID of the table Districts.</param>
        /// <returns>Returns a district if the ProvinceID is matched.</returns>
        public Task<IEnumerable<District>> GetDistrictByProvince(int ProvinceId);


        /// <summary>
        /// The method is used to get the list of districts.
        /// </summary>
        /// <returns>Returns a list of all districts.</returns>
        public Task<IEnumerable<District>> GetDistricts();
        public Task<IEnumerable<District>> GetDistrictsWithOutProvince();
    }
}