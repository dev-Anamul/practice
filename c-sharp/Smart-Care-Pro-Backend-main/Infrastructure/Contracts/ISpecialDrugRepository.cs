using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ISpecialDrugRepository : IRepository<SpecialDrug>
    {
        /// <summary>
        /// The method is used to get a SpecialDrug by key.
        /// </summary>
        /// <param name="key">Primary key of the table SpecialDrug.</param>
        /// <returns>Returns a SpecialDrug if the key is matched.</returns>
        public Task<SpecialDrug> GetSpecialDrugByKey(int key);

        /// <summary>
        /// The method is used to get the list of SpecialDrug  .
        /// </summary>
        /// <returns>Returns a list of all SpecialDrug.</returns>
        public Task<IEnumerable<SpecialDrug>> GetSpecialDrug();

        /// <summary>
        /// The method is used to get the list of SpecialDrug  .
        /// </summary>
        /// <returns>Returns a list of all SpecialDrug  .</returns>
        public Task<IEnumerable<SpecialDrug>> GetSpecialDrugsByRegimenId(int regimenId);

        /// <summary>
        /// The method is used to get an SpecialDrug by SpecialDrug Description.
        /// </summary>
        /// <param name="SpecialDrug">Description of an SpecialDrug.</param>
        /// <returns>Returns an SpecialDrug if the SpecialDrug name is matched.</returns>
        public Task<SpecialDrug> GetSpecialDrugByName(string specialDrug);
    }
}
