using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 11.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IDrugSubclassRepository : IRepository<DrugSubclass>
    {
        /// <summary>
        /// The method is used to get a DrugSubclass by Description.
        /// </summary>
        /// <param name="Description">Description of a DrugSubclass.</param>
        /// <returns>Returns a DrugSubclass   if the Description is matched.
        public Task<DrugSubclass> GetDrugSubclassByDescription(string description);

        /// <summary>
        /// The method is used to get a DrugSubclass   by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugSubclasss.</param>
        /// <returns>Returns a DrugSubclass   if the key is matched.</returns>
        public Task<DrugSubclass> GetDrugSubclassByKey(int key);

        /// <summary>
        /// The method is used to get the list of DrugSubclass  .
        /// </summary>
        /// <returns>Returns a list of all DrugSubclass  .</returns>
        public Task<IEnumerable<DrugSubclass>> GetDrugSubclasses();

        /// <summary>
        /// The method is used to get the list of DrugSubclass  .
        /// </summary>
        /// <returns>Returns a list of all DrugSubclass  .</returns>
        public Task<IEnumerable<DrugSubclass>> GetDrugSubClassByClassId(int drugClassId);
    }
}