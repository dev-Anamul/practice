using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 22.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface ITestItemRepository : IRepository<TestItem>
   {
      /// <summary>
      /// The method is used to get a TestItem by key.
      /// </summary>
      /// <param name="key">Primary key of the table TestItems.</param>
      /// <returns>Returns a TestItem if the key is matched.</returns>
      public Task<TestItem> GetTestItemByKey(int key);

      /// <summary>
      /// The method is used to get the list of TestItem.
      /// </summary>
      /// <returns>Returns a list of all TestItems.</returns>
      public Task<IEnumerable<TestItem>> GetTestItems();
   }
}