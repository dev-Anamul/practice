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
   public class SpecialDrugsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public SpecialDrugsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.SpecialDrugs
            .Include(s => s.DrugDosageUnit)
            .Include(f => f.DrugFormulation)
            .Include(r => r.DrugRegimen)
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var specialDrugs = query.ToPagedList(pageNumber, pageSize);

         if (specialDrugs.PageCount > 0)
         {
            if (pageNumber > specialDrugs.PageCount)
               specialDrugs = query.ToPagedList(specialDrugs.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(specialDrugs);
      }
      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create()
      {
         try
         {
            ViewBag.DosageUnits = new SelectList(context.DrugDosageUnites, FieldConstants.Oid, FieldConstants.Description);
            ViewBag.DrugFormulations = new SelectList(context.DrugFormulations, FieldConstants.Oid, FieldConstants.Description);
            ViewBag.DrugRegimens = new SelectList(context.DrugRegimens, FieldConstants.Oid, FieldConstants.Description);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(SpecialDrug specialDrug, string? module, string? parent)
      {
         try
         {
            var specialDrugInDb = IsSpecialDrugDuplicate(specialDrug);

            if (!specialDrugInDb)
            {
               if (ModelState.IsValid)
               {
                  specialDrug.CreatedBy = session?.GetCurrentAdmin().Oid;
                  specialDrug.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  specialDrug.DateCreated = DateTime.Now;
                  specialDrug.IsDeleted = false;
                  specialDrug.IsSynced = false;

                  context.SpecialDrugs.Add(specialDrug);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(specialDrug);
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

         var specialDrugInDb = await context.SpecialDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (specialDrugInDb == null)
            return NotFound();

         ViewBag.DosageUnits = new SelectList(context.DrugDosageUnites, FieldConstants.Oid, FieldConstants.Description);
         ViewBag.DrugFormulations = new SelectList(context.DrugFormulations, FieldConstants.Oid, FieldConstants.Description);
         ViewBag.DrugRegimens = new SelectList(context.DrugRegimens, FieldConstants.Oid, FieldConstants.Description);

         return View(specialDrugInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, SpecialDrug specialDrug, string? module, string? parent)
      {
         try
         {
            var specialDrugInDb = await context.SpecialDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == specialDrug.Oid);

            bool isSpecialDrugDuplicate = false;

            if (specialDrugInDb.Description != specialDrug.Description)
               isSpecialDrugDuplicate = IsSpecialDrugDuplicate(specialDrug);

            if (!isSpecialDrugDuplicate)
            {
               specialDrug.DateCreated = specialDrugInDb.DateCreated;
               specialDrug.CreatedBy = specialDrugInDb.CreatedBy;
               specialDrug.ModifiedBy = session?.GetCurrentAdmin().Oid;
               specialDrug.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               specialDrug.DateModified = DateTime.Now;
               specialDrug.IsDeleted = false;
               specialDrug.IsSynced = false;

               context.SpecialDrugs.Update(specialDrug);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isSpecialDrugDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(specialDrug);
      }
      #endregion

      #region Read
      public bool IsSpecialDrugDuplicate(SpecialDrug specialDrug)
      {
         try
         {
            var specialDrugInDb = context.SpecialDrugs.FirstOrDefault(c => c.Description.ToLower() == specialDrug.Description.ToLower() && c.Strength == specialDrug.Strength && c.IsDeleted == false);

            if (specialDrugInDb != null)
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