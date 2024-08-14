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
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class DistrictsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public DistrictsController(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      #region Index
      public async Task<IActionResult> Index(string? search, int? page, int oid)
      {
         if (oid == 0)
            oid = Convert.ToInt32(TempData["manualId"]);

         ViewBag.Oid = oid;

         var getProvince = await context.Provinces.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

         TempData["manualId"] = getProvince.Oid;

         ViewBag.ProvinceId = getProvince.Oid;
         ViewBag.ProvinceName = getProvince.Description;

         search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;

         if (page > 0 && search == null)
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.Districts
             .Include(d => d.Provinces)
             .Where(x => (x.Description.ToLower().Contains(search) || x.Provinces.Description.ToLower().Contains(search) || x.DistrictCode.ToLower().Contains(search) || search == null) && x.ProvinceId == oid && x.IsDeleted == false)
             .OrderBy(x => x.Provinces.Description);

         var districts = query.ToPagedList(pageNumber, pageSize);

         if (districts.PageCount > 0)
         {
            if (pageNumber > districts.PageCount)
               districts = query.ToPagedList(districts.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(districts);
      }
      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create(int oid)
      {
         try
         {
            var getProvince = await context.Provinces.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

            ViewBag.ProvinceId = getProvince.Oid;
            ViewBag.ProvinceName = getProvince.Description;
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(District district)
      {
         try
         {
            var districtInDb = IsDistrictDuplicate(district);

            if (!districtInDb)
            {
               if (ModelState.IsValid)
               {
                  district.CreatedBy = session?.GetCurrentAdmin().Oid;
                  district.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  district.DateCreated = DateTime.Now;
                  district.IsDeleted = false;
                  district.IsSynced = false;

                  context.Districts.Add(district);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  //return RedirectToAction("Create", new { oid = district.ProvinceId });
                  return RedirectToAction(nameof(Create), new { oid = district.ProvinceId, module = "Clients" });
               }

               return View(district);
            }
            else
            {
               var getProvince = await context.Provinces.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == district.ProvinceId);

               ViewBag.ProvinceId = getProvince.Oid;
               ViewBag.ProvinceName = getProvince.Description;

               ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
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

         var districtInDb = await context.Districts.Include(p => p.Provinces).AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (districtInDb == null)
            return NotFound();

         ViewBag.Provinces = new SelectList(context.Provinces, FieldConstants.Oid, FieldConstants.ProvinceName);

         ViewBag.ProvinceId = districtInDb.ProvinceId;

         return View(districtInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, District district)
      {
         try
         {
            var districtInDb = await context.Districts.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == district.Oid);

            bool isDistrictDuplicate = false;

            if (districtInDb.Description != district.Description)
               isDistrictDuplicate = IsDistrictDuplicate(district);

            if (!isDistrictDuplicate)
            {
               district.DateCreated = districtInDb.DateCreated;
               district.CreatedBy = districtInDb.CreatedBy;
               district.ModifiedBy = session?.GetCurrentAdmin().Oid;
               district.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               district.DateModified = DateTime.Now;
               district.IsDeleted = false;
               district.IsSynced = false;

               context.Districts.Update(district);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               ViewBag.ProvinceId = districtInDb.ProvinceId;

               return RedirectToAction(nameof(Create), new { module = "Clients", oid = district.ProvinceId });
            }
            else
            {
               ViewBag.Provinces = new SelectList(context.Provinces, FieldConstants.Oid, FieldConstants.ProvinceName);

               ViewBag.ProvinceId = districtInDb.ProvinceId;

               if (isDistrictDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(district);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var district = await context.Districts
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (district == null)
            return NotFound();

         return View(district);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var district = await context.Districts.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.Districts.Remove(district);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsDistrictDuplicate(District district)
      {
         try
         {
            var districtInDb = context.Districts.FirstOrDefault(c => c.Description.ToLower() == district.Description.ToLower() && c.IsDeleted == false);

            if (districtInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      [HttpGet]
      public async Task<IActionResult> districtByProvince(int ProvinceId)
      {
         return Json(await context.Districts.Where(d => d.IsDeleted == false && d.ProvinceId == ProvinceId).ToListAsync());
      }
      #endregion
   }
}