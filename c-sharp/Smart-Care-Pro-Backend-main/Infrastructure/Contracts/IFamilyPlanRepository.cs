using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IFamilyPlanRepository : IRepository<FamilyPlan>
   {
      /// <summary>
      /// The method is used to get a FamilyPlan by key.
      /// </summary>
      /// <param name="key">Primary key of the table FamilyPlans.</param>
      /// <returns>Returns a FamilyPlan if the key is matched.</returns>
      public Task<FamilyPlan> GetFamilyPlanByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of FamilyPlans.
      /// </summary>
      /// <returns>Returns a list of all FamilyPlans.</returns>
      public Task<IEnumerable<FamilyPlan>> GetFamilyPlans();

      /// <summary>
      /// The method is used to get a FamilyPlan by ClientID.
      /// </summary>
      /// <param name="clientId"></param>
      /// <returns>Returns a FamilyPlan if the ClientID is matched.</returns>
      public Task<IEnumerable<FamilyPlan>> GetFamilyPlanByClient(Guid clientId);

      /// <summary>
      /// The method is used to get the list of FamilyPlan by EncounterID.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns a list of all FamilyPlan by EncounterID.</returns>
      public Task<IEnumerable<FamilyPlan>> GetFamilyPlanByEncounter(Guid encounterId);
   }
}