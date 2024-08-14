using Domain.Entities;
using Infrastructure.Contracts;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IGynObsHistoryRepository interface.
    /// </summary>
    public class GynObsHistoryRepository : Repository<GynObsHistory>, IGynObsHistoryRepository
    {
        public GynObsHistoryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a gyn obs history by key.
        /// </summary>
        /// <param name="key">Primary key of the table GynObsHistories.</param>
        /// <returns>Returns a gyn obs history if the key is matched.</returns>
        public async Task<GynObsHistory> GetGynObsHistoryByKey(Guid key)
        {
            try
            {
                var gynObsHistory = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (gynObsHistory != null)
                {
                    gynObsHistory.ClinicianName = await context.UserAccounts.Where(x => x.Oid == gynObsHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    gynObsHistory.FacilityName = await context.Facilities.Where(x => x.Oid == gynObsHistory.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    gynObsHistory.EncounterDate = await context.Encounters.Where(x => x.Oid == gynObsHistory.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                }

                return gynObsHistory;

                // return await FirstOrDefaultAsync(g => g.InteractionId == key && g.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of gyn obs histories.
        /// </summary>
        /// <returns>Returns a list of all gyn obs histories.</returns>
        public async Task<IEnumerable<GynObsHistory>> GetGynObsHistories()
        {
            try
            {
                return await LoadListWithChildAsync<GynObsHistory>(g => g.IsDeleted == false, c => c.CaCxScreeningMethod,i=>i.InterCourseStatus);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a gyn obs encounterId by key.
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounter.</param>
        /// <returns>Returns a GynObsHistory if the encounterId is matched.</returns>
        public async Task<IEnumerable<GynObsHistory>> GetGynObsHistoryByEncounterId(Guid encounterId)
        {
            try
            {
                //      return await context.GynObsHistories.Include(x => x.CaCxScreeningMethod).Include(c => c.ContraceptiveHistories).Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
                //.Join(
                //    context.Encounters.AsNoTracking(),
                //    gynObsHistory => gynObsHistory.EncounterId,
                //    encounter => encounter.Oid,
                //    (gynObsHistory, encounter) => new GynObsHistory
                //    {
                //        EncounterId = gynObsHistory.EncounterId,
                //        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                //        BreastFeedingType = gynObsHistory.BreastFeedingType,
                //        BreastFeedingNote = gynObsHistory.BreastFeedingNote,
                //        BreastFeedingChoice = gynObsHistory.BreastFeedingChoice,
                //        AliveChildren = gynObsHistory.AliveChildren,
                //        ClientId = gynObsHistory.ClientId,
                //        CesareanHistory = gynObsHistory.CesareanHistory,
                //        CaCxScreeningMethodId = gynObsHistory.CaCxScreeningMethodId,
                //        CaCxScreeningMethod = gynObsHistory.CaCxScreeningMethod,
                //        ContraceptiveGiven = gynObsHistory.ContraceptiveGiven,
                //        CaCxResult = gynObsHistory.CaCxResult,
                //        CaCxLastScreened = gynObsHistory.CaCxLastScreened,
                //        CreatedBy = gynObsHistory.CreatedBy,
                //        CreatedIn = gynObsHistory.CreatedIn,
                //        CurrentFP = gynObsHistory.CurrentFP,
                //        DateCreated = gynObsHistory.DateCreated,
                //        DateModified = gynObsHistory.DateModified,
                //        DateOfDelivery = gynObsHistory.DateOfDelivery,
                //        GestationalAge = gynObsHistory.GestationalAge,
                //        EDD = gynObsHistory.EDD,
                //        EncounterType = gynObsHistory.EncounterType,
                //        InteractionId = gynObsHistory.InteractionId,
                //        HaveTreatedWithBenzathinePenicillin = gynObsHistory.HaveTreatedWithBenzathinePenicillin,
                //        HasCounselled = gynObsHistory.HasCounselled,
                //        IsBreastFeeding = gynObsHistory.IsBreastFeeding,
                //        IsCaCxScreened = gynObsHistory.IsCaCxScreened,
                //        IsChildTestedForHIV = gynObsHistory.IsChildTestedForHIV,
                //        IsClientNeedFP = gynObsHistory.IsClientNeedFP,
                //        IsCientOnFP = gynObsHistory.IsCientOnFP,
                //        IsDeleted = gynObsHistory.IsDeleted,
                //        IsPregnant = gynObsHistory.IsPregnant,
                //        IsSynced = gynObsHistory.IsSynced,
                //        LNMP = gynObsHistory.LNMP,
                //        IsScreenedForSyphilis = gynObsHistory.IsScreenedForSyphilis,
                //        LastDeliveryTime = gynObsHistory.LastDeliveryTime,
                //        MenstrualHistory = gynObsHistory.MenstrualHistory,
                //        MiscarriageStatus = gynObsHistory.MiscarriageStatus,
                //        ModifiedBy = gynObsHistory.ModifiedBy,
                //        MiscarriageWithinFourWeeks = gynObsHistory.MiscarriageWithinFourWeeks,
                //        ModifiedIn = gynObsHistory.ModifiedIn,
                //        ObstetricsHistoryNote = gynObsHistory.ObstetricsHistoryNote,
                //        PostAbortionSepsis = gynObsHistory.PostAbortionSepsis,
                //        Postpartum = gynObsHistory.Postpartum,
                //        PreviouslyGotPregnant = gynObsHistory.PreviouslyGotPregnant,
                //        PreviousSexualHistory = gynObsHistory.PreviousSexualHistory,
                //        RecentClientGivenBirth = gynObsHistory.RecentClientGivenBirth,
                //        TotalBirthGiven = gynObsHistory.TotalBirthGiven,
                //        TotalNumberOfPregnancy = gynObsHistory.TotalNumberOfPregnancy,
                //        ContraceptiveHistories = gynObsHistory.ContraceptiveHistories.ToList()

                //    }).OrderByDescending(x => x.EncounterDate).ToListAsync();

                return await QueryAsync(c => c.IsDeleted == false && c.EncounterId == encounterId);

                //return await LoadListWithChildAsync<GynObsHistory>(g => g.IsDeleted == false && g.EncounterId == encounterId, c => c.CaCxScreeningMethod, co => co.ContraceptiveHistories);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a GynObsHistory by clientID.
        /// </summary>
        /// <param name="clientId">Primary key of the table Clients.</param>
        /// <returns>Returns a GynObsHistory if the clientID is matched.</returns>
        public async Task<IEnumerable<GynObsHistory>> GetGynObsHistoryByClientIDLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                var data = await context.GynObsHistories.Include(i=>i.InterCourseStatus).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId && p.DateCreated >= Last24Hours).AsNoTracking()
   .Join(
       context.Encounters.AsNoTracking(),
       gynObsHistory => gynObsHistory.EncounterId,
       encounter => encounter.Oid,
       (gynObsHistory, encounter) => new GynObsHistory
       {
           EncounterId = gynObsHistory.EncounterId,
           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
           BreastFeedingType = gynObsHistory.BreastFeedingType,
           BreastFeedingNote = gynObsHistory.BreastFeedingNote,
           BreastFeedingChoice = gynObsHistory.BreastFeedingChoice,
           AliveChildren = gynObsHistory.AliveChildren,
           ClientId = gynObsHistory.ClientId,
           CesareanHistory = gynObsHistory.CesareanHistory,
           CaCxScreeningMethodId = gynObsHistory.CaCxScreeningMethodId,
           ContraceptiveGiven = gynObsHistory.ContraceptiveGiven,
           CaCxResult = gynObsHistory.CaCxResult,
           CaCxLastScreened = gynObsHistory.CaCxLastScreened,
           CreatedBy = gynObsHistory.CreatedBy,
           CreatedIn = gynObsHistory.CreatedIn,
           CurrentFP = gynObsHistory.CurrentFP,
           DateCreated = gynObsHistory.DateCreated,
           DateModified = gynObsHistory.DateModified,
           DateOfDelivery = gynObsHistory.DateOfDelivery,
           GestationalAge = gynObsHistory.GestationalAge,
           EDD = gynObsHistory.EDD,
           EncounterType = gynObsHistory.EncounterType,
           InteractionId = gynObsHistory.InteractionId,
           HaveTreatedWithBenzathinePenicillin = gynObsHistory.HaveTreatedWithBenzathinePenicillin,
           HasCounselled = gynObsHistory.HasCounselled,
           IsBreastFeeding = gynObsHistory.IsBreastFeeding,
           IsCaCxScreened = gynObsHistory.IsCaCxScreened,
           IsChildTestedForHIV = gynObsHistory.IsChildTestedForHIV,
           IsClientNeedFP = gynObsHistory.IsClientNeedFP,
           IsClientOnFP = gynObsHistory.IsClientOnFP,
           IsDeleted = gynObsHistory.IsDeleted,
           IsPregnant = gynObsHistory.IsPregnant,
           IsSynced = gynObsHistory.IsSynced,
           LNMP = gynObsHistory.LNMP,
           IsScreenedForSyphilis = gynObsHistory.IsScreenedForSyphilis,
           LastDeliveryTime = gynObsHistory.LastDeliveryTime,
           MenstrualHistory = gynObsHistory.MenstrualHistory,
           MiscarriageStatus = gynObsHistory.MiscarriageStatus,
           ModifiedBy = gynObsHistory.ModifiedBy,
           MiscarriageWithinFourWeeks = gynObsHistory.MiscarriageWithinFourWeeks,
           ModifiedIn = gynObsHistory.ModifiedIn,
           ObstetricsHistoryNote = gynObsHistory.ObstetricsHistoryNote,
           PostAbortionSepsis = gynObsHistory.PostAbortionSepsis,
           Postpartum = gynObsHistory.Postpartum,
           PreviouslyGotPregnant = gynObsHistory.PreviouslyGotPregnant,
           PreviousSexualHistory = gynObsHistory.PreviousSexualHistory,
           RecentClientGivenBirth = gynObsHistory.RecentClientGivenBirth,
           TotalBirthGiven = gynObsHistory.TotalBirthGiven,
           TotalNumberOfPregnancy = gynObsHistory.TotalNumberOfPregnancy,
           ClinicianName = context.UserAccounts.Where(x => x.Oid == gynObsHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
           FacilityName = context.Facilities.Where(x => x.Oid == gynObsHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
           IntercourseStatusId=gynObsHistory.IntercourseStatusId,
           InterCourseStatus=gynObsHistory.InterCourseStatus,
           FirstPregnancyAge=gynObsHistory.FirstPregnancyAge,
           AgeAtMenarche=gynObsHistory.AgeAtMenarche,
           Examination=gynObsHistory.Examination,
           FirstSexualIntercourseAge=gynObsHistory.FirstSexualIntercourseAge,
           HasAbnormalVaginalDischarge = gynObsHistory.HasAbnormalVaginalDischarge,
           HasFever= gynObsHistory.HasFever,
           HasLowerAbdominalPain= gynObsHistory.HasLowerAbdominalPain,
           IsAnythingUsedToCleanVagina= gynObsHistory.IsAnythingUsedToCleanVagina,
           IsBleedingDuringOrAfterCoitus= gynObsHistory.IsBleedingDuringOrAfterCoitus,
           IsMensAssociatedWithPain= gynObsHistory.IsMensAssociatedWithPain,
           ItemUsedToCleanVagina= gynObsHistory.ItemUsedToCleanVagina,
           MenstrualBloodFlow = gynObsHistory.MenstrualBloodFlow,
           MenstrualCycleRegularity= gynObsHistory.MenstrualCycleRegularity,
           NumberOfSexualPartners= gynObsHistory.NumberOfSexualPartners,
           OtherConcern= gynObsHistory.OtherConcern,

       }).ToListAsync();

                foreach (var item in data)
                {
                    item.CaCxScreeningMethod = context.CaCxScreeningMethods.Where(x => x.Oid == item.CaCxScreeningMethodId && x.IsDeleted == false).AsNoTracking().FirstOrDefault();
                    item.ContraceptiveHistories = context.ContraceptiveHistories.Where(x => x.GynObsHistoryId == item.InteractionId && x.IsDeleted == false).AsNoTracking().ToList();
                    if (item.ContraceptiveHistories != null)
                    {
                        foreach (var itemContraceptive in item.ContraceptiveHistories)
                        {
                            itemContraceptive.Contraceptive = context.Contraceptives.Where(x => x.Oid == itemContraceptive.ContraceptiveId).AsNoTracking().FirstOrDefault();
                        }
                    }
                }



                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<GynObsHistory>> GetGynObsHistoryByClientID(Guid clientId)
        {
            try
            {
                var data = await context.GynObsHistories.Include(i => i.InterCourseStatus).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
   .Join(
       context.Encounters.AsNoTracking(),
       gynObsHistory => gynObsHistory.EncounterId,
       encounter => encounter.Oid,
       (gynObsHistory, encounter) => new GynObsHistory
       {
           EncounterId = gynObsHistory.EncounterId,
           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
           BreastFeedingType = gynObsHistory.BreastFeedingType,
           BreastFeedingNote = gynObsHistory.BreastFeedingNote,
           BreastFeedingChoice = gynObsHistory.BreastFeedingChoice,
           AliveChildren = gynObsHistory.AliveChildren,
           ClientId = gynObsHistory.ClientId,
           CesareanHistory = gynObsHistory.CesareanHistory,
           CaCxScreeningMethodId = gynObsHistory.CaCxScreeningMethodId,
           ContraceptiveGiven = gynObsHistory.ContraceptiveGiven,
           CaCxResult = gynObsHistory.CaCxResult,
           CaCxLastScreened = gynObsHistory.CaCxLastScreened,
           CreatedBy = gynObsHistory.CreatedBy,
           CreatedIn = gynObsHistory.CreatedIn,
           CurrentFP = gynObsHistory.CurrentFP,
           DateCreated = gynObsHistory.DateCreated,
           DateModified = gynObsHistory.DateModified,
           DateOfDelivery = gynObsHistory.DateOfDelivery,
           GestationalAge = gynObsHistory.GestationalAge,
           EDD = gynObsHistory.EDD,
           EncounterType = gynObsHistory.EncounterType,
           InteractionId = gynObsHistory.InteractionId,
           HaveTreatedWithBenzathinePenicillin = gynObsHistory.HaveTreatedWithBenzathinePenicillin,
           HasCounselled = gynObsHistory.HasCounselled,
           IsBreastFeeding = gynObsHistory.IsBreastFeeding,
           IsCaCxScreened = gynObsHistory.IsCaCxScreened,
           IsChildTestedForHIV = gynObsHistory.IsChildTestedForHIV,
           IsClientNeedFP = gynObsHistory.IsClientNeedFP,
           IsClientOnFP = gynObsHistory.IsClientOnFP,
           IsDeleted = gynObsHistory.IsDeleted,
           IsPregnant = gynObsHistory.IsPregnant,
           IsSynced = gynObsHistory.IsSynced,
           LNMP = gynObsHistory.LNMP,
           IsScreenedForSyphilis = gynObsHistory.IsScreenedForSyphilis,
           LastDeliveryTime = gynObsHistory.LastDeliveryTime,
           MenstrualHistory = gynObsHistory.MenstrualHistory,
           MiscarriageStatus = gynObsHistory.MiscarriageStatus,
           ModifiedBy = gynObsHistory.ModifiedBy,
           MiscarriageWithinFourWeeks = gynObsHistory.MiscarriageWithinFourWeeks,
           ModifiedIn = gynObsHistory.ModifiedIn,
           ObstetricsHistoryNote = gynObsHistory.ObstetricsHistoryNote,
           PostAbortionSepsis = gynObsHistory.PostAbortionSepsis,
           Postpartum = gynObsHistory.Postpartum,
           PreviouslyGotPregnant = gynObsHistory.PreviouslyGotPregnant,
           PreviousSexualHistory = gynObsHistory.PreviousSexualHistory,
           RecentClientGivenBirth = gynObsHistory.RecentClientGivenBirth,
           TotalBirthGiven = gynObsHistory.TotalBirthGiven,
           TotalNumberOfPregnancy = gynObsHistory.TotalNumberOfPregnancy,
           ClinicianName = context.UserAccounts.Where(x => x.Oid == gynObsHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
           FacilityName = context.Facilities.Where(x => x.Oid == gynObsHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
           IntercourseStatusId = gynObsHistory.IntercourseStatusId,
           InterCourseStatus = gynObsHistory.InterCourseStatus,
           FirstPregnancyAge = gynObsHistory.FirstPregnancyAge,
           AgeAtMenarche = gynObsHistory.AgeAtMenarche,
           Examination = gynObsHistory.Examination,
           FirstSexualIntercourseAge = gynObsHistory.FirstSexualIntercourseAge,
           HasAbnormalVaginalDischarge = gynObsHistory.HasAbnormalVaginalDischarge,
           HasFever = gynObsHistory.HasFever,
           HasLowerAbdominalPain = gynObsHistory.HasLowerAbdominalPain,
           IsAnythingUsedToCleanVagina = gynObsHistory.IsAnythingUsedToCleanVagina,
           IsBleedingDuringOrAfterCoitus = gynObsHistory.IsBleedingDuringOrAfterCoitus,
           IsMensAssociatedWithPain = gynObsHistory.IsMensAssociatedWithPain,
           ItemUsedToCleanVagina = gynObsHistory.ItemUsedToCleanVagina,
           MenstrualBloodFlow = gynObsHistory.MenstrualBloodFlow,
           MenstrualCycleRegularity = gynObsHistory.MenstrualCycleRegularity,
           NumberOfSexualPartners = gynObsHistory.NumberOfSexualPartners,
           OtherConcern = gynObsHistory.OtherConcern,
       }).ToListAsync();

                foreach (var item in data)
                {
                    item.CaCxScreeningMethod = context.CaCxScreeningMethods.Where(x => x.Oid == item.CaCxScreeningMethodId && x.IsDeleted == false).AsNoTracking().FirstOrDefault();
                    item.ContraceptiveHistories = context.ContraceptiveHistories.Where(x => x.GynObsHistoryId == item.InteractionId && x.IsDeleted == false).AsNoTracking().ToList();
                    if (item.ContraceptiveHistories != null)
                    {
                        foreach (var itemContraceptive in item.ContraceptiveHistories)
                        {
                            itemContraceptive.Contraceptive = context.Contraceptives.Where(x => x.Oid == itemContraceptive.ContraceptiveId).AsNoTracking().FirstOrDefault();
                        }
                    }
                }



                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<GynObsHistory>> GetGynObsHistoryByClientID(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var gynObsHistoryAsQuerable = context.GynObsHistories.Include(i => i.InterCourseStatus).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
       .Join(
           context.Encounters.AsNoTracking(),
           gynObsHistory => gynObsHistory.EncounterId,
           encounter => encounter.Oid,
           (gynObsHistory, encounter) => new GynObsHistory
           {
               EncounterId = gynObsHistory.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               BreastFeedingType = gynObsHistory.BreastFeedingType,
               BreastFeedingNote = gynObsHistory.BreastFeedingNote,
               BreastFeedingChoice = gynObsHistory.BreastFeedingChoice,
               AliveChildren = gynObsHistory.AliveChildren,
               ClientId = gynObsHistory.ClientId,
               CesareanHistory = gynObsHistory.CesareanHistory,
               CaCxScreeningMethodId = gynObsHistory.CaCxScreeningMethodId,
               ContraceptiveGiven = gynObsHistory.ContraceptiveGiven,
               CaCxResult = gynObsHistory.CaCxResult,
               CaCxLastScreened = gynObsHistory.CaCxLastScreened,
               CreatedBy = gynObsHistory.CreatedBy,
               CreatedIn = gynObsHistory.CreatedIn,
               CurrentFP = gynObsHistory.CurrentFP,
               DateCreated = gynObsHistory.DateCreated,
               DateModified = gynObsHistory.DateModified,
               DateOfDelivery = gynObsHistory.DateOfDelivery,
               GestationalAge = gynObsHistory.GestationalAge,
               EDD = gynObsHistory.EDD,
               EncounterType = gynObsHistory.EncounterType,
               InteractionId = gynObsHistory.InteractionId,
               HaveTreatedWithBenzathinePenicillin = gynObsHistory.HaveTreatedWithBenzathinePenicillin,
               HasCounselled = gynObsHistory.HasCounselled,
               IsBreastFeeding = gynObsHistory.IsBreastFeeding,
               IsCaCxScreened = gynObsHistory.IsCaCxScreened,
               IsChildTestedForHIV = gynObsHistory.IsChildTestedForHIV,
               IsClientNeedFP = gynObsHistory.IsClientNeedFP,
               IsClientOnFP = gynObsHistory.IsClientOnFP,
               IsDeleted = gynObsHistory.IsDeleted,
               IsPregnant = gynObsHistory.IsPregnant,
               IsSynced = gynObsHistory.IsSynced,
               LNMP = gynObsHistory.LNMP,
               IsScreenedForSyphilis = gynObsHistory.IsScreenedForSyphilis,
               LastDeliveryTime = gynObsHistory.LastDeliveryTime,
               MenstrualHistory = gynObsHistory.MenstrualHistory,
               MiscarriageStatus = gynObsHistory.MiscarriageStatus,
               ModifiedBy = gynObsHistory.ModifiedBy,
               MiscarriageWithinFourWeeks = gynObsHistory.MiscarriageWithinFourWeeks,
               ModifiedIn = gynObsHistory.ModifiedIn,
               ObstetricsHistoryNote = gynObsHistory.ObstetricsHistoryNote,
               PostAbortionSepsis = gynObsHistory.PostAbortionSepsis,
               Postpartum = gynObsHistory.Postpartum,
               PreviouslyGotPregnant = gynObsHistory.PreviouslyGotPregnant,
               PreviousSexualHistory = gynObsHistory.PreviousSexualHistory,
               RecentClientGivenBirth = gynObsHistory.RecentClientGivenBirth,
               TotalBirthGiven = gynObsHistory.TotalBirthGiven,
               TotalNumberOfPregnancy = gynObsHistory.TotalNumberOfPregnancy,
               ClinicianName = context.UserAccounts.Where(x => x.Oid == gynObsHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
               FacilityName = context.Facilities.Where(x => x.Oid == gynObsHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
               IntercourseStatusId = gynObsHistory.IntercourseStatusId,
               InterCourseStatus = gynObsHistory.InterCourseStatus,
               FirstPregnancyAge = gynObsHistory.FirstPregnancyAge,
               AgeAtMenarche = gynObsHistory.AgeAtMenarche,
               Examination = gynObsHistory.Examination,
               FirstSexualIntercourseAge = gynObsHistory.FirstSexualIntercourseAge,
               HasAbnormalVaginalDischarge = gynObsHistory.HasAbnormalVaginalDischarge,
               HasFever = gynObsHistory.HasFever,
               HasLowerAbdominalPain = gynObsHistory.HasLowerAbdominalPain,
               IsAnythingUsedToCleanVagina = gynObsHistory.IsAnythingUsedToCleanVagina,
               IsBleedingDuringOrAfterCoitus = gynObsHistory.IsBleedingDuringOrAfterCoitus,
               IsMensAssociatedWithPain = gynObsHistory.IsMensAssociatedWithPain,
               ItemUsedToCleanVagina = gynObsHistory.ItemUsedToCleanVagina,
               MenstrualBloodFlow = gynObsHistory.MenstrualBloodFlow,
               MenstrualCycleRegularity = gynObsHistory.MenstrualCycleRegularity,
               NumberOfSexualPartners = gynObsHistory.NumberOfSexualPartners,
               OtherConcern = gynObsHistory.OtherConcern,
           }).AsQueryable();

                if (encounterType == null)
                {
                    var data = await gynObsHistoryAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

                    foreach (var item in data)
                    {
                        item.CaCxScreeningMethod = context.CaCxScreeningMethods.Where(x => x.Oid == item.CaCxScreeningMethodId && x.IsDeleted == false).AsNoTracking().FirstOrDefault();
                        item.ContraceptiveHistories = context.ContraceptiveHistories.Where(x => x.GynObsHistoryId == item.InteractionId && x.IsDeleted == false).AsNoTracking().ToList();
                        if (item.ContraceptiveHistories != null)
                        {
                            foreach (var itemContraceptive in item.ContraceptiveHistories)
                            {
                                itemContraceptive.Contraceptive = context.Contraceptives.Where(x => x.Oid == itemContraceptive.ContraceptiveId).AsNoTracking().FirstOrDefault();
                            }
                        }
                    }
                    return data;
                }
                else
                {
                    var data = await gynObsHistoryAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

                    foreach (var item in data)
                    {
                        item.CaCxScreeningMethod = context.CaCxScreeningMethods.Where(x => x.Oid == item.CaCxScreeningMethodId && x.IsDeleted == false).AsNoTracking().FirstOrDefault();
                        item.ContraceptiveHistories = context.ContraceptiveHistories.Where(x => x.GynObsHistoryId == item.InteractionId && x.IsDeleted == false).AsNoTracking().ToList();
                        if (item.ContraceptiveHistories != null)
                        {
                            foreach (var itemContraceptive in item.ContraceptiveHistories)
                            {
                                itemContraceptive.Contraceptive = context.Contraceptives.Where(x => x.Oid == itemContraceptive.ContraceptiveId).AsNoTracking().FirstOrDefault();
                            }
                        }
                    }

                    return data;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetGynObsHistoryByClientIDTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.GynObsHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.GynObsHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }

        /// <summary>
        /// The method is used to get a GynObsHistory by clientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a latest GynObsHistory if the clientID is matched.</returns>
        public async Task<GynObsHistory> GetLatestGynObsHistoryByClientID(Guid clientId)
        {
            try
            {
                return await context.GynObsHistories.Include(x => x.CaCxScreeningMethod).Include(i => i.InterCourseStatus).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
          .Join(
              context.Encounters.AsNoTracking(),
              gynObsHistory => gynObsHistory.EncounterId,
              encounter => encounter.Oid,
              (gynObsHistory, encounter) => new GynObsHistory
              {
                  EncounterId = gynObsHistory.EncounterId,
                  EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                  BreastFeedingType = gynObsHistory.BreastFeedingType,
                  BreastFeedingNote = gynObsHistory.BreastFeedingNote,
                  BreastFeedingChoice = gynObsHistory.BreastFeedingChoice,
                  AliveChildren = gynObsHistory.AliveChildren,
                  ClientId = gynObsHistory.ClientId,
                  CesareanHistory = gynObsHistory.CesareanHistory,
                  CaCxScreeningMethodId = gynObsHistory.CaCxScreeningMethodId,
                  CaCxScreeningMethod = gynObsHistory.CaCxScreeningMethod,
                  ContraceptiveGiven = gynObsHistory.ContraceptiveGiven,
                  CaCxResult = gynObsHistory.CaCxResult,
                  CaCxLastScreened = gynObsHistory.CaCxLastScreened,
                  CreatedBy = gynObsHistory.CreatedBy,
                  CreatedIn = gynObsHistory.CreatedIn,
                  CurrentFP = gynObsHistory.CurrentFP,
                  DateCreated = gynObsHistory.DateCreated,
                  DateModified = gynObsHistory.DateModified,
                  DateOfDelivery = gynObsHistory.DateOfDelivery,
                  GestationalAge = gynObsHistory.GestationalAge,
                  EDD = gynObsHistory.EDD,
                  EncounterType = gynObsHistory.EncounterType,
                  InteractionId = gynObsHistory.InteractionId,
                  HaveTreatedWithBenzathinePenicillin = gynObsHistory.HaveTreatedWithBenzathinePenicillin,
                  HasCounselled = gynObsHistory.HasCounselled,
                  IsBreastFeeding = gynObsHistory.IsBreastFeeding,
                  IsCaCxScreened = gynObsHistory.IsCaCxScreened,
                  IsChildTestedForHIV = gynObsHistory.IsChildTestedForHIV,
                  IsClientNeedFP = gynObsHistory.IsClientNeedFP,
                  IsClientOnFP = gynObsHistory.IsClientOnFP,
                  IsDeleted = gynObsHistory.IsDeleted,
                  IsPregnant = gynObsHistory.IsPregnant,
                  IsSynced = gynObsHistory.IsSynced,
                  LNMP = gynObsHistory.LNMP,
                  IsScreenedForSyphilis = gynObsHistory.IsScreenedForSyphilis,
                  LastDeliveryTime = gynObsHistory.LastDeliveryTime,
                  MenstrualHistory = gynObsHistory.MenstrualHistory,
                  MiscarriageStatus = gynObsHistory.MiscarriageStatus,
                  ModifiedBy = gynObsHistory.ModifiedBy,
                  MiscarriageWithinFourWeeks = gynObsHistory.MiscarriageWithinFourWeeks,
                  ModifiedIn = gynObsHistory.ModifiedIn,
                  ObstetricsHistoryNote = gynObsHistory.ObstetricsHistoryNote,
                  PostAbortionSepsis = gynObsHistory.PostAbortionSepsis,
                  Postpartum = gynObsHistory.Postpartum,
                  PreviouslyGotPregnant = gynObsHistory.PreviouslyGotPregnant,
                  PreviousSexualHistory = gynObsHistory.PreviousSexualHistory,
                  RecentClientGivenBirth = gynObsHistory.RecentClientGivenBirth,
                  TotalBirthGiven = gynObsHistory.TotalBirthGiven,
                  TotalNumberOfPregnancy = gynObsHistory.TotalNumberOfPregnancy,
                  ClinicianName = context.UserAccounts.Where(x => x.Oid == gynObsHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                  FacilityName = context.Facilities.Where(x => x.Oid == gynObsHistory.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                  IntercourseStatusId = gynObsHistory.IntercourseStatusId,
                  InterCourseStatus = gynObsHistory.InterCourseStatus,
                  FirstPregnancyAge = gynObsHistory.FirstPregnancyAge,
                  AgeAtMenarche = gynObsHistory.AgeAtMenarche,
                  Examination = gynObsHistory.Examination,
                  FirstSexualIntercourseAge = gynObsHistory.FirstSexualIntercourseAge,
                  HasAbnormalVaginalDischarge = gynObsHistory.HasAbnormalVaginalDischarge,
                  HasFever = gynObsHistory.HasFever,
                  HasLowerAbdominalPain = gynObsHistory.HasLowerAbdominalPain,
                  IsAnythingUsedToCleanVagina = gynObsHistory.IsAnythingUsedToCleanVagina,
                  IsBleedingDuringOrAfterCoitus = gynObsHistory.IsBleedingDuringOrAfterCoitus,
                  IsMensAssociatedWithPain = gynObsHistory.IsMensAssociatedWithPain,
                  ItemUsedToCleanVagina = gynObsHistory.ItemUsedToCleanVagina,
                  MenstrualBloodFlow = gynObsHistory.MenstrualBloodFlow,
                  MenstrualCycleRegularity = gynObsHistory.MenstrualCycleRegularity,
                  NumberOfSexualPartners = gynObsHistory.NumberOfSexualPartners,
                  OtherConcern = gynObsHistory.OtherConcern,
              }).OrderByDescending(x => x.EncounterDate).FirstOrDefaultAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}