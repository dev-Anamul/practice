using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 09.03.2023
 * Modified by  : Bella
 * Last modified: 12.03.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class NTGLevelOneDiagnosisController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public NTGLevelOneDiagnosisController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.NTGLevelOneDianoses
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var nTGLevelOneDiagnoses = query.ToPagedList(pageNumber, pageSize);

         if (nTGLevelOneDiagnoses.PageCount > 0)
         {
            if (pageNumber > nTGLevelOneDiagnoses.PageCount)
               nTGLevelOneDiagnoses = query.ToPagedList(nTGLevelOneDiagnoses.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(nTGLevelOneDiagnoses);
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
      public async Task<IActionResult> Create(NTGLevelOneDiagnosis nTGLevelOneDiagnosis, string? module, string? parent)
      {
         try
         {
            var nTGLevelOneDiagnosisInDb = IsNTGLevelOneDianosesDuplicate(nTGLevelOneDiagnosis);

            if (!nTGLevelOneDiagnosisInDb)
            {
               if (ModelState.IsValid)
               {
                  nTGLevelOneDiagnosis.CreatedBy = session?.GetCurrentAdmin().Oid;
                  nTGLevelOneDiagnosis.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  nTGLevelOneDiagnosis.DateCreated = DateTime.Now;
                  nTGLevelOneDiagnosis.IsDeleted = false;
                  nTGLevelOneDiagnosis.IsSynced = false;

                  context.NTGLevelOneDianoses.Add(nTGLevelOneDiagnosis);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(nTGLevelOneDiagnosis);
            }
            else
            {
               if (nTGLevelOneDiagnosisInDb)
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

         var nTGLevelOneDiagnosis = await context.NTGLevelOneDianoses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (nTGLevelOneDiagnosis == null)
            return NotFound();

         return View(nTGLevelOneDiagnosis);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, NTGLevelOneDiagnosis nTGLevelOneDiagnosis, string? module, string? parent)
      {
         try
         {
            var nTGLevelOneDiagnosisInDb = await context.NTGLevelOneDianoses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == nTGLevelOneDiagnosis.Oid);

            bool isnTGLevelOneDiagnosisModelDuplicate = false;

            if (nTGLevelOneDiagnosisInDb.Description != nTGLevelOneDiagnosis.Description)
               isnTGLevelOneDiagnosisModelDuplicate = IsNTGLevelOneDianosesDuplicate(nTGLevelOneDiagnosis);

            if (!isnTGLevelOneDiagnosisModelDuplicate)
            {
               nTGLevelOneDiagnosis.DateCreated = nTGLevelOneDiagnosisInDb.DateCreated;
               nTGLevelOneDiagnosis.CreatedBy = nTGLevelOneDiagnosisInDb.CreatedBy;
               nTGLevelOneDiagnosis.ModifiedBy = session?.GetCurrentAdmin().Oid;
               nTGLevelOneDiagnosis.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               nTGLevelOneDiagnosis.DateModified = DateTime.Now;
               nTGLevelOneDiagnosis.IsDeleted = false;
               nTGLevelOneDiagnosis.IsSynced = false;

               context.NTGLevelOneDianoses.Update(nTGLevelOneDiagnosis);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isnTGLevelOneDiagnosisModelDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(nTGLevelOneDiagnosis);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var nTGLevelOneDiagnosis = await context.NTGLevelOneDianoses
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (nTGLevelOneDiagnosis == null)
            return NotFound();

         return View(nTGLevelOneDiagnosis);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var nTGLevelOneDiagnosis = await context.NTGLevelOneDianoses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.NTGLevelOneDianoses.Remove(nTGLevelOneDiagnosis);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsNTGLevelOneDianosesDuplicate(NTGLevelOneDiagnosis nTGLevelOneDiagnosis)
      {
         try
         {
            var nTGLevelOneDiagnosisInDb = context.NTGLevelOneDianoses.AsNoTracking().FirstOrDefault(p => p.Description.ToLower() == nTGLevelOneDiagnosis.Description.ToLower() && p.Oid != nTGLevelOneDiagnosis.Oid && p.IsDeleted == false);

            if (nTGLevelOneDiagnosisInDb != null)
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