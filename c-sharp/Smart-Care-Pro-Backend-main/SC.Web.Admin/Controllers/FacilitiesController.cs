using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 06.03.2023
 * Modified by  : Bella
 * Last modified: 30.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class FacilitiesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public FacilitiesController(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      #region Index

      public async Task<IActionResult> Index(string? search, int? page, int oid)
      {
         if (oid == 0)
            oid = Convert.ToInt32(TempData["manualDistrictId"]);

         ViewBag.Oid = oid;

         var getDistricts = await context.Districts.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

         TempData["manualDistrictId"] = getDistricts.Oid;

         ViewBag.ProvinceId = getDistricts.ProvinceId;
         ViewBag.DistrictId = getDistricts.Oid;
         ViewBag.DistrictName = getDistricts.Description;

         search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;

         if (page > 0 && search == null)
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.Facilities
             .Include(d => d.District)
             .ThenInclude(p => p.Provinces)
             .Where(x => (x.Description.ToLower().Contains(search) || x.HMISCode.ToLower().Contains(search) || x.District.Description.ToLower().Contains(search) || x.District.Provinces.Description.ToLower().Contains(search) || search == null) && x.DistrictId == oid && x.IsDeleted == false)
             .OrderBy(x => x.District.Provinces.Description)
             .ThenBy(x => x.District.Description)
             .ThenBy(x => x.Description);

         var facilities = query.ToPagedList(pageNumber, pageSize);

         if (facilities.PageCount > 0)
         {
            if (pageNumber > facilities.PageCount)
               facilities = query.ToPagedList(facilities.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(facilities);
      }
      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create(int? oid)
      {
         try
         {
            var getDistricts = await context.Districts.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

            ViewBag.DistrictId = getDistricts.Oid;
            ViewBag.DistrictName = getDistricts.Description;
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(Facility facility)
      {
         try
         {
            var facilityInDb = IsFacilityDuplicate(facility);
            var isHMISCodeExist = IsHMISCodeDuplicate(facility);

            if (!facilityInDb && !isHMISCodeExist)
            {
               if (ModelState.IsValid)
               {
                  facility.CreatedBy = session?.GetCurrentAdmin().Oid;
                  facility.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  facility.DateCreated = DateTime.Now;
                  facility.IsDeleted = false;
                  facility.IsSynced = false;

                  context.Facilities.Add(facility);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  //return RedirectToAction("Create", new { oid = facility.DistrictId });
                  return RedirectToAction(nameof(Create), new { oid = facility.DistrictId, module = "Clients" });
               }

               return View(facility);
            }
            else
            {
               var getDistricts = await context.Districts.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == facility.DistrictId);

               ViewBag.DistrictId = getDistricts.Oid;
               ViewBag.DistrictName = getDistricts.Description;

               if (facilityInDb)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
               else if (isHMISCodeExist)
                  ModelState.AddModelError("HMISCode", MessageConstants.DuplicateFound);
            }

            return View();
         }
         catch (Exception ex)
         {
            throw;
         }
      }
      #endregion

      #region Edit
      [HttpGet]
      public async Task<IActionResult> Edit(int? id)
      {
         if (id == null)
            return NotFound();

         var facility = await context.Facilities.Include(t => t.District).ThenInclude(d => d.Provinces).AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);
         var districts = context.Districts.ToList();

         if (facility == null)
            return NotFound();

         ViewBag.Provinces = new SelectList(context.Provinces, FieldConstants.Oid, FieldConstants.ProvinceName, facility.District.Provinces.Oid);
         ViewBag.Districts = new SelectList(districts.Where(d => d.ProvinceId == facility.District.ProvinceId).ToList(), FieldConstants.Oid, FieldConstants.DistrictName, facility.DistrictId);

         ViewBag.DistrictId = facility.DistrictId;

         return View(facility);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, Facility facility)
      {
         try
         {
            var facilityInDb = await context.Facilities.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == facility.Oid);

            bool isFacilityDuplicate = false;
            bool isHMISCodeDuplicate = false;

            if (facilityInDb.Description != facility.Description)
               isFacilityDuplicate = IsFacilityDuplicate(facility);

            if (facilityInDb.HMISCode != facility.HMISCode)
               isHMISCodeDuplicate = IsHMISCodeDuplicate(facility);

            if (!isFacilityDuplicate && !isHMISCodeDuplicate)
            {
               facility.DateCreated = facilityInDb.DateCreated;
               facility.CreatedBy = facilityInDb.CreatedBy;
               facility.ModifiedBy = session?.GetCurrentAdmin().Oid;
               facility.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               facility.DateModified = DateTime.Now;
               facility.IsDeleted = false;
               facility.IsSynced = false;

               context.Facilities.Update(facility);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               ViewBag.DistrictId = facilityInDb.DistrictId;

               return RedirectToAction(nameof(Index), new { oid = facility.DistrictId, module = "Clients" });
            }
            else
            {
               ViewBag.Provinces = new SelectList(context.Provinces, FieldConstants.Oid, FieldConstants.ProvinceName, facility.District.Provinces.Oid);

               ViewBag.Districts = new SelectList(context.Districts.Where(d => d.ProvinceId == facility.District.ProvinceId).ToList(), FieldConstants.Oid, FieldConstants.DistrictName, facility.DistrictId);

               ViewBag.DistrictId = facility.DistrictId;

               if (isFacilityDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
               else if (isHMISCodeDuplicate)
                  ModelState.AddModelError("HMISCode", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(facility);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var facility = await context.Facilities
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (facility == null)
            return NotFound();

         return View(facility);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var facility = await context.Facilities.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.Facilities.Remove(facility);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read

      public bool IsFacilityDuplicate(Facility facility)
      {
         try
         {
            var facilityInDb = context.Facilities.FirstOrDefault(c => c.Description.ToLower() == facility.Description.ToLower() && c.IsDeleted == false);

            if (facilityInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      public bool IsHMISCodeDuplicate(Facility facility)
      {
         try
         {
            if (!string.IsNullOrEmpty(facility.HMISCode))
            {
               var facilityInDb = context.Facilities.FirstOrDefault(c => c.HMISCode.ToLower() == facility.HMISCode.ToLower() && c.IsDeleted == false);

               if (facilityInDb != null)
                  return true;
            }

            return false;
         }
         catch
         {
            throw;
         }
      }

      public JsonResult LoadDistrict(int id)
      {
         var town = context.Districts.Where(p => p.ProvinceId == id && p.IsDeleted == false).ToList();
         return Json(town);
      }
      #endregion
   }
}