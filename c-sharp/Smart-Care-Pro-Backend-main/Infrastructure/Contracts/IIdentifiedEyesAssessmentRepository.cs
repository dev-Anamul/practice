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
   public interface IIdentifiedEyesAssessmentRepository : IRepository<IdentifiedEyesAssessment>
   {
      /// <summary>
      /// The method is used to get a IdentifiedEyesAssessment by key.
      /// </summary>
      /// <param name="key">Primary key of the table IdentifiedEyesAssessments.</param>
      /// <returns>Returns a IdentifiedEyesAssessment if the key is matched.</returns>
      public Task<IdentifiedEyesAssessment> GetIdentifiedEyesAssessmentByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of IdentifiedEyesAssessments.
      /// </summary>
      /// <returns>Returns a list of all IdentifiedEyesAssessments.</returns>
      public Task<IEnumerable<IdentifiedEyesAssessment>> GetIdentifiedEyesAssessments();

      /// <summary>
      /// The method is used to get the list of IdentifiedEyesAssessment by EncounterID.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns a list of all IdentifiedEyesAssessment by EncounterID.</returns>
      public Task<IEnumerable<IdentifiedEyesAssessment>> GetIdentifiedEyesAssessmentByEncounter(Guid encounterId);

      /// <summary>
      /// The method is used to get the list of IdentifiedEyesAssessment by AssessmentId.
      /// </summary>
      /// <param name="assessmentId"></param>
      /// <returns>Returns a list of all IdentifiedEyesAssessment by assessmentId.</returns>
      public Task<IEnumerable<IdentifiedEyesAssessment>> ReadIdentifiedEyesAssessmentByAssessment(Guid assessmentId);
   }
}