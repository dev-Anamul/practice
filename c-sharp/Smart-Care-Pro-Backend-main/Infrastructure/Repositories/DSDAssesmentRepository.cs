using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IDSDAssesmentRepository interface.
    /// </summary>
    public class DSDAssesmentRepository : Repository<DSDAssessment>, IDSDAssesmentRepository
    {
        public DSDAssesmentRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a DSDAssesment by key.
        /// </summary>
        /// <param name="key">Primary key of the table DSDAssesment.</param>
        /// <returns>Returns a DSDAssesment if the key is matched.</returns>
        public async Task<IEnumerable<DSDAssessment>> GetDSDAssesmentByClient(Guid ClientID)
        {
            return await context.DSDAssesments.Where(p => p.IsDeleted == false && p.ClientId == ClientID).AsNoTracking()
         .Join(
             context.Encounters.AsNoTracking(),
            dSDAssesments => dSDAssesments.EncounterId,
             encounter => encounter.Oid,
             (dSDAssesments, encounter) => new DSDAssessment
             {
                 EncounterId = dSDAssesments.EncounterId,
                 EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                 CentralDispensingUnit = dSDAssesments.CentralDispensingUnit,
                 ClientId = dSDAssesments.ClientId,
                 CommunityARTDistributionPoints = dSDAssesments.CommunityARTDistributionPoints,
                 CommunityPost = dSDAssesments.CommunityPost,
                 CreatedBy = dSDAssesments.CreatedBy,
                 CreatedIn = dSDAssesments.CreatedIn,
                 DateCreated = dSDAssesments.DateCreated,
                 DateModified = dSDAssesments.DateModified,
                 EncounterType = dSDAssesments.EncounterType,
                 FastTrack = dSDAssesments.FastTrack,
                 HealthPost = dSDAssesments.HealthPost,
                 InteractionId = dSDAssesments.InteractionId,
                 IsClientStableOnCare = dSDAssesments.IsClientStableOnCare,
                 IsDeleted = dSDAssesments.IsDeleted,
                 IsSynced = dSDAssesments.IsSynced,
                 MobileARTDistributionModel = dSDAssesments.IsClientStableOnCare,
                 ModifiedBy = dSDAssesments.ModifiedBy,
                 ModifiedIn = dSDAssesments.ModifiedIn,
                 Other = dSDAssesments.Other,
                 RuralAdherenceModel = dSDAssesments.RuralAdherenceModel,
                 Scholar = dSDAssesments.Scholar,
                 ShouldContinueDSD = dSDAssesments.ShouldContinueDSD,
                 ShouldReferToClinician = dSDAssesments.ShouldReferToClinician,
                 Weekend = dSDAssesments.Weekend,
                 ClinicianName = context.UserAccounts.Where(x => x.Oid == dSDAssesments.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                 FacilityName = context.Facilities.Where(x => x.Oid == dSDAssesments.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

             }).ToListAsync();

        }

        /// <summary>
        /// The method is used to get a DSDAssesment by key.
        /// </summary>
        /// <param name="key">Primary key of the table DSDAssesment.</param>
        /// <returns>Returns a DSDAssesment if the key is matched.</returns>
        public async Task<IEnumerable<DSDAssessment>> GetDSDAssesmentByEncounter(Guid EncounterID)
        {
            return await context.DSDAssesments.Where(p => p.IsDeleted == false && p.EncounterId == EncounterID).AsNoTracking()
         .Join(
             context.Encounters.AsNoTracking(),
            dSDAssesments => dSDAssesments.EncounterId,
             encounter => encounter.Oid,
             (dSDAssesments, encounter) => new DSDAssessment
             {
                 EncounterId = dSDAssesments.EncounterId,
                 EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                 CentralDispensingUnit = dSDAssesments.CentralDispensingUnit,
                 ClientId = dSDAssesments.ClientId,
                 CommunityARTDistributionPoints = dSDAssesments.CommunityARTDistributionPoints,
                 CommunityPost = dSDAssesments.CommunityPost,
                 CreatedBy = dSDAssesments.CreatedBy,
                 CreatedIn = dSDAssesments.CreatedIn,
                 DateCreated = dSDAssesments.DateCreated,
                 DateModified = dSDAssesments.DateModified,
                 EncounterType = dSDAssesments.EncounterType,
                 FastTrack = dSDAssesments.FastTrack,
                 HealthPost = dSDAssesments.HealthPost,
                 InteractionId = dSDAssesments.InteractionId,
                 IsClientStableOnCare = dSDAssesments.IsClientStableOnCare,
                 IsDeleted = dSDAssesments.IsDeleted,
                 IsSynced = dSDAssesments.IsSynced,
                 MobileARTDistributionModel = dSDAssesments.IsClientStableOnCare,
                 ModifiedBy = dSDAssesments.ModifiedBy,
                 ModifiedIn = dSDAssesments.ModifiedIn,
                 Other = dSDAssesments.Other,
                 RuralAdherenceModel = dSDAssesments.RuralAdherenceModel,
                 Scholar = dSDAssesments.Scholar,
                 ShouldContinueDSD = dSDAssesments.ShouldContinueDSD,
                 ShouldReferToClinician = dSDAssesments.ShouldReferToClinician,
                 Weekend = dSDAssesments.Weekend,
                 ClinicianName = context.UserAccounts.Where(x => x.Oid == dSDAssesments.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                 FacilityName = context.Facilities.Where(x => x.Oid == dSDAssesments.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

             }).OrderByDescending(x => x.EncounterDate).ToListAsync();

        }

        /// <summary>
        /// The method is used to get a DSDAssesment by key.
        /// </summary>
        /// <param name="key">Primary key of the table DSDAssesment.</param>
        /// <returns>Returns a DSDAssesment if the key is matched.</returns>
        public async Task<DSDAssessment> GetDSDAssesmentByKey(Guid key)
        {
            var dsd = await FirstOrDefaultAsync(d => d.IsDeleted == false && d.InteractionId == key);

            if (dsd != null)
            {
                dsd.ClinicianName = await context.UserAccounts.Where(x => x.Oid == dsd.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                dsd.FacilityName = await context.Facilities.Where(x => x.Oid == dsd.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                dsd.EncounterDate = await context.Encounters.Where(x => x.Oid == dsd.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
            }

            return dsd;
            //return await FirstOrDefaultAsync(d => d.IsDeleted == false && d.InteractionId == key);
        }

        /// <summary>
        /// The method is used to get the list of DSDAssesment.
        /// </summary>
        /// <returns>Returns a list of all DSDAssesment.</returns>
        public async Task<IEnumerable<DSDAssessment>> GetDSDAssesments()
        {
            return await QueryAsync(d => d.IsDeleted == false);
        }
    }
}
