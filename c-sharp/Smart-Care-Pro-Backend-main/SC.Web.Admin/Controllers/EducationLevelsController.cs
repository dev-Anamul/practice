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
   public class EducationLevelsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public EducationLevelsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.EducationLevels
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(l => l.Description);

         var educationLevels = query.ToPagedList(pageNumber, pageSize);

         if (educationLevels.PageCount > 0)
         {
            if (pageNumber > educationLevels.PageCount)
               educationLevels = query.ToPagedList(educationLevels.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(educationLevels);
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
      public async Task<IActionResult> Create(EducationLevel educationLevel)
      {
         try
         {
            var educationLevelInDb = IsEducationLevelDuplicate(educationLevel);

            if (!educationLevelInDb)
            {
               if (ModelState.IsValid)
               {
                  educationLevel.CreatedBy = session?.GetCurrentAdmin().Oid;
                  educationLevel.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  educationLevel.DateCreated = DateTime.Now;
                  educationLevel.IsDeleted = false;
                  educationLevel.IsSynced = false;

                  context.EducationLevels.Add(educationLevel);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = "Clients" });
               }

               return View(educationLevel);
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

         var educationLevelInDb = await context.EducationLevels.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (educationLevelInDb == null)
            return NotFound();

         return View(educationLevelInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, EducationLevel educationLevel)
      {
         try
         {
            var educationLevelInDb = await context.EducationLevels.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == educationLevel.Oid);

            bool isEducationLevelDuplicate = false;

            if (educationLevelInDb.Description != educationLevel.Description)
               isEducationLevelDuplicate = IsEducationLevelDuplicate(educationLevel);

            if (!isEducationLevelDuplicate)
            {
               educationLevel.DateCreated = educationLevelInDb.DateCreated;
               educationLevel.CreatedBy = educationLevelInDb.CreatedBy;
               educationLevel.ModifiedBy = session?.GetCurrentAdmin().Oid;
               educationLevel.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               educationLevel.DateModified = DateTime.Now;
               educationLevel.IsDeleted = false;
               educationLevel.IsSynced = false;

               context.EducationLevels.Update(educationLevel);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Clients" });
            }
            else
            {
               if (isEducationLevelDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(educationLevel);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var educationLevel = await context.EducationLevels
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (educationLevel == null)
            return NotFound();

         return View(educationLevel);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var educationLevel = await context.EducationLevels.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.EducationLevels.Remove(educationLevel);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsEducationLevelDuplicate(EducationLevel educationLevel)
      {
         try
         {
            var educationLevelInDb = context.EducationLevels.FirstOrDefault(c => c.Description.ToLower() == educationLevel.Description.ToLower() && c.IsDeleted == false);

            if (educationLevelInDb != null)
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