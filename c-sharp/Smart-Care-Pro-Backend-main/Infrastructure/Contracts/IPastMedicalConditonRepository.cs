using Domain.Entities;

/*
 * Created by   : Bithy
 * Date created : 03.05.2023
 * Modified by  : Biplob Roy
 * Last modified: 01.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPastMedicalConditonRepository : IRepository<PastMedicalCondition>
    {
        /// <summary>
        /// The method is used to get the list of PastMedicalConditon.
        /// </summary>
        /// <returns>Returns a list of all PastMedicalConditons.</returns>
        public Task<IEnumerable<PastMedicalCondition>> GetPastMedicalConditons();

        /// <summary>
        /// The method is used to get a PastMedicalConditon by key.
        /// </summary>
        /// <param name="key">Primary key of the table PastMedicalConditons.</param>
        /// <returns>Returns a PastMedicalConditon if the key is matched.</returns>
        public Task<PastMedicalCondition> GetPastMedicalConditonByKey(int key);

        /// <summary>
        /// The method is used to get an PastMedicalConditon by PastMedicalConditon Description.
        /// </summary>
        /// <param name="pastMedicalConditon">Description of an PastMedicalConditon.</param>
        /// <returns>Returns an PastMedicalConditon if the PastMedicalConditon name is matched.</returns>
        public Task<PastMedicalCondition> GetPastMedicalConditonByName(string pastMedicalConditon);
    }
}