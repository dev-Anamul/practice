using Domain.Entities;
using Infrastructure.Contracts;
using static Utilities.Constants.Enums;

/*
 * Created by   : Sayem
 * Date created : 11-03-2023
 * Modified by  : Biplob Roy
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of DrugRegimenRepository class.
    /// </summary>
    public class DrugRegimenRepository : Repository<DrugRegimen>, IDrugRegimenRepository
    {
        public DrugRegimenRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of DrugRegimens.
        /// </summary>
        /// <returns>Returns a list of all DrugRegimens.</returns>
        public async Task<IEnumerable<DrugRegimen>> GetDrugRegimens()
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
        /// The method is used to get the list of DrugRegimen  .
        /// </summary>
        /// <returns>Returns a list of all DrugRegimen by RegimenFor.</returns>
        public async Task<IEnumerable<DrugRegimen>> GetDrugRegimensByRegimenFor(int regimenFor)
        {
            try
            {
                return await QueryAsync(d => d.IsDeleted == false && d.RegimenFor == (RegimenFor)Enum.Parse(typeof(RegimenFor), regimenFor.ToString()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a DrugRegimen by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugRegimens.</param>
        /// <returns>Returns a DrugRegimen  if the key is matched.</returns>
        public async Task<DrugRegimen> GetDrugRegimenByKey(int key)
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
        /// The method is used to get an DrugRegimen by DrugRegimen Description.
        /// </summary>
        /// <param name="description">Name of an DrugRegimen.</param>
        /// <returns>Returns an DrugRegimen if the DrugRegimen description is matched.</returns>
        public async Task<DrugRegimen> GetDrugRegimenByName(string drugRegimen)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == drugRegimen.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}