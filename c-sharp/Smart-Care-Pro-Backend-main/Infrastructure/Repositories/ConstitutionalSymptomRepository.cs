using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Bithy
 * Date created : 25.12.2022
 * Modified by  : Rezwana       Biplob Roy
 * Last modified: 27.12.2022    30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IConstitutionalSymptomRepository interface.
    /// </summary>
    public class ConstitutionalSymptomRepository : Repository<ConstitutionalSymptom>, IConstitutionalSymptomRepository
    {
        public ConstitutionalSymptomRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of constitutional symptoms.
        /// </summary>
        /// <returns>Returns a list of all constitutional symptoms.</returns>
        public async Task<IEnumerable<ConstitutionalSymptom>> GetConstitutionalSymptoms()
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
        /// The method is used to get a ConstitutionalSymptom by key.
        /// </summary>
        /// <param name="key">Primary key of the table ConstitutionalSymptom.</param>
        /// <returns>Returns a ConstitutionalSymptom if the key is matched.</returns>
        public async Task<ConstitutionalSymptom> GetConstitutionalSymptomByKey(int key)
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
        /// The method is used to get an ConstitutionalSymptom by ConstitutionalSymptom Description.
        /// </summary>
        /// <param name="description">Name of an ConstitutionalSymptom.</param>
        /// <returns>Returns an ConstitutionalSymptom if the ConstitutionalSymptom description is matched.</returns>
        public async Task<ConstitutionalSymptom> GetConstitutionalSymptomByName(string constitutionalSymptom)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == constitutionalSymptom.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}