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
    /// Implementation of INTGLevelThreeDiagnosisRepository interface.
    /// </summary>
    public class NTGLevelThreeDiagnosisRepository : Repository<NTGLevelThreeDiagnosis>, INTGLevelThreeDiagnosisRepository
    {
        public NTGLevelThreeDiagnosisRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an NTG level three diagnosis by key.
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelThreeDiagnoses.</param>
        /// <returns>Returns an NTG level three diagnosis if the key is matched.</returns>
        public async Task<NTGLevelThreeDiagnosis> GetNTGLevelThreeDiagnosisByKey(int key)
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
        /// The method is used to get an NTG level three diagnosis by NTGLevelTwoID.
        /// </summary>
        /// <param name="ntgLevelTwoId">Primary key of the table NTGLevelThreeDiagnoses.</param>
        /// <returns>Returns an NTG level three diagnosis if the NTGLevelTwoID is matched.</returns>
        public async Task<IEnumerable<NTGLevelThreeDiagnosis>> GetNTGLevelThreeDiagnosisByNTGLevelTwoDiagnosis(int ntgLevelTwoId)
        {
            try
            {
                return await QueryAsync(n => n.IsDeleted == false && n.NTGLevelTwoId == ntgLevelTwoId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of NTG level three diagnoses.
        /// </summary>
        /// <returns>Returns a list of all NTG level three diagnoses.</returns>
        public async Task<IEnumerable<NTGLevelThreeDiagnosis>> GetNTGLevelThreeDiagnoses()
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
        /// The method is used to get an NTGLevelThreeDiagnosis by NTGLevelThreeDiagnosis Description.
        /// </summary>
        /// <param name="nTgLevelThreeDiagnosis">Name of an NTGLevelThreeDiagnosis.</param>
        /// <returns>Returns an NTGLevelOneDiagnosis if the NTGLevelOneDiagnosis description is matched.</returns>
        public async Task<NTGLevelThreeDiagnosis> GetNTGLevelThreeDiagnosesByName(string nTgLevelThreeDiagnosis)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == nTgLevelThreeDiagnosis.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}