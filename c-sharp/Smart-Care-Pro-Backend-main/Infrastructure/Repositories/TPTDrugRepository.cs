using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Bithy
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   public class TPTDrugRepository : Repository<TPTDrug>, ITPTDrugRepository
   {
      public TPTDrugRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get an TPTDrug by drug name.
      /// </summary>
      /// <param name="anatomicAxis">Name of an TPTDrug.</param>
      /// <returns>Returns an TPTDrug if the drug name is matched.</returns>
      public async Task<TPTDrug> GetTPTDrugByName(string drugName)
      {
         try
         {
            return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == drugName.ToLower().Trim() && a.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get an TPTDrug by key.
      /// </summary>
      /// <param name="key">Primary key of the table TPTDrugs.</param>
      /// <returns>Returns an TPTDrug if the key is matched.</returns>
      public async Task<TPTDrug> GetTPTDrugByKey(int key)
      {
         try
         {
            return await FirstOrDefaultAsync(a => a.Oid == key && a.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of TPTDrugs.
      /// </summary>
      /// <returns>Returns a list of all TPTDrugs.</returns>
      public async Task<IEnumerable<TPTDrug>> GetTPTDrugs()
      {
         try
         {
            return await QueryAsync(a => a.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}