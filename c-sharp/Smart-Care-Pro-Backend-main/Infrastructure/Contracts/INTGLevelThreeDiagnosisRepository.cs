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
    public interface INTGLevelThreeDiagnosisRepository : IRepository<NTGLevelThreeDiagnosis>
    {
        /// <summary>
        /// The method is used to get an NTG level three diagnosis by key.
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelThreeDiagnoses.</param>
        /// <returns>Returns an NTG level three diagnosis if the key is matched.</returns>
        public Task<NTGLevelThreeDiagnosis> GetNTGLevelThreeDiagnosisByKey(int key);

        /// <summary>
        /// The method is used to get an NTG level three diagnosis by NTGLevelTwoID.
        /// </summary>
        /// <param name="ntgLevelTwoId">Primary key of the table NTGLevelThreeDiagnoses.</param>
        /// <returns>Returns an NTG level three diagnosis if the NTGLevelTwoID is matched.</returns>
        public Task<IEnumerable<NTGLevelThreeDiagnosis>> GetNTGLevelThreeDiagnosisByNTGLevelTwoDiagnosis(int ntgLevelTwoId);

        /// <summary>
        /// The method is used to get the list of NTG level three diagnoses.
        /// </summary>
        /// <returns>Returns a list of all NTG level three diagnoses.</returns>
        public Task<IEnumerable<NTGLevelThreeDiagnosis>> GetNTGLevelThreeDiagnoses();

        public Task<NTGLevelThreeDiagnosis> GetNTGLevelThreeDiagnosesByName(string nTgLevelThreeDiagnosis);




    }
}