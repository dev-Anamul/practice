using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IAllergyRepository : IRepository<Allergy>
    {
        /// <summary>
        /// The method is used to get the list of allergy.
        /// </summary>
        /// <returns>Returns a list of all allergies.</returns>
        public Task<IEnumerable<Allergy>> GetAllergies();

        /// <summary>
        /// The method is used to get alleries by encounterId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<Allergy>> GetAllergiesByEncounterId(Guid id);

        /// <summary>
        /// The method is used to get a allergy by key.
        /// </summary>
        /// <param name="key">Primary key of the table Allergies.</param>
        /// <returns>Returns a Allergy if the key is matched.</returns>
        public Task<Allergy> GetAllergyByKey(int key);

        /// <summary>
        /// The method is used to get an allergy by allergy name.
        /// </summary>
        /// <param name="allergy">Name of an allergy.</param>
        /// <returns>Returns an allergy if the allergy name is matched.</returns>
        public Task<Allergy> GetAllergyByName(string allergy);
    }
}