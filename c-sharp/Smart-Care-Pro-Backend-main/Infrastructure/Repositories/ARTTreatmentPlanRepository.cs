using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 10.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IAllergyRepository interface.
    /// </summary>
    public class ARTTreatmentPlanRepository : Repository<ARTTreatmentPlan>, IARTTreatmentPlanRepository
    {
        public ARTTreatmentPlanRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of ARTTreatmentPlan.
        /// </summary>
        /// <returns>Returns a list of all ARTTreatmentPlan.</returns>
        public async Task<IEnumerable<ARTTreatmentPlan>> GetARTTreatmentPlan()
        {
            try
            {
                return await context.ARTTreatmentPlans.AsNoTracking().Where(treatmentPlan => treatmentPlan.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        treatmentPlan => treatmentPlan.EncounterId,
                        encounter => encounter.Oid,
                        (treatmentPlan, encounter) => new ARTTreatmentPlan
                        {
                            InteractionId = treatmentPlan.InteractionId,
                            ArtPlan = treatmentPlan.ArtPlan,
                            TPTPlan = treatmentPlan.TPTPlan,
                            CTXPlan = treatmentPlan.CTXPlan,
                            EACPlan = treatmentPlan.EACPlan,
                            DSDPlan = treatmentPlan.DSDPlan,
                            FluconazolePlan = treatmentPlan.FluconazolePlan,
                            TPTEligibleToday = treatmentPlan.TPTEligibleToday,
                            HaveTPTProvidedToday = treatmentPlan.HaveTPTProvidedToday,
                            TPTNote = treatmentPlan.TPTNote,
                            ClientId = treatmentPlan.ClientId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            EncounterId = treatmentPlan.EncounterId,
                            EncounterType = treatmentPlan.EncounterType,
                            CreatedIn = treatmentPlan.CreatedIn,
                            DateCreated = treatmentPlan.DateCreated,
                            CreatedBy = treatmentPlan.CreatedBy,
                            ModifiedIn = treatmentPlan.ModifiedIn,
                            DateModified = treatmentPlan.DateModified,
                            ModifiedBy = treatmentPlan.ModifiedBy,
                            IsDeleted = treatmentPlan.IsDeleted,
                            IsSynced = treatmentPlan.IsSynced,
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


        /// <summary>
        /// The method is used to get an GetARTTreatmentPlan by key.
        /// </summary>
        /// <param name="key">Primary key of the table ARTTreatmentPlan.</param>
        /// <returns>Returns an ARTTreatmentPlan if the key is matched.</returns>
        public async Task<ARTTreatmentPlan> GetARTTreatmentPlanByKey(Guid key)
        {
            try
            {
                ARTTreatmentPlan aRTTreatmentPlan = await context.ARTTreatmentPlans.AsNoTracking()
                    .FirstOrDefaultAsync(treatmentPlan => treatmentPlan.InteractionId == key && treatmentPlan.IsDeleted == false);

                if (aRTTreatmentPlan is not null)
                {
                    aRTTreatmentPlan.EncounterDate = await context.Encounters.Where(x => x.Oid == aRTTreatmentPlan.EncounterId).AsNoTracking().Select(e => e.OPDVisitDate ?? e.IPDAdmissionDate ?? e.DateCreated).FirstOrDefaultAsync();
                    aRTTreatmentPlan.ClinicianName = await context.UserAccounts.Where(x => x.Oid == aRTTreatmentPlan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    aRTTreatmentPlan.FacilityName = await context.Facilities.Where(x => x.Oid == aRTTreatmentPlan.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                }

                return aRTTreatmentPlan;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get a TreatmentPlan by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a TreatmentPlan if the ClientID is matched.</returns>
        public async Task<IEnumerable<ARTTreatmentPlan>> GetARTTreatmentPlanByClient(Guid clientId)
        {
            try
            {
                return await context.ARTTreatmentPlans.AsNoTracking()
                    .Where(treatmentPlan => treatmentPlan.ClientId == clientId && treatmentPlan.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        treatmentPlan => treatmentPlan.EncounterId,
                        encounter => encounter.Oid,
                        (treatmentPlan, encounter) => new ARTTreatmentPlan
                        {
                            InteractionId = treatmentPlan.InteractionId,
                            ArtPlan = treatmentPlan.ArtPlan,
                            TPTPlan = treatmentPlan.TPTPlan,
                            CTXPlan = treatmentPlan.CTXPlan,
                            EACPlan = treatmentPlan.EACPlan,
                            DSDPlan = treatmentPlan.DSDPlan,
                            FluconazolePlan = treatmentPlan.FluconazolePlan,
                            TPTEligibleToday = treatmentPlan.TPTEligibleToday,
                            HaveTPTProvidedToday = treatmentPlan.HaveTPTProvidedToday,
                            TPTNote = treatmentPlan.TPTNote,
                            ClientId = treatmentPlan.ClientId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            EncounterId = treatmentPlan.EncounterId,
                            EncounterType = treatmentPlan.EncounterType,
                            CreatedIn = treatmentPlan.CreatedIn,
                            DateCreated = treatmentPlan.DateCreated,
                            CreatedBy = treatmentPlan.CreatedBy,
                            ModifiedIn = treatmentPlan.ModifiedIn,
                            DateModified = treatmentPlan.DateModified,
                            ModifiedBy = treatmentPlan.ModifiedBy,
                            IsDeleted = treatmentPlan.IsDeleted,
                            IsSynced = treatmentPlan.IsSynced,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == treatmentPlan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == treatmentPlan.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<ARTTreatmentPlan>> GetARTTreatmentPlanByClientLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.ARTTreatmentPlans.AsNoTracking()
                    .Where(treatmentPlan => treatmentPlan.ClientId == clientId && treatmentPlan.DateCreated >= Last24Hours && treatmentPlan.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        treatmentPlan => treatmentPlan.EncounterId,
                        encounter => encounter.Oid,
                        (treatmentPlan, encounter) => new ARTTreatmentPlan
                        {
                            InteractionId = treatmentPlan.InteractionId,
                            ArtPlan = treatmentPlan.ArtPlan,
                            TPTPlan = treatmentPlan.TPTPlan,
                            CTXPlan = treatmentPlan.CTXPlan,
                            EACPlan = treatmentPlan.EACPlan,
                            DSDPlan = treatmentPlan.DSDPlan,
                            FluconazolePlan = treatmentPlan.FluconazolePlan,
                            TPTEligibleToday = treatmentPlan.TPTEligibleToday,
                            HaveTPTProvidedToday = treatmentPlan.HaveTPTProvidedToday,
                            TPTNote = treatmentPlan.TPTNote,
                            ClientId = treatmentPlan.ClientId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            EncounterId = treatmentPlan.EncounterId,
                            EncounterType = treatmentPlan.EncounterType,
                            CreatedIn = treatmentPlan.CreatedIn,
                            DateCreated = treatmentPlan.DateCreated,
                            CreatedBy = treatmentPlan.CreatedBy,
                            ModifiedIn = treatmentPlan.ModifiedIn,
                            DateModified = treatmentPlan.DateModified,
                            ModifiedBy = treatmentPlan.ModifiedBy,
                            IsDeleted = treatmentPlan.IsDeleted,
                            IsSynced = treatmentPlan.IsSynced,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == treatmentPlan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == treatmentPlan.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ARTTreatmentPlan>> GetARTTreatmentPlanByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var aRTTreatmentPlanAsQuerable = context.ARTTreatmentPlans.AsNoTracking()
                    .Where(treatmentPlan => treatmentPlan.ClientId == clientId && treatmentPlan.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        treatmentPlan => treatmentPlan.EncounterId,
                        encounter => encounter.Oid,
                        (treatmentPlan, encounter) => new ARTTreatmentPlan
                        {
                            InteractionId = treatmentPlan.InteractionId,
                            ArtPlan = treatmentPlan.ArtPlan,
                            TPTPlan = treatmentPlan.TPTPlan,
                            CTXPlan = treatmentPlan.CTXPlan,
                            EACPlan = treatmentPlan.EACPlan,
                            DSDPlan = treatmentPlan.DSDPlan,
                            FluconazolePlan = treatmentPlan.FluconazolePlan,
                            TPTEligibleToday = treatmentPlan.TPTEligibleToday,
                            HaveTPTProvidedToday = treatmentPlan.HaveTPTProvidedToday,
                            TPTNote = treatmentPlan.TPTNote,
                            ClientId = treatmentPlan.ClientId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            EncounterId = treatmentPlan.EncounterId,
                            EncounterType = treatmentPlan.EncounterType,
                            CreatedIn = treatmentPlan.CreatedIn,
                            DateCreated = treatmentPlan.DateCreated,
                            CreatedBy = treatmentPlan.CreatedBy,
                            ModifiedIn = treatmentPlan.ModifiedIn,
                            DateModified = treatmentPlan.DateModified,
                            ModifiedBy = treatmentPlan.ModifiedBy,
                            IsDeleted = treatmentPlan.IsDeleted,
                            IsSynced = treatmentPlan.IsSynced,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == treatmentPlan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == treatmentPlan.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        })
                    .OrderByDescending(x => x.EncounterDate)
                    .AsQueryable();

                if (encounterType == null)
                    return await aRTTreatmentPlanAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await aRTTreatmentPlanAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetARTTreatmentPlanByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.ARTTreatmentPlans.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.ARTTreatmentPlans.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }

        /// <summary>
        /// The method is used to get a ARTTreatmentPlan by EncounterID.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a ARTTreatmentPlan if the EncounterID is matched.</returns>
        public async Task<IEnumerable<ARTTreatmentPlan>> GetARTTreatmentPlanByEncounter(Guid encounterId)
        {
            try
            {
                return await context.ARTTreatmentPlans.AsNoTracking()
                    .Where(treatmentPlan => treatmentPlan.IsDeleted == false && treatmentPlan.EncounterId == encounterId)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        treatmentPlan => treatmentPlan.EncounterId,
                        encounter => encounter.Oid,
                        (treatmentPlan, encounter) => new ARTTreatmentPlan
                        {
                            InteractionId = treatmentPlan.InteractionId,
                            ArtPlan = treatmentPlan.ArtPlan,
                            TPTPlan = treatmentPlan.TPTPlan,
                            CTXPlan = treatmentPlan.CTXPlan,
                            EACPlan = treatmentPlan.EACPlan,
                            DSDPlan = treatmentPlan.DSDPlan,
                            FluconazolePlan = treatmentPlan.FluconazolePlan,
                            TPTEligibleToday = treatmentPlan.TPTEligibleToday,
                            HaveTPTProvidedToday = treatmentPlan.HaveTPTProvidedToday,
                            TPTNote = treatmentPlan.TPTNote,
                            ClientId = treatmentPlan.ClientId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            EncounterId = treatmentPlan.EncounterId,
                            EncounterType = treatmentPlan.EncounterType,
                            CreatedIn = treatmentPlan.CreatedIn,
                            DateCreated = treatmentPlan.DateCreated,
                            CreatedBy = treatmentPlan.CreatedBy,
                            ModifiedIn = treatmentPlan.ModifiedIn,
                            DateModified = treatmentPlan.DateModified,
                            ModifiedBy = treatmentPlan.ModifiedBy,
                            IsDeleted = treatmentPlan.IsDeleted,
                            IsSynced = treatmentPlan.IsSynced,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == treatmentPlan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == treatmentPlan.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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