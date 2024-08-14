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
   public class MeasuringUnitsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public MeasuringUnitsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.MeasuringUnits
            .Include(r => r.Test)
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(l => l.Description);

         var measuringUnits = query.ToPagedList(pageNumber, pageSize);

         if (measuringUnits.PageCount > 0)
         {
            if (pageNumber > measuringUnits.PageCount)
               measuringUnits = query.ToPagedList(measuringUnits.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(measuringUnits);
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
      public async Task<IActionResult> Create(MeasuringUnit measuringUnit)
      {
         try
         {
            if (ModelState.IsValid)
            {
               measuringUnit.CreatedBy = session?.GetCurrentAdmin().Oid;
               measuringUnit.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
               measuringUnit.DateCreated = DateTime.Now;
               measuringUnit.IsDeleted = false;
               measuringUnit.IsSynced = false;

               context.MeasuringUnits.Add(measuringUnit);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

               return RedirectToAction(nameof(Create), new { module = "Investigation" });
            }

            return View(measuringUnit);
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

         var measuringUnitInDb = await context.MeasuringUnits.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (measuringUnitInDb == null)
            return NotFound();

         ViewBag.Test = new SelectList(context.Tests, FieldConstants.Oid, FieldConstants.TestName);

         return View(measuringUnitInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, MeasuringUnit measuringUnit)
      {
         try
         {
            var measuringUnitInDb = await context.MeasuringUnits.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == measuringUnit.Oid);

            if (id != measuringUnit.Oid)
               return NotFound();

            if (ModelState.IsValid)
            {
               measuringUnit.DateCreated = measuringUnitInDb.DateCreated;
               measuringUnit.CreatedBy = measuringUnitInDb.CreatedBy;
               measuringUnit.ModifiedBy = session?.GetCurrentAdmin().Oid;
               measuringUnit.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               measuringUnit.DateModified = DateTime.Now;
               measuringUnit.IsDeleted = false;
               measuringUnit.IsSynced = false;

               context.MeasuringUnits.Update(measuringUnit);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Investigation" });
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(measuringUnit);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var measuringUnit = await context.MeasuringUnits
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (measuringUnit == null)
            return NotFound();

         return View(measuringUnit);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var measuringUnit = await context.MeasuringUnits.FindAsync(id);

         context.MeasuringUnits.Remove(measuringUnit);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsMeasuringUnitDuplicate(MeasuringUnit measuringUnit)
      {
         try
         {
            var measuringUnitDb = context.MeasuringUnits.FirstOrDefault(c => c.Description.ToLower() == measuringUnit.Description.ToLower() && c.IsDeleted == false);

            if (measuringUnitDb != null)
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