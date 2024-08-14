using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 07-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IDrugClassRepository : IRepository<DrugClass>
    {
        /// <summary>
        /// The method is used to get a DrugClass by Description.
        /// </summary>
        /// <param name="Description">Description of a DrugClass.</param>
        /// <returns>Returns a DrugClass   if the Description is matched.
        public Task<DrugClass> GetDrugClassByDescription(string description);

        /// <summary>
        /// The method is used to get a DrugClass   by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugClasss.</param>
        /// <returns>Returns a DrugClass   if the key is matched.</returns>
        public Task<DrugClass> GetDrugClassByKey(int key);

        /// <summary>
        /// The method is used to get the list of DrugClass  .
        /// </summary>
        /// <returns>Returns a list of all DrugClass  .</returns>
        public Task<IEnumerable<DrugClass>> GetDrugClasses();
    }
}