using Domain.Entities;

/*
 * Created by   : Bithy
 * Date created : 07-02-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IPainRecordRepository : IRepository<PainRecord>
   {
      /// <summary>
      /// The method is used to get a pain record by key.
      /// </summary>
      /// <param name="key">Primary key of the table PainRecords.</param>
      /// <returns>Returns a PainRecords if the key is matched.</returns>
      public Task<PainRecord> GetPainRecordByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of PainRecords.
      /// </summary>
      /// <returns>Returns a list of all PainRecords.</returns>
      public Task<IEnumerable<PainRecord>> GetPainRecords();

      /// <summary>
      /// The method is used to get a PainRecord by ClientID.
      /// </summary>
      /// <param name="ClientID"></param>
      /// <returns>Returns a PainRecord if the ClientID is matched.</returns>
      public Task<IEnumerable<PainRecord>> GetPainRecordByClient(Guid ClientID);

      /// <summary>
      /// The method is used to get a PainRecord by OPD visit.
      /// </summary>
      /// <param name="EncounterID"></param>
      /// <returns>Returns a PainRecord if the Encounter is matched.</returns>
      public Task<IEnumerable<PainRecord>> GetPainRecordByEncounter(Guid EncounterID);

      /// <summary>
      /// The method is used to get a pain records by OPD visit.
      /// </summary>
      /// <param name="OPDVisitID"></param>
      /// <returns>Returns a pain records if the OPD visit ID is matched.</returns>
      public Task<IEnumerable<PainRecord>> GetPainRecordByOpdVisit(Guid OPDVisitID);
   }
}