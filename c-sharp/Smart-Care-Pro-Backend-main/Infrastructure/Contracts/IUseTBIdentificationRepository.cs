using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by   : Lion
 * Date created : 30.03.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IUseTBIdentificationMethodRepository : IRepository<UsedTBIdentificationMethod>
    {
        /// <summary>
        /// The method is used to get a usedTBIdentificationMethod by key.
        /// </summary>
        /// <param name="key">Primary key of the table UsedTBIdentificationMethods.</param>
        /// <returns>Returns a usedTBIdentificationMethod if the key is matched.</returns>
        public Task<UsedTBIdentificationMethod> GetUsedTBIdentificationMethodByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of usedTBIdentificationMethods.
        /// </summary>
        /// <returns>Returns a list of all usedTBIdentificationMethods.</returns>
        public Task<IEnumerable<UsedTBIdentificationMethod>> GetUsedTBIdentificationMethods();

        /// <summary>
        /// The method is used to get the list of usedTBIdentificationMethod by EncounterId.
        /// </summary>
        ///  <param name="EncounterId">EncounterId of the table UsedTBIdentificationMethods.</param>
        /// <returns>Returns a list of all usedTBIdentificationMethod by EncounterId.</returns>
        public Task<IEnumerable<UsedTBIdentificationMethod>> GetUsedTBIdentificationMethodByEncounter(Guid EncounterId);
    }
}