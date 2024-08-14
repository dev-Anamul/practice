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
    public interface ITPTHistoryRepository : IRepository<TPTHistory>
    {
        /// <summary>
        /// The method is used to get a TPTHistory by key.
        /// </summary>
        /// <param name="key">Primary key of the table TPTHistory.</param>
        /// <returns>Returns a TPTHistory if the key is matched.</returns>
        public Task<TPTHistory> GetTPTHistoryByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of TPTHistory.
        /// </summary>
        /// <returns>Returns a list of all TPTHistory.</returns>
        public Task<IEnumerable<TPTHistory>> GetTPTHistories();

        /// <summary>
        /// The method is used to get a TPTHistory by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a TPTHistory if the ClientID is matched.</returns>
        public Task<IEnumerable<TPTHistory>> GetTPTHistoryByClient(Guid ClientID);
        public Task<IEnumerable<TPTHistory>> GetTPTHistoryByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetTPTHistoryByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of TPTHistory by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all TPTHistory by EncounterID.</returns>
        public Task<IEnumerable<TPTHistory>> GetTPTHistoryByEncounter(Guid EncounterID);
    }
}
