using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Rezwana
 * Date created : 13.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   /// <summary>
   /// Implementation of ISkinPreparationRepository interface.
   /// </summary>
   public class SkinPreparationRepository : Repository<SkinPreparation>, ISkinPreparationRepository
   {
      public SkinPreparationRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get a skin preparation by key.
      /// </summary>
      /// <param name="key">Primary key of the table SkinPreparations.</param>
      /// <returns>Returns a skin preparation if the key is matched.</returns>
      public async Task<SkinPreparation> GetSkinPreparationByKey(Guid key)
      {
         try
         {
            return await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false );
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of skin preparation.
      /// </summary>
      /// <returns>Returns a list of all skin preparation.</returns>
      public async Task<IEnumerable<SkinPreparation>> GetSkinPreparations()
      {
         try
         {
            return await QueryAsync(s => s.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a skin preparation by EncounterID.
      /// </summary>
      /// <param name="EncounterID">Primary key of the table EncounterBaseModel.</param>
      /// <returns>Returns a skin preparation if the EncounterID is matched.</returns>
      public async Task<IEnumerable<SkinPreparation>> GetSkinPreparationByEncounter(Guid EncounterID)
      {
         try
         {
            return await QueryAsync(s => s.IsDeleted == false && s.EncounterId == EncounterID);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get an skinPreparation by AnestheticPlanID.
      /// </summary>
      /// <param name="AnestheticPlanID">Primary key of the table AnestheticPlans.</param>
      /// <returns>Returns a skinPreparation if the SurgeryID is matched.</returns>
      public async Task<SkinPreparation> GetSkinPreparationByAnestheticPlan(Guid AnestheticPlanID)
      {
         try
         {
            return await FirstOrDefaultAsync(a => a.IsDeleted == false && a.AnestheticPlanId == AnestheticPlanID);
         }
         catch (Exception)
         {
            throw;
         }
      }

      public async Task<IEnumerable<SkinPreparation>> GetSkinPreparationListByAnestheticPlan(Guid AnestheticPlanID)
      {
         try
         {
            return await QueryAsync(a => a.IsDeleted == false && a.AnestheticPlanId == AnestheticPlanID);
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}