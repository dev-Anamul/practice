using Domain.Entities;

/*
 * Created by    : Stephan
 * Date created  : 18.02.2023
 * Modified by   : Stephan
 * Last modified : 05.06.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */

namespace Infrastructure.Contracts
{
    public interface IExposerRiskRepository : IRepository<ExposureRisk>
    {
        /// <summary>
        /// The method is used to get the list of covid comorbidity.
        /// </summary>
        /// <returns>Returns a list of all covid comorbidities.</returns>
        public Task<IEnumerable<ExposureRisk>> GetExposerRiskes();

        /// <summary>
        /// The method is used to get covid exposureRisk to by CovidId.
        /// </summary>
        /// <param name="CovidId">CovidId of covid exposureRisk to.</param>
        /// <returns>Returns covid exposurerisk to if the CovidId is matched.</returns>
        public Task<IEnumerable<ExposureRisk>> GetExposureRiskByCovid(Guid CovidId);

        /// <summary>
        /// The method is used to get a birth history by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Returns a birth history if the key is matched.</returns>
        public Task<ExposureRisk> GetExposerRiskByKey(Guid key);
    }
}