using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by    : Bella
 * Date created  : 12.09.2022
 * Modified by   : Bella
 * Last modified : 13.08.2023
 * Reviewed by   :
 * Date reviewed :
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IFacilityRepository interface.
    /// </summary>
    public class FacilityRepository : Repository<Facility>, IFacilityRepository
    {
        public FacilityRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a facility by name.
        /// </summary>
        /// <param name="facilityName">Name of a facility.</param>
        /// <returns>Returns a facility if the facility name is matched.</returns>
        public async Task<Facility> GetFacilityByName(string facilityName)
        {
            try
            {
                return await FirstOrDefaultAsync(f => f.Description.ToLower().Trim() == facilityName.ToLower().Trim() && f.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a facility by key.
        /// </summary>
        /// <param name="key">Primary key of the table Facilities.</param>
        /// <returns>Returns a facility if the key is matched.</returns>
        public async Task<Facility> GetFacilityByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(f => f.Oid == key && f.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Facility>> GetNonDFZFacilities(int key)
        {
            return await context.Facilities.Where(f => f.IsDeleted == false && f.Oid == key && f.IsDFZ == false).ToListAsync();
        }

        /// <summary>
        /// The method is used to get the facilities by DistrictID.
        /// </summary>
        /// <param name="districtId">DistrictId of the table Facilities.</param>
        /// <returns>Returns a facility if the DistrictID is matched.</returns>
        public async Task<IEnumerable<Facility>> GetFacilityByDistrict(int districtId, string? searchFacility, int skip, int take)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchFacility))
                {
                    return await LoadListWithChildAsync<Facility>(f => f.IsDeleted == false && f.DistrictId == districtId && f.Description.Contains(searchFacility), skip, take, orderBy: d => d.OrderByDescending(y => y.DateCreated));
                }
                else
                {
                    return await LoadListWithChildAsync<Facility>(f => f.IsDeleted == false && f.DistrictId == districtId, skip, take, orderBy: d => d.OrderByDescending(y => y.DateCreated));

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of facilities.
        /// </summary>
        /// <returns>Returns a list of all facilities.</returns> 
        public async Task<IEnumerable<Facility>> GetFacilities()
        {
            try
            {
                return await QueryAsync(f => f.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of facilities.
        /// </summary>
        /// <returns>Returns a list of all facilities.</returns> 
        public async Task<IEnumerable<Facility>> GetAllActiveFacilities()
        {
            try
            {
                return await QueryAsync(f => f.IsDeleted == false && f.IsLive == true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Facility>> GetDFZFacilities()
        {
            try
            {
                return await QueryAsync(f => f.IsDeleted == false && f.IsLive == true && f.IsDFZ == true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Facility>> GetAllRequestActiveFacilities(Guid userId)
        {
            try
            {
                var isDFZUser = await CheckIfUserIsDFZ(userId);

                if (isDFZUser)
                {
                    return await context.Facilities
                       .Where(f => f.IsDeleted == false && f.IsLive == true && f.IsDFZ == true)
                       .ToListAsync();
                }
                else
                {
                    return await context.Facilities
                       .Where(f => f.IsDeleted == false && f.IsLive == true)
                       .ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> CheckIfUserIsDFZ(Guid userId)
        {
            try
            {
                var dfzFacilityIds = await context.FacilityAccesses
                   .Where(fa => fa.UserAccountId == userId && fa.IsDeleted == false && fa.Facility.IsDFZ == true)
                   .Select(fa => fa.FacilityId)
                   .ToListAsync();

                return dfzFacilityIds.Any();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Facility>> GetAllActiveFacilities(Guid userId)
        {
            try
            {
                var userFacilities = await context.FacilityAccesses
                   .Where(fa => fa.UserAccountId == userId && fa.IsDeleted == false && fa.Facility.IsLive == true && fa.Facility.IsDFZ == true)
                   .Select(fa => fa.Facility)
                   .ToListAsync();

                if (userFacilities.Any())
                {
                    return userFacilities;
                }
                else
                {
                    return await QueryAsync(f => f.IsDeleted == false && f.IsLive == true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the facilities.
        /// </summary>
        /// <param name="searchFacility"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>Returns a facility if key is matched.</returns>
        public async Task<IEnumerable<Facility>> GetFacilitiesWithPaging(string? searchFacility, int skip, int take)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchFacility))
                {
                    return await LoadListWithChildAsync<Facility>(p => p.IsDeleted == false && p.Description.Contains(searchFacility), skip, take, orderBy: d => d.OrderByDescending(y => y.DateCreated), d => d.District, p => p.District.Provinces);
                }
                else
                {
                    return await LoadListWithChildAsync<Facility>(p => p.IsDeleted == false, skip, take, orderBy: d => d.OrderByDescending(y => y.DateCreated), d => d.District, p => p.District.Provinces);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the facilities.
        /// </summary>
        /// <param name="searchFacility"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>Returns a facility if key is matched.</returns>
        public async Task<IEnumerable<Facility>> GetActiveFacilitiesByDistrice(int districtId, string? searchFacility, int skip, int take)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchFacility))
                {
                    return await LoadListWithChildAsync<Facility>(f => f.IsDeleted == false && f.IsLive == true && f.DistrictId == districtId && f.Description.Contains(searchFacility), skip, take, orderBy: d => d.OrderByDescending(y => y.DateCreated));
                }
                else
                {
                    return await LoadListWithChildAsync<Facility>(f => f.IsDeleted == false && f.IsLive == true && f.DistrictId == districtId, skip, take, orderBy: d => d.OrderByDescending(y => y.DateCreated));

                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get the total facilities.
        /// </summary>
        /// <param name="searchFacility"></param>
        /// <returns>Returns a total facility.</returns>
        public int GetFacilitiesWithPagingTotalCount(string? searchFacility)
        {
            if (string.IsNullOrEmpty(searchFacility))
            {
                return context.Facilities.Where(x => x.IsDeleted == false).Count();
            }

            else if (!string.IsNullOrEmpty(searchFacility))
            {
                return context.Facilities.Where(x => x.IsDeleted == false && x.Description.Contains(searchFacility)).Count();
            }

            return 0;
        }

        /// <summary>
        /// The method is used to get the facilities by type.
        /// </summary>
        /// <param name="key">key identifier of the table Facilities.</param>
        /// <returns>Returns a facility if the key is matched.</returns>
        public async Task<IEnumerable<Facility>> GetAllFacilityByFacilityType(bool key)
        {
            try
            {
                return await QueryAsync(d => d.IsDeleted == false && d.IsPrivateFacility == key);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}