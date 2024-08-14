using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class DFZRanksController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public DFZRanksController(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      #region Index
      public async Task<IActionResult> Index(string? search, int? page, int oid)
      {
         if (oid == 0)
            oid = Convert.ToInt32(TempData["manualPatientTypeId"]);

         ViewBag.Oid = oid;

         var getPatientTypes = await context.DFZPatientTypes.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

         TempData["manualPatientTypeId"] = getPatientTypes.Oid;

         ViewBag.ArmedForceId = getPatientTypes.ArmedForceId;
         ViewBag.PatientTypeId = getPatientTypes.Oid;
         ViewBag.PatientType = getPatientTypes.Description;

         search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;

         if (page > 0 && search == null)
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.DFZRanks
             .Include(d => d.DFZPatientType)
             .ThenInclude(p => p.ArmedForceService)
             .Where(x => (x.Description.ToLower().Contains(search) || x.DFZPatientType.Description.ToLower().Contains(search) || x.DFZPatientType.ArmedForceService.Description.ToLower().Contains(search) || search == null) && x.PatientTypeId == oid && x.IsDeleted == false)
             .OrderBy(x => x.DFZPatientType.ArmedForceService.Description)
             .ThenBy(x => x.DFZPatientType.Description)
             .ThenBy(x => x.Description);

         var dFZRanks = query.ToPagedList(pageNumber, pageSize);

         if (dFZRanks.PageCount > 0)
         {
            if (pageNumber > dFZRanks.PageCount)
               dFZRanks = query.ToPagedList(dFZRanks.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(dFZRanks);
      }
      #endregion

      #region Create
      [HttpGet]
      public async Task<IActionResult> Create(int? oid)
      {
         try
         {
            var getPatientTypes = await context.DFZPatientTypes.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

            ViewBag.PatientTypeId = getPatientTypes.Oid;
            ViewBag.PatientType = getPatientTypes.Description;
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(DFZRank dFZRank)
      {
         try
         {
            var dFZRankInDb = IsDFZRabkDuplicate(dFZRank);


            if (!dFZRankInDb)
            {
               if (ModelState.IsValid)
               {
                  dFZRank.CreatedBy = session?.GetCurrentAdmin().Oid;
                  dFZRank.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  dFZRank.DateCreated = DateTime.Now;
                  dFZRank.IsDeleted = false;
                  dFZRank.IsSynced = false;

                  context.DFZRanks.Add(dFZRank);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  //return RedirectToAction("Create", new { oid = dFZRank.PatientTypeId });
                  return RedirectToAction(nameof(Create), new { module = "Clients", oid = dFZRank.PatientTypeId });
               }

               return View(dFZRank);
            }
            else
            {
               var getPatientTypes = await context.Districts.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == dFZRank.PatientTypeId);

               ViewBag.PatientTypeId = getPatientTypes.Oid;
               ViewBag.PatientType = getPatientTypes.Description;

               if (dFZRankInDb)
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

         var dFZRank = await context.DFZRanks.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (dFZRank == null)
            return NotFound();

         var getPatientTypes = await context.DFZPatientTypes.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == dFZRank.PatientTypeId);

         ViewBag.PatientTypeId = getPatientTypes.Oid;
         ViewBag.PatientType = getPatientTypes.Description;

         return View(dFZRank);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, DFZRank dFZRank)
      {
         try
         {
            var dFZRankInDb = await context.DFZRanks.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == dFZRank.Oid);

            bool isDFZRankDuplicate = false;

            if (dFZRankInDb.Description != dFZRank.Description)
               isDFZRankDuplicate = IsDFZRabkDuplicate(dFZRank);


            if (!isDFZRankDuplicate)
            {
               dFZRank.DateCreated = dFZRankInDb.DateCreated;
               dFZRank.CreatedBy = dFZRankInDb.CreatedBy;
               dFZRank.ModifiedBy = session?.GetCurrentAdmin().Oid;
               dFZRank.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               dFZRank.DateModified = DateTime.Now;
               dFZRank.IsDeleted = false;
               dFZRank.IsSynced = false;

               context.DFZRanks.Update(dFZRank);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = "Clients" });
            }
            else
            {
               if (isDFZRankDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);

               var getPatientTypes = await context.DFZPatientTypes.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == dFZRankInDb.PatientTypeId);

               ViewBag.PatientTypeId = getPatientTypes.Oid;
               ViewBag.PatientType = getPatientTypes.Description;
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(dFZRank);
      }
      #endregion

      public bool IsDFZRabkDuplicate(DFZRank dFZRank)
      {
         try
         {
            var dFZRankInDb = context.DFZRanks.FirstOrDefault(c => c.Description.ToLower() == dFZRank.Description.ToLower() && c.PatientTypeId == dFZRank.PatientTypeId && c.IsDeleted == false);

            if (dFZRankInDb != null)
               return true;

            return false;
         }
         catch
         {
            throw;
         }
      }
   }
}
