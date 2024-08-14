using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Shakil
 * Date created : 02.01.2023
 * Modified by  : Shakil
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IContraceptiveRepository interface.
    /// </summary>
    public class ContraceptiveRepository : Repository<Contraceptive>, IContraceptiveRepository
    {
        public ContraceptiveRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a contraceptive by contraceptive name.
        /// </summary>
        /// <param name="contraceptiveName">Name of a contraceptive.</param>
        /// <returns>Returns a contraceptive if the contraceptive name is matched.</returns>
        public async Task<Contraceptive> GetContraceptiveByName(string contraceptiveName)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Description.ToLower().Trim() == contraceptiveName.ToLower().Trim() && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a contraceptive by key.
        /// </summary>
        /// <param name="key">Primary key of the table Contraceptives.</param>
        /// <returns>Returns a contraceptive if the key is matched.</returns>
        public async Task<Contraceptive> GetContraceptiveByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Oid == key && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of contraceptives.
        /// </summary>
        /// <returns>Returns a list of all contraceptives.</returns>
        public async Task<IEnumerable<Contraceptive>> GetContraceptives()
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
    }
}