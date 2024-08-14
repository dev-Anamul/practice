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
    public interface IModuleAccessRepository : IRepository<ModuleAccess>
    {
        /// <summary>
        /// The method is used to get the list of Module Access.
        /// </summary>
        /// <returns>Returns a list of all Module Access.</returns>
        public Task<IEnumerable<ModuleAccess>> GetModuleAccessesByFacilityAccess(Guid facilityAccessId);
    }
}
