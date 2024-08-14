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
    /// Implementation of ITestSubtypeRepository interface.
    /// </summary>
    public class TestSubtypeRepository : Repository<TestSubtype>, ITestSubtypeRepository
    {
        public TestSubtypeRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a test subtype by subtype.
        /// </summary>
        /// <param name="testSubtype">Subtype of a test subtype.</param>
        /// <returns>Returns a test subtype if the subtype is matched.</returns>
        public async Task<TestSubtype> GetTestSubtypeBySubtype(string testSubtype)
        {
            try
            {
                return await FirstOrDefaultAsync(t => t.Description.ToLower().Trim() == testSubtype.ToLower().Trim() && t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a test subtype by key.
        /// </summary>
        /// <param name="key">Primary key of the table TestSubtypes.</param>
        /// <returns>Returns a test subtype if the key is matched.</returns>
        public async Task<TestSubtype> GetTestSubtypeByKey(int key)
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
        /// The method is used to get the list of test subtypes.
        /// </summary>
        /// <returns>Returns a list of all test subtypes.</returns>
        public async Task<IEnumerable<TestSubtype>> GetTestSubtypes()
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
        /// The method is used to get the TestsubType by TestId.
        /// </summary>
        /// <param name="testId">testId Primary key of  the table TestType.</param>
        /// <returns>Returns a TestSubType if the testId is matched.</returns>
        public async Task<IEnumerable<TestSubtype>> GetSubTestByTestType(int testId)
        {
            try
            {
                return await QueryAsync(d => d.IsDeleted == false && d.TestTypeId == testId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the TestType by subTestId.
        /// </summary>
        /// <param name="subTestId">subTestId Primary key of  the table TestType.</param>
        /// <returns>Returns a TestType if the testTypeId is matched.</returns>
        public async Task<TestType> GetTestTypeBySubTest(int subTestId)
        {
            try
            {
                var data = await FirstOrDefaultAsync(d => d.IsDeleted == false && d.Oid == subTestId);

                return await context.TestTypes.FirstOrDefaultAsync(x => x.IsDeleted==false && x.Oid == data.TestTypeId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}