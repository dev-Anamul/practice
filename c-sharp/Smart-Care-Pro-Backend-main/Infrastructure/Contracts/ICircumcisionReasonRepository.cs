using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 14.08.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ICircumcisionReasonRepository : IRepository<CircumcisionReason>
    {
        /// <summary>
        /// The method is used to get a CircumcisionReason by key.
        /// </summary>
        /// <param name="key">Primary key of the table CircumcisionReason.</param>
        /// <returns>Returns a CircumcisionReason if the key is matched.</returns>
        public Task<CircumcisionReason> GetCircumcisionReasonByKey(int key);

        /// <summary>
        /// The method is used to get the list of CircumcisionReasons.
        /// </summary>
        /// <returns>Returns a list of all CircumcisionReasons.</returns>
        public Task<IEnumerable<CircumcisionReason>> GetCircumcisionReasons();

        /// <summary>
        /// The method is used to get an CircumcisionReason by CircumcisionReason Description.
        /// </summary>
        /// <param name="circumcisionReason">Description of an CircumcisionReason.</param>
        /// <returns>Returns an CircumcisionReason if the CircumcisionReason name is matched.</returns>
        public Task<CircumcisionReason> GetCircumcisionReasonByName(string circumcisionReason);
    }
}