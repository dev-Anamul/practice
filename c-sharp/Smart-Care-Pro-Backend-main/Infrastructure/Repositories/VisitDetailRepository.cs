using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IVisitDetailRepository interface.
    /// </summary>
    public class VisitDetailRepository : Repository<VisitDetail>, IVisitDetailRepository
    {
        /// <summary>
        /// Implementation of IVisitDetailRepository interface.
        /// </summary>
        public VisitDetailRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a VisitDetail by key.
        /// </summary>
        /// <param name="key">Primary key of the table VisitDetails.</param>
        /// <returns>Returns a VisitDetail if the key is matched.</returns>
        public async Task<VisitDetail> GetVisitDetailByKey(Guid key)
        {
            try
            {
                var visitDetail = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (visitDetail != null)
                    visitDetail.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return visitDetail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of VisitDetail.
        /// </summary>
        /// <returns>Returns a list of all VisitDetails.</returns>
        public async Task<IEnumerable<VisitDetail>> GetVisitDetails()
        {
            try
            {
                return await QueryAsync(n => n.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a VisitDetail by ClientId.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns>Returns a VisitDetail if the ClientId is matched.</returns>
        public async Task<IEnumerable<VisitDetail>> GetVisitDetailByClient(Guid ClientId)
        {
            try
            {
                return await context.VisitDetails.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == ClientId)
                   .Join(
                          context.Encounters.AsNoTracking(),
                          visitDetail => visitDetail.EncounterId,
                          encounter => encounter.Oid,
                          (visitDetail, encounter) => new VisitDetail
                          {
                              EncounterId = visitDetail.EncounterId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClientId = visitDetail.ClientId,
                              InteractionId = visitDetail.InteractionId,
                              CreatedBy = visitDetail.CreatedBy,
                              CreatedIn = visitDetail.CreatedIn,
                              DateCreated = visitDetail.DateCreated,
                              DateModified = visitDetail.DateModified,
                              EncounterType = visitDetail.EncounterType,
                              IsBabyNameGiven = visitDetail.IsBabyNameGiven,
                              IsDeleted = visitDetail.IsDeleted,
                              IsSynced = visitDetail.IsSynced,
                              ModifiedBy = visitDetail.ModifiedBy,
                              ModifiedIn = visitDetail.ModifiedIn,
                              VisitType = visitDetail.VisitType,

                          }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<VisitDetail>> GetVisitDetailByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var visitDetailAsQuerable = context.VisitDetails.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                   .Join(
                          context.Encounters.AsNoTracking(),
                          visitDetail => visitDetail.EncounterId,
                          encounter => encounter.Oid,
                          (visitDetail, encounter) => new VisitDetail
                          {
                              EncounterId = visitDetail.EncounterId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClientId = visitDetail.ClientId,
                              InteractionId = visitDetail.InteractionId,
                              CreatedBy = visitDetail.CreatedBy,
                              CreatedIn = visitDetail.CreatedIn,
                              DateCreated = visitDetail.DateCreated,
                              DateModified = visitDetail.DateModified,
                              EncounterType = visitDetail.EncounterType,
                              IsBabyNameGiven = visitDetail.IsBabyNameGiven,
                              IsDeleted = visitDetail.IsDeleted,
                              IsSynced = visitDetail.IsSynced,
                              ModifiedBy = visitDetail.ModifiedBy,
                              ModifiedIn = visitDetail.ModifiedIn,
                              VisitType = visitDetail.VisitType,

                          }).AsQueryable();

                if (encounterType == null)
                    return await visitDetailAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await visitDetailAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetVisitDetailByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.VisitDetails.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.VisitDetails.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get the list of VisitDetail by EncounterId.
        /// </summary>
        /// <returns>Returns a list of all VisitDetail by EncounterId.</returns>
        public async Task<IEnumerable<VisitDetail>> GetVisitDetailByEncounter(Guid EncounterId)
        {
            try
            {
                return await context.VisitDetails.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterId)
                   .Join(
                          context.Encounters.AsNoTracking(),
                          visitDetail => visitDetail.EncounterId,
                          encounter => encounter.Oid,
                          (visitDetail, encounter) => new VisitDetail
                          {
                              EncounterId = visitDetail.EncounterId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClientId = visitDetail.ClientId,
                              InteractionId = visitDetail.InteractionId,
                              CreatedBy = visitDetail.CreatedBy,
                              CreatedIn = visitDetail.CreatedIn,
                              DateCreated = visitDetail.DateCreated,
                              DateModified = visitDetail.DateModified,
                              EncounterType = visitDetail.EncounterType,
                              IsBabyNameGiven = visitDetail.IsBabyNameGiven,
                              IsDeleted = visitDetail.IsDeleted,
                              IsSynced = visitDetail.IsSynced,
                              ModifiedBy = visitDetail.ModifiedBy,
                              ModifiedIn = visitDetail.ModifiedIn,
                              VisitType = visitDetail.VisitType,

                          }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}