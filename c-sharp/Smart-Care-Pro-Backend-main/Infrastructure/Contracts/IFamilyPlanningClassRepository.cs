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
   public interface IFamilyPlanningClassRepository : IRepository<FamilyPlanningClass>
   {
      /// <summary>
      /// The method is used to get the list of FamilyPlanningClasses.
      /// </summary>
      /// <returns>Returns a list of all FamilyPlanningClasses.</returns>
      public Task<IEnumerable<FamilyPlanningClass>> GetFamilyPlanningClasses();

      /// <summary>
      /// The method is used to get the FamilyPlanningClass.
      /// </summary>
      /// <returns>Returns a list of all FamilyPlanningClass by key.</returns>
      public Task<FamilyPlanningClass> GetFamilyPlanningClassByKey(int key);

      /// <summary>
      /// The method is used to get an FamilyPlanningClass by FamilyPlanningClass Description.
      /// </summary>
      /// <param name="familyPlanningClass">Description of an FamilyPlanningClass.</param>
      /// <returns>Returns an FamilyPlanningClass if the FamilyPlanningClass name is matched.</returns>
      public Task<FamilyPlanningClass> GetFamilyPlanningClassByName(string familyPlanningClass);
   }
}