using Domain.Dto;
using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 29-01-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IBedRepository : IRepository<Bed>
    {
        /// <summary>
        /// The method is used to get a Bed by BedName.
        /// </summary>
        /// <param name="BedName">BedName of a Bed.</param>
        /// <returns>Returns a Bed   if the BedName is matched.
        public Task<Bed> GetBedByName(string bedName);

        /// <summary>
        /// The method is used to get a Bed   by key.
        /// </summary>
        /// <param name="key">Primary key of the table Beds.</param>
        /// <returns>Returns a Bed   if the key is matched.</returns>
        public Task<Bed> GetBedByKey(int key);

        /// <summary>
        /// The method is used to get the list of Bed by facilityId .
        /// </summary>
        /// <returns>Returns a list of all Bed by facilityId .</returns>
        public Task<IEnumerable<Bed>> GetBedsByFacilityId(int facilityId);

        /// <summary>
        /// The method is used to get the list of Bed  .
        /// </summary>
        /// <returns>Returns a list of all Bed  .</returns>
        public Task<IEnumerable<Bed>> GetBeds();

        /// <summary>
        /// The method is used to get a bed by WardID.
        /// </summary>
        /// <param name="WardID">WardID of the table Beds.</param>
        /// <returns>Returns a bed if the WardID is matched.</returns>
        public Task<IEnumerable<Bed>> GetBedByWard(int wardId);
        public Task<IEnumerable<BedDropDownDto>> GetBedByWardForDropDown(int wardId);

    }
}