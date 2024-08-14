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
 * Last modified: 22.05.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class NeonatalBirthOutcomesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public NeonatalBirthOutcomesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.NeonatalBirthOutcomes
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var neonatalBirthOutcomes = query.ToPagedList(pageNumber, pageSize);

         if (neonatalBirthOutcomes.PageCount > 0)
         {
            if (pageNumber > neonatalBirthOutcomes.PageCount)
               neonatalBirthOutcomes = query.ToPagedList(neonatalBirthOutcomes.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(neonatalBirthOutcomes);
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
      public async Task<IActionResult> Create(NeonatalBirthOutcome neonatalBirthOutcome, string? module)
      {
         try
         {
            var neonatalBirthOutcomeInDb = IsNeonatalBirthOutcomeDuplicate(neonatalBirthOutcome);

            if (!neonatalBirthOutcomeInDb)
            {
               if (ModelState.IsValid)
               {
                  neonatalBirthOutcome.CreatedBy = session?.GetCurrentAdmin().Oid;
                  neonatalBirthOutcome.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  neonatalBirthOutcome.DateCreated = DateTime.Now;
                  neonatalBirthOutcome.IsDeleted = false;
                  neonatalBirthOutcome.IsSynced = false;

                  context.NeonatalBirthOutcomes.Add(neonatalBirthOutcome);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module });
               }

               return View(neonatalBirthOutcome);
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

         var neonatalBirthOutcomeInDb = await context.NeonatalBirthOutcomes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (neonatalBirthOutcomeInDb == null)
            return NotFound();

         return View(neonatalBirthOutcomeInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, NeonatalBirthOutcome neonatalBirthOutcome, string? module)
      {
         try
         {
            var neonatalBirthOutcomeInDb = await context.NeonatalBirthOutcomes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == neonatalBirthOutcome.Oid);

            bool isNeonatalBirthOutcomeDuplicate = false;

            if (neonatalBirthOutcomeInDb.Description != neonatalBirthOutcome.Description)
               isNeonatalBirthOutcomeDuplicate = IsNeonatalBirthOutcomeDuplicate(neonatalBirthOutcome);

            if (!isNeonatalBirthOutcomeDuplicate)
            {
               neonatalBirthOutcome.DateCreated = neonatalBirthOutcomeInDb.DateCreated;
               neonatalBirthOutcome.CreatedBy = neonatalBirthOutcomeInDb.CreatedBy;
               neonatalBirthOutcome.ModifiedBy = session?.GetCurrentAdmin().Oid;
               neonatalBirthOutcome.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               neonatalBirthOutcome.DateModified = DateTime.Now;
               neonatalBirthOutcome.IsDeleted = false;
               neonatalBirthOutcome.IsSynced = false;

               context.NeonatalBirthOutcomes.Update(neonatalBirthOutcome);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module });
            }
            else
            {
               if (isNeonatalBirthOutcomeDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(neonatalBirthOutcome);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var neonatalBirthOutcome = await context.NeonatalBirthOutcomes
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (neonatalBirthOutcome == null)
            return NotFound();

         return View(neonatalBirthOutcome);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var neonatalBirthOutcome = await context.NeonatalBirthOutcomes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.NeonatalBirthOutcomes.Remove(neonatalBirthOutcome);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsNeonatalBirthOutcomeDuplicate(NeonatalBirthOutcome neonatalBirthOutcome)
      {
         try
         {
            var neonatalBirthOutcomeInDb = context.NeonatalBirthOutcomes.FirstOrDefault(c => c.Description.ToLower() == neonatalBirthOutcome.Description.ToLower() && c.IsDeleted == false);

            if (neonatalBirthOutcomeInDb != null)
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