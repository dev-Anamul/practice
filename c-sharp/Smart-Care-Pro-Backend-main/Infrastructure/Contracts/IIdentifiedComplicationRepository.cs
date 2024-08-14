using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 13.04.2023
 * Modified by  : Bella  
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IIdentifiedComplicationRepository : IRepository<IdentifiedComplication>
   {
      /// <summary>
      /// The method is used to get an identified complication by key.
      /// </summary>
      /// <param name="key">Primary key of the table IdentifiedComplications.</param>
      /// <returns>Returns an identified complication if the key is matched.</returns>
      public Task<IdentifiedComplication> GetIdentifiedComplicationByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of identified complications.
      /// </summary>
      /// <returns>Returns a list of all identified complications.</returns>
      public Task<IEnumerable<IdentifiedComplication>> GetIdentifiedComplications();

      /// <summary>
      /// The method is used to get an identified complication by ComplicationID.
      /// </summary>
      /// <param name="complicationId"></param>
      /// <returns>Returns an identified complication if the ComplicationID is matched.</returns>
      public Task<IEnumerable<IdentifiedComplication>> GetIdentifiedComplicationByComplication(Guid complicationId);

      /// <summary>
      /// The method is used to get an identified complication by EncounterID.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns an identified complication if the Encounter is matched.</returns>
      public Task<IEnumerable<IdentifiedComplication>> GetIdentifiedComplicationByEncounter(Guid encounterId);
   }
}