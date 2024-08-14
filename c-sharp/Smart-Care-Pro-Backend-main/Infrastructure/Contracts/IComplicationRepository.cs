using Domain.Entities;

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
    public interface IComplicationRepository : IRepository<Complication>
    {
        /// <summary>
        /// The method is used to get a complication by key.
        /// </summary>
        /// <param name="key">Primary key of the table Complications.</param>
        /// <returns>Returns a complication if the key is matched.</returns>
        public Task<Complication> GetComplicationByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of complications.
        /// </summary>
        /// <returns>Returns a list of all complications.</returns>
        public Task<IEnumerable<Complication>> GetComplications();

        /// <summary>
        /// The method is used to get a complication by VMMCID.
        /// </summary>
        /// <param name="VMMCServiceID"></param>
        /// <returns>Returns a complication if the VMMCID is matched.</returns>
        public Task<IEnumerable<Complication>> GetComplicationByVMMCService(Guid VMMCServiceID);

        /// <summary>
        /// The method is used to get a complication by EncounterID.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a complication if the EncounterID is matched.</returns>
        public Task<IEnumerable<Complication>> GetComplicationByEncounter(Guid EncounterID);
    }
}