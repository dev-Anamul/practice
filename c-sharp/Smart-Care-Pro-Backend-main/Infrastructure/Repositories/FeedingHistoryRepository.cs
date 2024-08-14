using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Bella
 * Date created : 28.03.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IFeedingHistoryRepository interface.
    /// </summary>
    public class FeedingHistoryRepository : Repository<FeedingHistory>, IFeedingHistoryRepository
    {
        public FeedingHistoryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a feeding history by key.
        /// </summary>
        /// <param name="key">Primary key of the table FeedingHistories.</param>
        /// <returns>Returns a feeding history if the key is matched.</returns>
        public async Task<FeedingHistory> GetFeedingHistoryByKey(Guid key)
        {
            try
            {
                var feedingHistory = await FirstOrDefaultAsync(f => f.InteractionId == key && f.IsDeleted == false);

                if (feedingHistory != null)
                {
                    feedingHistory.ClinicianName = await context.UserAccounts.Where(x => x.Oid == feedingHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    feedingHistory.FacilityName = await context.Facilities.Where(x => x.Oid == feedingHistory.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    feedingHistory.EncounterDate = await context.Encounters.Where(x => x.Oid == feedingHistory.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return feedingHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of feeding histories.
        /// </summary>
        /// <returns>Returns a list of all feeding histories.</returns>
        public async Task<IEnumerable<FeedingHistory>> GetFeedingHistories()
        {
            try
            {
                return await QueryAsync(f => f.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a feeding history by ClientID.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns>Returns a feeding history if the ClientID is matched.</returns>
        public async Task<IEnumerable<FeedingHistory>> GetFeedingHistoryByClient(Guid ClientId)
        {
            try
            {
                return await context.FeedingHistories.Where(p => p.IsDeleted == false && p.ClientId == ClientId).AsNoTracking()
                .Join(
                    context.Encounters.AsNoTracking(),
                  feedingHistories => feedingHistories.EncounterId,
                    encounter => encounter.Oid,
                    (feedingHistories, encounter) => new FeedingHistory
                    {
                        EncounterId = feedingHistories.EncounterId,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        ClientId = feedingHistories.ClientId,
                        Comments = feedingHistories.Comments,
                        CreatedBy = feedingHistories.CreatedBy,
                        CreatedIn = feedingHistories.CreatedIn,
                        DateCreated = feedingHistories.DateCreated,
                        DateModified = feedingHistories.DateModified,
                        EncounterType = feedingHistories.EncounterType,
                        FeedingCode = feedingHistories.FeedingCode,
                        InteractionId = feedingHistories.InteractionId,
                        IsDeleted = feedingHistories.IsDeleted,
                        IsSynced = feedingHistories.IsSynced,
                        ModifiedBy = feedingHistories.ModifiedBy,
                        ModifiedIn = feedingHistories.ModifiedIn,
                        OtherFeedingCode = feedingHistories.OtherFeedingCode,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == feedingHistories.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == feedingHistories.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


                    }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get a feeding history by EncounterId.
        /// </summary>
        /// <param name="EncounterId"></param>
        /// <returns>Returns a feeding history if the EncounterId is matched.</returns>
        public async Task<IEnumerable<FeedingHistory>> GetFeedingHistoryByEncounter(Guid EncounterId)
        {
            try
            {
                return await context.FeedingHistories.Where(p => p.IsDeleted == false && p.EncounterId == EncounterId).AsNoTracking()
                   .Join(
                       context.Encounters.AsNoTracking(),
                     feedingHistories => feedingHistories.EncounterId,
                       encounter => encounter.Oid,
                       (feedingHistories, encounter) => new FeedingHistory
                       {
                           EncounterId = feedingHistories.EncounterId,
                           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                           ClientId = feedingHistories.ClientId,
                           Comments = feedingHistories.Comments,
                           CreatedBy = feedingHistories.CreatedBy,
                           CreatedIn = feedingHistories.CreatedIn,
                           DateCreated = feedingHistories.DateCreated,
                           DateModified = feedingHistories.DateModified,
                           EncounterType = feedingHistories.EncounterType,
                           FeedingCode = feedingHistories.FeedingCode,
                           InteractionId = feedingHistories.InteractionId,
                           IsDeleted = feedingHistories.IsDeleted,
                           IsSynced = feedingHistories.IsSynced,
                           ModifiedBy = feedingHistories.ModifiedBy,
                           ModifiedIn = feedingHistories.ModifiedIn,
                           OtherFeedingCode = feedingHistories.OtherFeedingCode,
                           ClinicianName = context.UserAccounts.Where(x => x.Oid == feedingHistories.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                           FacilityName = context.Facilities.Where(x => x.Oid == feedingHistories.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


                       }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}