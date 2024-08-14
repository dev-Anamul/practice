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
namespace Web.Controllers
{
   [RequestAuthenticationFilter]
   public class ConstitutionalSymptomsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ConstitutionalSymptomsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.ConstitutionalSymptoms
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var constitutionalSymptoms = query.ToPagedList(pageNumber, pageSize);

         if (constitutionalSymptoms.PageCount > 0)
         {
            if (pageNumber > constitutionalSymptoms.PageCount)
               constitutionalSymptoms = query.ToPagedList(constitutionalSymptoms.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(constitutionalSymptoms);
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
      public async Task<IActionResult> Create(ConstitutionalSymptom constitutionalSymptom, string? module, string? parent)
      {
         try
         {
            var constitutionalSymptomInDb = IsConstitutionalSymptomDuplicate(constitutionalSymptom);

            if (!constitutionalSymptomInDb)
            {
               if (ModelState.IsValid)
               {
                  constitutionalSymptom.CreatedBy = session?.GetCurrentAdmin().Oid;
                  constitutionalSymptom.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  constitutionalSymptom.DateCreated = DateTime.Now;
                  constitutionalSymptom.IsDeleted = false;
                  constitutionalSymptom.IsSynced = false;

                  context.ConstitutionalSymptoms.Add(constitutionalSymptom);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(constitutionalSymptom);
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

         var constitutionalSymptom = await context.ConstitutionalSymptoms.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (constitutionalSymptom == null)
            return NotFound();

         return View(constitutionalSymptom);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ConstitutionalSymptom constitutionalSymptom, string? module, string? parent)
      {
         try
         {
            var constitutionalSymptomInDb = await context.ConstitutionalSymptoms.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == constitutionalSymptom.Oid);

            bool isConstitutionalSymptomDuplicate = false;

            if (constitutionalSymptomInDb.Description != constitutionalSymptom.Description)
               isConstitutionalSymptomDuplicate = IsConstitutionalSymptomDuplicate(constitutionalSymptom);

            if (!isConstitutionalSymptomDuplicate)
            {
               constitutionalSymptom.DateCreated = constitutionalSymptomInDb.DateCreated;
               constitutionalSymptom.CreatedBy = constitutionalSymptomInDb.CreatedBy;
               constitutionalSymptom.ModifiedBy = session?.GetCurrentAdmin().Oid;
               constitutionalSymptom.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               constitutionalSymptom.DateModified = DateTime.Now;
               constitutionalSymptom.IsDeleted = false;
               constitutionalSymptom.IsSynced = false;

               context.ConstitutionalSymptoms.Update(constitutionalSymptom);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(constitutionalSymptom);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var constitutionalSymptom = await context.ConstitutionalSymptoms
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (constitutionalSymptom == null)
            return NotFound();

         return View(constitutionalSymptom);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var constitutionalSymptom = await context.ConstitutionalSymptoms.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.ConstitutionalSymptoms.Remove(constitutionalSymptom);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsConstitutionalSymptomDuplicate(ConstitutionalSymptom symptom)
      {
         try
         {
            var constitutionalSymptomInDb = context.ConstitutionalSymptoms.FirstOrDefault(c => c.Description.ToLower() == symptom.Description.ToLower() && c.IsDeleted == false);

            if (constitutionalSymptomInDb != null)
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