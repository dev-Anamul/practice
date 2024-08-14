using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

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
    /// Implementation of IWHOStagesConditionRepository class.
    /// </summary>
    public class WHOStagesConditionRepository : Repository<WHOStagesCondition>, IWHOStagesConditionRepository
    {
        /// <summary>
        /// Implementation of WHOStagesConditionRepository interface.
        /// </summary>
        public WHOStagesConditionRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a WHOStagesCondition by key.
        /// </summary>
        /// <param name="key">Primary key of the table WHOStagesCondition.</param>
        /// <returns>Returns a WHOStagesCondition if the key is matched.</returns>
        public async Task<WHOStagesCondition> GetWHOStagesConditionByKey(int key)
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
        /// The method is used to get the list of WHOStagesConditions.
        /// </summary>
        /// <returns>Returns a list of all WHOStagesConditions.</returns>
        public async Task<IEnumerable<WHOStagesCondition>> GetWHOStagesConditions()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false, p => p.WHOClinicalStage);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of WHOStagesCondition by WHOClinicalStageId.
        /// </summary>
        /// <returns>Returns a list of all WHOStagesCondition by WHOClinicalStageId.</returns>
        public async Task<IEnumerable<WHOStagesCondition>> GetWHOStagesConditionByWHOClinicalStage(int WHOClinicalStageId)
        {
            try
            {
                return await QueryAsync(p => p.WHOClinicalStageId == WHOClinicalStageId && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}