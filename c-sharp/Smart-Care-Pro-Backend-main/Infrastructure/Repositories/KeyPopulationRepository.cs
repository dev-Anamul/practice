using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Bella
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified:  
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class KeyPopulationRepository : Repository<KeyPopulation>, IKeyPopulationRepository
    {
        public KeyPopulationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get KeyPopulation by key.
        /// </summary>
        /// <param name="key">Primary key of the table KeyPopulations.</param>
        /// <returns>Returns a KeyPopulation if the key is matched.</returns>
        public async Task<KeyPopulation> GetKeyPopulationByKey(int key)
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
        /// The method is used to get the list of KeyPopulations.
        /// </summary>
        /// <returns>Returns a list of all KeyPopulations.</returns>
        public async Task<IEnumerable<KeyPopulation>> GetKeyPopulations()
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
        /// The method is used to get a key Population by key Population name.
        /// </summary>
        /// <param name="keyPopulation">Name of a keyPopulation.</param>
        /// <returns>Returns a keyPopulation if the keyPopulation name is matched.</returns>
        public async Task<KeyPopulation> GetKeyPopulationByName(string keyPopulation)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Description.ToLower().Trim() == keyPopulation.ToLower().Trim() && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}