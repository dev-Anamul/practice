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
    public interface ITestSubtypeRepository : IRepository<TestSubtype>
    {
        /// <summary>
        /// The method is used to get a test subtype by subtype.
        /// </summary>
        /// <param name="testSubtype">Subtype of a test subtype.</param>
        /// <returns>Returns a test subtype if the subtype is matched.</returns>
        public Task<TestSubtype> GetTestSubtypeBySubtype(string testSubtype);

        /// <summary>
        /// The method is used to get a test subtype by key.
        /// </summary>
        /// <param name="key">Primary key of the table TestSubtypes.</param>
        /// <returns>Returns a test subtype if the key is matched.</returns>
        public Task<TestSubtype> GetTestSubtypeByKey(int key);

        /// <summary>
        /// The method is used to get the list of test subtypes.
        /// </summary>
        /// <returns>Returns a list of all test subtypes.</returns>
        public Task<IEnumerable<TestSubtype>> GetTestSubtypes();

        /// <summary>
        /// The method is used to get the TestsubType by TestId.
        /// </summary>
        /// <param name="testId">testId of the table TestType.</param>
        /// <returns>Returns a TestSubType if the testId is matched.</returns>
        public Task<IEnumerable<TestSubtype>> GetSubTestByTestType(int testId);

        /// <summary>
        /// The method is used to get the TestType by subTestId.
        /// </summary>
        /// <param name="subTestId">subTestId Primary key of  the table TestType.</param>
        /// <returns>Returns a TestType if the testTypeId is matched.</returns>
        public Task<TestType> GetTestTypeBySubTest(int subTestId);
    }
}