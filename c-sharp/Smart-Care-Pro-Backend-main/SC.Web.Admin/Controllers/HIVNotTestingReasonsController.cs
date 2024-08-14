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
   public class HIVNotTestingReasonsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public HIVNotTestingReasonsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.HIVNotTestingReasons
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(l => l.Description);

         var hivNotTestingReasons = query.ToPagedList(pageNumber, pageSize);

         if (hivNotTestingReasons.PageCount > 0)
         {
            if (pageNumber > hivNotTestingReasons.PageCount)
               hivNotTestingReasons = query.ToPagedList(hivNotTestingReasons.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(hivNotTestingReasons);
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
      public async Task<IActionResult> Create(HIVNotTestingReason hivNotTestingReason)
      {
         try
         {
            var hivNotTestingReasonInDb = IsNotTestingReasonDuplicate(hivNotTestingReason);

            if (!hivNotTestingReasonInDb)
            {
               if (ModelState.IsValid)
               {
                  hivNotTestingReason.CreatedBy = session?.GetCurrentAdmin().Oid;
                  hivNotTestingReason.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  hivNotTestingReason.DateCreated = DateTime.Now;
                  hivNotTestingReason.IsDeleted = false;
                  hivNotTestingReason.IsSynced = false;

                  context.HIVNotTestingReasons.Add(hivNotTestingReason);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = "HTS" });
               }

               return View(hivNotTestingReason);
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

         var hivNotTestingReasonInDb = await context.HIVNotTestingReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (hivNotTestingReasonInDb == null)
            return NotFound();

         return View(hivNotTestingReasonInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, HIVNotTestingReason hivNotTestingReason)
      {
         try
         {
            var hivNotTestingReasonInDb = await context.HIVNotTestingReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == hivNotTestingReason.Oid);

            bool isNotTestingReasonDuplicate = false;

            if (hivNotTestingReasonInDb.Description != hivNotTestingReason.Description)
               isNotTestingReasonDuplicate = IsNotTestingReasonDuplicate(hivNotTestingReason);

            if (!isNotTestingReasonDuplicate)
            {
               hivNotTestingReason.DateCreated = hivNotTestingReasonInDb.DateCreated;
               hivNotTestingReason.CreatedBy = hivNotTestingReasonInDb.CreatedBy;
               hivNotTestingReason.ModifiedBy = session?.GetCurrentAdmin().Oid;
               hivNotTestingReason.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               hivNotTestingReason.DateModified = DateTime.Now;
               hivNotTestingReason.IsDeleted = false;
               hivNotTestingReason.IsSynced = false;

               context.HIVNotTestingReasons.Update(hivNotTestingReason);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "HTS" });
            }
            else
            {
               if (isNotTestingReasonDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(hivNotTestingReason);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var hivNotTestingReason = await context.HIVNotTestingReasons
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (hivNotTestingReason == null)
            return NotFound();

         return View(hivNotTestingReason);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var hivNotTestingReason = await context.HIVNotTestingReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.HIVNotTestingReasons.Remove(hivNotTestingReason);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsNotTestingReasonDuplicate(HIVNotTestingReason hivNotTestingReason)
      {
         try
         {
            var notTestingReasonInDb = context.HIVNotTestingReasons.FirstOrDefault(c => c.Description.ToLower() == hivNotTestingReason.Description.ToLower() && c.IsDeleted == false);

            if (notTestingReasonInDb != null)
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