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
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class CauseOfStillBirthsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public CauseOfStillBirthsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.CauseOfStillbirths
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var causeOfStillbirths = query.ToPagedList(pageNumber, pageSize);

         if (causeOfStillbirths.PageCount > 0)
         {
            if (pageNumber > causeOfStillbirths.PageCount)
               causeOfStillbirths = query.ToPagedList(causeOfStillbirths.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(causeOfStillbirths);
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
      public async Task<IActionResult> Create(CauseOfStillbirth causeOfStillBirth, string? module)
      {
         try
         {
            var causeOfStillBirthInDb = IsCauseOfStillBirthDuplicate(causeOfStillBirth);

            if (!causeOfStillBirthInDb)
            {
               if (ModelState.IsValid)
               {
                  causeOfStillBirth.CreatedBy = session?.GetCurrentAdmin().Oid;
                  causeOfStillBirth.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  causeOfStillBirth.DateCreated = DateTime.Now;
                  causeOfStillBirth.IsDeleted = false;
                  causeOfStillBirth.IsSynced = false;

                  context.CauseOfStillbirths.Add(causeOfStillBirth);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module });
               }

               return View(causeOfStillBirth);
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

         var causeOfStillBirth = await context.CauseOfStillbirths.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (causeOfStillBirth == null)
            return NotFound();

         return View(causeOfStillBirth);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, CauseOfStillbirth causeOfStillBirth, string? module)
      {
         try
         {
            var causeOfStillBirthInDb = await context.CauseOfStillbirths.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == causeOfStillBirth.Oid);

            bool isCauseOfStillBirthDuplicate = false;

            if (causeOfStillBirthInDb.Description != causeOfStillBirth.Description)
               isCauseOfStillBirthDuplicate = IsCauseOfStillBirthDuplicate(causeOfStillBirth);

            if (!isCauseOfStillBirthDuplicate)
            {
               causeOfStillBirth.DateCreated = causeOfStillBirthInDb.DateCreated;
               causeOfStillBirth.CreatedBy = causeOfStillBirthInDb.CreatedBy;
               causeOfStillBirth.ModifiedBy = session?.GetCurrentAdmin().Oid;
               causeOfStillBirth.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               causeOfStillBirth.DateModified = DateTime.Now;
               causeOfStillBirth.IsDeleted = false;
               causeOfStillBirth.IsSynced = false;

               context.CauseOfStillbirths.Update(causeOfStillBirth);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module });
            }
            else
            {
               if (isCauseOfStillBirthDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(causeOfStillBirth);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var causeOfStillBirth = await context.CauseOfStillbirths
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (causeOfStillBirth == null)
            return NotFound();

         return View(causeOfStillBirth);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var causeOfStillBirth = await context.CauseOfStillbirths.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.CauseOfStillbirths.Remove(causeOfStillBirth);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsCauseOfStillBirthDuplicate(CauseOfStillbirth causeOfStillBirth)
      {
         try
         {
            var causeOfStillBirthInDb = context.CauseOfStillbirths.FirstOrDefault(c => c.Description.ToLower() == causeOfStillBirth.Description.ToLower() && c.IsDeleted == false);

            if (causeOfStillBirthInDb != null)
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