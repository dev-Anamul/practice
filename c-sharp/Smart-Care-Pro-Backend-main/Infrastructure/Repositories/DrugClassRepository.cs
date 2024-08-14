using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Sayem
 * Date created : 07-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of DrugClassRepository class.
    /// </summary>
    public class DrugClassRepository : Repository<DrugClass>, IDrugClassRepository
    {
        public DrugClassRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a DrugClass by Description.
        /// </summary>
        /// <param name="Description">Description of a DrugClass.</param>
        /// <returns>Returns a DrugClass   if the Description is matched.
        public async Task<DrugClass> GetDrugClassByDescription(string description)
        {
            try
            {
                return await LoadWithChildAsync<DrugClass>(d => d.Description.ToLower().Trim() == description.ToLower().Trim() && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a DrugClass   by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugClasss.</param>
        /// <returns>Returns a DrugClass   if the key is matched.</returns>
        public async Task<DrugClass> GetDrugClassByKey(int key)
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
        /// The method is used to get the list of DrugClass  .
        /// </summary>
        /// <returns>Returns a list of all DrugClass  .</returns>
        public async Task<IEnumerable<DrugClass>> GetDrugClasses()
        {
            try
            {
                return await QueryAsync(d => d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}