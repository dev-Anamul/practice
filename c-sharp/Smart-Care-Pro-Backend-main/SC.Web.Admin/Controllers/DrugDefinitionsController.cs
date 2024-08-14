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
   public class DrugDefinitionsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public DrugDefinitionsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.GeneralDrugDefinitions
            .Include(d => d.DrugDosageUnit).Include(f => f.DrugFormulation).Include(u => u.DrugUtility).Include(g => g.GenericDrug)
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var drugDefinitions = query.ToPagedList(pageNumber, pageSize);

         if (drugDefinitions.PageCount > 0)
         {
            if (pageNumber > drugDefinitions.PageCount)
               drugDefinitions = query.ToPagedList(drugDefinitions.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(drugDefinitions);
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
            ViewBag.DrugUtilities = new SelectList(context.DrugUtilities, FieldConstants.Oid, FieldConstants.Description);
            ViewBag.GenericDrugs = new SelectList(context.GenericDrugs, FieldConstants.Oid, FieldConstants.Description);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(GeneralDrugDefinition drugDefinition)
      {
         try
         {
            var drugDefinitionInDb = IsGenericDrugDuplicate(drugDefinition);

            if (!drugDefinitionInDb)
            {
               if (ModelState.IsValid)
               {
                  drugDefinition.CreatedBy = session?.GetCurrentAdmin().Oid;
                  drugDefinition.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  drugDefinition.DateCreated = DateTime.Now;
                  drugDefinition.IsDeleted = false;
                  drugDefinition.IsSynced = false;

                  context.GeneralDrugDefinitions.Add(drugDefinition);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = "Pharmacy" });
               }

               return View(drugDefinition);
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

         var drugDefinitionInDb = await context.GeneralDrugDefinitions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (drugDefinitionInDb == null)
            return NotFound();

         ViewBag.DosageUnits = new SelectList(context.DrugDosageUnites, FieldConstants.Oid, FieldConstants.Description);
         ViewBag.DrugFormulations = new SelectList(context.DrugFormulations, FieldConstants.Oid, FieldConstants.Description);
         ViewBag.DrugUtilities = new SelectList(context.DrugUtilities, FieldConstants.Oid, FieldConstants.Description);
         ViewBag.GenericDrugs = new SelectList(context.GenericDrugs, FieldConstants.Oid, FieldConstants.Description);

         return View(drugDefinitionInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, GeneralDrugDefinition drugDefinition)
      {
         try
         {
            var drugDefinitionInDb = await context.GeneralDrugDefinitions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == drugDefinition.Oid);

            bool isGenericDrugDuplicate = false;

            if (drugDefinitionInDb.Description != drugDefinition.Description)
               isGenericDrugDuplicate = IsGenericDrugDuplicate(drugDefinition);

            if (!isGenericDrugDuplicate)
            {
               drugDefinition.DateCreated = drugDefinitionInDb.DateCreated;
               drugDefinition.CreatedBy = drugDefinitionInDb.CreatedBy;
               drugDefinition.ModifiedBy = session?.GetCurrentAdmin().Oid;
               drugDefinition.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               drugDefinition.DateModified = DateTime.Now;
               drugDefinition.IsDeleted = false;
               drugDefinition.IsSynced = false;

               context.GeneralDrugDefinitions.Update(drugDefinition);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Pharmacy" });
            }
            else
            {
               if (isGenericDrugDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(drugDefinition);
      }
      #endregion

      #region Read
      public bool IsGenericDrugDuplicate(GeneralDrugDefinition drugDefinition)
      {
         try
         {
            var drugDefinitionInDb = context.GeneralDrugDefinitions.FirstOrDefault(c => c.Description.ToLower() == drugDefinition.Description.ToLower() && c.IsDeleted == false);

            if (drugDefinitionInDb != null)
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