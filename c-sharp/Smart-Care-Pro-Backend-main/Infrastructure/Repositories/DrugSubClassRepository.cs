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
    /// Implementation of DrugSubclassRepository class.
    /// </summary>
    public class DrugSubclassRepository : Repository<DrugSubclass>, IDrugSubclassRepository
    {
        public DrugSubclassRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a DrugSubclass by Description.
        /// </summary>
        /// <param name="Description">Description of a DrugSubclass.</param>
        /// <returns>Returns a DrugSubclass   if the Description is matched.
        public async Task<DrugSubclass> GetDrugSubclassByDescription(string description)
        {
            try
            {
                return await LoadWithChildAsync<DrugSubclass>(d => d.Description.ToLower().Trim() == description.ToLower().Trim() && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a DrugSubclass   by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugSubclasss.</param>
        /// <returns>Returns a DrugSubclass   if the key is matched.</returns>
        public async Task<DrugSubclass> GetDrugSubclassByKey(int key)
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
        /// The method is used to get the list of DrugSubclass  .
        /// </summary>
        /// <returns>Returns a list of all DrugSubclass  .</returns>
        public async Task<IEnumerable<DrugSubclass>> GetDrugSubclasses()
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
        /// The method is used to get the list of DrugSubclass  .
        /// </summary>
        /// <returns>Returns a list of all DrugSubclass  .</returns>
        public async Task<IEnumerable<DrugSubclass>> GetDrugSubClassByClassId(int drugClassId)
        {
            try
            {
                return await QueryAsync(a => a.IsDeleted == false && a.DrugClassId == drugClassId);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}