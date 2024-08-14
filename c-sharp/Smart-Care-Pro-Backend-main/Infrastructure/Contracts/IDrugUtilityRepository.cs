using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 11.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IDrugUtilityRepository : IRepository<DrugUtility>
    {
        /// <summary>
        /// The method is used to get a DrugUtility by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugUtility.</param>
        /// <returns>Returns a DrugUtility if the key is matched.</returns>
        public Task<DrugUtility> GetDrugUtilityByKey(int key);

        /// <summary>
        /// The method is used to get the list of DrugUtility  .
        /// </summary>
        /// <returns>Returns a list of all DrugUtility.</returns>
        public Task<IEnumerable<DrugUtility>> GetDrugUtility();

        /// <summary>
        /// The method is used to get an DrugUtility by DrugUtility Description.
        /// </summary>
        /// <param name="drugUtility">Description of an DrugUtility.</param>
        /// <returns>Returns an DrugUtility if the DrugUtility name is matched.</returns>
        public Task<DrugUtility> GetDrugUtilityByName(string drugUtility);
    }
}
