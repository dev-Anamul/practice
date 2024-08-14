using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : Brian
 * Last modified: 27.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class ARTDrugClassRepository : Repository<ARTDrugClass>, IARTDrugClassRepository
    {
        public ARTDrugClassRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of ARTDrugClasses.
        /// </summary>
        /// <returns>Returns a list of all ARTDrugClasses.</returns>
        public async Task<IEnumerable<ARTDrugClass>> GetARTDrugClasses()
        {
            try
            {
                return await QueryAsync(a => a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a ARTDrugClass by key.
        /// </summary>
        /// <param name="key">Primary key of the table ARTDrugClass.</param>
        /// <returns>Returns a ARTDrugClass if the key is matched.</returns>
        public async Task<ARTDrugClass> GetARTDrugClassByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(b => b.Oid == key && b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an aRTDrugClass by ARTDrugClass Description.
        /// </summary>
        /// <param name="artDrugClass">Name of an ARTDrugClass.</param>
        /// <returns>Returns an ARTDrugClass if the ARTDrugClass description is matched.</returns>
        public async Task<ARTDrugClass> GetARTDrugClassByName(string artDrugClass)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == artDrugClass.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}