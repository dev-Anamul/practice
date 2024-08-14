using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Tomas
 * Date created : 04.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of INTGLevelTwoDiagnosisRepository interface.
    /// </summary>
    public class NTGLevelTwoDiagnosisRepository : Repository<NTGLevelTwoDiagnosis>, INTGLevelTwoDiagnosisRepository
    {
        public NTGLevelTwoDiagnosisRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an NTG level two diagnosis by key.
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelTwoDiagnoses.</param>
        /// <returns>Returns an NTG level two diagnosis if the key is matched.</returns>
        public async Task<NTGLevelTwoDiagnosis> GetNTGLevelTwoDiagnosisByKey(int key)
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
        /// The method is used to get an NTG level two diagnosis by NTGLevelOneID.
        /// </summary>
        /// <param name="ntgLevelOneId">Primary key of the table NTGLevelTwoDiagnoses.</param>
        /// <returns>Returns an NTG level two diagnosis if the NTGLevelOneID is matched.</returns>
        public async Task<IEnumerable<NTGLevelTwoDiagnosis>> GetNTGLevelTwoDiagnosisByNTGLevelOneDiagnosis(int ntgLevelOneId)
        {
            try
            {
                return await QueryAsync(n => n.IsDeleted == false && n.NTGLevelOneId == ntgLevelOneId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of NTG level two diagnoses.
        /// </summary>
        /// <returns>Returns a list of all NTG level two diagnoses.</returns>
        public async Task<IEnumerable<NTGLevelTwoDiagnosis>> GetNTGLevelTwoDiagnoses()
        {
            try
            {
                return await QueryAsync(n => n.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an NTGLevelTwoDiagnosis by NTGLevelTwoDiagnosis Description.
        /// </summary>
        /// <param name="nTgLevelTwoDiagnosis">Name of an NTGLevelTwoDiagnosis.</param>
        /// <returns>Returns an NTGLevelTwoDiagnosis if the NTGLevelTwoDiagnosis description is matched.</returns>
        public async Task<NTGLevelTwoDiagnosis> GetNTGLevelTwoDiagnosesByName(string nTgLevelTwoDiagnosis)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == nTgLevelTwoDiagnosis.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

      
    }
}