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
    public interface IDrugRouteRepository : IRepository<DrugRoute>
    {
        /// <summary>
        /// The method is used to get the list of DrugRoutes  .
        /// </summary>
        /// <returns>Returns a list of all DrugRoutes.</returns>
        public Task<IEnumerable<DrugRoute>> GetDrugRoutes();

        /// <summary>
        /// The method is used to get a DrugRoute by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugRoutes.</param>
        /// <returns>Returns a DrugRoute if the key is matched.</returns>
        public Task<DrugRoute> GetDrugRouteByKey(int key);

        /// <summary>
        /// The method is used to get an DrugRoute by DrugRoute Description.
        /// </summary>
        /// <param name="drugRoute">Description of an DrugRoute.</param>
        /// <returns>Returns an DrugRoute if the DrugRoute name is matched.</returns>
        public Task<DrugRoute> GetDrugRouteByName(string drugRoute);
    }
}