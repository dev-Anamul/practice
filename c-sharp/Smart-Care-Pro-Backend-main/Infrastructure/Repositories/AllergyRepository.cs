using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : Lion
 * Last modified: 27.12.2022
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IAllergyRepository interface.
    /// </summary>
    public class AllergyRepository : Repository<Allergy>, IAllergyRepository
    {
        public AllergyRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of allergies.
        /// </summary>
        /// <returns>Returns a list of all allergies.</returns>
        public async Task<IEnumerable<Allergy>> GetAllergies()
        {
            try
            {
                return await QueryAsync(a => a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Allergy>> GetAllergiesByEncounterId(Guid id)
        {
            try
            {
                return await QueryAsync(a => a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Allergies by key.
        /// </summary>
        /// <param name="key">Primary key of the table Allergies.</param>
        /// <returns>Returns a Allergies if the key is matched.</returns>
        public async Task<Allergy> GetAllergyByKey(int key)
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
        /// The method is used to get an allergy by allergy name.
        /// </summary>
        /// <param name="allergy">Name of an allergy.</param>
        /// <returns>Returns an allergy if the allergy name is matched.</returns>
        public async Task<Allergy> GetAllergyByName(string allergy)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == allergy.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}