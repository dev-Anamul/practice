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
    public class NeonatalBirthOutcomeRepository : Repository<NeonatalBirthOutcome>, INeonatalBirthOutcomeRepository
    {
        public NeonatalBirthOutcomeRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get NeonatalBirthOutcome by key.
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalBirthOutcomes.</param>
        /// <returns>Returns a NeonatalBirthOutcome if the key is matched.</returns>
        public async Task<NeonatalBirthOutcome> GetNeonatalBirthOutcomeByKey(int key)
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
        /// The method is used to get the list of NeonatalBirthOutcomes.
        /// </summary>
        /// <returns>Returns a list of all NeonatalBirthOutcome.</returns>
        public async Task<IEnumerable<NeonatalBirthOutcome>> GetNeonatalBirthOutcomes()
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
        /// The method is used to get an NeonatalBirthOutcome by NeonatalBirthOutcome Description.
        /// </summary>
        /// <param name="neonatalBirthOutcome">Name of an NeonatalBirthOutcome.</param>
        /// <returns>Returns an NeonatalBirthOutcome if the NeonatalBirthOutcome description is matched.</returns>
        public async Task<NeonatalBirthOutcome> GetNeonatalBirthOutcomeByName(string neonatalBirthOutcome)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == neonatalBirthOutcome.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}