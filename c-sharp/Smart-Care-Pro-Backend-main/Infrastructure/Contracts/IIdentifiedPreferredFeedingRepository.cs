using Domain.Entities;
using static Utilities.Constants.Enums;

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
    public interface IIdentifiedPreferredFeedingRepository : IRepository<IdentifiedPreferredFeeding>
    {
        /// <summary>
        /// The method is used to get a IdentifiedPreferredFeeding by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPreferredFeedings.</param>
        /// <returns>Returns a IdentifiedPreferredFeeding if the key is matched.</returns>
        public Task<IdentifiedPreferredFeeding> GetIdentifiedPreferredFeedingByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of IdentifiedPreferredFeedings.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedPreferredFeedings.</returns>
        public Task<IEnumerable<IdentifiedPreferredFeeding>> GetIdentifiedPreferredFeedings();

        /// <summary>
        /// The method is used to get the list of IdentifiedPreferredFeeding by ClientID.
        /// </summary>
        /// <param name="clientId">Primary key of the table Clients.</param>
        /// <returns>Returns a list of all IdentifiedPreferredFeeding by ClientID.</returns>
        public Task<IEnumerable<IdentifiedPreferredFeeding>> GetIdentifiedPreferredFeedingByClient(Guid clientId);
        public Task<IEnumerable<IdentifiedPreferredFeeding>> GetIdentifiedPreferredFeedingByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetIdentifiedPreferredFeedingByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get the list of IdentifiedPreferredFeeding by encounterId.
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounters.</param>
        /// <returns>Returns a list of all IdentifiedPreferredFeeding by encounterId.</returns>
        public Task<IEnumerable<IdentifiedPreferredFeeding>> GetIdentifiedPreferredFeedingByEncounter(Guid encounterId);

        /// <summary>
        /// The method is used to get the list of IdentifiedPreferredFeeding by preferredFeedingId.
        /// </summary>
        /// <param name="preferredFeedingId">Primary key of the table PreferredFeeding.</param>
        /// <returns>Returns a list of all IdentifiedPreferredFeeding by preferredFeedingId.</returns>
        public Task<IEnumerable<IdentifiedPreferredFeeding>> ReadIdentifiedPreferredFeedingByPreferredFeeding(int preferredFeedingId);
    }
}