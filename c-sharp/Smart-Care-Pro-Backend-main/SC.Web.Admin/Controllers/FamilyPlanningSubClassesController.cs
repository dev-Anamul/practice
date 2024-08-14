using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Brian
 * Date created : o4.05.2023
 * Modified by  : Bella
 * Last modified: 22.05.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class FamilyPlanningSubClassesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public FamilyPlanningSubClassesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.FamilyPlanningSubclasses
             .Include(d => d.FamilyPlanningClass)
             .Where(x => x.Description.ToLower().Contains(search) || x.FamilyPlanningClass.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.FamilyPlanningClass.Description);

         var subclasses = query.ToPagedList(pageNumber, pageSize);

         if (subclasses.PageCount > 0)
         {
            if (pageNumber > subclasses.PageCount)
               subclasses = query.ToPagedList(subclasses.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(subclasses);
      }

      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create()
      {
         try
         {
            ViewBag.FamilyPlanningClass = new SelectList(context.FamilyPlaningClasses, FieldConstants.Oid, FieldConstants.Description);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(FamilyPlanningSubclass familyPlanningSubClass)
      {
         try
         {
            var familyPlanningClassInDb = IsFamilyPlanningSubClassDuplicate(familyPlanningSubClass);

            if (!familyPlanningClassInDb)
            {
               if (ModelState.IsValid)
               {
                  familyPlanningSubClass.CreatedBy = session?.GetCurrentAdmin().Oid;
                  familyPlanningSubClass.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  familyPlanningSubClass.DateCreated = DateTime.Now;
                  familyPlanningSubClass.IsDeleted = false;
                  familyPlanningSubClass.IsSynced = false;

                  context.FamilyPlanningSubclasses.Add(familyPlanningSubClass);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create));
               }

               return View(familyPlanningSubClass);
            }
            else
            {
               ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
               ViewBag.FamilyPlanningClass = new SelectList(context.FamilyPlaningClasses, FieldConstants.Oid, FieldConstants.Description);
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

         var familyPlanningSubClassInDb = await context.FamilyPlanningSubclasses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (familyPlanningSubClassInDb == null)
            return NotFound();

         ViewBag.FamilyPlanningClass = new SelectList(context.FamilyPlaningClasses, FieldConstants.Oid, FieldConstants.Description);

         return View(familyPlanningSubClassInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, FamilyPlanningSubclass familyPlanningSubClass)
      {
         try
         {
            var familyPlanningSubClassInDb = await context.FamilyPlanningSubclasses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == familyPlanningSubClass.Oid);

            bool isFamilyPlanningSubClassDuplicate = false;

            if (familyPlanningSubClassInDb.Description != familyPlanningSubClass.Description)
               isFamilyPlanningSubClassDuplicate = IsFamilyPlanningSubClassDuplicate(familyPlanningSubClass);

            if (!isFamilyPlanningSubClassDuplicate)
            {
               familyPlanningSubClass.DateCreated = familyPlanningSubClassInDb.DateCreated;
               familyPlanningSubClass.CreatedBy = familyPlanningSubClassInDb.CreatedBy;
               familyPlanningSubClass.ModifiedBy = session?.GetCurrentAdmin().Oid;
               familyPlanningSubClass.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               familyPlanningSubClass.DateModified = DateTime.Now;
               familyPlanningSubClass.IsDeleted = false;
               familyPlanningSubClass.IsSynced = false;

               context.FamilyPlanningSubclasses.Update(familyPlanningSubClass);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index));
            }
            else
            {
               if (isFamilyPlanningSubClassDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);

               ViewBag.FamilyPlanningClass = new SelectList(context.FamilyPlaningClasses, FieldConstants.Oid, FieldConstants.Description);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(familyPlanningSubClass);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var familyPlanningSubClass = await context.FamilyPlanningSubclasses
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (familyPlanningSubClass == null)
            return NotFound();

         return View(familyPlanningSubClass);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var familyPlanningSubClass = await context.FamilyPlanningSubclasses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.FamilyPlanningSubclasses.Remove(familyPlanningSubClass);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsFamilyPlanningSubClassDuplicate(FamilyPlanningSubclass familyPlanningSubClass)
      {
         try
         {
            var familyPlanningSubClassInDb = context.FamilyPlanningSubclasses.FirstOrDefault(c => c.Description.ToLower() == familyPlanningSubClass.Description.ToLower() && c.IsDeleted == false);

            if (familyPlanningSubClassInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      [HttpGet]
      public async Task<IActionResult> familyPlanningSubClassByFamilyPlanningClass(int ClassId)
      {
         return Json(await context.FamilyPlanningSubclasses.Where(d => d.IsDeleted == false && d.ClassId == ClassId).ToListAsync());
      }
      #endregion
   }
}