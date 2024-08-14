using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : Bella
 * Last modified: 26.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   public class ARTDrugRepository : Repository<ARTDrug>, IARTDrugRepository
   {
      public ARTDrugRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get an ARTDrug by drug name.
      /// </summary>
      /// <param name="drugName">Name of an ART Drug.</param>
      /// <returns>Returns an ARTDrug if the drug name is matched.</returns>
      public async Task<ARTDrug> GetARTDrugByName(string drugName)
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
      /// The method is used to get an ARTDrug by key.
      /// </summary>
      /// <param name="key">Primary key of the table ARTDrugs.</param>
      /// <returns>Returns an ARTDrug if the key is matched.</returns>
      public async Task<ARTDrug> GetARTDrugByKey(int key)
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
      /// The method is used to get the list of ARTDrugs.
      /// </summary>
      /// <returns>Returns a list of all ARTDrugs.</returns>
      public async Task<IEnumerable<ARTDrug>> GetARTDrugs()
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

      /// <summary>
      /// The method is used to Get ARTDrug by ARTDrugClassId.
      /// </summary>
      /// <param name="artDrugClassId"></param>
      /// <returns>Returns a ARTDrug by ARTDrugClassId.</returns>
      public async Task<IEnumerable<ARTDrug>> GetARTDrugByARTDrugClass(int artDrugClassId)
      {
         try
         {
            return await QueryAsync(c => c.IsDeleted == false && c.ARTDrugClassId == artDrugClassId);
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}