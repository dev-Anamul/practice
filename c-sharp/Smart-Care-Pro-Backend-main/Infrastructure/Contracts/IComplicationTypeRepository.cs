using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IComplicationTypeRepository : IRepository<ComplicationType>
    {
        /// <summary>
        /// The method is used to get a complication type by description.
        /// </summary>
        /// <param name="description">Description of ComplicationType.</param>
        /// <returns>Returns a complication type if the description is matched.</returns>
        public Task<ComplicationType> GetComplicationTypeByDescription(string description);

        /// <summary>
        /// The method is used to get a complication type by key.
        /// </summary>
        /// <param name="key">Primary key of the table ComplicationTypes.</param>
        /// <returns>Returns a complication type if the key is matched.</returns>
        public Task<ComplicationType> GetComplicationTypeByKey(int key);

        /// <summary>
        /// The method is used to get the list of complication types.
        /// </summary>
        /// <returns>Returns a list of all complication types.</returns>
        public Task<IEnumerable<ComplicationType>> GetComplicationTypes();
    }
}