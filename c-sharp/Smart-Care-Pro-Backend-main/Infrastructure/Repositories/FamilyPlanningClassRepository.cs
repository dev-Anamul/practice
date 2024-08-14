using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   public class FamilyPlanningClassRepository : Repository<FamilyPlanningClass>, IFamilyPlanningClassRepository
   {
      /// <summary>
      /// Implementation of IFamilyPlanningClassRepository interface.
      /// </summary>
      public FamilyPlanningClassRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get a FamilyPlanningClass by key.
      /// </summary>
      /// <param name="key">Primary key of the table FamilyPlanningClasses.</param>
      /// <returns>Returns a FamilyPlanningClass if the key is matched.</returns>
      public async Task<FamilyPlanningClass> GetFamilyPlanningClassByKey(int key)
      {
         try
         {
            return await FirstOrDefaultAsync(p => p.Oid == key && p.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of FamilyPlanningClass.
      /// </summary>
      /// <returns>Returns a list of all FamilyPlanningClasses.</returns>
      public async Task<IEnumerable<FamilyPlanningClass>> GetFamilyPlanningClasses()
      {
         try
         {
            return await QueryAsync(n => n.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get an FamilyPlanningClass by FamilyPlanningClass Description.
      /// </summary>
      /// <param name="familyPlanningClass">Name of an FamilyPlanningClass.</param>
      /// <returns>Returns an FamilyPlanningClass if the FamilyPlanningClass description is matched.</returns>
      public async Task<FamilyPlanningClass> GetFamilyPlanningClassByName(string familyPlanningClass)
      {
         try
         {
            return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == familyPlanningClass.ToLower().Trim() && a.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}