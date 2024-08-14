using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Bithy
 * Date created : 25.12.2022
 * Modified by  : Rezwana       Biplob Roy
 * Last modified: 27.12.2022    01.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IPhysicalSystemRepository interface.
    /// </summary>
    public class PhysicalSystemRepository : Repository<PhysicalSystem>, IPhysicalSystemRepository
    {
        public PhysicalSystemRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of physical systems.
        /// </summary>
        /// <returns>Returns a list of all physical systems.</returns>
        public async Task<IEnumerable<PhysicalSystem>> GetPhysicalSystems()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a PhysicalSystem by key.
        /// </summary>
        /// <param name="key">Primary key of the table PhysicalSystems.</param>
        /// <returns>Returns a PhysicalSystem  if the key is matched.</returns>
        public async Task<PhysicalSystem> GetPhysicalSystemByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.Oid == key && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an PhysicalSystem by PhysicalSystem Description.
        /// </summary>
        /// <param name="description">Name of an PhysicalSystem.</param>
        /// <returns>Returns an PhysicalSystem if the PhysicalSystem description is matched.</returns>
        public async Task<PhysicalSystem> GetPhysicalSystemByName(string physicalSystem)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == physicalSystem.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}