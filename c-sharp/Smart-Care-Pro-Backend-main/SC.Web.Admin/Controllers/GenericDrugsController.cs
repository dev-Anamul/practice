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
   public class GenericDrugsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public GenericDrugsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.GenericDrugs
             .Include(d => d.GeneralDrugDefinitions)
             .Include(s => s.DrugSubclass)
             .ThenInclude(c => c.DrugClass)
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var genericDrugs = query.ToPagedList(pageNumber, pageSize);

         if (genericDrugs.PageCount > 0)
         {
            if (pageNumber > genericDrugs.PageCount)
               genericDrugs = query.ToPagedList(genericDrugs.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(genericDrugs);
      }
      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create()
      {
         try
         {
            ViewBag.Drugclass = new SelectList(context.DrugClasses, FieldConstants.Oid, FieldConstants.Description);
            ViewBag.DrugSubclass = new SelectList(context.DrugSubclasses, FieldConstants.Oid, FieldConstants.Description);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(GenericDrug genericDrug, string? module)
      {
         try
         {
            var genericDrugInDb = IsGenericDrugDuplicate(genericDrug);

            if (!genericDrugInDb)
            {
               if (ModelState.IsValid)
               {
                  genericDrug.CreatedBy = session?.GetCurrentAdmin().Oid;
                  genericDrug.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  genericDrug.DateCreated = DateTime.Now;
                  genericDrug.IsDeleted = false;
                  genericDrug.IsSynced = false;

                  context.GenericDrugs.Add(genericDrug);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create));
               }

               return View(genericDrug);
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

         var genericDrug = await context.GenericDrugs.Include(t => t.DrugSubclass).ThenInclude(s => s.DrugClass).AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);
         var drugSubClass = context.DrugSubclasses.ToList();

         if (genericDrug == null)
            return NotFound();

         ViewBag.DrugClass = new SelectList(context.DrugClasses, FieldConstants.Oid, FieldConstants.Description, genericDrug.DrugSubclass.DrugClass.Oid);
         ViewBag.DrugSubClass = new SelectList(drugSubClass.Where(d => d.DrugClassId == genericDrug.DrugSubclass.DrugClassId).ToList(), FieldConstants.Oid, FieldConstants.Description, genericDrug.DrugSubclass);

         return View(genericDrug);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, GenericDrug genericDrug, string? module)
      {
         try
         {
            var genericDrugInDb = await context.GenericDrugs.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == genericDrug.Oid);

            bool isGenericDrugDuplicate = false;

            if (genericDrugInDb.Description != genericDrug.Description)
               isGenericDrugDuplicate = IsGenericDrugDuplicate(genericDrug);

            if (!isGenericDrugDuplicate)
            {
               genericDrug.DateCreated = genericDrugInDb.DateCreated;
               genericDrug.CreatedBy = genericDrugInDb.CreatedBy;
               genericDrug.ModifiedBy = session?.GetCurrentAdmin().Oid;
               genericDrug.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               genericDrug.DateModified = DateTime.Now;
               genericDrug.IsDeleted = false;
               genericDrug.IsSynced = false;

               context.GenericDrugs.Update(genericDrug);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "module" });
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

         return View(genericDrug);
      }
      #endregion

      #region Read
      public bool IsGenericDrugDuplicate(GenericDrug genericDrug)
      {
         try
         {
            var specialDrugInDb = context.GenericDrugs.FirstOrDefault(c => c.Description.ToLower() == genericDrug.Description.ToLower() && c.IsDeleted == false);

            if (specialDrugInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      [HttpGet]
      public async Task<IActionResult> GetAllGenericDrugs()
      {
         return Json(await context.GenericDrugs.ToListAsync());
      }

      [HttpGet]
      public async Task<IActionResult> GetAllDrugClass()
      {
         return Json(await context.DrugClasses.ToListAsync());
      }

      public JsonResult LoadDrugSubClass(int id)
      {
         var subClass = context.DrugSubclasses.Where(p => p.DrugClassId == id && p.IsDeleted == false).ToList();
         return Json(subClass);
      }

      #endregion
   }
}