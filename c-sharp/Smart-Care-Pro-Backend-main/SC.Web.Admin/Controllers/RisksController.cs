using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 27.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class RisksController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public RisksController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.Risks
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var risks = query.ToPagedList(pageNumber, pageSize);

         if (risks.PageCount > 0)
         {
            if (pageNumber > risks.PageCount)
               risks = query.ToPagedList(risks.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(risks);
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
      public async Task<IActionResult> Create(Risks risk, string? module, string? parent)
      {
         try
         {
            var riskInDb = IsRiskDuplicate(risk);

            if (!riskInDb)
            {
               if (ModelState.IsValid)
               {
                  risk.CreatedBy = session?.GetCurrentAdmin().Oid;
                  risk.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  risk.DateCreated = DateTime.Now;
                  risk.IsDeleted = false;
                  risk.IsSynced = false;

                  context.Risks.Add(risk);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(risk);
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

         var pepRisk = await context.Risks.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (pepRisk == null)
            return NotFound();

         return View(pepRisk);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, Risks risk, string? module, string? parent)
      {
         try
         {
            var riskInDb = await context.Risks.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == risk.Oid);

            bool isRiskDuplicate = false;

            if (riskInDb.Description != risk.Description)
               isRiskDuplicate = IsRiskDuplicate(risk);

            if (!isRiskDuplicate)
            {
               risk.DateCreated = riskInDb.DateCreated;
               risk.CreatedBy = riskInDb.CreatedBy;
               risk.ModifiedBy = session?.GetCurrentAdmin().Oid;
               risk.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               risk.DateModified = DateTime.Now;
               risk.IsDeleted = false;
               risk.IsSynced = false;

               context.Risks.Update(risk);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isRiskDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(risk);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var risk = await context.Risks
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (risk == null)
            return NotFound();

         return View(risk);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var pepRisk = await context.Risks.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.Risks.Remove(pepRisk);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsRiskDuplicate(Risks risk)
      {
         try
         {
            var riskInDb = context.Risks.FirstOrDefault(c => c.Description.ToLower() == risk.Description.ToLower() && c.IsDeleted == false);

            if (riskInDb != null)
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