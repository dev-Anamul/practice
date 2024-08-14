using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IScreeningRepository : IRepository<Screening>
    {    
        /// <summary>
          /// The method is used to get the list of Screening.
          /// </summary>
          /// <returns>Returns a list of all Screening.</returns>
        public Task<IEnumerable<Screening>> GetScreening();
        /// <summary>
        /// The method is used to get a Screening by key.
        /// </summary>
        /// <param name="key">Primary key of the table Screening.</param>
        /// <returns>Returns a Screening if the key is matched.</returns>
        public Task<Screening> GetScreeningByKey(Guid key);
        /// <summary>
        /// The method is used to get a   Screening by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a Screening if the ClientID is matched.</returns>
        public Task<IEnumerable<Screening>> GetScreeningbyClienId(Guid clientId);
        /// </summary>
        /// <returns>Returns a list of all Screening by EncounterID.</returns>
        public Task<IEnumerable<Screening>> GetScreeningByEncounter(Guid encounterId);
    }
}
