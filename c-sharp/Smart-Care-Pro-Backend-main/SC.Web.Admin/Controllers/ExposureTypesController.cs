using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 22.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class ExposureTypesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ExposureTypesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.ExposureTypes
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var exposureTypes = query.ToPagedList(pageNumber, pageSize);

         if (exposureTypes.PageCount > 0)
         {
            if (pageNumber > exposureTypes.PageCount)
               exposureTypes = query.ToPagedList(exposureTypes.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(exposureTypes);
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
      public async Task<IActionResult> Create(ExposureType exposureType, string? module, string? parent)
      {
         try
         {
            var exposureTypeInDb = IsExposureTypeDuplicate(exposureType);

            if (!exposureTypeInDb)
            {
               if (ModelState.IsValid)
               {
                  exposureType.CreatedBy = session?.GetCurrentAdmin().Oid;
                  exposureType.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  exposureType.DateCreated = DateTime.Now;
                  exposureType.IsDeleted = false;
                  exposureType.IsSynced = false;

                  context.ExposureTypes.Add(exposureType);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(exposureType);
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

         var exposureTypeInDb = await context.ExposureTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (exposureTypeInDb == null)
            return NotFound();

         return View(exposureTypeInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ExposureType exposureType, string? module, string? parent)
      {
         try
         {
            var exposureTypeInDb = await context.ExposureTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == exposureType.Oid);

            bool isExposureTypeDuplicate = false;

            if (exposureTypeInDb.Description != exposureType.Description)
               isExposureTypeDuplicate = IsExposureTypeDuplicate(exposureType);

            if (!isExposureTypeDuplicate)
            {
               exposureType.DateCreated = exposureTypeInDb.DateCreated;
               exposureType.CreatedBy = exposureTypeInDb.CreatedBy;
               exposureType.ModifiedBy = session?.GetCurrentAdmin().Oid;
               exposureType.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               exposureType.DateModified = DateTime.Now;
               exposureType.IsDeleted = false;
               exposureType.IsSynced = false;

               context.ExposureTypes.Update(exposureType);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isExposureTypeDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(exposureType);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var exposureType = await context.ExposureTypes
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (exposureType == null)
            return NotFound();

         return View(exposureType);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var exposureType = await context.ExposureTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.ExposureTypes.Remove(exposureType);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsExposureTypeDuplicate(ExposureType exposureType)
      {
         try
         {
            var exposureTypeInDb = context.ExposureTypes.FirstOrDefault(c => c.Description.ToLower() == exposureType.Description.ToLower() && c.IsDeleted == false);

            if (exposureTypeInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }
      #endregion
   }
}