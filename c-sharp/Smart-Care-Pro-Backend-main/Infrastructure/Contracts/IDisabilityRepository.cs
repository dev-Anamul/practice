using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IDisabilityRepository : IRepository<Disability>
    {
        /// <summary>
        /// The method is used to get the list of disabilities.
        /// </summary>
        /// <returns>Returns a list of all disabilities.</returns>
        public Task<IEnumerable<Disability>> GetDisabilities();

        /// <summary>
        /// The method is used to get a Disability by key.
        /// </summary>
        /// <param name="key">Primary key of the table Disabilitys.</param>
        /// <returns>Returns a Disability if the key is matched.</returns>
        public Task<Disability> GetDisabilityByKey(int key);

        /// <summary>
        /// The method is used to get an Disability by Disability Description.
        /// </summary>
        /// <param name="disability">Description of an Disability.</param>
        /// <returns>Returns an Disability if the Disability name is matched.</returns>
        public Task<Disability> GetDisabilityByName(string disability);
    }
}