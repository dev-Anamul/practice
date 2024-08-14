using Domain.Entities;

/*
 * Created by   : Biplob Roy
 * Date created : 01.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IPPHTreatmentRepository : IRepository<PPHTreatment>
   {
      /// <summary>
      /// The method is used to get a PPHTreatment by key.
      /// </summary>
      /// <param name="key">Primary key of the table PPHTreatments.</param>
      /// <returns>Returns a PPHTreatment if the key is matched.</returns>
      public Task<PPHTreatment> GetPPHTreatmentByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of PPHTreatments.
      /// </summary>
      /// <returns>Returns a list of all PPHTreatments.</returns>
      public Task<IEnumerable<PPHTreatment>> GetPPHTreatments();

      /// <summary>
      /// The method is used to get the list of PPHTreatment by DeliveryId.
      /// </summary>
      /// <returns>Returns a list of all PPHTreatment by DeliveryId.</returns>
      public Task<IEnumerable<PPHTreatment>> GetPPHTreatmentByDelivery(Guid DeliveryId);

      /// <summary>
      /// The method is used to get a PPHTreatment by Encounter.
      /// </summary>
      /// <param name="EncounterID"></param>
      /// <returns>Returns a PPHTreatment if the Encounter is matched.</returns>
      public Task<IEnumerable<PPHTreatment>> GetPPHTreatmentByEncounter(Guid EncounterID);
   }
}