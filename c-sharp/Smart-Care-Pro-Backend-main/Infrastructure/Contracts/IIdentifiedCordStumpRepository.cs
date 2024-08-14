using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IIdentifiedCordStumpRepository : IRepository<IdentifiedCordStump>
   {
      /// <summary>
      /// The method is used to get a IdentifiedCordStump by key.
      /// </summary>
      /// <param name="key">Primary key of the table IdentifiedCordStumps.</param>
      /// <returns>Returns a IdentifiedCordStump if the key is matched.</returns>
      public Task<IdentifiedCordStump> GetIdentifiedCordStumpByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of IdentifiedCordStumps.
      /// </summary>
      /// <returns>Returns a list of all IdentifiedCordStumps.</returns>
      public Task<IEnumerable<IdentifiedCordStump>> GetIdentifiedCordStumps();

      /// <summary>
      /// The method is used to get the list of IdentifiedCordStump by EncounterID.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns a list of all IdentifiedCordStump by EncounterID.</returns>
      public Task<IEnumerable<IdentifiedCordStump>> GetIdentifiedCordStumpByEncounter(Guid encounterId);

      /// <summary>
      /// The method is used to get the list of IdentifiedCordStump by AssessmentID.
      /// </summary>
      /// <param name="assessmentId"></param>
      /// <returns>Returns a list of all IdentifiedCordStump by AssessmentID.</returns>
      public Task<IEnumerable<IdentifiedCordStump>> ReadIdentifiedCordStumpByAssessment(Guid assessmentId);
   }
}