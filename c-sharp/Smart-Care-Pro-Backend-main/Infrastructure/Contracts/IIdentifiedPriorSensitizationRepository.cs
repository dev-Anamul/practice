using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 19.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IIdentifiedPriorSensitizationRepository : IRepository<IdentifiedPriorSensitization>
   {
      /// <summary>
      /// The method is used to get a IdentifiedPriorSensitization by key.
      /// </summary>
      /// <param name="key">Primary key of the table IdentifiedPriorSensitizations.</param>
      /// <returns>Returns a IdentifiedPriorSensitization if the key is matched.</returns>
      public Task<IdentifiedPriorSensitization> GetIdentifiedPriorSensitizationByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of IdentifiedPriorSensitization.
      /// </summary>
      /// <returns>Returns a list of all IdentifiedPriorSensitization.</returns>
      public Task<IEnumerable<IdentifiedPriorSensitization>> GetIdentifiedPriorSensitizations();

      /// <summary>
      /// The method is used to get the list of IdentifiedPriorSensitization by encounterId.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns a IdentifiedPriorSensitization by encounterId.</returns>
      public Task<IEnumerable<IdentifiedPriorSensitization>> GetIdentifiedPriorSensitizationByEncounter(Guid encounterId);

      /// <summary>
      /// The method is used to get the list of IdentifiedPriorSensitization by bloodTransfusionId.
      /// </summary>
      /// <param name="bloodTransfusionId"></param>
      /// <returns>Returns a IdentifiedPriorSensitization by bloodTransfusionId</returns>
      public Task<IEnumerable<IdentifiedPriorSensitization>> GetIdentifiedPriorSensitizationByBloodTransfusion(Guid bloodTransfusionId);
   }
}