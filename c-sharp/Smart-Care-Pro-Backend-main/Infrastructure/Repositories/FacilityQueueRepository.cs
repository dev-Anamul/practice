using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FacilityQueueRepository : Repository<FacilityQueue>, IFacilityQueueRepository
    {
        public FacilityQueueRepository(DataContext context) : base(context)
        {

        }

        public async Task<IEnumerable<FacilityQueue>> GetAllFacilityQueues()
        {
            try
            {
                return await QueryAsync(f => f.IsDeleted == false);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<IEnumerable<FacilityQueue>> GetFacilityQueueByFacilityId(int facilityId)
        {
            try
            {
                return await QueryAsync(f => f.IsDeleted == false && f.FacilityId == facilityId);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<IEnumerable<FacilityQueue>> GetFacilityQueueByFacilityId(int facilityId, string? search)
        {
            try
            {
                if (search is not null)
                    return await QueryAsync(f => f.IsDeleted == false && f.FacilityId == facilityId && f.Description.Contains(search));
                else
                    return await QueryAsync(f => f.IsDeleted == false && f.FacilityId == facilityId);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<IEnumerable<ServicePointWithActivePatientDto>> GetFacilityQueueByFacilityWithActivePatientCount(int facilityId)
        {
            try
            {
                DateTime today = DateTime.Today;
                return await context.FacilityQueues.AsNoTracking().Where(x => x.IsDeleted == false && x.FacilityId == facilityId).Select(x =>
                    new ServicePointWithActivePatientDto()
                    {
                        Oid = x.Oid,
                        Description = x.Description+" ("+  context.ServiceQueues.AsNoTracking().Where(f => f.FacilityQueueId == x.Oid&&f.DateQueued.Date== today&&f.IsCompleted==false).Count()+")"
                    }).ToListAsync();

                // return await QueryAsync(f => f.IsDeleted == false && f.FacilityId == facilityId);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<int> getFacilityIdByFacilityQueueId(int facilityQueueId)
        {
            return await context.FacilityQueues.AsNoTracking().Where(x => x.Oid == facilityQueueId).Select(x => x.FacilityId).FirstOrDefaultAsync();
        }
        public async Task<List<int>> getFacilityQueuesIdByFacility(int facilityId)
        {
            return await context.FacilityQueues.AsNoTracking().Where(x => x.FacilityId == facilityId).Select(x => x.Oid).ToListAsync();
        }
        public async Task<FacilityQueue> GetFacilityQueueById(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(f => f.IsDeleted == false && f.Oid == key);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<FacilityQueue> GetFacilityQueueById(int key, string? search)
        {
            try
            {
                return await FirstOrDefaultAsync(f => f.IsDeleted == false && f.Oid == key);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<FacilityQueue> GetFacilityQueueByName(string servicePoint, int facilityId)
        {
            try
            {
                return await FirstOrDefaultAsync(f => f.Description.Trim() == servicePoint.Trim() && f.FacilityId == facilityId);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}