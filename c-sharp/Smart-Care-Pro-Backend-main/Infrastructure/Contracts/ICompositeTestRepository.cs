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
    public interface ICompositeTestRepository : IRepository<CompositeTest>
    {
        /// <summary>
        /// The method is used to get a CompositeTest by key.
        /// </summary>
        /// <param name="key">Primary key of the table CompositeTests.</param>
        /// <returns>Returns a CompositeTest if the key is matched.</returns>
        public Task<CompositeTest> GetCompositeTestByKey(int key);

        /// <summary>
        /// The method is used to get the list of CompositeTest.
        /// </summary>
        /// <returns>Returns a list of all CompositeTests.</returns>
        public Task<IEnumerable<CompositeTest>> GetCompositeTests();

        /// <summary>
        /// The method is used to get an CompositeTest by CompositeTest Description.
        /// </summary>
        /// <param name="compositeTest">Description of an CompositeTest.</param>
        /// <returns>Returns an CompositeTest if the CompositeTest name is matched.</returns>
        public Task<CompositeTest> GetCompositeTestByName(string compositeTest);

    }
}