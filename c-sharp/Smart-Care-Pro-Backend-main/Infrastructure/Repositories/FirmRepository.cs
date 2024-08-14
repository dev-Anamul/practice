using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Bella
 * Date created : 29-01-2023
 * Modified by  : Bella 
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   /// <summary>
   /// Implementation of FirmRepository class.
   /// </summary>
   public class FirmRepository : Repository<Firm>, IFirmRepository
   {
      public FirmRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get a Firm by FirmName.
      /// </summary>
      /// <param name="FirmName">FirmName of a Firm.</param>
      /// <returns>Returns a Firm   if the FirmName is matched.
      public async Task<Firm> GetFirmByName(string firmName)
      {
         try
         {
            return await LoadWithChildAsync<Firm>(f => f.Description.ToLower().Trim() == firmName.ToLower().Trim() && f.IsDeleted == false, d => d.Department);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a Firm by FirmName and DepartmentId
      /// </summary>
      /// <param name="firmName">FirmName of a Firm.</param>
      /// <param name="departmentId">Departement Id primary key of the Department table</param>
      /// <returns>Returns a Firm   if the FirmName is matched</returns>
      public async Task<Firm> GetFirmByDepartment(string firmName, int departmentId)
      {
         try
         {
            return await LoadWithChildAsync<Firm>(f => f.Description.ToLower().Trim() == firmName.ToLower().Trim() && f.IsDeleted == false && f.DepartmentId == departmentId, d => d.Department);
         }
         catch (Exception)
         {
            throw;
         }
      }
      public async Task<Firm> checkDuplicateFirmByDepartment(string firmName, int departmentId, int firmId = 0)
      {
         try
         {
            return await LoadWithChildAsync<Firm>(f => f.Description.ToLower().Trim() == firmName.ToLower().Trim() && f.IsDeleted == false && f.DepartmentId == departmentId && f.Oid != firmId, d => d.Department);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a Firm by DepartmentId
      /// url: firm/firmByDepartment/{departmentId}
      /// </summary>
      /// <param name="departmentId">Departement Id primary key of the Department table</param>
      /// <returns>Returns a Firm   if the FirmName is matched</returns>
      public async Task<List<Firm>> GetFirmByDepartment(int departmentId)
      {
         try
         {
            var data = await LoadListWithChildAsync<Firm>(f => f.DepartmentId == departmentId && f.IsDeleted == false, d => d.Department);
            return data.ToList();
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a Firm   by key.
      /// </summary>
      /// <param name="key">Primary key of the table Firms.</param>
      /// <returns>Returns a Firm   if the key is matched.</returns>
      public async Task<Firm> GetFirmByKey(int key)
      {
         try
         {
            return await LoadWithChildAsync<Firm>(f => f.Oid == key && f.IsDeleted == false, d => d.Department);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of Firm by FacilityId .
      /// </summary>
      /// <returns>Returns a list of all Firm by FacilityId .</returns>
      public async Task<IEnumerable<Firm>> GetFirmsByFacilityId(int facilityId)
      {
         try
         {
            return await LoadListWithChildAsync<Firm>(f => f.IsDeleted == false && f.Department.FacilityId == facilityId, d => d.Department);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of Firm  .
      /// </summary>
      /// <returns>Returns a list of all Firm  .</returns>
      public async Task<IEnumerable<Firm>> GetFirms()
      {
         try
         {
            return await LoadListWithChildAsync<Firm>(f => f.IsDeleted == false, d => d.Department);
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}