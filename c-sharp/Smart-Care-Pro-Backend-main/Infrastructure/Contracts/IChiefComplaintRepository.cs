using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 14.08.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IChiefComplaintRepository : IRepository<ChiefComplaint>
    {
        /// <summary>
        /// The method is used to get a chief complaint by key.
        /// </summary>
        /// <param name="key">Primary key of the table ChiefComplaints.</param>
        /// <returns>Returns a chief complaint if the key is matched.</returns>
        public Task<ChiefComplaint> GetChiefComplaintByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of chief complaints.
        /// </summary>
        /// <returns>Returns a list of all chief complaints.</returns>
        public Task<IEnumerable<ChiefComplaint>> GetChiefComplaints();

        /// <summary>
        /// The method is used to get a chief complaints with pagination data load
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns>Returns a chief complaints if the Client ID is matched.</returns>
        public Task<Tuple<int, IEnumerable<ChiefComplaint>>> GetChiefComplaintsWithPagination(Guid clientID, int pageNo, int pageSize);

        /// <summary>
        /// The method is used to get a chief complaints by Client Id.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a chief complaints if the Client ID is matched.</returns>
        public Task<Tuple<int,IEnumerable<ChiefComplaint>>> GetChiefComplaintsByClient(Guid clientID, int pageNo, int pageSize);

        public Task<IEnumerable<ChiefComplaint>> GetChiefComplaintsByClient(Guid ClientID);
        public Task<IEnumerable<ChiefComplaint>> GetChiefComplaintsByClient(Guid ClientID, EncounterType encouterType);
        public Task<IEnumerable<ChiefComplaint>> GetChiefComplaintsByClientPagging(Guid ClientID, int page, int pageSize, EncounterType? encouterType);
        public int GetChiefComplaintsByClientPaggingTotalCount(Guid ClientID, EncounterType? encouterType); 
        public Task<IEnumerable<ChiefComplaint>> GetPEPChiefComplaintsByClient(Guid ClientID);
        public Task<IEnumerable<ChiefComplaint>> GetPEPChiefComplaintsByClientLast24Hours(Guid ClientID);
        public Task<IEnumerable<ChiefComplaint>> GetPrEPChiefComplaintsByClient(Guid ClientID);
        /// <summary>
        /// The method is used to get a chief complaints by OPD visit.
        /// </summary>
        /// <param name="OPDVisitID"></param>
        /// <returns>Returns a chief complaints if the OPD visit ID is matched.</returns>
        public Task<IEnumerable<ChiefComplaint>> GetChiefComplaintsByOpdVisit(Guid OPDVisitID);
        public Task<IEnumerable<ChiefComplaint>> GetChiefComplaintsByOpdVisitEncounterType(Guid OPDVisitID, EncounterType encouterType);

        public Task<ChiefComplaint> GetChiefComplaintByOpdVisit(Guid EncounterID);
    }
}