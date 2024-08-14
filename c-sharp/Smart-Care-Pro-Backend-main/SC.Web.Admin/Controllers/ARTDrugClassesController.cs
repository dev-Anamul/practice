using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 01.04.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class ARTDrugClassesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ARTDrugClassesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.ARTDrugClasses
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var artDrugClasses = query.ToPagedList(pageNumber, pageSize);

         if (artDrugClasses.PageCount > 0)
         {
            if (pageNumber > artDrugClasses.PageCount)
               artDrugClasses = query.ToPagedList(artDrugClasses.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(artDrugClasses);
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
      public async Task<IActionResult> Create(ARTDrugClass artDrugClass, string? module, string? parent)
      {
         try
         {
            var artDrugClassInDb = IsARTDrugClassDuplicate(artDrugClass);

            if (!artDrugClassInDb)
            {
               if (ModelState.IsValid)
               {
                  artDrugClass.CreatedBy = session?.GetCurrentAdmin().Oid;
                  artDrugClass.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  artDrugClass.DateCreated = DateTime.Now;
                  artDrugClass.IsDeleted = false;
                  artDrugClass.IsSynced = false;

                  context.ARTDrugClasses.Add(artDrugClass);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(artDrugClass);
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

         var artDrugClass = await context.ARTDrugClasses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (artDrugClass == null)
            return NotFound();

         return View(artDrugClass);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ARTDrugClass artDrugClass, string? module, string? parent)
      {
         try
         {
            var artDrugClassInDb = await context.ARTDrugClasses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == artDrugClass.Oid);

            bool isARTDrugClassDuplicate = false;

            if (artDrugClassInDb.Description != artDrugClass.Description)
               isARTDrugClassDuplicate = IsARTDrugClassDuplicate(artDrugClass);

            if (!isARTDrugClassDuplicate)
            {
               artDrugClass.DateCreated = artDrugClassInDb.DateCreated;
               artDrugClass.CreatedBy = artDrugClassInDb.CreatedBy;
               artDrugClass.ModifiedBy = session?.GetCurrentAdmin().Oid;
               artDrugClass.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               artDrugClass.DateModified = DateTime.Now;
               artDrugClass.IsDeleted = false;
               artDrugClass.IsSynced = false;

               context.ARTDrugClasses.Update(artDrugClass);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isARTDrugClassDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(artDrugClass);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var artDrugClass = await context.ARTDrugClasses
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (artDrugClass == null)
            return NotFound();

         return View(artDrugClass);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var artDrugClass = await context.ARTDrugClasses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.ARTDrugClasses.Remove(artDrugClass);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsARTDrugClassDuplicate(ARTDrugClass artDrugClass)
      {
         try
         {
            var artDrugClassInDb = context.ARTDrugClasses.FirstOrDefault(c => c.Description.ToLower() == artDrugClass.Description.ToLower() && c.IsDeleted == false);

            if (artDrugClassInDb != null)
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