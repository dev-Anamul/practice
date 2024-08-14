using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 06-02-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ISurgeryRepository : IRepository<Surgery>
    {
        /// <summary>
        /// The method is used to get a Surgery   by key.
        /// </summary>
        /// <param name="key">Primary key of the table Surgerys.</param>
        /// <returns>Returns a Surgery   if the key is matched.</returns>
        public Task<Surgery> GetSurgeryByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of Surgery  .
        /// </summary>
        /// <returns>Returns a list of all Surgery  .</returns>
        public Task<IEnumerable<Surgery>> GetSurgerys();

        /// <summary>
        /// The method is used to get a Surgery   by key.
        /// </summary>
        /// <param name="clientID">Primary key of the table Client.</param>
        /// <returns>Returns a Surgery   if the key is matched.</returns>
        public Task<IEnumerable<Surgery>> GetSurgeryByClientID(Guid clientId);
        public Task<IEnumerable<Surgery>> GetSurgeryByClientID(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetSurgeryByClientIDTotalCount(Guid clientID, EncounterType? encounterType);

        public Task<IEnumerable<Surgery>> GetSurgeryByEncounterID(Guid id);

    }
}