using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 21.05.2023
 * Modified by  : Bella
 * Last modified: 15.11.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class UserAccessesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public UserAccessesController(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      [HttpGet]
      public IActionResult Index(string? search, int? page)
      {
         search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;

         if (page > 0 && search == null)
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.FacilityAccesses
             .Include(fa => fa.UserAccount)
             .Include(fa => fa.Facility)
             .Where(fa => fa.UserAccount.UserType != Utilities.Constants.Enums.UserType.SystemAdministrator &&
                         (fa.UserAccount.Cellphone.ToLower().Contains(search) ||
                          fa.UserAccount.FirstName.ToLower().Contains(search) ||
                          fa.UserAccount.Surname.ToLower().Contains(search) ||
                          search == null && fa.UserAccount.IsDeleted == false))
             .OrderBy(fa => fa.UserAccount.Surname);

         var facilityAccesses = query.ToPagedList(pageNumber, pageSize);

         if (facilityAccesses.PageCount > 0)
         {
            if (pageNumber > facilityAccesses.PageCount)
               facilityAccesses = query.ToPagedList(facilityAccesses.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(facilityAccesses);
      }

      [HttpPost]
      public async Task<IActionResult> Index(Guid? id)
      {
         try
         {
            if (id == null)
               return NotFound();

            var userAccount = await context.UserAccounts.FindAsync(id);

            if (userAccount == null)
               return NotFound();

            var facilityAccess = await context.FacilityAccesses.Include(fa => fa.Facility).FirstOrDefaultAsync(fa => fa.UserAccountId == id);

            if (facilityAccess == null)
               return NotFound();

            //var facilityId = facilityAccess.FacilityId;

            //var existingAdministrator = await context.FacilityAccesses
            //    .FirstOrDefaultAsync(fa => fa.FacilityId == facilityId && fa.UserAccountId != id && fa.UserAccount.UserType == Utilities.Constants.Enums.UserType.FacilityAdministrator);

            //if (existingAdministrator != null)
            //{
            //   TempData[SessionConstants.Message] = MessageConstants.FacilityAdministratorTaken;
            //   return RedirectToAction("Index");
            //}
            //else
            //{
            userAccount.UserType = Utilities.Constants.Enums.UserType.FacilityAdministrator;

            context.UserAccounts.Update(userAccount);
            await context.SaveChangesAsync();
            //}

            facilityAccess.IsApproved = true;
            facilityAccess.DateApproved = DateTime.Now;
            facilityAccess.DateModified = DateTime.Now;
            facilityAccess.ModifiedBy = facilityAccess.ModifiedBy;

            context.FacilityAccesses.Update(facilityAccess);
            await context.SaveChangesAsync();

            TempData[SessionConstants.Message] = MessageConstants.MadeFacilityAdministratorSuccessfully;

            return RedirectToAction("Index");
         }
         catch (Exception ex)
         {
            throw;
         }
      }
   }
}