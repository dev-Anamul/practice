using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Biplob Roy
 * Date created : 19.04.2023
 * Modified by  : Biplob Roy
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class PriorSensitizationRepository : Repository<PriorSensitization>, IPriorSensitizationRepository
    {
        public PriorSensitizationRepository(DataContext context) : base(context)
        {
            
        }

        /// <summary>
        /// The method is used to get the list of PriorSensitizations.
        /// </summary>
        /// <returns>Returns a list of all PriorSensitization.</returns>
        public async Task<IEnumerable<PriorSensitization>> GetPriorSensitizations()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a DrugRoute by key.
        /// </summary>
        /// <param name="key">Primary key of the table PriorSensitizations.</param>
        /// <returns>Returns a PriorSensitization  if the key is matched.</returns>
        public async Task<PriorSensitization> GetPriorSensitizationByKey(int key)
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
        /// The method is used to get an PriorSensitization by PriorSensitization Description.
        /// </summary>
        /// <param name="description">Name of an PriorSensitization.</param>
        /// <returns>Returns an PriorSensitization if the PriorSensitization description is matched.</returns>
        public async Task<PriorSensitization> GetPriorSensitizationByName(string priorSensitization)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == priorSensitization.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}