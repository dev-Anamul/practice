using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IDeathRecordRepository : IRepository<DeathRecord>
   {
      /// <summary>
      /// The method is used to get a death record by key.
      /// </summary>
      /// <param name="key">Primary key of the table BirthHistories.</param>
      /// <returns>Returns a death record if the key is matched.</returns>
      public Task<DeathRecord> GetDeathRecordByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of birth histories.
      /// </summary>
      /// <returns>Returns a list of all birth histories.</returns>
      public Task<IEnumerable<DeathRecord>> GetDeathRecords();

      /// <summary>
      /// The method is used to get a death record by ClientID.
      /// </summary>
      /// <param name="ClientID"></param>
      /// <returns>Returns a death record if the ClientID is matched.</returns>
      public Task<DeathRecord> GetDeathRecordByClient(Guid clientId);

      /// <summary>
      /// The method is used to get a death record by OPD visit.
      /// </summary>
      /// <param name="EncounterID"></param>
      /// <returns>Returns a death record if the OPD EncounterID is matched.</returns>
      public Task<IEnumerable<DeathRecord>> GetDeathRecordByEncounterID(Guid encounterId);
   }
}