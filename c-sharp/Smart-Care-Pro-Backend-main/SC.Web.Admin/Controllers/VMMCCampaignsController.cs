using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 26.04.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class VMMCCampaignsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public VMMCCampaignsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.VMMCCampaigns
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var vMMCCampaigns = query.ToPagedList(pageNumber, pageSize);

         if (vMMCCampaigns.PageCount > 0)
         {
            if (pageNumber > vMMCCampaigns.PageCount)
               vMMCCampaigns = query.ToPagedList(vMMCCampaigns.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(vMMCCampaigns);
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
      public async Task<IActionResult> Create(VMMCCampaign vmmcCampaign, string? module)
      {
         try
         {
            var vmmcCampaignInDb = IsVMMCCampaignDuplicate(vmmcCampaign);

            if (!vmmcCampaignInDb)
            {
               if (ModelState.IsValid)
               {
                  vmmcCampaign.CreatedBy = session?.GetCurrentAdmin().Oid;
                  vmmcCampaign.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  vmmcCampaign.DateCreated = DateTime.Now;
                  vmmcCampaign.IsDeleted = false;
                  vmmcCampaign.IsSynced = false;

                  context.VMMCCampaigns.Add(vmmcCampaign);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module });
               }

               return View(vmmcCampaign);
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

         var vmmcCampaignInDb = await context.VMMCCampaigns.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (vmmcCampaignInDb == null)
            return NotFound();

         return View(vmmcCampaignInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, VMMCCampaign vmmcCampaign, string? module)
      {
         try
         {
            var vmmcCampaignInDb = await context.VMMCCampaigns.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == vmmcCampaign.Oid);

            bool isvmmcCampaignDuplicate = false;

            if (vmmcCampaignInDb.Description != vmmcCampaign.Description)
               isvmmcCampaignDuplicate = IsVMMCCampaignDuplicate(vmmcCampaign);

            if (!isvmmcCampaignDuplicate)
            {
               vmmcCampaign.DateCreated = vmmcCampaignInDb.DateCreated;
               vmmcCampaign.CreatedBy = vmmcCampaignInDb.CreatedBy;
               vmmcCampaign.ModifiedBy = session?.GetCurrentAdmin().Oid;
               vmmcCampaign.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               vmmcCampaign.DateModified = DateTime.Now;
               vmmcCampaign.IsDeleted = false;
               vmmcCampaign.IsSynced = false;

               context.VMMCCampaigns.Update(vmmcCampaign);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module });
            }
            else
            {
               if (isvmmcCampaignDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(vmmcCampaign);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var vmmcCampaign = await context.VMMCCampaigns
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (vmmcCampaign == null)
            return NotFound();

         return View(vmmcCampaign);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var vmmcCampaign = await context.VMMCCampaigns.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.VMMCCampaigns.Remove(vmmcCampaign);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsVMMCCampaignDuplicate(VMMCCampaign vmmcCampaign)
      {
         try
         {
            var vmmcCampaignInDb = context.VMMCCampaigns.FirstOrDefault(c => c.Description.ToLower() == vmmcCampaign.Description.ToLower() && c.IsDeleted == false);

            if (vmmcCampaignInDb != null)
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