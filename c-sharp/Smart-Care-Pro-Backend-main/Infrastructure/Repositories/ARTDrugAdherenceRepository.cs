using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Lion
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IARTDrugAdherenceRepository interface.
    /// </summary>
    public class ARTDrugAdherenceRepository : Repository<ARTDrugAdherence>, IARTDrugAdherenceRepository
    {
        public ARTDrugAdherenceRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an ART drug adherence by key.
        /// </summary>
        /// <param name="key">Primary key of the table ARTDrugAdherences.</param>
        /// <returns>Returns an ART drug adherence if the key is matched.</returns>
        public async Task<ARTDrugAdherence> GetARTDrugAdherenceByKey(Guid key)
        {
            try
            {
                ARTDrugAdherence aRTDrugAdherence = await context.ARTDrugAdherences.AsNoTracking()
                    .FirstOrDefaultAsync(a => a.InteractionId == key && a.IsDeleted == false);


                if (aRTDrugAdherence != null)
                {
                    aRTDrugAdherence.ClinicianName = context.UserAccounts.Where(x => x.Oid == aRTDrugAdherence.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "";
                    aRTDrugAdherence.FacilityName = context.Facilities.Where(x => x.Oid == aRTDrugAdherence.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "";
                    aRTDrugAdherence.EncounterDate = await context.Encounters.Where(x => x.Oid == aRTDrugAdherence.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return aRTDrugAdherence;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ART drug adherences.
        /// </summary>
        /// <returns>Returns a list of all ART drug adherences.</returns>
        //public async Task<IEnumerable<ARTDrugAdherence>> GetARTDrugAdherences()
        //{
        //    try
        //    {
        //        return await QueryAsync(a => a.IsDeleted == false);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task<IEnumerable<ARTDrugAdherence>> GetARTDrugAdherences()
        {
            try
            {
                return await context.ARTDrugAdherences.AsNoTracking().Where(a => a.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        artDrugAdherence => artDrugAdherence.EncounterId,
                        encounter => encounter.Oid,
                        (artDrugAdherence, encounter) => new ARTDrugAdherence
                        {
                            InteractionId = artDrugAdherence.InteractionId,
                            HaveTroubleTakingPills = artDrugAdherence.HaveTroubleTakingPills,
                            HowManyDosesMissed = artDrugAdherence.HowManyDosesMissed,
                            ReducePharmacyVisitTo = artDrugAdherence.ReducePharmacyVisitTo,
                            ReferForAdherenceCounselling = artDrugAdherence.ReferForAdherenceCounselling,
                            Note = artDrugAdherence.Note,
                            Forgot = artDrugAdherence.Forgot,
                            Illness = artDrugAdherence.Illness,
                            SideEffect = artDrugAdherence.SideEffect,
                            MedicineFinished = artDrugAdherence.MedicineFinished,
                            AwayFromHome = artDrugAdherence.AwayFromHome,
                            ClinicRunOutOfMedication = artDrugAdherence.ClinicRunOutOfMedication,
                            OtherMissingReason = artDrugAdherence.OtherMissingReason,
                            Nausea = artDrugAdherence.Nausea,
                            Vomitting = artDrugAdherence.Vomitting,
                            YellowEyes = artDrugAdherence.YellowEyes,
                            MouthSores = artDrugAdherence.MouthSores,
                            Diarrhea = artDrugAdherence.Diarrhea,
                            Headache = artDrugAdherence.Headache,
                            Rash = artDrugAdherence.Rash,
                            Numbness = artDrugAdherence.Numbness,
                            Insomnia = artDrugAdherence.Insomnia,
                            Depression = artDrugAdherence.Depression,
                            WeightGain = artDrugAdherence.WeightGain,
                            OtherSideEffect = artDrugAdherence.OtherSideEffect,
                            ClientId = artDrugAdherence.ClientId,
                            Client = artDrugAdherence.Client,
                            DrugId = artDrugAdherence.DrugId,
                            SpecialDrug = artDrugAdherence.SpecialDrug,
                            ARTDrugAdherences = artDrugAdherence.ARTDrugAdherences,
                            // Add other properties as needed
                            EncounterId = encounter.Oid,
                            EncounterType = artDrugAdherence.EncounterType,
                            CreatedBy = artDrugAdherence.CreatedBy,
                            CreatedIn = artDrugAdherence.CreatedIn,
                            DateCreated = artDrugAdherence.DateCreated,
                            DateModified = artDrugAdherence.DateModified,
                            ModifiedBy = artDrugAdherence.ModifiedBy,
                            ModifiedIn = artDrugAdherence.ModifiedIn,
                            IsDeleted = artDrugAdherence.IsDeleted,
                            IsSynced = artDrugAdherence.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == artDrugAdherence.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == artDrugAdherence.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get an ART drug adherence by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns an ART drug adherence if the ClientID is matched.</returns>


        public async Task<IEnumerable<ARTDrugAdherence>> GetARTDrugAdherenceByClient(Guid clientId)
        {
            try
            {
                return await context.ARTDrugAdherences.AsNoTracking().Where(adherence => adherence.IsDeleted == false && adherence.ClientId == clientId)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        adherence => adherence.EncounterId,
                        encounter => encounter.Oid,
                        (adherence, encounter) => new ARTDrugAdherence
                        {
                            InteractionId = adherence.InteractionId,
                            HaveTroubleTakingPills = adherence.HaveTroubleTakingPills,
                            HowManyDosesMissed = adherence.HowManyDosesMissed,
                            ReducePharmacyVisitTo = adherence.ReducePharmacyVisitTo,
                            ReferForAdherenceCounselling = adherence.ReferForAdherenceCounselling,
                            Note = adherence.Note,
                            Forgot = adherence.Forgot,
                            Illness = adherence.Illness,
                            SideEffect = adherence.SideEffect,
                            MedicineFinished = adherence.MedicineFinished,
                            AwayFromHome = adherence.AwayFromHome,
                            ClinicRunOutOfMedication = adherence.ClinicRunOutOfMedication,
                            OtherMissingReason = adherence.OtherMissingReason,
                            Nausea = adherence.Nausea,
                            Vomitting = adherence.Vomitting,
                            YellowEyes = adherence.YellowEyes,
                            MouthSores = adherence.MouthSores,
                            Diarrhea = adherence.Diarrhea,
                            Headache = adherence.Headache,
                            Rash = adherence.Rash,
                            Numbness = adherence.Numbness,
                            Insomnia = adherence.Insomnia,
                            Depression = adherence.Depression,
                            WeightGain = adherence.WeightGain,
                            OtherSideEffect = adherence.OtherSideEffect,
                            ClientId = adherence.ClientId,
                            Client = adherence.Client,
                            DrugId = adherence.DrugId,
                            SpecialDrug = adherence.SpecialDrug,
                            EncounterId = encounter.Oid,
                            EncounterType = adherence.EncounterType,
                            CreatedBy = adherence.CreatedBy,
                            CreatedIn = adherence.CreatedIn,
                            DateCreated = adherence.DateCreated,
                            DateModified = adherence.DateModified,
                            ModifiedBy = adherence.ModifiedBy,
                            ModifiedIn = adherence.ModifiedIn,
                            IsDeleted = adherence.IsDeleted,
                            IsSynced = adherence.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == adherence.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == adherence.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            // Add other properties as needed
                        })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<ARTDrugAdherence>> GetARTDrugAdherenceByClientLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.ARTDrugAdherences.AsNoTracking().Where(adherence => adherence.IsDeleted == false && adherence.DateCreated >= Last24Hours && adherence.ClientId == clientId)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        adherence => adherence.EncounterId,
                        encounter => encounter.Oid,
                        (adherence, encounter) => new ARTDrugAdherence
                        {
                            InteractionId = adherence.InteractionId,
                            HaveTroubleTakingPills = adherence.HaveTroubleTakingPills,
                            HowManyDosesMissed = adherence.HowManyDosesMissed,
                            ReducePharmacyVisitTo = adherence.ReducePharmacyVisitTo,
                            ReferForAdherenceCounselling = adherence.ReferForAdherenceCounselling,
                            Note = adherence.Note,
                            Forgot = adherence.Forgot,
                            Illness = adherence.Illness,
                            SideEffect = adherence.SideEffect,
                            MedicineFinished = adherence.MedicineFinished,
                            AwayFromHome = adherence.AwayFromHome,
                            ClinicRunOutOfMedication = adherence.ClinicRunOutOfMedication,
                            OtherMissingReason = adherence.OtherMissingReason,
                            Nausea = adherence.Nausea,
                            Vomitting = adherence.Vomitting,
                            YellowEyes = adherence.YellowEyes,
                            MouthSores = adherence.MouthSores,
                            Diarrhea = adherence.Diarrhea,
                            Headache = adherence.Headache,
                            Rash = adherence.Rash,
                            Numbness = adherence.Numbness,
                            Insomnia = adherence.Insomnia,
                            Depression = adherence.Depression,
                            WeightGain = adherence.WeightGain,
                            OtherSideEffect = adherence.OtherSideEffect,
                            ClientId = adherence.ClientId,
                            Client = adherence.Client,
                            DrugId = adherence.DrugId,
                            SpecialDrug = adherence.SpecialDrug,
                            EncounterId = encounter.Oid,
                            EncounterType = adherence.EncounterType,
                            CreatedBy = adherence.CreatedBy,
                            CreatedIn = adherence.CreatedIn,
                            DateCreated = adherence.DateCreated,
                            DateModified = adherence.DateModified,
                            ModifiedBy = adherence.ModifiedBy,
                            ModifiedIn = adherence.ModifiedIn,
                            IsDeleted = adherence.IsDeleted,
                            IsSynced = adherence.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == adherence.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == adherence.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            // Add other properties as needed
                        })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<ARTDrugAdherence>> GetARTDrugAdherenceByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var aRTDrugAdherence = context.ARTDrugAdherences.Include(x => x.SpecialDrug).AsNoTracking().Where(adherence => adherence.IsDeleted == false && adherence.ClientId == clientId)
                       .Join(
                           context.Encounters.AsNoTracking(),
                           adherence => adherence.EncounterId,
                           encounter => encounter.Oid,
                           (adherence, encounter) => new ARTDrugAdherence
                           {
                               InteractionId = adherence.InteractionId,
                               HaveTroubleTakingPills = adherence.HaveTroubleTakingPills,
                               HowManyDosesMissed = adherence.HowManyDosesMissed,
                               ReducePharmacyVisitTo = adherence.ReducePharmacyVisitTo,
                               ReferForAdherenceCounselling = adherence.ReferForAdherenceCounselling,
                               Note = adherence.Note,
                               Forgot = adherence.Forgot,
                               Illness = adherence.Illness,
                               SideEffect = adherence.SideEffect,
                               MedicineFinished = adherence.MedicineFinished,
                               AwayFromHome = adherence.AwayFromHome,
                               ClinicRunOutOfMedication = adherence.ClinicRunOutOfMedication,
                               OtherMissingReason = adherence.OtherMissingReason,
                               Nausea = adherence.Nausea,
                               Vomitting = adherence.Vomitting,
                               YellowEyes = adherence.YellowEyes,
                               MouthSores = adherence.MouthSores,
                               Diarrhea = adherence.Diarrhea,
                               Headache = adherence.Headache,
                               Rash = adherence.Rash,
                               Numbness = adherence.Numbness,
                               Insomnia = adherence.Insomnia,
                               Depression = adherence.Depression,
                               WeightGain = adherence.WeightGain,
                               OtherSideEffect = adherence.OtherSideEffect,
                               ClientId = adherence.ClientId,
                               Client = adherence.Client,
                               DrugId = adherence.DrugId,
                               SpecialDrug = adherence.SpecialDrug,
                               EncounterId = encounter.Oid,
                               EncounterType = adherence.EncounterType,
                               CreatedBy = adherence.CreatedBy,
                               CreatedIn = adherence.CreatedIn,
                               DateCreated = adherence.DateCreated,
                               DateModified = adherence.DateModified,
                               ModifiedBy = adherence.ModifiedBy,
                               ModifiedIn = adherence.ModifiedIn,
                               IsDeleted = adherence.IsDeleted,
                               IsSynced = adherence.IsSynced,
                               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                               ClinicianName = context.UserAccounts.Where(x => x.Oid == adherence.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                               FacilityName = context.Facilities.Where(x => x.Oid == adherence.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                               // Add other properties as needed
                           })
                       .AsQueryable();
                if (encounterType == null)
                    return await aRTDrugAdherence.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await aRTDrugAdherence.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();


            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetARTDrugAdherenceByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.ARTDrugAdherences.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.ARTDrugAdherences.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }

        /// <summary>
        /// The method is used to get the list of ART drug adherence by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ART drug adherence by EncounterID.</returns>
        //public async Task<IEnumerable<ARTDrugAdherence>> GetARTDrugAdherenceByEncounter(Guid encounterId)
        //{
        //    try
        //    {
        //        return await QueryAsync(a => a.IsDeleted == false && a.EncounterId == encounterId);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task<IEnumerable<ARTDrugAdherence>> GetARTDrugAdherenceByEncounter(Guid encounterId)
        {
            try
            {
                return await context.ARTDrugAdherences.AsNoTracking().Where(adherence => adherence.IsDeleted == false && adherence.EncounterId == encounterId)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        adherence => adherence.EncounterId,
                        encounter => encounter.Oid,
                        (adherence, encounter) => new ARTDrugAdherence
                        {
                            InteractionId = adherence.InteractionId,
                            HaveTroubleTakingPills = adherence.HaveTroubleTakingPills,
                            HowManyDosesMissed = adherence.HowManyDosesMissed,
                            ReducePharmacyVisitTo = adherence.ReducePharmacyVisitTo,
                            ReferForAdherenceCounselling = adherence.ReferForAdherenceCounselling,
                            Note = adherence.Note,
                            Forgot = adherence.Forgot,
                            Illness = adherence.Illness,
                            SideEffect = adherence.SideEffect,
                            MedicineFinished = adherence.MedicineFinished,
                            AwayFromHome = adherence.AwayFromHome,
                            ClinicRunOutOfMedication = adherence.ClinicRunOutOfMedication,
                            OtherMissingReason = adherence.OtherMissingReason,
                            Nausea = adherence.Nausea,
                            Vomitting = adherence.Vomitting,
                            YellowEyes = adherence.YellowEyes,
                            MouthSores = adherence.MouthSores,
                            Diarrhea = adherence.Diarrhea,
                            Headache = adherence.Headache,
                            Rash = adherence.Rash,
                            Numbness = adherence.Numbness,
                            Insomnia = adherence.Insomnia,
                            Depression = adherence.Depression,
                            WeightGain = adherence.WeightGain,
                            OtherSideEffect = adherence.OtherSideEffect,
                            ClientId = adherence.ClientId,
                            Client = adherence.Client,
                            DrugId = adherence.DrugId,
                            SpecialDrug = adherence.SpecialDrug,
                            EncounterId = adherence.EncounterId,
                            EncounterType = adherence.EncounterType,
                            CreatedBy = adherence.CreatedBy,
                            CreatedIn = adherence.CreatedIn,
                            DateCreated = adherence.DateCreated,
                            DateModified = adherence.DateModified,
                            ModifiedBy = adherence.ModifiedBy,
                            ModifiedIn = adherence.ModifiedIn,
                            IsDeleted = adherence.IsDeleted,
                            IsSynced = adherence.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == adherence.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == adherence.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            // Add other properties as needed
                        })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}