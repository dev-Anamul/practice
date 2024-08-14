using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 09.03.2023
 * Modified by  : Bella
 * Last modified: 14.03.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class DrugRoutesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public DrugRoutesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.DrugRoutes
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var drugRoutes = query.ToPagedList(pageNumber, pageSize);

         if (drugRoutes.PageCount > 0)
         {
            if (pageNumber > drugRoutes.PageCount)
               drugRoutes = query.ToPagedList(drugRoutes.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(drugRoutes);
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
      public async Task<IActionResult> Create(DrugRoute drugRoute)
      {
         try
         {
            var drugRouteInDb = IsDrugRouteDuplicate(drugRoute);

            if (!drugRouteInDb)
            {
               if (ModelState.IsValid)
               {
                  drugRoute.CreatedBy = session?.GetCurrentAdmin().Oid;
                  drugRoute.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  drugRoute.DateCreated = DateTime.Now;
                  drugRoute.IsDeleted = false;
                  drugRoute.IsSynced = false;

                  context.DrugRoutes.Add(drugRoute);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = "Pharmacy" });
               }

               return View(drugRoute);
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

         var drugRouteInDb = await context.DrugRoutes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (drugRouteInDb == null)
            return NotFound();

         return View(drugRouteInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, DrugRoute drugRoute)
      {
         try
         {
            var drugRouteInDb = await context.DrugRoutes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == drugRoute.Oid);

            bool isDrugRouteDuplicate = false;

            if (drugRouteInDb.Description != drugRoute.Description)
               isDrugRouteDuplicate = IsDrugRouteDuplicate(drugRoute);

            if (!isDrugRouteDuplicate)
            {
               drugRoute.DateCreated = drugRouteInDb.DateCreated;
               drugRoute.CreatedBy = drugRouteInDb.CreatedBy;
               drugRoute.ModifiedBy = session?.GetCurrentAdmin().Oid;
               drugRoute.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               drugRoute.DateModified = DateTime.Now;
               drugRoute.IsDeleted = false;
               drugRoute.IsSynced = false;

               context.DrugRoutes.Update(drugRoute);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Pharmacy" });
            }
            else
            {
               if (isDrugRouteDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(drugRoute);
      }
      #endregion

      #region Read
      public bool IsDrugRouteDuplicate(DrugRoute drugRoute)
      {
         try
         {
            var drugRouteInDb = context.DrugRoutes.FirstOrDefault(c => c.Description.ToLower() == drugRoute.Description.ToLower() && c.IsDeleted == false);

            if (drugRouteInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      [HttpGet]
      public async Task<IActionResult> GetAllDrugRoutes()
      {
         return Json(await context.DrugRoutes.ToListAsync());
      }

      #endregion
   }
}