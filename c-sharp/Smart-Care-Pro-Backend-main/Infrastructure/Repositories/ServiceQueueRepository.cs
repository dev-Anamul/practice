using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ServiceQueueRepository : Repository<ServiceQueue>, IServiceQueueRepository
    {
        /// <summary>
        /// Implementation of ITBHistoryRepository interface.
        /// </summary>
        public ServiceQueueRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a ServiceQueue by key.
        /// </summary>
        /// <param name="key">Primary key of the table ServiceQueue.</param>
        /// <returns>Returns a ServiceQueue if the key is matched.</returns>
        public async Task<ServiceQueue> GetServiceQueueByKey(Guid key)
        {
            try
            {
                return await LoadWithChildAsync<ServiceQueue>(p => p.InteractionId == key && p.IsDeleted == false, s => s.FacilityQueue);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ServiceQueues.
        /// </summary>
        /// <returns>Returns a list of all ServiceQueues.</returns>
        public async Task<IEnumerable<ServiceQueue>> GetServiceQueues()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<ServiceQueue>> GetServiceQueueByServicePoint(int facilityQueueId)
        {
            try
            {
                DateTime today = DateTime.Today;
                
                return await context.ServiceQueues.Include(x=>x.Client).AsNoTracking().Include(x=>x.FacilityQueue)
                     
                    .Where(x => x.FacilityQueueId == facilityQueueId&& x.DateQueued.Date == today && x.IsDeleted == false).OrderBy(x => x.UrgencyType).ThenBy(x => x.DateQueued)
                   .Select( y=>new ServiceQueue()
                {
                    DateQueued=y.DateQueued,
                    Client=y.Client,
                    ClientId=y.ClientId,
                    UrgencyType=y.UrgencyType,
                    FacilityQueue=y.FacilityQueue,
                    CreatedBy=y.CreatedBy,
                    CreatedIn=y.CreatedIn,
                    DateCreated=   y.DateCreated,
                    DateModified=y.DateModified,
                    EncounterId=y.EncounterId,
                    EncounterType=y.EncounterType,
                    FacilityQueueId=y.FacilityQueueId,
                    InteractionId   =y.InteractionId,
                    IsCompleted=y.IsCompleted,
                    IsDeleted=y.IsDeleted,
                    IsSynced=y.IsSynced,
                    ModifiedBy=y.ModifiedBy,
                    ModifiedIn=y.ModifiedIn,
                    Purpose=y.Purpose,
                    ClinicianName= context.UserAccounts.Where(x => x.Oid == y.CreatedBy).Select(x=>x.FirstName+" "+x.Surname).FirstOrDefault()??"",
                }).ToListAsync();
               
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// The method is used to get the list of today's ServiceQueues by Service Point.
        /// </summary>
        /// <returns>Returns a list of all ServiceQueues.</returns>
        //public async Task<IEnumerable<ServiceQueue>> GetTodayServiceQueuesByServicePoint(int servicePointId)
        //{
        //    try
        //    { 
        //        DateTime today = DateTime.Today;

        //        var serviceQueues = await context.ServiceQueues.Include(x=>x.ServicePoint).Include(x=>x.Client).AsNoTracking().Where(x => x.ServicePointId == servicePointId&&x.DateQueued.Date==today &&x.IsDeleted==false).OrderBy(x => x.UrgencyType).ThenBy(x => x.DateQueued).ToListAsync();

        //        return serviceQueues;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// The method is used to get a ServiceQueue by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a ServiceQueue if the ClientID is matched.</returns>
        //public async Task<IEnumerable<ServiceQueue>> GetServiceQueueByClient(Guid ClientID)
        //{
        //    try
        //    {
        //        return await QueryAsync(p => p.ClientID == ClientID && p.IsDeleted == false);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// The method is used to get the list of ServiceQueue by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ServiceQueue by EncounterID.</returns>
        public async Task<IEnumerable<ServiceQueue>> GetServiceQueueByEncounter(Guid EncounterID)
        {
            try
            {
                return await QueryAsync(p => p.EncounterId == EncounterID && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}