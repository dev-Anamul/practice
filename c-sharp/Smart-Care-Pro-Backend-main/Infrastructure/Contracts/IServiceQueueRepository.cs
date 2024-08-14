using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by   : Bella
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IServiceQueueRepository : IRepository<ServiceQueue>
    {
        /// <summary>
        /// The method is used to get a ServiceQueue by key.
        /// </summary>
        /// <param name="key">Primary key of the table ServiceQueue.</param>
        /// <returns>Returns a ServiceQueue if the key is matched.</returns>
        public Task<ServiceQueue> GetServiceQueueByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of ServiceQueues.
        /// </summary>
        /// <returns>Returns a list of all ServiceQueues.</returns>
        public Task<IEnumerable<ServiceQueue>> GetServiceQueues();
        //public Task<IEnumerable<ServiceQueue>> GetTodayServiceQueuesByServicePoint(int servicePointId);

        /// <summary>
        /// The method is used to get a ServiceQueue by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a ServiceQueue if the ClientID is matched.</returns>
        //public Task<IEnumerable<ServiceQueue>> GetServiceQueueByClient(Guid ClientID);

        /// <summary>
        /// The method is used to get the list of ServiceQueues by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ServiceQueues by EncounterID.</returns>
        public Task<IEnumerable<ServiceQueue>> GetServiceQueueByEncounter(Guid EncounterID);
        public Task<IEnumerable<ServiceQueue>> GetServiceQueueByServicePoint(int facilityQueueId);
    }
}
