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
    public interface IVaccineDoseRepository : IRepository<VaccineDose>
    {
        /// <summary>
        /// The method is used to get a VaccineDose by VaccineDose name.
        /// </summary>
        /// <param name="vaccineDose">Name of a VaccineDose.</param>
        /// <returns>Returns a VaccineDose if the VaccineDose name is matched.</returns>
        public Task<VaccineDose> GetVaccineDoseByName(string vaccineDose);

        /// <summary>
        /// The method is used to get a VaccineDose by key.
        /// </summary>
        /// <param name="key">Primary key of the table VaccineDoses.</param>
        /// <returns>Returns a VaccineDose if the key is matched.</returns>
        public Task<VaccineDose> GetVaccineDoseByKey(int key);

        /// <summary>
        /// The method is used to get the list of VaccineDoses.
        /// </summary>
        /// <returns>Returns a list of all VaccineDoses.</returns>
        public Task<IEnumerable<VaccineDose>> GetVaccineDoses();

        /// <summary>
        /// The method is used to get the list of VaccineDoses by VaccineDose types.
        /// </summary>
        /// <returns>Returns a list of all VaccineDoses by VaccineDose Id.</returns>
        public Task<IEnumerable<VaccineDose>> GetVaccineDoseByVaccineName(int key);
    }
}