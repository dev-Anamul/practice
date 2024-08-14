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
   public class DrugUtilitiesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public DrugUtilitiesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.DrugUtilities
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var drugUtilities = query.ToPagedList(pageNumber, pageSize);

         if (drugUtilities.PageCount > 0)
         {
            if (pageNumber > drugUtilities.PageCount)
               drugUtilities = query.ToPagedList(drugUtilities.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(drugUtilities);
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
      public async Task<IActionResult> Create(DrugUtility drugUtility)
      {
         try
         {
            var drugUtilityInDb = IsDrugUtilityDuplicate(drugUtility);

            if (!drugUtilityInDb)
            {
               if (ModelState.IsValid)
               {
                  drugUtility.CreatedBy = session?.GetCurrentAdmin().Oid;
                  drugUtility.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  drugUtility.DateCreated = DateTime.Now;
                  drugUtility.IsDeleted = false;
                  drugUtility.IsSynced = false;

                  context.DrugUtilities.Add(drugUtility);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = "Pharmacy" });
               }

               return View(drugUtility);
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

         var drugUtilityInDb = await context.DrugUtilities.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (drugUtilityInDb == null)
            return NotFound();

         return View(drugUtilityInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, DrugUtility drugUtility)
      {
         try
         {
            var drugUtilityInDb = await context.DrugUtilities.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == drugUtility.Oid);

            bool isDrugUtilityDuplicate = false;

            if (drugUtilityInDb.Description != drugUtility.Description)
               isDrugUtilityDuplicate = IsDrugUtilityDuplicate(drugUtility);

            if (!isDrugUtilityDuplicate)
            {
               drugUtility.DateCreated = drugUtilityInDb.DateCreated;
               drugUtility.CreatedBy = drugUtilityInDb.CreatedBy;
               drugUtility.ModifiedBy = session?.GetCurrentAdmin().Oid;
               drugUtility.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               drugUtility.DateModified = DateTime.Now;
               drugUtility.IsDeleted = false;
               drugUtility.IsSynced = false;

               context.DrugUtilities.Update(drugUtility);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Pharmacy" });
            }
            else
            {
               if (isDrugUtilityDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(drugUtility);
      }
      #endregion

      #region Read
      public bool IsDrugUtilityDuplicate(DrugUtility drugUtility)
      {
         try
         {
            var drugUtilityInDb = context.DrugUtilities.FirstOrDefault(c => c.Description.ToLower() == drugUtility.Description.ToLower() && c.IsDeleted == false);

            if (drugUtilityInDb != null)
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