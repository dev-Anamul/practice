using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Brian
 * Date created : 05.04.2023
 * Modified by  : Bella
 * Last modified: 22.05.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class TPTDrugsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public TPTDrugsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.TPTDrugs
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var tptDrugs = query.ToPagedList(pageNumber, pageSize);

         if (tptDrugs.PageCount > 0)
         {
            if (pageNumber > tptDrugs.PageCount)
               tptDrugs = query.ToPagedList(tptDrugs.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(tptDrugs);
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
      public async Task<IActionResult> Create(TPTDrug tPTDrug, string? module, string? parent)
      {
         try
         {
            var tPTDrugInDb = IsTPTDrugDuplicate(tPTDrug);

            if (!tPTDrugInDb)
            {
               if (ModelState.IsValid)
               {
                  tPTDrug.CreatedBy = session?.GetCurrentAdmin().Oid;
                  tPTDrug.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  tPTDrug.DateCreated = DateTime.Now;
                  tPTDrug.IsDeleted = false;
                  tPTDrug.IsSynced = false;

                  context.TPTDrugs.Add(tPTDrug);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(tPTDrug);
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

         var tPTDrugInDb = await context.TPTDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (tPTDrugInDb == null)
            return NotFound();

         return View(tPTDrugInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, TPTDrug tPTDrug, string? module, string? parent)
      {
         try
         {
            var tPTDrugInDb = await context.TPTDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == tPTDrug.Oid);

            bool isTPTDrugDuplicate = false;

            if (tPTDrugInDb.Description != tPTDrug.Description)
               isTPTDrugDuplicate = IsTPTDrugDuplicate(tPTDrug);

            if (!isTPTDrugDuplicate)
            {
               tPTDrug.DateCreated = tPTDrugInDb.DateCreated;
               tPTDrug.CreatedBy = tPTDrugInDb.CreatedBy;
               tPTDrug.ModifiedBy = session?.GetCurrentAdmin().Oid;
               tPTDrug.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               tPTDrug.DateModified = DateTime.Now;
               tPTDrug.IsDeleted = false;
               tPTDrug.IsSynced = false;

               context.TPTDrugs.Update(tPTDrug);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isTPTDrugDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(tPTDrug);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var tPTDrug = await context.TPTDrugs
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (tPTDrug == null)
            return NotFound();

         return View(tPTDrug);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var tPTDrug = await context.TPTDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.TPTDrugs.Remove(tPTDrug);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsTPTDrugDuplicate(TPTDrug tPTDrug)
      {
         try
         {
            var tPTDrugInDb = context.TPTDrugs.FirstOrDefault(c => c.Description.ToLower() == tPTDrug.Description.ToLower() && c.IsDeleted == false);

            if (tPTDrugInDb != null)
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