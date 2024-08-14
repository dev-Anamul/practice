using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 26.04.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class TBSuspectingReasonsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public TBSuspectingReasonsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.TBSuspectingReasons
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var tBSuspectingReasons = query.ToPagedList(pageNumber, pageSize);

         if (tBSuspectingReasons.PageCount > 0)
         {
            if (pageNumber > tBSuspectingReasons.PageCount)
               tBSuspectingReasons = query.ToPagedList(tBSuspectingReasons.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(tBSuspectingReasons);
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
      public async Task<IActionResult> Create(TBSuspectingReason tbSuspectingReason, string? module, string? parent)
      {
         try
         {
            var tbSuspectingReasonInDb = IsTBSuspectingReasonDuplicate(tbSuspectingReason);

            if (!tbSuspectingReasonInDb)
            {
               if (ModelState.IsValid)
               {
                  tbSuspectingReason.CreatedBy = session?.GetCurrentAdmin().Oid;
                  tbSuspectingReason.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  tbSuspectingReason.DateCreated = DateTime.Now;
                  tbSuspectingReason.IsDeleted = false;
                  tbSuspectingReason.IsSynced = false;

                  context.TBSuspectingReasons.Add(tbSuspectingReason);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(tbSuspectingReason);
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

         var tbSuspectingReasonInDb = await context.TBSuspectingReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (tbSuspectingReasonInDb == null)
            return NotFound();

         return View(tbSuspectingReasonInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, TBSuspectingReason tbSuspectingReason, string? module, string? parent)
      {
         try
         {
            var tbSuspectingReasonInDb = await context.TBSuspectingReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == tbSuspectingReason.Oid);

            bool isTBSuspectingReasonDuplicate = false;

            if (tbSuspectingReasonInDb.Description != tbSuspectingReason.Description)
               isTBSuspectingReasonDuplicate = IsTBSuspectingReasonDuplicate(tbSuspectingReason);

            if (!isTBSuspectingReasonDuplicate)
            {
               tbSuspectingReason.DateCreated = tbSuspectingReasonInDb.DateCreated;
               tbSuspectingReason.CreatedBy = tbSuspectingReasonInDb.CreatedBy;
               tbSuspectingReason.ModifiedBy = session?.GetCurrentAdmin().Oid;
               tbSuspectingReason.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               tbSuspectingReason.DateModified = DateTime.Now;
               tbSuspectingReason.IsDeleted = false;
               tbSuspectingReason.IsSynced = false;

               context.TBSuspectingReasons.Update(tbSuspectingReason);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isTBSuspectingReasonDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(tbSuspectingReason);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var tbSuspectingReason = await context.TBSuspectingReasons
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (tbSuspectingReason == null)
            return NotFound();

         return View(tbSuspectingReason);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var tbSuspectingReason = await context.TBSuspectingReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.TBSuspectingReasons.Remove(tbSuspectingReason);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsTBSuspectingReasonDuplicate(TBSuspectingReason tbSuspectingReason)
      {
         try
         {
            var tbSuspectingReasonInDb = context.TBSuspectingReasons.FirstOrDefault(c => c.Description.ToLower() == tbSuspectingReason.Description.ToLower() && c.IsDeleted == false);

            if (tbSuspectingReasonInDb != null)
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