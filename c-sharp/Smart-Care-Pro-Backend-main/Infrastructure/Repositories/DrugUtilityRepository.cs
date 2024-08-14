using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Tariqul Islam
 * Date created : 13-03-2023
 * Modified by  : Biplob Roy
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class DrugUtilityRepository : Repository<DrugUtility>, IDrugUtilityRepository
    {
        public DrugUtilityRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a DrugUtility by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugUtility.</param>
        /// <returns>Returns a DrugUtility if the key is matched.</returns>
        public async Task<DrugUtility> GetDrugUtilityByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(s => s.Oid == key && s.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of DrugUtility  .
        /// </summary>
        /// <returns>Returns a list of all DrugUtility.</returns>        
        public async Task<IEnumerable<DrugUtility>> GetDrugUtility()
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
        /// The method is used to get an DrugUtility by DrugUtility Description.
        /// </summary>
        /// <param name="description">Name of an DrugUtility.</param>
        /// <returns>Returns an DrugUtility if the DrugUtility description is matched.</returns>
        public async Task<DrugUtility> GetDrugUtilityByName(string drugUtility)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == drugUtility.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}