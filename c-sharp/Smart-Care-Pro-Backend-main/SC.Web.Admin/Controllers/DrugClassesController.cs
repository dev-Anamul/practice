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
   public class DrugClassesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public DrugClassesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.DrugClasses
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var drugClasses = query.ToPagedList(pageNumber, pageSize);

         if (drugClasses.PageCount > 0)
         {
            if (pageNumber > drugClasses.PageCount)
               drugClasses = query.ToPagedList(drugClasses.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(drugClasses);
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
      public async Task<IActionResult> Create(DrugClass drugClass)
      {
         try
         {
            var drugClassInDb = IsDrugClassDuplicate(drugClass);

            if (!drugClassInDb)
            {
               if (ModelState.IsValid)
               {
                  drugClass.CreatedBy = session?.GetCurrentAdmin().Oid;
                  drugClass.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  drugClass.DateCreated = DateTime.Now;
                  drugClass.IsDeleted = false;
                  drugClass.IsSynced = false;

                  context.DrugClasses.Add(drugClass);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = "Pharmacy" });
               }

               return View(drugClass);
            }
            else
            {
               ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }

            return View();
         }
         catch (Exception)
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

         var drugClassInDb = await context.DrugClasses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (drugClassInDb == null)
            return NotFound();

         return View(drugClassInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, DrugClass drugClass)
      {
         try
         {
            var drugClassInDb = await context.DrugClasses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == drugClass.Oid);

            bool isDrugClassDuplicate = false;

            if (drugClassInDb.Description != drugClass.Description)
               isDrugClassDuplicate = IsDrugClassDuplicate(drugClass);

            if (!isDrugClassDuplicate)
            {
               drugClass.DateCreated = drugClassInDb.DateCreated;
               drugClass.CreatedBy = drugClassInDb.CreatedBy;
               drugClass.ModifiedBy = session?.GetCurrentAdmin().Oid;
               drugClass.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               drugClass.DateModified = DateTime.Now;
               drugClass.IsDeleted = false;
               drugClass.IsSynced = false;

               context.DrugClasses.Update(drugClass);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Pharmacy" });
            }
            else
            {
               if (isDrugClassDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(drugClass);
      }
      #endregion

      #region Read
      public bool IsDrugClassDuplicate(DrugClass drugClass)
      {
         try
         {
            var drugClassInDb = context.DrugClasses.FirstOrDefault(c => c.Description.ToLower() == drugClass.Description.ToLower() && c.IsDeleted == false);

            if (drugClassInDb != null)
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