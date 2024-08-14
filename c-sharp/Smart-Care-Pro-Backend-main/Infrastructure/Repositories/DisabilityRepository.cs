using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Labib
 * Date created : 09.04.2023
 * Modified by  : Biplob Roy
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class DisabilityRepository : Repository<Disability>, IDisabilityRepository
    {
        public DisabilityRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of disabilities.
        /// </summary>
        /// <returns>Returns a list of all disabilities.</returns>
        public async Task<IEnumerable<Disability>> GetDisabilities()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Disability by key.
        /// </summary>
        /// <param name="key">Primary key of the table Disabilities.</param>
        /// <returns>Returns a Disability  if the key is matched.</returns>
        public async Task<Disability> GetDisabilityByKey(int key)
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
        /// The method is used to get an Disability by Disability Description.
        /// </summary>
        /// <param name="description">Name of an Disability.</param>
        /// <returns>Returns an Disability if the Disability description is matched.</returns>
        public async Task<Disability> GetDisabilityByName(string disability)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == disability.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}