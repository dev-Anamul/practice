using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Stephan
 * Date created : 25.12.2022
 * Modified by  : Stephan
 * Last modified: 27.12.2022
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IVaccineTypeRepository interface.
    /// </summary>
    public class VaccineTypeRepository : Repository<VaccineType>, IVaccineTypeRepository
    {
        public VaccineTypeRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a vaccine type by name.
        /// </summary>
        /// <param name="vaccineType">Name of a vaccine type.</param>
        /// <returns>Returns a vaccine type if the name is matched.</returns>
        public async Task<VaccineType> GetVaccineTypeByName(string vaccineType)
        {
            try
            {
                return await FirstOrDefaultAsync(v => v.Description.ToLower().Trim() == vaccineType.ToLower().Trim() && v.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a vaccine type by key.
        /// </summary>
        /// <param name="key">Primary key of the table VaccineTypes.</param>
        /// <returns>Returns a vaccine type if the key is matched.</returns>
        public async Task<VaccineType> GetVaccineTypeByKey(int key)
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
        /// The method is used to get the list of vaccine types.
        /// </summary>
        /// <returns>Returns a list of all vaccine types.</returns>
        public async Task<IEnumerable<VaccineType>> GetVaccineTypes()
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
    }
}