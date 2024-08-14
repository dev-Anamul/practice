using Domain.Entities;


/*
 * Created by   : Lion
 * Date created : 29-01-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IBirthDetailRepository : IRepository<BirthDetail>
    {
        /// <summary>
        /// The method is used to get a birth history by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthDetails.</param>
        /// <returns>Returns a birth history if the key is matched.</returns>
        public Task<BirthDetail> GetBirthDetailByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public Task<IEnumerable<BirthDetail>> GetBirthDetails();

        /// <summary>
        /// The method is used to get a birth history by OPD visit.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a birth history if the Encounter is matched.</returns>
        public Task<IEnumerable<BirthDetail>> GetBirthDetailByEncounter(Guid encounterId);
    }
}