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
    public interface IDrugFormulationRepository : IRepository<DrugFormulation>
    {
        /// <summary>
        /// The method is used to get a DrugFormulation by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugFormulation.</param>
        /// <returns>Returns a DrugFormulation if the key is matched.</returns>
        public Task<DrugFormulation> GetDrugFormulationByKey(int key);

        /// <summary>
        /// The method is used to get the list of DrugFormulation  .
        /// </summary>
        /// <returns>Returns a list of all DrugFormulation.</returns>
        public Task<IEnumerable<DrugFormulation>> GetDrugFormulation();

        /// <summary>
        /// The method is used to get an DrugFormulation by DrugFormulation Description.
        /// </summary>
        /// <param name="drugFormulation">Description of an DrugFormulation.</param>
        /// <returns>Returns an DrugFormulation if the DrugFormulation name is matched.</returns>
        public Task<DrugFormulation> GetDrugFormulationByName(string drugFormulation);
    }
}
