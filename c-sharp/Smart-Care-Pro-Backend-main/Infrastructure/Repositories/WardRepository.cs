using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
*Created by: Stephan
* Date created: 29.04.2023
* Modified by: Stephan
* Last modified: 13.08.2023
* Reviewed by:
*Date reviewed:
*/
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of WardRepository class.
    /// </summary>
    public class WardRepository : Repository<Ward>, IWardRepository
    {
        public WardRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a Ward by WardName.
        /// </summary>
        /// <param name="WardName">WardName of a Ward.</param>
        /// <returns>Returns a Ward   if the WardName is matched.
        public async Task<Ward> GetWardByName(string WardName)
        {
            try
            {
                return await LoadWithChildAsync<Ward>(w => w.Description.ToLower().Trim() == WardName.ToLower().Trim() && w.IsDeleted == false, f => f.Firm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Ward   by key.
        /// </summary>
        /// <param name="key">Primary key of the table Wards.</param>
        /// <returns>Returns a Ward   if the key is matched.</returns>
        public async Task<Ward> GetWardByKey(int key)
        {
            try
            {
                return await LoadWithChildAsync<Ward>(w => w.Oid == key && w.IsDeleted == false, x => x.Firm, d=> d.Firm.Department);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of Ward  .
        /// </summary>
        /// <returns>Returns a list of all Ward  .</returns>
        public async Task<IEnumerable<Ward>> GetWards()
        {
            try
            {
                return await LoadListWithChildAsync<Ward>(w => w.IsDeleted == false, f => f.Firm, d=>d.Firm.Department);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of Ward by Facility Id.
        /// </summary>
        /// <returns>Returns a list of all Ward by Facility Id.</returns>
        public async Task<IEnumerable<Ward>> GetWardsByFacilityId(int facilityId)
        {
            try
            {
                return await LoadListWithChildAsync<Ward>(w => w.IsDeleted == false && w.Firm.Department.FacilityId == facilityId, f => f.Firm, d => d.Firm.Department);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a town by FirmId.
        /// </summary>
        /// <param name="FirmId">FirmId of the table Firm.</param>
        /// <returns>Returns a town if the FirmId is matched.</returns>
        public async Task<IEnumerable<Ward>> GetWardByFirm(int FirmId)
        {
            try
            {
                return await LoadListWithChildAsync<Ward>(d => d.IsDeleted == false && d.FirmId == FirmId, p => p.Firm, d => d.Firm.Department);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}