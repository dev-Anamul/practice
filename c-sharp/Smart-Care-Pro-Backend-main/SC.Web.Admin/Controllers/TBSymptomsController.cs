using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 21.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Web.Controllers
{
   [RequestAuthenticationFilter]
   public class TBSymptomsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public TBSymptomsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.TBSymptoms
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var tBSymptoms = query.ToPagedList(pageNumber, pageSize);

         if (tBSymptoms.PageCount > 0)
         {
            if (pageNumber > tBSymptoms.PageCount)
               tBSymptoms = query.ToPagedList(tBSymptoms.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(tBSymptoms);
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
      public async Task<IActionResult> Create(TBSymptom tbSymptom, string? module, string? parent)
      {
         try
         {
            var tbSymptomInDb = IsTBSymptomDuplicate(tbSymptom);

            if (!tbSymptomInDb)
            {
               if (ModelState.IsValid)
               {
                  tbSymptom.CreatedBy = session?.GetCurrentAdmin().Oid;
                  tbSymptom.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  tbSymptom.DateCreated = DateTime.Now;
                  tbSymptom.IsDeleted = false;
                  tbSymptom.IsSynced = false;

                  context.TBSymptoms.Add(tbSymptom);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(tbSymptom);
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

         var tbSymptomInDb = await context.TBSymptoms.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (tbSymptomInDb == null)
            return NotFound();

         return View(tbSymptomInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, TBSymptom tbSymptom, string? module, string? parent)
      {
         try
         {
            var tbSymptomInDb = await context.TBSymptoms.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == tbSymptom.Oid);

            bool isTBSymptomDuplicate = false;

            if (tbSymptomInDb.Description != tbSymptom.Description)
               isTBSymptomDuplicate = IsTBSymptomDuplicate(tbSymptom);

            if (!isTBSymptomDuplicate)
            {
               tbSymptom.DateCreated = tbSymptomInDb.DateCreated;
               tbSymptom.CreatedBy = tbSymptomInDb.CreatedBy;
               tbSymptom.ModifiedBy = session?.GetCurrentAdmin().Oid;
               tbSymptom.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               tbSymptom.DateModified = DateTime.Now;
               tbSymptom.IsDeleted = false;
               tbSymptom.IsSynced = false;

               context.TBSymptoms.Update(tbSymptom);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(tbSymptom);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var tbSymptom = await context.TBSymptoms
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (tbSymptom == null)
            return NotFound();

         return View(tbSymptom);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var tbSymptom = await context.TBSymptoms.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.TBSymptoms.Remove(tbSymptom);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsTBSymptomDuplicate(TBSymptom tbSymptom)
      {
         try
         {
            var tbSymptomInDb = context.TBSymptoms.FirstOrDefault(c => c.Description.ToLower() == tbSymptom.Description.ToLower() && c.IsDeleted == false);

            if (tbSymptomInDb != null)
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