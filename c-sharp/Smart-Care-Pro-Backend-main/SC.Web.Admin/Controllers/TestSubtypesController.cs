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
   public class TestSubtypesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public TestSubtypesController(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      #region Index
      public async Task<IActionResult> Index(string? search, int? page, int oid)
      {
         if (oid == 0)
         {
            oid = Convert.ToInt32(TempData["TestTypeId"]);
         }

         ViewBag.Oid = oid;

         var getTestType = await context.TestTypes.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

         TempData["TestTypeId"] = getTestType.Oid;

         ViewBag.TestTypeId = oid;
         ViewBag.TestSubType = getTestType.Description;

         search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;
         if (page > 0 && search == null)
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.TestSubTypes
            .Include(t => t.TestType)
            .Where(x => (x.Description.ToLower().Contains(search) || x.TestType.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false) && x.TestTypeId == oid)
            .OrderBy(x => x.TestType.Description);

         var testSubTypes = query.ToPagedList(pageNumber, pageSize);

         if (testSubTypes.PageCount > 0)
         {
            if (pageNumber > testSubTypes.PageCount)
               testSubTypes = query.ToPagedList(testSubTypes.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(testSubTypes);
      }
      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create(int oid)
      {
         try
         {
            var getTestType = await context.TestTypes.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

            ViewBag.TestTypeId = oid;
            ViewBag.TestSubType = getTestType.Description;
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(TestSubtype testSubType)
      {
         try
         {
            var testSubTypeInDb = IsTestSubTypeDuplicate(testSubType);

            if (!testSubTypeInDb)
            {
               if (ModelState.IsValid)
               {
                  testSubType.CreatedBy = session?.GetCurrentAdmin().Oid;
                  testSubType.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  testSubType.DateCreated = DateTime.Now;
                  testSubType.IsDeleted = false;
                  testSubType.IsSynced = false;

                  context.TestSubTypes.Add(testSubType);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction("Create", new { oid = testSubType.TestTypeId, module = "Investigation" });
                  //return RedirectToAction(nameof(Create), new { module = "HTS" });
               }

               return View(testSubType);
            }
            else
            {
               var getTestType = await context.TestTypes.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == testSubType.TestTypeId);

               ViewBag.TestTypeId = getTestType.Oid;
               ViewBag.TestSubType = getTestType.Description;

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

         var testSubTypeInDb = await context.TestSubTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (testSubTypeInDb == null)
            return NotFound();

         ViewBag.TestType = new SelectList(context.TestTypes, FieldConstants.Oid, FieldConstants.Description);
         ViewBag.TestTypeId = testSubTypeInDb.TestTypeId;

         return View(testSubTypeInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, TestSubtype testSubType)
      {
         try
         {
            var testSubTypeInDb = await context.TestSubTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == testSubType.Oid);

            bool isTestSubTypeDuplicate = false;

            if (testSubTypeInDb.Description != testSubType.Description)
               isTestSubTypeDuplicate = IsTestSubTypeDuplicate(testSubType);

            if (!isTestSubTypeDuplicate)
            {
               testSubType.DateCreated = testSubTypeInDb.DateCreated;
               testSubType.CreatedBy = testSubTypeInDb.CreatedBy;
               testSubType.ModifiedBy = session?.GetCurrentAdmin().Oid;
               testSubType.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               testSubType.DateModified = DateTime.Now;
               testSubType.IsDeleted = false;
               testSubType.IsSynced = false;

               context.TestSubTypes.Update(testSubType);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction("Index", new { oid = testSubType.TestTypeId, module = "Investigation" });
            }
            else
            {
               ViewBag.TestType = new SelectList(context.TestTypes, FieldConstants.Oid, FieldConstants.Description);
               ViewBag.TestTypeId = testSubTypeInDb.TestTypeId;

               if (isTestSubTypeDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(testSubType);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var testSubTypeInDb = await context.TestSubTypes
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (testSubTypeInDb == null)
            return NotFound();

         return View(testSubTypeInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var testSubType = await context.TestSubTypes.FindAsync(id);

         context.TestSubTypes.Remove(testSubType);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsTestSubTypeDuplicate(TestSubtype testSubType)
      {
         try
         {
            var testSubTypeInDb = context.TestSubTypes
                .AsNoTracking()
                .FirstOrDefault(p =>
                    p.Description.ToLower() == testSubType.Description.ToLower() &&
                    p.TestTypeId == testSubType.TestTypeId &&
                    p.Oid != testSubType.Oid &&
                    p.IsDeleted == false);

            if (testSubTypeInDb != null)
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