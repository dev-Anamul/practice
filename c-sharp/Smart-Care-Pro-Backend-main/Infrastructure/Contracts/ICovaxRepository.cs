using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface ICovaxRepository : IRepository<Covax>
   {
      /// <summary>
      /// The method is used to get a birth history by key.
      /// </summary>
      /// <param name="key">Primary key of the table Covaxes.</param>
      /// <returns>Returns a birth history if the key is matched.</returns>
      public Task<Covax> GetCovaxByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of birth histories.
      /// </summary>
      /// <returns>Returns a list of all birth histories.</returns>
      public Task<IEnumerable<Covax>> GetCovaxes();

      /// <summary>
      /// The method is used to get a birth history by ClientID.
      /// </summary>
      /// <param name="ClientID"></param>
      /// <returns>Returns a birth history if the ClientID is matched.</returns>
      public Task<IEnumerable<Covax>> GetCovaxByClient(Guid ClientID);

      /// <summary>
      /// The method is used to get a birth history by OPD visit.
      /// </summary>
      /// <param name="EncounterID"></param>
      /// <returns>Returns a birth history if the Encounter is matched.</returns>
      public Task<IEnumerable<Covax>> GetCovaxByEncounter(Guid EncounterID);
   }
}