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
   public interface IIdentifiedPerineumIntactRepository : IRepository<IdentifiedPerineumIntact>
   {
      /// <summary>
      /// The method is used to get a IdentifiedPerineumIntact by key.
      /// </summary>
      /// <param name="key">Primary key of the table IdentifiedPerineumIntacts.</param>
      /// <returns>Returns a IdentifiedPerineumIntact if the key is matched.</returns>
      public Task<IdentifiedPerineumIntact> GetIdentifiedPerineumIntactByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of IdentifiedPerineumIntacts.
      /// </summary>
      /// <returns>Returns a list of all IdentifiedPerineumIntacts.</returns>
      public Task<IEnumerable<IdentifiedPerineumIntact>> GetIdentifiedPerineumIntacts();

      /// <summary>
      /// The method is used to get the list of IdentifiedPerineumIntact by DeliveryId.
      /// </summary>
      /// <param name="deliveryId"></param>
      /// <returns>Returns a list of all IdentifiedPerineumIntact by DeliveryId.</returns>
      public Task<IEnumerable<IdentifiedPerineumIntact>> GetIdentifiedPerineumIntactByDelivery(Guid deliveryId);

      /// <summary>
      /// The method is used to get the list of IdentifiedPerineumIntact by encounterId.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns a list of all IdentifiedPerineumIntact by encounterId.</returns>
      public Task<IEnumerable<IdentifiedPerineumIntact>> GetIdentifiedPerineumIntactByEncounter(Guid encounterId);
   }
}