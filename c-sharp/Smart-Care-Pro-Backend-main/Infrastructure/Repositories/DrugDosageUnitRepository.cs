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
    public class DrugDosageUnitRepository : Repository<DrugDosageUnit>, IDrugDosageUnitRepository
    {
        public DrugDosageUnitRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a DrugDosageUnit by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugDosageUnit.</param>
        /// <returns>Returns a DrugDosageUnit if the key is matched.</returns>
        public async Task<DrugDosageUnit> GetDrugDosageUnitByKey(int key)
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
        /// The method is used to get the list of DrugDosageUnit.
        /// </summary>
        /// <returns>Returns a list of all DrugDosageUnit.</returns>        
        public async Task<IEnumerable<DrugDosageUnit>> GetDrugDosageUnit()
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
        /// The method is used to get an DrugDosageUnit by DrugDosageUnit Description.
        /// </summary>
        /// <param name="description">Name of an DrugDosageUnit.</param>
        /// <returns>Returns an DrugDosageUnit if the DrugDosageUnit description is matched.</returns>
        public async Task<DrugDosageUnit> GetDrugDosageUnitByName(string drugDosageUnit)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == drugDosageUnit.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}