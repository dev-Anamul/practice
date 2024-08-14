using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Rezwana
 * Date created : 19.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IPEPRiskRepository interface.
    /// </summary>
    public class PEPRiskRepository : Repository<Risks>, IPEPRiskRepository
    {
        public PEPRiskRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a PEP risk by name.
        /// </summary>
        /// <param name="pEPRisk">Name of a PEP risk.</param>
        /// <returns>Returns a PEP risk if the name is matched.</returns>
        public async Task<Risks> GetPEPRiskByName(string pEPRisk)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.Description.ToLower().Trim() == pEPRisk.ToLower().Trim() && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a PEP risk by key.
        /// </summary>
        /// <param name="key">Primary key of the table PEPRisks.</param>
        /// <returns>Returns a PEP risk if the key is matched.</returns>
        public async Task<Risks> GetPEPRiskByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.Oid == key && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PEP risks.
        /// </summary>
        /// <returns>Returns a list of all PEP risks.</returns>
        public async Task<IEnumerable<Risks>> GetPEPRisks()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}