using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Stephan
 * Date created : 02.1.2024
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public interface IDFZClientDependentRepository : IRepository<DFZDependent>
    {
        /// <summary>
        /// The method is used to get a dfz Client by key.
        /// </summary>
        /// <param name="key">key of a DFZ client.</param>
        /// <returns>Returns a dfz dependent if the dfz client key is matched.</returns>
        public Task<IEnumerable<DFZDependent>> GetDFZDependentsByPrincipleId(Guid key);

        /// <summary>
        /// The method is used to get a dfz Client by dfzClientId.
        /// </summary>
        /// <param name="dfzClientId">key of a DFZ client.</param>
        /// <returns>Returns a dfz dependent if the dfz client key is matched.</returns>
        public Task<DFZDependent> GetDFZDependentByDFZClientId(Guid dfzClientId);

        /// <summary>
        /// The method is used to get a dfz Client by key.
        /// </summary>
        /// <param name="key">key of a DFZ dependent.</param>
        /// <returns>Returns a dfz dependent if the dfz dependent key is matched.</returns>
        public Task<DFZDependent> GetDFZDependentByKey(Guid key);

        /// <summary>
        /// The method is used to get a dfz Client by dfz hospitalNo.
        /// </summary>
        /// <param name="hospitalNo">hospitalNo of a hospitalNo.</param>
        /// <returns>Returns a dfz client if the dfz hospitalNo is matched.</returns>
        public Task<DFZDependent> GetDependentByHospitalNo(string hospitalNo);
    }
}