using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
   public class ICPC2DescriptionsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ICPC2DescriptionsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.ICPC2Descriptions
            .Include(a => a.AnatomicAxis).Include(p => p.PathologyAxis)
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(l => l.Description);

         var iCPC2Descriptions = query.ToPagedList(pageNumber, pageSize);

         if (iCPC2Descriptions.PageCount > 0)
         {
            if (pageNumber > iCPC2Descriptions.PageCount)
               iCPC2Descriptions = query.ToPagedList(iCPC2Descriptions.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(iCPC2Descriptions);
      }
      #endregion

      #region Create
      [HttpGet]
      public IActionResult Create()
      {
         try
         {
            ViewBag.AnatomicAxes = new SelectList(context.AnatomicAxes, FieldConstants.Oid, FieldConstants.AnatomicAxisName);
            ViewBag.PathologyAxes = new SelectList(context.PathologyAxes, FieldConstants.Oid, FieldConstants.PathologyAxisName);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(ICPC2Description iCPC2Description, string? module, string? parent)
      {
         try
         {
            var iCPC2DescriptionInDb = IsICPC2DescriptionDuplicate(iCPC2Description);

            if (!iCPC2DescriptionInDb)
            {
               if (ModelState.IsValid)
               {
                  iCPC2Description.CreatedBy = session?.GetCurrentAdmin().Oid;
                  iCPC2Description.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  iCPC2Description.DateCreated = DateTime.Now;
                  iCPC2Description.IsDeleted = false;
                  iCPC2Description.IsSynced = false;

                  context.ICPC2Descriptions.Add(iCPC2Description);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(iCPC2Description);
            }
            else
            {
               if (iCPC2DescriptionInDb)
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

         var iCPC2DescriptionInDb = await context.ICPC2Descriptions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (iCPC2DescriptionInDb == null)
            return NotFound();

         ViewBag.AnatomicAxes = new SelectList(context.AnatomicAxes, FieldConstants.Oid, FieldConstants.AnatomicAxisName);
         ViewBag.PathologyAxes = new SelectList(context.PathologyAxes, FieldConstants.Oid, FieldConstants.PathologyAxisName);

         return View(iCPC2DescriptionInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ICPC2Description iCPC2Description, string? module, string? parent)
      {
         try
         {
            var iCPC2DescriptionInDb = await context.ICPC2Descriptions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == iCPC2Description.Oid);

            bool isICPC2DescriptionsDuplicate = false;

            if (iCPC2DescriptionInDb.Description != iCPC2Description.Description)
               isICPC2DescriptionsDuplicate = IsICPC2DescriptionDuplicate(iCPC2Description);

            if (!isICPC2DescriptionsDuplicate)
            {
               iCPC2Description.DateCreated = iCPC2DescriptionInDb.DateCreated;
               iCPC2Description.CreatedBy = iCPC2DescriptionInDb.CreatedBy;
               iCPC2Description.ModifiedBy = session?.GetCurrentAdmin().Oid;
               iCPC2Description.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               iCPC2Description.DateModified = DateTime.Now;
               iCPC2Description.IsDeleted = false;
               iCPC2Description.IsSynced = false;

               context.ICPC2Descriptions.Update(iCPC2Description);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isICPC2DescriptionsDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(iCPC2Description);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var iCPC2Description = await context.ICPC2Descriptions
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (iCPC2Description == null)
            return NotFound();

         return View(iCPC2Description);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var iCPC2Description = await context.ICPC2Descriptions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.ICPC2Descriptions.Remove(iCPC2Description);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsICPC2DescriptionDuplicate(ICPC2Description iCPC2Description)
      {
         try
         {
            var iCPC2DescriptionInDb = context.ICPC2Descriptions.FirstOrDefault(p => p.Description.ToLower() == iCPC2Description.Description.ToLower() && p.Oid != iCPC2Description.Oid && p.IsDeleted == false);

            if (iCPC2DescriptionInDb != null)
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