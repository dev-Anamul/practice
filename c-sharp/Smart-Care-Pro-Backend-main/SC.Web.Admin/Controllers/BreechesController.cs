using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class BreechesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public BreechesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.Breeches
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var breeches = query.ToPagedList(pageNumber, pageSize);

         if (breeches.PageCount > 0)
         {
            if (pageNumber > breeches.PageCount)
               breeches = query.ToPagedList(breeches.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(breeches);
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
      public async Task<IActionResult> Create(Breech breech, string? module)
      {
         try
         {
            var breechInDb = IsBreechDuplicate(breech);

            if (!breechInDb)
            {
               if (ModelState.IsValid)
               {
                  breech.CreatedBy = session?.GetCurrentAdmin().Oid;
                  breech.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  breech.DateCreated = DateTime.Now;
                  breech.IsDeleted = false;
                  breech.IsSynced = false;

                  context.Breeches.Add(breech);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module });
               }

               return View(breech);
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

         var breech = await context.Breeches.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (breech == null)
            return NotFound();

         return View(breech);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, Breech breech, string? module)
      {
         try
         {
            var breechModel = await context.Breeches.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == breech.Oid);

            bool isBreechDuplicate = false;

            if (breechModel.Description != breech.Description)
               isBreechDuplicate = IsBreechDuplicate(breech);

            if (!isBreechDuplicate)
            {
               breech.DateCreated = breechModel.DateCreated;
               breech.CreatedBy = breechModel.CreatedBy;
               breech.ModifiedBy = session?.GetCurrentAdmin().Oid;
               breech.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               breech.DateModified = DateTime.Now;
               breech.IsDeleted = false;
               breech.IsSynced = false;

               context.Breeches.Update(breech);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module });
            }
            else
            {
               if (isBreechDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(breech);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var breech = await context.Breeches
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (breech == null)
            return NotFound();

         return View(breech);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var breech = await context.Breeches.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.Breeches.Remove(breech);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsBreechDuplicate(Breech breech)
      {
         try
         {
            var breechInDb = context.Breeches.FirstOrDefault(c => c.Description.ToLower() == breech.Description.ToLower() && c.IsDeleted == false);

            if (breechInDb != null)
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