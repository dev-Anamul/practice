using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  : Bella  
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class FamilyPlanRepository : Repository<FamilyPlan>, IFamilyPlanRepository
    {
        /// <summary>
        /// Implementation of IFamilyPlanRepository interface.
        /// </summary>
        public FamilyPlanRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a FamilyPlan by key.
        /// </summary>
        /// <param name="key">Primary key of the table FamilyPlans.</param>
        /// <returns>Returns a FamilyPlan if the key is matched.</returns>
        public async Task<FamilyPlan> GetFamilyPlanByKey(Guid key)
        {
            try
            {
                var familyPlan = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (familyPlan != null)
                {
                    familyPlan.ClinicianName = await context.UserAccounts.Where(x => x.Oid == familyPlan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    familyPlan.FacilityName = await context.Facilities.Where(x => x.Oid == familyPlan.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    familyPlan.EncounterDate = await context.Encounters.Where(x => x.Oid == familyPlan.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return familyPlan;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of FamilyPlan.
        /// </summary>
        /// <returns>Returns a list of all FamilyPlans.</returns>
        public async Task<IEnumerable<FamilyPlan>> GetFamilyPlans()
        {
            try
            {
                return await LoadListWithChildAsync<FamilyPlan>(s => s.IsDeleted == false, p => p.FamilyPlanningSubclass);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a FamilyPlan by ClientId.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns>Returns a FamilyPlan if the ClientID is matched.</returns>
        public async Task<IEnumerable<FamilyPlan>> GetFamilyPlanByClient(Guid ClientId)
        {
            try
            {
                return await context.FamilyPlans.Where(p => p.IsDeleted == false && p.ClientId == ClientId).AsNoTracking()
             .Join(
                 context.Encounters.AsNoTracking(),
               familyPlans => familyPlans.EncounterId,
                 encounter => encounter.Oid,
                 (familyPlans, encounter) => new FamilyPlan
                 {
                     EncounterId = familyPlans.EncounterId,
                     EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                     BackupMethodUsed = familyPlans.BackupMethodUsed,
                     FPMethodPlanRequest = familyPlans.FPMethodPlanRequest,
                     ClientId = familyPlans.ClientId,
                     DateModified = familyPlans.DateModified,
                     DateCreated = familyPlans.DateCreated,
                     CreatedIn = familyPlans.CreatedIn,
                     CreatedBy = familyPlans.CreatedBy,
                     FamilyPlans = familyPlans.FamilyPlans,
                     AnySexualViolenceSymptoms = familyPlans.AnySexualViolenceSymptoms,
                     ClassifyFPMethod = familyPlans.ClassifyFPMethod,
                     ClientIsNotPregnant = familyPlans.ClientIsNotPregnant,
                     ClientNotReceivePreferredOptions = familyPlans.ClientNotReceivePreferredOptions,
                     ClientPreferences = familyPlans.ClientPreferences,
                     ClientReceivePreferredOptions = familyPlans.ClientReceivePreferredOptions,
                     EncounterType = familyPlans.EncounterType,
                     FamilyPlanningSubclass = familyPlans.FamilyPlanningSubclass,
                     FPMethodPlan = familyPlans.FPMethodPlan,
                     FPProvidedPlace = familyPlans.FPProvidedPlace,
                     HasConsentForFP = familyPlans.HasConsentForFP,
                     InteractionId = familyPlans.InteractionId,
                     IsBreastCancer = familyPlans.IsCervicalCancer,
                     IsCervicalCancer = familyPlans.IsCervicalCancer,
                     IsDeleted = familyPlans.IsDeleted,
                     IsHIVTestingNeed = familyPlans.IsHIVTestingNeed,
                     IsProstateCancer = familyPlans.IsProstateCancer,
                     IsSTI = familyPlans.IsSTI,
                     IsSynced = familyPlans.IsSynced,
                     ModifiedBy = familyPlans.ModifiedBy,
                     ModifiedIn = familyPlans.ModifiedIn,
                     ReasonForNoPlan = familyPlans.ReasonForNoPlan,
                     ReasonOfNotPregnant = familyPlans.ReasonOfNotPregnant,
                     SelectedFamilyPlan = familyPlans.SelectedFamilyPlan,
                     SubclassId = familyPlans.SubclassId,
                     ClinicianName = context.UserAccounts.Where(x => x.Oid == familyPlans.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                     FacilityName = context.Facilities.Where(x => x.Oid == familyPlans.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


                 }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of FamilyPlan by EncounterID.
        /// </summary>
        /// <param name="EncounterId"></param>
        /// <returns>Returns a list of all FamilyPlan by EncounterID.</returns>
        public async Task<IEnumerable<FamilyPlan>> GetFamilyPlanByEncounter(Guid EncounterId)
        {
            try
            {
                return await context.FamilyPlans.Where(p => p.IsDeleted == false && p.EncounterId == EncounterId).AsNoTracking()
               .Join(
                   context.Encounters.AsNoTracking(),
                 familyPlans => familyPlans.EncounterId,
                   encounter => encounter.Oid,
                   (familyPlans, encounter) => new FamilyPlan
                   {
                       EncounterId = familyPlans.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       BackupMethodUsed = familyPlans.BackupMethodUsed,
                       FPMethodPlanRequest = familyPlans.FPMethodPlanRequest,
                       ClientId = familyPlans.ClientId,
                       DateModified = familyPlans.DateModified,
                       DateCreated = familyPlans.DateCreated,
                       CreatedIn = familyPlans.CreatedIn,
                       CreatedBy = familyPlans.CreatedBy,
                       FamilyPlans = familyPlans.FamilyPlans,
                       AnySexualViolenceSymptoms = familyPlans.AnySexualViolenceSymptoms,
                       ClassifyFPMethod = familyPlans.ClassifyFPMethod,
                       ClientIsNotPregnant = familyPlans.ClientIsNotPregnant,
                       ClientNotReceivePreferredOptions = familyPlans.ClientNotReceivePreferredOptions,
                       ClientPreferences = familyPlans.ClientPreferences,
                       ClientReceivePreferredOptions = familyPlans.ClientReceivePreferredOptions,
                       EncounterType = familyPlans.EncounterType,
                       FamilyPlanningSubclass = familyPlans.FamilyPlanningSubclass,
                       FPMethodPlan = familyPlans.FPMethodPlan,
                       FPProvidedPlace = familyPlans.FPProvidedPlace,
                       HasConsentForFP = familyPlans.HasConsentForFP,
                       InteractionId = familyPlans.InteractionId,
                       IsBreastCancer = familyPlans.IsCervicalCancer,
                       IsCervicalCancer = familyPlans.IsCervicalCancer,
                       IsDeleted = familyPlans.IsDeleted,
                       IsHIVTestingNeed = familyPlans.IsHIVTestingNeed,
                       IsProstateCancer = familyPlans.IsProstateCancer,
                       IsSTI = familyPlans.IsSTI,
                       IsSynced = familyPlans.IsSynced,
                       ModifiedBy = familyPlans.ModifiedBy,
                       ModifiedIn = familyPlans.ModifiedIn,
                       ReasonForNoPlan = familyPlans.ReasonForNoPlan,
                       ReasonOfNotPregnant = familyPlans.ReasonOfNotPregnant,
                       SelectedFamilyPlan = familyPlans.SelectedFamilyPlan,
                       SubclassId = familyPlans.SubclassId,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == familyPlans.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == familyPlans.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}