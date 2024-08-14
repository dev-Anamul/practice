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
   public class AllergiesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public AllergiesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.Allergies
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var allergies = query.ToPagedList(pageNumber, pageSize);

         if (allergies.PageCount > 0)
         {
            if (pageNumber > allergies.PageCount)
               allergies = query.ToPagedList(allergies.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(allergies);
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
      public async Task<IActionResult> Create(Allergy allergies, string? module, string? parent)
      {
         try
         {
            var allergyInDb = IsAllergiesDuplicate(allergies);

            if (!allergyInDb)
            {
               if (ModelState.IsValid)
               {
                  allergies.CreatedBy = session?.GetCurrentAdmin().Oid;
                  allergies.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  allergies.DateCreated = DateTime.Now;
                  allergies.IsDeleted = false;
                  allergies.IsSynced = false;

                  context.Allergies.Add(allergies);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  //return RedirectToAction(nameof(Create));
                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(allergies);
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

         var allergies = await context.Allergies.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (allergies == null)
            return NotFound();

         return View(allergies);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, Allergy allergies, string? module, string? parent)
      {
         try
         {
            var allergyInDb = await context.Allergies.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == allergies.Oid);

            bool isAllergiesDuplicate = false;

            if (allergyInDb.Description != allergies.Description)
               isAllergiesDuplicate = IsAllergiesDuplicate(allergies);

            if (!isAllergiesDuplicate)
            {
               allergies.DateCreated = allergyInDb.DateCreated;
               allergies.CreatedBy = allergyInDb.CreatedBy;
               allergies.ModifiedBy = session?.GetCurrentAdmin().Oid;
               allergies.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               allergies.DateModified = DateTime.Now;
               allergies.IsDeleted = false;
               allergies.IsSynced = false;

               context.Allergies.Update(allergies);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               //return RedirectToAction(nameof(Index));
               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isAllergiesDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(allergies);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var allergies = await context.Allergies
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (allergies == null)
            return NotFound();

         return View(allergies);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var allergies = await context.Allergies.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.Allergies.Remove(allergies);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsAllergiesDuplicate(Allergy allergies)
      {
         try
         {
            var allergiesInDb = context.Allergies.FirstOrDefault(c => c.Description.ToLower() == allergies.Description.ToLower() && c.IsDeleted == false);

            if (allergiesInDb != null)
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