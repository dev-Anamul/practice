using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 13.01.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IHIVRiskScreeningRepository : IRepository<HIVRiskScreening>
   {
      /// <summary>
      /// The method is used to get a HIVRiskScreening by key.
      /// </summary>
      /// <param name="key">Primary key of the table HIVRiskScreenings.</param>
      /// <returns>Returns a HIVRiskScreening if the key is matched.</returns>
      public Task<HIVRiskScreening> GetHIVRiskScreeningByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of HIVRiskScreening.
      /// </summary>
      /// <returns>Returns a list of all HIVRiskScreening.</returns>
      public Task<IEnumerable<HIVRiskScreening>> GetHIVRiskScreenings();

      /// <summary>
      /// The method is used to get a HIVRiskScreening by ClientID.
      /// </summary>
      /// <param name="clientId"></param>
      /// <returns>Returns a HIVRiskScreening if the ClientID is matched.</returns>
      public Task<IEnumerable<HIVRiskScreening>> GetHIVRiskScreeningByClient(Guid clientId);

      /// <summary>
      /// The method is used to get a HIVRiskScreening by OPD visit.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns a HIVRiskScreening if the Encounter is matched.</returns>
      public Task<IEnumerable<HIVRiskScreening>> GetHIVRiskScreeningByEncounter(Guid encounterId);
      public Task<IEnumerable<HIVRiskScreening>> GetHIVRiskScreeningByEncounterIdEncounterType(Guid encounterId,EncounterType encounterType);
   }
}