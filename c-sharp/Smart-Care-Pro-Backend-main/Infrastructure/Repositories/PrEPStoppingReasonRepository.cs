using Domain.Entities;
using Infrastructure.Contracts;

namespace Infrastructure.Repositories
{
    public class PrEPStoppingReasonRepository : Repository<StoppingReason>, IPrEPStoppingReasonRepository
    {
        public PrEPStoppingReasonRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get PrEPStoppingReason by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Returns a PrEPStoppingReason if the key is matched.</returns>
        public async Task<StoppingReason> GetPrEPStoppingReasonByKey(int key)
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
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public async Task<IEnumerable<StoppingReason>> GetPrEPStoppingReasons()
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

        /// <summary>
        /// The method is used to get a prepStoppingReason by prepStoppingReason name.
        /// </summary>
        /// <param name="prepStoppingReason">Name of a prepStoppingReason.</param>
        /// <returns>Returns a county if the prepStoppingReason name is matched.</returns>
        public async Task<StoppingReason> GetPrEPStoppingReasonByName(string prepStoppingReason)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.Description.ToLower().Trim() == prepStoppingReason.ToLower().Trim() && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}