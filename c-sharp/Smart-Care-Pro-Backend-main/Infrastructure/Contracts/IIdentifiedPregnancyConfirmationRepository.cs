using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IIdentifiedPregnancyConfirmationRepository : IRepository<IdentifiedPregnancyConfirmation>
    {
        /// <summary>
        /// The method is used to get a IdentifiedPregnancyConfirmation by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPregnancyConfirmations.</param>
        /// <returns>Returns a IdentifiedPregnancyConfirmation if the key is matched.</returns>
        public Task<IdentifiedPregnancyConfirmation> GetIdentifiedPregnancyConfirmationByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of IdentifiedPregnancyConfirmation.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedPregnancyConfirmation.</returns>
        public Task<IEnumerable<IdentifiedPregnancyConfirmation>> GetIdentifiedPregnancyConfirmations();

        /// <summary>
        /// The method is used to get the list of IdentifiedPregnancyConfirmation by encounterId.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedPregnancyConfirmation by encounterId.</returns>
        public Task<IEnumerable<IdentifiedPregnancyConfirmation>> GetIdentifiedPregnancyConfirmationByEncounter(Guid encounterId);
    }
}