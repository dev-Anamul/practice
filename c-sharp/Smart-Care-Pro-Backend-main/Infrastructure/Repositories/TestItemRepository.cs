using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Bithy
 * Date created : 22.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   public class TestItemRepository : Repository<TestItem>, ITestItemRepository
   {
      public TestItemRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get a TestItem by key.
      /// </summary>
      /// <param name="key">Primary key of the table TestItems.</param>
      /// <returns>Returns a TestItem  if the key is matched.</returns>
      public async Task<TestItem> GetTestItemByKey(int key)
      {
         try
         {
            return await FirstOrDefaultAsync(d => d.Oid == key && d.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of TestItem.
      /// </summary>
      /// <returns>Returns a list of all covid TestItems.</returns>
      public async Task<IEnumerable<TestItem>> GetTestItems()
      {
         try
         {
            return await QueryAsync(b => b.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}