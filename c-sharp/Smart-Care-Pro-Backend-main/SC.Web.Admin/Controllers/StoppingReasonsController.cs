using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 27.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class StoppingReasonsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public StoppingReasonsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.StoppingReasons
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var stoppingReasons = query.ToPagedList(pageNumber, pageSize);

         if (stoppingReasons.PageCount > 0)
         {
            if (pageNumber > stoppingReasons.PageCount)
               stoppingReasons = query.ToPagedList(stoppingReasons.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(stoppingReasons);
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
      public async Task<IActionResult> Create(StoppingReason stoppingReason, string? module, string? parent)
      {
         try
         {
            var stoppingReasonInDb = IsStoppingReasonDuplicate(stoppingReason);

            if (!stoppingReasonInDb)
            {
               if (ModelState.IsValid)
               {
                  stoppingReason.CreatedBy = session?.GetCurrentAdmin().Oid;
                  stoppingReason.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  stoppingReason.DateCreated = DateTime.Now;
                  stoppingReason.IsDeleted = false;
                  stoppingReason.IsSynced = false;

                  context.StoppingReasons.Add(stoppingReason);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(stoppingReason);
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

         var stoppingReasonInDb = await context.StoppingReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (stoppingReasonInDb == null)
            return NotFound();

         return View(stoppingReasonInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, StoppingReason stoppingReason, string? module, string? parent)
      {
         try
         {
            var stoppingReasonInDb = await context.StoppingReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == stoppingReason.Oid);

            bool isReasonDuplicate = false;

            if (stoppingReasonInDb.Description != stoppingReason.Description)
               isReasonDuplicate = IsStoppingReasonDuplicate(stoppingReason);

            if (!isReasonDuplicate)
            {
               stoppingReason.DateCreated = stoppingReasonInDb.DateCreated;
               stoppingReason.CreatedBy = stoppingReasonInDb.CreatedBy;
               stoppingReason.ModifiedBy = session?.GetCurrentAdmin().Oid;
               stoppingReason.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               stoppingReason.DateModified = DateTime.Now;
               stoppingReason.IsDeleted = false;
               stoppingReason.IsSynced = false;

               context.StoppingReasons.Update(stoppingReason);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isReasonDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(stoppingReason);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var prepStoppingReason = await context.StoppingReasons
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (prepStoppingReason == null)
            return NotFound();

         return View(prepStoppingReason);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var prepStoppingReason = await context.StoppingReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.StoppingReasons.Remove(prepStoppingReason);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsStoppingReasonDuplicate(StoppingReason stoppingReason)
      {
         try
         {
            var stoppingReasonInDb = context.StoppingReasons.FirstOrDefault(c => c.Description.ToLower() == stoppingReason.Description.ToLower() && c.IsDeleted == false);

            if (stoppingReasonInDb != null)
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