using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 01.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IIdentifiedDeliveryInterventionRepository : IRepository<IdentifiedDeliveryIntervention>
   {
      /// <summary>
      /// The method is used to get a IdentifiedDeliveryIntervention by key.
      /// </summary>
      /// <param name="key">Primary key of the table IdentifiedDeliveryInterventions.</param>
      /// <returns>Returns a IdentifiedDeliveryIntervention if the key is matched.</returns>
      public Task<IdentifiedDeliveryIntervention> GetIdentifiedDeliveryInterventionByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of IdentifiedDeliveryInterventions.
      /// </summary>
      /// <returns>Returns a list of all IdentifiedDeliveryInterventions.</returns>
      public Task<IEnumerable<IdentifiedDeliveryIntervention>> GetIdentifiedDeliveryInterventions();

      /// <summary>
      /// The method is used to get the list of IdentifiedDeliveryIntervention by DeliveryId.
      /// </summary>
      /// <param name="deliveryId"></param>
      /// <returns>Returns a list of all IdentifiedDeliveryIntervention by DeliveryId.</returns>
      public Task<IEnumerable<IdentifiedDeliveryIntervention>> GetIdentifiedDeliveryInterventionByDelivery(Guid deliveryId);
   }
}