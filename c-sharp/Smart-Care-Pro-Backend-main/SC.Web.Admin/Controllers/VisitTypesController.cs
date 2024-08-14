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
   public class VisitTypesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;
      public VisitTypesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.VisitTypes
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var visitTypes = query.ToPagedList(pageNumber, pageSize);

         if (visitTypes.PageCount > 0)
         {
            if (pageNumber > visitTypes.PageCount)
               visitTypes = query.ToPagedList(visitTypes.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(visitTypes);
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
      public async Task<IActionResult> Create(VisitType visitType)
      {
         try
         {
            var visitTypeInDb = IsVisitTypeDuplicate(visitType);

            if (!visitTypeInDb)
            {
               if (ModelState.IsValid)
               {
                  visitType.CreatedBy = session?.GetCurrentAdmin().Oid;
                  visitType.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  visitType.DateCreated = DateTime.Now;
                  visitType.IsDeleted = false;
                  visitType.IsSynced = false;

                  context.VisitTypes.Add(visitType);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = "HTS" });
               }

               return View(visitType);
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

         var visitType = await context.VisitTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (visitType == null)
            return NotFound();

         return View(visitType);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, VisitType visitType)
      {
         try
         {
            var visitTypeInDb = await context.VisitTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == visitType.Oid);

            bool isVisitTypeDuplicate = false;

            if (visitTypeInDb.Description != visitType.Description)
               isVisitTypeDuplicate = IsVisitTypeDuplicate(visitType);

            if (!isVisitTypeDuplicate)
            {
               visitType.DateCreated = visitTypeInDb.DateCreated;
               visitType.CreatedBy = visitTypeInDb.CreatedBy;
               visitType.ModifiedBy = session?.GetCurrentAdmin().Oid;
               visitType.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               visitType.DateModified = DateTime.Now;
               visitType.IsDeleted = false;
               visitType.IsSynced = false;

               context.VisitTypes.Update(visitType);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "HTS" });
            }
            else
            {
               if (isVisitTypeDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(visitType);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var visitType = await context.VisitTypes
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (visitType == null)
            return NotFound();

         return View(visitType);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var visitType = await context.VisitTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.VisitTypes.Remove(visitType);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsVisitTypeDuplicate(VisitType visitType)
      {
         try
         {
            var visitTypeInDb = context.VisitTypes.FirstOrDefault(c => c.Description.ToLower() == visitType.Description.ToLower() && c.IsDeleted == false);

            if (visitTypeInDb != null)
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