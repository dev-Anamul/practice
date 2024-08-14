using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 22.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class ContraceptivesController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public ContraceptivesController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.Contraceptives
             .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
             .OrderBy(x => x.Description);

         var contraceptives = query.ToPagedList(pageNumber, pageSize);

         if (contraceptives.PageCount > 0)
         {
            if (pageNumber > contraceptives.PageCount)
               contraceptives = query.ToPagedList(contraceptives.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(contraceptives);
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
      public async Task<IActionResult> Create(Contraceptive contraceptive, string? module, string? parent)
      {
         try
         {
            var contraceptiveInDb = IsContraceptiveDuplicate(contraceptive);

            if (!contraceptiveInDb)
            {
               if (ModelState.IsValid)
               {
                  contraceptive.CreatedBy = session?.GetCurrentAdmin().Oid;
                  contraceptive.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  contraceptive.DateCreated = DateTime.Now;
                  contraceptive.IsDeleted = false;
                  contraceptive.IsSynced = false;

                  context.Contraceptives.Add(contraceptive);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(contraceptive);
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

         var contraceptive = await context.Contraceptives.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (contraceptive == null)
            return NotFound();

         return View(contraceptive);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, Contraceptive contraceptive, string? module, string? parent)
      {
         try
         {
            var contraceptiveInDb = await context.Contraceptives.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == contraceptive.Oid);

            bool isContraceptiveDuplicate = false;

            if (contraceptiveInDb.Description != contraceptive.Description)
               isContraceptiveDuplicate = IsContraceptiveDuplicate(contraceptive);

            if (!isContraceptiveDuplicate)
            {
               contraceptive.DateCreated = contraceptiveInDb.DateCreated;
               contraceptive.CreatedBy = contraceptiveInDb.CreatedBy;
               contraceptive.ModifiedBy = session?.GetCurrentAdmin().Oid;
               contraceptive.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               contraceptive.DateModified = DateTime.Now;
               contraceptive.IsDeleted = false;
               contraceptive.IsSynced = false;

               context.Contraceptives.Update(contraceptive);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isContraceptiveDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(contraceptive);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var contraceptive = await context.Contraceptives
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (contraceptive == null)
            return NotFound();

         return View(contraceptive);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var contraceptive = await context.Contraceptives.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.Contraceptives.Remove(contraceptive);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsContraceptiveDuplicate(Contraceptive contraceptive)
      {
         try
         {
            var contraceptiveInDb = context.Contraceptives.FirstOrDefault(c => c.Description.ToLower() == contraceptive.Description.ToLower() && c.IsDeleted == false);

            if (contraceptiveInDb != null)
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