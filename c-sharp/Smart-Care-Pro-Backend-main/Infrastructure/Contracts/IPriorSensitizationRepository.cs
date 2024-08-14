using Domain.Entities;

/*
 * Created by   : Biplob Roy
 * Date created : 19.04.2023
 * Modified by  : Biplob Roy
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPriorSensitizationRepository : IRepository<PriorSensitization>
    {
        /// <summary>
        /// The method is used to get the list of PriorSensitization.
        /// </summary>
        /// <returns>Returns a list of all PriorSensitization.</returns>
        public Task<IEnumerable<PriorSensitization>> GetPriorSensitizations();

        /// <summary>
        /// The method is used to get a PriorSensitization by key.
        /// </summary>
        /// <param name="key">Primary key of the table PriorSensitizations.</param>
        /// <returns>Returns a PriorSensitization if the key is matched.</returns>
        public Task<PriorSensitization> GetPriorSensitizationByKey(int key);

        /// <summary>
        /// The method is used to get an PriorSensitization by PriorSensitization Description.
        /// </summary>
        /// <param name="priorSensitization">Description of an PriorSensitization.</param>
        /// <returns>Returns an PriorSensitization if the PriorSensitization name is matched.</returns>
        public Task<PriorSensitization> GetPriorSensitizationByName(string priorSensitization);
    }
}