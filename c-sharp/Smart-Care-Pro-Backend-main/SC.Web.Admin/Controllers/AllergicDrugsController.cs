using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 22.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class AllergicDrugsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public AllergicDrugsController(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      #region Index

      public IActionResult Index(string search, int? page)
      {
         search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;

         if (page > 0 && search == null)
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.AllergicDrugs
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var allergicDrugs = query.ToPagedList(pageNumber, pageSize);

         if (allergicDrugs.PageCount > 0)
         {
            if (pageNumber > allergicDrugs.PageCount)
               allergicDrugs = query.ToPagedList(allergicDrugs.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(allergicDrugs);
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
      public async Task<IActionResult> Create(AllergicDrug allergicDrug, string? module, string? parent)
      {
         try
         {
            var allergicDrugInDb = IsAllergicDrugDuplicate(allergicDrug);

            if (!allergicDrugInDb)
            {
               if (ModelState.IsValid)
               {
                  allergicDrug.CreatedBy = session?.GetCurrentAdmin().Oid;
                  allergicDrug.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  allergicDrug.DateCreated = DateTime.Now;
                  allergicDrug.IsDeleted = false;
                  allergicDrug.IsSynced = false;

                  context.AllergicDrugs.Add(allergicDrug);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(allergicDrug);
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

         var allergicDrug = await context.AllergicDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (allergicDrug == null)
            return NotFound();

         return View(allergicDrug);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, AllergicDrug allergicDrug, string? module, string? parent)
      {
         try
         {
            var allergicDrugInDb = await context.AllergicDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == allergicDrug.Oid);

            bool isAllergicDrugDuplicate = false;

            if (allergicDrugInDb.Description != allergicDrug.Description)
               isAllergicDrugDuplicate = IsAllergicDrugDuplicate(allergicDrug);

            if (!isAllergicDrugDuplicate)
            {
               allergicDrug.DateCreated = allergicDrugInDb.DateCreated;
               allergicDrug.CreatedBy = allergicDrugInDb.CreatedBy;
               allergicDrug.ModifiedBy = session?.GetCurrentAdmin().Oid;
               allergicDrug.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               allergicDrug.DateModified = DateTime.Now;
               allergicDrug.IsDeleted = false;
               allergicDrug.IsSynced = false;

               context.AllergicDrugs.Update(allergicDrug);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isAllergicDrugDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(allergicDrug);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var allergicDrug = await context.AllergicDrugs
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (allergicDrug == null)
            return NotFound();

         return View(allergicDrug);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var allergicDrug = await context.AllergicDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.AllergicDrugs.Remove(allergicDrug);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsAllergicDrugDuplicate(AllergicDrug allergicDrug)
      {
         try
         {
            var allergicDrugInDb = context.AllergicDrugs.FirstOrDefault(c => c.Description.ToLower() == allergicDrug.Description.ToLower() && c.IsDeleted == false);

            if (allergicDrugInDb != null)
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