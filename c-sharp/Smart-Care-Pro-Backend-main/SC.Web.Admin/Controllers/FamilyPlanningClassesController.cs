using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 22.05.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class FamilyPlanningClassesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public FamilyPlanningClassesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.FamilyPlaningClasses
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(l => l.Description);

         var familyPlanningClasses = query.ToPagedList(pageNumber, pageSize);

         if (familyPlanningClasses.PageCount > 0)
         {
            if (pageNumber > familyPlanningClasses.PageCount)
               familyPlanningClasses = query.ToPagedList(familyPlanningClasses.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(familyPlanningClasses);
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
      public async Task<IActionResult> Create(FamilyPlanningClass familyPlanningClass)
      {
         try
         {
            var familyPlanningClassInDb = IsFamilyPlanningClassDuplicate(familyPlanningClass);

            if (!familyPlanningClassInDb)
            {
               if (ModelState.IsValid)
               {
                  familyPlanningClass.CreatedBy = session?.GetCurrentAdmin().Oid;
                  familyPlanningClass.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  familyPlanningClass.DateCreated = DateTime.Now;
                  familyPlanningClass.IsDeleted = false;
                  familyPlanningClass.IsSynced = false;

                  context.FamilyPlaningClasses.Add(familyPlanningClass);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create));
               }

               return View(familyPlanningClass);
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

         var familyPlanningClassInDb = await context.FamilyPlaningClasses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (familyPlanningClassInDb == null)
            return NotFound();

         return View(familyPlanningClassInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, FamilyPlanningClass familyPlanningClass)
      {
         try
         {
            var familyPlanningClassInDb = await context.FamilyPlaningClasses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == familyPlanningClass.Oid);

            bool isFamilyPlanningClassDuplicate = false;

            if (familyPlanningClassInDb.Description != familyPlanningClass.Description)
               isFamilyPlanningClassDuplicate = IsFamilyPlanningClassDuplicate(familyPlanningClass);

            if (!isFamilyPlanningClassDuplicate)
            {
               familyPlanningClass.DateCreated = familyPlanningClassInDb.DateCreated;
               familyPlanningClass.CreatedBy = familyPlanningClassInDb.CreatedBy;
               familyPlanningClass.ModifiedBy = session?.GetCurrentAdmin().Oid;
               familyPlanningClass.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               familyPlanningClass.DateModified = DateTime.Now;
               familyPlanningClass.IsDeleted = false;
               familyPlanningClass.IsSynced = false;

               context.FamilyPlaningClasses.Update(familyPlanningClass);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index));
            }
            else
            {
               if (isFamilyPlanningClassDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(familyPlanningClass);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var familyPlanningClass = await context.FamilyPlaningClasses
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (familyPlanningClass == null)
            return NotFound();

         return View(familyPlanningClass);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var familyPlanningClass = await context.FamilyPlaningClasses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.FamilyPlaningClasses.Remove(familyPlanningClass);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsFamilyPlanningClassDuplicate(FamilyPlanningClass familyPlanningClass)
      {
         try
         {
            var familyPlanningClassInDb = context.FamilyPlaningClasses.FirstOrDefault(c => c.Description.ToLower() == familyPlanningClass.Description.ToLower() && c.IsDeleted == false);

            if (familyPlanningClassInDb != null)
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