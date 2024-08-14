using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IApgarScoreRepository : IRepository<ApgarScore>
    {
        /// <summary>
        /// The method is used to get a ApgarScore by key.
        /// </summary>
        /// <param name="key">Primary key of the table ApgarScores.</param>
        /// <returns>Returns a ApgarScore if the key is matched.</returns>
        public Task<ApgarScore> GetApgarScoreByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of ApgarScores.
        /// </summary>
        /// <returns>Returns a list of all ApgarScores.</returns>
        public Task<IEnumerable<ApgarScore>> GetApgarScores();

        /// <summary>
        /// The method is used to get a ApgarScore by NeonatalId
        /// </summary>
        /// <param name="neonatalId"></param>
        /// <returns>Returns a ApgarScore if the NeonatalId is matched.</returns>
        public Task<IEnumerable<ApgarScore>> GetApgarScoreByNeonatal(Guid neonatalId);

        /// <summary>
        /// The method is used to get the list of ApgarScore by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ApgarScore by EncounterID.</returns>
        public Task<IEnumerable<ApgarScore>> GetApgarScoreByEncounter(Guid encounterId);
    }
}