using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 25.04.2023
 * Modified by  : Bella
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class ComplicationTypesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ComplicationTypesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.ComplicationTypes
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var complicationTypes = query.ToPagedList(pageNumber, pageSize);

         if (complicationTypes.PageCount > 0)
         {
            if (pageNumber > complicationTypes.PageCount)
               complicationTypes = query.ToPagedList(complicationTypes.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(complicationTypes);
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
      public async Task<IActionResult> Create(ComplicationType complicationType, string? module)
      {
         try
         {
            var complicationTypeInDb = IsComplicationTypeDuplicate(complicationType);

            if (!complicationTypeInDb)
            {
               if (ModelState.IsValid)
               {
                  complicationType.CreatedBy = session?.GetCurrentAdmin().Oid;
                  complicationType.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  complicationType.DateCreated = DateTime.Now;
                  complicationType.IsDeleted = false;
                  complicationType.IsSynced = false;

                  context.ComplicationTypes.Add(complicationType);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module });
               }

               return View(complicationType);
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

         var complicationType = await context.ComplicationTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (complicationType == null)
            return NotFound();

         return View(complicationType);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ComplicationType complicationType, string? module)
      {
         try
         {
            var complicationTypeInDb = await context.ComplicationTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == complicationType.Oid);

            bool isComplicationTypeDuplicate = false;

            if (complicationTypeInDb.Description != complicationType.Description)
               isComplicationTypeDuplicate = IsComplicationTypeDuplicate(complicationType);

            if (!isComplicationTypeDuplicate)
            {
               complicationType.DateCreated = complicationTypeInDb.DateCreated;
               complicationType.CreatedBy = complicationTypeInDb.CreatedBy;
               complicationType.ModifiedBy = session?.GetCurrentAdmin().Oid;
               complicationType.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               complicationType.DateModified = DateTime.Now;
               complicationType.IsDeleted = false;
               complicationType.IsSynced = false;

               context.ComplicationTypes.Update(complicationType);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module });
            }
            else
            {
               if (isComplicationTypeDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(complicationType);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var complicationType = await context.ComplicationTypes
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (complicationType == null)
            return NotFound();

         return View(complicationType);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var complicationType = await context.ComplicationTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.ComplicationTypes.Remove(complicationType);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsComplicationTypeDuplicate(ComplicationType complicationType)
      {
         try
         {
            var complicationTypeInDb = context.ComplicationTypes.FirstOrDefault(c => c.Description.ToLower() == complicationType.Description.ToLower() && c.IsDeleted == false);

            if (complicationTypeInDb != null)
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