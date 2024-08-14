using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Bella
 * Date created : 13.08.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   /// <summary>
   /// Implementation of IHIVRiskFactorRepository interface.
   /// </summary>
   public class HIVRiskFactorRepository : Repository<HIVRiskFactor>, IHIVRiskFactorRepository
   {
      public HIVRiskFactorRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get the risk factor of HIV by risk factor.
      /// </summary>
      /// <param name="riskFactor">Risk factor of HIV.</param>
      /// <returns>Returns a risk factor of HIV if the risk factor is matched.</returns>
      public async Task<HIVRiskFactor> GetHIVRiskFactorByRiskFactor(string riskFactor)
      {
         try
         {
            return await FirstOrDefaultAsync(h => h.Description.ToLower().Trim() == riskFactor.ToLower().Trim() && h.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the risk factor of HIV by key.
      /// </summary>
      /// <param name="key">Primary key of the table HIVRiskFactors.</param>
      /// <returns>Returns a risk factor of HIV if the key is matched.</returns>
      public async Task<HIVRiskFactor> GetHIVRiskFactorByKey(int key)
      {
         try
         {
            return await FirstOrDefaultAsync(h => h.Oid == key && h.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of risk factors of HIV. 
      /// </summary>
      /// <returns>Returns a list of all risk factors of HIV.</returns>
      public async Task<IEnumerable<HIVRiskFactor>> GetHIVRiskFactors()
      {
         try
         {
            return await QueryAsync(h => h.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}