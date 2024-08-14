using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 13.01.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IHomeLanguageRepository : IRepository<HomeLanguage>
   {
      /// <summary>
      /// The method is used to get a home language by home language name.
      /// </summary>
      /// <param name="homeLanguageName">Name of a home language.</param>
      /// <returns>Returns a home language if the home language name is matched.</returns>
      public Task<HomeLanguage> GetHomeLanguageByName(string homeLanguageName);

      /// <summary>
      /// The method is used to get a home language by key.
      /// </summary>
      /// <param name="key">Primary key of the table HomeLanguages.</param>
      /// <returns>Returns a home language if the key is matched.</returns>
      public Task<HomeLanguage> GetHomeLanguageByKey(int key);

      /// <summary>
      /// The method is used to get the list of home languages.
      /// </summary>
      /// <returns>Returns a list of all home languages.</returns>
      public Task<IEnumerable<HomeLanguage>> GetHomeLanguages();
   }
}