using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Rezwana
 * Date created : 23.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IPathologyAxisRepository interface.
    /// </summary>
    public class PathologyAxisRepository : Repository<PathologyAxis>, IPathologyAxisRepository
    {
        public PathologyAxisRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a pathology axis by pathology axis name.
        /// </summary>
        /// <param name="pathologyAxis">Name of a pathology axis.</param>
        /// <returns>Returns a pathology axis if the pathology axis name is matched.</returns>
        public async Task<PathologyAxis> GetPathologyAxisByName(string pathologyAxis)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.Description.ToLower().Trim() == pathologyAxis.ToLower().Trim() && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a pathology axis by key.
        /// </summary>
        /// <param name="key">Primary key of the table PathologyAxes.</param>
        /// <returns>Returns a pathology axis if the key is matched.</returns>
        public async Task<PathologyAxis> GetPathologyAxisByKey(int key)
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
        /// The method is used to get the list of pathology axes.
        /// </summary>
        /// <returns>Returns a list of all pathology axes.</returns>
        public async Task<IEnumerable<PathologyAxis>> GetPathologyAxes()
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
    }
}