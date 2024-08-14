using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : Bella
 * Last modified: 10.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IGynObsHistoryRepository : IRepository<GynObsHistory>
   {

      /// <summary>
      /// The method is used to get a gyn obs history by key.
      /// </summary>
      /// <param name="key">Primary key of the table GynObsHistories.</param>
      /// <returns>Returns a gyn obs history if the key is matched.</returns>
      public Task<GynObsHistory> GetGynObsHistoryByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of gyn obs histories.
      /// </summary>
      /// <returns>Returns a list of all gyn obs histories.</returns>
      public Task<IEnumerable<GynObsHistory>> GetGynObsHistories();

      /// <summary>
      /// The method is used to get a gyn obs histories by encounterId.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns a gyn obs histories if the encounterId is matched.</returns>
      public Task<IEnumerable<GynObsHistory>> GetGynObsHistoryByEncounterId(Guid encounterId);

      /// <summary>
      /// The method is used to get a gyn obs histories by ClientID.
      /// </summary>
      /// <param name="clientId"></param>
      /// <returns>Returns a gyn obs histories if the ClientID is matched.</returns>
      public Task<IEnumerable<GynObsHistory>> GetGynObsHistoryByClientID(Guid clientId);
      public Task<IEnumerable<GynObsHistory>> GetGynObsHistoryByClientIDLast24Hours(Guid clientId);
      public Task<IEnumerable<GynObsHistory>> GetGynObsHistoryByClientID(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetGynObsHistoryByClientIDTotalCount(Guid clientID, EncounterType? encounterType);
        /// <summary>
        /// The method is used to get a gyn obs histories by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a gyn obs histories if the ClientID is matched.</returns>
        public Task<GynObsHistory> GetLatestGynObsHistoryByClientID(Guid clientId);
   }
}