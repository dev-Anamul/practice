using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Tomas
 * Date created : 04.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IOccupationRepository interface.
    /// </summary>
    public class OccupationRepository : Repository<Occupation>, IOccupationRepository
    {
        public OccupationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an occupation by name.
        /// </summary>
        /// <param name="occupationName">Name of an occupation.</param>
        /// <returns>Returns an occupation if the occupation name is matched.</returns>
        public async Task<Occupation> GetOccupationByName(string occupationName)
        {
            try
            {
                return await FirstOrDefaultAsync(o => o.Description.ToLower().Trim() == occupationName.ToLower().Trim() && o.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an occupation by key.
        /// </summary>
        /// <param name="key">Primary key of the table Occupations.</param>
        /// <returns>Returns an occupation if the key is matched.</returns>
        public async Task<Occupation> GetOccupationByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(o => o.Oid == key && o.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of occupations.
        /// </summary>
        /// <returns>Returns a list of all occupations.</returns>
        public async Task<IEnumerable<Occupation>> GetOccupations()
        {
            try
            {
                return await QueryAsync(o => o.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}