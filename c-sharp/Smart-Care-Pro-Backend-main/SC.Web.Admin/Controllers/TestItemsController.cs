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
   public class TestItemsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public TestItemsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.TestItems
            .Include(t => t.Test)
            .Include(t => t.CompositeTest)
            .Where(x => x.Test.Title.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Test.Title);

         var testItems = query.ToPagedList(pageNumber, pageSize);

         if (testItems.PageCount > 0)
         {
            if (pageNumber > testItems.PageCount)
               testItems = query.ToPagedList(testItems.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(testItems);
      }
      #endregion

      #region Create
      [HttpGet]
      public IActionResult Create()
      {
         try
         {
            ViewBag.Test = new SelectList(context.Tests, FieldConstants.Oid, FieldConstants.TestName);
            ViewBag.CompositeTests = new SelectList(context.CompositeTests, FieldConstants.Oid, FieldConstants.Description);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(TestItem testItem)
      {
         try
         {
            var testItemInDb = IsTestItemDuplicate(testItem);

            if (!testItemInDb)
            {
               if (ModelState.IsValid)
               {
                  testItem.CreatedBy = session?.GetCurrentAdmin().Oid;
                  testItem.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  testItem.DateCreated = DateTime.Now;
                  testItem.IsDeleted = false;
                  testItem.IsSynced = false;

                  context.TestItems.Add(testItem);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = "Investigation" });

               }

               return View(testItem);
            }
            else
            {
               ModelState.AddModelError("TestId", MessageConstants.DuplicateFound);
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

         var testItemInDb = await context.TestItems.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (testItemInDb == null)
            return NotFound();

         ViewBag.Test = new SelectList(context.Tests, FieldConstants.Oid, FieldConstants.TestName);
         ViewBag.CompositeTests = new SelectList(context.CompositeTests, FieldConstants.Oid, FieldConstants.Description);

         return View(testItemInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, TestItem testItem)
      {
         try
         {
            var testItemInDb = await context.TestItems.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == testItem.Oid);

            bool isTestItemDuplicate = false;

            if (testItemInDb.TestId != testItem.TestId)
               isTestItemDuplicate = IsTestItemDuplicate(testItem);

            if (!isTestItemDuplicate)
            {
               testItem.DateCreated = testItemInDb.DateCreated;
               testItem.CreatedBy = testItemInDb.CreatedBy;
               testItem.ModifiedBy = session?.GetCurrentAdmin().Oid;
               testItem.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               testItem.DateModified = DateTime.Now;
               testItem.IsDeleted = false;
               testItem.IsSynced = false;

               context.TestItems.Update(testItem);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Investigation" });
            }
            else
            {
               if (isTestItemDuplicate)
                  ModelState.AddModelError("TestId", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(testItem);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var testItem = await context.TestItems
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (testItem == null)
            return NotFound();

         return View(testItem);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var testItem = await context.TestItems.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.TestItems.Remove(testItem);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsTestItemDuplicate(TestItem testItem)
      {
         try
         {
            var testItemInDb = context.TestItems.FirstOrDefault(c => c.TestId == testItem.TestId && c.IsDeleted == false);

            if (testItemInDb != null)
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