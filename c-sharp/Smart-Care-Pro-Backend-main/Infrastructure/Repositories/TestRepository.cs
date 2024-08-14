using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Rezwana
 * Date created : 21.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ITestRepository interface.
    /// </summary>
    public class TestRepository : Repository<Test>, ITestRepository
    {
        public TestRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a test by name.
        /// </summary>
        /// <param name="test">Name of a test.</param>
        /// <returns>Returns a test if the name is matched.</returns>
        public async Task<Test> GetTestByName(string test)
        {
            try
            {
                return await FirstOrDefaultAsync(t => t.Title.ToLower().Trim() == test.ToLower().Trim() && t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a test by key.
        /// </summary>
        /// <param name="key">Primary key of the table Tests.</param>
        /// <returns>Returns a test if the key is matched.</returns>
        public async Task<Test> GetTestByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(t => t.Oid == key && t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of tests.
        /// </summary>
        /// <returns>Returns a list of all tests.</returns>
        public async Task<IEnumerable<Test>> GetTests()
        {
            try
            {
                return await QueryAsync(t => t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the Test by testId.
        /// </summary>
        /// <param name="subTestId">subTestId Primary key of  the table TestType.</param>
        /// <returns>Returns a TestSubType if the testId is matched.</returns>
        public async Task<IEnumerable<Test>> GetTestBySubTest(int subTestId)
        {
            try
            {
                return await QueryAsync(d => d.IsDeleted == false && d.SubtypeId == subTestId);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get the TestSubType by testId.
        /// </summary>
        /// <param name="testId">testId Primary key of  the table Test.</param>
        /// <returns>Returns a TestSubType if the testId is matched.</returns>
         public async Task<TestSubtype> GetTestSubTypeByTest(int testId)
        {
            try
            {
                var data = await FirstOrDefaultAsync(d => d.IsDeleted == false && d.Oid == testId);

                return await context.TestSubTypes.FirstOrDefaultAsync(x => x.IsDeleted==false && x.Oid == data.SubtypeId) ;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}