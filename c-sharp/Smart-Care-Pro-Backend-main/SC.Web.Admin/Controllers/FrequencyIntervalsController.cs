using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 22.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class FrequencyIntervalsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public FrequencyIntervalsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.FrequencyIntervals
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(l => l.Description);

         var frequencyIntervals = query.ToPagedList(pageNumber, pageSize);

         if (frequencyIntervals.PageCount > 0)
         {
            if (pageNumber > frequencyIntervals.PageCount)
               frequencyIntervals = query.ToPagedList(frequencyIntervals.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(frequencyIntervals);
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
        public async Task<IActionResult> Create(FrequencyInterval frequencyInterval)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    frequencyInterval.CreatedBy = session?.GetCurrentAdmin()?.Oid;
                    frequencyInterval.CreatedIn = session?.GetCurrentAdmin()?.CreatedIn;
                    frequencyInterval.DateCreated = DateTime.Now;
                    frequencyInterval.IsDeleted = false;
                    frequencyInterval.IsSynced = false;

                    context.FrequencyIntervals.Add(frequencyInterval);
                    await context.SaveChangesAsync();

                    TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                    return RedirectToAction(nameof(Create), new { module = "Pharmacy" });
                }

                return View(frequencyInterval);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return View(frequencyInterval);
            }
        }

        #endregion

        #region Edit
        [HttpGet]
      public async Task<IActionResult> Edit(int? id)
      {
         if (id == null)
            return NotFound();

         var frequencyIntervalInDb = await context.FrequencyIntervals.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (frequencyIntervalInDb == null)
            return NotFound();

         return View(frequencyIntervalInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FrequencyInterval frequencyInterval)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    frequencyInterval.ModifiedBy = session?.GetCurrentAdmin()?.Oid;
                    frequencyInterval.ModifiedIn = session?.GetCurrentAdmin()?.ModifiedIn;
                    frequencyInterval.DateModified = DateTime.Now;
                    frequencyInterval.IsDeleted = false;
                    frequencyInterval.IsSynced = false;

                    context.FrequencyIntervals.Update(frequencyInterval);
                    await context.SaveChangesAsync();

                    TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

                    return RedirectToAction(nameof(Index), new { module = "Pharmacy" });
                }

                return View(frequencyInterval);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Delete
        [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var frequencyInterval = await context.FrequencyIntervals
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (frequencyInterval == null)
            return NotFound();

         return View(frequencyInterval);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var frequencyInterval = await context.FrequencyIntervals.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.FrequencyIntervals.Remove(frequencyInterval);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsFrequencyIntervalDuplicate(FrequencyInterval frequencyInterval)
      {
         try
         {
            var frequencyInDb = context.FrequencyIntervals.FirstOrDefault(c => c.Description.ToLower() == frequencyInterval.Description.ToLower() && c.IsDeleted == false);

            if (frequencyInDb != null)
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