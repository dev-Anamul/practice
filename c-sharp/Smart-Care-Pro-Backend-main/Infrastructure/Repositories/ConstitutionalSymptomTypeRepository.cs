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
    /// Implementation of IConstitutionalSymptomTypeRepository interface.
    /// </summary>
    public class ConstitutionalSymptomTypeRepository : Repository<ConstitutionalSymptomType>, IConstitutionalSymptomTypeRepository
    {
        public ConstitutionalSymptomTypeRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of constitutional symptom type.
        /// </summary>
        /// <returns>Returns a list of all constitutional symptom types.</returns>
        public async Task<IEnumerable<ConstitutionalSymptomType>> GetConstitutionalSymptomTypes()
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
        /// The method is used to Get constitutional symptom types by constitutional symtom.
        /// </summary>
        /// <param name="constitutionalSymtomId"></param>
        /// <returns>Returns a constitutional symptom types by constitutional symtom.</returns>
        public async Task<IEnumerable<ConstitutionalSymptomType>> GetConstitutionalSymptomTypesByConstitutionalSymtom(int constitutionalSymtomId)
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false && c.ConstitutionalSymptomId == constitutionalSymtomId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a ConstitutionalSymptomType by key.
        /// </summary>
        /// <param name="key">Primary key of the table ConstitutionalSymptomType.</param>
        /// <returns>Returns a ConstitutionalSymptomType if the key is matched.</returns>
        public async Task<ConstitutionalSymptomType> GetConstitutionalSymptomTypeByKey(int key)
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
        /// The method is used to get an ConstitutionalSymptomType by ConstitutionalSymptomType Description.
        /// </summary>
        /// <param name="description">Name of an ConstitutionalSymptomType.</param>
        /// <returns>Returns an ConstitutionalSymptomType if the ConstitutionalSymptomType description is matched.</returns>
        public async Task<ConstitutionalSymptomType> GetConstitutionalSymptomTypeByName(string constitutionalSymptomType)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == constitutionalSymptomType.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}