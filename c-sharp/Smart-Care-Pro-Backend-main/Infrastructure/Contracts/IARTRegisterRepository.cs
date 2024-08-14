using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IARTRegisterRepository : IRepository<ARTService>
    {
        /// <summary>
        /// The method is used to get a artRegister by key.
        /// </summary>
        /// <param name="key">Primary key of the table artRegisters.</param>
        /// <returns>Returns a artRegister if the key is matched.</returns>
        public Task<ARTService> GetARTRegisterByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of artRegisters.
        /// </summary>
        /// <returns>Returns a list of all artRegisters.</returns>
        public Task<IEnumerable<ARTService>> GetARTRegisters();

        /// <summary>
        /// The method is used to get a ART Register by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a ART register if the ClientID is matched.</returns>
        public Task<IEnumerable<ARTService>> GetARTRegisterbyClienId(Guid clientId);
        public Task<IEnumerable<ARTService>> GetARTRegisterbyClienIdLast24Hours(Guid clientId);

        /// <summary>
        /// The method is used to get a ARTService by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a ARTService if the Encounter is matched.</returns>
        public Task<IEnumerable<ARTService>> GetARTRegisterByEncounterID(Guid encounterId);
    }
}