using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 07.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class PainScalesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public PainScalesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.PainScales
            .Where(p => p.Description.ToLower().Contains(search) || search == null && p.IsDeleted == false)
            .OrderBy(x => x.Description);

         var painScales = query.ToPagedList(pageNumber, pageSize);

         if (painScales.PageCount > 0)
         {
            if (pageNumber > painScales.PageCount)
               painScales = query.ToPagedList(painScales.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(painScales);
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
      public async Task<IActionResult> Create(PainScale painScale)
      {
         try
         {
            var painScaleInDb = IsPainScaleDuplicate(painScale);

            if (!painScaleInDb)
            {
               if (ModelState.IsValid)
               {
                  painScale.CreatedBy = session?.GetCurrentAdmin().Oid;
                  painScale.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  painScale.DateCreated = DateTime.Now;
                  painScale.IsDeleted = false;
                  painScale.IsSynced = false;

                  context.PainScales.Add(painScale);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create));
               }

               return View(painScale);
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

         var painScaleInDb = await context.PainScales.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (painScaleInDb == null)
            return NotFound();

         return View(painScaleInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, PainScale painScale)
      {
         try
         {
            var painScaleInDb = await context.PainScales.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == painScale.Oid);

            bool isPainScaleDuplicate = false;

            if (painScaleInDb.Description != painScale.Description)
               isPainScaleDuplicate = IsPainScaleDuplicate(painScale);

            if (!isPainScaleDuplicate)
            {
               painScale.DateCreated = painScaleInDb.DateCreated;
               painScale.CreatedBy = painScaleInDb.CreatedBy;
               painScale.ModifiedBy = session?.GetCurrentAdmin().Oid;
               painScale.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               painScale.DateModified = DateTime.Now;
               painScale.IsDeleted = false;
               painScale.IsSynced = false;

               context.PainScales.Update(painScale);

               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index));
            }
            else
            {
               if (isPainScaleDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(painScale);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var painScale = await context.PainScales
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (painScale == null)
            return NotFound();

         return View(painScale);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var painScale = await context.PainScales.FindAsync(id);

         context.PainScales.Remove(painScale);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsPainScaleDuplicate(PainScale painScale)
      {
         try
         {
            var painScaleInDb = context.PainScales.FirstOrDefault(c => c.Description.ToLower() == painScale.Description.ToLower() && c.IsDeleted == false);

            if (painScaleInDb != null)
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