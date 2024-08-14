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
    public interface ICovidComorbidityRepository : IRepository<CovidComorbidity>
    {
        /// <summary>
        /// The method is used to get the list of covid comorbidity.
        /// </summary>
        /// <returns>Returns a list of all covid comorbidities.</returns>
        public Task<IEnumerable<CovidComorbidity>> GetCovidComorbidities();

        /// <summary>
        /// The method is used to get covid comorbidity to by CovidId.
        /// </summary>
        /// <param name="CovidId">CovidId of covid comorbidity to.</param>
        /// <returns>Returns covid comorbidity to if the CovidId is matched.</returns>
        public Task<IEnumerable<CovidComorbidity>> GetCovidComorbidityByCovid(Guid CovidID);

        /// <summary>
        /// The method is used to get a birth history by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Returns a birth history if the key is matched.</returns>
        public Task<CovidComorbidity> GetCovidComobidityByKey(Guid key);
    }
}