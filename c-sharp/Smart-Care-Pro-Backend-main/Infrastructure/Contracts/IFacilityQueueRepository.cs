using Domain.Dto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
* Created by   : Lion
* Date created : 12.11.2023
* Modified by  :
* Last modified:
* Reviewed by  :
* Date reviewed:
*/
namespace Infrastructure.Contracts
{
    public interface IFacilityQueueRepository : IRepository<FacilityQueue>
    {
        public Task<IEnumerable<FacilityQueue>> GetFacilityQueueByFacilityId(int facilityId);
        public Task<IEnumerable<FacilityQueue>> GetFacilityQueueByFacilityId(int facilityId, string? search);
        public Task<IEnumerable<ServicePointWithActivePatientDto>> GetFacilityQueueByFacilityWithActivePatientCount(int facilityId);

        public Task<FacilityQueue> GetFacilityQueueById(int key);

        Task<FacilityQueue> GetFacilityQueueById(int key, string? search);
        public Task<IEnumerable<FacilityQueue>> GetAllFacilityQueues();
        public Task<List<int>> getFacilityQueuesIdByFacility(int facilityId);
        public Task<int> getFacilityIdByFacilityQueueId(int facilityId);

        public Task<FacilityQueue> GetFacilityQueueByName(string servicePoint,int facilityId);
    }
}
