using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Brian
 * Date created : 06.05.2023
 * Modified by  : Bella
 * Last modified: 22.05.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class ModeOfDeliveriesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ModeOfDeliveriesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.ModeOfDeliveries
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(l => l.Description);

         var modeOfDeliveries = query.ToPagedList(pageNumber, pageSize);

         if (modeOfDeliveries.PageCount > 0)
         {
            if (pageNumber > modeOfDeliveries.PageCount)
               modeOfDeliveries = query.ToPagedList(modeOfDeliveries.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(modeOfDeliveries);
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
      public async Task<IActionResult> Create(ModeOfDelivery modeOfDelivery, string? module)
      {
         try
         {
            var modeOfDeliveryInDb = IsModeOfDeliveryDuplicate(modeOfDelivery);

            if (!modeOfDeliveryInDb)
            {
               if (ModelState.IsValid)
               {
                  modeOfDelivery.CreatedBy = session?.GetCurrentAdmin().Oid;
                  modeOfDelivery.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  modeOfDelivery.DateCreated = DateTime.Now;
                  modeOfDelivery.IsDeleted = false;
                  modeOfDelivery.IsSynced = false;

                  context.ModeOfDeliveries.Add(modeOfDelivery);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module });
               }

               return View(modeOfDelivery);
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

         var modeOfDeliveryInDb = await context.ModeOfDeliveries.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (modeOfDeliveryInDb == null)
            return NotFound();

         return View(modeOfDeliveryInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ModeOfDelivery modeOfDelivery, string? module)
      {
         try
         {
            var modeOfDeliveryInDb = await context.ModeOfDeliveries.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == modeOfDelivery.Oid);

            bool isModeOfDeliveryDuplicate = false;

            if (modeOfDeliveryInDb.Description != modeOfDelivery.Description)
               isModeOfDeliveryDuplicate = IsModeOfDeliveryDuplicate(modeOfDelivery);

            if (!isModeOfDeliveryDuplicate)
            {
               modeOfDelivery.DateCreated = modeOfDeliveryInDb.DateCreated;
               modeOfDelivery.CreatedBy = modeOfDeliveryInDb.CreatedBy;
               modeOfDelivery.ModifiedBy = session?.GetCurrentAdmin().Oid;
               modeOfDelivery.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               modeOfDelivery.DateModified = DateTime.Now;
               modeOfDelivery.IsDeleted = false;
               modeOfDelivery.IsSynced = false;

               context.ModeOfDeliveries.Update(modeOfDelivery);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module });
            }
            else
            {
               if (isModeOfDeliveryDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(modeOfDelivery);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var modeOfDelivery = await context.ModeOfDeliveries
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (modeOfDelivery == null)
            return NotFound();

         return View(modeOfDelivery);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var modeOfDelivery = await context.ModeOfDeliveries.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.ModeOfDeliveries.Remove(modeOfDelivery);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsModeOfDeliveryDuplicate(ModeOfDelivery modeOfDelivery)
      {
         try
         {
            var modeOfDeliveryInDb = context.ModeOfDeliveries.FirstOrDefault(c => c.Description.ToLower() == modeOfDelivery.Description.ToLower() && c.IsDeleted == false);

            if (modeOfDeliveryInDb != null)
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