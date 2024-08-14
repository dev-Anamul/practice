using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Brian
 * Date created : 07.05.2023
 * Modified by  : Bella
 * Last modified: 22.05.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class GynConfirmationsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public GynConfirmationsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.GynConfirmations
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(l => l.Description);

         var gynConfirmations = query.ToPagedList(pageNumber, pageSize);

         if (gynConfirmations.PageCount > 0)
         {
            if (pageNumber > gynConfirmations.PageCount)
               gynConfirmations = query.ToPagedList(gynConfirmations.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(gynConfirmations);
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
      public async Task<IActionResult> Create(GynConfirmation gynConfirmation, string? module, string? parent)
      {
         try
         {
            var gynConfirmationInDb = IsGynConfirmationDuplicate(gynConfirmation);

            if (!gynConfirmationInDb)
            {
               if (ModelState.IsValid)
               {
                  gynConfirmation.CreatedBy = session?.GetCurrentAdmin().Oid;
                  gynConfirmation.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  gynConfirmation.DateCreated = DateTime.Now;
                  gynConfirmation.IsDeleted = false;
                  gynConfirmation.IsSynced = false;

                  context.GynConfirmations.Add(gynConfirmation);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(gynConfirmation);
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

         var gynConfirmationInDb = await context.GynConfirmations.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (gynConfirmationInDb == null)
            return NotFound();

         return View(gynConfirmationInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, GynConfirmation gynConfirmation, string? module, string? parent)
      {
         try
         {
            var gynConfirmationInDb = await context.GynConfirmations.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == gynConfirmation.Oid);

            bool isGynConfirmationDuplicate = false;

            if (gynConfirmationInDb.Description != gynConfirmation.Description)
               isGynConfirmationDuplicate = IsGynConfirmationDuplicate(gynConfirmation);

            if (!isGynConfirmationDuplicate)
            {
               gynConfirmation.DateCreated = gynConfirmationInDb.DateCreated;
               gynConfirmation.CreatedBy = gynConfirmationInDb.CreatedBy;
               gynConfirmation.ModifiedBy = session?.GetCurrentAdmin().Oid;
               gynConfirmation.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               gynConfirmation.DateModified = DateTime.Now;
               gynConfirmation.IsDeleted = false;
               gynConfirmation.IsSynced = false;

               context.GynConfirmations.Update(gynConfirmation);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isGynConfirmationDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(gynConfirmation);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var gynConfirmation = await context.GynConfirmations
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (gynConfirmation == null)
            return NotFound();

         return View(gynConfirmation);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var gynConfirmation = await context.GynConfirmations.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.GynConfirmations.Remove(gynConfirmation);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsGynConfirmationDuplicate(GynConfirmation gynConfirmation)
      {
         try
         {
            var gynConfirmationInDb = context.GynConfirmations.FirstOrDefault(c => c.Description.ToLower() == gynConfirmation.Description.ToLower() && c.IsDeleted == false);

            if (gynConfirmationInDb != null)
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