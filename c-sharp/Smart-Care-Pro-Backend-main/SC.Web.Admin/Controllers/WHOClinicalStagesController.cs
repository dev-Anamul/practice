using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Brian
 * Date created : 19.04.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class WHOClinicalStagesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;
      public WHOClinicalStagesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.WHOClinicalStages
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var wHOClinicalStages = query.ToPagedList(pageNumber, pageSize);

         if (wHOClinicalStages.PageCount > 0)
         {
            if (pageNumber > wHOClinicalStages.PageCount)
               wHOClinicalStages = query.ToPagedList(wHOClinicalStages.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(wHOClinicalStages);
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
      public async Task<IActionResult> Create(WHOClinicalStage wHOClinicalStage, string? module, string? parent)
      {
         try
         {
            var wHOClinicalStageInDb = IsWHOClinicalStageDuplicate(wHOClinicalStage);

            if (!wHOClinicalStageInDb)
            {
               if (ModelState.IsValid)
               {
                  wHOClinicalStage.CreatedBy = session?.GetCurrentAdmin().Oid;
                  wHOClinicalStage.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  wHOClinicalStage.DateCreated = DateTime.Now;
                  wHOClinicalStage.IsDeleted = false;
                  wHOClinicalStage.IsSynced = false;

                  context.WHOClinicalStages.Add(wHOClinicalStage);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(wHOClinicalStage);
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

         var wHOClinicalStageInDb = await context.WHOClinicalStages.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (wHOClinicalStageInDb == null)
            return NotFound();

         return View(wHOClinicalStageInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, WHOClinicalStage wHOClinicalStage, string? module, string? parent)
      {
         try
         {
            var wHOClinicalStageInDb = await context.WHOClinicalStages.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == wHOClinicalStage.Oid);

            bool isWHOClinicalStageDuplicate = false;

            if (wHOClinicalStageInDb.Description != wHOClinicalStage.Description)
               isWHOClinicalStageDuplicate = IsWHOClinicalStageDuplicate(wHOClinicalStage);

            if (!isWHOClinicalStageDuplicate)
            {
               wHOClinicalStage.DateCreated = wHOClinicalStageInDb.DateCreated;
               wHOClinicalStage.CreatedBy = wHOClinicalStageInDb.CreatedBy;
               wHOClinicalStage.ModifiedBy = session?.GetCurrentAdmin().Oid;
               wHOClinicalStage.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               wHOClinicalStage.DateModified = DateTime.Now;
               wHOClinicalStage.IsDeleted = false;
               wHOClinicalStage.IsSynced = false;

               context.WHOClinicalStages.Update(wHOClinicalStage);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isWHOClinicalStageDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(wHOClinicalStage);
      }
      #endregion

      #region Read
      public bool IsWHOClinicalStageDuplicate(WHOClinicalStage wHOClinicalStage)
      {
         try
         {
            var wHOClinicalStageInDb = context.WHOClinicalStages.FirstOrDefault(c => c.Description.ToLower() == wHOClinicalStage.Description.ToLower() && c.IsDeleted == false);

            if (wHOClinicalStageInDb != null)
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