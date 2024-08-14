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
   public class ICDDiagnosisController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ICDDiagnosisController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.ICDDiagnoses
             .Include(a => a.ICPC2Description)
             .Where(x => x.Description.ToLower().Contains(search) || x.ICDCode.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.ICDCode);

         var iCDDiagnoses = query.ToPagedList(pageNumber, pageSize);

         if (iCDDiagnoses.PageCount > 0)
         {
            if (pageNumber > iCDDiagnoses.PageCount)
               iCDDiagnoses = query.ToPagedList(iCDDiagnoses.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(iCDDiagnoses);
      }
      #endregion

      #region Create
      [HttpGet]
      public IActionResult Create()
      {
         try
         {
            ViewBag.ICPC2Description = new SelectList(context.ICPC2Descriptions, FieldConstants.Oid, FieldConstants.Description);
            ViewBag.ICDCode = new SelectList(context.ICDDiagnoses, FieldConstants.Oid, FieldConstants.ICDCode);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(ICDDiagnosis iCDDiagnosis, string? module, string? parent)
      {
         try
         {
            var iCDDiagnosisInDb = IsICDDiagnosisDuplicate(iCDDiagnosis);

            if (!iCDDiagnosisInDb)
            {
               if (ModelState.IsValid)
               {
                  iCDDiagnosis.CreatedBy = session?.GetCurrentAdmin().Oid;
                  iCDDiagnosis.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  iCDDiagnosis.DateCreated = DateTime.Now;
                  iCDDiagnosis.IsDeleted = false;
                  iCDDiagnosis.IsSynced = false;

                  context.ICDDiagnoses.Add(iCDDiagnosis);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(iCDDiagnosis);
            }
            else
            {
               if (iCDDiagnosisInDb)
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

         var iCDDiagnosisInDb = await context.ICDDiagnoses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (iCDDiagnosisInDb == null)
            return NotFound();

         ViewBag.ICPC2Description = new SelectList(context.ICPC2Descriptions, FieldConstants.Oid, FieldConstants.Description);
         ViewBag.ICDCode = new SelectList(context.ICDDiagnoses, FieldConstants.Oid, FieldConstants.ICDCode);

         return View(iCDDiagnosisInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ICDDiagnosis iCDDiagnosis, string? module, string? parent)
      {
         try
         {
            var iCDDiagnosisInDb = await context.ICDDiagnoses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == iCDDiagnosis.Oid);

            bool isICDDiagnosisDuplicate = false;

            if (iCDDiagnosisInDb.Description != iCDDiagnosis.Description)
               isICDDiagnosisDuplicate = IsICDDiagnosisDuplicate(iCDDiagnosis);

            if (!isICDDiagnosisDuplicate)
            {
               iCDDiagnosis.DateCreated = iCDDiagnosisInDb.DateCreated;
               iCDDiagnosis.CreatedBy = iCDDiagnosisInDb.CreatedBy;
               iCDDiagnosis.ModifiedBy = session?.GetCurrentAdmin().Oid;
               iCDDiagnosis.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               iCDDiagnosis.DateModified = DateTime.Now;
               iCDDiagnosis.IsDeleted = false;
               iCDDiagnosis.IsSynced = false;

               context.ICDDiagnoses.Update(iCDDiagnosis);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isICDDiagnosisDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(iCDDiagnosis);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var iCDDiagnosis = await context.ICDDiagnoses
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (iCDDiagnosis == null)
            return NotFound();

         return View(iCDDiagnosis);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var iCDDiagnosis = await context.ICDDiagnoses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.ICDDiagnoses.Remove(iCDDiagnosis);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsICDDiagnosisDuplicate(ICDDiagnosis iCDDiagnosis)
      {
         try
         {
            var iCDDiagnosisInDb = context.ICDDiagnoses.AsNoTracking().FirstOrDefault(c => c.Description.ToLower() == iCDDiagnosis.Description.ToLower() && c.Oid != iCDDiagnosis.Oid && c.IsDeleted == false);

            if (iCDDiagnosisInDb != null)
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