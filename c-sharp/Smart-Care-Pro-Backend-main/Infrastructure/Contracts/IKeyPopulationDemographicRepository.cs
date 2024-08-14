using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 12.09.2022
 * Modified by  : Bella
 * Last modified: 14.08.2023 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IKeyPopulationDemographicRepository : IRepository<KeyPopulationDemographic>
   {
      /// <summary>
      /// The method is used to get a key population by key.
      /// </summary>
      /// <param name="key">Primary key of the table KeyPopulationDemographics.</param>
      /// <returns>Returns a key population if the key is matched.</returns>
      public Task<KeyPopulationDemographic> GetKeyPopulationDemographicByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of key population.
      /// </summary>
      /// <returns>Returns a list of all key population.</returns>
      public Task<IEnumerable<KeyPopulationDemographic>> GetKeyPopulationDemographics();

      /// <summary>
      /// The method is used to get a key population by ClientID.
      /// </summary>
      /// <param name="clientId"></param>
      /// <returns>Returns a key population if the ClientID is matched.</returns>
      public Task<IEnumerable<KeyPopulationDemographic>> GetKeyPopulationDemographicByClient(Guid clientId);

      /// <summary>
      /// The method is used to get a key population by OPD visit.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns a key population if the Encounter is matched.</returns>
      public Task<IEnumerable<KeyPopulationDemographic>> GetKeyPopulationDemographicByEncounter(Guid encounterId);
      public Task<IEnumerable<KeyPopulationDemographic>> GetKeyPopulationDemographicByEncounterIdEncounterType(Guid encounterId, EncounterType encouterType);
   }
}