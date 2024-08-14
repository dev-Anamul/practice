using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 29.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IARTResponseRepository : IRepository<ARTResponse>
    {
        /// <summary>
        /// The method is used to get a ART Response by key.
        /// </summary>
        /// <param name="key">Primary key of the table ART Responses.</param>
        /// <returns>Returns a ARTResponse if the key is matched.</returns>
        public Task<ARTResponse> GetARTResponseByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of ARTResponse.
        /// </summary>
        /// <returns>Returns a list of all AR TResponses.</returns>
        public Task<IEnumerable<ARTResponse>> GetARTResponses();

        /// <summary>
        /// The method is used to get a ART Responses by Client Id.
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns>Returns a ART Responses if the Client ID is matched.</returns>
        public Task<IEnumerable<ARTResponse>> GetARTResponseByClient(Guid clientId);

        /// <summary>
        /// The method is used to get the list of ART Response by OPD visit.
        /// </summary>
        /// <returns>Returns a list of all ART Response by OPD visit.</returns>
        public Task<IEnumerable<ARTResponse>> GetARTResponseByEncounterId(Guid encounterId);
    }
}