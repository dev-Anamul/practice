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
    public class ModeOfDeliveryRepository : Repository<ModeOfDelivery>, IModeOfDeliveryRepository
    {
        public ModeOfDeliveryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get ModeOfDelivery by key.
        /// </summary>
        /// <param name="key">Primary key of the table ModeOfDeliveries.</param>
        /// <returns>Returns a ModeOfDelivery if the key is matched.</returns>
        public async Task<ModeOfDelivery> GetModeOfDeliveryByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(b => b.Oid == key && b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ModeOfDeliveries.
        /// </summary>
        /// <returns>Returns a list of all Medical treatments.</returns>
        public async Task<IEnumerable<ModeOfDelivery>> GetModeOfDeliveries()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an ModeOfDelivery by ModeOfDelivery Description.
        /// </summary>
        /// <param name="modeofDelivery">Name of an ModeOfDelivery.</param>
        /// <returns>Returns an ModeOfDelivery if the ModeOfDelivery description is matched.</returns>
        public async Task<ModeOfDelivery> GetModeOfDeliveryByName(string modeofDelivery)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == modeofDelivery.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}