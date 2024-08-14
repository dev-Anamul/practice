using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IFamilyPlanRegisterRepository : IRepository<FamilyPlanRegister>
    {
        /// <summary>
        /// The method is used to get a FamilyPlanRegister by key.
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlanRegisters.</param>
        /// <returns>Returns a FamilyPlanRegister if the key is matched.</returns>
        public Task<FamilyPlanRegister> GetFamilyPlanRegisterByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of FamilyPlanRegister.
        /// </summary>
        /// <returns>Returns a list of all FamilyPlanRegisters.</returns>
        public Task<IEnumerable<FamilyPlanRegister>> GetFamilyPlanRegisters();

        /// <summary>
        /// The method is used to get a FamilyPlanRegister by Client Id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a FamilyPlanRegisters if the Client ID is matched.</returns>
        public Task<IEnumerable<FamilyPlanRegister>> GetFamilyPlanRegisterByClient(Guid clientId);
        public Task<IEnumerable<FamilyPlanRegister>> GetFamilyPlanRegisterByClientLast24Hours(Guid clientId);
        public Task<IEnumerable<FamilyPlanRegister>> GetFamilyPlanRegisterByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetFamilyPlanRegisterByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of FamilyPlanRegister by EncounterId.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a list of all FamilyPlanRegister by EncounterId.</returns>
        public Task<IEnumerable<FamilyPlanRegister>> GetFamilyPlanRegisterByEncounterID(Guid encounterId);
    }
}