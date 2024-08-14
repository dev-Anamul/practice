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
    public class InterCourseStatusController : Controller
    {
        private readonly DataContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private ISession? session => httpContextAccessor.HttpContext?.Session;
        public InterCourseStatusController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

            var query = context.InterCourseStatuses
                .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
                .OrderBy(l => l.Description);

            var interCourseStatus = query.ToPagedList(pageNumber, pageSize);

            if (interCourseStatus.PageCount > 0)
            {
                if (pageNumber > interCourseStatus.PageCount)
                    interCourseStatus = query.ToPagedList(interCourseStatus.PageCount, pageSize);
            }

            ViewData["Search"] = search;

            return View(interCourseStatus);
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
        public async Task<IActionResult> Create(InterCourseStatus interCourseStatus, string? module)
        {
            try
            {
                var interCourseStatusInDb = IsTestingReasonDuplicate(interCourseStatus);

                if (!interCourseStatusInDb)
                {
                    if (ModelState.IsValid)
                    {
                        interCourseStatus.CreatedBy = session?.GetCurrentAdmin().Oid;
                        interCourseStatus.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                        interCourseStatus.DateCreated = DateTime.Now;
                        interCourseStatus.IsDeleted = false;
                        interCourseStatus.IsSynced = false;

                        context.InterCourseStatuses.Add(interCourseStatus);
                        await context.SaveChangesAsync();

                        TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                        return RedirectToAction(nameof(Create), new { module = module });
                    }

                    return View(interCourseStatus);
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

            var interCourseStatusInDb = await context.InterCourseStatuses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

            if (interCourseStatusInDb == null)
                return NotFound();

            return View(interCourseStatusInDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,InterCourseStatus interCourseStatus, string? module)
        {
            try
            {
                var interCourseStatusInDb = await context.InterCourseStatuses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == interCourseStatus.Oid);

                bool isTestingReasonDuplicate = false;

                if (interCourseStatusInDb.Description != interCourseStatus.Description)
                    isTestingReasonDuplicate = IsTestingReasonDuplicate(interCourseStatus);

                if (!isTestingReasonDuplicate)
                {
                    interCourseStatus.DateCreated = interCourseStatusInDb.DateCreated;
                    interCourseStatus.CreatedBy = interCourseStatusInDb.CreatedBy;
                    interCourseStatus.ModifiedBy = session?.GetCurrentAdmin().Oid;
                    interCourseStatus.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
                    interCourseStatus.DateModified = DateTime.Now;
                    interCourseStatus.IsDeleted = false;
                    interCourseStatus.IsSynced = false;

                    context.InterCourseStatuses.Update(interCourseStatus);
                    await context.SaveChangesAsync();

                    TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

                    return RedirectToAction(nameof(Index), new { module = module });
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

            return View(interCourseStatus);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var interCourseStatus = await context.InterCourseStatuses
                .FirstOrDefaultAsync(c => c.Oid == id);

            if (interCourseStatus == null)
                return NotFound();

            return View(interCourseStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var interCourseStatus = await context.InterCourseStatuses.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

            context.InterCourseStatuses.Remove(interCourseStatus);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Read
        public bool IsTestingReasonDuplicate(InterCourseStatus interCourseStatus)
        {
            try
            {
                var interCourseStatusInDb = context.InterCourseStatuses.FirstOrDefault(c => c.Description.ToLower() == interCourseStatus.Description.ToLower() && c.IsDeleted == false);

                if (interCourseStatusInDb != null)
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
