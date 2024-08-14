using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IConstitutionalSymptomRepository : IRepository<ConstitutionalSymptom>
    {
        /// <summary>
        /// The method is used to get the list of constitutional symptom.
        /// </summary>
        /// <returns>Returns a list of all constitutional symptoms.</returns>
        public Task<IEnumerable<ConstitutionalSymptom>> GetConstitutionalSymptoms();

        /// <summary>
        /// The method is used to get a ConstitutionalSymptom by key.
        /// </summary>
        /// <param name="key">Primary key of the table ConstitutionalSymptom.</param>
        /// <returns>Returns a ConstitutionalSymptom if the key is matched.</returns>
        public Task<ConstitutionalSymptom> GetConstitutionalSymptomByKey(int key);

        /// <summary>
        /// The method is used to get an ConstitutionalSymptom by ConstitutionalSymptom Description.
        /// </summary>
        /// <param name="constitutionalSymptom">Description of an ConstitutionalSymptom.</param>
        /// <returns>Returns an ConstitutionalSymptom if the ConstitutionalSymptom name is matched.</returns>
        public Task<ConstitutionalSymptom> GetConstitutionalSymptomByName(string constitutionalSymptom);
    }
}