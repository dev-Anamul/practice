using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IGynConfirmationRepository : IRepository<GynConfirmation>
   {
      /// <summary>
      /// The method is used to get a GynConfirmation by key.
      /// </summary>
      /// <param name="key">Primary key of the table GynConfirmation.</param>
      /// <returns>Returns a GynConfirmation if the key is matched.</returns>
      public Task<GynConfirmation> GetGynConfirmationByKey(int key);

      /// <summary>
      /// The method is used to get the list of GynConfirmation.
      /// </summary>
      /// <returns>Returns a list of all GynConfirmation.</returns>
      public Task<IEnumerable<GynConfirmation>> GetGynConfirmation();

      /// <summary>
      /// The method is used to get an GynConfirmation by GynConfirmation Description.
      /// </summary>
      /// <param name="gynConfirmation">Description of an GynConfirmation.</param>
      /// <returns>Returns an GynConfirmation if the GynConfirmation name is matched.</returns>
      public Task<GynConfirmation> GetGynConfirmationByName(string gynConfirmation);
   }
}