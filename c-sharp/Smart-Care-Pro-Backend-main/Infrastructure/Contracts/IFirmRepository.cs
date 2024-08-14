using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 29-01-2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IFirmRepository : IRepository<Firm>
   {
      /// <summary>
      /// The method is used to get a Firm by FirmName.
      /// </summary>
      /// <param name="firmName">FirmName of a Firm.</param>
      /// <returns>Returns a Firm if the FirmName is matched.</returns>
      public Task<Firm> GetFirmByName(string firmName);

      /// <summary>
      /// The method is used to get a Firm by FirmName and DepartmentId
      /// </summary>
      /// <param name="firmName">FirmName of a Firm.</param>
      /// <param name="departmentId">Department Id primary key of the Department table</param>
      /// <returns>Returns a Firm if the FirmName is matched</returns>
      public Task<Firm> GetFirmByDepartment(string firmName, int departmentId);
      public Task<Firm> checkDuplicateFirmByDepartment(string firmName, int departmentId,int firmId=0);

      /// <summary>
      /// The method is used to get a Firm by DepartmentId
      /// </summary>
      /// <param name="departmentId">Department Id primary key of the Department table</param>
      /// <returns>Returns a Firm   if the FirmName is matched</returns>
      public Task<List<Firm>> GetFirmByDepartment(int departmentId);

      /// <summary>
      /// The method is used to get a Firm   by key.
      /// </summary>
      /// <param name="key">Primary key of the table Firms.</param>
      /// <returns>Returns a Firm   if the key is matched.</returns>
      public Task<Firm> GetFirmByKey(int key);

      /// <summary>
      /// The method is used to get the list of Firm by FacilityId .
      /// </summary>
      /// <returns>Returns a list of all Firm by FacilityId .</returns>
      public Task<IEnumerable<Firm>> GetFirmsByFacilityId(int facilityId);

      /// <summary>
      /// The method is used to get the list of Firm  .
      /// </summary>
      /// <returns>Returns a list of all Firm  .</returns>
      public Task<IEnumerable<Firm>> GetFirms();
   }
}