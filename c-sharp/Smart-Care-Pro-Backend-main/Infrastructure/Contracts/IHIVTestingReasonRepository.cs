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
   public interface IHIVTestingReasonRepository : IRepository<HIVTestingReason>
   {
      /// <summary>
      /// The method is used to get reason for testing HIV by testing reason.
      /// </summary>
      /// <param name="testingReason">Reason for testing HIV.</param>
      /// <returns>Returns a reason for testing HIV if the testing reason is matched.</returns>
      public Task<HIVTestingReason> GetHIVTestingReasonByTestingReason(string testingReason);

      /// <summary>
      /// The method is used to get reason for testing HIV by key.
      /// </summary>
      /// <param name="key">Primary key of the table HIVTestingReasons.</param>
      /// <returns>Returns a reason for testing HIV if the key is matched.</returns>
      public Task<HIVTestingReason> GetHIVTestingReasonByKey(int key);

      /// <summary>
      /// The method is used to get the list of reasons for testing HIV.
      /// </summary>
      /// <returns>Returns a list of all reasons for testing HIV.</returns>
      public Task<IEnumerable<HIVTestingReason>> GetHIVTestingReasons();
   }
}