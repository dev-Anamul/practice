using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Rezwana
 * Date created : 13.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IComplicationTypeRepository interface.
    /// </summary>
    public class ComplicationTypeRepository : Repository<ComplicationType>, IComplicationTypeRepository
    {
        public ComplicationTypeRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a complication type by description.
        /// </summary>
        /// <param name="description">Description of ComplicationType.</param>
        /// <returns>Returns a complication type if the description is matched.</returns>
        public async Task<ComplicationType> GetComplicationTypeByDescription(string description)
        {
            try
            {
                return await FirstOrDefaultAsync(h => h.Description.ToLower().Trim() == description.ToLower().Trim() && h.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a complication type by key.
        /// </summary>
        /// <param name="key">Primary key of the table ComplicationTypes.</param>
        /// <returns>Returns a complication type if the key is matched.</returns>
        public async Task<ComplicationType> GetComplicationTypeByKey(int key)
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
        /// The method is used to get the list of complication types.
        /// </summary>
        /// <returns>Returns a list of all complication types.</returns>
        public async Task<IEnumerable<ComplicationType>> GetComplicationTypes()
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