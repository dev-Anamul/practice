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
    public interface IVisitDetailRepository : IRepository<VisitDetail>
    {
        /// <summary>
        /// The method is used to get a VisitDetail by key.
        /// </summary>
        /// <param name="key">Primary key of the table VisitDetails.</param>
        /// <returns>Returns a visitDetail if the key is matched.</returns>
        public Task<VisitDetail> GetVisitDetailByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of VisitDetails.
        /// </summary>
        /// <returns>Returns a list of all VisitDetails.</returns>
        public Task<IEnumerable<VisitDetail>> GetVisitDetails();

        /// <summary>
        /// The method is used to get a VisitDetail by ClientId.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns>Returns a VisitDetail if the ClientId is matched.</returns>
        public Task<IEnumerable<VisitDetail>> GetVisitDetailByClient(Guid ClientId);
        public Task<IEnumerable<VisitDetail>> GetVisitDetailByClient(Guid ClientId, int page, int pageSize, EncounterType? encounterType);
        public int GetVisitDetailByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of VisitDetail by EncounterId.
        /// </summary>
        /// <param name="EncounterId">EncounterId</param>
        /// <returns>Returns a list of all VisitDetail by EncounterId.</returns>
        public Task<IEnumerable<VisitDetail>> GetVisitDetailByEncounter(Guid EncounterId);
    }
}