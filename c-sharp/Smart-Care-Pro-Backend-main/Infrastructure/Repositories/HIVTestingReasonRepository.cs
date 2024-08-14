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
   /// Implementation of IHIVTestingReasonRepository interface.
   /// </summary>
   public class HIVTestingReasonRepository : Repository<HIVTestingReason>, IHIVTestingReasonRepository
   {
      public HIVTestingReasonRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get reason for testing HIV by testing reason.
      /// </summary>
      /// <param name="testingReason">Reason for testing HIV.</param>
      /// <returns>Returns a reason for testing HIV if the testing reason is matched.</returns>
      public async Task<HIVTestingReason> GetHIVTestingReasonByTestingReason(string testingReason)
      {
         try
         {
            return await FirstOrDefaultAsync(h => h.Description.ToLower().Trim() == testingReason.ToLower().Trim() && h.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get reason for testing HIV by key.
      /// </summary>
      /// <param name="key">Primary key of the table HIVTestingReasons.</param>
      /// <returns>Returns a reason for testing HIV if the key is matched.</returns>
      public async Task<HIVTestingReason> GetHIVTestingReasonByKey(int key)
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
      /// The method is used to get the list of reasons for testing HIV.
      /// </summary>
      /// <returns>Returns a list of all reasons for testing HIV.</returns>
      public async Task<IEnumerable<HIVTestingReason>> GetHIVTestingReasons()
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