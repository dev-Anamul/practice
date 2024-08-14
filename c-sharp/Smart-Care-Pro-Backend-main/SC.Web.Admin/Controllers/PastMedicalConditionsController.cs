using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 03.05.2023
 * Modified by  : Bella
 * Last modified: 23.05.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class PastMedicalConditionsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public PastMedicalConditionsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.PastMedicalConditions
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var pastMedicalConditions = query.ToPagedList(pageNumber, pageSize);

         if (pastMedicalConditions.PageCount > 0)
         {
            if (pageNumber > pastMedicalConditions.PageCount)
               pastMedicalConditions = query.ToPagedList(pastMedicalConditions.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(pastMedicalConditions);
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
      public async Task<IActionResult> Create(PastMedicalCondition pastMedicalCondition)
      {
         try
         {
            var pastMedicalConditionInDb = IsPastMedicalConditonDuplicate(pastMedicalCondition);

            if (!pastMedicalConditionInDb)
            {
               if (ModelState.IsValid)
               {
                  pastMedicalCondition.CreatedBy = session?.GetCurrentAdmin().Oid;
                  pastMedicalCondition.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  pastMedicalCondition.DateCreated = DateTime.Now;
                  pastMedicalCondition.IsDeleted = false;
                  pastMedicalCondition.IsSynced = false;

                  context.PastMedicalConditions.Add(pastMedicalCondition);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create));
               }

               return View(pastMedicalCondition);
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

         var pastMedicalConditionInDb = await context.PastMedicalConditions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (pastMedicalConditionInDb == null)
            return NotFound();

         return View(pastMedicalConditionInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, PastMedicalCondition pastMedicalCondition)
      {
         try
         {
            var pastMedicalConditionInDb = await context.PastMedicalConditions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == pastMedicalCondition.Oid);

            bool isPastMedicalConditionDuplicate = false;

            if (pastMedicalConditionInDb.Description != pastMedicalCondition.Description)
               isPastMedicalConditionDuplicate = IsPastMedicalConditonDuplicate(pastMedicalCondition);

            if (!isPastMedicalConditionDuplicate)
            {
               pastMedicalCondition.DateCreated = pastMedicalConditionInDb.DateCreated;
               pastMedicalCondition.CreatedBy = pastMedicalConditionInDb.CreatedBy;
               pastMedicalCondition.ModifiedBy = session?.GetCurrentAdmin().Oid;
               pastMedicalCondition.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               pastMedicalCondition.DateModified = DateTime.Now;
               pastMedicalCondition.IsDeleted = false;
               pastMedicalCondition.IsSynced = false;

               context.PastMedicalConditions.Update(pastMedicalCondition);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index));
            }
            else
            {
               if (isPastMedicalConditionDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(pastMedicalCondition);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var pastMedicalConditon = await context.PastMedicalConditions
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (pastMedicalConditon == null)
            return NotFound();

         return View(pastMedicalConditon);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var pastMedicalConditon = await context.PastMedicalConditions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.PastMedicalConditions.Remove(pastMedicalConditon);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsPastMedicalConditonDuplicate(PastMedicalCondition pastMedicalCondition)
      {
         try
         {
            var pastMedicalConditionInDb = context.PastMedicalConditions.FirstOrDefault(c => c.Description.ToLower() == pastMedicalCondition.Description.ToLower() && c.IsDeleted == false);

            if (pastMedicalConditionInDb != null)
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