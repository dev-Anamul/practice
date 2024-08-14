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
   public class HIVRiskFactorsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public HIVRiskFactorsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.HIVRiskFactors
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(l => l.Description);

         var hivRiskFactors = query.ToPagedList(pageNumber, pageSize);

         if (hivRiskFactors.PageCount > 0)
         {
            if (pageNumber > hivRiskFactors.PageCount)
               hivRiskFactors = query.ToPagedList(hivRiskFactors.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(hivRiskFactors);
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
      public async Task<IActionResult> Create(HIVRiskFactor hivRiskFactor)
      {
         try
         {
            var hivRiskFactorInDb = IsRiskFactorDuplicate(hivRiskFactor);

            if (!hivRiskFactorInDb)
            {
               if (ModelState.IsValid)
               {
                  hivRiskFactor.CreatedBy = session?.GetCurrentAdmin().Oid;
                  hivRiskFactor.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  hivRiskFactor.DateCreated = DateTime.Now;
                  hivRiskFactor.IsDeleted = false;
                  hivRiskFactor.IsSynced = false;

                  context.HIVRiskFactors.Add(hivRiskFactor);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = "HTS" });
               }

               return View(hivRiskFactor);
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

         var hivRiskFactorInDb = await context.HIVRiskFactors.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (hivRiskFactorInDb == null)
            return NotFound();

         return View(hivRiskFactorInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, HIVRiskFactor hivRiskFactor)
      {
         try
         {
            var hivRiskFactorInDb = await context.HIVRiskFactors.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == hivRiskFactor.Oid);

            bool isRiskFactorDuplicate = false;

            if (hivRiskFactorInDb.Description != hivRiskFactor.Description)
               isRiskFactorDuplicate = IsRiskFactorDuplicate(hivRiskFactor);

            if (!isRiskFactorDuplicate)
            {
               hivRiskFactor.DateCreated = hivRiskFactorInDb.DateCreated;
               hivRiskFactor.CreatedBy = hivRiskFactorInDb.CreatedBy;
               hivRiskFactor.ModifiedBy = session?.GetCurrentAdmin().Oid;
               hivRiskFactor.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               hivRiskFactor.DateModified = DateTime.Now;
               hivRiskFactor.IsDeleted = false;
               hivRiskFactor.IsSynced = false;

               context.HIVRiskFactors.Update(hivRiskFactor);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "HTS" });
            }
            else
            {
               if (isRiskFactorDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(hivRiskFactor);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var hivRiskFactor = await context.HIVRiskFactors
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (hivRiskFactor == null)
            return NotFound();

         return View(hivRiskFactor);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var hivRiskFactor = await context.HIVRiskFactors.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.HIVRiskFactors.Remove(hivRiskFactor);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsRiskFactorDuplicate(HIVRiskFactor hivRiskFactor)
      {
         try
         {
            var riskFactorInDb = context.HIVRiskFactors.FirstOrDefault(c => c.Description.ToLower() == hivRiskFactor.Description.ToLower() && c.IsDeleted == false);

            if (riskFactorInDb != null)
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