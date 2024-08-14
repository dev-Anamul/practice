using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 14.08.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ICauseOfNeonatalDeathRepository : IRepository<CauseOfNeonatalDeath>
    {
        /// <summary>
        /// The method is used to get a CauseOfNeonatalDeath by key.
        /// </summary>
        /// <param name="key">Primary key of the table CauseOfNeonatalDeaths.</param>
        /// <returns>Returns a CauseOfNeonatalDeath if the key is matched.</returns>
        public Task<CauseOfNeonatalDeath> GetCauseOfNeonatalDeathByKey(int key);

        /// <summary>
        /// The method is used to get the list of CauseOfNeonatalDeaths.
        /// </summary>
        /// <returns>Returns a list of all CauseOfNeonatalDeaths.</returns>
        public Task<IEnumerable<CauseOfNeonatalDeath>> GetCauseOfNeonatalDeaths();

        /// <summary>
        /// The method is used to get an CauseOfNeonatalDeath by CauseOfNeonatalDeath Description.
        /// </summary>
        /// <param name="causeOfNeonatalDeath">Description of an CauseOfNeonatalDeath.</param>
        /// <returns>Returns an CauseOfNeonatalDeath if the CauseOfNeonatalDeath name is matched.</returns>
        public Task<CauseOfNeonatalDeath> GetCauseOfNeonatalDeathByName(string causeOfNeonatalDeath);
    }
}
