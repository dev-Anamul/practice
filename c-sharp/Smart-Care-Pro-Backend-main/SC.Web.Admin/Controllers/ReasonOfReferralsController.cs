using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 21.05.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class ReasonOfReferralsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ReasonOfReferralsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.ReasonOfReferrals
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var reasonOfReferrals = query.ToPagedList(pageNumber, pageSize);

         if (reasonOfReferrals.PageCount > 0)
         {
            if (pageNumber > reasonOfReferrals.PageCount)
               reasonOfReferrals = query.ToPagedList(reasonOfReferrals.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(reasonOfReferrals);
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
      public async Task<IActionResult> Create(ReasonOfReferral reasonOfReferral, string? module)
      {
         try
         {
            var reasonOfReferralInDb = IsReasonOfReferralDuplicate(reasonOfReferral);

            if (!reasonOfReferralInDb)
            {
               if (ModelState.IsValid)
               {
                  reasonOfReferral.CreatedBy = session?.GetCurrentAdmin().Oid;
                  reasonOfReferral.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  reasonOfReferral.DateCreated = DateTime.Now;
                  reasonOfReferral.IsDeleted = false;
                  reasonOfReferral.IsSynced = false;

                  context.ReasonOfReferrals.Add(reasonOfReferral);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module });
               }

               return View(reasonOfReferral);
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

         var reasonOfReferralInDb = await context.ReasonOfReferrals.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (reasonOfReferralInDb == null)
            return NotFound();

         return View(reasonOfReferralInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ReasonOfReferral reasonOfReferral, string? module)
      {
         try
         {
            var reasonOfReferralInDb = await context.ReasonOfReferrals.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == reasonOfReferral.Oid);

            bool isReasonOfReferralDuplicate = false;

            if (reasonOfReferralInDb.Description != reasonOfReferral.Description)
               isReasonOfReferralDuplicate = IsReasonOfReferralDuplicate(reasonOfReferral);

            if (!isReasonOfReferralDuplicate)
            {
               reasonOfReferral.DateCreated = reasonOfReferralInDb.DateCreated;
               reasonOfReferral.CreatedBy = reasonOfReferralInDb.CreatedBy;
               reasonOfReferral.ModifiedBy = session?.GetCurrentAdmin().Oid;
               reasonOfReferral.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               reasonOfReferral.DateModified = DateTime.Now;
               reasonOfReferral.IsDeleted = false;
               reasonOfReferral.IsSynced = false;

               context.ReasonOfReferrals.Update(reasonOfReferral);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module });
            }
            else
            {
               if (isReasonOfReferralDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(reasonOfReferral);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var reasonOfReferral = await context.ReasonOfReferrals
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (reasonOfReferral == null)
            return NotFound();

         return View(reasonOfReferral);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var reasonOfReferral = await context.ReasonOfReferrals.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.ReasonOfReferrals.Remove(reasonOfReferral);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsReasonOfReferralDuplicate(ReasonOfReferral reasonOfReferral)
      {
         try
         {
            var reasonOfReferralInDb = context.ReasonOfReferrals.FirstOrDefault(c => c.Description.ToLower() == reasonOfReferral.Description.ToLower() && c.IsDeleted == false);

            if (reasonOfReferralInDb != null)
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