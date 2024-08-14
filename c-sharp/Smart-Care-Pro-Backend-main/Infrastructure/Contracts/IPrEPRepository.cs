using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;

namespace Infrastructure.Contracts
{
    public interface IPrEPRepository : IRepository<Plan>
    {
        /// <summary>
        /// The method is used to get a PrEP by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Returns a PrEP if the key is matched.</returns>
        public Task<Plan> GetPrEPByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public Task<IEnumerable<Plan>> GetPrEPs();

        /// <summary>
        /// The method is used to get a PrEP by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a PrEP if the ClientID is matched.</returns>
        public Task<IEnumerable<Plan>> GetPrEPClient(Guid ClientID);
        public Task<IEnumerable<Plan>> GetPrEPClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetPrEPClientTotalCount(Guid clientID, EncounterType? encounterType);
        /// <summary>
        /// The method is used to get a PrEP by Encounter.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a PrEP if the EncounterID is matched.</returns>
        public Task<IEnumerable<Plan>> GetPrEPByEncounter(Guid encounterId);

        public Task<Plan> GetPrEPEncounterId(Guid encounterId);
    }
}
