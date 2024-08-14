using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 13.01.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IHTSRepository interface.
    /// </summary>
    public class HTSRepository : Repository<HTS>, IHTSRepository
    {
        public HTSRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get HTS by ClientID.
        /// </summary>
        /// <param name="key">ClientID of HTS.</param>
        /// <returns>Returns HTS if the ClientID is matched.</returns>
        public async Task<IEnumerable<HTS>> GetHTSByClient(Guid key)
        {
            try
            {
                var hts = await QueryAsync(h => h.IsDeleted == false && h.ClientId == key);
                foreach (var item in hts)
                {
                    item.Client = await context.Clients.AsNoTracking().Where(x => x.Oid == key).FirstOrDefaultAsync();
                    item.Client.Occupation = await context.Occupations.AsNoTracking().Where(x => x.Oid == item.Client.OccupationId).FirstOrDefaultAsync();
                    item.Client.EducationLevel = await context.EducationLevels.AsNoTracking().Where(x => x.Oid == item.Client.EducationLevelId).FirstOrDefaultAsync();
                    item.Client.HomeLanguage = await context.HomeLanguages.AsNoTracking().Where(x => x.Oid == item.Client.HomeLanguageId).FirstOrDefaultAsync();
                    item.Client.Country = await context.Countries.AsNoTracking().Where(x => x.Oid == item.Client.CountryId).FirstOrDefaultAsync();
                    item.VisitType = await context.VisitTypes.AsNoTracking().Where(x => x.Oid == item.VisitTypeId).FirstOrDefaultAsync();
                    item.ClientType = await context.ClientTypes.AsNoTracking().Where(x => x.Oid == item.ClientTypeId).FirstOrDefaultAsync();
                    item.ServicePoint = await context.ServicePoints.AsNoTracking().Where(x => x.Oid == item.ServicePointId).FirstOrDefaultAsync();
                    item.ClinicianName = context.UserAccounts.Where(x => x.Oid == item.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "";
                    item.FacilityName = context.Facilities.Where(x => x.Oid == item.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "";

                }

                return hts;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get HTS by key.
        /// </summary>
        /// <param name="key">Primary key of the table HTS.</param>
        /// <returns>Returns HTS if the key is matched.</returns>
        public async Task<HTS> GetHTSByKey(Guid key)
        {
            try
            {
                return await context.HTS.Where(p => p.IsDeleted == false && p.InteractionId == key)
               .Include(c => c.Client)
               .Include(x => x.HIVNotTestingReason)
               .Include(h => h.HIVTestingReason)
               .Include(v => v.VisitType)
               .Include(c => c.ClientType)
               .Include(s => s.ServicePoint)
                .AsNoTracking()
.Join(
   context.Encounters.AsNoTracking(),
   hts => hts.EncounterId,
   encounter => encounter.Oid,
   (hts, encounter) => new HTS
   {
       EncounterId = hts.EncounterId,
       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
       ClientId = hts.ClientId,
       Client = hts.Client,
       BiolineTestResult = hts.BiolineTestResult,
       ClientSource = hts.ClientSource,
       ClientType = hts.ClientType,
       ClientTypeId = hts.ClientTypeId,
       ConsentForSMS = hts.ConsentForSMS,
       CreatedBy = hts.CreatedBy,
       CreatedIn = hts.CreatedIn,
       DateCreated = hts.DateCreated,
       DateModified = hts.DateModified,
       DetermineTestResult = hts.DetermineTestResult,
       EncounterType = hts.EncounterType,
       HasConsented = hts.HasConsented,
       HasCounselled = hts.HasCounselled,
       HIVNotTestingReason = hts.HIVNotTestingReason,
       HIVNotTestingReasonId = hts.HIVNotTestingReasonId,
       HIVTestingReasonId = hts.HIVTestingReasonId,
       HIVType = hts.HIVType,
       HIVTestingReason = hts.HIVTestingReason,
       InteractionId = hts.InteractionId,
       IsDeleted = hts.IsDeleted,
       IsDNAPCRSampleCollected = hts.IsDNAPCRSampleCollected,
       IsResultReceived = hts.IsResultReceived,
       IsSynced = hts.IsSynced,
       LastTested = hts.LastTested,
       LastTestResult = hts.LastTestResult,
       ModifiedBy = hts.ModifiedBy,
       ModifiedIn = hts.ModifiedIn,
       NotTestingReason = hts.NotTestingReason,
       PartnerHIVStatus = hts.PartnerHIVStatus,
       PartnerLastTestDate = hts.PartnerLastTestDate,
       RetestDate = hts.RetestDate,
       SampleCollectionDate = hts.SampleCollectionDate,
       ServicePoint = hts.ServicePoint,
       ServicePointId = hts.ServicePointId,
       TestNo = hts.TestNo,
       VisitType = hts.VisitType,
       VisitTypeId = hts.VisitTypeId,
       ClinicianName = context.UserAccounts.Where(x => x.Oid == hts.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
       FacilityName = context.Facilities.Where(x => x.Oid == hts.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

   }).FirstOrDefaultAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get HTS by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<HTS> GetLatestHTSByClient(Guid clientId)
        {
            return await context.HTS.Where(p => p.IsDeleted == false && p.ClientId == clientId)
                .Include(x => x.HIVNotTestingReason)
                .Include(h => h.HIVTestingReason)
                .Include(hv => hv.HIVNotTestingReason)
                .Include(v => v.VisitType)
                .Include(c => c.ClientType)
                .Include(s => s.ServicePoint)
                 .AsNoTracking()
.Join(
    context.Encounters.AsNoTracking(),
    hts => hts.EncounterId,
    encounter => encounter.Oid,
    (hts, encounter) => new HTS
    {
        EncounterId = hts.EncounterId,
        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
        ClientId = hts.ClientId,
        BiolineTestResult = hts.BiolineTestResult,
        ClientSource = hts.ClientSource,
        ClientType = hts.ClientType,
        ClientTypeId = hts.ClientTypeId,
        ConsentForSMS = hts.ConsentForSMS,
        CreatedBy = hts.CreatedBy,
        CreatedIn = hts.CreatedIn,
        DateCreated = hts.DateCreated,
        DateModified = hts.DateModified,
        DetermineTestResult = hts.DetermineTestResult,
        EncounterType = hts.EncounterType,
        HasConsented = hts.HasConsented,
        HasCounselled = hts.HasCounselled,
        HIVNotTestingReason = hts.HIVNotTestingReason,
        HIVNotTestingReasonId = hts.HIVNotTestingReasonId,
        HIVTestingReasonId = hts.HIVTestingReasonId,
        HIVType = hts.HIVType,
        HIVTestingReason = hts.HIVTestingReason,
        InteractionId = hts.InteractionId,
        IsDeleted = hts.IsDeleted,
        IsDNAPCRSampleCollected = hts.IsDNAPCRSampleCollected,
        IsResultReceived = hts.IsResultReceived,
        IsSynced = hts.IsSynced,
        LastTested = hts.LastTested,
        LastTestResult = hts.LastTestResult,
        ModifiedBy = hts.ModifiedBy,
        ModifiedIn = hts.ModifiedIn,
        NotTestingReason = hts.NotTestingReason,
        PartnerHIVStatus = hts.PartnerHIVStatus,
        PartnerLastTestDate = hts.PartnerLastTestDate,
        RetestDate = hts.RetestDate,
        SampleCollectionDate = hts.SampleCollectionDate,
        ServicePoint = hts.ServicePoint,
        ServicePointId = hts.ServicePointId,
        TestNo = hts.TestNo,
        VisitType = hts.VisitType,
        VisitTypeId = hts.VisitTypeId,
        ClinicianName = context.UserAccounts.Where(x => x.Oid == hts.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
        FacilityName = context.Facilities.Where(x => x.Oid == hts.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


    }).OrderByDescending(x => x.EncounterDate).FirstOrDefaultAsync();

        }

        /// <summary>
        /// The method is used to get the list of HTS.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a list of all HTS.</returns>
        public async Task<IEnumerable<HTS>> GetHTS(Guid clientId)
        {
            try
            {
                return await context.HTS.Where(p => p.IsDeleted == false && p.ClientId == clientId).Include(x => x.HIVNotTestingReason).AsNoTracking()
    .Join(
        context.Encounters.AsNoTracking(),
        hts => hts.EncounterId,
        encounter => encounter.Oid,
        (hts, encounter) => new HTS
        {
            EncounterId = hts.EncounterId,
            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
            ClientId = hts.ClientId,
            BiolineTestResult = hts.BiolineTestResult,
            ClientSource = hts.ClientSource,
            ClientType = hts.ClientType,
            ClientTypeId = hts.ClientTypeId,
            ConsentForSMS = hts.ConsentForSMS,
            CreatedBy = hts.CreatedBy,
            CreatedIn = hts.CreatedIn,
            DateCreated = hts.DateCreated,
            DateModified = hts.DateModified,
            DetermineTestResult = hts.DetermineTestResult,
            EncounterType = hts.EncounterType,
            HasConsented = hts.HasConsented,
            HasCounselled = hts.HasCounselled,
            HIVNotTestingReason = hts.HIVNotTestingReason,
            HIVNotTestingReasonId = hts.HIVNotTestingReasonId,
            HIVTestingReasonId = hts.HIVTestingReasonId,
            HIVType = hts.HIVType,
            HIVTestingReason = hts.HIVTestingReason,
            InteractionId = hts.InteractionId,
            IsDeleted = hts.IsDeleted,
            IsDNAPCRSampleCollected = hts.IsDNAPCRSampleCollected,
            IsResultReceived = hts.IsResultReceived,
            IsSynced = hts.IsSynced,
            LastTested = hts.LastTested,
            LastTestResult = hts.LastTestResult,
            ModifiedBy = hts.ModifiedBy,
            ModifiedIn = hts.ModifiedIn,
            NotTestingReason = hts.NotTestingReason,
            PartnerHIVStatus = hts.PartnerHIVStatus,
            PartnerLastTestDate = hts.PartnerLastTestDate,
            RetestDate = hts.RetestDate,
            SampleCollectionDate = hts.SampleCollectionDate,
            ServicePoint = hts.ServicePoint,
            ServicePointId = hts.ServicePointId,
            TestNo = hts.TestNo,
            VisitType = hts.VisitType,
            VisitTypeId = hts.VisitTypeId,
            ClinicianName = context.UserAccounts.Where(x => x.Oid == hts.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
            FacilityName = context.Facilities.Where(x => x.Oid == hts.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

        }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<HTS>> GetHTSLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.HTS.Where(p => p.IsDeleted == false && p.ClientId == clientId && p.DateCreated >= Last24Hours).Include(x => x.HIVNotTestingReason).AsNoTracking()
    .Join(
        context.Encounters.AsNoTracking(),
        hts => hts.EncounterId,
        encounter => encounter.Oid,
        (hts, encounter) => new HTS
        {
            EncounterId = hts.EncounterId,
            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
            ClientId = hts.ClientId,
            BiolineTestResult = hts.BiolineTestResult,
            ClientSource = hts.ClientSource,
            ClientType = hts.ClientType,
            ClientTypeId = hts.ClientTypeId,
            ConsentForSMS = hts.ConsentForSMS,
            CreatedBy = hts.CreatedBy,
            CreatedIn = hts.CreatedIn,
            DateCreated = hts.DateCreated,
            DateModified = hts.DateModified,
            DetermineTestResult = hts.DetermineTestResult,
            EncounterType = hts.EncounterType,
            HasConsented = hts.HasConsented,
            HasCounselled = hts.HasCounselled,
            HIVNotTestingReason = hts.HIVNotTestingReason,
            HIVNotTestingReasonId = hts.HIVNotTestingReasonId,
            HIVTestingReasonId = hts.HIVTestingReasonId,
            HIVType = hts.HIVType,
            HIVTestingReason = hts.HIVTestingReason,
            InteractionId = hts.InteractionId,
            IsDeleted = hts.IsDeleted,
            IsDNAPCRSampleCollected = hts.IsDNAPCRSampleCollected,
            IsResultReceived = hts.IsResultReceived,
            IsSynced = hts.IsSynced,
            LastTested = hts.LastTested,
            LastTestResult = hts.LastTestResult,
            ModifiedBy = hts.ModifiedBy,
            ModifiedIn = hts.ModifiedIn,
            NotTestingReason = hts.NotTestingReason,
            PartnerHIVStatus = hts.PartnerHIVStatus,
            PartnerLastTestDate = hts.PartnerLastTestDate,
            RetestDate = hts.RetestDate,
            SampleCollectionDate = hts.SampleCollectionDate,
            ServicePoint = hts.ServicePoint,
            ServicePointId = hts.ServicePointId,
            TestNo = hts.TestNo,
            VisitType = hts.VisitType,
            VisitTypeId = hts.VisitTypeId,
            ClinicianName = context.UserAccounts.Where(x => x.Oid == hts.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
            FacilityName = context.Facilities.Where(x => x.Oid == hts.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

        }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<HTS>> GetHTS(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var hts = context.HTS.Where(p => p.IsDeleted == false && p.ClientId == clientId).Include(x => x.HIVNotTestingReason).AsNoTracking()
    .Join(
        context.Encounters.AsNoTracking(),
        hts => hts.EncounterId,
        encounter => encounter.Oid,
        (hts, encounter) => new HTS
        {
            EncounterId = hts.EncounterId,
            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
            ClientId = hts.ClientId,
            BiolineTestResult = hts.BiolineTestResult,
            ClientSource = hts.ClientSource,
            ClientType = hts.ClientType,
            ClientTypeId = hts.ClientTypeId,
            ConsentForSMS = hts.ConsentForSMS,
            CreatedBy = hts.CreatedBy,
            CreatedIn = hts.CreatedIn,
            DateCreated = hts.DateCreated,
            DateModified = hts.DateModified,
            DetermineTestResult = hts.DetermineTestResult,
            EncounterType = hts.EncounterType,
            HasConsented = hts.HasConsented,
            HasCounselled = hts.HasCounselled,
            HIVNotTestingReason = hts.HIVNotTestingReason,
            HIVNotTestingReasonId = hts.HIVNotTestingReasonId,
            HIVTestingReasonId = hts.HIVTestingReasonId,
            HIVType = hts.HIVType,
            HIVTestingReason = hts.HIVTestingReason,
            InteractionId = hts.InteractionId,
            IsDeleted = hts.IsDeleted,
            IsDNAPCRSampleCollected = hts.IsDNAPCRSampleCollected,
            IsResultReceived = hts.IsResultReceived,
            IsSynced = hts.IsSynced,
            LastTested = hts.LastTested,
            LastTestResult = hts.LastTestResult,
            ModifiedBy = hts.ModifiedBy,
            ModifiedIn = hts.ModifiedIn,
            NotTestingReason = hts.NotTestingReason,
            PartnerHIVStatus = hts.PartnerHIVStatus,
            PartnerLastTestDate = hts.PartnerLastTestDate,
            RetestDate = hts.RetestDate,
            SampleCollectionDate = hts.SampleCollectionDate,
            ServicePoint = hts.ServicePoint,
            ServicePointId = hts.ServicePointId,
            TestNo = hts.TestNo,
            VisitType = hts.VisitType,
            VisitTypeId = hts.VisitTypeId,
            ClinicianName = context.UserAccounts.Where(x => x.Oid == hts.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
            FacilityName = context.Facilities.Where(x => x.Oid == hts.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


        }).AsQueryable();

                if (encounterType == null)
                    return await hts.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await hts.Where(p => p.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetHTSTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.HTS.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.HTS.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
    }
}