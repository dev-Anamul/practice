using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 05.03.2023
 * Modified by  : Bella  
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IGuidedExaminationRepository : IRepository<GuidedExamination>
   {
      /// <summary>
      /// The method is used to get a GuidedExaminationByKey by key.
      /// </summary>
      /// <param name="key">Primary key of the table GuidedExaminationByKey.</param>
      /// <returns>Returns a GuidedExaminationByKey if the key is matched.</returns>
      public Task<GuidedExamination> ReadGuidedExaminationByKey(Guid key);

      /// <summary>
      /// The method is used to get a GuidedExaminationByKey by key.
      /// </summary>
      /// <returns>Returns a GuidedExaminationByKey if the key is matched.</returns>
      public Task<IEnumerable<GuidedExamination>> ReadGuidedExaminations();

      /// <summary>
      /// The method is used to get a birth record by ClientID.
      /// </summary>
      /// <param name="clientId"></param>
      /// <returns>Returns a GuidedExamination if the ClientID is matched.</returns>
      public Task<IEnumerable<GuidedExamination>> ReadGuidedExaminationByClient(Guid clientId);

      /// <summary>
      /// The method is used to get the list of GuidedExamination by EncounterID.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns a list of all GuidedExamination by EncounterID.</returns>
      public Task<IEnumerable<GuidedExamination>> ReadGuidedExaminationByEncounter(Guid encounterId);
   }
}