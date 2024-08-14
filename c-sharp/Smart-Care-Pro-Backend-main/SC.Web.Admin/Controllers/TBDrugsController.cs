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
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class TBDrugsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public TBDrugsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.TBDrugs
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var tBDrugs = query.ToPagedList(pageNumber, pageSize);

         if (tBDrugs.PageCount > 0)
         {
            if (pageNumber > tBDrugs.PageCount)
               tBDrugs = query.ToPagedList(tBDrugs.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(tBDrugs);
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
      public async Task<IActionResult> Create(TBDrug tbDrug, string? module, string? parent)
      {
         try
         {
            var tbDrugInDb = IsTBDrugDuplicate(tbDrug);

            if (!tbDrugInDb)
            {
               if (ModelState.IsValid)
               {
                  tbDrug.CreatedBy = session?.GetCurrentAdmin().Oid;
                  tbDrug.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  tbDrug.DateCreated = DateTime.Now;
                  tbDrug.IsDeleted = false;
                  tbDrug.IsSynced = false;

                  context.TBDrugs.Add(tbDrug);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(tbDrug);
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

         var tBDrugInDb = await context.TBDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (tBDrugInDb == null)
            return NotFound();

         return View(tBDrugInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, TBDrug tBDrug, string? module, string? parent)
      {
         try
         {
            var tBDrugInDb = await context.TBDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == tBDrug.Oid);

            bool isTBDrugDuplicate = false;

            if (tBDrugInDb.Description != tBDrug.Description)
               isTBDrugDuplicate = IsTBDrugDuplicate(tBDrug);

            if (!isTBDrugDuplicate)
            {
               tBDrug.DateCreated = tBDrugInDb.DateCreated;
               tBDrug.CreatedBy = tBDrugInDb.CreatedBy;
               tBDrug.ModifiedBy = session?.GetCurrentAdmin().Oid;
               tBDrug.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               tBDrug.DateModified = DateTime.Now;
               tBDrug.IsDeleted = false;
               tBDrug.IsSynced = false;

               context.TBDrugs.Update(tBDrug);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isTBDrugDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(tBDrug);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var tBDrug = await context.TBDrugs
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (tBDrug == null)
            return NotFound();

         return View(tBDrug);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var tBDrug = await context.TBDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.TBDrugs.Remove(tBDrug);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsTBDrugDuplicate(TBDrug tBDrug)
      {
         try
         {
            var tBDrugInDb = context.TBDrugs.FirstOrDefault(c => c.Description.ToLower() == tBDrug.Description.ToLower() && c.IsDeleted == false);

            if (tBDrugInDb != null)
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