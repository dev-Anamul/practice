using Domain.Entities;
using Infrastructure.Contracts;

/*
*Created by: Stephan
* Date created: 29.04.2023
* Modified by: Stephan
* Last modified: 13.08.2023
* Reviewed by:
*Date reviewed:
*/
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IWHOClinicalStageRepository class.
    /// </summary>
    public class WHOClinicalStageRepository : Repository<WHOClinicalStage>, IWHOClinicalStageRepository
    {
        public WHOClinicalStageRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a TestItem by key.
        /// </summary>
        /// <param name="key">Primary key of the table TestItems.</param>
        /// <returns>Returns a TestItem  if the key is matched.</returns>
        public async Task<WHOClinicalStage> GetWHOClinicalStageByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.Oid == key && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of WHOClinicalStage.
        /// </summary>
        /// <returns>Returns a list of all covid WHOClinicalStages.</returns>
        public async Task<IEnumerable<WHOClinicalStage>> GetWHOClinicalStages()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}