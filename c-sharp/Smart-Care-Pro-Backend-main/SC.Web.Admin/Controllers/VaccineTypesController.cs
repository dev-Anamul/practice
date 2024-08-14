using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 21.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class VaccineTypesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public VaccineTypesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.VaccineTypes
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var vaccineTypes = query.ToPagedList(pageNumber, pageSize);

         if (vaccineTypes.PageCount > 0)
         {
            if (pageNumber > vaccineTypes.PageCount)
               vaccineTypes = query.ToPagedList(vaccineTypes.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(vaccineTypes);
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
      public async Task<IActionResult> Create(VaccineType vaccines, string? module, string? parent)
      {
         try
         {
            var vaccineTypeInDb = IsVaccineTypeDuplicate(vaccines);

            if (!vaccineTypeInDb)
            {
               if (ModelState.IsValid)
               {
                  vaccines.CreatedBy = session?.GetCurrentAdmin().Oid;
                  vaccines.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  vaccines.DateCreated = DateTime.Now;
                  vaccines.IsDeleted = false;
                  vaccines.IsSynced = false;

                  context.VaccineTypes.Add(vaccines);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Index), new { module = module, parent = parent });
               }

               return View(vaccines);
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

         var vaccinesInDb = await context.VaccineTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (vaccinesInDb == null)
            return NotFound();

         return View(vaccinesInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, VaccineType vaccines, string? module, string? parent)
      {
         try
         {
            var vaccineTypeInDb = await context.VaccineTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == vaccines.Oid);

            bool isVaccineTypeDuplicate = false;

            if (vaccineTypeInDb.Description != vaccines.Description)
               isVaccineTypeDuplicate = IsVaccineTypeDuplicate(vaccines);

            if (!isVaccineTypeDuplicate)
            {
               vaccines.DateCreated = vaccineTypeInDb.DateCreated;
               vaccines.CreatedBy = vaccineTypeInDb.CreatedBy;
               vaccines.ModifiedBy = session?.GetCurrentAdmin().Oid;
               vaccines.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               vaccines.DateModified = DateTime.Now;
               vaccines.IsDeleted = false;
               vaccines.IsSynced = false;

               context.VaccineTypes.Update(vaccines);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isVaccineTypeDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(vaccines);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var vaccineTypes = await context.VaccineTypes
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (vaccineTypes == null)
            return NotFound();

         return View(vaccineTypes);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var vaccineTypes = await context.VaccineTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.VaccineTypes.Remove(vaccineTypes);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsVaccineTypeDuplicate(VaccineType vaccineType)
      {
         try
         {
            var vaccineTypeInDb = context.VaccineTypes.FirstOrDefault(c => c.Description.ToLower() == vaccineType.Description.ToLower() && c.IsDeleted == false);

            if (vaccineTypeInDb != null)
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