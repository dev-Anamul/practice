using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class PatientTypesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public PatientTypesController(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      #region Index
      public async Task<IActionResult> Index(string? search, int? page, int oid)
      {
         if (oid == 0)
            oid = Convert.ToInt32(TempData["manualId"]);

         ViewBag.Oid = oid;

         var getArmedForceService = await context.ArmedForceServices.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

         TempData["manualId"] = getArmedForceService.Oid;

         ViewBag.ArmedForceServiceId = getArmedForceService.Oid;
         ViewBag.ArmedForceServiceName = getArmedForceService.Description;

         search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;

         if (page > 0 && search == null)
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.DFZPatientTypes
             .Include(d => d.ArmedForceService)
             .Where(x => (x.Description.ToLower().Contains(search) || x.ArmedForceService.Description.ToLower().Contains(search) || search == null) && x.ArmedForceId == oid && x.IsDeleted == false)
             .OrderBy(x => x.ArmedForceService.Description);

         var DfzPetientType = query.ToPagedList(pageNumber, pageSize);

         if (DfzPetientType.PageCount > 0)
         {
            if (pageNumber > DfzPetientType.PageCount)
               DfzPetientType = query.ToPagedList(DfzPetientType.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(DfzPetientType);
      }
      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create(int oid)
      {
         try
         {
            var getArmedForceService = await context.ArmedForceServices.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

            ViewBag.ArmedForceServiceId = getArmedForceService.Oid;
            ViewBag.ArmedForceServiceName = getArmedForceService.Description;
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(DFZPatientType dFZPatientType)
      {
         try
         {
            var dFZPatientTypeInDb = IsPatientTypeDuplicate(dFZPatientType);

            if (!dFZPatientTypeInDb)
            {
               if (ModelState.IsValid)
               {
                  dFZPatientType.CreatedBy = session?.GetCurrentAdmin().Oid;
                  dFZPatientType.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  dFZPatientType.DateCreated = DateTime.Now;
                  dFZPatientType.IsDeleted = false;
                  dFZPatientType.IsSynced = false;

                  context.DFZPatientTypes.Add(dFZPatientType);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;
                  //return RedirectToAction("Create", new { oid = dFZPatientType.ArmedForceId });
                  return RedirectToAction(nameof(Create), new { module = "Clients", oid = dFZPatientType.ArmedForceId });
               }

               return View(dFZPatientType);
            }
            else
            {
               var getArmedForceService = await context.ArmedForceServices.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == dFZPatientType.ArmedForceId);

               ViewBag.ArmedForceServiceId = getArmedForceService.Oid;
               ViewBag.ArmedForceServiceName = getArmedForceService.Description;

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

         var dFZPatientTypeInDb = await context.DFZPatientTypes.Include(a => a.ArmedForceService).AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (dFZPatientTypeInDb == null)
            return NotFound();

         ViewBag.ArmedForceService = new SelectList(context.ArmedForceServices, FieldConstants.Oid, FieldConstants.Description);

         var getArmedForceService = await context.ArmedForceServices.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == dFZPatientTypeInDb.ArmedForceId);

         ViewBag.ArmedForceServiceId = getArmedForceService.Oid;
         ViewBag.ArmedForceServiceName = getArmedForceService.Description;

         return View(dFZPatientTypeInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, DFZPatientType dFZPatientType)
      {
         try
         {
            var dFZPatientTypeInDb = await context.DFZPatientTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == dFZPatientType.Oid);

            bool isDFZPatientTypeDuplicate = false;

            if (dFZPatientTypeInDb.Description != dFZPatientType.Description)
               isDFZPatientTypeDuplicate = IsPatientTypeDuplicate(dFZPatientType);

            if (!isDFZPatientTypeDuplicate)
            {
               dFZPatientType.DateCreated = dFZPatientTypeInDb.DateCreated;
               dFZPatientType.CreatedBy = dFZPatientTypeInDb.CreatedBy;
               dFZPatientType.ModifiedBy = session?.GetCurrentAdmin().Oid;
               dFZPatientType.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               dFZPatientType.DateModified = DateTime.Now;
               dFZPatientType.IsDeleted = false;
               dFZPatientType.IsSynced = false;

               context.DFZPatientTypes.Update(dFZPatientType);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               ViewBag.ArmedForceId = dFZPatientTypeInDb.ArmedForceId;

               //return RedirectToAction("Index", new { oid = dFZPatientType.ArmedForceId });
               return RedirectToAction(nameof(Index), new { module = "Clients", oid = dFZPatientType.ArmedForceId });
            }
            else
            {
               ViewBag.ArmedForceService = new SelectList(context.ArmedForceServices, FieldConstants.Oid, FieldConstants.Description);

               var getArmedForceService = await context.ArmedForceServices.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == dFZPatientTypeInDb.ArmedForceId);

               ViewBag.ArmedForceServiceId = getArmedForceService.Oid;
               ViewBag.ArmedForceServiceName = getArmedForceService.Description;

               if (isDFZPatientTypeDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(dFZPatientType);
      }
      #endregion

      #region Read
      public bool IsPatientTypeDuplicate(DFZPatientType dFZPatientType)
      {
         try
         {
            var dFZPatientTypeInDb = context.DFZPatientTypes.FirstOrDefault(c => c.Description.ToLower() == dFZPatientType.Description.ToLower() && c.IsDeleted == false);

            if (dFZPatientTypeInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }

      //[HttpGet]
      //public async Task<IActionResult> districtByProvince(int ProvinceId)
      //{
      //    return Json(await context.Districts.Where(d => d.IsDeleted == false && d.ProvinceId == ProvinceId).ToListAsync());
      //}
      #endregion
   }
}
