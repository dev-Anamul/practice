using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Biplob Roy
 * Date created : 08.04.2023
 * Modified by  : Biplob Roy
 * Last modified: 27.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class CircumcisionReasonRepository : Repository<CircumcisionReason>, ICircumcisionReasonRepository
    {
        public CircumcisionReasonRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get CircumcisionReason by key.
        /// </summary>
        /// <param name="key">Primary key of the table CircumcisionReasons.</param>
        /// <returns>Returns a CircumcisionReason if the key is matched.</returns>
        public async Task<CircumcisionReason> GetCircumcisionReasonByKey(int key)
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
        /// The method is used to get the list of Circumcision Reason.
        /// </summary>
        /// <returns>Returns a list of all Circumcision Reason.</returns>
        public async Task<IEnumerable<CircumcisionReason>> GetCircumcisionReasons()
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
        /// The method is used to get an circumcisionReason by CircumcisionReason Description.
        /// </summary>
        /// <param name="description">Name of an CircumcisionReason.</param>
        /// <returns>Returns an CircumcisionReason if the CircumcisionReason description is matched.</returns>
        public async Task<CircumcisionReason> GetCircumcisionReasonByName(string circumcisionReason)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == circumcisionReason.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}