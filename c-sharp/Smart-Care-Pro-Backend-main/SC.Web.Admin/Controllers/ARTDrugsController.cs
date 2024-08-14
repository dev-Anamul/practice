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
 * Date created : 05.04.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class ARTDrugsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ARTDrugsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.ARTDrugs
            .Include(d => d.ARTDrugClass)
             .Where(x => x.Description.ToLower().Contains(search) || x.ARTDrugClass.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.ARTDrugClass.Description);

         var aRTDrugs = query.ToPagedList(pageNumber, pageSize);

         if (aRTDrugs.PageCount > 0)
         {
            if (pageNumber > aRTDrugs.PageCount)
               aRTDrugs = query.ToPagedList(aRTDrugs.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(aRTDrugs);
      }
      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create()
      {
         try
         {
            ViewBag.ARTDrugClasses = new SelectList(context.ARTDrugClasses, FieldConstants.Oid, FieldConstants.DrugClass);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(ARTDrug aRTDrug, string? module, string? parent)
      {
         try
         {
            var IsExist = IsARTDrugDuplicate(aRTDrug);

            if (!IsExist)
            {
               if (ModelState.IsValid)
               {
                  aRTDrug.CreatedBy = session?.GetCurrentAdmin().Oid;
                  aRTDrug.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  aRTDrug.DateCreated = DateTime.Now;
                  aRTDrug.IsDeleted = false;
                  aRTDrug.IsSynced = false;

                  context.ARTDrugs.Add(aRTDrug);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(aRTDrug);
            }
            else
            {
               ModelState.AddModelError("DrugName", MessageConstants.DuplicateFound);
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

         var aRTDrug = await context.ARTDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (aRTDrug == null)
            return NotFound();

         ViewBag.ARTDrugClasses = new SelectList(context.ARTDrugClasses, FieldConstants.Oid, FieldConstants.DrugClass);

         return View(aRTDrug);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ARTDrug aRTDrug, string? module, string? parent)
      {
         try
         {
            var aRTDrugModel = await context.ARTDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == aRTDrug.Oid);

            bool isARTDrugDuplicate = false;

            if (aRTDrugModel.Description != aRTDrug.Description)
               isARTDrugDuplicate = IsARTDrugDuplicate(aRTDrug);

            if (!isARTDrugDuplicate)
            {
               aRTDrug.DateCreated = aRTDrugModel.DateCreated;
               aRTDrug.CreatedBy = aRTDrugModel.CreatedBy;
               aRTDrug.ModifiedBy = session?.GetCurrentAdmin().Oid;
               aRTDrug.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               aRTDrug.DateModified = DateTime.Now;
               aRTDrug.IsDeleted = false;
               aRTDrug.IsSynced = false;

               context.ARTDrugs.Update(aRTDrug);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isARTDrugDuplicate)
                  ModelState.AddModelError("DrugName", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(aRTDrug);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var aRTDrug = await context.ARTDrugs
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (aRTDrug == null)
            return NotFound();

         return View(aRTDrug);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var aRTDrug = await context.ARTDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.ARTDrugs.Remove(aRTDrug);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsARTDrugDuplicate(ARTDrug aRTDrug)
      {
         try
         {
            var aRTDrugInDb = context.ARTDrugs.FirstOrDefault(c => c.Description.ToLower() == aRTDrug.Description.ToLower() && c.IsDeleted == false);

            if (aRTDrugInDb != null)
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