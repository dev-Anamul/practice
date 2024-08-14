using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 08.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IOptedCircumcisionReasonRepository : IRepository<OptedCircumcisionReason>
    {
        /// <summary>
        /// The method is used to get a OptedCircumcisionReason by key.
        /// </summary>
        /// <param name="key">Primary key of the table OptedCircumcisionReasons.</param>
        /// <returns>Returns a OptedCircumcisionReason if the key is matched.</returns>
        public Task<OptedCircumcisionReason> GetOptedCircumcisionReasonByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of OptedCircumcisionReasons.
        /// </summary>
        /// <returns>Returns a list of all OptedCircumcisionReasons.</returns>
        public Task<IEnumerable<OptedCircumcisionReason>> GetOptedCircumcisionReasons();

        /// <summary>
        /// The method is used to get a OptedCircumcisionReason by CircumcisionReasonId.
        /// </summary>
        /// <param name="circumcisionReasonId"></param>
        /// <returns>Returns a OptedCircumcisionReason if the CircumcisionReasonId is matched.</returns>
        public Task<IEnumerable<OptedCircumcisionReason>> GetOptedCircumcisionReasonByCircumcisionReason(int circumcisionReasonId);

        /// <summary>
        /// The method is used to get a OptedCircumcisionReason by VMMCServiceId.
        /// </summary>
        /// <param name="vmmcServiceId"></param>
        /// <returns>Returns a OptedCircumcisionReason if the VMMCServiceId is matched.</returns>
        public Task<IEnumerable<OptedCircumcisionReason>> GetCircumcisionReasonByVMMCService(Guid vmmcServiceId);
    }
}