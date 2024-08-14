using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 25.12.2022
 * Modified by  : Lion
 * Last modified: 27.12.2022
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ICaCxScreeningMethodRepository : IRepository<CaCxScreeningMethod>
    {
        /// <summary>
        /// The method is used to get cacx screening method by cacx screening method name.
        /// </summary>
        /// <param name="screeningMethod">Name of cacx screening method.</param>
        /// <returns>Returns a cacx screening method if the cacx screening method name is matched.</returns>
        public Task<CaCxScreeningMethod> GetCaCxScreeningMethodByName(string screeningMethod);

        /// <summary>
        /// The method is used to get a cacx screening method by key.
        /// </summary>
        /// <param name="key">Primary key of the table CaCxScreeningMethods.</param>
        /// <returns>Returns a cacx screening method if the key is matched.</returns>
        public Task<CaCxScreeningMethod> GetCaCxScreeningMethodByKey(int key);

        /// <summary>
        /// The method is used to get the list of cacx screening methods.
        /// </summary>
        /// <returns>Returns a list of all cacx screening methods.</returns>
        public Task<IEnumerable<CaCxScreeningMethod>> GetCaCxScreeningMethods();
    }
}