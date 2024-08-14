using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;

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
    public interface IDrugAdherenceRepository : IRepository<DrugAdherence>
    {
        /// <summary>
        /// The method is used to get a drug adherences by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugAdherences.</param>
        /// <returns>Returns a drug adherences if the key is matched.</returns>
        public Task<DrugAdherence> GetDrugAdherenceByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of drug adherences.
        /// </summary>
        /// <returns>Returns a list of all drug adherences.</returns>
        public Task<IEnumerable<DrugAdherence>> GetDrugAdherences();

        /// <summary>
        /// The method is used to get a drug adherences by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a drug adherences if the ClientID is matched.</returns>
        public Task<IEnumerable<DrugAdherence>> GetDrugAdherenceByClient(Guid ClientID);
        public Task<IEnumerable<DrugAdherence>> GetDrugAdherenceByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType);
        public int GetDrugAdherenceByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a drug adherences by OPD visit.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a drug adherences if the Encounter is matched.</returns>
        public Task<IEnumerable<DrugAdherence>> GetDrugAdherenceByEncounter(Guid EncounterID);
    }
}