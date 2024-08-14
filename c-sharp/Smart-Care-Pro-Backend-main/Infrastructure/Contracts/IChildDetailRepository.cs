using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 14.08.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IChildDetailRepository : IRepository<ChildDetail>
    {
        /// <summary>
        /// The method is used to get a ChildDetail by key.
        /// </summary>
        /// <param name="key">Primary key of the table ChildDetails.</param>
        /// <returns>Returns a ChildDetail if the key is matched.</returns>
        public Task<ChildDetail> GetChildDetailByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of ChildDetail.
        /// </summary>
        /// <returns>Returns a list of all ChildDetail.</returns>
        public Task<IEnumerable<ChildDetail>> GetChildDetails();

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a ChildDetail if the ClientID is matched.</returns>
        public Task<IEnumerable<ChildDetail>> GetChildDetailByClient(Guid ClientID);

        /// <summary>
        /// The method is used to get the list of ChildDetail by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ChildDetail by EncounterID.</returns>
        public Task<IEnumerable<ChildDetail>> GetChildDetailByEncounter(Guid EncounterID);
    }
}