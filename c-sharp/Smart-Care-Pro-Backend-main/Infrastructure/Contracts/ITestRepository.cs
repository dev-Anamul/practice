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
    public interface ITestRepository : IRepository<Test>
    {
        /// <summary>
        /// The method is used to get a test by name.
        /// </summary>
        /// <param name="test">Name of a test.</param>
        /// <returns>Returns a test if the name is matched.</returns>
        public Task<Test> GetTestByName(string test);

        /// <summary>
        /// The method is used to get a test by key.
        /// </summary>
        /// <param name="key">Primary key of the table Tests.</param>
        /// <returns>Returns a test if the key is matched.</returns>
        public Task<Test> GetTestByKey(int key);

        /// <summary>
        /// The method is used to get the list of tests.
        /// </summary>
        /// <returns>Returns a list of all tests.</returns>
        public Task<IEnumerable<Test>> GetTests();

        /// <summary>
        /// The method is used to get the Test by subTestId.
        /// </summary>
        /// <param name="subTestId">subTestId Foregin key of the table TestSubType.</param>
        /// <returns>Returns a Test if the subTestId is matched.</returns>
        public Task<IEnumerable<Test>> GetTestBySubTest(int subTestId);

        /// <summary>
        /// The method is used to get the TestSubType by testId.
        /// </summary>
        /// <param name="testId">testId Primary key of  the table Test.</param>
        /// <returns>Returns a TestSubType if the testId is matched.</returns>
        public Task<TestSubtype> GetTestSubTypeByTest(int testId);
    }
}