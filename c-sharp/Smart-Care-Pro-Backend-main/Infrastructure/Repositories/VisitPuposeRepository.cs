using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 29.04.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IVisitPuposeRepository interface.
    /// </summary>
    public class VisitPuposeRepository : Repository<VisitPurpose>, IVisitPuposeRepository
    {
        public VisitPuposeRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a visit purposes by ClientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a visit purposes if the ClientId is matched.</returns>
        public async Task<IEnumerable<VisitPurpose>> GetVisitPurposeByClient(Guid clientId)
        {
            return await context.VisitPurposes.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                     .Join(
                            context.Encounters.AsNoTracking(),
                            visitPurpose => visitPurpose.EncounterId,
                            encounter => encounter.Oid,
                            (visitPurpose, encounter) => new VisitPurpose
                            {
                                EncounterId = visitPurpose.EncounterId,
                                EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                ClientId = visitPurpose.ClientId,
                                ModifiedIn = visitPurpose.ModifiedIn,
                                EncounterType = visitPurpose.EncounterType,
                                IsSynced = visitPurpose.IsSynced,
                                ModifiedBy = visitPurpose.ModifiedBy,
                                IsDeleted = visitPurpose.IsDeleted,
                                InteractionId = visitPurpose.InteractionId,
                                DateModified = visitPurpose.DateModified,
                                DateCreated = visitPurpose.DateCreated,
                                CreatedIn = visitPurpose.CreatedIn,
                                CreatedBy = visitPurpose.CreatedBy,
                                OtherReasonForFollowUp = visitPurpose.OtherReasonForFollowUp,
                                OtherReasonForStopping = visitPurpose.OtherReasonForStopping,
                                PregnancyIntension = visitPurpose.PregnancyIntension,
                                ReasonForFollowUp = visitPurpose.ReasonForFollowUp,
                                ReasonForStopping = visitPurpose.ReasonForStopping,
                                ReasonForVisit = visitPurpose.ReasonForVisit,
                                VisitPurposes = visitPurpose.VisitPurposes,

                            }).OrderByDescending(x => x.EncounterDate).ToListAsync();
        }

        public async Task<IEnumerable<VisitPurpose>> GetVisitPurposeByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            var visitPurposeAsQuerable = context.VisitPurposes.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                   .Join(
                          context.Encounters.AsNoTracking(),
                          visitPurpose => visitPurpose.EncounterId,
                          encounter => encounter.Oid,
                          (visitPurpose, encounter) => new VisitPurpose
                          {
                              EncounterId = visitPurpose.EncounterId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClientId = visitPurpose.ClientId,
                              ModifiedIn = visitPurpose.ModifiedIn,
                              EncounterType = visitPurpose.EncounterType,
                              IsSynced = visitPurpose.IsSynced,
                              ModifiedBy = visitPurpose.ModifiedBy,
                              IsDeleted = visitPurpose.IsDeleted,
                              InteractionId = visitPurpose.InteractionId,
                              DateModified = visitPurpose.DateModified,
                              DateCreated = visitPurpose.DateCreated,
                              CreatedIn = visitPurpose.CreatedIn,
                              CreatedBy = visitPurpose.CreatedBy,
                              OtherReasonForFollowUp = visitPurpose.OtherReasonForFollowUp,
                              OtherReasonForStopping = visitPurpose.OtherReasonForStopping,
                              PregnancyIntension = visitPurpose.PregnancyIntension,
                              ReasonForFollowUp = visitPurpose.ReasonForFollowUp,
                              ReasonForStopping = visitPurpose.ReasonForStopping,
                              ReasonForVisit = visitPurpose.ReasonForVisit,
                              VisitPurposes = visitPurpose.VisitPurposes,

                          }).AsQueryable();

            if (encounterType == null)
                return await visitPurposeAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            else
                return await visitPurposeAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

        }

        public int GetVisitPurposeByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.VisitPurposes.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.VisitPurposes.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }

        /// <summary>
        /// The method is used to get a visit purposes by EncounterId.
        /// </summary>
        /// <param name="EncounterId"></param>
        /// <returns>Returns a visit purposes if the EncounterId is matched.</returns>
        public async Task<IEnumerable<VisitPurpose>> GetVisitPurposeByEncounter(Guid EncounterId)
        {
            return await QueryAsync(v => v.IsDeleted == false && v.EncounterId == EncounterId);
        }

        /// <summary>
        /// The method is used to get a VisitPurpose by clientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a latest VisitPurpose if the clientID is matched.</returns>
        public async Task<VisitPurpose> GetLatestVisitPurposeByClientID(Guid clientId)
        {
            try
            {
                return await context.VisitPurposes.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                     .Join(
                            context.Encounters.AsNoTracking(),
                            visitPurpose => visitPurpose.EncounterId,
                            encounter => encounter.Oid,
                            (visitPurpose, encounter) => new VisitPurpose
                            {
                                EncounterId = visitPurpose.EncounterId,
                                EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                ClientId = visitPurpose.ClientId,
                                ModifiedIn = visitPurpose.ModifiedIn,
                                EncounterType = visitPurpose.EncounterType,
                                IsSynced = visitPurpose.IsSynced,
                                ModifiedBy = visitPurpose.ModifiedBy,
                                IsDeleted = visitPurpose.IsDeleted,
                                InteractionId = visitPurpose.InteractionId,
                                DateModified = visitPurpose.DateModified,
                                DateCreated = visitPurpose.DateCreated,
                                CreatedIn = visitPurpose.CreatedIn,
                                CreatedBy = visitPurpose.CreatedBy,
                                OtherReasonForFollowUp = visitPurpose.OtherReasonForFollowUp,
                                OtherReasonForStopping = visitPurpose.OtherReasonForStopping,
                                PregnancyIntension = visitPurpose.PregnancyIntension,
                                ReasonForFollowUp = visitPurpose.ReasonForFollowUp,
                                ReasonForStopping = visitPurpose.ReasonForStopping,
                                ReasonForVisit = visitPurpose.ReasonForVisit,
                                VisitPurposes = visitPurpose.VisitPurposes,

                            }).OrderByDescending(x => x.EncounterDate).FirstOrDefaultAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to a get visit purposes by key.
        /// </summary>
        /// <param name="key">Primary key of the table VisitPurposes.</param>
        /// <returns>Returns a visit purposes if the key is matched.</returns>
        public async Task<VisitPurpose> GetVisitPurposeByKey(Guid key)
        {
            var visitPurpose = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

            if (visitPurpose != null)
                visitPurpose.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

            return visitPurpose;
        }

        /// <summary>
        /// The method is used to get the list of visit purposes.
        /// </summary>
        /// <returns>Returns a list of all visit purposes.</returns>
        public async Task<IEnumerable<VisitPurpose>> GetVisitPurposes()
        {
            return await QueryAsync(v => v.IsDeleted == false);
        }
    }
}