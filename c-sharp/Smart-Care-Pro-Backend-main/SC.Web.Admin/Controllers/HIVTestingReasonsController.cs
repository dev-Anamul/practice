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
   public class HIVTestingReasonsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public HIVTestingReasonsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.HIVTestingReasons
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(l => l.Description);

         var hivTestingReasons = query.ToPagedList(pageNumber, pageSize);

         if (hivTestingReasons.PageCount > 0)
         {
            if (pageNumber > hivTestingReasons.PageCount)
               hivTestingReasons = query.ToPagedList(hivTestingReasons.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(hivTestingReasons);
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
      public async Task<IActionResult> Create(HIVTestingReason hivTestingReason)
      {
         try
         {
            var hivTestingReasonInDb = IsTestingReasonDuplicate(hivTestingReason);

            if (!hivTestingReasonInDb)
            {
               if (ModelState.IsValid)
               {
                  hivTestingReason.CreatedBy = session?.GetCurrentAdmin().Oid;
                  hivTestingReason.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  hivTestingReason.DateCreated = DateTime.Now;
                  hivTestingReason.IsDeleted = false;
                  hivTestingReason.IsSynced = false;

                  context.HIVTestingReasons.Add(hivTestingReason);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = "HTS" });
               }

               return View(hivTestingReason);
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

         var hivTestingReasonInDb = await context.HIVTestingReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (hivTestingReasonInDb == null)
            return NotFound();

         return View(hivTestingReasonInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, HIVTestingReason hivTestingReason)
      {
         try
         {
            var hivTestingReasonInDb = await context.HIVTestingReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == hivTestingReason.Oid);

            bool isTestingReasonDuplicate = false;

            if (hivTestingReasonInDb.Description != hivTestingReason.Description)
               isTestingReasonDuplicate = IsTestingReasonDuplicate(hivTestingReason);

            if (!isTestingReasonDuplicate)
            {
               hivTestingReason.DateCreated = hivTestingReasonInDb.DateCreated;
               hivTestingReason.CreatedBy = hivTestingReasonInDb.CreatedBy;
               hivTestingReason.ModifiedBy = session?.GetCurrentAdmin().Oid;
               hivTestingReason.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               hivTestingReason.DateModified = DateTime.Now;
               hivTestingReason.IsDeleted = false;
               hivTestingReason.IsSynced = false;

               context.HIVTestingReasons.Update(hivTestingReason);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "HTS" });
            }
            else
            {
               if (isTestingReasonDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(hivTestingReason);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var hivTestingReason = await context.HIVTestingReasons
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (hivTestingReason == null)
            return NotFound();

         return View(hivTestingReason);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var hivTestingReason = await context.HIVTestingReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.HIVTestingReasons.Remove(hivTestingReason);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsTestingReasonDuplicate(HIVTestingReason hivTestingReason)
      {
         try
         {
            var hivTestingReasonInDb = context.HIVTestingReasons.FirstOrDefault(c => c.Description.ToLower() == hivTestingReason.Description.ToLower() && c.IsDeleted == false);

            if (hivTestingReasonInDb != null)
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