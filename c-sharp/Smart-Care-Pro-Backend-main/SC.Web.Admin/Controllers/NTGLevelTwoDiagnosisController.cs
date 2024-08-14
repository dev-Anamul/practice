using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Lion
 * Date created : 09.03.2023
 * Modified by  : Bella
 * Last modified: 30.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class NTGLevelTwoDiagnosisController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public NTGLevelTwoDiagnosisController(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      #region Index

      public async Task<IActionResult> Index(string? search, int? page, int oid)
      {
         if (oid == 0)
            oid = Convert.ToInt32(TempData["manualId"]);

         ViewBag.Oid = oid;

         var getNTG = await context.NTGLevelOneDianoses.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

         TempData["manualId"] = getNTG.Oid;

         ViewBag.NTGLevelTwoID = oid;
         ViewBag.NTGLevelTwo = getNTG.Description;

         search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;

         if (page > 0 && search == null)
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.NTGLevelTwoDiagnoses
            .Include(n => n.NTGLevelOneDiagnosis)
            .Where(x => (x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false) && x.NTGLevelOneId == oid)
            .OrderBy(x => x.Description);

         var nTGLevelTwoDiagnoses = query.ToPagedList(pageNumber, pageSize);

         if (nTGLevelTwoDiagnoses.PageCount > 0)
         {
            if (pageNumber > nTGLevelTwoDiagnoses.PageCount)
               nTGLevelTwoDiagnoses = query.ToPagedList(nTGLevelTwoDiagnoses.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(nTGLevelTwoDiagnoses);
      }

      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create(int oid)
      {
         try
         {
            var getNTG = await context.NTGLevelOneDianoses.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

            ViewBag.NTGLevelTwoID = oid;
            ViewBag.NTGLevelTwo = getNTG.Description;
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(NTGLevelTwoDiagnosis nTGLevelTwoDiagnosis, string? module, string? parent)
      {
         try
         {
            var nTGLevelTwoDiagnosisInDb = IsNTGLevelTwoDiagnosesDuplicate(nTGLevelTwoDiagnosis);

            if (!nTGLevelTwoDiagnosisInDb)
            {
               if (ModelState.IsValid)
               {
                  nTGLevelTwoDiagnosis.CreatedBy = session?.GetCurrentAdmin().Oid;
                  nTGLevelTwoDiagnosis.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  nTGLevelTwoDiagnosis.DateCreated = DateTime.Now;
                  nTGLevelTwoDiagnosis.IsDeleted = false;
                  nTGLevelTwoDiagnosis.IsSynced = false;

                  context.NTGLevelTwoDiagnoses.Add(nTGLevelTwoDiagnosis);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  //return RedirectToAction("Create", new { oid = nTGLevelTwoDiagnosis.NTGLevelOneId, module = module, parent = parent });
                  return RedirectToAction(nameof(Create), new { oid = nTGLevelTwoDiagnosis.NTGLevelOneId, module = module, parent = parent });
               }

               return View(nTGLevelTwoDiagnosis);
            }
            else
            {
               if (nTGLevelTwoDiagnosisInDb)
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

         var nTGLevelTwoDiagnosisInDb = await context.NTGLevelTwoDiagnoses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (nTGLevelTwoDiagnosisInDb == null)
            return NotFound();

         ViewBag.NTGLevelOneDiagnoses = new SelectList(context.NTGLevelOneDianoses, FieldConstants.Oid, FieldConstants.Description);
         ViewBag.NTGId = nTGLevelTwoDiagnosisInDb.NTGLevelOneId;

         return View(nTGLevelTwoDiagnosisInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, NTGLevelTwoDiagnosis nTGLevelTwoDiagnosis, string? module, string? parent)
      {
         try
         {
            var nTGLevelTwoDiagnosisInDb = await context.NTGLevelTwoDiagnoses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == nTGLevelTwoDiagnosis.Oid);

            bool isnTGLevelTwoDiagnosisDuplicate = false;

            if (nTGLevelTwoDiagnosisInDb.Description != nTGLevelTwoDiagnosis.Description)
               isnTGLevelTwoDiagnosisDuplicate = IsNTGLevelTwoDiagnosesDuplicate(nTGLevelTwoDiagnosis);

            if (!isnTGLevelTwoDiagnosisDuplicate)
            {
               nTGLevelTwoDiagnosis.DateCreated = nTGLevelTwoDiagnosisInDb.DateCreated;
               nTGLevelTwoDiagnosis.CreatedBy = nTGLevelTwoDiagnosisInDb.CreatedBy;
               nTGLevelTwoDiagnosis.ModifiedBy = session?.GetCurrentAdmin().Oid;
               nTGLevelTwoDiagnosis.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               nTGLevelTwoDiagnosis.DateModified = DateTime.Now;
               nTGLevelTwoDiagnosis.IsDeleted = false;
               nTGLevelTwoDiagnosis.IsSynced = false;

               context.NTGLevelTwoDiagnoses.Update(nTGLevelTwoDiagnosis);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               //return RedirectToAction("Index", new { oid = nTGLevelTwoDiagnosis.NTGLevelOneId });
               return RedirectToAction(nameof(Index), new { oid = nTGLevelTwoDiagnosis.NTGLevelOneId, module = module, parent = parent });
            }
            else
            {
               if (isnTGLevelTwoDiagnosisDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(nTGLevelTwoDiagnosis);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var nTGLevelTwoDiagnosis = await context.NTGLevelTwoDiagnoses
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (nTGLevelTwoDiagnosis == null)
            return NotFound();

         return View(nTGLevelTwoDiagnosis);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var nTGLevelTwoDiagnosis = await context.NTGLevelTwoDiagnoses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.NTGLevelTwoDiagnoses.Remove(nTGLevelTwoDiagnosis);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsNTGLevelTwoDiagnosesDuplicate(NTGLevelTwoDiagnosis nTGLevelTwoDiagnosis)
      {
         try
         {
            var nTGLevelTwoDiagnosisInDb = context.NTGLevelTwoDiagnoses
               .AsNoTracking()
               .FirstOrDefault(p =>
                  p.Description.ToLower() == nTGLevelTwoDiagnosis.Description.ToLower() &&
                  p.NTGLevelOneId == nTGLevelTwoDiagnosis.NTGLevelOneId &&
                  p.Oid != nTGLevelTwoDiagnosis.Oid &&
                  p.IsDeleted == false);

            if (nTGLevelTwoDiagnosisInDb != null)
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