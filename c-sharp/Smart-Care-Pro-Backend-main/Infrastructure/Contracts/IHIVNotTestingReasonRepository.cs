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
   public interface IHIVNotTestingReasonRepository : IRepository<HIVNotTestingReason>
   {
      /// <summary>
      /// The method is used to get reason for not testing HIV by not testing reason.
      /// </summary>
      /// <param name="notTestingReason">Reason for not testing HIV.</param>
      /// <returns>Returns a reason for not testing HIV if the not testing reason is matched.</returns>
      public Task<HIVNotTestingReason> GetHIVNotTestingReasonByNotTestingReason(string notTestingReason);

      /// <summary>
      /// The method is used to get reason for not testing HIV by key.
      /// </summary>
      /// <param name="key">Primary key of the table HIVNotTestingReasons.</param>
      /// <returns>Returns a reason for not testing HIV if the key is matched.</returns>
      public Task<HIVNotTestingReason> GetHIVNotTestingReasonByKey(int key);

      /// <summary>
      /// The method is used to get the list of reasons for not testing HIV.
      /// </summary>
      /// <returns>Returns a list of all reasons for not testing HIV.</returns>
      public Task<IEnumerable<HIVNotTestingReason>> GetHIVNotTestingReasons();
   }
}