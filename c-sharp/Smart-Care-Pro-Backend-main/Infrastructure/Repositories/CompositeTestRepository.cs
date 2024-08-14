using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Bithy
 * Date created : 22.02.2023
 * Modified by  : Biplob Roy
 * Last modified: 27.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class CompositeTestRepository : Repository<CompositeTest>, ICompositeTestRepository
    {
        public CompositeTestRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a CompositeTest by key.
        /// </summary>
        /// <param name="key">Primary key of the table CompositeTests.</param>
        /// <returns>Returns a CompositeTest  if the key is matched.</returns>
        public async Task<CompositeTest> GetCompositeTestByKey(int key)
        {
            try
            {
                return await LoadWithChildAsync<CompositeTest>(d => d.Oid == key && d.IsDeleted == false, x => x.TestItems);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of CompositeTest.
        /// </summary>
        /// <returns>Returns a list of all covid CompositeTests.</returns>
        public async Task<IEnumerable<CompositeTest>> GetCompositeTests()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an CompositeTest by CompositeTest Description.
        /// </summary>
        /// <param name="description">Name of an CompositeTest.</param>
        /// <returns>Returns an CompositeTest if the CompositeTest description is matched.</returns>
        public async Task<CompositeTest> GetCompositeTestByName(string compositeTest)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == compositeTest.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}