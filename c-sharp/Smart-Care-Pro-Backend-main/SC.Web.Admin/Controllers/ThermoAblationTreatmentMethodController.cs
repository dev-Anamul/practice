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
    public class ThermoAblationTreatmentMethodController : Controller
    {
        private readonly DataContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private ISession? session => httpContextAccessor.HttpContext?.Session;
        public ThermoAblationTreatmentMethodController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

            var query = context.ThermoAblationTreatmentMethods
                .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
                .OrderBy(l => l.Description);

            var thermoAblationTreatmentMethod = query.ToPagedList(pageNumber, pageSize);

            if (thermoAblationTreatmentMethod.PageCount > 0)
            {
                if (pageNumber > thermoAblationTreatmentMethod.PageCount)
                    thermoAblationTreatmentMethod = query.ToPagedList(thermoAblationTreatmentMethod.PageCount, pageSize);
            }

            ViewData["Search"] = search;

            return View(thermoAblationTreatmentMethod);
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
        public async Task<IActionResult> Create(ThermoAblationTreatmentMethod thermoAblationTreatmentMethod)
        {
            try
            {
                var thermoAblationTreatmentMethodInDb = IsTestingReasonDuplicate(thermoAblationTreatmentMethod);

                if (!thermoAblationTreatmentMethodInDb)
                {
                    if (ModelState.IsValid)
                    {
                        thermoAblationTreatmentMethod.CreatedBy = session?.GetCurrentAdmin().Oid;
                        thermoAblationTreatmentMethod.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                        thermoAblationTreatmentMethod.DateCreated = DateTime.Now;
                        thermoAblationTreatmentMethod.IsDeleted = false;
                        thermoAblationTreatmentMethod.IsSynced = false;

                        context.ThermoAblationTreatmentMethods.Add(thermoAblationTreatmentMethod);
                        await context.SaveChangesAsync();

                        TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                        return RedirectToAction(nameof(Create), new { module = "CervicalCancer" });
                    }

                    return View(thermoAblationTreatmentMethod);
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

            var thermoAblationTreatmentMethodInDb = await context.ThermoAblationTreatmentMethods.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

            if (thermoAblationTreatmentMethodInDb == null)
                return NotFound();

            return View(thermoAblationTreatmentMethodInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ThermoAblationTreatmentMethod thermoAblationTreatmentMethod)
        {
            try
            {
                var thermoAblationTreatmentMethodInDb = await context.ThermoAblationTreatmentMethods.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == thermoAblationTreatmentMethod.Oid);

                bool isTestingReasonDuplicate = false;

                if (thermoAblationTreatmentMethodInDb.Description != thermoAblationTreatmentMethod.Description)
                    isTestingReasonDuplicate = IsTestingReasonDuplicate(thermoAblationTreatmentMethod);

                if (!isTestingReasonDuplicate)
                {
                    thermoAblationTreatmentMethod.DateCreated = thermoAblationTreatmentMethodInDb.DateCreated;
                    thermoAblationTreatmentMethod.CreatedBy = thermoAblationTreatmentMethodInDb.CreatedBy;
                    thermoAblationTreatmentMethod.ModifiedBy = session?.GetCurrentAdmin().Oid;
                    thermoAblationTreatmentMethod.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
                    thermoAblationTreatmentMethod.DateModified = DateTime.Now;
                    thermoAblationTreatmentMethod.IsDeleted = false;
                    thermoAblationTreatmentMethod.IsSynced = false;

                    context.ThermoAblationTreatmentMethods.Update(thermoAblationTreatmentMethod);
                    await context.SaveChangesAsync();

                    TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

                    return RedirectToAction(nameof(Create), new { module = "CervicalCancer" });
                }
                else
                {
                    if (isTestingReasonDuplicate)
                        ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(thermoAblationTreatmentMethod);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var thermoAblationTreatmentMethod = await context.ThermoAblationTreatmentMethods
                .FirstOrDefaultAsync(c => c.Oid == id);

            if (thermoAblationTreatmentMethod == null)
                return NotFound();

            return View(thermoAblationTreatmentMethod);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var thermoAblationTreatmentMethod = await context.ThermoAblationTreatmentMethods.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

            context.ThermoAblationTreatmentMethods.Remove(thermoAblationTreatmentMethod);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region Read
        public bool IsTestingReasonDuplicate(ThermoAblationTreatmentMethod thermoAblationTreatmentMethod)
        {
            try
            {
                var thermoAblationTreatmentMethodInDb = context.ThermoAblationTreatmentMethods.FirstOrDefault(c => c.Description.ToLower() == thermoAblationTreatmentMethod.Description.ToLower() && c.IsDeleted == false);

                if (thermoAblationTreatmentMethodInDb != null)
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
