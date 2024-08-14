using Domain.Entities;
using static Utilities.Constants.Enums;

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
    public interface IVisitPuposeRepository : IRepository<VisitPurpose>
    {
        /// <summary>
        /// The method is used to get a visit purpose by key.
        /// </summary>
        /// <param name="key">Primary key of the table VisitPurposes.</param>
        /// <returns>Returns a visit purpose if the key is matched.</returns>
        public Task<VisitPurpose> GetVisitPurposeByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public Task<IEnumerable<VisitPurpose>> GetVisitPurposes();

        /// <summary>
        /// The method is used to get a visit purpose by ClientId.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns>Returns a visit purpose if the ClientId is matched.</returns>
        public Task<IEnumerable<VisitPurpose>> GetVisitPurposeByClient(Guid ClientId);       

        public Task<IEnumerable<VisitPurpose>> GetVisitPurposeByClient(Guid ClientId, int page, int pageSize, EncounterType? encounterType);

        public int GetVisitPurposeByClientTotalCount(Guid clientID, EncounterType? encounterType);

       public Task<VisitPurpose> GetLatestVisitPurposeByClientID(Guid clientId);

         /// <summary>
         /// The method is used to get a Visit Purpose by Encounter.
         /// </summary>
         /// <param name="EncounterId"></param>
         /// <returns>Returns a visit purpose if the Encounter is matched.</returns>
         public Task<IEnumerable<VisitPurpose>> GetVisitPurposeByEncounter(Guid EncounterId);
    }
}