using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

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
    /// Implementation of ITestTypeRepository interface.
    /// </summary>
    public class TestTypeRepository : Repository<TestType>, ITestTypeRepository
    {
        public TestTypeRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a test type by type.
        /// </summary>
        /// <param name="testType">Type of a test type.</param>
        /// <returns>Returns a test type if the type is matched.</returns>
        public async Task<TestType> GetTestTypeByType(string testType)
        {
            try
            {
                return await FirstOrDefaultAsync(t => t.Description.ToLower().Trim() == testType.ToLower().Trim() && t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a test type by key.
        /// </summary>
        /// <param name="key">Primary key of the table TestTypes.</param>
        /// <returns>Returns a test type if the key is matched.</returns>
        public async Task<TestType> GetTestTypeByKey(int key)
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
        /// The method is used to get the list of test types.
        /// </summary>
        /// <returns>Returns a list of all test types.</returns>
        public async Task<IEnumerable<TestType>> GetTestTypes()
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
    }
}