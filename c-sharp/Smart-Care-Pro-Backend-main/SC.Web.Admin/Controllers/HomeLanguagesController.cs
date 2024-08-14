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
   public class HomeLanguagesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public HomeLanguagesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.HomeLanguages
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(l => l.Description);

         var homeLanguages = query.ToPagedList(pageNumber, pageSize);

         if (homeLanguages.PageCount > 0)
         {
            if (pageNumber > homeLanguages.PageCount)
               homeLanguages = query.ToPagedList(homeLanguages.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(homeLanguages);
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
      public async Task<IActionResult> Create(HomeLanguage homeLanguage)
      {
         try
         {
            var homeLanguageInDb = IsHomeLanguageDuplicate(homeLanguage);

            if (!homeLanguageInDb)
            {
               if (ModelState.IsValid)
               {
                  homeLanguage.CreatedBy = session?.GetCurrentAdmin().Oid;
                  homeLanguage.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  homeLanguage.DateCreated = DateTime.Now;
                  homeLanguage.IsDeleted = false;
                  homeLanguage.IsSynced = false;

                  context.HomeLanguages.Add(homeLanguage);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = "Clients" });
               }

               return View(homeLanguage);
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

         var homeLanguageInDb = await context.HomeLanguages.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (homeLanguageInDb == null)
            return NotFound();

         return View(homeLanguageInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, HomeLanguage homeLanguage)
      {
         try
         {
            var languageModel = await context.HomeLanguages.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == homeLanguage.Oid);

            bool isHomeLanguageDuplicate = false;

            if (languageModel.Description != homeLanguage.Description)
               isHomeLanguageDuplicate = IsHomeLanguageDuplicate(homeLanguage);

            if (!isHomeLanguageDuplicate)
            {
               homeLanguage.DateCreated = languageModel.DateCreated;
               homeLanguage.CreatedBy = languageModel.CreatedBy;
               homeLanguage.ModifiedBy = session?.GetCurrentAdmin().Oid;
               homeLanguage.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               homeLanguage.DateModified = DateTime.Now;
               homeLanguage.IsDeleted = false;
               homeLanguage.IsSynced = false;

               context.HomeLanguages.Update(homeLanguage);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Clients" });
            }
            else
            {
               if (isHomeLanguageDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(homeLanguage);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var homeLanguage = await context.HomeLanguages
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (homeLanguage == null)
            return NotFound();

         return View(homeLanguage);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var homeLanguage = await context.HomeLanguages.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.HomeLanguages.Remove(homeLanguage);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read

      public bool IsHomeLanguageDuplicate(HomeLanguage homeLanguage)
      {
         try
         {
            var homeLanguageInDb = context.HomeLanguages.FirstOrDefault(c => c.Description.ToLower() == homeLanguage.Description.ToLower() && c.IsDeleted == false);

            if (homeLanguageInDb != null)
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