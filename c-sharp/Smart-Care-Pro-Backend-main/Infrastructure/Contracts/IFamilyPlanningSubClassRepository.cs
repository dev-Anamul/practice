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
   public interface IFamilyPlanningSubClassRepository : IRepository<FamilyPlanningSubclass>
   {
      /// <summary>
      /// The method is used to get the list of FamilyPlanningSubClasses.
      /// </summary>
      /// <returns>Returns a list of all FamilyPlanningSubClasses.</returns>
      public Task<IEnumerable<FamilyPlanningSubclass>> GetFamilyPlanningSubClasses();

      /// <summary>
      /// The method is used to get the list of FamilyPlanningSubClass  .
      /// </summary>
      /// <param name="key"></param>
      /// <returns>Returns a list of all FamilyPlanningSubClass by key.</returns>
      public Task<FamilyPlanningSubclass> GetFamilyPlanningSubClassByKey(int key);

      /// <summary>
      /// The method is used to get an FamilyPlanningSubclass by FamilyPlanningSubclass Description.
      /// </summary>
      /// <param name="familyPlanningSubclass">Description of an FamilyPlanningSubclass.</param>
      /// <returns>Returns an FamilyPlanningSubclass if the FamilyPlanningSubclass name is matched.</returns>
      public Task<FamilyPlanningSubclass> GetFamilyPlanningSubclassByName(string familyPlanningSubclass);

      /// <summary>
      /// The method is used to get the FamilyPlanningSubclass by ClassId.
      /// </summary>
      /// <param name="classId">ClassId of the table FamilyPlanningClass.</param>
      /// <returns>Returns a FamilyPlanningSubclass if the ClassId is matched.</returns>
      public Task<IEnumerable<FamilyPlanningSubclass>> GetFamilyPlanningSubclassByClass(int classId);
   }
}