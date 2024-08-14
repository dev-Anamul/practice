using Domain.Entities;
using Infrastructure.Contracts;


namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IStoppingReasonRepository interface.
    /// </summary>
    public class RiskRepository : Repository<Risks>,IRiskRepository
    {
        public RiskRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an Risk by key.
        /// </summary>
        /// <param name="key">Primary key of the table Risk.</param>
        /// <returns>Returns an Risk if the key is matched.</returns>
        public async Task<Risks> GetRiskByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(n => n.Oid == key && n.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get the list of Risk.
        /// </summary>
        /// <returns>Returns a list of all Risk.</returns>
        public async Task<IEnumerable<Risks>> GetRisk()
        {
            try
            {
                return await QueryAsync(n => n.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get an Risk by Risk Description.
        /// </summary>
        /// <param name="risk">Name of a Risk .</param>
        /// <returns>Returns an Risk if the Risk is matched.</returns>
        public async Task<Risks> GetRiskByName(string risk)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == risk.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
