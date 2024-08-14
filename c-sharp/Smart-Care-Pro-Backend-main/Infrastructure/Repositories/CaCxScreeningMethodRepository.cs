using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Rezwana
 * Date created : 25.12.2022
 * Modified by  : Rezwana
 * Last modified: 27.12.2022
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ICaCxScreeningMethodRepository interface.
    /// </summary>
    public class CaCxScreeningMethodRepository : Repository<CaCxScreeningMethod>, ICaCxScreeningMethodRepository
    {
        public CaCxScreeningMethodRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get cacx screening method by cacx screening method name.
        /// </summary>
        /// <param name="screeningMethod">Name of cacx screening method.</param>
        /// <returns>Returns a cacx screening method if the cacx screening method name is matched.</returns>
        public async Task<CaCxScreeningMethod> GetCaCxScreeningMethodByName(string screeningMethod)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Description.ToLower().Trim() == screeningMethod.ToLower().Trim() && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a cacx screening method by key.
        /// </summary>
        /// <param name="key">Primary key of the table CaCxScreeningMethods.</param>
        /// <returns>Returns a cacx screening method if the key is matched.</returns>
        public async Task<CaCxScreeningMethod> GetCaCxScreeningMethodByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Oid == key && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of cacx screening methods.
        /// </summary>
        /// <returns>Returns a list of all cacx screening methods.</returns>
        public async Task<IEnumerable<CaCxScreeningMethod>> GetCaCxScreeningMethods()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}