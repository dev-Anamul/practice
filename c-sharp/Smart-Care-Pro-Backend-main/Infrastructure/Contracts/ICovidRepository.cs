using Domain.Entities;
using static Utilities.Constants.Enums;

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
    public interface ICovidRepository : IRepository<Covid>
    {
        /// <summary>
        /// The method is used to get a covid by key.
        /// </summary>
        /// <param name="key">Primary key of the table Covids.</param>
        /// <returns>Returns a covid if the key is matched.</returns>
        public Task<Covid> GetCovidByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of covid.
        /// </summary>
        /// <returns>Returns a list of all covid.</returns>
        public Task<IEnumerable<Covid>> GetCovids();

        /// <summary>
        /// The method is used to get a covid by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a covid if the ClientID is matched.</returns>
        public Task<IEnumerable<Covid>> GetCovidByClient(Guid clientId);
        public Task<IEnumerable<Covid>> GetCovidByClientLast24Hours(Guid clientId);
        public Task<IEnumerable<Covid>> GetCovidByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetCovidByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a covid by Encounter.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a covid if the OPD EncounterID is matched.</returns>
        public Task<IEnumerable<Covid>> GetCovidByEncounter(Guid EncounterID);
    }
}