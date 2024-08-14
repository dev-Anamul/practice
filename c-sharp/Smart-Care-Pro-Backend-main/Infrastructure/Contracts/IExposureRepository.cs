using Domain.Entities;

/*
 * Created by    : Stephan
 * Date created  : 18.02.2023
 * Modified by   : Stephan
 * Last modified : 05.06.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Contracts
{
    public interface IExposureRepository : IRepository<Exposure>
    {
        /// <summary>
        /// The method is used to get an exposure by key.
        /// </summary>
        /// <param name="key">Primary key of the table Exposures.</param>
        /// <returns>Returns an exposure if the key is matched.</returns>
        public Task<Exposure> GetExposureByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of exposures.
        /// </summary>
        /// <returns>Returns a list of all exposures.</returns>
        public Task<IEnumerable<Exposure>> GetExposures();

        public Task<IEnumerable<Exposure>> GetExposureByID(Guid ChiefComplaintID);

    }
}