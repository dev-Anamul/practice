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
    /// <summary>
    /// Implementation of INTGLevelOneDiagnosisRepository interface.
    /// </summary>
    public class NTGLevelOneDiagnosisRepository : Repository<NTGLevelOneDiagnosis>, INTGLevelOneDiagnosisRepository
    {
        public NTGLevelOneDiagnosisRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an NTG level one diagnosis by key.
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelOneDiagnoses.</param>
        /// <returns>Returns an NTG level one diagnosis if the key is matched.</returns>
        public async Task<NTGLevelOneDiagnosis> GetNTGLevelOneDiagnosisByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(n => n.Oid == key && n.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of NTG level one diagnoses.
        /// </summary>
        /// <returns>Returns a list of all NTG level one diagnoses.</returns> 
        public async Task<IEnumerable<NTGLevelOneDiagnosis>> GetNTGLevelOneDiagnoses()
        {
            try
            {
                return await LoadListWithChildAsync<NTGLevelOneDiagnosis>(n => n.IsDeleted == false, p => p.NTGLevelTwoDiagnoses);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an NTGLevelOneDiagnosis by NTGLevelOneDiagnosis Description.
        /// </summary>
        /// <param name="nTgLevelOneDiagnosis">Name of an NTGLevelOneDiagnosis.</param>
        /// <returns>Returns an NTGLevelOneDiagnosis if the NTGLevelOneDiagnosis description is matched.</returns>
        public async Task<NTGLevelOneDiagnosis> GetNTGLevelOneDiagnosisByName(string nTgLevelOneDiagnosis)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == nTgLevelOneDiagnosis.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}