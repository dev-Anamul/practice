using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : Brian
 * Last modified: 27.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IARTDrugClassRepository : IRepository<ARTDrugClass>
    {
        /// <summary>
        /// The method is used to get the list of ARTDrugClassess.
        /// </summary>
        /// <returns>Returns a list of all ARTDrugClassess.</returns>
        public Task<IEnumerable<ARTDrugClass>> GetARTDrugClasses();

        /// <summary>
        /// The method is used to get a ARTDrugClass by key.
        /// </summary>
        /// <param name="key">Primary key of the table ARTDrugClass.</param>
        /// <returns>Returns a ARTDrugClass if the key is matched.</returns>
        public Task<ARTDrugClass> GetARTDrugClassByKey(int key);

        /// <summary>
        /// The method is used to get an ARTDrugClass by ARTDrugClass Description.
        /// </summary>
        /// <param name="artDrugClass">Description of an ARTDrugClass.</param>
        /// <returns>Returns an ARTDrugClass if the ARTDrugClass name is matched.</returns>
        public Task<ARTDrugClass> GetARTDrugClassByName(string artDrugClass);

    }
}