using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public interface IDrugDosageUnitRepository : IRepository<DrugDosageUnit>
    {
        /// <summary>
        /// The method is used to get a DrugDosageUnit by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugDosageUnit.</param>
        /// <returns>Returns a DrugDosageUnit if the key is matched.</returns>
        public Task<DrugDosageUnit> GetDrugDosageUnitByKey(int key);

        /// <summary>
        /// The method is used to get the list of DrugDosageUnit  .
        /// </summary>
        /// <returns>Returns a list of all DrugDosageUnit.</returns>
        public Task<IEnumerable<DrugDosageUnit>> GetDrugDosageUnit();

        /// <summary>
        /// The method is used to get an DrugDosageUnit by DrugDosageUnit Description.
        /// </summary>
        /// <param name="drugDosageUnit">Description of an DrugDosageUnit.</param>
        /// <returns>Returns an DrugDosageUnit if the DrugDosageUnit name is matched.</returns>
        public Task<DrugDosageUnit> GetDrugDosageUnitByName(string drugDosageUnit);
    }
}
