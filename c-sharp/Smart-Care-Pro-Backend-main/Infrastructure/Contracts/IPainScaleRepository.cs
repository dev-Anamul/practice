using Domain.Entities;

/*
 * Created by   : Bithy
 * Date created : 06-02-2023
 * Modified by  : Biplob Roy
 * Last modified: 01.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPainScaleRepository : IRepository<PainScale>
    {
        /// <summary>
        /// The method is used to get a pain scale by key.
        /// </summary>
        /// <param name="key">Primary key of the table PainScales.</param>
        /// <returns>Returns a painScales if the key is matched.</returns>
        public Task<PainScale> GetPainScaleByKey(int key);

        /// <summary>
        /// The method is used to get the list of painScales.
        /// </summary>
        /// <returns>Returns a list of all painScales.</returns>
        public Task<IEnumerable<PainScale>> GetPainScales();

        /// <summary>
        /// The method is used to get an PainScale by PainScale Description.
        /// </summary>
        /// <param name="painScale">Description of an PainScale.</param>
        /// <returns>Returns an PainScale if the PainScale name is matched.</returns>
        public Task<PainScale> GetPainScaleByName(string painScale);
    }
}