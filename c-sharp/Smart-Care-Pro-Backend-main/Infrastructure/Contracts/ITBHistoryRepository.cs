using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 05.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ITBHistoryRepository : IRepository<TBHistory>
    {
        /// <summary>
        /// The method is used to get a TBHistory by key.
        /// </summary>
        /// <param name="key">Primary key of the table TBHistory.</param>
        /// <returns>Returns a TBHistory if the key is matched.</returns>
        public Task<TBHistory> GetTBHistoryByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of TBHistory.
        /// </summary>
        /// <returns>Returns a list of all TBHistory.</returns>
        public Task<IEnumerable<TBHistory>> GetTBHistories();

        /// <summary>
        /// The method is used to get a TBHistory by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a TBHistory if the ClientID is matched.</returns>
        public Task<IEnumerable<TBHistory>> GetTBHistoryByClient(Guid ClientID);
        public Task<IEnumerable<TBHistory>> GetTBHistoryByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetTBHistoryByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of TBHistory by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all TBHistory by EncounterID.</returns>
        public Task<IEnumerable<TBHistory>> GetTBHistoryByEncounter(Guid EncounterID);
    }
}
