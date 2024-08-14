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
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class PhysicalSystemsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public PhysicalSystemsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.PhysicalSystems
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false).OrderBy(x => x.Description).OrderBy(x => x.Description);

         var physicalSystems = query.ToPagedList(pageNumber, pageSize);

         if (physicalSystems.PageCount > 0)
         {
            if (pageNumber > physicalSystems.PageCount)
               physicalSystems = query.ToPagedList(physicalSystems.PageCount, pageSize);
         }

         ViewData["Search"] = search;
         return View(physicalSystems);
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
      public async Task<IActionResult> Create(PhysicalSystem physicalSystem, string? module, string? parent)
      {
         try
         {
            var physicalSystemInDb = IsPhysicalSystemDuplicate(physicalSystem);

            if (!physicalSystemInDb)
            {
               if (ModelState.IsValid)
               {
                  physicalSystem.CreatedBy = session?.GetCurrentAdmin().Oid;
                  physicalSystem.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  physicalSystem.DateCreated = DateTime.Now;
                  physicalSystem.IsDeleted = false;
                  physicalSystem.IsSynced = false;

                  context.PhysicalSystems.Add(physicalSystem);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(physicalSystem);
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

         var physicalSystemInDb = await context.PhysicalSystems.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (physicalSystemInDb == null)
            return NotFound();

         return View(physicalSystemInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, PhysicalSystem physicalSystem, string? module, string? parent)
      {
         try
         {
            var physicalSystemInDb = await context.PhysicalSystems.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == physicalSystem.Oid);

            bool isPhysicalSystemDuplicate = false;

            if (physicalSystemInDb.Description != physicalSystem.Description)
               isPhysicalSystemDuplicate = IsPhysicalSystemDuplicate(physicalSystem);

            if (!isPhysicalSystemDuplicate)
            {
               physicalSystem.DateCreated = physicalSystemInDb.DateCreated;
               physicalSystem.CreatedBy = physicalSystemInDb.CreatedBy;
               physicalSystem.ModifiedBy = session?.GetCurrentAdmin().Oid;
               physicalSystem.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               physicalSystem.DateModified = DateTime.Now;
               physicalSystem.IsDeleted = false;
               physicalSystem.IsSynced = false;

               context.PhysicalSystems.Update(physicalSystem);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isPhysicalSystemDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(physicalSystem);
      }
      #endregion

      #region Read
      public bool IsPhysicalSystemDuplicate(PhysicalSystem physicalSystem)
      {
         try
         {
            var physicalSystemInDb = context.PhysicalSystems.FirstOrDefault(c => c.Description.ToLower() == physicalSystem.Description.ToLower() && c.IsDeleted == false);

            if (physicalSystemInDb != null)
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