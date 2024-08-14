using Domain.Dto;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using Utilities.Encryptions;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 09.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   public class HomeController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public HomeController(DataContext context, IHttpContextAccessor httpContextAccessor, ILogger<HomeController> logger)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      #region Login

      [HttpGet]
      public IActionResult Index()
      {
         return View();
      }

      [HttpPost]
      public IActionResult Index(UserLoginDto userLogin)
      {
         bool loginFlag = false;

         try
         {
            if (ModelState.IsValid)
            {
               var userInDb = context.UserAccounts
                   .AsNoTracking()
                   .Where(u => u.Username.ToLower() == userLogin.Username.ToLower() && u.IsAccountActive == true)
                   .FirstOrDefault();

               if (userInDb != null)
               {
                  if (userInDb.UserType == UserType.SystemAdministrator)
                  {
                     EncryptionHelpers encryptionHelpers = new EncryptionHelpers();

                     string decryptedPassword = encryptionHelpers.Decrypt(userInDb.Password);

                     if (decryptedPassword == userLogin.Password)
                     {
                        loginFlag = true;
                     }
                  }
               }

               if (loginFlag == true)
               {
                  session?.SetCurrentAdmin(userInDb);
                  return RedirectToAction("Index", "UserAccesses", new { module = "User Management" });
               }
               else
               {
                  TempData[SessionConstants.Message] = MessageConstants.InvalidLogin;
               }
            }
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", ex.Message);
         }

         return View();
      }

      #endregion

      #region Logout
      [HttpGet]
      public async Task<ActionResult> Logout()
      {
         try
         {
            session?.Clear();
            TempData.Clear();
            //await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index));
         }
         catch (Exception)
         {
            throw;
         }
      }

      public IActionResult UnauthorizedAccess()
      {
         return View();
      }
      #endregion Logout
   }
}