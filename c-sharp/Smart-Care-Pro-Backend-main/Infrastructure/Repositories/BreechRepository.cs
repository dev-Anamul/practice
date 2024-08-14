using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class BreechRepository : Repository<Breech>, IBreechRepository
    {
        public BreechRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// The method is used to a get Breech by key.
        /// </summary>
        /// <param name="key">Primary key of the table Breechs.</param>
        /// <returns>Returns a Breech if the key is matched.</returns>
        public async Task<Breech> GetBreechByKey(int key)
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
        /// The method is used to get the list of Breechs.
        /// </summary>
        /// <returns>Returns a list of all Medical treatments.</returns>
        public async Task<IEnumerable<Breech>> GetBreeches()
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
        /// The method is used to get an Breechs by Breechs Description.
        /// </summary>
        /// <param name="breeches">Name of an Breechs.</param>
        /// <returns>Returns an Breechs if the Breechs description is matched.</returns>
        public async Task<Breech> GetBreechByName(string breeches)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == breeches.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}