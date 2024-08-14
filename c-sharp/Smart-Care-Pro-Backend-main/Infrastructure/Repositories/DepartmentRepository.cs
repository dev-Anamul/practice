using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Sayem
 * Date created : 29-01-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   /// <summary>
   /// Implementation of DepartmentRepository class.
   /// </summary>
   public class DepartmentRepository : Repository<Department>, IDepartmentRepository
   {
      public DepartmentRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get a Department by DepartmentName.
      /// </summary>
      /// <param name="DepartmentName">DepartmentName of a Department.</param>
      /// <returns>Returns a Department   if the DepartmentName is matched.
      public async Task<Department> GetDepartmentByName(string DepartmentName)
      {
         try
         {
            return await LoadWithChildAsync<Department>(d => d.Description.ToLower().Trim() == DepartmentName.ToLower().Trim() && d.IsDeleted == false, f => f.Facility);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a Department    by DepartmentName.
      /// </summary>
      /// <param name="DepartmentName">DepartmentName of a Department.</param>
      /// <param name="facilittId">facilittId primary key of Facilities table.</param>
      /// <returns>Returns a Department    if the DepartmentName is matched.
      public async Task<Department> GetDepartmentByFacility(string DepartmentName, int facilittId)
      {
         try
         {
            return await LoadWithChildAsync<Department>(d => d.Description.ToLower().Trim() == DepartmentName.ToLower().Trim() && d.IsDeleted == false && d.FacilityId == facilittId, f => f.Facility);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a Department   by key.
      /// </summary>
      /// <param name="key">Primary key of the table Departments.</param>
      /// <returns>Returns a Department   if the key is matched.</returns>
      public async Task<Department> GetDepartmentByKey(int key)
      {
         try
         {
            return await FirstOrDefaultAsync(d => d.Oid == key && d.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of Department  .
      /// </summary>
      /// <returns>Returns a list of all Department  .</returns>
      public async Task<IEnumerable<Department>> GetDepartments(int facilityID)
      {
         try
         {
            return await QueryAsync(d => d.IsDeleted == false && d.FacilityId == facilityID);
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}