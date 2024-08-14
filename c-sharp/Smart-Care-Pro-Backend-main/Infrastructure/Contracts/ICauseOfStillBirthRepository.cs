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
    public interface ICauseOfStillBirthRepository : IRepository<CauseOfStillbirth>
    {
        /// <summary>
        /// The method is used to get a CauseOfStillBirth by key.
        /// </summary>
        /// <param name="key">Primary key of the table CauseOfStillBirths.</param>
        /// <returns>Returns a CauseOfStillBirth if the key is matched.</returns>
        public Task<CauseOfStillbirth> GetCauseOfStillBirthByKey(int key);

        /// <summary>
        /// The method is used to get the list of CauseOfStillBirths.
        /// </summary>
        /// <returns>Returns a list of all CauseOfStillBirths.</returns>
        public Task<IEnumerable<CauseOfStillbirth>> GetCauseOfStillBirths();

        /// <summary>
        /// The method is used to get an CauseOfStillbirth by CauseOfStillbirth Description.
        /// </summary>
        /// <param name="causeOfStillbirth">Description of an CauseOfStillbirth.</param>
        /// <returns>Returns an CauseOfStillbirth if the CauseOfStillbirth name is matched.</returns>
        public Task<CauseOfStillbirth> GetCauseOfStillBirthByName(string causeOfStillbirth);
    }
}
