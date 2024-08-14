using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Bella
 * Last modified: 14.08.2023 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IInteractionRepository : IRepository<Interaction>
   {
      /// <summary>
      /// The method is used to get an interaction by key.
      /// </summary>
      /// <param name="key">Primary key of the table Interactions.</param>
      /// <returns>Returns an interaction if the key is matched.</returns>
      public Task<Interaction> GetInteractionByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of interactions.
      /// </summary>
      /// <returns>Returns a list of all interactions.</returns>
      public Task<IEnumerable<Interaction>> GetInteractions();
   }
}