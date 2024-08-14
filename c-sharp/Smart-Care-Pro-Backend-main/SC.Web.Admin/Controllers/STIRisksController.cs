using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 22.05.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class STIRisksController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public STIRisksController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.STIRisks
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var sTIRisks = query.ToPagedList(pageNumber, pageSize);

         if (sTIRisks.PageCount > 0)
         {
            if (pageNumber > sTIRisks.PageCount)
               sTIRisks = query.ToPagedList(sTIRisks.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(sTIRisks);
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
      public async Task<IActionResult> Create(STIRisk sTIRisk)
      {
         try
         {
            var sTIRiskInDb = IsSTIRiskDuplicate(sTIRisk);

            if (!sTIRiskInDb)
            {
               if (ModelState.IsValid)
               {
                  sTIRisk.CreatedBy = session?.GetCurrentAdmin().Oid;
                  sTIRisk.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  sTIRisk.DateCreated = DateTime.Now;
                  sTIRisk.IsDeleted = false;
                  sTIRisk.IsSynced = false;

                  context.STIRisks.Add(sTIRisk);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create));
               }

               return View(sTIRisk);
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

         var sTIRiskInDb = await context.STIRisks.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (sTIRiskInDb == null)
            return NotFound();

         return View(sTIRiskInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, STIRisk sTIRisk)
      {
         try
         {
            var sTIRiskInDb = await context.STIRisks.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == sTIRisk.Oid);

            bool isSTIRiskDuplicate = false;

            if (sTIRiskInDb.Description != sTIRisk.Description)
               isSTIRiskDuplicate = IsSTIRiskDuplicate(sTIRisk);

            if (!isSTIRiskDuplicate)
            {
               sTIRisk.DateCreated = sTIRiskInDb.DateCreated;
               sTIRisk.CreatedBy = sTIRiskInDb.CreatedBy;
               sTIRisk.ModifiedBy = session?.GetCurrentAdmin().Oid;
               sTIRisk.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               sTIRisk.DateModified = DateTime.Now;
               sTIRisk.IsDeleted = false;
               sTIRisk.IsSynced = false;

               context.STIRisks.Update(sTIRisk);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index));
            }
            else
            {
               if (isSTIRiskDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(sTIRisk);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var sTIRisk = await context.STIRisks
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (sTIRisk == null)
            return NotFound();

         return View(sTIRisk);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var sTIRisk = await context.STIRisks.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.STIRisks.Remove(sTIRisk);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsSTIRiskDuplicate(STIRisk sTIRisk)
      {
         try
         {
            var sTIRiskInDb = context.STIRisks.FirstOrDefault(c => c.Description.ToLower() == sTIRisk.Description.ToLower() && c.IsDeleted == false);

            if (sTIRiskInDb != null)
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