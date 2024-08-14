using Domain.Entities;

/*
 * Created by   : Biplob Roy
 * Date created : 29.04.2023
 * Modified by  : Biplob Roy
 * Last modified: 02.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPreferredFeedingRepository : IRepository<PreferredFeeding>
    {
        /// <summary>
        /// The method is used to get an PreferredFeeding by key.
        /// </summary>
        /// <param name="key">Primary key of the table PreferredFeedings.</param>
        /// <returns>Returns an PreferredFeeding if the key is matched.</returns>
        public Task<PreferredFeeding> GetPreferredFeedingByKey(int key);

        /// <summary>
        /// The method is used to get the list of PreferredFeedings.
        /// </summary>
        /// <returns>Returns a list of all PreferredFeedings.</returns>
        public Task<IEnumerable<PreferredFeeding>> GetPreferredFeedings();

        /// <summary>
        /// The method is used to get an PreferredFeeding by PreferredFeeding Description.
        /// </summary>
        /// <param name="preferredFeeding">Description of an PreferredFeeding.</param>
        /// <returns>Returns an PreferredFeeding if the PreferredFeeding name is matched.</returns>
        public Task<PreferredFeeding> GetPreferredFeedingByName(string preferredFeeding);
    }
}