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
 * Date created : 07.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class CompositeTestsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public CompositeTestsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.CompositeTests
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false).Include(t => t.TestItems.Where(ti => ti.IsDeleted == false)).ThenInclude(t => t.Test)
             .OrderBy(x => x.Description);

         var compositeTests = query.ToPagedList(pageNumber, pageSize);

         if (compositeTests.PageCount > 0)
         {
            if (pageNumber > compositeTests.PageCount)
               compositeTests = query.ToPagedList(compositeTests.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(compositeTests);
      }
      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create()
      {
         var testData = await context.Tests.ToListAsync();
         ViewBag.TestData = new SelectList(testData, FieldConstants.Oid, FieldConstants.TestName);
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(CompositeTest compositeTest)
      {
         try
         {
            var compositeTestInDb = IsCompositeDuplicate(compositeTest);

            if (!compositeTestInDb)
            {
               if (ModelState.IsValid)
               {
                  compositeTest.CreatedBy = session?.GetCurrentAdmin().Oid;
                  compositeTest.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  compositeTest.DateCreated = DateTime.Now;
                  compositeTest.IsDeleted = false;
                  compositeTest.IsSynced = false;

                  context.CompositeTests.Add(compositeTest);
                  await context.SaveChangesAsync();

                  if (compositeTest?.TestList.Length > 0)
                  {
                     int currentCompositeId = context.CompositeTests.First(c => c.Description.ToLower() == compositeTest.Description.ToLower() && c.IsDeleted == false).Oid;
                     List<TestItem> testItems = new List<TestItem>();
                     for (int i = 0; i < compositeTest.TestList.Length; i++)
                     {
                        TestItem testItem = new TestItem()
                        {
                           CompositeTestId = currentCompositeId,
                           TestId = compositeTest.TestList[i],
                           CreatedBy = session?.GetCurrentAdmin().Oid,
                           CreatedIn = session?.GetCurrentAdmin().CreatedIn,
                           DateCreated = DateTime.Now,
                           IsDeleted = false,
                           IsSynced = false
                        };
                        testItems.Add(testItem);

                     }
                     context.TestItems.AddRange(testItems);
                     await context.SaveChangesAsync();
                  }

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = "Investigation" });
               }

               return View(compositeTest);
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
         try
         {
            if (id == null)
            {
               return NotFound(MessageConstants.NoMatchFoundError);
            }

            var compositeTests = await context.CompositeTests.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id && i.IsDeleted == false);

            if (compositeTests == null)
            {
               return NotFound(MessageConstants.NoMatchFoundError);
            }

            compositeTests.TestItems = await context.TestItems.Where(c => c.CompositeTestId == compositeTests.Oid && c.IsDeleted == false).Include(t => t.Test).ToListAsync();

            var testData = await context.Tests.ToListAsync();
            ViewBag.TestData = new SelectList(testData, FieldConstants.Oid, FieldConstants.TestName);

            return View(compositeTests);
         }
         catch (Exception ex)
         {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
         }
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, CompositeTest compositeTest)
      {
         try
         {
            var compositeTestInDb = await context.CompositeTests
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Oid == compositeTest.Oid);

            bool isCompositeDuplicate = false;

            if (compositeTestInDb.Description != compositeTest.Description)
               isCompositeDuplicate = IsCompositeDuplicate(compositeTest);

            if (!isCompositeDuplicate)
            {
               compositeTest.DateCreated = compositeTestInDb.DateCreated;
               compositeTest.CreatedBy = compositeTestInDb.CreatedBy;
               compositeTest.ModifiedBy = session?.GetCurrentAdmin().Oid;
               compositeTest.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               compositeTest.DateModified = DateTime.Now;
               compositeTest.IsDeleted = false;
               compositeTest.IsSynced = false;

               context.CompositeTests.Update(compositeTest);
               await context.SaveChangesAsync();

               // Remove existing TestItems
               var existingTestItems = await context.TestItems.Where(ti => ti.CompositeTestId == compositeTest.Oid && ti.IsDeleted == false).ToListAsync();

               foreach (var existingTestItem in existingTestItems)
               {
                  existingTestItem.IsDeleted = true;
                  existingTestItem.ModifiedBy = session?.GetCurrentAdmin().Oid;
                  existingTestItem.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
                  existingTestItem.DateModified = DateTime.Now;
                  context.TestItems.Update(existingTestItem);
               }

               // Add new TestItems
               if (compositeTest?.TestList.Length > 0)
               {
                  int currentCompositeId = context.CompositeTests
                      .First(c => c.Description.ToLower() == compositeTest.Description.ToLower() && c.IsDeleted == false)
                      .Oid;

                  List<TestItem> testItems = new List<TestItem>();
                  for (int i = 0; i < compositeTest.TestList.Length; i++)
                  {
                     TestItem testItem = new TestItem()
                     {
                        CompositeTestId = currentCompositeId,
                        TestId = compositeTest.TestList[i],
                        CreatedBy = session?.GetCurrentAdmin().Oid,
                        CreatedIn = session?.GetCurrentAdmin().CreatedIn,
                        DateCreated = DateTime.Now,
                        IsDeleted = false,
                        IsSynced = false
                     };
                     testItems.Add(testItem);
                  }

                  context.TestItems.AddRange(testItems);
                  await context.SaveChangesAsync();
               }

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Investigation" });
            }
            else
            {
               if (isCompositeDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(compositeTest);
      }

      //[HttpPost]
      //[ValidateAntiForgeryToken]
      //public async Task<IActionResult> Edit(int id, CompositeTest compositeTest)
      //{
      //    try
      //    {
      //        var compositeTestInDb = await context.CompositeTests.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == compositeTest.Oid);

      //        bool isCompositeDuplicate = false;

      //        if (compositeTestInDb.Description != compositeTest.Description)
      //            isCompositeDuplicate = IsCompositeDuplicate(compositeTest);

      //        if (!isCompositeDuplicate)
      //        {
      //            compositeTest.ModifiedBy = session?.GetCurrentAdmin().Oid;
      //            compositeTest.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
      //            compositeTest.DateModified = DateTime.Now;
      //            compositeTest.IsDeleted = false;
      //            compositeTest.IsSynced = false;

      //            context.CompositeTests.Update(compositeTest);
      //            await context.SaveChangesAsync();

      //             if (compositeTest?.TestList.Length > 0)
      //                {
      //                    int currentCompositeId = context.CompositeTests.First(c => c.Description.ToLower() == compositeTest.Description.ToLower() && c.IsDeleted == false).Oid;
      //                    List<TestItem> testItems = new List<TestItem>();
      //                    for (int i = 0; i < compositeTest.TestList.Length; i++)
      //                    {
      //                        TestItem testItem = new TestItem()
      //                        {
      //                            CompositeTestId = currentCompositeId,
      //                            TestId = compositeTest.TestList[i],
      //                            CreatedBy = session?.GetCurrentAdmin().Oid,
      //                            CreatedIn = session?.GetCurrentAdmin().CreatedIn,
      //                            DateCreated = DateTime.Now,
      //                            IsDeleted = false,
      //                            IsSynced = false
      //                        };
      //                        testItems.Add(testItem);

      //                    }
      //                    context.TestItems.AddRange(testItems);
      //                    await context.SaveChangesAsync();
      //                }

      //            TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

      //            return RedirectToAction(nameof(Index));
      //        }
      //        else
      //        {
      //            if (isCompositeDuplicate)
      //                ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
      //        }
      //    }
      //    catch (Exception)
      //    {
      //        throw;
      //    }

      //    return View(compositeTest);
      //}
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var compositeTests = await context.CompositeTests
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (compositeTests == null)
            return NotFound();

         return View(compositeTests);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var compositeTest = await context.CompositeTests.FindAsync(id);

         context.CompositeTests.Remove(compositeTest);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsCompositeDuplicate(CompositeTest compositeTest)
      {
         try
         {
            var compositeTestInDb = context.CompositeTests.FirstOrDefault(c => c.Description.ToLower() == compositeTest.Description.ToLower() && c.IsDeleted == false);

            if (compositeTestInDb != null)
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