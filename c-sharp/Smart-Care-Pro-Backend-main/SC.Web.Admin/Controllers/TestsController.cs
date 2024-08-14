using Domain.Dto;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SC.Web.Admin.SessionManager;
using Utilities.Constants;
using X.PagedList;

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
    [RequestAuthenticationFilter]
    public class TestsController : Controller
    {
        private readonly DataContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private ISession? session => httpContextAccessor.HttpContext?.Session;

        public TestsController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }

        #region Index
        public async Task<IActionResult> Index(string? search, int? page, int? oid)
        {
            if (oid == 0)
                oid = Convert.ToInt32(TempData["manualTestSubTypeId"]);

            ViewBag.Oid = oid;

            var getTestSubType = await context.TestSubTypes.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

            TempData["manualTestSubTypeId"] = getTestSubType.Oid;

            ViewBag.TestSubTypeId = oid;
            ViewBag.TestSubType = getTestSubType.Description;
            ViewBag.TestTypeId = getTestSubType.TestTypeId;

            search = search?.ToLower();

            int pageSize = 10;
            int pageNumber = page ?? 1;

            ViewBag.SlNo = 1;
            if (page > 0 && search == null)
                ViewBag.SlNo = ((page - 1) * 10) + 1;

            var query = context.Tests
               .Include(t => t.TestSubtype)
               .ThenInclude(t => t.TestType)
               .Where(x => (x.Title.ToLower().Contains(search) || x.TestSubtype.Description.ToLower().Contains(search) || x.TestSubtype.TestType.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false) && x.SubtypeId == oid)
               .OrderBy(x => x.TestSubtype.TestType.Description);

            var tests = query.ToPagedList(pageNumber, pageSize);

            if (tests.PageCount > 0)
            {
                if (pageNumber > tests.PageCount)
                    tests = query.ToPagedList(tests.PageCount, pageSize);
            }

            ViewData["Search"] = search;

            return View(tests);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create(int oid)
        {
            try
            {
                var getTestSubType = await context.TestSubTypes.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == oid);

                var viewModel = new TestCreateDto
                {
                    SubtypeId = oid,
                };

                ViewBag.TestSubTypeId = oid;
                ViewBag.TestType = getTestSubType.Description;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestCreateDto viewModel)
        {
            try
            {
                var isExist = IsTestDuplicate(viewModel);

                if (!isExist)
                {
                    var test = new Test
                    {
                        CreatedBy = session?.GetCurrentAdmin().Oid,
                        CreatedIn = session?.GetCurrentAdmin().CreatedIn,
                        DateCreated = DateTime.Now,
                        IsDeleted = false,
                        IsSynced = false,
                        SubtypeId = viewModel.SubtypeId,
                        Title = viewModel.Title,
                        LONIC = viewModel.LONIC,
                        Description = viewModel.Description,
                        ResultType = viewModel.ResultType,
                        Oid = viewModel.TestId,
                    };

                    context.Tests.Add(test);
                    await context.SaveChangesAsync();

                    if (viewModel.MeasuringUnitDescription != null)
                    {
                        var measuringUnit = new MeasuringUnit
                        {
                            TestId = test.Oid,
                            CreatedBy = session?.GetCurrentAdmin().Oid,
                            CreatedIn = session?.GetCurrentAdmin().CreatedIn,
                            DateCreated = DateTime.Now,
                            IsDeleted = false,
                            IsSynced = false,
                            Description = viewModel.MeasuringUnitDescription,
                            MinimumRange = viewModel.MinimumRange,
                            MaximumRange = viewModel.MaximumRange,
                        };

                        context.MeasuringUnits.Add(measuringUnit);
                        await context.SaveChangesAsync();
                    }

                    if (viewModel.ResultOptionDescriptions != null && viewModel.ResultOptionDescriptions.Any())
                    {
                        string[] resultOptionDescriptions = viewModel.ResultOptionDescriptions.Split(',').ToArray();
                        foreach (var resultOptionDescription in resultOptionDescriptions)
                        {
                            var resultOption = new ResultOption
                            {
                                TestId = test.Oid,
                                CreatedBy = session?.GetCurrentAdmin().Oid,
                                CreatedIn = session?.GetCurrentAdmin().CreatedIn,
                                DateCreated = DateTime.Now,
                                IsDeleted = false,
                                IsSynced = false,
                                Description = resultOptionDescription,
                            };

                            context.ResultOptions.Add(resultOption);
                        }

                        await context.SaveChangesAsync();
                    }

                    TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                    return RedirectToAction("Create", new { oid = viewModel.SubtypeId, module = "Investigation" });
                }

                var getTestSubType = await context.TestSubTypes.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == viewModel.SubtypeId);

                var viewModels = new TestCreateDto
                {
                    SubtypeId = getTestSubType.Oid,
                };

                ViewBag.TestSubTypeId = getTestSubType.Oid;
                ViewBag.TestType = getTestSubType.Description;

                ModelState.AddModelError("Title", MessageConstants.DuplicateFound);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(viewModel);
            }
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var test = await context.Tests.Include(t => t.TestSubtype).ThenInclude(s => s.TestType).AsNoTracking().FirstOrDefaultAsync(i => i.Oid == id);
            var testSubType = context.TestSubTypes.ToList();

            if (test == null)
                return NotFound();

            ViewBag.TestTypes = new SelectList(context.TestTypes, FieldConstants.Oid, FieldConstants.Description, test.TestSubtype.TestType.Oid);
            ViewBag.TestSubtypes = new SelectList(testSubType.Where(d => d.TestTypeId == test.TestSubtype.TestTypeId).ToList(), FieldConstants.Oid, FieldConstants.Description, test.TestSubtype);

            ViewBag.TestType = new SelectList(context.TestTypes, FieldConstants.Oid, FieldConstants.Description);

            ViewBag.SubTypeId = test.SubtypeId;

            return View(test);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Test test)
        {
            try
            {
                var testModel = await context.Tests.AsNoTracking().FirstOrDefaultAsync(i => i.Oid == test.Oid);

                bool isTestDuplicate = false;

                if (testModel.Title != test.Title)
                    isTestDuplicate = IsDuplicate(test);

                if (!isTestDuplicate)
                {
                    test.ModifiedBy = session?.GetCurrentAdmin().Oid;
                    test.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
                    test.DateModified = DateTime.Now;
                    test.IsDeleted = false;
                    test.IsSynced = false;

                    context.Tests.Update(test);
                    await context.SaveChangesAsync();

                    TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

                    ViewBag.SubTypeId = testModel.SubtypeId;

                    return RedirectToAction("Index", new { oid = test.SubtypeId });
                }
                else
                {
                    if (isTestDuplicate)
                        ModelState.AddModelError("Title", MessageConstants.DuplicateFound);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(test);
        }

        //[HttpGet]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //        return NotFound();

        //    try
        //    {
        //        var test = await context.Tests
        //            .Include(t => t.TestSubtype)
        //            .ThenInclude(s => s.TestType)
        //            .Include(t => t.MeasuringUnits)
        //            .Include(t => t.ResultOptions)
        //            .AsNoTracking()
        //            .FirstOrDefaultAsync(i => i.Oid == id);

        //        var testSubType = context.TestSubTypes.ToList();

        //        if (test == null)
        //            return NotFound();

        //        var testCreateDto = new TestCreateDto
        //        {
        //            SubtypeId = test.SubtypeId,
        //            Title = test.Title,
        //            LONIC = test.LONIC,
        //            Description = test.Description,
        //            ResultType = test.ResultType,
        //            MeasuringUnitDescription = test.MeasuringUnits != null ? test.MeasuringUnits.FirstOrDefault().Description : null,
        //            MinimumRange = test.MeasuringUnits != null ? test.MeasuringUnits.FirstOrDefault().MaximumRange : 0,
        //            MaximumRange = test.MeasuringUnits != null ? test.MeasuringUnits.FirstOrDefault().MinimumRange : 0,
        //            ResultOptionDescriptions = test.ResultOptions != null ? string.Join(",", test.ResultOptions.Select(ro => ro.Description)) : null
        //        };

        //        ViewBag.TestTypes = new SelectList(context.TestTypes, FieldConstants.Oid, FieldConstants.Description, test.TestSubtype.TestType.Oid);
        //        ViewBag.TestSubtypes = new SelectList(testSubType.Where(d => d.TestTypeId == test.TestSubtype.TestTypeId).ToList(), FieldConstants.Oid, FieldConstants.Description, test.TestSubtype);

        //        ViewBag.SubTypeId = test.SubtypeId;

        //        return View(testCreateDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //        return View();
        //    }
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, TestCreateDto viewModel)
        //{
        //    try
        //    {
        //        var testModel = await context.Tests
        //            .Include(t => t.MeasuringUnits)
        //            .Include(t => t.ResultOptions)
        //            .FirstOrDefaultAsync(i => i.Oid == id);

        //        if (testModel != null)
        //        {
        //            testModel.DateCreated = testModel.DateCreated;
        //            testModel.CreatedBy = testModel.CreatedBy;
        //            testModel.ModifiedBy = session?.GetCurrentAdmin().Oid;
        //            testModel.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
        //            testModel.DateModified = DateTime.Now;
        //            testModel.SubtypeId = viewModel.SubtypeId;
        //            testModel.Title = viewModel.Title;
        //            testModel.LONIC = viewModel.LONIC;
        //            testModel.Description = viewModel.Description;
        //            testModel.ResultType = viewModel.ResultType;

        //            //if (testModel.MeasuringUnits != null)
        //            //{
        //            //   testModel.MeasuringUnits.Description = viewModel.MeasuringUnitDescription;
        //            //   testModel.MeasuringUnits.MinimumRange = viewModel.MinimumRange;
        //            //   testModel.MeasuringUnits.MaximumRange = viewModel.MaximumRange;
        //            //   testModel.MeasuringUnits.ModifiedBy = session?.GetCurrentAdmin().Oid;
        //            //   testModel.MeasuringUnits.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
        //            //   testModel.MeasuringUnits.DateModified = DateTime.Now;
        //            //}
        //            if (viewModel.MeasuringUnitDescription != null)
        //            {
        //                var measuringUnit = new MeasuringUnit
        //                {
        //                    TestId = testModel.Oid,
        //                    CreatedBy = session?.GetCurrentAdmin().Oid,
        //                    CreatedIn = session?.GetCurrentAdmin().CreatedIn,
        //                    DateCreated = DateTime.Now,
        //                    IsDeleted = false,
        //                    IsSynced = false,
        //                    Description = viewModel.MeasuringUnitDescription,
        //                    MinimumRange = viewModel.MinimumRange,
        //                    MaximumRange = viewModel.MaximumRange,
        //                };
        //                context.MeasuringUnits.Update(measuringUnit);
        //            }

        //            if (viewModel.ResultOptionDescriptions != null && viewModel.ResultOptionDescriptions.Any())
        //            {
        //                var existingResultOptions = testModel.ResultOptions.ToDictionary(ro => ro.Description);

        //                string[] resultOptionDescriptions = viewModel.ResultOptionDescriptions.Split(',').ToArray();
        //                foreach (var resultOptionDescription in resultOptionDescriptions)
        //                {
        //                    if (existingResultOptions.TryGetValue(resultOptionDescription, out var resultOption))
        //                    {
        //                        resultOption.ModifiedBy = session?.GetCurrentAdmin().Oid;
        //                        resultOption.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
        //                        resultOption.DateModified = DateTime.Now;
        //                        resultOption.Description = resultOptionDescription;
        //                    }
        //                    else
        //                    {
        //                        var newResultOption = new ResultOption
        //                        {
        //                            TestId = testModel.Oid,
        //                            CreatedBy = session?.GetCurrentAdmin().Oid,
        //                            CreatedIn = session?.GetCurrentAdmin().CreatedIn,
        //                            DateCreated = DateTime.Now,
        //                            IsDeleted = false,
        //                            IsSynced = false,
        //                            Description = resultOptionDescription,
        //                        };
        //                        context.ResultOptions.Update(newResultOption);
        //                    }
        //                }
        //            }

        //            await context.SaveChangesAsync();

        //            TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

        //            ViewBag.SubTypeId = testModel.SubtypeId;

        //            return RedirectToAction("Index", new { oid = viewModel.SubtypeId, module = "Investigation" });
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //        return View(viewModel);
        //    }
        //}
        #endregion

        #region Single Test
        #region Index
        public async Task<IActionResult> IndexTest(string? search, int? page)
        {
            search = search?.ToLower();

            int pageSize = 10;
            int pageNumber = page ?? 1;

            ViewBag.SlNo = 1;
            if (page > 0 && search == null)
                ViewBag.SlNo = ((page - 1) * 10) + 1;

            var query = context.Tests
               .Include(t => t.TestSubtype)
               .ThenInclude(t => t.TestType)
               .Where(x => (x.Title.ToLower().Contains(search) || x.TestSubtype.Description.ToLower().Contains(search) || x.TestSubtype.TestType.Description.ToLower().Contains(search) || search == null && x.IsDeleted == false))
               .OrderBy(x => x.TestSubtype.TestType.Description);

            var tests = query.ToPagedList(pageNumber, pageSize);

            if (tests.PageCount > 0)
            {
                if (pageNumber > tests.PageCount)
                    tests = query.ToPagedList(tests.PageCount, pageSize);
            }

            ViewData["Search"] = search;

            return View(tests);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> CreateTest()
        {
            try
            {
                ViewBag.TestType = new SelectList(context.TestTypes, FieldConstants.Oid, FieldConstants.Description);
                ViewBag.TestSubtypes = new SelectList(context.TestSubTypes, FieldConstants.Oid, FieldConstants.Description);

                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTest(TestCreateDto viewModel)
        {
            try
            {
                var isExist = IsTestDuplicate(viewModel);

                if (!isExist)
                {
                    var test = new Test
                    {
                        CreatedBy = session?.GetCurrentAdmin().Oid,
                        CreatedIn = session?.GetCurrentAdmin().CreatedIn,
                        DateCreated = DateTime.Now,
                        IsDeleted = false,
                        IsSynced = false,
                        SubtypeId = viewModel.SubtypeId,
                        Title = viewModel.Title,
                        LONIC = viewModel.LONIC,
                        Description = viewModel.Description,
                        ResultType = viewModel.ResultType,
                        Oid = viewModel.TestId,
                    };

                    context.Tests.Add(test);
                    await context.SaveChangesAsync();

                    if (viewModel.MeasuringUnitDescription != null)
                    {
                        var measuringUnit = new MeasuringUnit
                        {
                            TestId = test.Oid,
                            CreatedBy = session?.GetCurrentAdmin().Oid,
                            CreatedIn = session?.GetCurrentAdmin().CreatedIn,
                            DateCreated = DateTime.Now,
                            IsDeleted = false,
                            IsSynced = false,
                            Description = viewModel.MeasuringUnitDescription,
                            MinimumRange = viewModel.MinimumRange,
                            MaximumRange = viewModel.MaximumRange,
                        };

                        context.MeasuringUnits.Add(measuringUnit);
                        await context.SaveChangesAsync();
                    }

                    if (viewModel.ResultOptionDescriptions != null && viewModel.ResultOptionDescriptions.Any())
                    {
                        string[] resultOptionDescriptions = viewModel.ResultOptionDescriptions.Split(',').ToArray();
                        foreach (var resultOptionDescription in resultOptionDescriptions)
                        {
                            var resultOption = new ResultOption
                            {
                                TestId = test.Oid,
                                CreatedBy = session?.GetCurrentAdmin().Oid,
                                CreatedIn = session?.GetCurrentAdmin().CreatedIn,
                                DateCreated = DateTime.Now,
                                IsDeleted = false,
                                IsSynced = false,
                                Description = resultOptionDescription,
                            };

                            context.ResultOptions.Add(resultOption);
                        }

                        await context.SaveChangesAsync();
                    }

                    TempData[SessionConstants.Message] = MessageConstants.RecordSavedSuccessfully;

                    //return RedirectToAction("CreateTest");
                    return RedirectToAction(nameof(CreateTest), new { module = "Investigation" });
                }

                var getTestSubType = await context.TestSubTypes.AsNoTracking().FirstOrDefaultAsync(o => o.Oid == viewModel.SubtypeId);

                var viewModels = new TestCreateDto
                {
                    SubtypeId = getTestSubType.Oid,
                };

                ViewBag.TestSubTypeId = getTestSubType.Oid;
                ViewBag.TestType = getTestSubType.Description;

                ModelState.AddModelError("Title", MessageConstants.DuplicateFound);

                ViewBag.TestType = new SelectList(context.TestTypes, FieldConstants.Oid, FieldConstants.Description);
                ViewBag.TestSubtypes = new SelectList(context.TestSubTypes, FieldConstants.Oid, FieldConstants.Description);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(viewModel);
            }
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> EditTest(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var test = await context.Tests
                    .Include(t => t.TestSubtype)
                    .ThenInclude(s => s.TestType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.Oid == id);

                var testSubType = context.TestSubTypes.ToList();

                if (test == null)
                    return NotFound();

                var testCreateDto = new TestCreateDto
                {
                    SubtypeId = test.SubtypeId,
                    Title = test.Title,
                    LONIC = test.LONIC,
                    Description = test.Description,
                    ResultType = test.ResultType,
                };

                ViewBag.TestTypes = new SelectList(context.TestTypes, FieldConstants.Oid, FieldConstants.Description, test.TestSubtype.TestType.Oid);
                ViewBag.TestSubtypes = new SelectList(testSubType.Where(d => d.TestTypeId == test.TestSubtype.TestTypeId).ToList(), FieldConstants.Oid, FieldConstants.Description, test.TestSubtype);

                ViewBag.SubTypeId = test.SubtypeId;

                return View(testCreateDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTest(int id, TestCreateDto viewModel)
        {
            try
            {
                var testModel = await context.Tests
                    .Include(t => t.MeasuringUnits)
                    .Include(t => t.ResultOptions)
                    .FirstOrDefaultAsync(i => i.Oid == id);

                if (testModel != null)
                {
                    testModel.DateCreated = testModel.DateCreated;
                    testModel.CreatedBy = testModel.CreatedBy;
                    testModel.ModifiedBy = session?.GetCurrentAdmin().Oid;
                    testModel.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
                    testModel.DateModified = DateTime.Now;
                    testModel.SubtypeId = viewModel.SubtypeId;
                    testModel.Title = viewModel.Title;
                    testModel.LONIC = viewModel.LONIC;
                    testModel.Description = viewModel.Description;
                    testModel.ResultType = viewModel.ResultType;

                    if (testModel.MeasuringUnits != null)
                    {
                        //testModel.MeasuringUnits.Description = viewModel.MeasuringUnitDescription;
                        //testModel.MeasuringUnits.MinimumRange = viewModel.MinimumRange;
                        //testModel.MeasuringUnits.MaximumRange = viewModel.MaximumRange;
                        //testModel.MeasuringUnits.ModifiedBy = session?.GetCurrentAdmin().Oid;
                        //testModel.MeasuringUnits.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
                        //testModel.MeasuringUnits.DateModified = DateTime.Now;
                    }
                    else if (viewModel.MeasuringUnitDescription != null)
                    {
                        var measuringUnit = new MeasuringUnit
                        {
                            TestId = testModel.Oid,
                            CreatedBy = session?.GetCurrentAdmin().Oid,
                            CreatedIn = session?.GetCurrentAdmin().CreatedIn,
                            DateCreated = DateTime.Now,
                            IsDeleted = false,
                            IsSynced = false,
                            Description = viewModel.MeasuringUnitDescription,
                            MinimumRange = viewModel.MinimumRange,
                            MaximumRange = viewModel.MaximumRange,
                        };
                        context.MeasuringUnits.Add(measuringUnit);
                    }

                    if (viewModel.ResultOptionDescriptions != null && viewModel.ResultOptionDescriptions.Any())
                    {
                        var existingResultOptions = testModel.ResultOptions.ToDictionary(ro => ro.Description);

                        string[] resultOptionDescriptions = viewModel.ResultOptionDescriptions.Split(',').ToArray();
                        foreach (var resultOptionDescription in resultOptionDescriptions)
                        {
                            if (existingResultOptions.TryGetValue(resultOptionDescription, out var resultOption))
                            {
                                resultOption.ModifiedBy = session?.GetCurrentAdmin().Oid;
                                resultOption.ModifiedIn = session?.GetCurrentAdmin().ModifiedIn;
                                resultOption.DateModified = DateTime.Now;
                                resultOption.Description = resultOptionDescription;
                            }
                            else
                            {
                                var newResultOption = new ResultOption
                                {
                                    TestId = testModel.Oid,
                                    CreatedBy = session?.GetCurrentAdmin().Oid,
                                    CreatedIn = session?.GetCurrentAdmin().CreatedIn,
                                    DateCreated = DateTime.Now,
                                    IsDeleted = false,
                                    IsSynced = false,
                                    Description = resultOptionDescription,
                                };
                                context.ResultOptions.Add(newResultOption);
                            }
                        }
                    }

                    await context.SaveChangesAsync();

                    TempData[SessionConstants.Message] = MessageConstants.RecordUpdatedSuccessfully;

                    //return RedirectToAction("IndexTest");
                    return RedirectToAction(nameof(IndexTest), new { module = "Investigation" });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(viewModel);
            }
        }

        #endregion

        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var compositeTests = await context.CompositeTests
                .FirstOrDefaultAsync(c => c.Oid == id);

            if (compositeTests == null)
                return NotFound();

            return View(compositeTests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var test = await context.Tests.FindAsync(id);

            context.Tests.Remove(test);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Read
        public bool IsTestDuplicate(TestCreateDto test)
        {
            try
            {
                var testInDb = context.Tests.AsNoTracking().FirstOrDefault(p => p.Title.ToLower() == test.Title.ToLower() && p.SubtypeId == test.SubtypeId && p.Oid != test.TestId && p.IsDeleted == false);

                if (testInDb != null)
                    return true;

                return false;
            }
            catch
            {
                throw;
            }
        }

        public bool IsDuplicate(Test test)
        {
            try
            {
                var testInDb = context.Tests.AsNoTracking().FirstOrDefault(p => p.Title.ToLower() == test.Title.ToLower() && p.SubtypeId == test.SubtypeId && p.Oid != test.Oid && p.IsDeleted == false);

                if (testInDb != null)
                    return true;

                return false;
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTestType()
        {
            return Json(await context.TestTypes.ToListAsync());
        }

        public JsonResult LoadTestSubType(int id)
        {
            var town = context.TestSubTypes.Where(p => p.TestTypeId == id && p.IsDeleted == false).ToList();
            return Json(town);
        }
        #endregion
    }
}