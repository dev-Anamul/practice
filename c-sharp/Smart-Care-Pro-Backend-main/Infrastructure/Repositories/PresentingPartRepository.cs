using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Biplob Roy
 * Date created : 02.05.2023
 * Modified by  : Biplob Roy
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class PresentingPartRepository : Repository<PresentingPart>, IPresentingPartRepository
    {
        public PresentingPartRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// The method is used to a get PresentingPart by key.
        /// </summary>
        /// <param name="key">Primary key of the table PresentingParts.</param>
        /// <returns>Returns a PresentingPart if the key is matched.</returns>
        public async Task<PresentingPart> GetPresentingPartByKey(int key)
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
        /// The method is used to get the list of PresentingParts.
        /// </summary>
        /// <returns>Returns a list of all Medical treatments.</returns>
        public async Task<IEnumerable<PresentingPart>> GetPresentingParts()
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
        /// The method is used to get an PresentingPart by PresentingPart Description.
        /// </summary>
        /// <param name="description">Name of an PresentingPart.</param>
        /// <returns>Returns an PresentingPart if the PresentingPart description is matched.</returns>
        public async Task<PresentingPart> GetPresentingPartByName(string presentingPart)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == presentingPart.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}