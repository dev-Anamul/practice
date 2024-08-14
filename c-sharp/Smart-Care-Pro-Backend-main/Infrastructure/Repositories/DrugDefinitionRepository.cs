using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by   : Tariqul Islam
 * Date created : 11.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IDrugRouteRepository interface.
    /// </summary>
    public class DrugDefinitionRepository : Repository<GeneralDrugDefinition>, IDrugDefinitionRepository
    {
        public DrugDefinitionRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of DrugDefinition  .
        /// </summary>
        /// <returns>Returns a list of all DrugDefinition.</returns>
        public async Task<IEnumerable<GeneralDrugDefinition>> GetDrugDefinition()
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
    }
}
