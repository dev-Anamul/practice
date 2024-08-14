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
    public interface IDrugRegimenRepository : IRepository<DrugRegimen>
    {
        /// <summary>
        /// The method is used to get the list of DrugRegimens.
        /// </summary>
        /// <returns>Returns a list of all DrugRegimens.</returns>
        public Task<IEnumerable<DrugRegimen>> GetDrugRegimens();


        /// <summary>
        /// The method is used to get the list of DrugRegimen  .
        /// </summary>
        /// <returns>Returns a list of all DrugRegimen by RegimenFor.</returns>
        public Task<IEnumerable<DrugRegimen>> GetDrugRegimensByRegimenFor(int regimenFor);

        /// <summary>
        /// The method is used to get a DrugRegimen by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugRegimens.</param>
        /// <returns>Returns a DrugRegimen if the key is matched.</returns>
        public Task<DrugRegimen> GetDrugRegimenByKey(int key);

        /// <summary>
        /// The method is used to get an DrugRegimen by DrugRegimen Description.
        /// </summary>
        /// <param name="drugRegimen">Description of an DrugRegimen.</param>
        /// <returns>Returns an DrugRegimen if the DrugRegimen name is matched.</returns>
        public Task<DrugRegimen> GetDrugRegimenByName(string drugRegimen);
    }
}