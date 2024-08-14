using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IFacilityAccessRepository : IRepository<FacilityAccess>
    {

        /// <summary>
        /// The method is used to get the list of facility accesses.
        /// </summary>
        /// <returns>Returns a list of all facility accesses.</returns>
        public Task<IEnumerable<FacilityAccess>> GetFacilityAccesses();

        /// <summary>
        /// The method is used to get a non admin user list.
        /// </summary>
        /// <returns>Returns a non admin user.</returns>
        public Task<IEnumerable<UserAccount>> GetNonAdminUsersForAdmin();

        /// <summary>
        /// The method is used to get a FacilityAccess by facility id.
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns>Returns a facility access if the FacilityId is matched.</returns>
        public Task<IEnumerable<FacilityAccess>> GetFacilityAccessesByFacility(int facilityId);

        public Task<IEnumerable<FacilityAccess>> GetFacilityAccessesByLoginRequestFacility(int facilityId, string searchNameOrCellPhone, int page, int pageSize);
        public int GetFacilityAccessesByLoginRequestFacilityTotalCount(int facilityId, string searchNameOrCellPhone);
        public Task<IEnumerable<FacilityAccess>> GetFacilityAccessesByApprovedRequestFacility(int facilityId, string searchNameOrCellPhone, int page, int pageSize);
        public int GetFacilityAccessesByApprovedRequestFacilityTotalCount(int facilityId, string searchNameOrCellPhone);
        public Task<IEnumerable<FacilityAccess>> GetFacilityAccessesByRecoveryRequestFacility(int facilityId, string searchNameOrCellPhone, int page, int pageSize);
        public int GetFacilityAccessesByRecoveryRequestFacilityTotalCount(int facilityId, string searchNameOrCellPhone);

        public Task<FacilityAccess> GetFacilityAccessByUserAndFacilityId(Guid userAccountId, Guid key);

        public Task<List<FacilityAccess>> GetNonDFZFacilities(Guid userId);

        /// <summary>
        /// The method is used to get a facility access by key.
        /// </summary>
        /// <param name="key">Primary key of the table FacilityAccesses.</param>
        /// <returns>Returns a facility access if the key is matched.</returns>
        public Task<FacilityAccess> GetFacilityAccessByKey(Guid key);

        /// <summary>
        /// The method is used to get a UserAccount by UserAccountID.
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <returns>Returns a UserAccount if the UserAccountId is matched.</returns>
        public Task<FacilityAccess> GetFacilityAccessByUserAccountId(Guid userAccountId);

        /// <summary>
        /// The method is used to get a UserAccount by UserAccountID.
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <returns>Returns a UserAccount if the UserAccountId is matched.</returns>
        public Task<IEnumerable<FacilityAccess>> GetFacilityAccessesByUserAccountId(Guid userAccountId);

        /// <summary>
        /// The method is used to get all facility access by userAccount.
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <returns></returns>
        public Task<IEnumerable<FacilityAccess>> GetAllFacilityAccessesByUserAccountId(Guid userAccountId);

        /// <summary>
        /// The method is used to get a FacilityAccess by user id and facility id.
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <param name="facilityId"></param>
        /// <returns>Returns a UserAccount if the key is matched.</returns>
        public Task<FacilityAccess> GetFacilityAccessByUserIdWithFacilityId(Guid userAccountId, int facilityId);

        /// <summary>
        /// The method is used to check duplicate facility access request by user id.
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <param name="facilityId"></param>
        /// <returns>Returns a UserAccount if the key is matched.</returns>
        public Task<FacilityAccess> CheckDuplicateFacilityAccessRequestByUserId(Guid userAccountId, int facilityId);
    }
}