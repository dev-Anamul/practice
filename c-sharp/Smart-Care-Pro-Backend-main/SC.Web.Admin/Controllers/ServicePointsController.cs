using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
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
   public class ServicePointsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ServicePointsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.ServicePoints
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false).OrderBy(x => x.Description);

         var servicePoints = query.ToPagedList(pageNumber, pageSize);

         if (servicePoints.PageCount > 0)
         {
            if (pageNumber > servicePoints.PageCount)
               servicePoints = query.ToPagedList(servicePoints.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(servicePoints);
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
      public async Task<IActionResult> Create(ServicePoint servicePoint)
      {
         try
         {
            var servicePointInDb = IsServicePointDuplicate(servicePoint);

            if (!servicePointInDb)
            {
               if (ModelState.IsValid)
               {
                  servicePoint.CreatedBy = session?.GetCurrentAdmin().Oid;
                  servicePoint.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  servicePoint.DateCreated = DateTime.Now;
                  servicePoint.IsDeleted = false;
                  servicePoint.IsSynced = false;

                  context.ServicePoints.Add(servicePoint);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = "HTS" });
               }

               return View(servicePoint);
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

         var servicePointInDb = await context.ServicePoints.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (servicePointInDb == null)
            return NotFound();

         return View(servicePointInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ServicePoint servicePoint)
      {
         try
         {
            var servicePointInDb = await context.ServicePoints.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == servicePoint.Oid);

            bool isServicePointDuplicate = false;

            if (servicePointInDb.Description != servicePoint.Description)
               isServicePointDuplicate = IsServicePointDuplicate(servicePoint);

            if (!isServicePointDuplicate)
            {
               servicePoint.DateCreated = servicePointInDb.DateCreated;
               servicePoint.CreatedBy = servicePointInDb.CreatedBy;
               servicePoint.ModifiedBy = session?.GetCurrentAdmin().Oid;
               servicePoint.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               servicePoint.DateModified = DateTime.Now;
               servicePoint.IsDeleted = false;
               servicePoint.IsSynced = false;

               context.ServicePoints.Update(servicePoint);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "HTS" });
            }
            else
            {
               if (isServicePointDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(servicePoint);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var servicePoint = await context.ServicePoints
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (servicePoint == null)
            return NotFound();

         return View(servicePoint);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var servicePoint = await context.ServicePoints.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.ServicePoints.Remove(servicePoint);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsServicePointDuplicate(ServicePoint servicePoint)
      {
         try
         {
            var servicePointInDb = context.ServicePoints.FirstOrDefault(c => c.Description.ToLower() == servicePoint.Description.ToLower() && c.IsDeleted == false);

            if (servicePointInDb != null)
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