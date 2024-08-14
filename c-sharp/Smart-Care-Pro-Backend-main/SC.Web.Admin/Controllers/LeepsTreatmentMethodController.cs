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
    public class LeepsTreatmentMethodController : Controller
    {
        private readonly DataContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private ISession? session => httpContextAccessor.HttpContext?.Session;
        public LeepsTreatmentMethodController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

            var query = context.LeepsTreatmentMethods
                .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
                .OrderBy(l => l.Description);

            var leepsTreatmentMethod = query.ToPagedList(pageNumber, pageSize);

            if (leepsTreatmentMethod.PageCount > 0)
            {
                if (pageNumber > leepsTreatmentMethod.PageCount)
                    leepsTreatmentMethod = query.ToPagedList(leepsTreatmentMethod.PageCount, pageSize);
            }

            ViewData["Search"] = search;

            return View(leepsTreatmentMethod);
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
        public async Task<IActionResult> Create(LeepsTreatmentMethod leepsTreatmentMethod, string? module)
        {
            try
            {
                var leepsTreatmentMethodInDb = IsTestingReasonDuplicate(leepsTreatmentMethod);

                if (!leepsTreatmentMethodInDb)
                {
                    if (ModelState.IsValid)
                    {
                        leepsTreatmentMethod.CreatedBy = session?.GetCurrentAdmin().Oid;
                        leepsTreatmentMethod.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                        leepsTreatmentMethod.DateCreated = DateTime.Now;
                        leepsTreatmentMethod.IsDeleted = false;
                        leepsTreatmentMethod.IsSynced = false;

                        context.LeepsTreatmentMethods.Add(leepsTreatmentMethod);
                        await context.SaveChangesAsync();

                        TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                        return RedirectToAction(nameof(Create), new { module = module });
                    }

                    return View(leepsTreatmentMethod);
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

            var leepsTreatmentMethodInDb = await context.LeepsTreatmentMethods.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

            if (leepsTreatmentMethodInDb == null)
                return NotFound();

            return View(leepsTreatmentMethodInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeepsTreatmentMethod leepsTreatmentMethod, string? module)
        {
            try
            {
                var leepsTreatmentMethodInDb = await context.LeepsTreatmentMethods.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == leepsTreatmentMethod.Oid);

                bool isTestingReasonDuplicate = false;

                if (leepsTreatmentMethodInDb.Description != leepsTreatmentMethod.Description)
                    isTestingReasonDuplicate = IsTestingReasonDuplicate(leepsTreatmentMethod);

                if (!isTestingReasonDuplicate)
                {
                    leepsTreatmentMethod.DateCreated = leepsTreatmentMethodInDb.DateCreated;
                    leepsTreatmentMethod.CreatedBy = leepsTreatmentMethodInDb.CreatedBy;
                    leepsTreatmentMethod.ModifiedBy = session?.GetCurrentAdmin().Oid;
                    leepsTreatmentMethod.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
                    leepsTreatmentMethod.DateModified = DateTime.Now;
                    leepsTreatmentMethod.IsDeleted = false;
                    leepsTreatmentMethod.IsSynced = false;

                    context.LeepsTreatmentMethods.Update(leepsTreatmentMethod);
                    await context.SaveChangesAsync();

                    TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

                      return RedirectToAction(nameof(Create), new { module = module });
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

            return View(leepsTreatmentMethod);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var leepsTreatmentMethod = await context.LeepsTreatmentMethods
                .FirstOrDefaultAsync(c => c.Oid == id);

            if (leepsTreatmentMethod == null)
                return NotFound();

            return View(leepsTreatmentMethod);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var leepsTreatmentMethod = await context.LeepsTreatmentMethods.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

            context.LeepsTreatmentMethods.Remove(leepsTreatmentMethod);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region Read
        public bool IsTestingReasonDuplicate(LeepsTreatmentMethod leepsTreatmentMethod)
        {
            try
            {
                var leepsTreatmentMethodInDb = context.LeepsTreatmentMethods.FirstOrDefault(c => c.Description.ToLower() == leepsTreatmentMethod.Description.ToLower() && c.IsDeleted == false);

                if (leepsTreatmentMethodInDb != null)
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
