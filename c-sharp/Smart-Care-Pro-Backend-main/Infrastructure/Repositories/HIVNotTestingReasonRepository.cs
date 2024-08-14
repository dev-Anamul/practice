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
   /// Implementation of IHIVNotTestingReasonRepository interface.
   /// </summary>
   public class HIVNotTestingReasonRepository : Repository<HIVNotTestingReason>, IHIVNotTestingReasonRepository
   {
      public HIVNotTestingReasonRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get reason for not testing HIV by not testing reason.
      /// </summary>
      /// <param name="notTestingReason">Reason for not testing HIV.</param>
      /// <returns>Returns a reason for not testing HIV if the not testing reason is matched.</returns>
      public async Task<HIVNotTestingReason> GetHIVNotTestingReasonByNotTestingReason(string notTestingReason)
      {
         try
         {
            return await FirstOrDefaultAsync(h => h.Description.ToLower().Trim() == notTestingReason.ToLower().Trim() && h.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get reason for not testing HIV by key.
      /// </summary>
      /// <param name="key">Primary key of the table HIVNotTestingReasons.</param>
      /// <returns>Returns a reason for not testing HIV if the key is matched.</returns>
      public async Task<HIVNotTestingReason> GetHIVNotTestingReasonByKey(int key)
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
      /// The method is used to get the list of reasons for not testing HIV.
      /// </summary>
      /// <returns>Returns a list of all reasons for not testing HIV.</returns>
      public async Task<IEnumerable<HIVNotTestingReason>> GetHIVNotTestingReasons()
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