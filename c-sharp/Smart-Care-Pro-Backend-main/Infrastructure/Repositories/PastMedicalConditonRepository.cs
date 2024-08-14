using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Bithy
 * Date created : 03.05.2023
 * Modified by  : Biplob Roy
 * Last modified: 01.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IPastMedicalConditonRepository interface.
    /// </summary>
    public class PastMedicalConditonRepository : Repository<PastMedicalCondition>, IPastMedicalConditonRepository
    {
        public PastMedicalConditonRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of PastMedicalConditons.
        /// </summary>
        /// <returns>Returns a list of all PastMedicalConditons.</returns>
        public async Task<IEnumerable<PastMedicalCondition>> GetPastMedicalConditons()
        {
            try
            {
                return await QueryAsync(a => a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a PastMedicalConditon by key.
        /// </summary>
        /// <param name="key">Primary key of the table PastMedicalConditons.</param>
        /// <returns>Returns a PastMedicalConditon  if the key is matched.</returns>
        public async Task<PastMedicalCondition> GetPastMedicalConditonByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.Oid == key && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an PastMedicalConditon by PastMedicalConditon Description.
        /// </summary>
        /// <param name="description">Name of an PastMedicalConditon.</param>
        /// <returns>Returns an PastMedicalConditon if the PastMedicalConditon description is matched.</returns>
        public async Task<PastMedicalCondition> GetPastMedicalConditonByName(string pastMedicalConditon)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == pastMedicalConditon.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}