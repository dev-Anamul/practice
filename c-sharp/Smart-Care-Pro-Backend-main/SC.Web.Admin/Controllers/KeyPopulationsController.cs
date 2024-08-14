using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 27.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class KeyPopulationsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public KeyPopulationsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.KeyPopulations
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(l => l.Description);

         var keyPopulations = query.ToPagedList(pageNumber, pageSize);

         if (keyPopulations.PageCount > 0)
         {
            if (pageNumber > keyPopulations.PageCount)
               keyPopulations = query.ToPagedList(keyPopulations.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(keyPopulations);
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
      public async Task<IActionResult> Create(KeyPopulation keyPopulation, string? module, string? parent)
      {
         try
         {
            var keyPopulationInDb = IsKeyPopulationDuplicate(keyPopulation);

            if (!keyPopulationInDb)
            {
               if (ModelState.IsValid)
               {
                  keyPopulation.CreatedBy = session?.GetCurrentAdmin().Oid;
                  keyPopulation.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  keyPopulation.DateCreated = DateTime.Now;
                  keyPopulation.IsDeleted = false;
                  keyPopulation.IsSynced = false;

                  context.KeyPopulations.Add(keyPopulation);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(keyPopulation);
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

         var keyPopulationInDb = await context.KeyPopulations.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (keyPopulationInDb == null)
            return NotFound();

         return View(keyPopulationInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, KeyPopulation keyPopulation, string? module, string? parent)
      {
         try
         {
            var keyPopulationInDb = await context.KeyPopulations.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == keyPopulation.Oid);

            bool isKeyPopulationDuplicate = false;

            if (keyPopulationInDb.Description != keyPopulation.Description)
               isKeyPopulationDuplicate = IsKeyPopulationDuplicate(keyPopulation);

            if (!isKeyPopulationDuplicate)
            {
               keyPopulation.DateCreated = keyPopulationInDb.DateCreated;
               keyPopulation.CreatedBy = keyPopulationInDb.CreatedBy;
               keyPopulation.ModifiedBy = session?.GetCurrentAdmin().Oid;
               keyPopulation.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               keyPopulation.DateModified = DateTime.Now;
               keyPopulation.IsDeleted = false;
               keyPopulation.IsSynced = false;

               context.KeyPopulations.Update(keyPopulation);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isKeyPopulationDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(keyPopulation);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var keyPopulation = await context.KeyPopulations
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (keyPopulation == null)
            return NotFound();

         return View(keyPopulation);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var keyPopulation = await context.KeyPopulations.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.KeyPopulations.Remove(keyPopulation);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsKeyPopulationDuplicate(KeyPopulation keyPopulation)
      {
         try
         {
            var keyPopulationInDb = context.KeyPopulations.FirstOrDefault(c => c.Description.ToLower() == keyPopulation.Description.ToLower() && c.IsDeleted == false);

            if (keyPopulationInDb != null)
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