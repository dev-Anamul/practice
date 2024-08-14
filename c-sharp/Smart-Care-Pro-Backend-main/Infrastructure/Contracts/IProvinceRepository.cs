using Domain.Entities;

/*
 * Created by: Phoenix(1)
 * Date created: 17.09.2022
 * Modified by: Sphinx(1)
 * Last modified: 06.11.2022
 */
namespace Infrastructure.Contracts
{
    public interface IProvinceRepository : IRepository<Province>
    {
        /// <summary>
        /// The method is used to get a province by name.
        /// </summary>
        /// <param name="provinceName">Name of a province.</param>
        /// <returns>Returns a province if the province name is matched.</returns>
        public Task<Province> GetProvinceByName(string provinceName);

        /// <summary>
        /// The method is used to get a province by key.
        /// </summary>
        /// <param name="key">Primary key of the table Provinces.</param>
        /// <returns>Returns a province if the key is matched.</returns>
        public Task<Province> GetProvinceByKey(int key);

        /// <summary>
        /// The method is used to get the list of provinces.
        /// </summary>
        /// <returns>Returns a list of all provinces.</returns>
        public Task<IEnumerable<Province>> GetProvinces();
    }
}