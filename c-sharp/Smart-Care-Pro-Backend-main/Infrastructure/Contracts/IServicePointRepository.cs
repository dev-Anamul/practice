using Domain.Entities;
using Domain.Dto;

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
    public interface IServicePointRepository : IRepository<ServicePoint>
    {
        /// <summary>
        /// The method is used to get a service point by service point name.
        /// </summary>
        /// <param name="servicePoint">Name of a service point.</param>
        /// <returns>Returns a service point if the service point name is matched.</returns>
        public Task<ServicePoint> GetServicePointByService(string servicePoint);

        /// <summary>
        /// The method is used to get a service point by key.
        /// </summary>
        /// <param name="key">Primary key of the table ServicePoints.</param>
        /// <returns>Returns a service point if the key is matched.</returns>
        public Task<ServicePoint> GetServicePointByKey(int key);
        /// The method is used to get a service point by Facility.
        /// </summary>
        /// <param name="key">Primary key of the table ServicePoints.</param>
        /// <returns>Returns a service point if the key is matched.</returns>


        //public Task<IEnumerable<ServicePoint>> GetServicePointByFacility(int FacilityId);
        //public Task<IEnumerable<ServicePointWithActivePatientDto>> GetServicePointByFacilityWithActivePatientCount(int FacilityId);

        /// <summary>
        /// The method is used to get the list of service points.
        /// </summary>
        /// <returns>Returns a list of all service points.</returns>
        public Task<IEnumerable<ServicePoint>> GetServicePoints();
    }
}