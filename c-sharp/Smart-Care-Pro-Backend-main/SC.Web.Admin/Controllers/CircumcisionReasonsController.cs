using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 26.04.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class CircumcisionReasonsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public CircumcisionReasonsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.CircumcisionReasons
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var circumcisionReasons = query.ToPagedList(pageNumber, pageSize);

         if (circumcisionReasons.PageCount > 0)
         {
            if (pageNumber > circumcisionReasons.PageCount)
               circumcisionReasons = query.ToPagedList(circumcisionReasons.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(circumcisionReasons);
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
      public async Task<IActionResult> Create(CircumcisionReason circumcisionReason, string? module)
      {
         try
         {
            var circumcisionReasonInDb = IsCircumcisionReasonDuplicate(circumcisionReason);

            if (!circumcisionReasonInDb)
            {
               if (ModelState.IsValid)
               {
                  circumcisionReason.CreatedBy = session?.GetCurrentAdmin().Oid;
                  circumcisionReason.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  circumcisionReason.DateCreated = DateTime.Now;
                  circumcisionReason.IsDeleted = false;
                  circumcisionReason.IsSynced = false;

                  context.CircumcisionReasons.Add(circumcisionReason);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module });
               }

               return View(circumcisionReason);
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

         var circumcisionReason = await context.CircumcisionReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (circumcisionReason == null)
            return NotFound();

         return View(circumcisionReason);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, CircumcisionReason circumcisionReason, string? module)
      {
         try
         {
            var circumcisionReasonInDb = await context.CircumcisionReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == circumcisionReason.Oid);

            bool isCircumcisionReasonDuplicate = false;

            if (circumcisionReasonInDb.Description != circumcisionReason.Description)
               isCircumcisionReasonDuplicate = IsCircumcisionReasonDuplicate(circumcisionReason);

            if (!isCircumcisionReasonDuplicate)
            {
               circumcisionReason.DateCreated = circumcisionReasonInDb.DateCreated;
               circumcisionReason.CreatedBy = circumcisionReasonInDb.CreatedBy;
               circumcisionReason.ModifiedBy = session?.GetCurrentAdmin().Oid;
               circumcisionReason.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               circumcisionReason.DateModified = DateTime.Now;
               circumcisionReason.IsDeleted = false;
               circumcisionReason.IsSynced = false;

               context.CircumcisionReasons.Update(circumcisionReason);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module });
            }
            else
            {
               if (isCircumcisionReasonDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(circumcisionReason);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var circumcisionReason = await context.CircumcisionReasons
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (circumcisionReason == null)
            return NotFound();

         return View(circumcisionReason);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var circumcisionReason = await context.CircumcisionReasons.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.CircumcisionReasons.Remove(circumcisionReason);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsCircumcisionReasonDuplicate(CircumcisionReason circumcisionReason)
      {
         try
         {
            var circumcisionReasonInDb = context.CircumcisionReasons.FirstOrDefault(c => c.Description.ToLower() == circumcisionReason.Description.ToLower() && c.IsDeleted == false);

            if (circumcisionReasonInDb != null)
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