using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IIdentifiedAllergyRepository : IRepository<IdentifiedAllergy>
    {
        /// <summary>
        /// The method is used to get a identified allergy by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedAllergies.</param>
        /// <returns>Returns a identified allergy if the key is matched.</returns>
        public Task<IdentifiedAllergy> GetIdentifiedAllergyByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of identified allergies.
        /// </summary>
        /// <returns>Returns a list of all identified allergies.</returns>
        public Task<IEnumerable<IdentifiedAllergy>> GetIdentifiedAllergies();

        /// <summary>
        /// The method is used to get a identified allergy by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a identified allergy if the ClientID is matched.</returns>
        public Task<IEnumerable<IdentifiedAllergy>> GetIdentifiedAllergyByClient(Guid clientId);
        public Task<IEnumerable<IdentifiedAllergy>> GetIdentifiedAllergyByClientLast24Hours(Guid clientId);
        public Task<IEnumerable<IdentifiedAllergy>> GetIdentifiedAllergyByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetIdentifiedAllergyByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a identified allergy by encounterId.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a identified allergy if the encounterId is matched.</returns>
        public Task<IEnumerable<IdentifiedAllergy>> GetIdentifiedAllergyByEncounterId(Guid encounterId);
    }
}