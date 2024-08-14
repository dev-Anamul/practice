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
   public interface IIdentifiedCurrentDeliveryComplicationRepository : IRepository<IdentifiedCurrentDeliveryComplication>
   {
      /// <summary>
      /// The method is used to get a IdentifiedCurrentDeliveryComplication by key.
      /// </summary>
      /// <param name="key">Primary key of the table IdentifiedCurrentDeliveryComplications.</param>
      /// <returns>Returns a IdentifiedCurrentDeliveryComplication if the key is matched.</returns>
      public Task<IdentifiedCurrentDeliveryComplication> GetIdentifiedCurrentDeliveryComplicationByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of IdentifiedCurrentDeliveryComplications.
      /// </summary>
      /// <returns>Returns a list of all IdentifiedCurrentDeliveryComplications.</returns>
      public Task<IEnumerable<IdentifiedCurrentDeliveryComplication>> GetIdentifiedCurrentDeliveryComplications();

      /// <summary>
      /// The method is used to get the list of IdentifiedCurrentDeliveryComplication by DeliveryId.
      /// </summary>
      /// <param name="deliveryId"></param>
      /// <returns>Returns a list of all IdentifiedCurrentDeliveryComplication by DeliveryId.</returns>
      public Task<IEnumerable<IdentifiedCurrentDeliveryComplication>> GetIdentifiedCurrentDeliveryComplicationByDelivery(Guid deliveryId);
   }
}