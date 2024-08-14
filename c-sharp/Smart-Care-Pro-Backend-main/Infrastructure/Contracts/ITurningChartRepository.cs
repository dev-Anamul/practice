using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by    : Stephan
 * Date created  : 07.02.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Contracts
{
    public interface ITurningChartRepository : IRepository<TurningChart>
    {
        /// <summary>
        /// The method is used to get a birth record by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthRecords.</param>
        /// <returns>Returns a birth record if the key is matched.</returns>
        public Task<TurningChart> GetTurningChartByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of birth TurningChart.
        /// </summary>
        /// <returns>Returns a list of all birth TurningChart.</returns>
        public Task<IEnumerable<TurningChart>> GetTurningCharts();

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a birth record if the ClientID is matched.</returns>
        public Task<IEnumerable<TurningChart>> GetTurningChartByClient(Guid ClientID);
        public Task<IEnumerable<TurningChart>> GetTurningChartByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetTurningChartByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a birth record by Encounter.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a birth record if the OPD EncounterID is matched.</returns>
        public Task<IEnumerable<TurningChart>> GetTurningChartByEncounterId(Guid EncounterID);
    }
}
