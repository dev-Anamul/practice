using Domain.Entities;

/*
 * Created by   : Biplob Roy
 * Date created : 02.05.2023
 * Modified by  : Biplob Roy
 * Last modified: 03.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPresentingPartRepository : IRepository<PresentingPart>
    {
        /// <summary>
        /// The method is used to get a PresentingPart by key.
        /// </summary>
        /// <param name="key">Primary key of the table PresentingParts.</param>
        /// <returns>Returns a PresentingPart if the key is matched.</returns>
        public Task<PresentingPart> GetPresentingPartByKey(int key);

        /// <summary>
        /// The method is used to get the list of PresentingParts.
        /// </summary>
        /// <returns>Returns a list of all PresentingParts.</returns>
        public Task<IEnumerable<PresentingPart>> GetPresentingParts();

        /// <summary>
        /// The method is used to get an PresentingPart by PresentingPart Description.
        /// </summary>
        /// <param name="presentingPart">Description of an PresentingPart.</param>
        /// <returns>Returns an PresentingPart if the PresentingPart name is matched.</returns>
        public Task<PresentingPart> GetPresentingPartByName(string presentingPart);
    }
}