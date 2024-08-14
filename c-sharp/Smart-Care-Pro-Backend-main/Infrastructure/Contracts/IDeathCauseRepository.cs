using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IDeathCauseRepository : IRepository<DeathCause>
    {
        /// <summary>
        /// The method is used to get a death cause by key.
        /// </summary>
        /// <param name="key">Primary key of the table DeathCauses.</param>
        /// <returns>Returns a death cause if the key is matched.</returns>
        public Task<DeathCause> GetDeathCauseByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of death causes.
        /// </summary>
        /// <returns>Returns a list of all death causes.</returns>
        public Task<IEnumerable<DeathCause>> GetDeathCauses();

        /// <summary>
        /// The method is used to get a death cause by deathRecordId.
        /// </summary>
        /// <param name="deathRecordId"></param>
        /// <returns>Returns a death cause if the deathRecordId is matched.</returns>
        public Task<IEnumerable<DeathCause>> GetDeathCauseByDeathRecordID(Guid deathRecordId);
    }
}