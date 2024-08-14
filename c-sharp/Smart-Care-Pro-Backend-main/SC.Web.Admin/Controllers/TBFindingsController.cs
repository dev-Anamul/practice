using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 25.04.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class TBFindingsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public TBFindingsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.TBFindings
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var tBFindings = query.ToPagedList(pageNumber, pageSize);

         if (tBFindings.PageCount > 0)
         {
            if (pageNumber > tBFindings.PageCount)
               tBFindings = query.ToPagedList(tBFindings.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(tBFindings);
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
      public async Task<IActionResult> Create(TBFinding tbFindings, string? module, string? parent)
      {
         try
         {
            var tbFindingsInDb = IsTBFindingDuplicate(tbFindings);

            if (!tbFindingsInDb)
            {
               if (ModelState.IsValid)
               {
                  tbFindings.CreatedBy = session?.GetCurrentAdmin().Oid;
                  tbFindings.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  tbFindings.DateCreated = DateTime.Now;
                  tbFindings.IsDeleted = false;
                  tbFindings.IsSynced = false;

                  context.TBFindings.Add(tbFindings);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(tbFindings);
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

         var tbFindingsInDb = await context.TBFindings.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (tbFindingsInDb == null)
            return NotFound();

         return View(tbFindingsInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, TBFinding tbFindings, string? module, string? parent)
      {
         try
         {
            var tbFindingsInDb = await context.TBFindings.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == tbFindings.Oid);

            bool isTBFindingDuplicate = false;

            if (tbFindingsInDb.Description != tbFindings.Description)
               isTBFindingDuplicate = IsTBFindingDuplicate(tbFindings);

            if (!isTBFindingDuplicate)
            {
               tbFindings.DateCreated = tbFindingsInDb.DateCreated;
               tbFindings.CreatedBy = tbFindingsInDb.CreatedBy;
               tbFindings.ModifiedBy = session?.GetCurrentAdmin().Oid;
               tbFindings.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               tbFindings.DateModified = DateTime.Now;
               tbFindings.IsDeleted = false;
               tbFindings.IsSynced = false;

               context.TBFindings.Update(tbFindings);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isTBFindingDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(tbFindings);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var tbFindings = await context.TBFindings
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (tbFindings == null)
            return NotFound();

         return View(tbFindings);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var tbFindings = await context.TBFindings.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.TBFindings.Remove(tbFindings);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsTBFindingDuplicate(TBFinding tbFindings)
      {
         try
         {
            var tbFindingInDb = context.TBFindings.FirstOrDefault(c => c.Description.ToLower() == tbFindings.Description.ToLower() && c.IsDeleted == false);

            if (tbFindingInDb != null)
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