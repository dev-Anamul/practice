using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Brian
 * Date created : 05.04.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class TBIdentificationMethodsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public TBIdentificationMethodsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.TBIdentificationMethods
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var tBIdentificationMethods = query.ToPagedList(pageNumber, pageSize);

         if (tBIdentificationMethods.PageCount > 0)
         {
            if (pageNumber > tBIdentificationMethods.PageCount)
               tBIdentificationMethods = query.ToPagedList(tBIdentificationMethods.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(tBIdentificationMethods);
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
      public async Task<IActionResult> Create(TBIdentificationMethod tbIdentificationMethod, string? module, string? parent)
      {
         try
         {
            var tbIdentificationMethodInDb = IsTBIdentificationMethodDuplicate(tbIdentificationMethod);

            if (!tbIdentificationMethodInDb)
            {
               if (ModelState.IsValid)
               {
                  tbIdentificationMethod.CreatedBy = session?.GetCurrentAdmin().Oid;
                  tbIdentificationMethod.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  tbIdentificationMethod.DateCreated = DateTime.Now;
                  tbIdentificationMethod.IsDeleted = false;
                  tbIdentificationMethod.IsSynced = false;

                  context.TBIdentificationMethods.Add(tbIdentificationMethod);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(tbIdentificationMethod);
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

         var tbIdentificationMethodInDb = await context.TBIdentificationMethods.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (tbIdentificationMethodInDb == null)
            return NotFound();

         return View(tbIdentificationMethodInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, TBIdentificationMethod tBIdentificationMethod, string? module, string? parent)
      {
         try
         {
            var tbIdentificationMethodInDb = await context.TBIdentificationMethods.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == tBIdentificationMethod.Oid);

            bool isTBIdentificationMethodDuplicate = false;

            if (tbIdentificationMethodInDb.Description != tBIdentificationMethod.Description)
               isTBIdentificationMethodDuplicate = IsTBIdentificationMethodDuplicate(tBIdentificationMethod);

            if (!isTBIdentificationMethodDuplicate)
            {
               tBIdentificationMethod.DateCreated = tbIdentificationMethodInDb.DateCreated;
               tBIdentificationMethod.CreatedBy = tbIdentificationMethodInDb.CreatedBy;
               tBIdentificationMethod.ModifiedBy = session?.GetCurrentAdmin().Oid;
               tBIdentificationMethod.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               tBIdentificationMethod.DateModified = DateTime.Now;
               tBIdentificationMethod.IsDeleted = false;
               tBIdentificationMethod.IsSynced = false;

               context.TBIdentificationMethods.Update(tBIdentificationMethod);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isTBIdentificationMethodDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(tBIdentificationMethod);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var tBIdentificationMethod = await context.TBIdentificationMethods
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (tBIdentificationMethod == null)
            return NotFound();

         return View(tBIdentificationMethod);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var tBIdentificationMethod = await context.TBIdentificationMethods.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.TBIdentificationMethods.Remove(tBIdentificationMethod);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsTBIdentificationMethodDuplicate(TBIdentificationMethod tBIdentificationMethod)
      {
         try
         {
            var tbIdentificationMethodInDb = context.TBIdentificationMethods.FirstOrDefault(c => c.Description.ToLower() == tBIdentificationMethod.Description.ToLower() && c.IsDeleted == false);

            if (tbIdentificationMethodInDb != null)
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