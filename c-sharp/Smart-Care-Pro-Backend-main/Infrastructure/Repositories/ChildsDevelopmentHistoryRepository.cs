using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

/*
 * Created by    : Sayem
 * Date created  : 25.12.2022
 * Modified by   : Shakil
 * Last modified : 18.01.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IChildsDevelopmentHistoryRepository interface.
    /// </summary>
    public class ChildsDevelopmentHistoryRepository : Repository<ChildsDevelopmentHistory>, IChildsDevelopmentHistoryRepository
    {
        public ChildsDevelopmentHistoryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a child's development history by key.
        /// </summary>
        /// <param name="key">Primary key of the table ChildsDevelopmentHistories.</param>
        /// <returns>Returns a child development history if the key is matched.</returns>
        public async Task<ChildsDevelopmentHistory> GetChildsDevelopmentHistoryByKey(Guid key)
        {
            try
            {
                var childsDevelopmentHistory = await context.ChildsDevelopmentHistories.AsNoTracking().FirstOrDefaultAsync(c => c.InteractionId == key && c.IsDeleted == false);

                if (childsDevelopmentHistory != null)
                {
                    childsDevelopmentHistory.EncounterDate = await context.Encounters.Where(x => x.Oid == childsDevelopmentHistory.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                    childsDevelopmentHistory.ClinicianName = context.UserAccounts.Where(x => x.Oid == childsDevelopmentHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "";
                    childsDevelopmentHistory.FacilityName = context.Facilities.Where(x => x.Oid == childsDevelopmentHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "";
                }

                return childsDevelopmentHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of child's development histories.
        /// </summary>
        /// <returns>Returns a list of all child's development histories.</returns>
        public async Task<IEnumerable<ChildsDevelopmentHistory>> GetChildsDevelopmentHistories()
        {
            try
            {
                try
                {
                    return await context.ChildsDevelopmentHistories.AsNoTracking().Where(x => x.IsDeleted == false)
                        .Join(
                            context.Encounters.AsNoTracking(),
                            childDevelopment => childDevelopment.EncounterId,
                            encounter => encounter.Oid,
                            (childDevelopment, encounter) => new ChildsDevelopmentHistory
                            {
                                // Properties from ChildsDevelopmentHistory
                                InteractionId = childDevelopment.InteractionId,
                                FeedingHistory = childDevelopment.FeedingHistory,
                                SocialSmile = childDevelopment.SocialSmile,
                                HeadHolding = childDevelopment.HeadHolding,
                                TurnTowardSoundOrigin = childDevelopment.TurnTowardSoundOrigin,
                                GraspToy = childDevelopment.GraspToy,
                                FollowObjectsWithEyes = childDevelopment.FollowObjectsWithEyes,
                                RollsOver = childDevelopment.RollsOver,
                                Babbles = childDevelopment.Babbles,
                                TakesObjectsToMouth = childDevelopment.TakesObjectsToMouth,
                                RepeatsSyllables = childDevelopment.RepeatsSyllables,
                                MovesObjects = childDevelopment.MovesObjects,
                                PlaysPeekaBoo = childDevelopment.PlaysPeekaBoo,
                                RespondsToOwnName = childDevelopment.RespondsToOwnName,
                                TakesStepsWithSupport = childDevelopment.TakesStepsWithSupport,
                                PicksUpSmallObjects = childDevelopment.PicksUpSmallObjects,
                                ImitatesSimpleGestures = childDevelopment.ImitatesSimpleGestures,
                                SaysTwoToThreeWords = childDevelopment.SaysTwoToThreeWords,
                                Sitting = childDevelopment.Sitting,
                                Standing = childDevelopment.Standing,
                                Walking = childDevelopment.Walking,
                                Talking = childDevelopment.Talking,
                                DrinksFromCup = childDevelopment.DrinksFromCup,
                                SaysSevenToTenWords = childDevelopment.SaysSevenToTenWords,
                                PointsToBodyParts = childDevelopment.PointsToBodyParts,
                                StartsToRun = childDevelopment.StartsToRun,
                                PointsPictureOnRequest = childDevelopment.PointsPictureOnRequest,
                                Sings = childDevelopment.Sings,
                                BuildTowerWithBox = childDevelopment.BuildTowerWithBox,
                                JumpsAndRun = childDevelopment.JumpsAndRun,
                                BeginsToDressAndUndress = childDevelopment.BeginsToDressAndUndress,
                                GroupsSimilarObjects = childDevelopment.GroupsSimilarObjects,
                                PlaysWithOtherChildren = childDevelopment.PlaysWithOtherChildren,
                                SaysFirstNameAndShortStory = childDevelopment.SaysFirstNameAndShortStory,

                                // Properties from EncounterBaseModel
                                EncounterId = childDevelopment.EncounterId,
                                EncounterType = childDevelopment.EncounterType,
                                CreatedIn = childDevelopment.CreatedIn,
                                DateCreated = childDevelopment.DateCreated,
                                CreatedBy = childDevelopment.CreatedBy,
                                ModifiedIn = childDevelopment.ModifiedIn,
                                DateModified = childDevelopment.DateModified,
                                ModifiedBy = childDevelopment.ModifiedBy,
                                IsDeleted = childDevelopment.IsDeleted,
                                IsSynced = childDevelopment.IsSynced,
                                EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,

                                ClientId = childDevelopment.ClientId,
                                Client = childDevelopment.Client,
                                ClinicianName = context.UserAccounts.Where(x => x.Oid == childDevelopment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                                FacilityName = context.Facilities.Where(x => x.Oid == childDevelopment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            }
                        )
                        .OrderByDescending(x => x.EncounterDate)
                        .ToListAsync();


                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a OPDVisit by key.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisit.</param>
        /// <returns>Returns a OPDVisit if the key is matched.</returns>
        public async Task<IEnumerable<ChildsDevelopmentHistory>> GetChildsDevelopmentHistoryByOpdVisit(Guid OPDVisitID)
        {
            try
            {
                return await context.ChildsDevelopmentHistories.AsNoTracking().Where(x => x.IsDeleted == false && x.EncounterId == OPDVisitID)
                        .Join(
                            context.Encounters.AsNoTracking(),
                            childDevelopment => childDevelopment.EncounterId,
                            encounter => encounter.Oid,
                            (childDevelopment, encounter) => new ChildsDevelopmentHistory
                            {
                                // Properties from ChildsDevelopmentHistory
                                InteractionId = childDevelopment.InteractionId,
                                FeedingHistory = childDevelopment.FeedingHistory,
                                SocialSmile = childDevelopment.SocialSmile,
                                HeadHolding = childDevelopment.HeadHolding,
                                TurnTowardSoundOrigin = childDevelopment.TurnTowardSoundOrigin,
                                GraspToy = childDevelopment.GraspToy,
                                FollowObjectsWithEyes = childDevelopment.FollowObjectsWithEyes,
                                RollsOver = childDevelopment.RollsOver,
                                Babbles = childDevelopment.Babbles,
                                TakesObjectsToMouth = childDevelopment.TakesObjectsToMouth,
                                RepeatsSyllables = childDevelopment.RepeatsSyllables,
                                MovesObjects = childDevelopment.MovesObjects,
                                PlaysPeekaBoo = childDevelopment.PlaysPeekaBoo,
                                RespondsToOwnName = childDevelopment.RespondsToOwnName,
                                TakesStepsWithSupport = childDevelopment.TakesStepsWithSupport,
                                PicksUpSmallObjects = childDevelopment.PicksUpSmallObjects,
                                ImitatesSimpleGestures = childDevelopment.ImitatesSimpleGestures,
                                SaysTwoToThreeWords = childDevelopment.SaysTwoToThreeWords,
                                Sitting = childDevelopment.Sitting,
                                Standing = childDevelopment.Standing,
                                Walking = childDevelopment.Walking,
                                Talking = childDevelopment.Talking,
                                DrinksFromCup = childDevelopment.DrinksFromCup,
                                SaysSevenToTenWords = childDevelopment.SaysSevenToTenWords,
                                PointsToBodyParts = childDevelopment.PointsToBodyParts,
                                StartsToRun = childDevelopment.StartsToRun,
                                PointsPictureOnRequest = childDevelopment.PointsPictureOnRequest,
                                Sings = childDevelopment.Sings,
                                BuildTowerWithBox = childDevelopment.BuildTowerWithBox,
                                JumpsAndRun = childDevelopment.JumpsAndRun,
                                BeginsToDressAndUndress = childDevelopment.BeginsToDressAndUndress,
                                GroupsSimilarObjects = childDevelopment.GroupsSimilarObjects,
                                PlaysWithOtherChildren = childDevelopment.PlaysWithOtherChildren,
                                SaysFirstNameAndShortStory = childDevelopment.SaysFirstNameAndShortStory,

                                // Properties from EncounterBaseModel
                                EncounterId = childDevelopment.EncounterId,
                                EncounterType = childDevelopment.EncounterType,
                                CreatedIn = childDevelopment.CreatedIn,
                                DateCreated = childDevelopment.DateCreated,
                                CreatedBy = childDevelopment.CreatedBy,
                                ModifiedIn = childDevelopment.ModifiedIn,
                                DateModified = childDevelopment.DateModified,
                                ModifiedBy = childDevelopment.ModifiedBy,
                                IsDeleted = childDevelopment.IsDeleted,
                                IsSynced = childDevelopment.IsSynced,
                                EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,

                                ClientId = childDevelopment.ClientId,
                                Client = childDevelopment.Client,
                                ClinicianName = context.UserAccounts.Where(x => x.Oid == childDevelopment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                                FacilityName = context.Facilities.Where(x => x.Oid == childDevelopment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            }
                        )
                        .OrderByDescending(x => x.EncounterDate)
                        .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Client.</param>
        /// <returns>Returns a Client if the key is matched.</returns>
        public async Task<IEnumerable<ChildsDevelopmentHistory>> GetChildsDevelopmentHistoryByClient(Guid ClientID, int page, int pageSize)
        {
            return await context.ChildsDevelopmentHistories.Include(c => c.Client).AsNoTracking().Where(x => x.IsDeleted == false && x.ClientId == ClientID)
                       .Join(
                           context.Encounters.AsNoTracking(),
                           childDevelopment => childDevelopment.EncounterId,
                           encounter => encounter.Oid,
                           (childDevelopment, encounter) => new ChildsDevelopmentHistory
                           {
                               // Properties from ChildsDevelopmentHistory
                               InteractionId = childDevelopment.InteractionId,
                               FeedingHistory = childDevelopment.FeedingHistory,
                               SocialSmile = childDevelopment.SocialSmile,
                               HeadHolding = childDevelopment.HeadHolding,
                               TurnTowardSoundOrigin = childDevelopment.TurnTowardSoundOrigin,
                               GraspToy = childDevelopment.GraspToy,
                               FollowObjectsWithEyes = childDevelopment.FollowObjectsWithEyes,
                               RollsOver = childDevelopment.RollsOver,
                               Babbles = childDevelopment.Babbles,
                               TakesObjectsToMouth = childDevelopment.TakesObjectsToMouth,
                               RepeatsSyllables = childDevelopment.RepeatsSyllables,
                               MovesObjects = childDevelopment.MovesObjects,
                               PlaysPeekaBoo = childDevelopment.PlaysPeekaBoo,
                               RespondsToOwnName = childDevelopment.RespondsToOwnName,
                               TakesStepsWithSupport = childDevelopment.TakesStepsWithSupport,
                               PicksUpSmallObjects = childDevelopment.PicksUpSmallObjects,
                               ImitatesSimpleGestures = childDevelopment.ImitatesSimpleGestures,
                               SaysTwoToThreeWords = childDevelopment.SaysTwoToThreeWords,
                               Sitting = childDevelopment.Sitting,
                               Standing = childDevelopment.Standing,
                               Walking = childDevelopment.Walking,
                               Talking = childDevelopment.Talking,
                               DrinksFromCup = childDevelopment.DrinksFromCup,
                               SaysSevenToTenWords = childDevelopment.SaysSevenToTenWords,
                               PointsToBodyParts = childDevelopment.PointsToBodyParts,
                               StartsToRun = childDevelopment.StartsToRun,
                               PointsPictureOnRequest = childDevelopment.PointsPictureOnRequest,
                               Sings = childDevelopment.Sings,
                               BuildTowerWithBox = childDevelopment.BuildTowerWithBox,
                               JumpsAndRun = childDevelopment.JumpsAndRun,
                               BeginsToDressAndUndress = childDevelopment.BeginsToDressAndUndress,
                               GroupsSimilarObjects = childDevelopment.GroupsSimilarObjects,
                               PlaysWithOtherChildren = childDevelopment.PlaysWithOtherChildren,
                               SaysFirstNameAndShortStory = childDevelopment.SaysFirstNameAndShortStory,

                               // Properties from EncounterBaseModel
                               EncounterId = childDevelopment.EncounterId,
                               EncounterType = childDevelopment.EncounterType,
                               CreatedIn = childDevelopment.CreatedIn,
                               DateCreated = childDevelopment.DateCreated,
                               CreatedBy = childDevelopment.CreatedBy,
                               ModifiedIn = childDevelopment.ModifiedIn,
                               DateModified = childDevelopment.DateModified,
                               ModifiedBy = childDevelopment.ModifiedBy,
                               IsDeleted = childDevelopment.IsDeleted,
                               IsSynced = childDevelopment.IsSynced,
                               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,

                               ClientId = childDevelopment.ClientId,
                               Client = childDevelopment.Client,
                               ClinicianName = context.UserAccounts.Where(x => x.Oid == childDevelopment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                               FacilityName = context.Facilities.Where(x => x.Oid == childDevelopment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                           }
                       )
                       .OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize)
                       .ToListAsync();
        }
        public async Task<IEnumerable<ChildsDevelopmentHistory>> GetChildsDevelopmentHistoryByClient(Guid ClientID)
        {

            return await context.ChildsDevelopmentHistories.AsNoTracking().Where(x => x.IsDeleted == false && x.ClientId == ClientID)
                       .Join(
                           context.Encounters.AsNoTracking(),
                           childDevelopment => childDevelopment.EncounterId,
                           encounter => encounter.Oid,
                           (childDevelopment, encounter) => new ChildsDevelopmentHistory
                           {
                               // Properties from ChildsDevelopmentHistory
                               InteractionId = childDevelopment.InteractionId,
                               FeedingHistory = childDevelopment.FeedingHistory,
                               SocialSmile = childDevelopment.SocialSmile,
                               HeadHolding = childDevelopment.HeadHolding,
                               TurnTowardSoundOrigin = childDevelopment.TurnTowardSoundOrigin,
                               GraspToy = childDevelopment.GraspToy,
                               FollowObjectsWithEyes = childDevelopment.FollowObjectsWithEyes,
                               RollsOver = childDevelopment.RollsOver,
                               Babbles = childDevelopment.Babbles,
                               TakesObjectsToMouth = childDevelopment.TakesObjectsToMouth,
                               RepeatsSyllables = childDevelopment.RepeatsSyllables,
                               MovesObjects = childDevelopment.MovesObjects,
                               PlaysPeekaBoo = childDevelopment.PlaysPeekaBoo,
                               RespondsToOwnName = childDevelopment.RespondsToOwnName,
                               TakesStepsWithSupport = childDevelopment.TakesStepsWithSupport,
                               PicksUpSmallObjects = childDevelopment.PicksUpSmallObjects,
                               ImitatesSimpleGestures = childDevelopment.ImitatesSimpleGestures,
                               SaysTwoToThreeWords = childDevelopment.SaysTwoToThreeWords,
                               Sitting = childDevelopment.Sitting,
                               Standing = childDevelopment.Standing,
                               Walking = childDevelopment.Walking,
                               Talking = childDevelopment.Talking,
                               DrinksFromCup = childDevelopment.DrinksFromCup,
                               SaysSevenToTenWords = childDevelopment.SaysSevenToTenWords,
                               PointsToBodyParts = childDevelopment.PointsToBodyParts,
                               StartsToRun = childDevelopment.StartsToRun,
                               PointsPictureOnRequest = childDevelopment.PointsPictureOnRequest,
                               Sings = childDevelopment.Sings,
                               BuildTowerWithBox = childDevelopment.BuildTowerWithBox,
                               JumpsAndRun = childDevelopment.JumpsAndRun,
                               BeginsToDressAndUndress = childDevelopment.BeginsToDressAndUndress,
                               GroupsSimilarObjects = childDevelopment.GroupsSimilarObjects,
                               PlaysWithOtherChildren = childDevelopment.PlaysWithOtherChildren,
                               SaysFirstNameAndShortStory = childDevelopment.SaysFirstNameAndShortStory,

                               // Properties from EncounterBaseModel
                               EncounterId = childDevelopment.EncounterId,
                               EncounterType = childDevelopment.EncounterType,
                               CreatedIn = childDevelopment.CreatedIn,
                               DateCreated = childDevelopment.DateCreated,
                               CreatedBy = childDevelopment.CreatedBy,
                               ModifiedIn = childDevelopment.ModifiedIn,
                               DateModified = childDevelopment.DateModified,
                               ModifiedBy = childDevelopment.ModifiedBy,
                               IsDeleted = childDevelopment.IsDeleted,
                               IsSynced = childDevelopment.IsSynced,
                               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,

                               ClientId = childDevelopment.ClientId,
                               Client = childDevelopment.Client,
                               ClinicianName = context.UserAccounts.Where(x => x.Oid == childDevelopment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                               FacilityName = context.Facilities.Where(x => x.Oid == childDevelopment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                           }
                       )
                       .OrderByDescending(x => x.EncounterDate)
                       .ToListAsync();
        }
        public async Task<IEnumerable<ChildsDevelopmentHistory>> GetChildsDevelopmentHistoryByClientLast24Hours(Guid ClientID)
        {
            DateTime Last24Hours = DateTime.Now.AddHours(-24);
 
            return await context.ChildsDevelopmentHistories.AsNoTracking().Where(x => x.IsDeleted == false && x.DateCreated >= Last24Hours && x.ClientId == ClientID)
                       .Join(
                           context.Encounters.AsNoTracking(),
                           childDevelopment => childDevelopment.EncounterId,
                           encounter => encounter.Oid,
                           (childDevelopment, encounter) => new ChildsDevelopmentHistory
                           {
                               // Properties from ChildsDevelopmentHistory
                               InteractionId = childDevelopment.InteractionId,
                               FeedingHistory = childDevelopment.FeedingHistory,
                               SocialSmile = childDevelopment.SocialSmile,
                               HeadHolding = childDevelopment.HeadHolding,
                               TurnTowardSoundOrigin = childDevelopment.TurnTowardSoundOrigin,
                               GraspToy = childDevelopment.GraspToy,
                               FollowObjectsWithEyes = childDevelopment.FollowObjectsWithEyes,
                               RollsOver = childDevelopment.RollsOver,
                               Babbles = childDevelopment.Babbles,
                               TakesObjectsToMouth = childDevelopment.TakesObjectsToMouth,
                               RepeatsSyllables = childDevelopment.RepeatsSyllables,
                               MovesObjects = childDevelopment.MovesObjects,
                               PlaysPeekaBoo = childDevelopment.PlaysPeekaBoo,
                               RespondsToOwnName = childDevelopment.RespondsToOwnName,
                               TakesStepsWithSupport = childDevelopment.TakesStepsWithSupport,
                               PicksUpSmallObjects = childDevelopment.PicksUpSmallObjects,
                               ImitatesSimpleGestures = childDevelopment.ImitatesSimpleGestures,
                               SaysTwoToThreeWords = childDevelopment.SaysTwoToThreeWords,
                               Sitting = childDevelopment.Sitting,
                               Standing = childDevelopment.Standing,
                               Walking = childDevelopment.Walking,
                               Talking = childDevelopment.Talking,
                               DrinksFromCup = childDevelopment.DrinksFromCup,
                               SaysSevenToTenWords = childDevelopment.SaysSevenToTenWords,
                               PointsToBodyParts = childDevelopment.PointsToBodyParts,
                               StartsToRun = childDevelopment.StartsToRun,
                               PointsPictureOnRequest = childDevelopment.PointsPictureOnRequest,
                               Sings = childDevelopment.Sings,
                               BuildTowerWithBox = childDevelopment.BuildTowerWithBox,
                               JumpsAndRun = childDevelopment.JumpsAndRun,
                               BeginsToDressAndUndress = childDevelopment.BeginsToDressAndUndress,
                               GroupsSimilarObjects = childDevelopment.GroupsSimilarObjects,
                               PlaysWithOtherChildren = childDevelopment.PlaysWithOtherChildren,
                               SaysFirstNameAndShortStory = childDevelopment.SaysFirstNameAndShortStory,

                               // Properties from EncounterBaseModel
                               EncounterId = childDevelopment.EncounterId,
                               EncounterType = childDevelopment.EncounterType,
                               CreatedIn = childDevelopment.CreatedIn,
                               DateCreated = childDevelopment.DateCreated,
                               CreatedBy = childDevelopment.CreatedBy,
                               ModifiedIn = childDevelopment.ModifiedIn,
                               DateModified = childDevelopment.DateModified,
                               ModifiedBy = childDevelopment.ModifiedBy,
                               IsDeleted = childDevelopment.IsDeleted,
                               IsSynced = childDevelopment.IsSynced,
                               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,

                               ClientId = childDevelopment.ClientId,
                               Client = childDevelopment.Client,
                               ClinicianName = context.UserAccounts.Where(x => x.Oid == childDevelopment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                               FacilityName = context.Facilities.Where(x => x.Oid == childDevelopment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                           }
                       )
                       .OrderByDescending(x => x.EncounterDate)
                       .ToListAsync();
        }
        public async Task<int> GetChildsDevelopmentHistoryByClientCount(Guid ClientID)
        {
            return await context.ChildsDevelopmentHistories.AsNoTracking().CountAsync(c => c.IsDeleted == false && c.ClientId == ClientID);
        }
    }
}