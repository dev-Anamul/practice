using Domain.Entities;

/*
 * Created by   : Rezwana
 * Date created : 23.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPathologyAxisRepository : IRepository<PathologyAxis>
    {
        /// <summary>
        /// The method is used to get a pathology axis by pathology axis name.
        /// </summary>
        /// <param name="pathologyAxis">Name of a pathology axis.</param>
        /// <returns>Returns a pathology axis if the pathology axis name is matched.</returns>
        public Task<PathologyAxis> GetPathologyAxisByName(string pathologyAxis);

        /// <summary>
        /// The method is used to get a pathology axis by key.
        /// </summary>
        /// <param name="key">Primary key of the table PathologyAxes.</param>
        /// <returns>Returns a pathology axis if the key is matched.</returns>
        public Task<PathologyAxis> GetPathologyAxisByKey(int key);

        /// <summary>
        /// The method is used to get the list of pathology axes.
        /// </summary>
        /// <returns>Returns a list of all pathology axes.</returns>
        public Task<IEnumerable<PathologyAxis>> GetPathologyAxes();
    }
}