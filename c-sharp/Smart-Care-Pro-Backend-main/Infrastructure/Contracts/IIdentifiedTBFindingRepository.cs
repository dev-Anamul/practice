using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 06.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IIdentifiedTBFindingRepository : IRepository<IdentifiedTBFinding>
   {
      /// <summary>
      /// The method is used to get a IdentifiedTBFinding by key.
      /// </summary>
      /// <param name="key">Primary key of the table IdentifiedTBFinding.</param>
      /// <returns>Returns a IdentifiedTBFinding if the key is matched.</returns>
      public Task<IdentifiedTBFinding> GetIdentifiedTBFindingByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of IdentifiedTBFindings.
      /// </summary>
      /// <returns>Returns a list of all IdentifiedTBFindings.</returns>
      public Task<IEnumerable<IdentifiedTBFinding>> GetIdentifiedTBFindings();

      /// <summary>
      /// The method is used to get a IdentifiedTBFinding by TBFindingId.
      /// </summary>
      /// <param name="tbFindingId"></param>
      /// <returns>Returns a IdentifiedTBFinding if the TBFindingId is matched.</returns>
      public Task<IEnumerable<IdentifiedTBFinding>> GetIdentifiedTBFindingByTBFinding(int tbFindingId);

      /// <summary>
      /// The method is used to get a IdentifiedTBFinding by ClientID.
      /// </summary>
      /// <param name="clientId"></param>
      /// <returns>Returns a IdentifiedTBFinding if the ClientID is matched.</returns>
      public Task<IEnumerable<IdentifiedTBFinding>> GetIdentifiedTBFindingByClient(Guid clientId);
      public Task<IEnumerable<IdentifiedTBFinding>> GetIdentifiedTBFindingByClientLast24Hours(Guid clientId);
      public Task<IEnumerable<IdentifiedTBFinding>> GetIdentifiedTBFindingByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetIdentifiedTBFindingByClientTotalCount(Guid clientID, EncounterType? encounterType);


        /// <summary>
        /// The method is used to get a IdentifiedTBFinding by EncounterId.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a IdentifiedTBFinding if the EncounterId is matched.</returns>
        public Task<IEnumerable<IdentifiedTBFinding>> GetIdentifiedTBFindingByEncounterId(Guid encounterId);
   }
}