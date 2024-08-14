using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

/*
 * Created by   : Lion
 * Date created : 13.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IAnestheticPlanRepository interface.
    /// </summary>
    public class AnestheticPlanRepository : Repository<AnestheticPlan>, IAnestheticPlanRepository
    {
        public AnestheticPlanRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an anesthetic plan by key.
        /// </summary>
        /// <param name="key">Primary key of the table AnestheticPlans.</param>
        /// <returns>Returns an anesthetic plan if the key is matched.</returns>
        public async Task<AnestheticPlan> GetAnestheticPlanByKey(Guid key)
        {
            try
            {
                var anestheticPlan = await context.AnestheticPlans.AsNoTracking()
                    .Where(a => a.InteractionId == key && a.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (anestheticPlan is not null)
                {
                    anestheticPlan.EncounterDate = await context.Encounters.Where(x => x.Oid == anestheticPlan.EncounterId).AsNoTracking().Select(e => e.OPDVisitDate ?? e.IPDAdmissionDate ?? e.DateCreated).FirstOrDefaultAsync();
                    anestheticPlan.ClinicianName = context.UserAccounts.Where(x => x.Oid == anestheticPlan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "";
                    anestheticPlan.FacilityName = context.Facilities.Where(x => x.Oid == anestheticPlan.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "";
                }

                return anestheticPlan;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of anesthetic plans.
        /// </summary>
        /// <returns>Returns a list of all anesthetic plans.</returns>
        public async Task<IEnumerable<AnestheticPlan>> GetAnestheticPlans()
        {
            try
            {
                return await context.AnestheticPlans.AsNoTracking().Where(a => a.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        anestheticPlan => anestheticPlan.EncounterId,
                        encounter => encounter.Oid,
                        (anestheticPlan, encounter) => new AnestheticPlan
                        {
                            InteractionId = anestheticPlan.InteractionId,
                            ClientHistory = anestheticPlan.ClientHistory,
                            ClientExamination = anestheticPlan.ClientExamination,
                            AnestheticPlans = anestheticPlan.AnestheticPlans,
                            PatientInstructions = anestheticPlan.PatientInstructions,
                            AnesthesiaStartTime = anestheticPlan.AnesthesiaStartTime,
                            AnesthesiaEndTime = anestheticPlan.AnesthesiaEndTime,
                            PostAnesthesia = anestheticPlan.PostAnesthesia,
                            PreOperativeAdverse = anestheticPlan.PreOperativeAdverse,
                            PostOperative = anestheticPlan.PostOperative,
                            SurgeryId = anestheticPlan.SurgeryId,
                            Surgery = anestheticPlan.Surgery,
                            EncounterId = anestheticPlan.EncounterId,
                            EncounterType = anestheticPlan.EncounterType,
                            CreatedBy = anestheticPlan.CreatedBy,
                            CreatedIn = anestheticPlan.CreatedIn,
                            DateCreated = anestheticPlan.DateCreated,
                            DateModified = anestheticPlan.DateModified,
                            ModifiedBy = anestheticPlan.ModifiedBy,
                            ModifiedIn = anestheticPlan.ModifiedIn,
                            IsDeleted = anestheticPlan.IsDeleted,
                            IsSynced = anestheticPlan.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == anestheticPlan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == anestheticPlan.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        }).OrderByDescending(e => e.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an anesthetic plan by EncounterID.
        /// </summary>
        /// <param name="encounterId">Primary key of the table EncounterBaseModel.</param>
        /// <returns>Returns an anesthetic plan if the EncounterID is matched.</returns>
        public async Task<IEnumerable<AnestheticPlan>> GetAnestheticPlanByEncounter(Guid encounterId)
        {
            try
            {
                return await context.AnestheticPlans.AsNoTracking().Where(a => a.IsDeleted == false && a.EncounterId == encounterId)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        anestheticPlan => anestheticPlan.EncounterId,
                        encounter => encounter.Oid,
                        (anestheticPlan, encounter) => new AnestheticPlan
                        {
                            InteractionId = anestheticPlan.InteractionId,
                            ClientHistory = anestheticPlan.ClientHistory,
                            ClientExamination = anestheticPlan.ClientExamination,
                            AnestheticPlans = anestheticPlan.AnestheticPlans,
                            PatientInstructions = anestheticPlan.PatientInstructions,
                            AnesthesiaStartTime = anestheticPlan.AnesthesiaStartTime,
                            AnesthesiaEndTime = anestheticPlan.AnesthesiaEndTime,
                            PostAnesthesia = anestheticPlan.PostAnesthesia,
                            PreOperativeAdverse = anestheticPlan.PreOperativeAdverse,
                            PostOperative = anestheticPlan.PostOperative,
                            SurgeryId = anestheticPlan.SurgeryId,
                            Surgery = anestheticPlan.Surgery,
                            EncounterId = anestheticPlan.EncounterId,
                            EncounterType = anestheticPlan.EncounterType,
                            CreatedBy = anestheticPlan.CreatedBy,
                            CreatedIn = anestheticPlan.CreatedIn,
                            DateCreated = anestheticPlan.DateCreated,
                            DateModified = anestheticPlan.DateModified,
                            ModifiedBy = anestheticPlan.ModifiedBy,
                            ModifiedIn = anestheticPlan.ModifiedIn,
                            IsDeleted = anestheticPlan.IsDeleted,
                            IsSynced = anestheticPlan.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == anestheticPlan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == anestheticPlan.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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
        /// The method is used to get an anesthetic plan by SurgeryID.
        /// </summary>
        /// <param name="surgeryId">Primary key of the table Surgeries.</param>
        /// <returns>Returns an anesthetic plan if the SurgeryID is matched.</returns>
        public async Task<AnestheticPlan> GetAnestheticPlanBySurgery(Guid surgeryId)
        {
            try
            {
                var anestheticPlan = await context.AnestheticPlans.AsNoTracking().
                    Where(a => a.IsDeleted == false && a.SurgeryId == surgeryId)
                    .FirstOrDefaultAsync();

                if (anestheticPlan != null)
                    anestheticPlan.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                anestheticPlan.ClinicianName = context.UserAccounts.Where(x => x.Oid == anestheticPlan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "";
                anestheticPlan.FacilityName = context.Facilities.Where(x => x.Oid == anestheticPlan.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "";

                return anestheticPlan;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an anesthetic plan by SurgeryID.
        /// </summary>
        /// <param name="surgeryId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AnestheticPlan>> GetAnestheticPlanListBySurgery(Guid surgeryId)
        {
            try
            {
                return await context.AnestheticPlans.AsNoTracking().Where(a => a.IsDeleted == false && a.SurgeryId == surgeryId)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        anestheticPlan => anestheticPlan.EncounterId,
                        encounter => encounter.Oid,
                        (anestheticPlan, encounter) => new AnestheticPlan
                        {
                            InteractionId = anestheticPlan.InteractionId,
                            ClientHistory = anestheticPlan.ClientHistory,
                            ClientExamination = anestheticPlan.ClientExamination,
                            AnestheticPlans = anestheticPlan.AnestheticPlans,
                            PatientInstructions = anestheticPlan.PatientInstructions,
                            AnesthesiaStartTime = anestheticPlan.AnesthesiaStartTime,
                            AnesthesiaEndTime = anestheticPlan.AnesthesiaEndTime,
                            PostAnesthesia = anestheticPlan.PostAnesthesia,
                            PreOperativeAdverse = anestheticPlan.PreOperativeAdverse,
                            PostOperative = anestheticPlan.PostOperative,
                            SurgeryId = anestheticPlan.SurgeryId,
                            Surgery = anestheticPlan.Surgery,
                            EncounterId = anestheticPlan.EncounterId,
                            EncounterType = anestheticPlan.EncounterType,
                            CreatedBy = anestheticPlan.CreatedBy,
                            CreatedIn = anestheticPlan.CreatedIn,
                            DateCreated = anestheticPlan.DateCreated,
                            DateModified = anestheticPlan.DateModified,
                            ModifiedBy = anestheticPlan.ModifiedBy,
                            ModifiedIn = anestheticPlan.ModifiedIn,
                            IsDeleted = anestheticPlan.IsDeleted,
                            IsSynced = anestheticPlan.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == anestheticPlan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == anestheticPlan.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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