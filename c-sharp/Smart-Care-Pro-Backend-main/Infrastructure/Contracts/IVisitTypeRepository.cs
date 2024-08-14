using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IVisitTypeRepository : IRepository<VisitType>
    {
        /// <summary>
        /// The method is used to get a type of visit by visit types.
        /// </summary>
        /// <param name="visitType">Visit type name of the visit of a client.</param>
        /// <returns>Returns a type of visit if the visit type is matched.</returns>
        public Task<VisitType> GetVisitTypeByType(string visitType);

        /// <summary>
        /// The method is used to get a visit type by key.
        /// </summary>
        /// <param name="key">Primary key of the table VisitTypes.</param>
        /// <returns>Returns a visit type if the key is matched.</returns>
        public Task<VisitType> GetVisitTypeByKey(int key);

        /// <summary>
        /// The method is used to get the list of visit types.
        /// </summary>
        /// <returns>Returns a list of all visit types.</returns>
        public Task<IEnumerable<VisitType>> GetVisitTypes();
    }
}