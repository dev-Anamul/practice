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
   public class CaCxScreeningMethodsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public CaCxScreeningMethodsController(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      #region Index
      public IActionResult Index(string search, int? page)
      {
         search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;

         if (page > 0 && search == null)
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.CaCxScreeningMethods
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var caCxScreeningMethods = query.ToPagedList(pageNumber, pageSize);

         if (caCxScreeningMethods.PageCount > 0)
         {
            if (pageNumber > caCxScreeningMethods.PageCount)
               caCxScreeningMethods = query.ToPagedList(caCxScreeningMethods.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(caCxScreeningMethods);
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
      public async Task<IActionResult> Create(CaCxScreeningMethod caCxScreeningMethod, string? module, string? parent)
      {
         try
         {
            var caCxScreeningMethodInDb = IsScreeningMethodDuplicate(caCxScreeningMethod);

            if (!caCxScreeningMethodInDb)
            {
               if (ModelState.IsValid)
               {
                  caCxScreeningMethod.CreatedBy = session?.GetCurrentAdmin().Oid;
                  caCxScreeningMethod.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  caCxScreeningMethod.DateCreated = DateTime.Now;
                  caCxScreeningMethod.IsDeleted = false;
                  caCxScreeningMethod.IsSynced = false;

                  context.CaCxScreeningMethods.Add(caCxScreeningMethod);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(caCxScreeningMethod);
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

         var caCxScreeningMethod = await context.CaCxScreeningMethods.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (caCxScreeningMethod == null)
            return NotFound();

         return View(caCxScreeningMethod);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, CaCxScreeningMethod caCxScreeningMethod, string? module, string? parent)
      {
         try
         {
            var caCxScreeningMethodInDb = await context.CaCxScreeningMethods.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == caCxScreeningMethod.Oid);

            bool isScreeningMethodDuplicate = false;

            if (caCxScreeningMethodInDb.Description != caCxScreeningMethod.Description)
               isScreeningMethodDuplicate = IsScreeningMethodDuplicate(caCxScreeningMethod);

            if (!isScreeningMethodDuplicate)
            {
               caCxScreeningMethod.DateCreated = caCxScreeningMethodInDb.DateCreated;
               caCxScreeningMethod.CreatedBy = caCxScreeningMethodInDb.CreatedBy;
               caCxScreeningMethod.ModifiedBy = session?.GetCurrentAdmin().Oid;
               caCxScreeningMethod.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               caCxScreeningMethod.DateModified = DateTime.Now;
               caCxScreeningMethod.IsDeleted = false;
               caCxScreeningMethod.IsSynced = false;

               context.CaCxScreeningMethods.Update(caCxScreeningMethod);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isScreeningMethodDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(caCxScreeningMethod);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var caCxScreeningMethod = await context.CaCxScreeningMethods
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (caCxScreeningMethod == null)
            return NotFound();

         return View(caCxScreeningMethod);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var caCxScreeningMethod = await context.CaCxScreeningMethods.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.CaCxScreeningMethods.Remove(caCxScreeningMethod);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsScreeningMethodDuplicate(CaCxScreeningMethod caCxScreeningMethod)
      {
         try
         {
            var screeningMethodInDb = context.CaCxScreeningMethods.FirstOrDefault(c => c.Description.ToLower() == caCxScreeningMethod.Description.ToLower() && c.IsDeleted == false);

            if (screeningMethodInDb != null)
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