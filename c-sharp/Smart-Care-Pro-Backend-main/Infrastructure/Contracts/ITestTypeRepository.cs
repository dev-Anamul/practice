using Domain.Entities;

/*
 * Created by    : Stephan
 * Date created  : 07.02.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Contracts
{
    public interface ITestTypeRepository : IRepository<TestType>
    {
        /// <summary>
        /// The method is used to get a test type by type.
        /// </summary>
        /// <param name="testType">Type of a test type.</param>
        /// <returns>Returns a test type if the type is matched.</returns>
        public Task<TestType> GetTestTypeByType(string testType);

        /// <summary>
        /// The method is used to get a test type by key.
        /// </summary>
        /// <param name="key">Primary key of the table TestTypes.</param>
        /// <returns>Returns a test type if the key is matched.</returns>
        public Task<TestType> GetTestTypeByKey(int key);

        /// <summary>
        /// The method is used to get the list of test types.
        /// </summary>
        /// <returns>Returns a list of all test types.</returns>
        public Task<IEnumerable<TestType>> GetTestTypes();
    }
}