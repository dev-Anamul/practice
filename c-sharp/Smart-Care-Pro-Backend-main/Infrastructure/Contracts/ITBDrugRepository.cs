using Domain.Entities;

/*
 * Created by    : Stephan
 * Date created  : 07.02.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Contracts
{
    public interface ITBDrugRepository : IRepository<TBDrug>
    {
        /// <summary>
        /// The method is used to get a TB drug by drug name.
        /// </summary>
        /// <param name="drugName">Name of a TB drug.</param>
        /// <returns>Returns a TB drug if the drug name is matched.</returns>
        public Task<TBDrug> GetTBDrugByName(string drugName);

        /// <summary>
        /// The method is used to get a TB drug by key.
        /// </summary>
        /// <param name="key">Primary key of the table TBDrugs.</param>
        /// <returns>Returns a TB drug if the key is matched.</returns>
        public Task<TBDrug> GetTBDrugByKey(int key);

        /// <summary>
        /// The method is used to get the list of TB drugs.
        /// </summary>
        /// <returns>Returns a list of all TB drugs.</returns>
        public Task<IEnumerable<TBDrug>> GetTBDrugs();
    }
}