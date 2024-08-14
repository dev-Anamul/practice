using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 28.03.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IFeedingHistoryRepository : IRepository<FeedingHistory>
   {
      /// <summary>
      /// The method is used to get a feeding history by key.
      /// </summary>
      /// <param name="key">Primary key of the table FeedingHistories.</param>
      /// <returns>Returns a feeding history if the key is matched.</returns>
      public Task<FeedingHistory> GetFeedingHistoryByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of feeding histories.
      /// </summary>
      /// <returns>Returns a list of all feeding histories.</returns>
      public Task<IEnumerable<FeedingHistory>> GetFeedingHistories();

      /// <summary>
      /// The method is used to get a feeding history by ClientID.
      /// </summary>
      /// <param name="clientId"></param>
      /// <returns>Returns a feeding history if the ClientID is matched.</returns>
      public Task<IEnumerable<FeedingHistory>> GetFeedingHistoryByClient(Guid clientId);

      /// <summary>
      /// The method is used to get a feeding history by EncounterID.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns a feeding history if the EncounterID is matched.</returns>
      public Task<IEnumerable<FeedingHistory>> GetFeedingHistoryByEncounter(Guid encounterId);
   }
}