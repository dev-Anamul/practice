using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ICovidSymptomScreeningRepository : IRepository<CovidSymptomScreening>
    {
        /// <summary>
        /// The method is used to get a covid symptom screening by key.
        /// </summary>
        /// <param name="key">Primary key of the table CovidsymptomScreenings.</param>
        /// <returns>Returns a covid symptom screening if the key is matched.</returns>
        public Task<CovidSymptomScreening> GetCovidSymptomScreeningByKey(Guid key);

        /// <summary>
        /// The method is used to get Covid Semptom Screening to by CovidId.
        /// </summary>
        /// <param name="CovidId">CovidId of Covid Semptom Screening to.</param>
        /// <returns>Returns Covid Semptom Screening to if the CovidId is matched.</returns>
        public Task<IEnumerable<CovidSymptomScreening>> GetCovidSymptomScreeenByCovid(Guid CovidId);

        /// <summary>
        /// The method is used to get the list of covid symptom screening.
        /// </summary>
        /// <returns>Returns a list of all covid symptom screening.</returns>
        public Task<IEnumerable<CovidSymptomScreening>> GetCovidSymptomScreenings();
    }
}