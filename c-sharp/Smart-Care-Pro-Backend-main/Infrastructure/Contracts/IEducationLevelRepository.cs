using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 11.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */

namespace Infrastructure.Contracts
{
    public interface IEducationLevelRepository : IRepository<EducationLevel>
    {
        /// <summary>
        /// The method is used to get the education level by education level name.
        /// </summary>
        /// <param name="educationLevelName">Name of the education level.</param>
        /// <returns>Returns the education level if the education level name is matched.</returns>
        public Task<EducationLevel> GetEducationLevelByName(string educationLevelName);

        /// <summary>
        /// The method is used to get the education level by key.
        /// </summary>
        /// <param name="key">Primary key of the table EducationLevels.</param>
        /// <returns>Returns the education level if the key is matched.</returns>
        public Task<EducationLevel> GetEducationLevelByKey(int key);

        /// <summary>
        /// The method is used to get the list of the education levels.
        /// </summary>
        /// <returns>Returns a list of all the education levels.</returns>
        public Task<IEnumerable<EducationLevel>> GetEducationLevels();
    }
}