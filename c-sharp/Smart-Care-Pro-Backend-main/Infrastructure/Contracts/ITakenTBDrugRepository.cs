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
   public interface ITakenTBDrugRepository : IRepository<TakenTBDrug>
   {
      /// <summary>
      /// The method is used to get a TakenTBDrug by key.
      /// </summary>
      /// <param name="key">Primary key of the table TakenTBDrugs.</param>
      /// <returns>Returns a TakenTBDrug if the key is matched.</returns>
      public Task<TakenTBDrug> GetTakenTBDrugByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of TakenTBDrugs.
      /// </summary>
      /// <returns>Returns a list of all TakenTBDrugs.</returns>
      public Task<IEnumerable<TakenTBDrug>> GetTakenTBDrugs();

      /// <summary>
      /// The method is used to get a TakenTBDrug by Encounter.
      /// </summary>
      /// <param name="EncounterID"></param>
      /// <returns>Returns a TakenTBDrug if the Encounter is matched.</returns>
      public Task<IEnumerable<TakenTBDrug>> GetTakenTBDrugByEncounter(Guid EncounterID);
   }
}