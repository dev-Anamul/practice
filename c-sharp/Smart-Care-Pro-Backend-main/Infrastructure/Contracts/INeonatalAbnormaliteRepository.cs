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
    public interface INeonatalAbnormalityRepository : IRepository<NeonatalAbnormality>
    {
        /// <summary>
        /// The method is used to get a NeonatalAbnormality by key.
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalAbnormalities.</param>
        /// <returns>Returns a NeonatalAbnormality if the key is matched.</returns>
        public Task<NeonatalAbnormality> GetNeonatalAbnormalityByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of NeonatalAbnormalities.
        /// </summary>
        /// <returns>Returns a list of all NeonatalAbnormalities.</returns>
        public Task<IEnumerable<NeonatalAbnormality>> GetNeonatalAbnormalities();

        /// <summary>
        /// The method is used to get a NeonatalAbnormality by NeonatalId
        /// </summary>
        /// <param name="neonatalId"></param>
        /// <returns>Returns a NeonatalAbnormality if the NeonatalId is matched.</returns>
        public Task<IEnumerable<NeonatalAbnormality>> GetNeonatalAbnormalityByNeonatal(Guid neonatalId);

        /// <summary>
        /// The method is used to get the list of NeonatalAbnormality by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all NeonatalAbnormality by EncounterID.</returns>
        public Task<IEnumerable<NeonatalAbnormality>> GetNeonatalAbnormalityByEncounter(Guid encounterId);
    }
}