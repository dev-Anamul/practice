using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Biplob Roy
 * Date created : 06.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class TBSuspectingReasonRepository : Repository<TBSuspectingReason>, ITBSuspectingReasonRepository
    {
        public TBSuspectingReasonRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a TBSuspectingReason by key.
        /// </summary>
        /// <param name="key">Primary key of the table TBSuspectingReasons.</param>
        /// <returns>Returns a TBSuspectingReason  if the key is matched.</returns>
        public async Task<TBSuspectingReason> GetTBSuspectingReasonByKey(int key)
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
        /// The method is used to get the list of TBSuspectingReason.
        /// </summary>
        /// <returns>Returns a list of all covid TBSuspectingReasons.</returns>
        public async Task<IEnumerable<TBSuspectingReason>> GetTBSuspectingReasons()
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
    }
}
