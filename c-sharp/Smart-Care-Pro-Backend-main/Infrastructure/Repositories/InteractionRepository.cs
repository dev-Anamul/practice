using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Lion
 * Date created : 13.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   /// <summary>
   /// Implementation of IInteractionRepository interface.
   /// </summary>
   public class InteractionRepository : Repository<Interaction>, IInteractionRepository
   {
      public InteractionRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get an interaction by key.
      /// </summary>
      /// <param name="key">Primary key of the table Interactions.</param>
      /// <returns>Returns an interaction if the key is matched.</returns>
      public async Task<Interaction> GetInteractionByKey(Guid key)
      {
         try
         {
            return await FirstOrDefaultAsync(i => i.Oid == key && i.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of interactions.
      /// </summary>
      /// <returns>Returns a list of all interactions.</returns>
      public async Task<IEnumerable<Interaction>> GetInteractions()
      {
         try
         {
            return await QueryAsync(i => i.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}