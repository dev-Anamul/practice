using CsvHelper;
using CsvHelper.Configuration;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using X.PagedList;

namespace SC.Web.Admin.Controllers
{
   public class LogDataController : Controller
   {
      private readonly DataContext context;

      public LogDataController(DataContext _context)
      {
         context = _context;
      }

      #region Index

      public IActionResult Index(DateTime? fromDate, DateTime? toDate, int? page)
      {
         // search = search?.ToLower();

         int pageSize = 10;
         int pageNumber = page ?? 1;

         ViewBag.SlNo = 1;

         if (page > 0 && (fromDate == null || toDate == null))
            ViewBag.SlNo = ((page - 1) * 10) + 1;

         var query = context.CareProLogs.Where(c => c.ErrorMessage != null).AsQueryable();

         // Apply date range filter
         if (fromDate.HasValue)
            query = query.Where(x => x.LogDate >= fromDate.Value.Date);

         if (toDate.HasValue)
            query = query.Where(x => x.LogDate <= toDate.Value.Date);

         var logData = query.ToPagedList(pageNumber, pageSize);

         if (logData.PageCount > 0)
         {
            if (pageNumber > logData.PageCount)
               logData = query.ToPagedList(logData.PageCount, pageSize);
         }

         ViewData["FromDate"] = fromDate;
         ViewData["ToDate"] = toDate;

         return View(logData);
      }
      #endregion

      #region CSV Export Action
      public IActionResult ExportToCsv(DateTime? fromDate, DateTime? toDate)
      {
         var query = context.CareProLogs.Where(c => c.ErrorMessage != null).AsQueryable();

         // Apply date range filter
         if (fromDate.HasValue)
            query = query.Where(x => x.LogDate >= fromDate.Value.Date);

         if (toDate.HasValue)
            query = query.Where(x => x.LogDate <= toDate.Value.Date);

         var records = query.ToList();

         var csvBuilder = new StringBuilder();
         using (var csv = new CsvWriter(new StringWriter(csvBuilder), new CsvConfiguration(CultureInfo.InvariantCulture)))
         {
            csv.WriteRecords(records);
         }

         var csvContent = csvBuilder.ToString();

         var fileName = $"LogDataExport_{DateTime.Now.ToString("dd/MM/yyyyHHmmss")}.csv";

         return File(Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
      }

      #endregion
   }
}