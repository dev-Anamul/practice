using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface ITPTDrugRepository : IRepository<TPTDrug>
   {
      /// <summary>
      /// The method is used to get an TPTDrug by drug name.
      /// </summary>
      /// <param name="artDrug">Name of an TPTDrug.</param>
      /// <returns>Returns an TPTDrug if the Drug name is matched.</returns>
      public Task<TPTDrug> GetTPTDrugByName(string artDrug);

      /// <summary>
      /// The method is used to get an TPTDrug by key.
      /// </summary>
      /// <param name="key">Primary key of the table TPTDrugs.</param>
      /// <returns>Returns an TPTDrug if the key is matched.</returns>
      public Task<TPTDrug> GetTPTDrugByKey(int key);

      /// <summary>
      /// The method is used to get the list of TPTDrugs.
      /// </summary>
      /// <returns>Returns a list of all TPTDrugs.</returns>
      public Task<IEnumerable<TPTDrug>> GetTPTDrugs();
   }
}