using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
*Created by: Stephan
* Date created: 29.04.2023
* Modified by: Stephan
* Last modified: 13.08.2023
* Reviewed by:
*Date reviewed:
*/
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IVMMCCampaignRepository interface.
    /// </summary>
    public class VMMCServiceRepository : Repository<VMMCService>, IVMMCServiceRepository
    {
        public VMMCServiceRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a VMMCService by ClientId.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns>Returns a VMMCService if the ClientId is matched.</returns>
        public async Task<IEnumerable<VMMCService>> GetVMMCServiceByClient(Guid clientId)
        {
            try
            {
                return await context.VMMCServices.AsNoTracking().Where(p => p.IsDeleted == false && p.Oid == clientId)
                    .Join(
                           context.Encounters.AsNoTracking(),
                           vMMCService => vMMCService.EncounterId,
                           encounter => encounter.Oid,
                           (vMMCService, encounter) => new VMMCService
                           {
                               EncounterId = vMMCService.EncounterId,
                               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                               Oid = vMMCService.Oid,
                               CreatedIn = vMMCService.CreatedIn,
                               CreatedBy = vMMCService.CreatedBy,
                               DateCreated = vMMCService.DateCreated,
                               AtlantoOccipitalFlexion = vMMCService.AtlantoOccipitalFlexion,
                               ASAClassification = vMMCService.ASAClassification,
                               DateModified = vMMCService.DateModified,
                               EncounterType = vMMCService.EncounterType,
                               HasDentures = vMMCService.HasDentures,
                               BonyLandmarks = vMMCService.BonyLandmarks,
                               HasAbnormalitiesOfTheNeck = vMMCService.HasAbnormalitiesOfTheNeck,
                               HasLooseTeeth = vMMCService.HasLooseTeeth,
                               HIVStatusEvidencePresented = vMMCService.HIVStatusEvidencePresented,
                               InterincisorGap = vMMCService.InterincisorGap,
                               IsConsentGiven = vMMCService.IsConsentGiven,
                               IsDeleted = vMMCService.IsDeleted,
                               IsHIVTestingServiceOffered = vMMCService.IsHIVTestingServiceOffered,
                               IsPostTestCounsellingOffered = vMMCService.IsPostTestCounsellingOffered,
                               IsPreTestCounsellingOffered = vMMCService.IsPreTestCounsellingOffered,
                               IsReferredToARTIfPositive = vMMCService.IsReferredToARTIfPositive,
                               IsSynced = vMMCService.IsSynced,
                               IVAccess = vMMCService.IVAccess,
                               MallampatiClass = vMMCService.MallampatiClass,
                               MandibleSize = vMMCService.MandibleSize,
                               MCNumber = vMMCService.MCNumber,
                               ModifiedBy = vMMCService.ModifiedBy,
                               ModifiedIn = vMMCService.ModifiedIn,
                               MovementOfHeadNeck = vMMCService.MovementOfHeadNeck,
                               PresentedHIVStatus = vMMCService.PresentedHIVStatus,
                               ThyromentalDistance = vMMCService.ThyromentalDistance,
                               TongueSize = vMMCService.TongueSize,

                           }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to a get VMMCService by key.
        /// </summary>
        /// <param name="key">Primary key of the table VMMCServices.</param>
        /// <returns>Returns a VMMCService if the key is matched.</returns>
        public async Task<VMMCService> GetVMMCServiceByKey(Guid key)
        {
            try
            {
                var vMMCService = await FirstOrDefaultAsync(s => s.Oid == key && s.IsDeleted == false);

                if (vMMCService != null)
                    vMMCService.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return vMMCService;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of VMMC Service.
        /// </summary>
        /// <returns>Returns a list of all VMMC Service.</returns>
        public async Task<IEnumerable<VMMCService>> GetVMMCServices()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to Get dot by EncounterId.
        /// </summary>
        /// <param name="EncounterId"></param>
        /// <returns>Returns a dots by EncounterId.</returns>
        public async Task<IEnumerable<VMMCService>> GetVMMCServiceByEncounterId(Guid encounterId)
        {
            try
            {
                return await context.VMMCServices.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId)
                     .Join(
                            context.Encounters.AsNoTracking(),
                            vMMCService => vMMCService.EncounterId,
                            encounter => encounter.Oid,
                            (vMMCService, encounter) => new VMMCService
                            {
                                EncounterId = vMMCService.EncounterId,
                                EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                Oid = vMMCService.Oid,
                                CreatedIn = vMMCService.CreatedIn,
                                CreatedBy = vMMCService.CreatedBy,
                                DateCreated = vMMCService.DateCreated,
                                AtlantoOccipitalFlexion = vMMCService.AtlantoOccipitalFlexion,
                                ASAClassification = vMMCService.ASAClassification,
                                DateModified = vMMCService.DateModified,
                                EncounterType = vMMCService.EncounterType,
                                HasDentures = vMMCService.HasDentures,
                                BonyLandmarks = vMMCService.BonyLandmarks,
                                HasAbnormalitiesOfTheNeck = vMMCService.HasAbnormalitiesOfTheNeck,
                                HasLooseTeeth = vMMCService.HasLooseTeeth,
                                HIVStatusEvidencePresented = vMMCService.HIVStatusEvidencePresented,
                                InterincisorGap = vMMCService.InterincisorGap,
                                IsConsentGiven = vMMCService.IsConsentGiven,
                                IsDeleted = vMMCService.IsDeleted,
                                IsHIVTestingServiceOffered = vMMCService.IsHIVTestingServiceOffered,
                                IsPostTestCounsellingOffered = vMMCService.IsPostTestCounsellingOffered,
                                IsPreTestCounsellingOffered = vMMCService.IsPreTestCounsellingOffered,
                                IsReferredToARTIfPositive = vMMCService.IsReferredToARTIfPositive,
                                IsSynced = vMMCService.IsSynced,
                                IVAccess = vMMCService.IVAccess,
                                MallampatiClass = vMMCService.MallampatiClass,
                                MandibleSize = vMMCService.MandibleSize,
                                MCNumber = vMMCService.MCNumber,
                                ModifiedBy = vMMCService.ModifiedBy,
                                ModifiedIn = vMMCService.ModifiedIn,
                                MovementOfHeadNeck = vMMCService.MovementOfHeadNeck,
                                PresentedHIVStatus = vMMCService.PresentedHIVStatus,
                                ThyromentalDistance = vMMCService.ThyromentalDistance,
                                TongueSize = vMMCService.TongueSize,

                            }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}