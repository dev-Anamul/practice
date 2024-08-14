using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 16.02.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IVaccineRepository : IRepository<Vaccine>
    {
        /// <summary>
        /// The method is used to get a vaccine by vaccine name.
        /// </summary>
        /// <param name="vaccine">Name of a vaccine.</param>
        /// <returns>Returns a vaccine if the vaccine name is matched.</returns>
        public Task<Vaccine> GetVaccineByName(string vaccine);

        /// <summary>
        /// The method is used to get a vaccine by key.
        /// </summary>
        /// <param name="key">Primary key of the table Vaccines.</param>
        /// <returns>Returns a vaccine if the key is matched.</returns>
        public Task<Vaccine> GetVaccineByKey(int key);

        /// <summary>
        /// The method is used to get the list of vaccines.
        /// </summary>
        /// <returns>Returns a list of all vaccines.</returns>
        public Task<IEnumerable<Vaccine>> GetVaccines();

        /// <summary>
        /// The method is used to get the list of vaccines by vaccine types.
        /// </summary>
        /// <returns>Returns a list of all vaccines by vaccine Id.</returns>
        public Task<IEnumerable<Vaccine>> GetVaccineNamesByVaccineType(int key);
    }
}