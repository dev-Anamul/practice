using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class FeedingMethodRepository : Repository<FeedingMethod>, IFeedingMethodRepository
    {
        /// <summary>
        /// Implementation of IFeedingMethodRepository interface.
        /// </summary>
        public FeedingMethodRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a FeedingMethod by key.
        /// </summary>
        /// <param name="key">Primary key of the table FeedingMethods.</param>
        /// <returns>Returns a FeedingMethod if the key is matched.</returns>
        public async Task<FeedingMethod> GetFeedingMethodByKey(Guid key)
        {
            try
            {
                var feedingMethod = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (feedingMethod != null)
                {
                    feedingMethod.ClinicianName = await context.UserAccounts.Where(x => x.Oid == feedingMethod.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    feedingMethod.FacilityName = await context.Facilities.Where(x => x.Oid == feedingMethod.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    feedingMethod.EncounterDate = await context.Encounters.Where(x => x.Oid == feedingMethod.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();


                }
                return feedingMethod;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of FeedingMethod.
        /// </summary>
        /// <returns>Returns a list of all FeedingMethods.</returns>
        public async Task<IEnumerable<FeedingMethod>> GetFeedingMethods()
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
        /// The method is used to get a FeedingMethod by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a FeedingMethod if the ClientID is matched.</returns>
        public async Task<IEnumerable<FeedingMethod>> GetFeedingMethodByClient(Guid clientId)
        {
            try
            {
                return await context.FeedingMethods.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
                .Join(
                    context.Encounters.AsNoTracking(),
                  feedingMethod => feedingMethod.EncounterId,
                    encounter => encounter.Oid,
                    (feedingMethod, encounter) => new FeedingMethod
                    {
                        EncounterId = feedingMethod.EncounterId,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        DateCreated = feedingMethod.DateCreated,
                        CreatedIn = feedingMethod.CreatedIn,
                        CreatedBy = feedingMethod.CreatedBy,
                        ClientId = feedingMethod.ClientId,
                        DateModified = feedingMethod.DateModified,
                        EncounterType = feedingMethod.EncounterType,
                        InteractionId = feedingMethod.InteractionId,
                        IsDeleted = feedingMethod.IsDeleted,
                        IsSynced = feedingMethod.IsSynced,
                        Methods = feedingMethod.Methods,
                        ModifiedBy = feedingMethod.ModifiedBy,
                        ModifiedIn = feedingMethod.ModifiedIn,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == feedingMethod.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == feedingMethod.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<FeedingMethod>> GetFeedingMethodByClientLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.FeedingMethods.Where(p => p.IsDeleted == false && p.ClientId == clientId && p.DateCreated >= Last24Hours).AsNoTracking()
                            .Join(
                                context.Encounters.AsNoTracking(),
                              feedingMethod => feedingMethod.EncounterId,
                                encounter => encounter.Oid,
                                (feedingMethod, encounter) => new FeedingMethod
                                {
                                    EncounterId = feedingMethod.EncounterId,
                                    EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                    DateCreated = feedingMethod.DateCreated,
                                    CreatedIn = feedingMethod.CreatedIn,
                                    CreatedBy = feedingMethod.CreatedBy,
                                    ClientId = feedingMethod.ClientId,
                                    DateModified = feedingMethod.DateModified,
                                    EncounterType = feedingMethod.EncounterType,
                                    InteractionId = feedingMethod.InteractionId,
                                    IsDeleted = feedingMethod.IsDeleted,
                                    IsSynced = feedingMethod.IsSynced,
                                    Methods = feedingMethod.Methods,
                                    ModifiedBy = feedingMethod.ModifiedBy,
                                    ModifiedIn = feedingMethod.ModifiedIn,
                                    ClinicianName = context.UserAccounts.Where(x => x.Oid == feedingMethod.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                                    FacilityName = context.Facilities.Where(x => x.Oid == feedingMethod.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                                }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<FeedingMethod>> GetFeedingMethodByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var feedingMethodAsQuerable = context.FeedingMethods.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
                .Join(
                    context.Encounters.AsNoTracking(),
                  feedingMethod => feedingMethod.EncounterId,
                    encounter => encounter.Oid,
                    (feedingMethod, encounter) => new FeedingMethod
                    {
                        EncounterId = feedingMethod.EncounterId,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        DateCreated = feedingMethod.DateCreated,
                        CreatedIn = feedingMethod.CreatedIn,
                        CreatedBy = feedingMethod.CreatedBy,
                        ClientId = feedingMethod.ClientId,
                        DateModified = feedingMethod.DateModified,
                        EncounterType = feedingMethod.EncounterType,
                        InteractionId = feedingMethod.InteractionId,
                        IsDeleted = feedingMethod.IsDeleted,
                        IsSynced = feedingMethod.IsSynced,
                        Methods = feedingMethod.Methods,
                        ModifiedBy = feedingMethod.ModifiedBy,
                        ModifiedIn = feedingMethod.ModifiedIn,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == feedingMethod.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == feedingMethod.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    }).AsQueryable();

                if (encounterType == null)
                    return await feedingMethodAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await feedingMethodAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetFeedingMethodByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.FeedingMethods.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.FeedingMethods.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }

        /// <summary>
        /// The method is used to get the list of FeedingMethod by EncounterID.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a list of all FeedingMethod by EncounterID.</returns>
        public async Task<IEnumerable<FeedingMethod>> GetFeedingMethodByEncounter(Guid encounterId)
        {
            try
            {
                return await context.FeedingMethods.Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
                .Join(
                    context.Encounters.AsNoTracking(),
                  feedingMethod => feedingMethod.EncounterId,
                    encounter => encounter.Oid,
                    (feedingMethod, encounter) => new FeedingMethod
                    {
                        EncounterId = feedingMethod.EncounterId,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        DateCreated = feedingMethod.DateCreated,
                        CreatedIn = feedingMethod.CreatedIn,
                        CreatedBy = feedingMethod.CreatedBy,
                        ClientId = feedingMethod.ClientId,
                        DateModified = feedingMethod.DateModified,
                        EncounterType = feedingMethod.EncounterType,
                        InteractionId = feedingMethod.InteractionId,
                        IsDeleted = feedingMethod.IsDeleted,
                        IsSynced = feedingMethod.IsSynced,
                        Methods = feedingMethod.Methods,
                        ModifiedBy = feedingMethod.ModifiedBy,
                        ModifiedIn = feedingMethod.ModifiedIn,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == feedingMethod.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == feedingMethod.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}