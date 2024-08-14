using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 02.01.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IFamilyMemberRepository : IRepository<FamilyMember>
    {
        /// <summary>
        /// The method is used to get a family member by key.
        /// </summary>
        /// <param name="key">Primary key of the table FamilyMembers.</param>
        /// <returns>Returns a family member if the key is matched.</returns>
        public Task<FamilyMember> GetFamilyMemberByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of family members.
        /// </summary>
        /// <returns>Returns a list of all family members.</returns>
        public Task<IEnumerable<FamilyMember>> GetFamilyMembers();

        /// <summary>
        /// The method is used to get a family member by ClientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a family member if the ClientId is matched.</returns>
        public Task<IEnumerable<FamilyMember>> GetFamilyMemberByClientId(Guid clientId);
        public Task<IEnumerable<FamilyMember>> GetFamilyMemberByClientIdLast24Hours(Guid clientId);
        public Task<IEnumerable<FamilyMember>> GetFamilyMemberByClientId(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetFamilyMemberByClientIdTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a family member by encounter id.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a chief complaints if the encounterId is matched.</returns>
        public Task<IEnumerable<FamilyMember>> GetFamilyMemberByEncounterId(Guid encounterId);
    }
}