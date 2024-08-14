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
   public class VaccineDosesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public VaccineDosesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.VaccineDoses
            .Include(v => v.Vaccine)
            .ThenInclude(p => p.VaccineType)
            .Where(x => x.Description.ToLower().Contains(search) || x.Vaccine.Description.ToLower().Contains(search) || x.Vaccine.VaccineType.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var vaccineDoses = query.ToPagedList(pageNumber, pageSize);

         if (vaccineDoses.PageCount > 0)
         {
            if (pageNumber > vaccineDoses.PageCount)
               vaccineDoses = query.ToPagedList(vaccineDoses.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(vaccineDoses);
      }
      #endregion

      #region Create
      [HttpGet]
      public IActionResult Create()
      {
         try
         {
            ViewBag.Vaccine = new SelectList(context.Vaccines, FieldConstants.Oid, FieldConstants.Description);
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
      public async Task<IActionResult> Create(VaccineDose vaccineDose, string? module, string? parent)
      {
         try
         {
            if (ModelState.IsValid)
            {
               vaccineDose.CreatedBy = session?.GetCurrentAdmin().Oid;
               vaccineDose.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
               vaccineDose.DateCreated = DateTime.Now;
               vaccineDose.IsDeleted = false;
               vaccineDose.IsSynced = false;

               context.VaccineDoses.Add(vaccineDose);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

               return RedirectToAction(nameof(Create), new { module = module, parent = parent });
            }

            return View(vaccineDose);
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

         var vaccineDose = await context.VaccineDoses.Include(t => t.Vaccine).ThenInclude(d => d.VaccineType).AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);
         var vaccine = context.Vaccines.ToList();

         if (vaccineDose == null)
            return NotFound();

         ViewBag.VaccineType = new SelectList(context.VaccineTypes, FieldConstants.Oid, FieldConstants.Description, vaccineDose.Vaccine.VaccineType.Oid);
         ViewBag.Vaccine = new SelectList(vaccine.Where(d => d.VaccineTypeId == vaccineDose.Vaccine.VaccineTypeId).ToList(), FieldConstants.Oid, FieldConstants.Description, vaccineDose.VaccineId);

         return View(vaccineDose);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, VaccineDose vaccineDose, string? module, string? parent)
      {
         try
         {
            var vaccineDoseInDb = await context.VaccineDoses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == vaccineDose.Oid);

            if (ModelState.IsValid)
            {
               vaccineDose.DateCreated = vaccineDoseInDb.DateCreated;
               vaccineDose.CreatedBy = vaccineDoseInDb.CreatedBy;
               vaccineDose.ModifiedBy = session?.GetCurrentAdmin().Oid;
               vaccineDose.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               vaccineDose.DateModified = DateTime.Now;
               vaccineDose.IsDeleted = false;
               vaccineDose.IsSynced = false;

               context.VaccineDoses.Update(vaccineDose);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(vaccineDose);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var vaccineDose = await context.VaccineDoses
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (vaccineDose == null)
            return NotFound();

         return View(vaccineDose);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var vaccineDose = await context.VaccineDoses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.VaccineDoses.Remove(vaccineDose);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsVaccineDoseDuplicate(VaccineDose vaccineDose)
      {
         try
         {
            var vaccineInDb = context.VaccineDoses.FirstOrDefault(c => c.Description.ToLower() == vaccineDose.Description.ToLower() && c.IsDeleted == false);

            if (vaccineInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      public JsonResult LoadVaccine(int id)
      {
         var vaccines = context.Vaccines.Where(p => p.VaccineTypeId == id && p.IsDeleted == false).ToList();
         return Json(vaccines);
      }
      #endregion
   }
}