using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Brian
 * Date created : 06.05.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class PreferredFeedingsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public PreferredFeedingsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.PreferredFeedings
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false).OrderBy(x => x.Description);

         var preferredFeedings = query.ToPagedList(pageNumber, pageSize);

         if (preferredFeedings.PageCount > 0)
         {
            if (pageNumber > preferredFeedings.PageCount)
               preferredFeedings = query.ToPagedList(preferredFeedings.PageCount, pageSize);
         }

         ViewData["Search"] = search;
         return View(preferredFeedings);

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
      public async Task<IActionResult> Create(PreferredFeeding preferredFeeding, string? module, string? parent)
      {
         try
         {
            var familyPlanningClassInDb = IsPreferredFeedingDuplicate(preferredFeeding);

            if (!familyPlanningClassInDb)
            {
               if (ModelState.IsValid)
               {
                  preferredFeeding.CreatedBy = session?.GetCurrentAdmin().Oid;
                  preferredFeeding.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  preferredFeeding.DateCreated = DateTime.Now;
                  preferredFeeding.IsDeleted = false;
                  preferredFeeding.IsSynced = false;

                  context.PreferredFeedings.Add(preferredFeeding);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(preferredFeeding);
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

         var preferredFeeding = await context.PreferredFeedings.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (preferredFeeding == null)
            return NotFound();

         return View(preferredFeeding);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, PreferredFeeding preferredFeeding, string? module, string? parent)
      {
         try
         {
            var preferredFeedingModel = await context.PreferredFeedings.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == preferredFeeding.Oid);

            bool isPreferredFeedingDuplicate = false;

            if (preferredFeeding.Description != preferredFeeding.Description)
               isPreferredFeedingDuplicate = IsPreferredFeedingDuplicate(preferredFeeding);

            if (!isPreferredFeedingDuplicate)
            {
               preferredFeeding.DateCreated = preferredFeedingModel.DateCreated;
               preferredFeeding.CreatedBy = preferredFeedingModel.CreatedBy;
               preferredFeeding.ModifiedBy = session?.GetCurrentAdmin().Oid;
               preferredFeeding.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               preferredFeeding.DateModified = DateTime.Now;
               preferredFeeding.IsDeleted = false;
               preferredFeeding.IsSynced = false;

               context.PreferredFeedings.Update(preferredFeeding);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isPreferredFeedingDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(preferredFeeding);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var preferredFeeding = await context.PreferredFeedings
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (preferredFeeding == null)
            return NotFound();

         return View(preferredFeeding);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var preferredFeeding = await context.PreferredFeedings.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.PreferredFeedings.Remove(preferredFeeding);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsPreferredFeedingDuplicate(PreferredFeeding preferredFeeding)
      {
         try
         {
            var preferredFeedingInDb = context.PreferredFeedings.FirstOrDefault(c => c.Description.ToLower() == preferredFeeding.Description.ToLower() && c.IsDeleted == false);

            if (preferredFeedingInDb != null)
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