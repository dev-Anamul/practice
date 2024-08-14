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
   public class PriorSensitizationsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public PriorSensitizationsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.PriorSensitizations
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false).OrderBy(x => x.Description);

         var priorSensitizations = query.ToPagedList(pageNumber, pageSize);

         if (priorSensitizations.PageCount > 0)
         {
            if (pageNumber > priorSensitizations.PageCount)
               priorSensitizations = query.ToPagedList(priorSensitizations.PageCount, pageSize);
         }

         ViewData["Search"] = search;
         return View(priorSensitizations);
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
      public async Task<IActionResult> Create(PriorSensitization priorSensitization, string? module, string? parent)
      {
         try
         {
            var priorSensitizationInDb = IsPriorSensitizationDuplicate(priorSensitization);

            if (!priorSensitizationInDb)
            {
               if (ModelState.IsValid)
               {
                  priorSensitization.CreatedBy = session?.GetCurrentAdmin().Oid;
                  priorSensitization.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  priorSensitization.DateCreated = DateTime.Now;
                  priorSensitization.IsDeleted = false;
                  priorSensitization.IsSynced = false;

                  context.PriorSensitizations.Add(priorSensitization);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(priorSensitization);
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

         var priorSensitizationInDb = await context.PriorSensitizations.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (priorSensitizationInDb == null)
            return NotFound();

         return View(priorSensitizationInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, PriorSensitization priorSensitization, string? module, string? parent)
      {
         try
         {
            var priorSensitizationInDb = await context.PriorSensitizations.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == priorSensitization.Oid);

            bool isPriorSensitizationDuplicate = false;

            if (priorSensitizationInDb.Description != priorSensitization.Description)
               isPriorSensitizationDuplicate = IsPriorSensitizationDuplicate(priorSensitization);

            if (!isPriorSensitizationDuplicate)
            {
               priorSensitization.DateCreated = priorSensitizationInDb.DateCreated;
               priorSensitization.CreatedBy = priorSensitizationInDb.CreatedBy;
               priorSensitization.ModifiedBy = session?.GetCurrentAdmin().Oid;
               priorSensitization.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               priorSensitization.DateModified = DateTime.Now;
               priorSensitization.IsDeleted = false;
               priorSensitization.IsSynced = false;

               context.PriorSensitizations.Update(priorSensitization);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isPriorSensitizationDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(priorSensitization);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var priorSensitization = await context.PriorSensitizations
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (priorSensitization == null)
            return NotFound();

         return View(priorSensitization);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var priorSensitization = await context.PriorSensitizations.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.PriorSensitizations.Remove(priorSensitization);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsPriorSensitizationDuplicate(PriorSensitization priorSensitization)
      {
         try
         {
            var priorSensitizationInDb = context.PriorSensitizations.FirstOrDefault(c => c.Description.ToLower() == priorSensitization.Description.ToLower() && c.IsDeleted == false);

            if (priorSensitizationInDb != null)
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