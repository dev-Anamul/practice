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
 * Date created : 21.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class VaccinesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public VaccinesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.Vaccines
            .Include(d => d.VaccineType)
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var vaccines = query.ToPagedList(pageNumber, pageSize);

         if (vaccines.PageCount > 0)
         {
            if (pageNumber > vaccines.PageCount)
               vaccines = query.ToPagedList(vaccines.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(vaccines);
      }
      #endregion

      #region Create
      [HttpGet]
      public IActionResult Create()
      {
         try
         {
            ViewBag.VaccineType = new SelectList(context.VaccineTypes, FieldConstants.Oid, FieldConstants.Description);
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(Vaccine vaccine, string? module, string? parent)
      {
         try
         {
            var vaccineInDb = IsVaccineDuplicate(vaccine);

            if (!vaccineInDb)
            {
               if (ModelState.IsValid)
               {
                  vaccine.CreatedBy = session?.GetCurrentAdmin().Oid;
                  vaccine.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  vaccine.DateCreated = DateTime.Now;
                  vaccine.IsDeleted = false;
                  vaccine.IsSynced = false;

                  context.Vaccines.Add(vaccine);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(vaccine);
            }
            else
            {
               ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
               ViewBag.VaccineType = new SelectList(context.VaccineTypes, FieldConstants.Oid, FieldConstants.Description);
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

         var vaccineInDb = await context.Vaccines.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (vaccineInDb == null)
            return NotFound();

         ViewBag.VaccineType = new SelectList(context.VaccineTypes, FieldConstants.Oid, FieldConstants.Description);

         return View(vaccineInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, Vaccine vaccine, string? module, string? parent)
      {
         try
         {
            var vaccineInDb = await context.Vaccines.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == vaccine.Oid);

            bool isVaccineDuplicate = false;

            if (vaccineInDb.Description != vaccine.Description)
               isVaccineDuplicate = IsVaccineDuplicate(vaccine);

            if (!isVaccineDuplicate)
            {
               vaccine.DateCreated = vaccineInDb.DateCreated;
               vaccine.CreatedBy = vaccineInDb.CreatedBy;
               vaccine.ModifiedBy = session?.GetCurrentAdmin().Oid;
               vaccine.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               vaccine.DateModified = DateTime.Now;
               vaccine.IsDeleted = false;
               vaccine.IsSynced = false;

               context.Vaccines.Update(vaccine);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isVaccineDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);

               ViewBag.VaccineType = new SelectList(context.VaccineTypes, FieldConstants.Oid, FieldConstants.Description);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(vaccine);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var vaccine = await context.Vaccines
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (vaccine == null)
            return NotFound();

         return View(vaccine);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var vaccine = await context.Vaccines.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.Vaccines.Remove(vaccine);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsVaccineDuplicate(Vaccine vaccine)
      {
         try
         {
            var vaccineInDb = context.Vaccines.FirstOrDefault(c => c.Description.ToLower() == vaccine.Description.ToLower() && c.Oid != vaccine.Oid && c.IsDeleted == false);

            if (vaccineInDb != null)
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