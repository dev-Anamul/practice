using Domain.Entities;
using System.Security.Cryptography;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : Brian       
 * Last modified: 27.07.2023   
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IAllergicDrugRepository : IRepository<AllergicDrug>
    {
        /// <summary>
        /// The method is used to get the list of allergic drug.
        /// </summary>
        /// <returns>Returns a list of all allergic drugs.</returns>
        public Task<IEnumerable<AllergicDrug>> GetAllergicDrugs();

        /// <summary>
        /// The method is used to get a allergic grug by key.
        /// </summary>
        /// <param name="key">Primary key of the table AllergicDrugs.</param>
        /// <returns>Returns a allergic drug if the key is matched.</returns>
        public Task<AllergicDrug> GetAllergicDrugByKey(int key);

        /// <summary>
        /// The method is used to get an AllergicDrug by AllergicDrug DrugType.
        /// </summary>
        /// <param name="drugtype">Name of an AllergicDrug.</param>
        /// <returns>Returns an AllergicDrug if the drugtype is matched.</returns>
        public Task<AllergicDrug> GetAllergicDrugByName(string allergicDrug);
    }
}