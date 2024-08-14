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
    public interface IWHOClinicalStageRepository : IRepository<WHOClinicalStage>
    {
        /// <summary>
        /// The method is used to get a WHOClinicalStage by key.
        /// </summary>
        /// <param name="key">Primary key of the table WHOClinicalStages.</param>
        /// <returns>Returns a WHOClinicalStage if the key is matched.</returns>
        public Task<WHOClinicalStage> GetWHOClinicalStageByKey(int key);

        /// <summary>
        /// The method is used to get the list of WHOClinicalStage.
        /// </summary>
        /// <returns>Returns a list of all WHOClinicalStages.</returns>
        public Task<IEnumerable<WHOClinicalStage>> GetWHOClinicalStages();
    }
}