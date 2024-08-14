using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 04.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IOccupationRepository : IRepository<Occupation>
    {
        /// <summary>
        /// The method is used to get an occupation by name.
        /// </summary>
        /// <param name="occupationName">Name of an occupation.</param>
        /// <returns>Returns an occupation if the occupation name is matched.</returns>
        public Task<Occupation> GetOccupationByName(string occupationName);

        /// <summary>
        /// The method is used to get an occupation by key.
        /// </summary>
        /// <param name="key">Primary key of the table Occupations.</param>
        /// <returns>Returns an occupation if the key is matched.</returns>
        public Task<Occupation> GetOccupationByKey(int key);

        /// <summary>
        /// The method is used to get the list of occupations.
        /// </summary>
        /// <returns>Returns a list of all occupations.</returns>
        public Task<IEnumerable<Occupation>> GetOccupations();
    }
}