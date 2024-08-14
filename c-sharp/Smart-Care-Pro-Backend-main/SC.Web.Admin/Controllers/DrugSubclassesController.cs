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
   public class DrugSubclassesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public DrugSubclassesController(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      #region Index
      public async Task<IActionResult> Index(string? search, int? page, int oid)
      {
         if (oid == 0)
         {
            oid = Convert.ToInt32(TempData["DrugClassId"]);
         }

         ViewBag.Oid = oid;

         var getDrugClasses = await context.DrugClasses.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

         TempData["DrugClassId"] = getDrugClasses.Oid;

         ViewBag.DrugClassId = getDrugClasses.Oid;
         ViewBag.DrugClassName = getDrugClasses.Description;

         search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;

         if (page > 0 && search == null)
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.DrugSubclasses
            .Include(d => d.DrugClass)
             .Where(x => (x.Description.ToLower().Contains(search) || search == null) && x.DrugClassId == oid && x.IsDeleted == false)
             .OrderBy(x => x.DrugClass.Description);

         var drugSubClasses = query.ToPagedList(pageNumber, pageSize);

         if (drugSubClasses.PageCount > 0)
         {
            if (pageNumber > drugSubClasses.PageCount)
               drugSubClasses = query.ToPagedList(drugSubClasses.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(drugSubClasses);
      }
      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create(int oid)
      {
         try
         {
            var getDrugClasses = await context.DrugClasses.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

            ViewBag.DrugClassId = getDrugClasses.Oid;
            ViewBag.DrugClassName = getDrugClasses.Description;
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(DrugSubclass drugSubclass)
      {
         try
         {
            var drugSubclassInDb = IsDrugSubclassDuplicate(drugSubclass);

            if (!drugSubclassInDb)
            {
               if (ModelState.IsValid)
               {
                  drugSubclass.CreatedBy = session?.GetCurrentAdmin().Oid;
                  drugSubclass.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  drugSubclass.DateCreated = DateTime.Now;
                  drugSubclass.IsDeleted = false;
                  drugSubclass.IsSynced = false;

                  context.DrugSubclasses.Add(drugSubclass);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  //return RedirectToAction("Create", new { oid = drugSubclass.DrugClassId });
                  return RedirectToAction(nameof(Create), new { oid = drugSubclass.DrugClassId, module = "Pharmacy" });
               }

               return View(drugSubclass);
            }
            else
            {
               var getDrugClasses = await context.DrugClasses.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == drugSubclass.DrugClassId);

               ViewBag.DrugClassId = getDrugClasses.Oid;
               ViewBag.DrugClassName = getDrugClasses.Description;

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

         var drugSubclassInDb = await context.DrugSubclasses.FindAsync(id);

         if (drugSubclassInDb == null)
            return NotFound();

         ViewBag.DrugClass = new SelectList(context.DrugClasses, FieldConstants.Oid, FieldConstants.Description);

         ViewBag.ClassId = drugSubclassInDb.DrugClassId;

         return View(drugSubclassInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, DrugSubclass drugSubclass)
      {
         try
         {
            var drugSubclassInDb = await context.DrugSubclasses.FindAsync(drugSubclass.Oid);
            context.Entry(drugSubclassInDb).State = EntityState.Detached;

            bool isDrugSubclassDuplicate = false;

            if (drugSubclassInDb.Description != drugSubclass.Description)
               isDrugSubclassDuplicate = IsDrugSubclassDuplicate(drugSubclass);

            if (!isDrugSubclassDuplicate)
            {
               drugSubclass.DateCreated = drugSubclassInDb.DateCreated;
               drugSubclass.CreatedBy = drugSubclassInDb.CreatedBy;
               drugSubclass.ModifiedBy = session?.GetCurrentAdmin().Oid;
               drugSubclass.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               drugSubclass.DateModified = DateTime.Now;
               drugSubclass.IsDeleted = false;
               drugSubclass.IsSynced = false;

               context.DrugSubclasses.Update(drugSubclass);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               ViewBag.ClassId = drugSubclassInDb.DrugClassId;

               return RedirectToAction(nameof(Index), new { oid = drugSubclass.DrugClassId, module = "Pharmacy" });
            }
            else
            {
               ViewBag.DrugClass = new SelectList(context.DrugClasses, FieldConstants.Oid, FieldConstants.Description);

               ViewBag.ClassId = drugSubclassInDb.DrugClassId;

               if (isDrugSubclassDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(drugSubclass);
      }
      #endregion

      #region Read
      public bool IsDrugSubclassDuplicate(DrugSubclass drugSubclass)
      {
         try
         {
            var drugSubClassInDb = context.DrugSubclasses.FirstOrDefault(c => c.Description.ToLower() == drugSubclass.Description.ToLower() && c.DrugClassId == drugSubclass.DrugClassId && c.IsDeleted == false);

            if (drugSubClassInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      [HttpGet]
      public async Task<IActionResult> GetAllDrugSubClass()
      {
         return Json(await context.DrugSubclasses.ToListAsync());
      }

      #endregion
   }
}