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
   public class PathologyAxisesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public PathologyAxisesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.PathologyAxes
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false).OrderBy(x => x.Description);

         var pathologyAxes = query.ToPagedList(pageNumber, pageSize);

         if (pathologyAxes.PageCount > 0)
         {
            if (pageNumber > pathologyAxes.PageCount)
               pathologyAxes = query.ToPagedList(pathologyAxes.PageCount, pageSize);
         }

         ViewData["Search"] = search;
         return View(pathologyAxes);
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
      public async Task<IActionResult> Create(PathologyAxis pathologyAxis, string? module, string? parent)
      {
         try
         {
            var pathologyAxisInDb = IsPathologyAxesDuplicate(pathologyAxis);

            if (!pathologyAxisInDb)
            {
               if (ModelState.IsValid)
               {
                  pathologyAxis.CreatedBy = session?.GetCurrentAdmin().Oid;
                  pathologyAxis.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  pathologyAxis.DateCreated = DateTime.Now;
                  pathologyAxis.IsDeleted = false;
                  pathologyAxis.IsSynced = false;

                  context.PathologyAxes.Add(pathologyAxis);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(pathologyAxis);
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

         var pathologyAxisInDb = await context.PathologyAxes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (pathologyAxisInDb == null)
            return NotFound();

         return View(pathologyAxisInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, PathologyAxis pathologyAxis, string? module, string? parent)
      {
         try
         {
            var pathologyAxisInDb = await context.PathologyAxes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == pathologyAxis.Oid);

            bool isPathologyDuplicate = false;

            if (pathologyAxisInDb.Description != pathologyAxis.Description)
               isPathologyDuplicate = IsPathologyAxesDuplicate(pathologyAxis);

            if (!isPathologyDuplicate)
            {
               pathologyAxis.DateCreated = pathologyAxisInDb.DateCreated;
               pathologyAxis.CreatedBy = pathologyAxisInDb.CreatedBy;
               pathologyAxis.ModifiedBy = session?.GetCurrentAdmin().Oid;
               pathologyAxis.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               pathologyAxis.DateModified = DateTime.Now;
               pathologyAxis.IsDeleted = false;
               pathologyAxis.IsSynced = false;

               context.PathologyAxes.Update(pathologyAxis);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isPathologyDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(pathologyAxis);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var pathology = await context.PathologyAxes
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (pathology == null)
            return NotFound();

         return View(pathology);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var pathology = await context.PathologyAxes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.PathologyAxes.Remove(pathology);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsPathologyAxesDuplicate(PathologyAxis pathologyAxis)
      {
         try
         {
            var pathologyAxisInDb = context.PathologyAxes.FirstOrDefault(p => p.Description.ToLower() == pathologyAxis.Description.ToLower() && p.IsDeleted == false);

            if (pathologyAxisInDb != null)
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