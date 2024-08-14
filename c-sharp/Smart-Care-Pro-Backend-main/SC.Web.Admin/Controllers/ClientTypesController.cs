using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 07.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class ClientTypesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ClientTypesController(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         this.context = context;
         this.httpContextAccessor = httpContextAccessor;
      }

      #region Index
      public IActionResult Index(string search, int? page)
      {
         search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;

         if (page > 0 && search == null)
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.ClientTypes
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var clientTypes = query.ToPagedList(pageNumber, pageSize);

         if (clientTypes.PageCount > 0)
         {
            if (pageNumber > clientTypes.PageCount)
               clientTypes = query.ToPagedList(clientTypes.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(clientTypes);
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
      public async Task<IActionResult> Create(ClientType clientType)
      {
         try
         {
            var clientTypeInDb = IsClientTypeDuplicate(clientType);

            if (!clientTypeInDb)
            {
               if (ModelState.IsValid)
               {
                  clientType.CreatedBy = session?.GetCurrentAdmin().Oid;
                  clientType.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  clientType.DateCreated = DateTime.Now;
                  clientType.IsDeleted = false;
                  clientType.IsSynced = false;

                  context.ClientTypes.Add(clientType);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  //return RedirectToAction(nameof(Create));
                  return RedirectToAction(nameof(Create), new { module = "HTS" });
               }

               return View(clientType);
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

         var clientType = await context.ClientTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (clientType == null)
            return NotFound();

         return View(clientType);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ClientType clientType)
      {
         try
         {
            var clientTypeInDb = await context.ClientTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == clientType.Oid);

            bool isClientTypeDuplicate = false;

            if (clientTypeInDb.Description != clientType.Description)
               isClientTypeDuplicate = IsClientTypeDuplicate(clientType);

            if (!isClientTypeDuplicate)
            {
               clientType.DateCreated = clientTypeInDb.DateCreated;
               clientType.CreatedBy = clientTypeInDb.CreatedBy;
               clientType.ModifiedBy = session?.GetCurrentAdmin().Oid;
               clientType.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               clientType.DateModified = DateTime.Now;
               clientType.IsDeleted = false;
               clientType.IsSynced = false;

               context.ClientTypes.Update(clientType);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               //return RedirectToAction(nameof(Index));
               return RedirectToAction(nameof(Index), new { module = "HTS" });
            }
            else
            {
               if (isClientTypeDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(clientType);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var clientType = await context.ClientTypes
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (clientType == null)
            return NotFound();

         return View(clientType);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var clientType = await context.ClientTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.ClientTypes.Remove(clientType);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsClientTypeDuplicate(ClientType clientType)
      {
         try
         {
            var clientTypeInDb = context.ClientTypes.FirstOrDefault(c => c.Description.ToLower() == clientType.Description.ToLower() && c.IsDeleted == false);

            if (clientTypeInDb != null)
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