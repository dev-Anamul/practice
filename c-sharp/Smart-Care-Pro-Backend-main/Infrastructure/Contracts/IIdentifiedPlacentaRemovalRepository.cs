using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IIdentifiedPlacentaRemovalRepository : IRepository<IdentifiedPlacentaRemoval>
   {
      /// <summary>
      /// The method is used to get a IdentifiedPlacentaRemoval by key.
      /// </summary>
      /// <param name="key">Primary key of the table IdentifiedPlacentaRemovals.</param>
      /// <returns>Returns a IdentifiedPlacentaRemoval if the key is matched.</returns>
      public Task<IdentifiedPlacentaRemoval> GetIdentifiedPlacentaRemovalByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of IdentifiedPlacentaRemovals.
      /// </summary>
      /// <returns>Returns a list of all IdentifiedPlacentaRemovals.</returns>
      public Task<IEnumerable<IdentifiedPlacentaRemoval>> GetIdentifiedPlacentaRemovals();

      /// <summary>
      /// The method is used to get a IdentifiedPlacentaRemoval by PlacentaRemovalId
      /// </summary>
      /// <param name="placentaRemovalId"></param>
      /// <returns>Returns a IdentifiedPlacentaRemoval if the PlacentaRemovalId is matched.</returns>
      public Task<IEnumerable<IdentifiedPlacentaRemoval>> GetIdentifiedPlacentaRemovalByPlacentaRemoval(Guid placentaRemovalId);

      /// <summary>
      /// The method is used to get the list of IdentifiedPlacentaRemoval by EncounterID.
      /// </summary
      /// <param name="encounterId"></param>
      /// <returns>Returns a list of all IdentifiedPlacentaRemoval by EncounterID.</returns>
      public Task<IEnumerable<IdentifiedPlacentaRemoval>> GetIdentifiedPlacentaRemovalByEncounter(Guid encounterId);
   }
}