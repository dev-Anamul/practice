using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 21.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class CovidSymptomsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public CovidSymptomsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.CovidSymptoms
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var covidSymptoms = query.ToPagedList(pageNumber, pageSize);

         if (covidSymptoms.PageCount > 0)
         {
            if (pageNumber > covidSymptoms.PageCount)
               covidSymptoms = query.ToPagedList(covidSymptoms.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(covidSymptoms);
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
      public async Task<IActionResult> Create(CovidSymptom covidSymptom)
      {
         try
         {
            var covidSymptomInDb = IsCovidSymptomDuplicate(covidSymptom);

            if (!covidSymptomInDb)
            {
               if (ModelState.IsValid)
               {
                  covidSymptom.CreatedBy = session?.GetCurrentAdmin().Oid;
                  covidSymptom.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  covidSymptom.DateCreated = DateTime.Now;
                  covidSymptom.IsDeleted = false;
                  covidSymptom.IsSynced = false;

                  context.CovidSymptoms.Add(covidSymptom);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create));
               }

               return View(covidSymptom);
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

         var covidSymptom = await context.CovidSymptoms.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (covidSymptom == null)
            return NotFound();

         return View(covidSymptom);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, CovidSymptom covidSymptom)
      {
         try
         {
            var covidSymptomInDb = await context.CovidSymptoms.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == covidSymptom.Oid);

            bool isCovidSymptomDuplicate = false;

            if (covidSymptomInDb.Description != covidSymptom.Description)
               isCovidSymptomDuplicate = IsCovidSymptomDuplicate(covidSymptom);

            if (!isCovidSymptomDuplicate)
            {
               covidSymptom.DateCreated = covidSymptomInDb.DateCreated;
               covidSymptom.CreatedBy = covidSymptomInDb.CreatedBy;
               covidSymptom.ModifiedBy = session?.GetCurrentAdmin().Oid;
               covidSymptom.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               covidSymptom.DateModified = DateTime.Now;
               covidSymptom.IsDeleted = false;
               covidSymptom.IsSynced = false;

               context.CovidSymptoms.Update(covidSymptom);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index));
            }
            else
            {
               if (isCovidSymptomDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(covidSymptom);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var covidSymptom = await context.CovidSymptoms
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (covidSymptom == null)
            return NotFound();

         return View(covidSymptom);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var covidSymptom = await context.CovidSymptoms.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.CovidSymptoms.Remove(covidSymptom);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsCovidSymptomDuplicate(CovidSymptom covidSymptom)
      {
         try
         {
            var covidSymptomInDb = context.CovidSymptoms.FirstOrDefault(c => c.Description.ToLower() == covidSymptom.Description.ToLower() && c.IsDeleted == false);

            if (covidSymptomInDb != null)
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