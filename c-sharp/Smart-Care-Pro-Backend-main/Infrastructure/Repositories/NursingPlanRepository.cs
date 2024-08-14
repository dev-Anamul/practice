using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 04.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class NursingPlanRepository : Repository<NursingPlan>, INursingPlanRepository
    {
        public NursingPlanRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a nursing plan by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a nursing plan if the ClientID is matched.</returns>
        public async Task<IEnumerable<NursingPlan>> GetNursingPlanByClient(Guid clientId)
        {
            try
            {
                return await context.NursingPlans.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
      .Join(
          context.Encounters.AsNoTracking(),
          nursingPlan => nursingPlan.EncounterId,
          encounter => encounter.Oid,
          (nursingPlan, encounter) => new NursingPlan
          {
              EncounterId = nursingPlan.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              InteractionId = nursingPlan.InteractionId,
              CreatedIn = nursingPlan.CreatedIn,
              CreatedBy = nursingPlan.CreatedBy,
              ClientId = nursingPlan.ClientId,
              EncounterType = nursingPlan.EncounterType,
              DateCreated = nursingPlan.DateCreated,
              DateModified = nursingPlan.DateModified,
              Evaluation = nursingPlan.Evaluation,
              IsDeleted = nursingPlan.IsDeleted,
              IsSynced = nursingPlan.IsSynced,
              ModifiedBy = nursingPlan.ModifiedBy,
              ModifiedIn = nursingPlan.ModifiedIn,
              NursingDiagnosis = nursingPlan.NursingDiagnosis,
              NursingIntervention = nursingPlan.NursingDiagnosis,
              Objective = nursingPlan.Objective,
              PlanningDate = nursingPlan.PlanningDate,
              PlanningTime = nursingPlan.PlanningTime,
              Problem = nursingPlan.Problem,

          }).OrderByDescending(x => x.EncounterDate).ToListAsync();



            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<NursingPlan>> GetNursingPlanByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var nursingPlansAsQuerable = context.NursingPlans.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
      .Join(
          context.Encounters.AsNoTracking(),
          nursingPlan => nursingPlan.EncounterId,
          encounter => encounter.Oid,
          (nursingPlan, encounter) => new NursingPlan
          {
              EncounterId = nursingPlan.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              InteractionId = nursingPlan.InteractionId,
              CreatedIn = nursingPlan.CreatedIn,
              CreatedBy = nursingPlan.CreatedBy,
              ClientId = nursingPlan.ClientId,
              EncounterType = nursingPlan.EncounterType,
              DateCreated = nursingPlan.DateCreated,
              DateModified = nursingPlan.DateModified,
              Evaluation = nursingPlan.Evaluation,
              IsDeleted = nursingPlan.IsDeleted,
              IsSynced = nursingPlan.IsSynced,
              ModifiedBy = nursingPlan.ModifiedBy,
              ModifiedIn = nursingPlan.ModifiedIn,
              NursingDiagnosis = nursingPlan.NursingDiagnosis,
              NursingIntervention = nursingPlan.NursingDiagnosis,
              Objective = nursingPlan.Objective,
              PlanningDate = nursingPlan.PlanningDate,
              PlanningTime = nursingPlan.PlanningTime,
              Problem = nursingPlan.Problem,

          }).AsQueryable();

                if (encounterType == null)
                    return await nursingPlansAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await nursingPlansAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetNursingPlanByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.NursingPlans.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.NursingPlans.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get Nursing Plan by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a Nursing Plan if the OPD EncounterID is matched.</returns>
        public async Task<IEnumerable<NursingPlan>> GetNursingPlanByEncounterId(Guid encounterId)
        {
            try
            {
                return await context.NursingPlans.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId)
       .Join(
           context.Encounters.AsNoTracking(),
           nursingPlan => nursingPlan.EncounterId,
           encounter => encounter.Oid,
           (nursingPlan, encounter) => new NursingPlan
           {
               EncounterId = nursingPlan.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               InteractionId = nursingPlan.InteractionId,
               CreatedIn = nursingPlan.CreatedIn,
               CreatedBy = nursingPlan.CreatedBy,
               ClientId = nursingPlan.ClientId,
               EncounterType = nursingPlan.EncounterType,
               DateCreated = nursingPlan.DateCreated,
               DateModified = nursingPlan.DateModified,
               Evaluation = nursingPlan.Evaluation,
               IsDeleted = nursingPlan.IsDeleted,
               IsSynced = nursingPlan.IsSynced,
               ModifiedBy = nursingPlan.ModifiedBy,
               ModifiedIn = nursingPlan.ModifiedIn,
               NursingDiagnosis = nursingPlan.NursingDiagnosis,
               NursingIntervention = nursingPlan.NursingDiagnosis,
               Objective = nursingPlan.Objective,
               PlanningDate = nursingPlan.PlanningDate,
               PlanningTime = nursingPlan.PlanningTime,
               Problem = nursingPlan.Problem,

           }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get Nursing Plan by key.
        /// </summary>
        /// <param name="key">Primary key of the table NursingPlans.</param>
        /// <returns>Returns a Nursing Plan if the key is matched.</returns>
        public async Task<NursingPlan> GetNursingPlanByKey(Guid key)
        {
            try
            {
                var nursingPlan = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (nursingPlan != null)
                    nursingPlan.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return nursingPlan;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of nursing plans.
        /// </summary>
        /// <returns>Returns a list of all nursing plan.</returns>
        public async Task<IEnumerable<NursingPlan>> GetNursingPlans()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}