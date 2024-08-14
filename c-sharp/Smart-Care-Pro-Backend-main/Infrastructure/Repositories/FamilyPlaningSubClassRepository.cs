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
   public class FamilyPlanningSubClassRepository : Repository<FamilyPlanningSubclass>, IFamilyPlanningSubClassRepository
   {
      /// <summary>
      /// Implementation of IFamilyPlanningSubClassRepository interface.
      /// </summary>
      public FamilyPlanningSubClassRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get a FamilyPlanningSubClass by key.
      /// </summary>
      /// <param name="key">Primary key of the table FamilyPlanningSubClasses.</param>
      /// <returns>Returns a FamilyPlanningSubClass if the key is matched.</returns>
      public async Task<FamilyPlanningSubclass> GetFamilyPlanningSubClassByKey(int key)
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
      /// The method is used to get the list of FamilyPlanningSubClass.
      /// </summary>
      /// <returns>Returns a list of all FamilyPlanningSubClasses.</returns>
      public async Task<IEnumerable<FamilyPlanningSubclass>> GetFamilyPlanningSubClasses()
      {
         try
         {
            return await QueryAsync(n => n.IsDeleted == false, h => h.FamilyPlanningClass);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get an FamilyPlanningSubclass by FamilyPlanningSubclass Description.
      /// </summary>
      /// <param name="familyPlanningSubclass">Name of an FamilyPlanningSubclass.</param>
      /// <returns>Returns an FamilyPlanningSubclass if the FamilyPlanningSubclass description is matched.</returns>
      public async Task<FamilyPlanningSubclass> GetFamilyPlanningSubclassByName(string familyPlanningSubclass)
      {
         try
         {
            return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == familyPlanningSubclass.ToLower().Trim() && a.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the FamilyPlanningSubclass by classId.
      /// </summary>
      /// <param name="classId">ClassId of the table FamilyPlanningSubclass.</param>
      /// <returns>Returns a FamilyPlanningSubclass if the classId is matched.</returns>
      public async Task<IEnumerable<FamilyPlanningSubclass>> GetFamilyPlanningSubclassByClass(int classId)
      {
         try
         {
            var familyPlanSubclass = await QueryAsync(d => d.IsDeleted == false && d.ClassId == classId, p => p.FamilyPlanningClass);

            return familyPlanSubclass.OrderBy(d => d.Description);
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}