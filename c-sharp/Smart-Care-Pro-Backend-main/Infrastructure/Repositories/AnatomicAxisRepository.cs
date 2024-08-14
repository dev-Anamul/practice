using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Lion 
 * Date created : 23.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IAnatomicAxisRepository interface.
    /// </summary>
    public class AnatomicAxisRepository : Repository<AnatomicAxis>, IAnatomicAxisRepository
    {
        public AnatomicAxisRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an anatomic axis by anatomic axis name.
        /// </summary>
        /// <param name="anatomicAxis">Name of an anatomic axis.</param>
        /// <returns>Returns an anatomic axis if the anatomic axis name is matched.</returns>
        public async Task<AnatomicAxis> GetAnatomicAxisByName(string anatomicAxis)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == anatomicAxis.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an anatomic axis by key.
        /// </summary>
        /// <param name="key">Primary key of the table AnatomicAxes.</param>
        /// <returns>Returns an anatomic axis if the key is matched.</returns>
        public async Task<AnatomicAxis> GetAnatomicAxisByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Oid == key && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of anatomic axes.
        /// </summary>
        /// <returns>Returns a list of all anatomic axes.</returns>
        public async Task<IEnumerable<AnatomicAxis>> GetAnatomicAxes()
        {
            try
            {
                return await QueryAsync(a => a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}