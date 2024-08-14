using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Tariqul Islam
 * Date created : 13-03-2023
 * Modified by  : Biplob Roy
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class DrugFormulationRepository : Repository<DrugFormulation>, IDrugFormulationRepository
    {
        public DrugFormulationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a DrugFormulation by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugFormulation.</param>
        /// <returns>Returns a DrugFormulation if the key is matched.</returns>
        public async Task<DrugFormulation> GetDrugFormulationByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(s => s.Oid == key && s.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of DrugFormulation  .
        /// </summary>
        /// <returns>Returns a list of all DrugFormulation.</returns>        
        public async Task<IEnumerable<DrugFormulation>> GetDrugFormulation()
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

        /// <summary>
        /// The method is used to get an DrugFormulation by DrugFormulation Description.
        /// </summary>
        /// <param name="description">Name of an DrugFormulation.</param>
        /// <returns>Returns an DrugFormulation if the DrugFormulation description is matched.</returns>
        public async Task<DrugFormulation> GetDrugFormulationByName(string drugFormulation)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == drugFormulation.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
