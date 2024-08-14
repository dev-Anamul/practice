using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface ICaCXPlanRepository : IRepository<CaCXPlan>
    {/// <summary>
     /// The method is used to get the list of CaCXPlan.
     /// </summary>
     /// <returns>Returns a list of all CaCXPlan.</returns>
        public Task<IEnumerable<CaCXPlan>> GetCaCXPlan();
        /// <summary>
        /// The method is used to get a CaCXPlan by key.
        /// </summary>
        /// <param name="key">Primary key of the table CaCXPlan.</param>
        /// <returns>Returns a CaCXPlan if the key is matched.</returns>
        public Task<CaCXPlan> GetCaCXPlanByKey(Guid key);
        /// <summary>
        /// The method is used to get a   CaCXPlan by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a CaCXPlan if the ClientID is matched.</returns>
        public Task<IEnumerable<CaCXPlan>> GetCaCXPlanbyClienId(Guid clientId);
        /// </summary>
        /// <returns>Returns a list of all CaCXPlan by EncounterID.</returns>
        public Task<IEnumerable<CaCXPlan>> GetCaCXPlanByEncounter(Guid encounterId);
    }
}
