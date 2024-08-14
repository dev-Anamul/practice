using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface INTGLevelOneDiagnosisRepository : IRepository<NTGLevelOneDiagnosis>
    {
        /// <summary>
        /// The method is used to get an NTG level one diagnosis by key.
        /// </summary>
        /// <param name="key">Primary key of the table NTGLevelOneDiagnoses.</param>
        /// <returns>Returns an NTG level one diagnosis if the key is matched.</returns>
        public Task<NTGLevelOneDiagnosis> GetNTGLevelOneDiagnosisByKey(int key);

        /// <summary>
        /// The method is used to get the list of NTG level one diagnoses.
        /// </summary>
        /// <returns>Returns a list of all NTG level one diagnoses.</returns>
        public Task<IEnumerable<NTGLevelOneDiagnosis>> GetNTGLevelOneDiagnoses();

        /// <summary>
        /// The method is used to get an NTGLevelOneDiagnosis by NTGLevelOneDiagnosis Description.
        /// </summary>
        /// <param name="nTgLevelOneDiagnosis">Description of an NTGLevelOneDiagnosis.</param>
        /// <returns>Returns an NTGLevelOneDiagnosis if the NTGLevelOneDiagnosis name is matched.</returns>
        public Task<NTGLevelOneDiagnosis> GetNTGLevelOneDiagnosisByName(string nTgLevelOneDiagnosis);
    }
}