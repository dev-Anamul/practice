using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Tariqul Islam
 * Date created : 06.03.2023
 * Modified by  : Biplob Roy
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IDrugRouteRepository interface.
    /// </summary>
    public class DrugRouteRepository : Repository<DrugRoute>, IDrugRouteRepository
    {
        public DrugRouteRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of DrugRoute  .
        /// </summary>
        /// <returns>Returns a list of all DrugRoute.</returns>
        public async Task<IEnumerable<DrugRoute>> GetDrugRoutes()
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

        /// <summary>
        /// The method is used to get a DrugRoute by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugRoutes.</param>
        /// <returns>Returns a DrugRoute  if the key is matched.</returns>
        public async Task<DrugRoute> GetDrugRouteByKey(int key)
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
        /// The method is used to get an DrugRoute by DrugRoute Description.
        /// </summary>
        /// <param name="description">Name of an DrugRoute.</param>
        /// <returns>Returns an DrugRoute if the DrugRoute description is matched.</returns>
        public async Task<DrugRoute> GetDrugRouteByName(string drugRoute)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == drugRoute.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}