using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 06.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class CountriesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public CountriesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.Countries
             .Where(x => x.Description.ToLower().Contains(search) || x.CountryCode.ToLower().Contains(search) || x.ISOCodeAlpha2.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var countries = query.ToPagedList(pageNumber, pageSize);

         // Check if the requested page number is greater than the actual page count
         if (countries.PageCount > 0)
         {
            if (pageNumber > countries.PageCount)
               countries = query.ToPagedList(countries.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(countries);
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
      public async Task<IActionResult> Create(Country country, string module)
      {
         try
         {
            ViewBag.Module = module;

            var countryInDb = IsCountryDuplicate(country);
            var IsCountryCodeExist = IsCountryCodeDuplicate(country);
            var IsISOCodeExist = IsCountryISODuplicate(country);

            if (!countryInDb && !IsISOCodeExist && !IsCountryCodeExist)
            {
               //if (ModelState.IsValid)
               //{
               country.CreatedBy = session?.GetCurrentAdmin().Oid;
               country.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
               country.DateCreated = DateTime.Now;
               country.IsDeleted = false;
               country.IsSynced = false;

               context.Countries.Add(country);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

               return RedirectToAction(nameof(Create), new { module = "Clients" });
               //}

               //return View(country);
            }
            else
            {
               if (countryInDb)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
               else if (IsISOCodeExist)
                  ModelState.AddModelError("ISOCodeAlpha2", MessageConstants.DuplicateFound);
               else
                  ModelState.AddModelError("CountryCode", MessageConstants.DuplicateFound);
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

         var country = await context.Countries.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (country == null)
            return NotFound();

         return View(country);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, Country country)
      {
         try
         {
            var countryInDb = await context.Countries.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == country.Oid);

            bool isCountryNameDuplicate = false;
            bool isISOCodeDuplicate = false;
            bool isCountryCodeDuplicate = false;

            if (countryInDb.Description != country.Description)
               isCountryNameDuplicate = IsCountryDuplicate(country);

            if (countryInDb.ISOCodeAlpha2 != country.ISOCodeAlpha2)
               isISOCodeDuplicate = IsCountryISODuplicate(country);

            if (countryInDb.CountryCode != country.CountryCode)
               isCountryCodeDuplicate = IsCountryCodeDuplicate(country);

            if (!isCountryNameDuplicate && !isISOCodeDuplicate && !isCountryCodeDuplicate)
            {
               country.DateCreated = countryInDb.DateCreated;
               country.CreatedBy = countryInDb.CreatedBy;
               country.ModifiedBy = session?.GetCurrentAdmin().Oid;
               country.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               country.DateModified = DateTime.Now;
               country.IsDeleted = false;
               country.IsSynced = false;

               context.Countries.Update(country);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Clients" });
            }
            else
            {
               if (isCountryNameDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);

               if (isISOCodeDuplicate)
                  ModelState.AddModelError("ISOCodeAlpha2", MessageConstants.DuplicateFound);

               if (isCountryCodeDuplicate)
                  ModelState.AddModelError("CountryCode", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(country);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var country = await context.Countries
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (country == null)
            return NotFound();

         return View(country);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var country = await context.Countries.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.Countries.Remove(country);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsCountryDuplicate(Country country)
      {
         try
         {
            var countryInDb = context.Countries.FirstOrDefault(c => c.Description.ToLower() == country.Description.ToLower() && c.IsDeleted == false);

            if (countryInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      public bool IsCountryCodeDuplicate(Country country)
      {
         try
         {
            var countryInDb = context.Countries.FirstOrDefault(c => c.CountryCode.ToLower() == country.CountryCode.ToLower() && c.IsDeleted == false);

            if (countryInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      public bool IsCountryISODuplicate(Country country)
      {
         try
         {
            var countryInDb = context.Countries.FirstOrDefault(c => c.ISOCodeAlpha2.ToLower() == country.ISOCodeAlpha2.ToLower() && c.IsDeleted == false);

            if (countryInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      [HttpGet]
      public async Task<IActionResult> GetAllCountry()
      {
         return Json(await context.Countries.ToListAsync());
      }
      #endregion
   }
}