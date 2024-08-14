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
 * Last modified: 13.03.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class NTGLevelThreeDiagnosisController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public NTGLevelThreeDiagnosisController(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      #region Index
      public async Task<IActionResult> Index(string? search, int? page, int oid)
      {
         if (oid == 0)
            oid = Convert.ToInt32(TempData["manualNTGTwoId"]);

         ViewBag.Oid = oid;

         var getNTG = await context.NTGLevelTwoDiagnoses.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

         TempData["manualNTGTwoId"] = getNTG.Oid;

         ViewBag.NTGLevelTwoID = oid;
         ViewBag.NTGLevelTwo = getNTG.Description;
         ViewBag.NTGLevelOneID = getNTG.NTGLevelOneId;

         search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;

         if (page > 0 && search == null)
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.NTGLevelThreeDiagnoses
            .Include(n => n.NTGLevelTwoDiagnosis)
            .ThenInclude(n => n.NTGLevelOneDiagnosis)
            .Where(x => (x.Description.ToLower().Contains(search) || search == null) && x.NTGLevelTwoId == oid && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var nTGLevelThreeDiagnoses = query.ToPagedList(pageNumber, pageSize);

         if (nTGLevelThreeDiagnoses.PageCount > 0)
         {
            if (pageNumber > nTGLevelThreeDiagnoses.PageCount)
               nTGLevelThreeDiagnoses = query.ToPagedList(nTGLevelThreeDiagnoses.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(nTGLevelThreeDiagnoses);
      }

      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create(int oid)
      {
         try
         {
            var getNTG = await context.NTGLevelTwoDiagnoses.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

            ViewBag.NTGLevelTwoID = oid;
            ViewBag.NTGLevelTwo = getNTG.Description;

            //var icdItems = await context.ICDDiagnoses
            //             .OrderBy(icd => icd.ICDCode)
            //             .ToListAsync();

            //ViewBag.ICD = new SelectList(icdItems, FieldConstants.Oid, FieldConstants.ICDCode);

            var icdItems = await context.ICDDiagnoses
                   .OrderBy(icd => icd.ICDCode)
                   .Select(icd => new SelectListItem
                   {
                      Value = icd.Oid.ToString(),
                      Text = $"{icd.ICDCode} - {icd.Description}"
                   })
                   .ToListAsync();

            ViewBag.ICD = new SelectList(icdItems, "Value", "Text");
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(NTGLevelThreeDiagnosis nTGLevelThreeDiagnosis, string? module, string? parent)
      {
         try
         {
            var nTGLevelThreeDiagnosisInDb = IsNTGLevelThreeDiagnosisDuplicate(nTGLevelThreeDiagnosis);

            if (!nTGLevelThreeDiagnosisInDb)
            {
               if (ModelState.IsValid)
               {
                  nTGLevelThreeDiagnosis.CreatedBy = session?.GetCurrentAdmin().Oid;
                  nTGLevelThreeDiagnosis.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  nTGLevelThreeDiagnosis.DateCreated = DateTime.Now;
                  nTGLevelThreeDiagnosis.IsDeleted = false;
                  nTGLevelThreeDiagnosis.IsSynced = false;

                  nTGLevelThreeDiagnosis.Oid = default(int);

                  context.NTGLevelThreeDiagnoses.Add(nTGLevelThreeDiagnosis);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction("Create", new { oid = nTGLevelThreeDiagnosis.NTGLevelTwoId, module = module, parent = parent });
               }

               return View(nTGLevelThreeDiagnosis);
            }
            else
            {
               if (nTGLevelThreeDiagnosisInDb)
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

         var nTGLevelThreeDiagnosis = await context.NTGLevelThreeDiagnoses.Include(n => n.NTGLevelTwoDiagnosis).ThenInclude(t => t.NTGLevelOneDiagnosis).AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         var ntgLevelTwo = context.NTGLevelTwoDiagnoses.ToList();

         if (nTGLevelThreeDiagnosis == null)
            return NotFound();

         ViewBag.NTGLevelOneDiagnoses = new SelectList(context.NTGLevelOneDianoses, FieldConstants.Oid, FieldConstants.Description, nTGLevelThreeDiagnosis.NTGLevelTwoDiagnosis.NTGLevelOneDiagnosis.Oid);
         ViewBag.NTGLevelTwo = new SelectList(ntgLevelTwo.Where(d => d.NTGLevelOneId == nTGLevelThreeDiagnosis.NTGLevelTwoDiagnosis.NTGLevelOneId).ToList(), FieldConstants.Oid, FieldConstants.Description, nTGLevelThreeDiagnosis.NTGLevelTwoId);

         var icdItems = await context.ICDDiagnoses
                   .OrderBy(icd => icd.ICDCode)
                   .Select(icd => new SelectListItem
                   {
                      Value = icd.Oid.ToString(),
                      Text = $"{icd.ICDCode} - {icd.Description}"
                   })
                   .ToListAsync();

         ViewBag.ICD = new SelectList(icdItems, "Value", "Text");

         ViewBag.NtgId = nTGLevelThreeDiagnosis.NTGLevelTwoId;

         return View(nTGLevelThreeDiagnosis);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, NTGLevelThreeDiagnosis nTGLevelThreeDiagnosis, string? module, string? parent)
      {
         try
         {
            var nTGLevelThreeDiagnosisInDb = await context.NTGLevelThreeDiagnoses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == nTGLevelThreeDiagnosis.Oid);

            bool isnTGLevelThreeDiagnosisDuplicate = false;

            if (nTGLevelThreeDiagnosisInDb.Description != nTGLevelThreeDiagnosis.Description)
               isnTGLevelThreeDiagnosisDuplicate = IsNTGLevelThreeDiagnosisDuplicate(nTGLevelThreeDiagnosis);

            if (!isnTGLevelThreeDiagnosisDuplicate)
            {
               nTGLevelThreeDiagnosis.DateCreated = nTGLevelThreeDiagnosisInDb.DateCreated;
               nTGLevelThreeDiagnosis.CreatedBy = nTGLevelThreeDiagnosisInDb.CreatedBy;
               nTGLevelThreeDiagnosis.ModifiedBy = session?.GetCurrentAdmin().Oid;
               nTGLevelThreeDiagnosis.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               nTGLevelThreeDiagnosis.DateModified = DateTime.Now;
               nTGLevelThreeDiagnosis.IsDeleted = false;
               nTGLevelThreeDiagnosis.IsSynced = false;

               context.NTGLevelThreeDiagnoses.Update(nTGLevelThreeDiagnosis);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction("Index", new { oid = nTGLevelThreeDiagnosis.NTGLevelTwoId, module = module, parent = parent });
            }
            else
            {
               if (isnTGLevelThreeDiagnosisDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(nTGLevelThreeDiagnosis);
      }
      #endregion

      #region Details
      [HttpGet]
      public async Task<IActionResult> Details(int? id, string? module, string? parent)
      {
         if (id == null)
         {
            return NotFound();
         }

         var levelThreeInDb = await context.NTGLevelThreeDiagnoses.FirstOrDefaultAsync(n => n.Oid == id);

         if (levelThreeInDb == null)
         {
            return NotFound();
         }

         ViewBag.levelThree = levelThreeInDb.NTGLevelTwoId;

         return View(levelThreeInDb);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var nTGLevelThreeDiagnosis = await context.NTGLevelThreeDiagnoses
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (nTGLevelThreeDiagnosis == null)
            return NotFound();

         return View(nTGLevelThreeDiagnosis);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var nTGLevelThreeDiagnosis = await context.NTGLevelThreeDiagnoses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.NTGLevelThreeDiagnoses.Remove(nTGLevelThreeDiagnosis);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsNTGLevelThreeDiagnosisDuplicate(NTGLevelThreeDiagnosis nTGLevelThreeDiagnosis)
      {
         try
         {
            var nTGLevelThreeDiagnosisInDb = context.NTGLevelThreeDiagnoses
               .AsNoTracking()
               .FirstOrDefault(p =>
                  p.Description.ToLower() == nTGLevelThreeDiagnosis.Description.ToLower() &&
                  p.NTGLevelTwoId == nTGLevelThreeDiagnosis.NTGLevelTwoId &&
                  p.Oid != nTGLevelThreeDiagnosis.Oid &&
                  p.IsDeleted == false);

            if (nTGLevelThreeDiagnosisInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      public JsonResult LoadNTGLevelThree(int id)
      {
         var ntgLevelThree = context.NTGLevelTwoDiagnoses.Where(p => p.NTGLevelOneId == id && p.IsDeleted == false).ToList();
         return Json(ntgLevelThree);
      }
      #endregion
   }
}