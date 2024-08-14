using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 06.04.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IWHOStagesConditionRepository : IRepository<WHOStagesCondition>
    {
        /// <summary>
        /// The method is used to get a WHOStagesCondition by key.
        /// </summary>
        /// <param name="key">Primary key of the table WHOStagesCondition.</param>
        /// <returns>Returns a WHOStagesCondition if the key is matched.</returns>
        public Task<WHOStagesCondition> GetWHOStagesConditionByKey(int key);

        /// <summary>
        /// The method is used to get the list of WHOStagesConditions.
        /// </summary>
        /// <returns>Returns a list of all WHOStagesConditions.</returns>
        public Task<IEnumerable<WHOStagesCondition>> GetWHOStagesConditions();

        /// <summary>
        /// The method is used to get a WHOStagesCondition by WHOClinicalStageId.
        /// </summary>
        /// <param name="WHOClinicalStageId"></param>
        /// <returns>Returns a WHOStagesCondition if the WHOClinicalStageId is matched.</returns>
        public Task<IEnumerable<WHOStagesCondition>> GetWHOStagesConditionByWHOClinicalStage(int WHOClinicalStageId);
    }
}