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
 * Date created : 09.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class SystemsRelevancesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public SystemsRelevancesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.SystemsRelevances
            .Include(p => p.PhysicalSystem).Include(d => d.GeneralDrugDefinitions).Where(x => x.PhysicalSystem.Description.ToLower().Contains(search)
            || search == null && x.IsDeleted == false).OrderBy(x => x.PhysicalSystem.Description).ToList().ToPagedList(page ?? 1, 10);

         var systemsRelevances = query.ToPagedList(pageNumber, pageSize);

         if (systemsRelevances.PageCount > 0)
         {
            if (pageNumber > systemsRelevances.PageCount)
               systemsRelevances = query.ToPagedList(systemsRelevances.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(systemsRelevances);
      }
      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create()
      {
         try
         {
            ViewBag.PhysicalSystem = new SelectList(context.PhysicalSystems, FieldConstants.Oid, FieldConstants.SystemName);
            ViewBag.DrugDefinition = new SelectList(context.GeneralDrugDefinitions, FieldConstants.Oid, FieldConstants.Description);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(SystemRelevance systemsRelevance)
      {
         try
         {
            var systemsRelevanceInDb = IsSystemsRelevanceDuplicate(systemsRelevance);

            if (!systemsRelevanceInDb)
            {
               if (ModelState.IsValid)
               {
                  systemsRelevance.CreatedBy = session?.GetCurrentAdmin().Oid;
                  systemsRelevance.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  systemsRelevance.DateCreated = DateTime.Now;
                  systemsRelevance.IsDeleted = false;
                  systemsRelevance.IsSynced = false;

                  context.SystemsRelevances.Add(systemsRelevance);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = "Pharmacy" });
               }

               return View(systemsRelevance);
            }
            else
            {
               ModelState.AddModelError("PhysicalSystemId", MessageConstants.DuplicateFound);
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

         var systemsRelevanceInDb = await context.SystemsRelevances.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (systemsRelevanceInDb == null)
            return NotFound();

         ViewBag.PhysicalSystem = new SelectList(context.PhysicalSystems, FieldConstants.Oid, FieldConstants.SystemName);
         ViewBag.DrugDefinition = new SelectList(context.GeneralDrugDefinitions, FieldConstants.Oid, FieldConstants.Description);

         return View(systemsRelevanceInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, SystemRelevance systemsRelevance)
      {
         try
         {
            var systemsRelevanceInDb = await context.SystemsRelevances.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == systemsRelevance.Oid);

            bool isSystemsRelevanceDuplicate = false;

            if (systemsRelevanceInDb.PhysicalSystemId != systemsRelevance.PhysicalSystemId)
               isSystemsRelevanceDuplicate = IsSystemsRelevanceDuplicate(systemsRelevance);

            if (!isSystemsRelevanceDuplicate)
            {
               systemsRelevance.DateCreated = systemsRelevanceInDb.DateCreated;
               systemsRelevance.CreatedBy = systemsRelevanceInDb.CreatedBy;
               systemsRelevance.ModifiedBy = session?.GetCurrentAdmin().Oid;
               systemsRelevance.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               systemsRelevance.DateModified = DateTime.Now;
               systemsRelevance.IsDeleted = false;
               systemsRelevance.IsSynced = false;

               context.SystemsRelevances.Update(systemsRelevance);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Pharmacy" });
            }
            else
            {
               if (isSystemsRelevanceDuplicate)
                  ModelState.AddModelError("PhysicalSystemId", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(systemsRelevance);
      }
      #endregion

      #region Read
      public bool IsSystemsRelevanceDuplicate(SystemRelevance systemsRelevance)
      {
         try
         {
            var systemsRelevanceInDb = context.SystemsRelevances.FirstOrDefault(c => c.PhysicalSystemId == systemsRelevance.PhysicalSystemId && c.DrugDefinitionId == systemsRelevance.DrugDefinitionId && c.IsDeleted == false);

            if (systemsRelevanceInDb != null)
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