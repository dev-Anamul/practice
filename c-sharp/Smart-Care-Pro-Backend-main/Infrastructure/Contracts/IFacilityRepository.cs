using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IFacilityRepository : IRepository<Facility>
   {
      /// <summary>
      /// The method is used to get a facility by name.
      /// </summary>
      /// <param name="facilityName">Name of a facility.</param>
      /// <returns>Returns a facility if the facility name is matched.</returns>
      public Task<Facility> GetFacilityByName(string facilityName);

      /// <summary>
      /// The method is used to get a facility by key.
      /// </summary>
      /// <param name="key">Primary key of the table Facilities.</param>
      /// <returns>Returns a facility if the key is matched.</returns>
      public Task<Facility> GetFacilityByKey(int key);

      public Task<List<Facility>> GetNonDFZFacilities(int key);

      /// <summary>
      /// The method is used to get the facilities by DistrictId.
      /// </summary>
      /// <param name="DistrictId">DistrictId of the table Facilities.</param>
      /// <returns>Returns a facility if the DistrictId is matched.</returns>
      public Task<IEnumerable<Facility>> GetFacilityByDistrict(int DistrictId, string? searchFacility, int skip, int take);

      /// <summary>
      /// The method is used to get the list of facilities.
      /// </summary>
      /// <returns>Returns a list of all facilities.</returns> 
      public Task<IEnumerable<Facility>> GetFacilities();

      public Task<IEnumerable<Facility>> GetAllActiveFacilities();

      public Task<IEnumerable<Facility>> GetDFZFacilities();

      public Task<IEnumerable<Facility>> GetAllActiveFacilities(Guid userId);

      public Task<IEnumerable<Facility>> GetAllRequestActiveFacilities(Guid userId);

      /// <summary>
      /// The method is used to get the facility with paging.
      /// </summary>
      /// <param name="searchFacility"></param>
      /// <param name="skip"></param>
      /// <param name="take"></param>
      /// <returns>Returns a facility if the key is matched.</returns>
      public Task<IEnumerable<Facility>> GetFacilitiesWithPaging(string? searchFacility, int skip, int take);

        /// <summary>
        /// The method is used to get the facility with paging.
        /// </summary>
        /// <param name="searchFacility"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>Returns a facility if the key is matched.</returns>
        public Task<IEnumerable<Facility>> GetActiveFacilitiesByDistrice(int DistrictId, string? searchFacility, int skip, int take);

        /// <summary>
        /// The method is used to get the total facility.
        /// </summary>
        /// <param name="searchFacility"></param>
        /// <returns>Returns a total facility</returns>
        public int GetFacilitiesWithPagingTotalCount(string? searchFacility);

      /// <summary>
      /// The method is used to get the facility by facility type.
      /// </summary>
      /// <param name="key"></param>
      /// <returns>Returns a  facility if key if matched.</returns>
      public Task<IEnumerable<Facility>> GetAllFacilityByFacilityType(bool key);
   }
}