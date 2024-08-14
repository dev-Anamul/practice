using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Stephan
 * Date created : 06.01.2024
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class ArmedForceServicesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ArmedForceServicesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.ArmedForceServices
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var armedForceServices = query.ToPagedList(pageNumber, pageSize);

         if (armedForceServices.PageCount > 0)
         {
            if (pageNumber > armedForceServices.PageCount)
               armedForceServices = query.ToPagedList(armedForceServices.PageCount, pageSize);
         }

         ViewData["Search"] = search;
         return View(armedForceServices);
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
      public async Task<IActionResult> Create(ArmedForceService armedForceService)
      {
         try
         {
            var armedForceServiceInDb = IsArmedForceServiceDuplicate(armedForceService);

            if (!armedForceServiceInDb)
            {
               if (ModelState.IsValid)
               {
                  armedForceService.CreatedBy = session?.GetCurrentAdmin().Oid;
                  armedForceService.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  armedForceService.DateCreated = DateTime.Now;
                  armedForceService.IsDeleted = false;
                  armedForceService.IsSynced = false;

                  context.ArmedForceServices.Add(armedForceService);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = "Clients" });
               }

               return View(armedForceService);
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

         var armedForceServiceInDb = await context.ArmedForceServices.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (armedForceServiceInDb == null)
            return NotFound();

         return View(armedForceServiceInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ArmedForceService armedForceService)
      {
         try
         {
            var armedForceServiceInDb = await context.Provinces.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == armedForceService.Oid);

            bool isArmedForceServiceDuplicate = false;

            if (armedForceServiceInDb.Description != armedForceService.Description)
               isArmedForceServiceDuplicate = IsArmedForceServiceDuplicate(armedForceService);

            if (!isArmedForceServiceDuplicate)
            {
               armedForceService.DateCreated = armedForceServiceInDb.DateCreated;
               armedForceService.CreatedBy = armedForceServiceInDb.CreatedBy;
               armedForceService.ModifiedBy = session?.GetCurrentAdmin().Oid;
               armedForceService.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               armedForceService.DateModified = DateTime.Now;
               armedForceService.IsDeleted = false;
               armedForceService.IsSynced = false;

               context.ArmedForceServices.Update(armedForceService);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Clients" });
            }
            else
            {
               if (isArmedForceServiceDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(armedForceService);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var armedForceService = await context.ArmedForceServices
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (armedForceService == null)
            return NotFound();

         return View(armedForceService);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var armedForceService = await context.ArmedForceServices.FindAsync(id);

         if (armedForceService == null)
            return NotFound();

         var dfzPatientType = await context.DFZPatientTypes.Where(d => d.ArmedForceId == id).ToListAsync();

         if (dfzPatientType.Any())
         {
            ViewData["ErrorMessage"] = "Cannot delete province because it has associated districts.";
            return View(armedForceService);
         }

         armedForceService.IsDeleted = true;

         context.ArmedForceServices.Update(armedForceService);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      public bool IsArmedForceServiceDuplicate(ArmedForceService armedForceService)
      {
         try
         {
            var armedForceServiceInDb = context.ArmedForceServices.FirstOrDefault(c => c.Description.ToLower() == armedForceService.Description.ToLower() && c.IsDeleted == false);

            if (armedForceServiceInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

   }
}