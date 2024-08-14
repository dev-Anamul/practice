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
    public interface INeonatalInjuryRepository : IRepository<NeonatalInjury>
    {
        /// <summary>
        /// The method is used to get a NeonatalInjury by key.
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalInjuries.</param>
        /// <returns>Returns a NeonatalInjury if the key is matched.</returns>
        public Task<NeonatalInjury> GetNeonatalInjuryByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of NeonatalInjuries.
        /// </summary>
        /// <returns>Returns a list of all NeonatalInjuries.</returns>
        public Task<IEnumerable<NeonatalInjury>> GetNeonatalInjuries();

        /// <summary>
        /// The method is used to get a NeonatalInjury by NeonatalId
        /// </summary>
        /// <param name="neonatalId"></param>
        /// <returns>Returns a NeonatalInjury if the NeonatalId is matched.</returns>
        public Task<IEnumerable<NeonatalInjury>> GetNeonatalInjuryByNeonatal(Guid neonatalId);

        /// <summary>
        /// The method is used to get the list of NeonatalInjury by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all NeonatalInjury by EncounterID.</returns>
        public Task<IEnumerable<NeonatalInjury>> GetNeonatalInjuryByEncounter(Guid encounterId);
    }
}
