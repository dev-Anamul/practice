using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Strphan
 * Date created : 25.12.2022
 * Modified by  : Stephan
 * Last modified: 27.12.2022
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IVaccineRepository interface.
    /// </summary>
    public class VaccineRepository : Repository<Vaccine>, IVaccineRepository
    {
        public VaccineRepository(DataContext contex) : base(contex)
        {

        }

        /// <summary>
        /// The method is used to get a vaccine by key.
        /// </summary>
        /// <param name="key">Primary key of the table Vaccines.</param>
        /// <returns>Returns a vaccine if the key is matched.</returns>
        public async Task<Vaccine> GetVaccineByKey(int key)
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
        /// The method is used to get a vaccine by vaccine name.
        /// </summary>
        /// <param name="vaccine">Name of a vaccine.</param>
        /// <returns>Returns a vaccine if the vaccine name is matched.</returns>
        public async Task<Vaccine> GetVaccineByName(string vaccine)
        {
            try
            {
                return await FirstOrDefaultAsync(v => v.Description.ToLower().Trim() == vaccine.ToLower().Trim() && v.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of vaccines.
        /// </summary>
        /// <returns>Returns a list of all vaccines.</returns>
        public async Task<IEnumerable<Vaccine>> GetVaccines()
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
        /// The method is used to get the vaccine by vaccine types.
        /// </summary>
        /// <param name="VaccineTypeId">VaccineTypeId of the table Vaccine.</param>
        /// <returns>Returns a vaccine if the VaccineTypeId is matched.</returns>
        public async Task<IEnumerable<Vaccine>> GetVaccineNamesByVaccineType(int VaccineTypeId)
        {
            try
            {
                return await QueryAsync(v => v.IsDeleted == false && v.VaccineTypeId == VaccineTypeId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}