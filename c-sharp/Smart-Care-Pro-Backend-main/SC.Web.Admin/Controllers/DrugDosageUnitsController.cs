using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
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
   public class DrugDosageUnitsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public DrugDosageUnitsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.DrugDosageUnites
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var drugDosageUnites = query.ToPagedList(pageNumber, pageSize);

         if (drugDosageUnites.PageCount > 0)
         {
            if (pageNumber > drugDosageUnites.PageCount)
               drugDosageUnites = query.ToPagedList(drugDosageUnites.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(drugDosageUnites);
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
      public async Task<IActionResult> Create(DrugDosageUnit drugDosageUnit)
      {
         try
         {
            var drugDosageUnitInDb = IsDrugDosageUnitDuplicate(drugDosageUnit);

            if (!drugDosageUnitInDb)
            {
               if (ModelState.IsValid)
               {
                  drugDosageUnit.CreatedBy = session?.GetCurrentAdmin().Oid;
                  drugDosageUnit.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  drugDosageUnit.DateCreated = DateTime.Now;
                  drugDosageUnit.IsDeleted = false;
                  drugDosageUnit.IsSynced = false;

                  context.DrugDosageUnites.Add(drugDosageUnit);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = "Pharmacy" });
               }

               return View(drugDosageUnit);
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

         var drugDosageUnitInDb = await context.DrugDosageUnites.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (drugDosageUnitInDb == null)
            return NotFound();

         return View(drugDosageUnitInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, DrugDosageUnit drugDosageUnit)
      {
         try
         {
            var drugDosageUnitInDb = await context.DrugDosageUnites.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == drugDosageUnit.Oid);

            bool isDrugDosageUnitDuplicate = false;

            if (drugDosageUnitInDb.Description != drugDosageUnit.Description)
               isDrugDosageUnitDuplicate = IsDrugDosageUnitDuplicate(drugDosageUnit);

            if (!isDrugDosageUnitDuplicate)
            {
               drugDosageUnit.DateCreated = drugDosageUnitInDb.DateCreated;
               drugDosageUnit.CreatedBy = drugDosageUnitInDb.CreatedBy;
               drugDosageUnit.ModifiedBy = session?.GetCurrentAdmin().Oid;
               drugDosageUnit.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               drugDosageUnit.DateModified = DateTime.Now;
               drugDosageUnit.IsDeleted = false;
               drugDosageUnit.IsSynced = false;

               context.DrugDosageUnites.Update(drugDosageUnit);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Pharmacy" });
            }
            else
            {
               if (isDrugDosageUnitDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(drugDosageUnit);
      }
      #endregion

      #region Read
      public bool IsDrugDosageUnitDuplicate(DrugDosageUnit drugDosageUnit)
      {
         try
         {
            var drugDosageUnitInDb = context.DrugDosageUnites.FirstOrDefault(c => c.Description.ToLower() == drugDosageUnit.Description.ToLower() && c.IsDeleted == false);

            if (drugDosageUnitInDb != null)
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