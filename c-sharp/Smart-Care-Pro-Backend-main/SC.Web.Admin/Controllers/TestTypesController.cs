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
   public class TestTypesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public TestTypesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.TestTypes
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var testTypes = query.ToPagedList(pageNumber, pageSize);

         if (testTypes.PageCount > 0)
         {
            if (pageNumber > testTypes.PageCount)
               testTypes = query.ToPagedList(testTypes.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(testTypes);
      }
      #endregion

      #region Create
      [HttpGet]
      public IActionResult Create()
      {
         try
         {
            ViewBag.TestType = new SelectList(context.TestTypes, FieldConstants.Oid, FieldConstants.Description);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(TestType testType)
      {
         try
         {
            var testTypeInDb = IsTestTypeDuplicate(testType);

            if (!testTypeInDb)
            {
               if (ModelState.IsValid)
               {
                  testType.CreatedBy = session?.GetCurrentAdmin().Oid;
                  testType.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  testType.DateCreated = DateTime.Now;
                  testType.IsDeleted = false;
                  testType.IsSynced = false;

                  context.TestTypes.Add(testType);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = "Investigation" });
               }

               return View(testType);
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

         var testTypeInDb = await context.TestTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (testTypeInDb == null)
            return NotFound();

         return View(testTypeInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, TestType testType)
      {
         try
         {
            var testTypeInDb = await context.TestTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == testType.Oid);

            bool istestTypeDuplicate = false;

            if (testTypeInDb.Description != testType.Description)
               istestTypeDuplicate = IsTestTypeDuplicate(testType);

            if (!istestTypeDuplicate)
            {
               testType.DateCreated = testTypeInDb.DateCreated;
               testType.CreatedBy = testTypeInDb.CreatedBy;
               testType.ModifiedBy = session?.GetCurrentAdmin().Oid;
               testType.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               testType.DateModified = DateTime.Now;
               testType.IsDeleted = false;
               testType.IsSynced = false;

               context.TestTypes.Update(testType);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Investigation" });
            }
            else
            {
               if (istestTypeDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(testType);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var testType = await context.TestTypes
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (testType == null)
            return NotFound();

         return View(testType);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var testType = await context.TestTypes.FindAsync(id);

         context.TestTypes.Remove(testType);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsTestTypeDuplicate(TestType testType)
      {
         try
         {
            var testTypeInDb = context.TestTypes.FirstOrDefault(p => p.Description.ToLower() == testType.Description.ToLower() && p.IsDeleted == false);

            if (testTypeInDb != null)
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