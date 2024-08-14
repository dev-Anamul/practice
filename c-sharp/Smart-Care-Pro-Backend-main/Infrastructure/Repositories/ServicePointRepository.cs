using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Domain.Dto;
using Microsoft.EntityFrameworkCore;


/*
 * Created by: Tomas
 * Date created: 13.09.2022
 * Modified by: Lion
 * Last modified: 06.11.2022
 */

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IServicePointRepository interface.
    /// </summary>
    public class ServicePointRepository : Repository<ServicePoint>, IServicePointRepository
    {
        public ServicePointRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a service point by service point name.
        /// </summary>
        /// <param name="servicePoint">Name of a service point.</param>
        /// <returns>Returns a service point if the service point name is matched.</returns>
        public async Task<ServicePoint> GetServicePointByService(string servicePoint)
        {
            try
            {
                return await FirstOrDefaultAsync(s => s.Description.ToLower().Trim() == servicePoint.ToLower().Trim() && s.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a service point by key.
        /// </summary>
        /// <param name="key">Primary key of the table ServicePoints.</param>
        /// <returns>Returns a service point if the key is matched.</returns>
        public async Task<ServicePoint> GetServicePointByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(s => s.Oid == key && s.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //public async Task<IEnumerable<ServicePoint>> GetServicePointByFacility(int FacilityId)
        //{
        //    try
        //    {
        //        return await QueryAsync(s => s.FacilityId == FacilityId && s.IsDeleted == false);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public async Task<IEnumerable<ServicePointWithActivePatientDto>> GetServicePointByFacilityWithActivePatientCount(int FacilityId)
        //{
        //    try
        //    {
        //        DateTime twentyFourHoursAgo = DateTime.Now.AddHours(-24);
        //        DateTime today = DateTime.Today;
        //        var servicePoint = await context.ServicePoints.Where(x => x.FacilityId == FacilityId).Select(x => new ServicePointWithActivePatientDto()
        //        {
        //            ServicePointId=x.Oid,
        //            ServiceCode = (int)x.ServiceCode,
        //           // ServicePointDescription = x.Description + " Active " + context.ServiceQueues.Where(y => y.FacilityId == FacilityId && y.ServiceCode == x.ServiceCode && y.IsCompleted == false && y.DateQueued >= twentyFourHoursAgo).Count()
        //            ServicePointDescription = x.Description + " Active " + context.ServiceQueues.Include(z=>z.ServicePoint).Where(y => y.FacilityId == FacilityId && y.ServicePoint.ServiceCode == x.ServiceCode && y.IsCompleted == false && y.DateQueued.Date == today).Count()
        //        }).ToListAsync();

        //        return servicePoint;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        /// <summary>
        /// The method is used to get the list of service points.
        /// </summary>
        /// <returns>Returns a list of all service points.</returns>
        public async Task<IEnumerable<ServicePoint>> GetServicePoints()
        {
            try
            {
                return await QueryAsync(s => s.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}