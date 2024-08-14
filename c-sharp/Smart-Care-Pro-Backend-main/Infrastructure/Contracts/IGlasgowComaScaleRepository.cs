using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IGlasgowComaScaleRepository : IRepository<GlasgowComaScale>
    {
        /// <summary>
        /// The method is used to get a glasgow-coma scale by key.
        /// </summary>
        /// <param name="key">Primary key of the table GlasgowComaScales.</param>
        /// <returns>Returns a glasgow coma scale if the key is matched.</returns>
        public Task<GlasgowComaScale> GetGlasgowComaScaleByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of glasgow coma scales.
        /// </summary>
        /// <returns>Returns a list of all glasgow coma scales.</returns>
        public Task<IEnumerable<GlasgowComaScale>> GetGlasgowComaScales();

        /// <summary>
        /// The method is used to get a GlasgowComaScale by Encounter id.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a GlasgowComaScale if the Encounter id is matched.</returns>
        public Task<IEnumerable<GlasgowComaScale>> GetGlasgowComaScalesByEncounterId(Guid encounterId);

        /// <summary>
        /// The method is used to get a GlasgowComaScale by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a GlasgowComaScale if the ClientID is matched.</returns>
        public Task<IEnumerable<GlasgowComaScale>> GetGlasgowComaScalesByClientIDLast24Hours(Guid clientId);
        public Task<IEnumerable<GlasgowComaScale>> GetGlasgowComaScalesByClientID(Guid clientId);
        public Task<IEnumerable<GlasgowComaScale>> GetGlasgowComaScalesByClientID(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetGlasgowComaScalesByClientIDTotalCount(Guid clientID, EncounterType? encounterType);
    }
}