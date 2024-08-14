using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 06.03.2023
 * Modified by  : Bella
 * Last modified: 09.03.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class ProvincesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ProvincesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.Provinces
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var provinces = query.ToPagedList(pageNumber, pageSize);

         if (provinces.PageCount > 0)
         {
            if (pageNumber > provinces.PageCount)
               provinces = query.ToPagedList(provinces.PageCount, pageSize);
         }

         ViewData["Search"] = search;
         return View(provinces);
      }
      #endregion

      #region Create
      [HttpGet]
      public IActionResult Create()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(Province province)
      {
         try
         {
            var provinceInDb = IsProvinceDuplicate(province);

            if (!provinceInDb)
            {
               if (ModelState.IsValid)
               {
                  province.CreatedBy = session?.GetCurrentAdmin().Oid;
                  province.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  province.DateCreated = DateTime.Now;
                  province.IsDeleted = false;
                  province.IsSynced = false;

                  context.Provinces.Add(province);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = "Clients" });
               }

               return View(province);
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

         var provinceInDb = await context.Provinces.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (provinceInDb == null)
            return NotFound();

         return View(provinceInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, Province province)
      {
         try
         {
            var provinceInDb = await context.Provinces.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == province.Oid);

            bool isProvinceDuplicate = false;

            if (provinceInDb.Description != province.Description)
               isProvinceDuplicate = IsProvinceDuplicate(province);

            if (!isProvinceDuplicate)
            {
               province.DateCreated = provinceInDb.DateCreated;
               province.CreatedBy = provinceInDb.CreatedBy;
               province.ModifiedBy = session?.GetCurrentAdmin().Oid;
               province.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               province.DateModified = DateTime.Now;
               province.IsDeleted = false;
               province.IsSynced = false;

               context.Provinces.Update(province);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Clients" });
            }
            else
            {
               if (isProvinceDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(province);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var province = await context.Provinces
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (province == null)
            return NotFound();

         return View(province);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var province = await context.Provinces.FindAsync(id);

         if (province == null)
            return NotFound();

         var districts = await context.Districts.Where(d => d.ProvinceId == id).ToListAsync();

         if (districts.Any())
         {
            ViewData["ErrorMessage"] = "Cannot delete province because it has associated districts.";
            return View(province);
         }

         province.IsDeleted = true;

         context.Provinces.Update(province);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsProvinceDuplicate(Province province)
      {
         try
         {
            var provinceInDb = context.Provinces.FirstOrDefault(c => c.Description.ToLower() == province.Description.ToLower() && c.IsDeleted == false);

            if (provinceInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      [HttpGet]
      public async Task<IActionResult> GetAllProvinces()
      {
         return Json(await context.Provinces.ToListAsync());
      }
      #endregion
   }
}