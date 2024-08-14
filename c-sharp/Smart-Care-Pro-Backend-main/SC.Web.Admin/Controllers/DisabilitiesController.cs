using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 22.04.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class DisabilitiesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public DisabilitiesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.Disabilities
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var disabilities = query.ToPagedList(pageNumber, pageSize);

         if (disabilities.PageCount > 0)
         {
            if (pageNumber > disabilities.PageCount)
               disabilities = query.ToPagedList(disabilities.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(disabilities);
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
      public async Task<IActionResult> Create(Disability disability, string? module, string? parent)
      {
         try
         {
            var disabilityInDb = IsDisabilitiesDuplicate(disability);

            if (!disabilityInDb)
            {
               if (ModelState.IsValid)
               {
                  disability.CreatedBy = session?.GetCurrentAdmin().Oid;
                  disability.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  disability.DateCreated = DateTime.Now;
                  disability.IsDeleted = false;
                  disability.IsSynced = false;

                  context.Disabilities.Add(disability);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(disability);
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

         var disabilityInDb = await context.Disabilities.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (disabilityInDb == null)
            return NotFound();

         return View(disabilityInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, Disability disability, string? module, string? parent)
      {
         try
         {
            var disabilityInDb = await context.Disabilities.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == disability.Oid);

            bool isDisabilitiesDuplicate = false;

            if (disabilityInDb.Description != disability.Description)
               isDisabilitiesDuplicate = IsDisabilitiesDuplicate(disability);

            if (!isDisabilitiesDuplicate)
            {
               disability.DateCreated = disabilityInDb.DateCreated;
               disability.CreatedBy = disabilityInDb.CreatedBy;
               disability.ModifiedBy = session?.GetCurrentAdmin().Oid;
               disability.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               disability.DateModified = DateTime.Now;
               disability.IsDeleted = false;
               disability.IsSynced = false;

               context.Disabilities.Update(disability);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isDisabilitiesDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(disability);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var disability = await context.Disabilities
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (disability == null)
            return NotFound();

         return View(disability);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var disability = await context.Disabilities.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.Disabilities.Remove(disability);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsDisabilitiesDuplicate(Disability disability)
      {
         try
         {
            var disabilityInDb = context.Disabilities.FirstOrDefault(c => c.Description.ToLower() == disability.Description.ToLower() && c.IsDeleted == false);

            if (disabilityInDb != null)
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