using Domain.Entities;

/*
 * Created by    : Stephan
 * Date created  : 07.02.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Contracts
{
    public interface ITownRepository : IRepository<Town>
    {
        /// <summary>
        /// The method is used to get a town by name.
        /// </summary>
        /// <param name="townName">Name of a town.</param>
        /// <returns>Returns a town if the town name is matched.</returns>
        public Task<Town> GetTownByName(string townName);

        /// <summary>
        /// The method is used to get a town by key.
        /// </summary>
        /// <param name="key">Primary key of the table Towns.</param>
        /// <returns>Returns a town if the key is matched.</returns>
        public Task<Town> GetTownByKey(int key);

        /// <summary>
        /// The method is used to get a town by DistrictID.
        /// </summary>
        /// <param name="DistrictID">DistrictID of the table Towns.</param>
        /// <returns>Returns a town if the DistrictID is matched.</returns>
        public Task<IEnumerable<Town>> GetTownByDistrict(int DistrictID);

        /// <summary>
        /// The method is used to get the list of towns.
        /// </summary>
        /// <returns>Returns a list of all towns.</returns>
        public Task<IEnumerable<Town>> GetTowns();
        
    }
}