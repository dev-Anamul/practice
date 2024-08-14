using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Bella
 * Date created : 13.01.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   /// <summary>
   /// Implementation of IHomeLanguageRepository interface.
   /// </summary>
   public class HomeLanguageRepository : Repository<HomeLanguage>, IHomeLanguageRepository
   {
      public HomeLanguageRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get a home language by home language name.
      /// </summary>
      /// <param name="homeLanguageName">Name of a home language.</param>
      /// <returns>Returns a home language if the home language name is matched.</returns>
      public async Task<HomeLanguage> GetHomeLanguageByName(string homeLanguageName)
      {
         try
         {
            return await FirstOrDefaultAsync(h => h.Description.ToLower().Trim() == homeLanguageName.ToLower().Trim() && h.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a home language by key.
      /// </summary>
      /// <param name="key">Primary key of the table HomeLanguages.</param>
      /// <returns>Returns a home language if the key is matched.</returns>
      public async Task<HomeLanguage> GetHomeLanguageByKey(int key)
      {
         try
         {
            return await FirstOrDefaultAsync(h => h.Oid == key && h.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of home languages.
      /// </summary>
      /// <returns>Returns a list of all home languages.</returns>
      public async Task<IEnumerable<HomeLanguage>> GetHomeLanguages()
      {
         try
         {
            return await QueryAsync(h => h.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}