using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Tomas
 * Date created : 05.03.2023
 * Modified by  : Bella  
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   public class GynConfirmationRepository : Repository<GynConfirmation>, IGynConfirmationRepository
   {
      /// <summary>
      /// Implementation of GynConfirmationRepository.
      /// </summary>
      public GynConfirmationRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get a Gyn Confirmation by key.
      /// </summary>
      /// <param name="key"></param>
      /// <returns>Returns a GynConfirmation if the key is matched.</returns>
      public async Task<GynConfirmation> GetGynConfirmationByKey(int key)
      {
         try
         {
            return await FirstOrDefaultAsync(g => g.Oid == key && g.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a GynConfirmation.
      /// </summary>
      /// <returns>Returns a GynConfirmation.</returns>
      public async Task<IEnumerable<GynConfirmation>> GetGynConfirmation()
      {
         try
         {
            return await QueryAsync(g => g.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get an GynConfirmation by GynConfirmation Description.
      /// </summary>
      /// <param name="gynConfirmation">Name of an GynConfirmation.</param>
      /// <returns>Returns an GynConfirmation if the GynConfirmation gynConfirmation is matched.</returns>
      public async Task<GynConfirmation> GetGynConfirmationByName(string gynConfirmation)
      {
         try
         {
            return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == gynConfirmation.ToLower().Trim() && a.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}