using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

/*
 * Created by   : Bella
 * Date created : 27.03.2023
 * Modified by  : Bella
 * Last modified: 20.08.2023
 * Reviewed by  : 
 * Date reviewed:
 */
namespace SC.Web.Admin.Controllers
{
   [RequestAuthenticationFilter]
   public class QuestionsController : Controller
   {
      private readonly DataContext context;
      private readonly IHttpContextAccessor httpContextAccessor;
      private ISession? session => httpContextAccessor.HttpContext?.Session;

      public QuestionsController(DataContext context, IHttpContextAccessor httpContextAccessor)
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

         var query = context.Questions
            .Where(x => x.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false)
            .OrderBy(x => x.Description);

         var questions = query.ToPagedList(pageNumber, pageSize);

         if (questions.PageCount > 0)
         {
            if (pageNumber > questions.PageCount)
               questions = query.ToPagedList(questions.PageCount, pageSize);
         }

         ViewData["Search"] = search;

         return View(questions);

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
      public async Task<IActionResult> Create(Question question, string? module, string? parent)
      {
         try
         {
            var questionInDb = IsQuestionDuplicate(question);

            if (!questionInDb)
            {
               if (ModelState.IsValid)
               {
                  question.CreatedBy = session?.GetCurrentAdmin().Oid;
                  question.CreatedIn = session?.GetCurrentAdmin().CreatedIn;
                  question.DateCreated = DateTime.Now;
                  question.IsDeleted = false;
                  question.IsSynced = false;

                  context.Questions.Add(question);
                  await context.SaveChangesAsync();

                  TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                  return RedirectToAction(nameof(Create), new { module = module, parent = parent });
               }

               return View(question);
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

         var questionInDb = await context.Questions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         if (questionInDb == null)
            return NotFound();

         return View(questionInDb);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, Question question, string? module, string? parent)
      {
         try
         {
            var questionInDb = await context.Questions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == question.Oid);

            bool isQuestionDuplicate = false;

            if (questionInDb.Description != question.Description)
               isQuestionDuplicate = IsQuestionDuplicate(question);

            if (!isQuestionDuplicate)
            {
               question.DateCreated = questionInDb.DateCreated;
               question.CreatedBy = questionInDb.CreatedBy;
               question.ModifiedBy = session?.GetCurrentAdmin().Oid;
               question.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
               question.DateModified = DateTime.Now;
               question.IsDeleted = false;
               question.IsSynced = false;

               context.Questions.Update(question);
               await context.SaveChangesAsync();

               TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

               return RedirectToAction(nameof(Index), new { module = module, parent = parent });
            }
            else
            {
               if (isQuestionDuplicate)
                  ModelState.AddModelError("Description", MessageConstants.DuplicateFound);
            }
         }
         catch (Exception)
         {
            throw;
         }

         return View(question);
      }
      #endregion

      #region Delete
      [HttpGet]
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
            return NotFound();

         var question = await context.Questions
             .FirstOrDefaultAsync(c => c.Oid == id);

         if (question == null)
            return NotFound();

         return View(question);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Delete(int id)
      {
         var question = await context.Questions.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);

         context.Questions.Remove(question);
         await context.SaveChangesAsync();

         return RedirectToAction(nameof(Index));
      }
      #endregion

      #region Read
      public bool IsQuestionDuplicate(Question question)
      {
         try
         {
            var questionInDb = context.Questions.FirstOrDefault(c => c.Description.ToLower() == question.Description.ToLower() && c.IsDeleted == false);

            if (questionInDb != null)
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