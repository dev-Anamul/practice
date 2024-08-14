using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface ITakenARTDrugRepository : IRepository<TakenARTDrug>
   {
      /// <summary>
      /// The method is used to get a TakenARTDrug by key.
      /// </summary>
      /// <param name="key">Primary key of the table TakenARTDrugs.</param>
      /// <returns>Returns a TakenARTDrug if the key is matched.</returns>
      public Task<TakenARTDrug> GetTakenARTDrugByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of TakenARTDrugs.
      /// </summary>
      /// <returns>Returns a list of all TakenARTDrugs.</returns>
      public Task<IEnumerable<TakenARTDrug>> GetTakenARTDrugs();

      /// <summary>
      /// The method is used to get a TakenARTDrug by OPD visit.
      /// </summary>
      /// <param name="EncounterID"></param>
      /// <returns>Returns a TakenARTDrug if the Encounter is matched.</returns>
      public Task<IEnumerable<TakenARTDrug>> GetTakenARTDrugByEncounter(Guid EncounterID);
   }
}