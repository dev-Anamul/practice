using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by   : Lion
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ITBIdentificationMethodRepository : IRepository<TBIdentificationMethod>
    {
        /// <summary>
        /// The method is used to get a country by country name.
        /// </summary>
        /// <param name="tBIdentificationMethod">Name of a country.</param>
        /// <returns>Returns a county if the country name is matched.</returns>
        public Task<TBIdentificationMethod> GetTBIdentificationMethodByName(string tBIdentificationMethod);

        /// <summary>
        /// The method is used to get a country by key.
        /// </summary>
        /// <param name="key">Primary key of the table tBIdentificationMethods.</param>
        /// <returns>Returns a country if the key is matched.</returns>
        public Task<TBIdentificationMethod> GetTBIdentificationMethodByKey(int key);

        /// <summary>
        /// The method is used to get the list of tBIdentificationMethods.
        /// </summary>
        /// <returns>Returns a list of all tBIdentificationMethods.</returns>
        public Task<IEnumerable<TBIdentificationMethod>> GetTBIdentificationMethods();
    }
}