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
 * Last modified: 22.05.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class PresentingPartsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public PresentingPartsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.PresentingParts
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false).OrderBy(x => x.Description);

         var presentingParts = query.ToPagedList(pageNumber, pageSize);

         if (presentingParts.PageCount > 0)
         {
            if (pageNumber > presentingParts.PageCount)
               presentingParts = query.ToPagedList(presentingParts.PageCount, pageSize);
         }

         ViewData["Search"] = search;
         return View(presentingParts);
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
      public async Task<IActionResult> Create(PresentingPart presentingPart, string? module)
      {
         try
         {
            var presentingPartInDb = IsPresentingPartDuplicate(presentingPart);

            if (!presentingPartInDb)
            {
               if (ModelState.IsValid)
               {
                  presentingPart.CreatedBy = session?.GetCurrentAdmin().Oid;
                  presentingPart.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  presentingPart.DateCreated = DateTime.Now;
                  presentingPart.IsDeleted = false;
                  presentingPart.IsSynced = false;

                  context.PresentingParts.Add(presentingPart);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module });
               }

               return View(presentingPart);
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

         var presentingPartInDb = await context.PresentingParts.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (presentingPartInDb == null)
            return NotFound();

         return View(presentingPartInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, PresentingPart presentingPart, string? module)
      {
         try
         {
            var presentingPartInDb = await context.PresentingParts.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == presentingPart.Oid);

            bool isPresentingPartDuplicate = false;

            if (presentingPartInDb.Description != presentingPart.Description)
               isPresentingPartDuplicate = IsPresentingPartDuplicate(presentingPart);

            if (!isPresentingPartDuplicate)
            {
               presentingPart.DateCreated = presentingPartInDb.DateCreated;
               presentingPart.CreatedBy = presentingPartInDb.CreatedBy;
               presentingPart.ModifiedBy = session?.GetCurrentAdmin().Oid;
               presentingPart.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               presentingPart.DateModified = DateTime.Now;
               presentingPart.IsDeleted = false;
               presentingPart.IsSynced = false;

               context.PresentingParts.Update(presentingPart);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module });
            }
            else
            {
               if (isPresentingPartDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(presentingPart);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var presentingPart = await context.PresentingParts
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (presentingPart == null)
            return NotFound();

         return View(presentingPart);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var presentingPart = await context.PresentingParts.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.PresentingParts.Remove(presentingPart);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsPresentingPartDuplicate(PresentingPart presentingPart)
      {
         try
         {
            var presentingPartInDb = context.PresentingParts.FirstOrDefault(c => c.Description.ToLower() == presentingPart.Description.ToLower() && c.IsDeleted == false);

            if (presentingPartInDb != null)
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