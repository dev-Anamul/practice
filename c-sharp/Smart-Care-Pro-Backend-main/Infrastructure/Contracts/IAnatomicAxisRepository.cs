using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 23.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IAnatomicAxisRepository : IRepository<AnatomicAxis>
    {
        /// <summary>
        /// The method is used to get an anatomic axis by anatomic axis name.
        /// </summary>
        /// <param name="anatomicAxis">Name of an anatomic axis.</param>
        /// <returns>Returns an anatomic axis if the anatomic axis name is matched.</returns>
        public Task<AnatomicAxis> GetAnatomicAxisByName(string anatomicAxis);

        /// <summary>
        /// The method is used to get an anatomic axis by key.
        /// </summary>
        /// <param name="key">Primary key of the table AnatomicAxes.</param>
        /// <returns>Returns an anatomic axis if the key is matched.</returns>
        public Task<AnatomicAxis> GetAnatomicAxisByKey(int key);

        /// <summary>
        /// The method is used to get the list of anatomic axes.
        /// </summary>
        /// <returns>Returns a list of all anatomic axes.</returns>
        public Task<IEnumerable<AnatomicAxis>> GetAnatomicAxes();
    }
}