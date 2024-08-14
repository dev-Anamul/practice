using Domain.Entities;
using Infrastructure.Contracts;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IStoppingReasonRepository interface.
    /// </summary>
    public class StoppingReasonRepository : Repository<StoppingReason>, IStoppingReasonRepository
    {
        public StoppingReasonRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an Stopping Reason by key.
        /// </summary>
        /// <param name="key">Primary key of the table StoppingReason.</param>
        /// <returns>Returns an Stopping Reason if the key is matched.</returns>
        public async Task<StoppingReason> GetStoppingReasonByKey(int key)
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
        /// The method is used to get the list of Stopping Reason.
        /// </summary>
        /// <returns>Returns a list of all Stopping Reason.</returns>
        public async Task<IEnumerable<StoppingReason>> GetStoppingReason()
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
        /// The method is used to get an Stopping Reason by Stopping Reason Description.
        /// </summary>
        /// <param name="stoppingReason">Name of a StoppingReason.</param>
        /// <returns>Returns an StoppingReason if the Stopping Reason description is matched.</returns>
        public async Task<StoppingReason> GetStoppingReasonByName(string stoppingReason)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == stoppingReason.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
