using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : Bella
 * Last modified: 27.12.2022
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IAllergicDrugRepository interface.
    /// </summary>
    public class AllergicDrugRepository : Repository<AllergicDrug>, IAllergicDrugRepository
    {
        public AllergicDrugRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a Allergic Drug by key.
        /// </summary>
        /// <param name="key">Primary key of the table AllergicDrugs.</param>
        /// <returns>Returns a Allergic Drug if the key is matched.</returns>
        public async Task<AllergicDrug> GetAllergicDrugByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(b => b.Oid == key && b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of allergic drugs.
        /// </summary>
        /// <returns>Returns a list of all allergic drugs.</returns>
        public async Task<IEnumerable<AllergicDrug>> GetAllergicDrugs()
        {
            try
            {
                return await QueryAsync(a => a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an AllergicDrug by AllergicDrug DrugType.
        /// </summary>
        /// <param name="drugtype">Name of an AllergicDrug.</param>
        /// <returns>Returns an AllergicDrug if the drugtype name is matched.</returns>
        public async Task<AllergicDrug> GetAllergicDrugByName(string allergicDrug)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == allergicDrug.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}