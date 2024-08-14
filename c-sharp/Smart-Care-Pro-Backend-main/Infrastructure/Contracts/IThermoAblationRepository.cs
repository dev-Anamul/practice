using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IThermoAblationRepository : IRepository<ThermoAblation>
    {
        /// <summary>
      /// The method is used to get a ThermoAblation by key.
      /// </summary>
      /// <param name="key">Primary key of the table ThermoAblation.</param>
      /// <returns>Returns a ThermoAblation if the key is matched.</returns>
        public Task<ThermoAblation> GetThermoAblationByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of ThermoAblation.
        /// </summary>
        /// <returns>Returns a list of all ThermoAblation.</returns>
        public Task<IEnumerable<ThermoAblation>> GetThermoAblation();

        /// <summary>
        /// The method is used to get a   ThermoAblation by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a ThermoAblation if the ClientID is matched.</returns>
        public Task<IEnumerable<ThermoAblation>> GetThermoAblationbyClienId(Guid clientId);

        /// </summary>
        /// <returns>Returns a list of all ThermoAblation by EncounterID.</returns>
        public Task<IEnumerable<ThermoAblation>> GetThermoAblationByEncounter(Guid encounterId);
    }
}
