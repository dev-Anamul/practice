using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IDotRepository : IRepository<Dot>
   {
      /// <summary>
      /// The method is used to get a Dot by key.
      /// </summary>
      /// <param name="key">Primary key of the table Dots.</param>
      /// <returns>Returns a Dot if the key is matched.</returns>
      public Task<Dot> GetDotByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of Dots.
      /// </summary>
      /// <returns>Returns a list of all Dots.</returns>
      public Task<IEnumerable<Dot>> GetDots();

      /// <summary>
      /// The method is used to get a Dot by OPD visit.
      /// </summary>
      /// <param name="EncounterID"></param>
      /// <returns>Returns a Dot if the Encounter is matched.</returns>
      public Task<IEnumerable<Dot>> GetDotByEncounter(Guid EncounterID);

      /// <summary>
      /// The method is used to get a Dot by TBserviceId.
      /// </summary>
      /// <param name="TBServiceId"></param>
      /// <returns>Returns a Dot if the TBserviceId is matched.</returns>
      //public Task<IEnumerable<Dot>> GetDotByTBService(Guid TBServiceId);
      public Task<Dot> GetDotByTBService(Guid tbserviceid);
   }
}