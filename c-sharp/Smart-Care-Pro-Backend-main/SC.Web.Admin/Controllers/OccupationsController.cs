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
   public class OccupationsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public OccupationsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.Occupations
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(l => l.Description);

         var occupations = query.ToPagedList(pageNumber, pageSize);

         if (occupations.PageCount > 0)
         {
            if (pageNumber > occupations.PageCount)
               occupations = query.ToPagedList(occupations.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(occupations);
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
      public async Task<IActionResult> Create(Occupation occupation)
      {
         try
         {
            var occupationInDb = IsOccupationDuplicate(occupation);

            if (!occupationInDb)
            {
               if (ModelState.IsValid)
               {
                  occupation.CreatedBy = session?.GetCurrentAdmin().Oid;
                  occupation.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  occupation.DateCreated = DateTime.Now;
                  occupation.IsDeleted = false;
                  occupation.IsSynced = false;

                  context.Occupations.Add(occupation);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  return RedirectToAction(nameof(Create), new { module = "Clients" });
               }

               return View(occupation);
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

         var occupationInDb = await context.Occupations.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (occupationInDb == null)
            return NotFound();

         return View(occupationInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, Occupation occupation)
      {
         try
         {
            var occupationInDb = await context.Occupations.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == occupation.Oid);

            bool isOccupationDuplicate = false;

            if (occupationInDb.Description != occupation.Description)
               isOccupationDuplicate = IsOccupationDuplicate(occupation);

            if (!isOccupationDuplicate)
            {
               occupation.DateCreated = occupationInDb.DateCreated;
               occupation.CreatedBy = occupationInDb.CreatedBy;
               occupation.ModifiedBy = session?.GetCurrentAdmin().Oid;
               occupation.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               occupation.DateModified = DateTime.Now;
               occupation.IsDeleted = false;
               occupation.IsSynced = false;

               context.Occupations.Update(occupation);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Clients" });
            }
            else
            {
               if (isOccupationDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(occupation);
      }
      #endregion

      #region Read
      public bool IsOccupationDuplicate(Occupation occupation)
      {
         try
         {
            var occupationInDb = context.Occupations.FirstOrDefault(c => c.Description.ToLower() == occupation.Description.ToLower() && c.IsDeleted == false);

            if (occupationInDb != null)
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