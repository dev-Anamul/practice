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
    public interface IDrugDefinitionRepository : IRepository<GeneralDrugDefinition>
    {
        /// <summary>
        /// The method is used to get the list of Drug Definition.
        /// </summary>
        /// <returns>Returns a list of all Drug Definition.</returns>
        public Task<IEnumerable<GeneralDrugDefinition>> GetDrugDefinition();
    }
}
