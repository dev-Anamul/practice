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
   public class DrugFormulationsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public DrugFormulationsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.DrugFormulations
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var drugFormulations = query.ToPagedList(pageNumber, pageSize);

         if (drugFormulations.PageCount > 0)
         {
            if (pageNumber > drugFormulations.PageCount)
               drugFormulations = query.ToPagedList(drugFormulations.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(drugFormulations);
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
      public async Task<IActionResult> Create(DrugFormulation drugFormulation)
      {
         try
         {
            var drugFormulationInDb = IsDrugFormulationDuplicate(drugFormulation);

            if (!drugFormulationInDb)
            {
               if (ModelState.IsValid)
               {
                  drugFormulation.CreatedBy = session?.GetCurrentAdmin().Oid;
                  drugFormulation.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  drugFormulation.DateCreated = DateTime.Now;
                  drugFormulation.IsDeleted = false;
                  drugFormulation.IsSynced = false;

                  context.DrugFormulations.Add(drugFormulation);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = "Pharmacy" });
               }

               return View(drugFormulation);
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

         var drugFormulationInDb = await context.DrugFormulations.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (drugFormulationInDb == null)
            return NotFound();

         return View(drugFormulationInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, DrugFormulation drugFormulation)
      {
         try
         {
            var drugFormulationInDb = await context.DrugFormulations.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == drugFormulation.Oid);

            bool isDrugFormulationDuplicate = false;

            if (drugFormulationInDb.Description != drugFormulation.Description)
               isDrugFormulationDuplicate = IsDrugFormulationDuplicate(drugFormulation);

            if (!isDrugFormulationDuplicate)
            {
               drugFormulation.DateCreated = drugFormulationInDb.DateCreated;
               drugFormulation.CreatedBy = drugFormulationInDb.CreatedBy;
               drugFormulation.ModifiedBy = session?.GetCurrentAdmin().Oid;
               drugFormulation.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               drugFormulation.DateModified = DateTime.Now;
               drugFormulation.IsDeleted = false;
               drugFormulation.IsSynced = false;

               context.DrugFormulations.Update(drugFormulation);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Pharmacy" });
            }
            else
            {
               if (isDrugFormulationDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(drugFormulation);
      }
      #endregion

      #region Read
      public bool IsDrugFormulationDuplicate(DrugFormulation drugFormulation)
      {
         try
         {
            var drugFormulationInDb = context.DrugFormulations.FirstOrDefault(c => c.Description.ToLower() == drugFormulation.Description.ToLower() && c.IsDeleted == false);

            if (drugFormulationInDb != null)
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