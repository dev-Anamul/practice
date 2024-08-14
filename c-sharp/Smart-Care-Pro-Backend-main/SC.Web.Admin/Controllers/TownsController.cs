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
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class TownsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public TownsController(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      #region Index

      public IActionResult Index(string? search, int? page)
      {
         search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;
         if (page > 0 && search == null)
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.Towns
            .Include(d => d.District)
            .ThenInclude(p => p.Provinces)
            .Where(x => x.Description.ToLower().Contains(search) || x.District.Description.ToLower().Contains(search) || x.District.Provinces.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.District.Provinces.Description);

         var towns = query.ToPagedList(pageNumber, pageSize);

         if (towns.PageCount > 0)
         {
            if (pageNumber > towns.PageCount)
               towns = query.ToPagedList(towns.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(towns);

      }

      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create()
      {
         try
         {
            ViewBag.Provinces = new SelectList(context.Provinces, FieldConstants.Oid, FieldConstants.ProvinceName);
            ViewBag.Districts = new SelectList(context.Districts, FieldConstants.Oid, FieldConstants.DistrictName);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(Town town)
      {
         try
         {
            var townInDb = IsTownDuplicate(town);

            if (!townInDb)
            {
               if (ModelState.IsValid)
               {
                  town.CreatedBy = session?.GetCurrentAdmin().Oid;
                  town.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  town.DateCreated = DateTime.Now;
                  town.IsDeleted = false;
                  town.IsSynced = false;

                  context.Towns.Add(town);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create));
               }

               return View(town);
            }
            else
            {
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

         var town = await context.Towns.Include(t => t.District).ThenInclude(d => d.Provinces).AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);
         var districts = context.Districts.ToList();

         if (town == null)
            return NotFound();

         ViewBag.Provinces = new SelectList(context.Provinces, FieldConstants.Oid, FieldConstants.ProvinceName, town.District.Provinces.Oid);
         ViewBag.Districts = new SelectList(districts.Where(d => d.ProvinceId == town.District.ProvinceId).ToList(), FieldConstants.Oid, FieldConstants.DistrictName, town.DistrictId);

         return View(town);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, Town town)
      {
         try
         {
            var townInDb = await context.Towns.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == town.Oid);

            bool isTownDuplicate = false;

            if (townInDb.Description != town.Description)
               isTownDuplicate = IsTownDuplicate(town);

            if (!isTownDuplicate)
            {
               town.DateCreated = townInDb.DateCreated;
               town.CreatedBy = townInDb.CreatedBy;
               town.ModifiedBy = session?.GetCurrentAdmin().Oid;
               town.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               town.DateModified = DateTime.Now;
               town.IsDeleted = false;
               town.IsSynced = false;

               context.Towns.Update(town);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index));
            }
            else
            {
               if (isTownDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(town);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var town = await context.Towns
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (town == null)
            return NotFound();

         return View(town);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var town = await context.Towns.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.Towns.Remove(town);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsTownDuplicate(Town town)
      {
         try
         {
            var townInDb = context.Towns.FirstOrDefault(c => c.Description.ToLower() == town.Description.ToLower() && c.IsDeleted == false);

            if (townInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      [HttpGet]
      public async Task<IActionResult> GetAllProvince()
      {
         return Json(await context.Provinces.ToListAsync());
      }

      [HttpGet]
      public async Task<IActionResult> ReadDistricts()
      {
         return Json(await context.Districts.ToListAsync());
      }

      public JsonResult ReadTownByDistrict(int id)
      {
         var townInDb = context.Towns.Where(t => t.DistrictId == id && t.IsDeleted == false).ToList();
         return Json(townInDb);
      }

      public JsonResult LoadDistrict(int id)
      {
         var town = context.Districts.Where(p => p.ProvinceId == id && p.IsDeleted == false).ToList();
         return Json(town);
      }
      #endregion
   }
}