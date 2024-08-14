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
   public class CauseOfNeonatalDeathsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public CauseOfNeonatalDeathsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.CauseOfNeonatalDeaths
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var causeOfNeonatalDeaths = query.ToPagedList(pageNumber, pageSize);

         if (causeOfNeonatalDeaths.PageCount > 0)
         {
            if (pageNumber > causeOfNeonatalDeaths.PageCount)
               causeOfNeonatalDeaths = query.ToPagedList(causeOfNeonatalDeaths.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(causeOfNeonatalDeaths);
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
      public async Task<IActionResult> Create(CauseOfNeonatalDeath causeOfNeonatalDeath, string? module)
      {
         try
         {
            var causeOfNeonatalDeathInDb = IsCauseOfNeonatalDeathDuplicate(causeOfNeonatalDeath);

            if (!causeOfNeonatalDeathInDb)
            {
               if (ModelState.IsValid)
               {
                  causeOfNeonatalDeath.CreatedBy = session?.GetCurrentAdmin().Oid;
                  causeOfNeonatalDeath.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  causeOfNeonatalDeath.DateCreated = DateTime.Now;
                  causeOfNeonatalDeath.IsDeleted = false;
                  causeOfNeonatalDeath.IsSynced = false;

                  context.CauseOfNeonatalDeaths.Add(causeOfNeonatalDeath);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module });
               }

               return View(causeOfNeonatalDeath);
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

         var causeOfNeonatalDeath = await context.CauseOfNeonatalDeaths.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (causeOfNeonatalDeath == null)
            return NotFound();

         return View(causeOfNeonatalDeath);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, CauseOfNeonatalDeath causeOfNeonatalDeath, string? module)
      {
         try
         {
            var causeOfNeonatalDeathInDb = await context.CauseOfNeonatalDeaths.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == causeOfNeonatalDeath.Oid);

            bool isCauseOfNeonatalDeathDuplicate = false;

            if (causeOfNeonatalDeathInDb.Description != causeOfNeonatalDeath.Description)
               isCauseOfNeonatalDeathDuplicate = IsCauseOfNeonatalDeathDuplicate(causeOfNeonatalDeath);

            if (!isCauseOfNeonatalDeathDuplicate)
            {
               causeOfNeonatalDeath.DateCreated = causeOfNeonatalDeathInDb.DateCreated;
               causeOfNeonatalDeath.CreatedBy = causeOfNeonatalDeathInDb.CreatedBy;
               causeOfNeonatalDeath.ModifiedBy = session?.GetCurrentAdmin().Oid;
               causeOfNeonatalDeath.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               causeOfNeonatalDeath.DateModified = DateTime.Now;
               causeOfNeonatalDeath.IsDeleted = false;
               causeOfNeonatalDeath.IsSynced = false;

               context.CauseOfNeonatalDeaths.Update(causeOfNeonatalDeath);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module });
            }
            else
            {
               if (isCauseOfNeonatalDeathDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(causeOfNeonatalDeath);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var causeOfNeonatalDeath = await context.CauseOfNeonatalDeaths
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (causeOfNeonatalDeath == null)
            return NotFound();

         return View(causeOfNeonatalDeath);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var causeOfNeonatalDeath = await context.CauseOfNeonatalDeaths.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.CauseOfNeonatalDeaths.Remove(causeOfNeonatalDeath);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsCauseOfNeonatalDeathDuplicate(CauseOfNeonatalDeath causeOfNeonatalDeath)
      {
         try
         {
            var causeOfNeonatalDeathInDb = context.CauseOfNeonatalDeaths.FirstOrDefault(c => c.Description.ToLower() == causeOfNeonatalDeath.Description.ToLower() && c.IsDeleted == false);

            if (causeOfNeonatalDeathInDb != null)
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