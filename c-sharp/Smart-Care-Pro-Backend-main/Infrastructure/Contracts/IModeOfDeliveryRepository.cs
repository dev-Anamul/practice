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
    public interface IModeOfDeliveryRepository : IRepository<ModeOfDelivery>
    {
        /// <summary>
        /// The method is used to get a ModeOfDelivery by key.
        /// </summary>
        /// <param name="key">Primary key of the table ModeOfDeliveries.</param>
        /// <returns>Returns a ModeOfDelivery if the key is matched.</returns>
        public Task<ModeOfDelivery> GetModeOfDeliveryByKey(int key);

        /// <summary>
        /// The method is used to get the list of ModeOfDeliveries.
        /// </summary>
        /// <returns>Returns a list of all ModeOfDeliveries.</returns>
        public Task<IEnumerable<ModeOfDelivery>> GetModeOfDeliveries();

        /// <summary>
        /// The method is used to get an ModeOfDelivery by ModeOfDelivery Description.
        /// </summary>
        /// <param name="modeOfDelivery">Description of an ModeOfDelivery.</param>
        /// <returns>Returns an ModeOfDelivery if the ModeOfDelivery name is matched.</returns>
        public Task<ModeOfDelivery> GetModeOfDeliveryByName(string modeOfDelivery);
    }
}
