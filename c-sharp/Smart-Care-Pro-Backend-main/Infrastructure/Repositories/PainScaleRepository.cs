using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Bithy
 * Date created : 07-02-2023
 * Modified by  : Biplob Roy
 * Last modified: 01.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IPainScaleRepository interface.
    /// </summary>
    public class PainScaleRepository : Repository<PainScale>, IPainScaleRepository
    {
        public PainScaleRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a PainScale by key.
        /// </summary>
        /// <param name="key">Primary key of the table PainScales.</param>
        /// <returns>Returns a PainScale if the key is matched.</returns>
        public async Task<PainScale> GetPainScaleByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.Oid == key && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PainScales.
        /// </summary>
        /// <returns>Returns a list of all PainScales.</returns>
        public async Task<IEnumerable<PainScale>> GetPainScales()
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
        /// The method is used to get an PainScale by PainScale Description.
        /// </summary>
        /// <param name="description">Name of an PainScale.</param>
        /// <returns>Returns an PainScale if the PainScale description is matched.</returns>
        public async Task<PainScale> GetPainScaleByName(string painScale)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == painScale.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}