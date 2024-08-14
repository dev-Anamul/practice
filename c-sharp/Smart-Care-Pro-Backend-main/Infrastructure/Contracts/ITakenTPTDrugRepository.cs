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
   public interface ITakenTPTDrugRepository : IRepository<TakenTPTDrug>
   {
      /// <summary>
      /// The method is used to get a TakenTPTDrug by key.
      /// </summary>
      /// <param name="key">Primary key of the table TakenTPTDrugs.</param>
      /// <returns>Returns a TakenTPTDrug if the key is matched.</returns>
      public Task<TakenTPTDrug> GetTakenTPTDrugByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of TakenTPTDrugs.
      /// </summary>
      /// <returns>Returns a list of all TakenTPTDrugs.</returns>
      public Task<IEnumerable<TakenTPTDrug>> GetTakenTPTDrugs();

      /// <summary>
      /// The method is used to get a TakenTPTDrug by OPD visit.
      /// </summary>
      /// <param name="EncounterID"></param>
      /// <returns>Returns a TakenTPTDrug if the Encounter is matched.</returns>
      public Task<IEnumerable<TakenTPTDrug>> GetTakenTPTDrugByEncounter(Guid EncounterID);
   }
}