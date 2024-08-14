using Domain.Entities;

/*
 * Created by   : Bithy
 * Date created : 25.12.2022
 * Modified by  : Rezwana       Biplob Roy
 * Last modified: 27.12.2022    01.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPhysicalSystemRepository : IRepository<PhysicalSystem>
    {
        /// <summary>
        /// The method is used to get the list of physical systems.
        /// </summary>
        /// <returns>Returns a list of all physical systems.</returns>
        public Task<IEnumerable<PhysicalSystem>> GetPhysicalSystems();

        /// <summary>
        /// The method is used to get a PhysicalSystem by key.
        /// </summary>
        /// <param name="key">Primary key of the table PhysicalSystems.</param>
        /// <returns>Returns a PhysicalSystem if the key is matched.</returns>
        public Task<PhysicalSystem> GetPhysicalSystemByKey(int key);

        /// <summary>
        /// The method is used to get an PhysicalSystem by PhysicalSystem Description.
        /// </summary>
        /// <param name="physicalSystem">Description of an PhysicalSystem.</param>
        /// <returns>Returns an PhysicalSystem if the PhysicalSystem name is matched.</returns>
        public Task<PhysicalSystem> GetPhysicalSystemByName(string physicalSystem);
    }
}