using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Stephan
 * Date created : 29.04.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IVisitTypeRepository interface.
    /// </summary>
    public class VisitTypeRepository : Repository<VisitType>, IVisitTypeRepository
    {
        public VisitTypeRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a type of visit by visit types.
        /// </summary>
        /// <param name="visitType">Visit type name of the visit of a client.</param>
        /// <returns>Returns a type of visit if the visit type is matched.</returns>
        public async Task<VisitType> GetVisitTypeByType(string visitType)
        {
            try
            {
                return await FirstOrDefaultAsync(v => v.Description.ToLower().Trim() == visitType.ToLower().Trim() && v.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a visit type by key.
        /// </summary>
        /// <param name="key">Primary key of the table VisitTypes.</param>
        /// <returns>Returns a visit type if the key is matched.</returns>
        public async Task<VisitType> GetVisitTypeByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(v => v.Oid == key && v.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of visit types.
        /// </summary>
        /// <returns>Returns a list of all visit types.</returns>
        public async Task<IEnumerable<VisitType>> GetVisitTypes()
        {
            try
            {
                return await QueryAsync(v => v.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}