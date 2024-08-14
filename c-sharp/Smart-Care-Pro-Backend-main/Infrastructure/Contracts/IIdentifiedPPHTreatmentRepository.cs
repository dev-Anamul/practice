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
   public interface IIdentifiedPPHTreatmentRepository : IRepository<IdentifiedPPHTreatment>
   {
      /// <summary>
      /// The method is used to get a IdentifiedPPHTreatment by key.
      /// </summary>
      /// <param name="key">Primary key of the table IdentifiedPPHTreatments.</param>
      /// <returns>Returns a IdentifiedPPHTreatment if the key is matched.</returns>
      public Task<IdentifiedPPHTreatment> GetIdentifiedPPHTreatmentByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of IdentifiedPPHTreatments.
      /// </summary>
      /// <returns>Returns a list of all IdentifiedPPHTreatments.</returns>
      public Task<IEnumerable<IdentifiedPPHTreatment>> GetIdentifiedPPHTreatments();

      /// <summary>
      /// The method is used to get a IdentifiedPPHTreatment by PPHTreatmentsId
      /// </summary>
      /// <param name="pphTreatmentsId"></param>
      /// <returns>Returns a IdentifiedPPHTreatment if the PPHTreatmentsId is matched.</returns>
      public Task<IEnumerable<IdentifiedPPHTreatment>> GetIdentifiedPPHTreatmentByPPHTreatments(Guid pphTreatmentsId);

      /// <summary>
      /// The method is used to get the list of IdentifiedPPHTreatment by encounterId.
      /// </summary>
      /// <returns>Returns a list of all IdentifiedPPHTreatment by encounterId.</returns>
      public Task<IEnumerable<IdentifiedPPHTreatment>> GetIdentifiedPPHTreatmentByEncounter(Guid encounterId);
   }
}