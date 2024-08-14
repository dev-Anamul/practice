using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IMotherDetailRepository : IRepository<MotherDetail>
    {
        /// <summary>
        /// The method is used to get a MotherDetail by key.
        /// </summary>
        /// <param name="key">Primary key of the table MotherDetails.</param>
        /// <returns>Returns a MotherDetail if the key is matched.</returns>
        public Task<MotherDetail> GetMotherDetailByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of MotherDetail.
        /// </summary>
        /// <returns>Returns a list of all MotherDetail.</returns>
        public Task<IEnumerable<MotherDetail>> GetMotherDetails();

        /// <summary>
        /// The method is used to get a birth record by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a MotherDetail if the clientId is matched.</returns>
        public Task<IEnumerable<MotherDetail>> GetMotherDetailByClient(Guid clientId);
        public Task<IEnumerable<MotherDetail>> GetMotherDetailByClient(Guid clientId, int page, int pageSize);
        public int GetMotherDetailByClientTotalCount(Guid clientID);
        /// <summary>
        /// The method is used to get the list of MotherDetail by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all MotherDetail by EncounterID.</returns>
        public Task<IEnumerable<MotherDetail>> GetMotherDetailByEncounter(Guid encounterId);
    }
}