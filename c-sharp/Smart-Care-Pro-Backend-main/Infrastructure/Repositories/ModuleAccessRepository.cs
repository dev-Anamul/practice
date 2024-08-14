using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IModuleAccessRepository interface.
    /// </summary>
    public class ModuleAccessRepository : Repository<ModuleAccess>, IModuleAccessRepository
    {
        public ModuleAccessRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of ModuleAccesses.
        /// </summary>
        /// <returns>Returns a list of all Module Access.</returns>
        public async Task<IEnumerable<ModuleAccess>> GetModuleAccessesByFacilityAccess(Guid facilityAccessId)
        {
            try
            {
                return await QueryAsync(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityAccessId == facilityAccessId);
            }
            catch (Exception)
            {
                throw;
            }
            throw new NotImplementedException();
        }
    }
}