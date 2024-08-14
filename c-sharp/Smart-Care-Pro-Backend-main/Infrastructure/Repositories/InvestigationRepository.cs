using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 13.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class InvestigationRepository : Repository<Investigation>, IInvestigationRepository
    {
        public InvestigationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by key.
        /// </summary>
        /// <param name="key">Primary key of the table Investigations.</param>
        /// <returns>Returns a birth record if the key is matched.</returns>
        public async Task<Investigation> GetInvestigationByKey(Guid key)
        {
            try
            {
                return await LoadWithChildAsync<Investigation>(b => b.InteractionId == key && b.IsDeleted == false, t => t.Test, tts => tts.Test.TestSubtype, tt => tt.Test.TestSubtype.TestType, c => c.Client, u => u.UserAccount);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a birth record by key.
        /// </summary>
        /// <param name="key">Primary key of the table Investigations.</param>
        /// <returns>Returns a birth record if the key is matched.</returns>
        public async Task<IEnumerable<Investigation>> GetInvestigationByEncounterId(Guid key)
        {
            try
            {
                return await LoadListWithChildAsync<Investigation>(b => b.EncounterId == key && b.IsDeleted == false, t => t.Test, tts => tts.Test.TestSubtype, tt => tt.Test.TestSubtype.TestType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public async Task<IEnumerable<Investigation>> GetInvestigations()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a investigation by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a Investigation if the ClientID is matched.</returns>
        public async Task<IEnumerable<Investigation>> GetInvestigationByClient(Guid clientId)
        {
            try
            {
                return await LoadListWithChildAsync<Investigation>(b => b.IsDeleted == false && b.ClientId == clientId, t => t.Test, tts => tts.Test.TestSubtype, tt => tt.Test.TestSubtype.TestType, r => r.Results);

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        ///  The method is used to get a investigationDto by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a InvestigationDto if the ClientID is matched</returns>
        public async Task<IEnumerable<InvestigationDto>> GetInvestigationDtoByClient(Guid clientId)
        {
            try
            {
                var investigationsAsQuerable = context.Investigations.
                    Include(x => x.Test).
                    ThenInclude(y => y.TestSubtype).
                    ThenInclude(z => z.TestType).
                    Include(r => r.Results).AsNoTracking().
                    Where(p => p.IsDeleted == false && p.ClientId == clientId)
                    .Join(context.Encounters.AsNoTracking(),investigation => investigation.EncounterId,encounter => encounter.Oid,
                    (investigation, encounter) => new Investigation
                    {
                        EncounterId = investigation.EncounterId,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        DateCreated = investigation.DateCreated,
                        AdditionalComment = investigation.AdditionalComment,
                        Client = investigation.Client,
                        ClientId = investigation.ClientId,
                        ClinicianId = investigation.ClinicianId,
                        CreatedBy = investigation.CreatedBy,
                        CreatedIn = investigation.CreatedIn,
                        DateModified = investigation.DateModified,
                        EncounterType = investigation.EncounterType,
                        ImagingTestDetails = investigation.ImagingTestDetails,
                        InteractionId = investigation.InteractionId,
                        IsDeleted = investigation.IsDeleted,
                        IsResultReceived = investigation.IsResultReceived,
                        IsSynced = investigation.IsSynced,
                        ModifiedBy = investigation.ModifiedBy,
                        ModifiedIn = investigation.ModifiedIn,
                        OrderDate = investigation.OrderDate,
                        OrderNumber = investigation.OrderNumber,
                        Piority = investigation.Piority,
                        Quantity = investigation.Quantity,
                        Results = investigation.Results.ToList(),
                        SampleCollectionDate = investigation.SampleCollectionDate,
                        SampleQuantity = investigation.SampleQuantity,
                        Test = investigation.Test,
                        TestId = investigation.TestId,
                        TestTypeId = investigation.TestTypeId,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == investigation.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == investigation.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    }).AsQueryable();


                // var Investigations = await LoadListWithChildAsync<Investigation>(b => b.IsDeleted == false && b.ClientId == clientId, t => t.Test, tts => tts.Test.TestSubtype, tt => tt.Test.TestSubtype.TestType, r => r.Results);
                var Investigations = investigationsAsQuerable.OrderByDescending(x => x.EncounterDate).ToList();

                foreach (var item in Investigations.Where(x => x.IsResultReceived == true).ToList())
                {
                    var results = item.Results.Where(x => x.ResultOptionId != null).ToList();

                    foreach (var item2 in results)
                    {
                        var ResultoPtion = await context.ResultOptions.Where(x => x.Oid == item2.ResultOptionId).FirstOrDefaultAsync();

                        item2.ResultOption = ResultoPtion;
                    }
                    var measuringResults = item.Results.Where(x => x.MeasuringUnitId != null).ToList();

                    foreach (var Measurements in measuringResults)
                    {
                        var MeasurementUnit = await context.MeasuringUnits.Where(x => x.Oid == Measurements.MeasuringUnitId.Value).FirstOrDefaultAsync();

                        Measurements.MeasuringUnit = MeasurementUnit;
                    }

                }
                List<InvestigationDto> investigationDto = Investigations.GroupBy(x => x.EncounterId).Select(x => new InvestigationDto()
                {
                    EncounterID = x.Key,
                    investigation = x.ToList(),
                    ClientID = clientId,
                    DateCreated = x.Select(x => x.DateCreated).FirstOrDefault(),
                    EncounterDate = x.Select(x => x.EncounterDate).FirstOrDefault(),
                    CreatedIn = x.Select(x => x.CreatedIn).FirstOrDefault(),
                    CreatedBy = x.Select(x => x.CreatedBy).FirstOrDefault(),
                    ClinicianName = x.Select(x => x.ClinicianName).FirstOrDefault(),
                    FacilityName = x.Select(x => x.FacilityName).FirstOrDefault(),
                    investigationWithOutComposite = x.Select(x => new InvestigationWithOutComposite()
                    {
                        AdditionalComment = x.AdditionalComment,
                        InteractionID = x.InteractionId,
                        ClientID = x.ClientId,
                        Client = x.Client,
                        Quantity = x.Quantity,
                        ClinicianID = x.ClinicianId,
                        EncounterID = x.EncounterId,
                        ImagingTestDetails = x.ImagingTestDetails,
                        IsResultReceived = x.IsResultReceived,
                        OrderDate = x.OrderDate,
                        Piority = x.Piority,
                        OrderNumber = x.OrderNumber,
                        Results = x.Results,
                        SampleQuantity = x.SampleQuantity,
                        Test = x.Test,

                        TestID = x.TestId,
                        TestTypeId = x.TestTypeId,
                        UserAccount = x.UserAccount,
                        CreatedBy = x.CreatedBy,
                        DateCreated = x.DateCreated,
                        CreatedIn = x.CreatedIn,
                        DateModified = x.DateModified,
                        IsDeleted = (bool)x.IsDeleted,
                        IsSynced = (bool)x.IsSynced,
                        TestResult =
                        x.IsResultReceived == true ?
                        (x.Results.Count() > 0 ?
                        (string.IsNullOrEmpty(x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().ResultDescriptive) ?

                        (x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().ResultNumeric == null ?
                        (x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().ResultOptionId == null ? ""
                        : x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().ResultOption.Description)
                        : x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().ResultNumeric.Value.ToString()) : x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().ResultDescriptive.ToString())
                        : "") : "",
                        MaxumumRange = ((x.IsResultReceived == true && x.Results.Count() > 0 && x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().MeasuringUnitId != null)

                        ? x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().MeasuringUnit.MaximumRange : null),
                        UnitTest = ((x.IsResultReceived == true && x.Results.Count() > 0 && x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().MeasuringUnitId != null) ? x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().MeasuringUnit.Description : null),
                        MinumumRange = ((x.IsResultReceived == true && x.Results.Count() > 0 && x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().MeasuringUnitId != null) ? x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().MeasuringUnit.MinimumRange : null)

                    }).ToList(),

                }).ToList();

                var testItems = await context.TestItems.Include(tts => tts.CompositeTest).Where(b => b.IsDeleted == false).ToListAsync();
                var testItemsByGroup = testItems.GroupBy(x => x.CompositeTestId).ToList();

                foreach (var item in investigationDto)
                {

                    foreach (var test in testItemsByGroup)
                    {
                        var ListCompositeTest = test.Select(x => x.TestId).ToList();

                        var InvestigationTest = item.investigationWithOutComposite.Select(x => x.TestID).ToList();
                        var C = ListCompositeTest.All(item => InvestigationTest.Contains(item));

                        if (C)
                        {

                            var investigationWithComposite = item.investigationWithOutComposite.Where(x => ListCompositeTest.Contains(x.TestID)).Select(x => new InvestigationWithComposite()
                            {
                                CompositeName = test.Select(x => x.CompositeTest.Description).FirstOrDefault(),
                                AdditionalComment = x.AdditionalComment,
                                InteractionID = x.InteractionID,
                                ClientID = x.ClientID,
                                Client = x.Client,
                                Quantity = x.Quantity,
                                ClinicianID = x.ClinicianID,
                                EncounterID = x.EncounterID,
                                ImagingTestDetails = x.ImagingTestDetails,
                                IsResultReceived = x.IsResultReceived,
                                OrderDate = x.OrderDate,
                                Piority = x.Piority,
                                OrderNumber = x.OrderNumber,
                                Results = x.Results,
                                SampleQuantity = x.SampleQuantity,
                                Test = x.Test,
                                TestID = x.TestID,
                                TestTypeId = x.TestTypeId,
                                UserAccount = x.UserAccount,
                                CreatedBy = x.CreatedBy,
                                DateCreated = x.DateCreated,
                                EncounterDate = x.EncounterDate,
                                CreatedIn = x.CreatedIn,
                                DateModified = x.DateModified,
                                IsDeleted = x.IsDeleted,
                                IsSynced = x.IsSynced,
                                ClinicianName=x.ClinicianName,
                                FacilityName=x.FacilityName,
                                TestResult =
                         x.IsResultReceived == true ?
                         (x.Results.Count() > 0 ?
                         (string.IsNullOrEmpty(x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().ResultDescriptive) ?

                         (x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().ResultNumeric == null ?
                         (x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().ResultOptionId == null ? "" : x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().ResultOption.Description)
                         : x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().ResultNumeric.Value.ToString()) :
                         x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().ResultDescriptive.ToString())
                         : "") : "",
                                MaxumumRange = ((x.IsResultReceived == true && x.Results.Count() > 0 && x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().MeasuringUnitId != null)

                         ? x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().MeasuringUnit.MaximumRange : null),

                                UnitTest = ((x.IsResultReceived == true && x.Results.Count() > 0 && x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().MeasuringUnitId != null) ? x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().MeasuringUnit.Description : null),

                                MinumumRange = ((x.IsResultReceived == true && x.Results.Count() > 0 && x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().MeasuringUnitId != null)

                         ? x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().MeasuringUnit.MinimumRange : null)

                            }).ToList();
                            if (item.investigationWithComposite == null)
                            {
                                item.investigationWithComposite = investigationWithComposite;
                            }
                            else
                            {
                                item.investigationWithComposite.AddRange(investigationWithComposite);
                            }
                            item.investigationWithOutComposite = item.investigationWithOutComposite.Where(x => !ListCompositeTest.Contains(x.TestID)).ToList();
                        }
                    }
                }
                return investigationDto;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<IEnumerable<InvestigationDto>> GetInvestigationDtoByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var investigationsAsQuerable = context.Investigations.Include(x => x.Test).ThenInclude(y => y.TestSubtype).ThenInclude(z => z.TestType).Include(r => r.Results).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
  .Join(
      context.Encounters.AsNoTracking(),
      investigation => investigation.EncounterId,
      encounter => encounter.Oid,
      (investigation, encounter) => new Investigation
      {
          EncounterId = investigation.EncounterId,
          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
          DateCreated = investigation.DateCreated,
          AdditionalComment = investigation.AdditionalComment,
          Client = investigation.Client,
          ClientId = investigation.ClientId,
          ClinicianId = investigation.ClinicianId,
          CreatedBy = investigation.CreatedBy,
          CreatedIn = investigation.CreatedIn,
          DateModified = investigation.DateModified,
          EncounterType = investigation.EncounterType,
          ImagingTestDetails = investigation.ImagingTestDetails,
          InteractionId = investigation.InteractionId,
          IsDeleted = investigation.IsDeleted,
          IsResultReceived = investigation.IsResultReceived,
          IsSynced = investigation.IsSynced,
          ModifiedBy = investigation.ModifiedBy,
          ModifiedIn = investigation.ModifiedIn,
          OrderDate = investigation.OrderDate,
          OrderNumber = investigation.OrderNumber,
          Piority = investigation.Piority,
          Quantity = investigation.Quantity,
          Results = investigation.Results.ToList(),
          SampleCollectionDate = investigation.SampleCollectionDate,
          SampleQuantity = investigation.SampleQuantity,
          Test = investigation.Test,
          TestId = investigation.TestId,
          TestTypeId = investigation.TestTypeId,
          ClinicianName = context.UserAccounts.Where(x => x.Oid == investigation.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
          FacilityName = context.Facilities.Where(x => x.Oid == investigation.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

      }).AsQueryable();

                List<Investigation> Investigations = new List<Investigation>();

                if (encounterType == null)
                    Investigations = await investigationsAsQuerable.OrderByDescending(x => x.EncounterDate).ToListAsync();
                else
                    Investigations = await investigationsAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

                //if (encounterType == null)
                //{
                //    var i = await LoadListWithChildAsync<Investigation>(p => p.IsDeleted == false && p.ClientId == clientId, page, pageSize, orderBy: d => d.OrderByDescending(y => y.DateCreated), t => t.Test, tts => tts.Test.TestSubtype, tt => tt.Test.TestSubtype.TestType, r => r.Results);
                //    Investigations = i.ToList();
                //}
                //else
                //{
                //    var i = await LoadListWithChildAsync<Investigation>(p => p.IsDeleted == false && p.ClientId == clientId && p.EncounterType == encounterType, page, pageSize, orderBy: d => d.OrderByDescending(y => y.DateCreated), t => t.Test, tts => tts.Test.TestSubtype, tt => tt.Test.TestSubtype.TestType, r => r.Results);
                //    Investigations = i.ToList();
                //}

                foreach (var item in Investigations.Where(x => x.IsResultReceived == true).ToList())
                {
                    var results = item.Results.Where(x => x.ResultOptionId != null).ToList();

                    foreach (var item2 in results)
                    {
                        var ResultoPtion = await context.ResultOptions.Where(x => x.Oid == item2.ResultOptionId).FirstOrDefaultAsync();

                        item2.ResultOption = ResultoPtion;
                    }

                    var measuringResults = item.Results.Where(x => x.MeasuringUnitId != null).ToList();

                    foreach (var Measurements in measuringResults)
                    {
                        var MeasurementUnit = await context.MeasuringUnits.Where(x => x.Oid == Measurements.MeasuringUnitId.Value).FirstOrDefaultAsync();

                        Measurements.MeasuringUnit = MeasurementUnit;
                    }

                }
                List<InvestigationDto> investigationDto = Investigations.GroupBy(x => x.EncounterId).Select(x => new InvestigationDto()
                {
                    EncounterID = x.Key,
                    investigation = x.ToList(),
                    ClientID = clientId,
                    DateCreated = x.Select(x => x.DateCreated).FirstOrDefault(),
                    EncounterDate = x.Select(x => x.EncounterDate).FirstOrDefault(),
                    CreatedIn = x.Select(x => x.CreatedIn).FirstOrDefault(),
                    CreatedBy = x.Select(x => x.CreatedBy).FirstOrDefault(),
                    FacilityName = x.Select(x => x.FacilityName).FirstOrDefault(),
                    ClinicianName = x.Select(x => x.ClinicianName).FirstOrDefault(),
                    investigationWithOutComposite = x.Select(x => new InvestigationWithOutComposite()
                    {
                        AdditionalComment = x.AdditionalComment,
                        InteractionID = x.InteractionId,
                        ClientID = x.ClientId,
                        Client = x.Client,
                        Quantity = x.Quantity,
                        ClinicianID = x.ClinicianId,
                        EncounterID = x.EncounterId,
                        ImagingTestDetails = x.ImagingTestDetails,
                        IsResultReceived = x.IsResultReceived,
                        OrderDate = x.OrderDate,
                        Piority = x.Piority,
                        OrderNumber = x.OrderNumber,
                        Results = x.Results,
                        SampleQuantity = x.SampleQuantity,
                        Test = x.Test,

                        TestID = x.TestId,
                        TestTypeId = x.TestTypeId,
                        UserAccount = x.UserAccount,
                        CreatedBy = x.CreatedBy,
                        DateCreated = x.DateCreated,
                        CreatedIn = x.CreatedIn,
                        DateModified = x.DateModified,
                        IsDeleted = (bool)x.IsDeleted,
                        IsSynced = (bool)x.IsSynced,
                        TestResult =
                        x.IsResultReceived == true ?
                        (x.Results.Count() > 0 ?
                        (string.IsNullOrEmpty(x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().ResultDescriptive) ?

                        (x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().ResultNumeric == null ?
                        (x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().ResultOptionId == null ? ""
                        : x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().ResultOption.Description)
                        : x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().ResultNumeric.Value.ToString()) : x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().ResultDescriptive.ToString())
                        : "") : "",
                        MaxumumRange = ((x.IsResultReceived == true && x.Results.Count() > 0 && x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().MeasuringUnitId != null)

                        ? x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().MeasuringUnit.MaximumRange : null),
                        UnitTest = ((x.IsResultReceived == true && x.Results.Count() > 0 && x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().MeasuringUnitId != null) ? x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().MeasuringUnit.Description : null),
                        MinumumRange = ((x.IsResultReceived == true && x.Results.Count() > 0 && x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().MeasuringUnitId != null) ? x.Results.Where(y => y.InvestigationId == x.InteractionId).FirstOrDefault().MeasuringUnit.MinimumRange : null)

                    }).ToList(),

                }).ToList();

                var testItems = await context.TestItems.Include(tts => tts.CompositeTest).Where(b => b.IsDeleted == false).ToListAsync();
                var testItemsByGroup = testItems.GroupBy(x => x.CompositeTestId).ToList();

                foreach (var item in investigationDto)
                {

                    foreach (var test in testItemsByGroup)
                    {
                        var ListCompositeTest = test.Select(x => x.TestId).ToList();

                        var InvestigationTest = item.investigationWithOutComposite.Select(x => x.TestID).ToList();
                        var compositeTest = ListCompositeTest.All(item => InvestigationTest.Contains(item));

                        if (compositeTest)
                        {
                            var investigationWithComposite = item.investigationWithOutComposite.Where(x => ListCompositeTest.Contains(x.TestID)).Select(x => new InvestigationWithComposite()
                            {
                                CompositeName = test.Select(x => x.CompositeTest.Description).FirstOrDefault(),
                                AdditionalComment = x.AdditionalComment,
                                InteractionID = x.InteractionID,
                                ClientID = x.ClientID,
                                Client = x.Client,
                                Quantity = x.Quantity,
                                ClinicianID = x.ClinicianID,
                                EncounterID = x.EncounterID,
                                ImagingTestDetails = x.ImagingTestDetails,
                                IsResultReceived = x.IsResultReceived,
                                OrderDate = x.OrderDate,
                                Piority = x.Piority,
                                OrderNumber = x.OrderNumber,
                                Results = x.Results,
                                SampleQuantity = x.SampleQuantity,
                                Test = x.Test,
                                TestID = x.TestID,
                                TestTypeId = x.TestTypeId,
                                UserAccount = x.UserAccount,
                                CreatedBy = x.CreatedBy,
                                DateCreated = x.DateCreated,
                                EncounterDate = x.EncounterDate,
                                CreatedIn = x.CreatedIn,
                                DateModified = x.DateModified,
                                IsDeleted = x.IsDeleted,
                                IsSynced = x.IsSynced,
                                FacilityName = x.FacilityName,
                                ClinicianName=x.ClinicianName,
                                TestResult =
                         x.IsResultReceived == true ?
                         (x.Results.Count() > 0 ?
                         (string.IsNullOrEmpty(x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().ResultDescriptive) ?

                         (x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().ResultNumeric == null ?
                         (x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().ResultOptionId == null ? "" : x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().ResultOption.Description)
                         : x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().ResultNumeric.Value.ToString()) :
                         x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().ResultDescriptive.ToString())
                         : "") : "",
                                MaxumumRange = ((x.IsResultReceived == true && x.Results.Count() > 0 && x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().MeasuringUnitId != null)

                         ? x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().MeasuringUnit.MaximumRange : null),

                                UnitTest = ((x.IsResultReceived == true && x.Results.Count() > 0 && x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().MeasuringUnitId != null) ? x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().MeasuringUnit.Description : null),

                                MinumumRange = ((x.IsResultReceived == true && x.Results.Count() > 0 && x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().MeasuringUnitId != null)

                         ? x.Results.Where(y => y.InvestigationId == x.InteractionID).FirstOrDefault().MeasuringUnit.MinimumRange : null)

                            }).ToList();
                            if (item.investigationWithComposite == null)
                            {
                                item.investigationWithComposite = investigationWithComposite;
                            }
                            else
                            {
                                item.investigationWithComposite.AddRange(investigationWithComposite);
                            }
                            item.investigationWithOutComposite = item.investigationWithOutComposite.Where(x => !ListCompositeTest.Contains(x.TestID)).ToList();
                        }
                    }
                }
                return investigationDto;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int GetInvestigationByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.Investigations.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.Investigations.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get a Investigation by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a Investigation if the EncounterID is matched.</returns>
        public async Task<IEnumerable<Investigation>> GetInvestigationByEncounter(Guid encounterId)
        {
            try
            {

                return await LoadListWithChildAsync<Investigation>(b => b.IsDeleted == false && b.EncounterId == encounterId, t => t.Test, tts => tts.Test.TestSubtype, tt => tt.Test.TestSubtype.TestType, r => r.Results, c => c.Client);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Investigation.
        /// </summary>
        /// <param name="facilityId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="PatientName"></param>
        /// <param name="investigationDateSearch"></param>
        /// <returns>Returns a Investigation.</returns>
        public async Task<IEnumerable<Investigation>> GetInvestigationDashBoard(int facilityId, int skip, int take, string PatientName, string investigationDateSearch)
        {
            try
            {
                if (string.IsNullOrEmpty(PatientName) && string.IsNullOrEmpty(investigationDateSearch))
                {
                    return await LoadListWithChildAsync<Investigation>(p => p.IsDeleted == false && p.CreatedIn == facilityId, skip, take, orderBy: d => d.OrderByDescending(y => y.OrderDate), c => c.Client, m => m.Test, r => r.Results);
                }
                else if (!string.IsNullOrEmpty(PatientName) && string.IsNullOrEmpty(investigationDateSearch))
                {
                    string[] searchTerms = PatientName.Split(' ');
                    return await LoadListWithChildAsync<Investigation>(p => p.IsDeleted == false && p.CreatedIn == facilityId && (searchTerms.Contains(p.Client.FirstName) || searchTerms.Contains(p.Client.Surname)), skip, take, orderBy: d => d.OrderByDescending(y => y.OrderDate), c => c.Client, m => m.Test, c => c.Client, r => r.Results);
                }
                else if (string.IsNullOrEmpty(PatientName) && !string.IsNullOrEmpty(investigationDateSearch))
                {
                    DateTime InvestigationDate = DateTime.ParseExact(investigationDateSearch, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                    return await LoadListWithChildAsync<Investigation>(p => p.IsDeleted == false && p.CreatedIn == facilityId && p.OrderDate.Date == InvestigationDate, skip, take, orderBy: d => d.OrderByDescending(y => y.OrderDate), c => c.Client, m => m.Test, r => r.Results);
                }
                else if (!string.IsNullOrEmpty(PatientName) && !string.IsNullOrEmpty(investigationDateSearch))
                {
                    string[] searchTerms = PatientName.Split(' ');
                    DateTime InvestigationDate = DateTime.ParseExact(investigationDateSearch, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                    return await LoadListWithChildAsync<Investigation>(p => p.IsDeleted == false && p.CreatedIn == facilityId && p.OrderDate.Date == InvestigationDate && (searchTerms.Contains(p.Client.FirstName) || searchTerms.Contains(p.Client.Surname)), skip, take, orderBy: d => d.OrderByDescending(y => y.OrderDate), c => c.Client, m => m.Test, c => c.Client, r => r.Results);
                }
                else
                {
                    return await LoadListWithChildAsync<Investigation>(p => p.IsDeleted == false && p.CreatedIn == facilityId, skip, take, orderBy: d => d.OrderByDescending(y => y.OrderDate), c => c.Client, m => m.Test);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}