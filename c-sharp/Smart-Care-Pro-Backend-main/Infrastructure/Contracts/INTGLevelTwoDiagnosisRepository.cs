using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 04.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface INTGLevelTwoDiagnosisRepository : IRepository<NTGLevelTwoDiagnosis>
    {
        /// <summary>
        /// The method is used to get an NTG level two diagnosis by key.
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelTwoDiagnoses.</param>
        /// <returns>Returns an NTG level two diagnosis if the key is matched.</returns>
        public Task<NTGLevelTwoDiagnosis> GetNTGLevelTwoDiagnosisByKey(int key);

        /// <summary>
        /// The method is used to get an NTG level two diagnosis by NTGLevelOneID.
        /// </summary>
        /// <param name="ntgLevelOneId">Primary key of the table NTGLevelTwoDiagnoses.</param>
        /// <returns>Returns an NTG level two diagnosis if the NTGLevelOneID is matched.</returns>
        public Task<IEnumerable<NTGLevelTwoDiagnosis>> GetNTGLevelTwoDiagnosisByNTGLevelOneDiagnosis(int ntgLevelOneId);

        /// <summary>
        /// The method is used to get the list of NTG level two diagnoses.
        /// </summary>
        /// <returns>Returns a list of all NTG level two diagnoses.</returns>
        public Task<IEnumerable<NTGLevelTwoDiagnosis>> GetNTGLevelTwoDiagnoses();

        public Task<NTGLevelTwoDiagnosis> GetNTGLevelTwoDiagnosesByName(string nTgLevelTwoDiagnosis);


    }
}