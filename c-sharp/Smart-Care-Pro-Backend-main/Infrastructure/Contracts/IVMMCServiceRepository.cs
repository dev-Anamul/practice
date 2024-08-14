using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IVMMCServiceRepository : IRepository<VMMCService>
    {
        /// <summary>
        /// The method is used to get a VMMCService by key.
        /// </summary>
        /// <param name="key">Primary key of the table VMMCServices.</param>
        /// <returns>Returns a VMMCService if the key is matched.</returns>
        public Task<VMMCService> GetVMMCServiceByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of VMMCServices.
        /// </summary>
        /// <returns>Returns a list of all VMMCServices.</returns>
        public Task<IEnumerable<VMMCService>> GetVMMCServices();

        /// <summary>
        /// The method is used to get a VMMCService by ClientId.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns>Returns a VMMCService if the ClientId is matched.</returns>
        public Task<IEnumerable<VMMCService>> GetVMMCServiceByClient(Guid clientId);

        /// <summary>
        /// The method is used to get a VMMCService by Encounter.
        /// </summary>
        /// <param name="EncounterId"></param>
        /// <returns>Returns a VMMCService if the Encounter is matched.</returns>
        public Task<IEnumerable<VMMCService>> GetVMMCServiceByEncounterId(Guid encounterId);
    }
}