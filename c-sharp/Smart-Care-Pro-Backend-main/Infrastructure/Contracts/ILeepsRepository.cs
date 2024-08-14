using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface ILeepsRepository : IRepository<Leeps>
    {   /// <summary>
        /// The method is used to get the list of Leeps.
        /// </summary>
        /// <returns>Returns a list of all Leeps.</returns>
        public Task<IEnumerable<Leeps>> GetLeeps();

        /// <summary>
        /// The method is used to get a Leeps by key.
        /// </summary>
        /// <param name="key">Primary key of the table Leeps.</param>
        /// <returns>Returns a Leeps if the key is matched.</returns>
        public Task<Leeps> GetLeepsByKey(Guid key);

        /// <summary>
        /// The method is used to get a   Leeps by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a Leeps if the ClientID is matched.</returns>
        public Task<IEnumerable<Leeps>> GetLeepsbyClienId(Guid clientId);

        /// </summary>
        /// <returns>Returns a list of all Leeps by EncounterID.</returns>
        public Task<IEnumerable<Leeps>> GetLeepsByEncounter(Guid encounterId);
    }
}
