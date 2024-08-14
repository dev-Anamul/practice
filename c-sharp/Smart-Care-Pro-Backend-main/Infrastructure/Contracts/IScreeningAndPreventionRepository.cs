using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IScreeningAndPreventionRepository : IRepository<ScreeningAndPrevention>
    {
        /// <summary>
        /// The method is used to get a ScreeningAndPrevention by key.
        /// </summary>
        /// <param name="key">Primary key of the table ScreeningAndPreventions.</param>
        /// <returns>Returns a ScreeningAndPrevention if the key is matched.</returns>
        public Task<ScreeningAndPrevention> GetScreeningAndPreventionByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of ScreeningAndPrevention.
        /// </summary>
        /// <returns>Returns a list of all ScreeningAndPrevention.</returns>
        public Task<IEnumerable<ScreeningAndPrevention>> GetScreeningAndPreventions();

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a ScreeningAndPrevention if the ClientID is matched.</returns>
        public Task<IEnumerable<ScreeningAndPrevention>> GetScreeningAndPreventionByClient(Guid ClientID);
        public Task<IEnumerable<ScreeningAndPrevention>> GetScreeningAndPreventionByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetScreeningAndPreventionByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of ScreeningAndPrevention by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ScreeningAndPrevention by EncounterID.</returns>
        public Task<IEnumerable<ScreeningAndPrevention>> GetScreeningAndPreventionByEncounter(Guid EncounterID);
    }
}
