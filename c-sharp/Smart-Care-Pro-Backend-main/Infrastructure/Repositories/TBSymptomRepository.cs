using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Bithy
 * Date created : 25.12.2022
 * Modified by  : Rezwana
 * Last modified: 27.12.2022
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ITBSymptomRepository interface.
    /// </summary>
    public class TBSymptomRepository : Repository<TBSymptom>, ITBSymptomRepository
    {
        public TBSymptomRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of tb symptoms.
        /// </summary>
        /// <returns>Returns a list of all tb symptoms.</returns>
        public async Task<IEnumerable<TBSymptom>> GetTBSymptoms()
        {
            try
            {
                return await QueryAsync(t => t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}