using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Bithy
 * Date created : 03.05.2023
 * Modified by  : Biplob Roy
 * Last modified: 03.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ISTIRiskRepository interface.
    /// </summary>
    public class STIRiskRepository : Repository<STIRisk>, ISTIRiskRepository
    {
        public STIRiskRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of STIRisks.
        /// </summary>
        /// <returns>Returns a list of all STIRisks.</returns>
        public async Task<IEnumerable<STIRisk>> GetSTIRisks()
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
        /// The method is used to get a STIRisk by key.
        /// </summary>
        /// <param name="key">Primary key of the table STIRisks.</param>
        /// <returns>Returns a STIRisk  if the key is matched.</returns>
        public async Task<STIRisk> GetSTIRiskByKey(int key)
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
        /// The method is used to get an STIRisk by STIRisk Description.
        /// </summary>
        /// <param name="description">Name of an STIRisk.</param>
        /// <returns>Returns an STIRisk if the STIRisk description is matched.</returns>
        public async Task<STIRisk> GetSTIRiskByName(string sTIRisk)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == sTIRisk.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}