using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Tariqul Islam
 * Date created : 13-03-2023
 * Modified by  : Biplob Roy
 * Last modified: 03.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class SpecialDrugRepository : Repository<SpecialDrug>, ISpecialDrugRepository
    {
        public SpecialDrugRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a SpecialDrug by key.
        /// </summary>
        /// <param name="key">Primary key of the table SpecialDrug.</param>
        /// <returns>Returns a SpecialDrug if the key is matched.</returns>
        public async Task<SpecialDrug> GetSpecialDrugByKey(int key)
        {
            try
            {
                return await LoadWithChildAsync<SpecialDrug>(s => s.Oid == key && s.IsDeleted == false, x => x.DrugDosageUnit, z => z.DrugFormulation);
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
        public async Task<IEnumerable<SpecialDrug>> GetSpecialDrug()
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
        /// The method is used to get the list of SpecialDrug.
        /// </summary>
        /// <param name="regimenId">Name of an specialDrug.</param>
        /// <returns>Returns a list of all SpecialDrug.</returns>
        public async Task<IEnumerable<SpecialDrug>> GetSpecialDrugsByRegimenId(int regimenId)
        {
            try
            {
                return await LoadListWithChildAsync<SpecialDrug>(s => s.IsDeleted == false && s.RegimenId == regimenId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an SpecialDrug by SpecialDrug specialDrug.
        /// </summary>
        /// <param name="specialDrug">Name of an specialDrug.</param>
        /// <returns>Returns an SpecialDrug if the  specialDrug is matched.</returns>
        public async Task<SpecialDrug> GetSpecialDrugByName(string specialDrug)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == specialDrug.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}