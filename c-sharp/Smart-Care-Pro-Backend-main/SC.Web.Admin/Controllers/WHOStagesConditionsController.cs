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
 * Date created : 25.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class WHOStagesConditionsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;
      public WHOStagesConditionsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.WHOStagesConditions
            .Include(d => d.WHOClinicalStage)
            .Where(x => x.Description.ToLower().Contains(search) || x.WHOClinicalStage.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.WHOClinicalStage.Description);

         var wHOStagesConditions = query.ToPagedList(pageNumber, pageSize);

         if (wHOStagesConditions.PageCount > 0)
         {
            if (pageNumber > wHOStagesConditions.PageCount)
               wHOStagesConditions = query.ToPagedList(wHOStagesConditions.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(wHOStagesConditions);

      }
      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create()
      {
         try
         {
            ViewBag.WHOClinicalStage = new SelectList(context.WHOClinicalStages, FieldConstants.Oid, FieldConstants.ClinicalStage);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(WHOStagesCondition whoStagesCondition, string? module, string? parent)
      {
         try
         {
            var IsExist = IsWHOStagesConditionDuplicate(whoStagesCondition);

            if (!IsExist)
            {
               if (ModelState.IsValid)
               {
                  whoStagesCondition.CreatedBy = session?.GetCurrentAdmin().Oid;
                  whoStagesCondition.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  whoStagesCondition.DateCreated = DateTime.Now;
                  whoStagesCondition.IsDeleted = false;
                  whoStagesCondition.IsSynced = false;

                  context.WHOStagesConditions.Add(whoStagesCondition);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(whoStagesCondition);
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

         var whoStagesCondition = await context.WHOStagesConditions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (whoStagesCondition == null)
            return NotFound();

         ViewBag.WHOClinicalStage = new SelectList(context.WHOClinicalStages, FieldConstants.Oid, FieldConstants.ClinicalStage);

         return View(whoStagesCondition);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, WHOStagesCondition whoStagesCondition, string? module, string? parent)
      {
         try
         {
            var whoStageConditionModel = await context.WHOStagesConditions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == whoStagesCondition.Oid);

            bool iswhoStagesConditionDuplicate = false;

            if (whoStageConditionModel.Description != whoStagesCondition.Description)
               iswhoStagesConditionDuplicate = IsWHOStagesConditionDuplicate(whoStagesCondition);

            if (!iswhoStagesConditionDuplicate)
            {
               whoStagesCondition.DateCreated = whoStageConditionModel.DateCreated;
               whoStagesCondition.CreatedBy = whoStageConditionModel.CreatedBy;
               whoStagesCondition.ModifiedBy = session?.GetCurrentAdmin().Oid;
               whoStagesCondition.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               whoStagesCondition.DateModified = DateTime.Now;
               whoStagesCondition.IsDeleted = false;
               whoStagesCondition.IsSynced = false;

               context.WHOStagesConditions.Update(whoStagesCondition);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (iswhoStagesConditionDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(whoStagesCondition);
      }
      #endregion

      #region Read
      public bool IsWHOStagesConditionDuplicate(WHOStagesCondition whoStagesCondition)
      {
         try
         {
            var wHOClinicalStageInDb = context.WHOStagesConditions.FirstOrDefault(c => c.Description.ToLower() == whoStagesCondition.Description.ToLower() && c.IsDeleted == false);

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