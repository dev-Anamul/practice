using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Domain.Dto;

/*
 * Created by   : Lion
 * Date created : 29-01-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   /// <summary>
   /// Implementation of BedRepository class.
   /// </summary>
   public class BedRepository : Repository<Bed>, IBedRepository
   {
      public BedRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get a Bed by BedName.
      /// </summary>
      /// <param name="bedName">BedName of a Bed.</param>
      /// <returns>Returns a Bed   if the BedName is matched.
      public async Task<Bed> GetBedByName(string bedName)
      {
         try
         {
            return await LoadWithChildAsync<Bed>(b => b.Description.ToLower().Trim() == bedName.ToLower().Trim() && b.IsDeleted == false, w => w.Ward);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a Bed   by key.
      /// </summary>
      /// <param name="key">Primary key of the table Beds.</param>
      /// <returns>Returns a Bed   if the key is matched.</returns>
      public async Task<Bed> GetBedByKey(int key)
      {
         try
         {
            return await LoadWithChildAsync<Bed>(b => b.Oid == key && b.IsDeleted == false, w => w.Ward, f => f.Ward.Firm, d => d.Ward.Firm.Department);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of Bed by facilityId .
      /// </summary>
      /// <returns>Returns a list of all Bed by facilityId .</returns>
      public async Task<IEnumerable<Bed>> GetBedsByFacilityId(int facilityId)
      {
         try
         {
            return await LoadListWithChildAsync<Bed>(b => b.IsDeleted == false && b.Ward.Firm.Department.FacilityId == facilityId, w => w.Ward, f => f.Ward.Firm, d => d.Ward.Firm.Department);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of Bed  .
      /// </summary>
      /// <returns>Returns a list of all Bed  .</returns>
      public async Task<IEnumerable<Bed>> GetBeds()
      {
         try
         {
            return await LoadListWithChildAsync<Bed>(b => b.IsDeleted == false, w => w.Ward, f => f.Ward.Firm, d => d.Ward.Firm.Department);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of Bed by ward id  .
      /// </summary>
      /// <param name="wardId"></param>
      /// <returns></returns>
      public async Task<IEnumerable<Bed>> GetBedByWard(int wardId)
      {
         try
         {
            return await LoadListWithChildAsync<Bed>(d => d.IsDeleted == false && d.WardId == wardId, w => w.Ward, f => f.Ward.Firm, d => d.Ward.Firm.Department);
         }
         catch (Exception)
         {
            throw;
         }
      }

      public async Task<IEnumerable<BedDropDownDto>> GetBedByWardForDropDown(int wardId)
      {
         try
         {
            var bedsByWard = await LoadListWithChildAsync<Bed>(d => d.IsDeleted == false && d.WardId == wardId, w => w.Ward, f => f.Ward.Firm, d => d.Ward.Firm.Department);
            return bedsByWard.Select(x => new BedDropDownDto()
            {
               Oid = x.Oid,
               Description = x.Description,
               Taken = context.Encounters.Where(y => y.IsDeleted == false && y.BedId == x.Oid && y.IPDDischargeDate == null).Any()
            }).ToList();
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}