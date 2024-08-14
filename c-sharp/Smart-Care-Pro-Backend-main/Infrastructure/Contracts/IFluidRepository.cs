using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by    : Stephan
 * Date created  : 09.02.2023
 * Modified by   : Bella
 * Last modified : 13.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Contracts
{
    public interface IFluidRepository : IRepository<Fluid>
    {
        /// <summary>
        /// The method is used to get a birth record by key.
        /// </summary>
        /// <param name="key">Primary key of the table Fluids.</param>
        /// <returns>Returns a birth record if the key is matched.</returns>
        public Task<Fluid> GetFluidByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public Task<IEnumerable<Fluid>> GetFluids();

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a birth record if the ClientID is matched.</returns>
        public Task<IEnumerable<Fluid>> GetFluidByClient(Guid clientId);
        public Task<IEnumerable<Fluid>> GetFluidByClientLast24Hours(Guid clientId);
        public Task<IEnumerable<Fluid>> GetFluidByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetFluidByClientTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a birth record by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a birth record if the OPD EncounterID is matched.</returns>
        public Task<IEnumerable<Fluid>> GetFluidByEncounter(Guid encounterId);
    }
}