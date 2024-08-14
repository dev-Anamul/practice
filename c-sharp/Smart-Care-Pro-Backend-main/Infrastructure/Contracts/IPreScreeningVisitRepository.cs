using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IPreScreeningVisitRepository :IRepository<PreScreeningVisit>
    {
        /// <summary>
        /// The method is used to get the list of PreScreeningVisit.
        /// </summary>
        /// <returns>Returns a list of all PreScreeningVisit.</returns>
        public Task<IEnumerable<PreScreeningVisit>> GetPreScreeningVisit();
        /// <summary>
        /// The method is used to get a PreScreeningVisit by key.
        /// </summary>
        /// <param name="key">Primary key of the table PreScreeningVisit.</param>
        /// <returns>Returns a PreScreeningVisit if the key is matched.</returns>
        public Task<PreScreeningVisit> GetPreScreeningVisitByKey(Guid key);
        /// <summary>
        /// The method is used to get a Pre Screening Visit by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a PreScreeningVisit if the ClientID is matched.</returns>
        public Task<IEnumerable<PreScreeningVisit>> GetPreScreeningVisitbyClienId(Guid clientId);
      
        /// </summary>
        /// <returns>Returns a list of all PreScreeningVisit by EncounterID.</returns>
        public Task<IEnumerable<PreScreeningVisit>> GetPreScreeningVisitsByEncounter(Guid encounterId);
    }
}
