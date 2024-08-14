using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 04.01.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IICDDiagnosisRepository : IRepository<ICDDiagnosis>
   {
      /// <summary>
      /// The method is used to get an ICD diagnosis by key.
      /// </summary>
      /// <param name="key">Primary key of the table ICDDiagnoses.</param>
      /// <returns>Returns an ICD diagnosis if the key is matched.</returns>
      public Task<ICDDiagnosis> GetICDDiagnosisByKey(int key);

      /// <summary>
      /// The method is used to get the list of ICD diagnoses.
      /// </summary>
      /// <returns>Returns a list of all ICD diagnoses.</returns>
      public Task<IEnumerable<ICDDiagnosis>> GetICDDiagnoses();
      public Task<IEnumerable<ICDDiagnosis>> GetICDDiagnosesBySearchTerm(string searchTerm);

      /// <summary>
      /// The method is used to get an ICD diagnosis by ICPC2.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public Task<IEnumerable<ICDDiagnosis>> GetICDDiagnosesByICPC2(int id);

      /// <summary>
      /// The method is used to get an ICDDiagnosis by ICDDiagnosis Description.
      /// </summary>
      /// <param name="iCDDiagnosis">Description of an ICDDiagnosis.</param>
      /// <returns>Returns an ICDDiagnosis if the ICDDiagnosis name is matched.</returns>
      public Task<ICDDiagnosis> GetICDDiagnosisByName(string iCDDiagnosis);
   }
}