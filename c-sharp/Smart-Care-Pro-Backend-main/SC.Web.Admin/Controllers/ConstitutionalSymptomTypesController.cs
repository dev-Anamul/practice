using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 21.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Web.Controllers
{
   [RequestAuthenticationFilter]
   public class ConstitutionalSymptomTypesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ConstitutionalSymptomTypesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.ConstitutionalSymptomTypes
            .Include(c => c.ConstitutionalSymptom)
             .Where(x => x.Description.ToLower().Contains(search) || x.ConstitutionalSymptom.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.ConstitutionalSymptom.Description);

         var constitutionalSymptomTypes = query.ToPagedList(pageNumber, pageSize);

         if (constitutionalSymptomTypes.PageCount > 0)
         {
            if (pageNumber > constitutionalSymptomTypes.PageCount)
               constitutionalSymptomTypes = query.ToPagedList(constitutionalSymptomTypes.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(constitutionalSymptomTypes);
      }

      #endregion

      #region Create
      [HttpGet]
      public IActionResult Create()
      {
         ViewBag.ConstitutionalSymptom = new SelectList(context.ConstitutionalSymptoms, FieldConstants.Oid, FieldConstants.Symptom);

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(ConstitutionalSymptomType symptom, string? module, string? parent)
      {
         try
         {
            var symptomInDb = IsConstitutionalSymptomTypeDuplicate(symptom);

            if (!symptomInDb)
            {
               if (ModelState.IsValid)
               {
                  symptom.CreatedBy = session?.GetCurrentAdmin().Oid;
                  symptom.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  symptom.DateCreated = DateTime.Now;
                  symptom.IsDeleted = false;
                  symptom.IsSynced = false;

                  context.ConstitutionalSymptomTypes.Add(symptom);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(symptom);
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

         var symptom = await context.ConstitutionalSymptomTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (symptom == null)
            return NotFound();

         ViewBag.ConstitutionalSymptom = new SelectList(context.ConstitutionalSymptoms, FieldConstants.Oid, FieldConstants.Symptom);

         return View(symptom);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ConstitutionalSymptomType symptom, string? module, string? parent)
      {
         try
         {
            var symptomInDb = await context.ConstitutionalSymptomTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == symptom.Oid);

            bool isConstitutionalSymptomDuplicate = false;

            if (symptomInDb.Description != symptom.Description)
               isConstitutionalSymptomDuplicate = IsConstitutionalSymptomTypeDuplicate(symptom);

            if (!isConstitutionalSymptomDuplicate)
            {
               symptom.DateCreated = symptomInDb.DateCreated;
               symptom.CreatedBy = symptomInDb.CreatedBy;
               symptom.ModifiedBy = session?.GetCurrentAdmin().Oid;
               symptom.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               symptom.DateModified = DateTime.Now;
               symptom.IsDeleted = false;
               symptom.IsSynced = false;

               context.ConstitutionalSymptomTypes.Update(symptom);
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

         return View(symptom);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var symptom = await context.ConstitutionalSymptomTypes
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (symptom == null)
            return NotFound();

         return View(symptom);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var symptom = await context.ConstitutionalSymptomTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.ConstitutionalSymptomTypes.Remove(symptom);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsConstitutionalSymptomTypeDuplicate(ConstitutionalSymptomType symptom)
      {
         try
         {
            var symptomInDb = context.ConstitutionalSymptomTypes.FirstOrDefault(c => c.Description.ToLower() == symptom.Description.ToLower() && c.IsDeleted == false);

            if (symptomInDb != null)
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