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
    public interface IDFZClientRepository : IRepository<DFZClient>
    {
        /// <summary>
        /// The method is used to get a dfz Client by dfz hospitalNo.
        /// </summary>
        /// <param name="hospitalNo">hospitalNo of a hospitalNo.</param>
        /// <returns>Returns a dfz client if the dfz hospitalNo is matched.</returns>
        public Task<DFZClient> GetDFZByHospitalNo(string hospitalNo);

        /// <summary>
        /// The method is used to get a dfz Client by dfz ServiceNo.
        /// </summary>
        /// <param name="ServiceNo">ServiceNo of a ServiceNo.</param>
        /// <returns>Returns a dfz client if the dfz ServiceNo is matched.</returns>
        public Task<DFZClient> GetDFZByServiceNo(string serviceNo);


        /// <summary>
        /// The method is used to get a dfz Client by DFZ client key.
        /// </summary>
        /// <param name="key">Primary key of the table DFZ client.</param>
        /// <returns>Returns a dfz client if the dfz key is matched.</returns>
        public Task<DFZClient> GetDFZClientByKey(Guid key);
    }
}