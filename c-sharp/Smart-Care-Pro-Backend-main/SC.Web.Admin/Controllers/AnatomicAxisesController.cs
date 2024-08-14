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
   public class AnatomicAxisesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public AnatomicAxisesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.AnatomicAxes
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var anatomicAxes = query.ToPagedList(pageNumber, pageSize);

         if (anatomicAxes.PageCount > 0)
         {
            if (pageNumber > anatomicAxes.PageCount)
               anatomicAxes = query.ToPagedList(anatomicAxes.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(anatomicAxes);
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
      public async Task<IActionResult> Create(AnatomicAxis anatomicAxis, string? module, string? parent)
      {
         try
         {
            var anatomicAxisInDb = IsAnatomicAxisDuplicate(anatomicAxis);

            if (!anatomicAxisInDb)
            {
               if (ModelState.IsValid)
               {
                  anatomicAxis.CreatedBy = session?.GetCurrentAdmin().Oid;
                  anatomicAxis.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  anatomicAxis.DateCreated = DateTime.Now;
                  anatomicAxis.IsDeleted = false;
                  anatomicAxis.IsSynced = false;

                  context.AnatomicAxes.Add(anatomicAxis);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(anatomicAxis);
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

         var anatomicAxis = await context.AnatomicAxes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (anatomicAxis == null)
            return NotFound();

         return View(anatomicAxis);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, AnatomicAxis anatomicAxis, string? module, string? parent)
      {
         try
         {
            var anatomicAxisInDb = await context.AnatomicAxes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == anatomicAxis.Oid);

            bool isAnatomicAxisDuplicate = false;

            if (anatomicAxisInDb.Description != anatomicAxis.Description)
               isAnatomicAxisDuplicate = IsAnatomicAxisDuplicate(anatomicAxis);

            if (!isAnatomicAxisDuplicate)
            {
               anatomicAxis.DateCreated = anatomicAxisInDb.DateCreated;
               anatomicAxis.CreatedBy = anatomicAxisInDb.CreatedBy;
               anatomicAxis.ModifiedBy = session?.GetCurrentAdmin().Oid;
               anatomicAxis.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               anatomicAxis.DateModified = DateTime.Now;
               anatomicAxis.IsDeleted = false;
               anatomicAxis.IsSynced = false;

               context.AnatomicAxes.Update(anatomicAxis);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isAnatomicAxisDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(anatomicAxis);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var anatomicAxis = await context.AnatomicAxes
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (anatomicAxis == null)
            return NotFound();

         return View(anatomicAxis);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var anatomicAxis = await context.AnatomicAxes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.AnatomicAxes.Remove(anatomicAxis);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsAnatomicAxisDuplicate(AnatomicAxis anatomicAxis)
      {
         try
         {
            var anatomicAxisInDb = context.AnatomicAxes.FirstOrDefault(p => p.Description.ToLower() == anatomicAxis.Description.ToLower() && p.IsDeleted == false);

            if (anatomicAxisInDb != null)
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