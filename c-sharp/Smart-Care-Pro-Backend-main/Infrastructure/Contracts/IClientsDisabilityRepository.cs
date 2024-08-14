using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by   : Lion
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */

namespace Infrastructure.Contracts
{
    public interface IClientsDisabilityRepository : IRepository<ClientsDisability>
    {
        /// <summary>
        /// The method is used to get a clients disability by key.
        /// </summary>
        /// <param name="key">Primary key of the table ClientsDisabilities.</param>
        /// <returns>Returns a clients disability if the key is matched.</returns>
        public Task<ClientsDisability> GetClientsDisabilityByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of clients disabilities.
        /// </summary>
        /// <returns>Returns a list of all clients disabilities.</returns>
        public Task<IEnumerable<ClientsDisability>> GetClientsDisabilities();

        public Task<IEnumerable<ClientsDisability>> GetClientsDisabilityByClient(Guid clientId);
        /// <summary>
        /// The method is used to get a clients disabilities by Encounter.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a clients disabilities if the Encounter ID is matched.</returns>
        public Task<IEnumerable<ClientsDisability>> GetClientsDisabilityByEncounter(Guid EncounterID);
    }
}