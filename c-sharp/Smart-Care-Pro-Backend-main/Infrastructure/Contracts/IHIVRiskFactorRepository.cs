using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 13.01.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IHIVRiskFactorRepository : IRepository<HIVRiskFactor>
   {
      /// <summary>
      /// The method is used to get the risk factor of HIV by risk factor.
      /// </summary>
      /// <param name="riskFactor">Risk factor of HIV.</param>
      /// <returns>Returns a risk factor of HIV if the risk factor is matched.</returns>
      public Task<HIVRiskFactor> GetHIVRiskFactorByRiskFactor(string riskFactor);

      /// <summary>
      /// The method is used to get the risk factor of HIV by key.
      /// </summary>
      /// <param name="key">Primary key of the table HIVRiskFactors.</param>
      /// <returns>Returns a risk factor of HIV if the key is matched.</returns>
      public Task<HIVRiskFactor> GetHIVRiskFactorByKey(int key);

      /// <summary>
      /// The method is used to get the list of risk factors of HIV. 
      /// </summary>
      /// <returns>Returns a list of all risk factors of HIV.</returns>
      public Task<IEnumerable<HIVRiskFactor>> GetHIVRiskFactors();
   }
}