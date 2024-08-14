using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface INeonatalDeathRepository : IRepository<NeonatalDeath>
    {
        /// <summary>
        /// The method is used to get a NeonatalDeath by key.
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalDeaths.</param>
        /// <returns>Returns a NeonatalDeath if the key is matched.</returns>
        public Task<NeonatalDeath> GetNeonatalDeathByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of NeonatalDeaths.
        /// </summary>
        /// <returns>Returns a list of all NeonatalDeaths.</returns>
        public Task<IEnumerable<NeonatalDeath>> GetNeonatalDeaths();

        /// <summary>
        /// The method is used to get a NeonatalDeath by CauseOfNeonatalDeathId
        /// </summary>
        /// <param name="causeOfNeonatalDeathId"></param>
        /// <returns>Returns a NeonatalDeath if the CauseOfNeonatalDeathId is matched.</returns>
        public Task<IEnumerable<NeonatalDeath>> GetNeonatalDeathByCauseOfNeonatalDeath(int causeOfNeonatalDeathId);

        /// <summary>
        /// The method is used to get a NeonatalDeath by NeonatalId
        /// </summary>
        /// <param name="neonatalId"></param>
        /// <returns>Returns a NeonatalDeath if the NeonatalId is matched.</returns>
        public Task<IEnumerable<NeonatalDeath>> GetNeonatalDeathByNeonatal(Guid neonatalId);

        /// <summary>
        /// The method is used to get the list of NeonatalDeath by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all NeonatalDeath by EncounterID.</returns>
        public Task<IEnumerable<NeonatalDeath>> GetNeonatalDeathByEncounter(Guid encounterId);
    }
}