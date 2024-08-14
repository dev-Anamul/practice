using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Rezwana
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ITBDrugRepository interface.
    /// </summary>
    public class TBDrugRepository : Repository<TBDrug>, ITBDrugRepository
    {
        public TBDrugRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a TB drug by drug name.
        /// </summary>
        /// <param name="drugName">Name of a TB drug.</param>
        /// <returns>Returns a TB drug if the drug name is matched.</returns>
        public async Task<TBDrug> GetTBDrugByName(string drugName)
        {
            try
            {
                return await FirstOrDefaultAsync(t => t.Description.ToLower().Trim() == drugName.ToLower().Trim() && t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a TB drug by key.
        /// </summary>
        /// <param name="key">Primary key of the table TBDrugs.</param>
        /// <returns>Returns a TB drug if the key is matched.</returns>
        public async Task<TBDrug> GetTBDrugByKey(int key)
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
        /// The method is used to get the list of TB drugs.
        /// </summary>
        /// <returns>Returns a list of all TB drugs.</returns>
        public async Task<IEnumerable<TBDrug>> GetTBDrugs()
        {
            try
            {
                return await QueryAsync(t => t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}