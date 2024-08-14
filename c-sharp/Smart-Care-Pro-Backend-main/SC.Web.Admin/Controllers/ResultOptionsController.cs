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
   public class ResultOptionsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ResultOptionsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.ResultOptions
            .Include(r => r.Test)
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Test.Description);

         var resultOptions = query.ToPagedList(pageNumber, pageSize);

         if (resultOptions.PageCount > 0)
         {
            if (pageNumber > resultOptions.PageCount)
               resultOptions = query.ToPagedList(resultOptions.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(resultOptions);

      }
      #endregion

      #region Create
      [HttpGet]
      public IActionResult Create()
      {
         try
         {
            ViewBag.Test = new SelectList(context.Tests, FieldConstants.Oid, FieldConstants.TestName);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(ResultOption resultOption)
      {
         try
         {
            if (ModelState.IsValid)
            {
               resultOption.CreatedBy = session?.GetCurrentAdmin().Oid;
               resultOption.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
               resultOption.DateCreated = DateTime.Now;
               resultOption.IsDeleted = false;
               resultOption.IsSynced = false;

               context.ResultOptions.Add(resultOption);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

               return RedirectToAction(nameof(Create), new { module = "Investigation" });
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

         var resultOptionInDb = await context.ResultOptions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (resultOptionInDb == null)
            return NotFound();

         ViewBag.Test = new SelectList(context.Tests, FieldConstants.Oid, FieldConstants.TestName);

         return View(resultOptionInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ResultOption resultOption)
      {
         try
         {
            var resultOptionInDb = await context.ResultOptions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == resultOption.Oid);

            bool isResultOptionDuplicate = false;

            if (!isResultOptionDuplicate)
            {
               resultOption.DateCreated = resultOptionInDb.DateCreated;
               resultOption.CreatedBy = resultOptionInDb.CreatedBy;
               resultOption.ModifiedBy = session?.GetCurrentAdmin().Oid;
               resultOption.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               resultOption.DateModified = DateTime.Now;
               resultOption.IsDeleted = false;
               resultOption.IsSynced = false;

               context.ResultOptions.Update(resultOption);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Investigation" });
            }
            else
            {
               if (isResultOptionDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(resultOption);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var resultOption = await context.ResultOptions
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (resultOption == null)
            return NotFound();

         return View(resultOption);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var resultOption = await context.ResultOptions.FindAsync(id);

         context.ResultOptions.Remove(resultOption);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion
   }
}