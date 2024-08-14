using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by: Phoenix(1)
 * Date created: 12.09.2022
 * Modified by: Sphinx(1)
 * Last modified: 06.11.2022
 */

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IEducationLevelRepository interface.
    /// </summary>
    public class EducationLevelRepository : Repository<EducationLevel>, IEducationLevelRepository
    {
        public EducationLevelRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the education level by education level name.
        /// </summary>
        /// <param name="educationLevelName">Name of the education level.</param>
        /// <returns>Returns the education level if the education level name is matched.</returns>
        public async Task<EducationLevel> GetEducationLevelByName(string educationLevelName)
        {
            try
            {
                return await FirstOrDefaultAsync(ed => ed.Description.ToLower().Trim() == educationLevelName.ToLower().Trim() && ed.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the education level by key.
        /// </summary>
        /// <param name="key">Primary key of the table EducationLevels.</param>
        /// <returns>Returns the education level if the key is matched.</returns>
        public async Task<EducationLevel> GetEducationLevelByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(ed => ed.Oid == key && ed.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of the education levels.
        /// </summary>
        /// <returns>Returns a list of all the education levels.</returns>
        public async Task<IEnumerable<EducationLevel>> GetEducationLevels()
        {
            try
            {
                return await QueryAsync(ed => ed.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}