using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    /// <summary>
    /// Implementation of IDepartmentRepository interface.
    /// </summary>
    public interface IDepartmentRepository : IRepository<Department>
    {

        /// <summary>
        /// The method is used to get a Department    by DepartmentName.
        /// </summary>
        /// <param name="DepartmentName">DepartmentName of a Department.</param>
        /// <returns>Returns a Department    if the DepartmentName is matched.
        public Task<Department> GetDepartmentByName(string DepartmentName);

        /// <summary>
        /// The method is used to get a Department    by DepartmentName.
        /// </summary>
        /// <param name="DepartmentName">DepartmentName of a Department.</param>
        /// <param name="facilittId">facilittId primary key of Facilities table.</param>
        /// <returns>Returns a Department    if the DepartmentName is matched.
        public Task<Department> GetDepartmentByFacility(string DepartmentName, int facilittId);

        /// <summary>
        /// The method is used to get a Department    by key.
        /// </summary>
        /// <param name="key">Primary key of the table Departments.</param>
        /// <returns>Returns a Department    if the key is matched.</returns>
        public Task<Department> GetDepartmentByKey(int key);


        /// <summary>
        /// The method is used to get the list of Department 
        /// </summary>
        /// <param name="facilityID">Primary key of facility table</param>
        /// <returns>Returns a list of all Department </returns>
        public Task<IEnumerable<Department>> GetDepartments(int facilityID);
    }
}
