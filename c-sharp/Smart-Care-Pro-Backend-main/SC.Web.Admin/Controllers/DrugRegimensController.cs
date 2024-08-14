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
   public class DrugRegimensController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public DrugRegimensController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.DrugRegimens
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var drugRegimens = query.ToPagedList(pageNumber, pageSize);

         if (drugRegimens.PageCount > 0)
         {
            if (pageNumber > drugRegimens.PageCount)
               drugRegimens = query.ToPagedList(drugRegimens.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(drugRegimens);
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
      public async Task<IActionResult> Create(DrugRegimen drugRegimen)
      {
         try
         {
            var drugRegimenInDb = IsDrugRegimenDuplicate(drugRegimen);

            if (!drugRegimenInDb)
            {
               if (ModelState.IsValid)
               {
                  drugRegimen.CreatedBy = session?.GetCurrentAdmin().Oid;
                  drugRegimen.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  drugRegimen.DateCreated = DateTime.Now;
                  drugRegimen.IsDeleted = false;
                  drugRegimen.IsSynced = false;

                  context.DrugRegimens.Add(drugRegimen);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = "Pharmacy" });
               }

               return View(drugRegimen);
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

         var drugRegimenInDb = await context.DrugRegimens.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (drugRegimenInDb == null)
            return NotFound();

         return View(drugRegimenInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, DrugRegimen drugRegimen)
      {
         try
         {
            var drugRegimenInDb = await context.DrugRegimens.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == drugRegimen.Oid);

            bool isDrugRegimenDuplicate = false;

            if (drugRegimenInDb.Description != drugRegimen.Description)
               isDrugRegimenDuplicate = IsDrugRegimenDuplicate(drugRegimen);

            if (!isDrugRegimenDuplicate)
            {
               drugRegimen.DateCreated = drugRegimenInDb.DateCreated;
               drugRegimen.CreatedBy = drugRegimenInDb.CreatedBy;
               drugRegimen.ModifiedBy = session?.GetCurrentAdmin().Oid;
               drugRegimen.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               drugRegimen.DateModified = DateTime.Now;
               drugRegimen.IsDeleted = false;
               drugRegimen.IsSynced = false;

               context.DrugRegimens.Update(drugRegimen);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Pharmacy" });
            }
            else
            {
               if (isDrugRegimenDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(drugRegimen);
      }
      #endregion

      #region Read
      public bool IsDrugRegimenDuplicate(DrugRegimen drugRegimen)
      {
         try
         {
            var drugRegimenInDb = context.DrugRegimens.FirstOrDefault(c => c.Description.ToLower() == drugRegimen.Description.ToLower() && c.IsDeleted == false);

            if (drugRegimenInDb != null)
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