using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Stephan
 * Date created : 02.05.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IVaccineDoseRepository interface.
    /// </summary>
    public class VaccineDoseRepository : Repository<VaccineDose>, IVaccineDoseRepository
    {
        public VaccineDoseRepository(DataContext contex) : base(contex)
        {

        }

        /// <summary>
        /// The method is used to get a VaccineDose by key.
        /// </summary>
        /// <param name="key">Primary key of the table VaccineDoses.</param>
        /// <returns>Returns a VaccineDose if the key is matched.</returns>
        public async Task<VaccineDose> GetVaccineDoseByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(v => v.Oid == key && v.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a VaccineDose by VaccineDose name.
        /// </summary>
        /// <param name="VaccineDose">Name of a VaccineDose.</param>
        /// <returns>Returns a VaccineDose if the VaccineDose name is matched.</returns>
        public async Task<VaccineDose> GetVaccineDoseByName(string vaccineDose)
        {
            try
            {
                return await FirstOrDefaultAsync(v => v.Description.ToLower().Trim() == vaccineDose.ToLower().Trim() && v.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of VaccineDoses.
        /// </summary>
        /// <returns>Returns a list of all VaccineDoses.</returns>
        public async Task<IEnumerable<VaccineDose>> GetVaccineDoses()
        {
            try
            {
                return await QueryAsync(v => v.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the VaccineDose by VaccineDose types.
        /// </summary>
        /// <param name="VaccineId">VaccineId of the table VaccineDose.</param>
        /// <returns>Returns a VaccineDose if the VaccineDoseTypeId is matched.</returns>
        public async Task<IEnumerable<VaccineDose>> GetVaccineDoseByVaccineName(int VaccineId)
        {
            try
            {
                return await QueryAsync(v => v.IsDeleted == false && v.VaccineId == VaccineId,v => v.Vaccine);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}