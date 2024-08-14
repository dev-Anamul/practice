using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 29-01-2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IWardRepository : IRepository<Ward>
    {
        /// <summary>
        /// The method is used to get a Ward by WardName.
        /// </summary>
        /// <param name="WardName">WardName of a Ward.</param>
        /// <returns>Returns a Ward   if the WardName is matched.
        public Task<Ward> GetWardByName(string WardName);


        /// <summary>
        /// The method is used to get a Ward   by key.
        /// </summary>
        /// <param name="key">Primary key of the table Wards.</param>
        /// <returns>Returns a Ward   if the key is matched.</returns>
        public Task<Ward> GetWardByKey(int key);

        /// <summary>
        /// The method is used to get the list of Ward by Facility Id.
        /// </summary>
        /// <returns>Returns a list of all Ward by Facility Id.</returns>
        public Task<IEnumerable<Ward>> GetWardsByFacilityId(int facilityId);

        /// <summary>
        /// The method is used to get the list of Ward  .
        /// </summary>
        /// <returns>Returns a list of all Ward  .</returns>
        public Task<IEnumerable<Ward>> GetWards();

        /// <summary>
        /// The method is used to get a town by FirmId.
        /// </summary>
        /// <param name="FirmId">FirmId of the table Towns.</param>
        /// <returns>Returns a town if the FirmId is matched.</returns>
        public Task<IEnumerable<Ward>> GetWardByFirm(int FirmId);
    }
}