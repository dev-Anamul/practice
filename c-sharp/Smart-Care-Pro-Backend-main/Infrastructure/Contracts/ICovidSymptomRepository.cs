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
    public interface ICovidSymptomRepository : IRepository<CovidSymptom>
    {
        /// <summary>
        /// The method is used to get the list of covid symptoms.
        /// </summary>
        /// <returns>Returns a list of all covid symptoms.</returns>
        public Task<IEnumerable<CovidSymptom>> GetCovidSymptoms();

        /// <summary>
        /// The method is used to get a CovidSymptom by key.
        /// </summary>
        /// <param name="key">Primary key of the table CovidSymptoms.</param>
        /// <returns>Returns a CovidSymptom if the key is matched.</returns>
        public Task<CovidSymptom> GetCovidSymptomByKey(int key);

        /// <summary>
        /// The method is used to get an CovidSymptom by CovidSymptom Description.
        /// </summary>
        /// <param name="covidSymptom">Description of an CovidSymptom.</param>
        /// <returns>Returns an CovidSymptom if the CovidSymptom name is matched.</returns>
        public Task<CovidSymptom> GetCovidSymptomByName(string covidSymptom);
    }
}