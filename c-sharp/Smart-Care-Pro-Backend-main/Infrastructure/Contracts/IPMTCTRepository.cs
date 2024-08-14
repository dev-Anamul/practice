using Domain.Entities;

/*
 * Created by   : Biplob Roy
 * Date created : 11.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPMTCTRepository : IRepository<PMTCT>
    {
        /// <summary>
        /// The method is used to get a PMTCT by key.
        /// </summary>
        /// <param name="key">Primary key of the table PMTCTs.</param>
        /// <returns>Returns a PMTCT if the key is matched.</returns>
        public Task<PMTCT> GetPMTCTByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of PMTCT.
        /// </summary>
        /// <returns>Returns a list of all PMTCT.</returns>
        public Task<IEnumerable<PMTCT>> GetPMTCTs();

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a PMTCT if the ClientID is matched.</returns>
        public Task<IEnumerable<PMTCT>> GetPMTCTByClient(Guid ClientID);

        /// <summary>
        /// The method is used to get the list of PMTCT by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all PMTCT by EncounterID.</returns>
        public Task<IEnumerable<PMTCT>> GetPMTCTByEncounter(Guid EncounterID);
    }
}