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
   public interface ISkinPreparationRepository : IRepository<SkinPreparation>
   {
      /// <summary>
      /// The method is used to get a skin preparation by key.
      /// </summary>
      /// <param name="key">Primary key of the table SkinPreparations.</param>
      /// <returns>Returns a skin preparation if the key is matched.</returns>
      public Task<SkinPreparation> GetSkinPreparationByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of skin preparation.
      /// </summary>
      /// <returns>Returns a list of all skin preparation.</returns>
      public Task<IEnumerable<SkinPreparation>> GetSkinPreparations();

      /// <summary>
      /// The method is used to get a skin preparation by EncounterID.
      /// </summary>
      /// <param name="EncounterID">Primary key of the table EncounterBaseModel.</param>
      /// <returns>Returns a skin preparation if the EncounterID is matched.</returns>
      public Task<IEnumerable<SkinPreparation>> GetSkinPreparationByEncounter(Guid EncounterID);

      /// <summary>
      /// The method is used to get an skinPreparation by AnestheticPlanID.
      /// </summary>
      /// <param name="AnestheticPlanID">Primary key of the table AnestheticPlans.</param>
      /// <returns>Returns an skinPreparation if the AnestheticPlanID is matched.</returns>
      public Task<SkinPreparation> GetSkinPreparationByAnestheticPlan(Guid AnestheticPlanID);

      public Task<IEnumerable<SkinPreparation>> GetSkinPreparationListByAnestheticPlan(Guid AnestheticPlanID);
   }
}