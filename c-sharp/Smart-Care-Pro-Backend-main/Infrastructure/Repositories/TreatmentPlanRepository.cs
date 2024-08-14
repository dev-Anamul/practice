using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;

/*
 * Created by   : Shakil
 * Date created : 25.12.2022
 * Modified by  : Shakil
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ITreatmentPlanRepository interface.
    /// </summary>
    public class TreatmentPlanRepository : Repository<TreatmentPlan>, ITreatmentPlanRepository
    {
        public TreatmentPlanRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a treatment plan by key.
        /// </summary>
        /// <param name="key">Primary key of the table TreatmentPlans.</param>
        /// <returns>Returns a treatment plan if the key is matched.</returns>
        public async Task<TreatmentPlan> GetTreatmentPlanByKey(Guid key)
        {
            try
            {
                var treatmentPlan = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (treatmentPlan != null)
                    treatmentPlan.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return treatmentPlan;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of treatment plans.
        /// </summary>
        /// <returns>Returns a list of all treatment plans.</returns>
        public async Task<IEnumerable<TreatmentPlan>> GetTreatmentPlans()
        {
            try
            {
                return await QueryAsync(t => t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a OPD visit by key.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisit.</param>
        /// <returns>Returns a OPD visit if the key is matched.</returns>
        public async Task<IEnumerable<TreatmentPlan>> GetTreatmentPlansOPDVisitID(Guid OPDVisitID)
        {
            try
            {
                return await QueryAsync(t => t.IsDeleted == false && t.EncounterId == OPDVisitID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a OPD visit by key.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisit.</param>
        /// <returns>Returns a OPD visit if the key is matched.</returns>
        public async Task<TreatmentPlan> GetTreatmentPlansEncounterId(Guid encounterId)
        {
            try
            {
                try
                {
                    var treatmentPlan = await FirstOrDefaultAsync(s => s.EncounterId == encounterId && s.IsDeleted == false);

                    if (treatmentPlan != null)
                        treatmentPlan.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                    return treatmentPlan;

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
        /// The method is used to get a TreatmentPlan by Surgery key.
        /// </summary>
        /// <param name="key">Primary key of the table Surgery.</param>
        /// <returns>Returns a TreatmentPlan if the key is matched.</returns>
        public async Task<TreatmentPlan> GetTreatmentPlanBySurgeryId(Guid key)
        {
            try
            {
                var treatmentPlan = await FirstOrDefaultAsync(s => s.SurgeryId == key && s.IsDeleted == false);

                if (treatmentPlan != null)
                    treatmentPlan.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return treatmentPlan;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Returns a client if the key is matched.</returns>
        public async Task<IEnumerable<TreatmentPlan>> GetTreatmentPlansClient(Guid clientID)
        {
            try
            {
                return await context.TreatmentPlans.Include(x => x.Surgery).Where(p => p.IsDeleted == false && p.ClientId == clientID)
     .Join(
         context.Encounters.AsNoTracking(),
         treatmentPlan => treatmentPlan.EncounterId,
         encounter => encounter.Oid,
         (treatmentPlan, encounter) => new TreatmentPlan
         {
             EncounterId = treatmentPlan.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             CreatedIn = treatmentPlan.CreatedIn,
             CreatedBy = treatmentPlan.CreatedBy,
             ClientId = treatmentPlan.ClientId,
             DateCreated = treatmentPlan.DateCreated,
             DateModified = treatmentPlan.DateModified,
             EncounterType = treatmentPlan.EncounterType,
             InteractionId = treatmentPlan.InteractionId,
             IsDeleted = treatmentPlan.IsDeleted,
             IsSynced = treatmentPlan.IsSynced,
             ModifiedBy = treatmentPlan.ModifiedBy,
             ModifiedIn = treatmentPlan.ModifiedIn,
             Surgery = treatmentPlan.Surgery,
             SurgeryId = treatmentPlan.SurgeryId,
             TreatmentPlans = treatmentPlan.TreatmentPlans

         }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<IEnumerable<TreatmentPlan>> GetTreatmentPlansClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {

                var treatmentPlanAsQuerable = context.TreatmentPlans.Include(x => x.Surgery).Where(p => p.IsDeleted == false && p.ClientId == clientId)
  .Join(
      context.Encounters.AsNoTracking(),
      treatmentPlan => treatmentPlan.EncounterId,
      encounter => encounter.Oid,
      (treatmentPlan, encounter) => new TreatmentPlan
      {
          EncounterId = treatmentPlan.EncounterId,
          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
          CreatedIn = treatmentPlan.CreatedIn,
          CreatedBy = treatmentPlan.CreatedBy,
          ClientId = treatmentPlan.ClientId,
          DateCreated = treatmentPlan.DateCreated,
          DateModified = treatmentPlan.DateModified,
          EncounterType = treatmentPlan.EncounterType,
          InteractionId = treatmentPlan.InteractionId,
          IsDeleted = treatmentPlan.IsDeleted,
          IsSynced = treatmentPlan.IsSynced,
          ModifiedBy = treatmentPlan.ModifiedBy,
          ModifiedIn = treatmentPlan.ModifiedIn,
          Surgery = treatmentPlan.Surgery,
          SurgeryId = treatmentPlan.SurgeryId,
          TreatmentPlans = treatmentPlan.TreatmentPlans

      }).AsQueryable();

                if (encounterType == null)
                    return await treatmentPlanAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await treatmentPlanAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public int GetTreatmentPlansClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.TreatmentPlans.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.TreatmentPlans.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        public async Task<IEnumerable<TreatmentPlan>> GetLastEncounterTreatmentPlanByClient(Guid clientID)
        {
            try
            {
                var lastTreatmentPlan = await context.TreatmentPlans.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientID)
.Join(
context.Encounters.AsNoTracking(),
treatmentPlan => treatmentPlan.EncounterId,
encounter => encounter.Oid,
(treatmentPlan, encounter) => new TreatmentPlan
{
    EncounterId = treatmentPlan.EncounterId,
    EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,

}).OrderByDescending(x => x.EncounterDate).FirstOrDefaultAsync();

                if (lastTreatmentPlan == null)
                    return new List<TreatmentPlan>();
                else
                    return await QueryAsync(p => p.IsDeleted == false && p.ClientId == clientID && p.EncounterId == lastTreatmentPlan.EncounterId);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// The method is used to get a client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Returns a client if the key is matched.</returns>
        public async Task<TreatmentPlan> GetLatestTreatmentPlanByClient(Guid ClientID)
        {
            return await LoadWithChildWithOrderByAsync<TreatmentPlan>(c => c.ClientId == ClientID && c.IsDeleted == false);
        }
        public async Task<TreatmentPlan> GetLatestTreatmentPlanByClientForFluid(Guid clientID)
        {

            var treatmentPlan = await context.TreatmentPlans.Where(x => x.ClientId == clientID && x.IsDeleted == false && x.EncounterType == EncounterType.MedicalEncounter).OrderByDescending(x => x.DateCreated).FirstOrDefaultAsync();
            if (treatmentPlan == null)
                treatmentPlan = await context.TreatmentPlans.Where(x => x.ClientId == clientID && x.IsDeleted == false && x.EncounterType == EncounterType.MedicalEncounterIPD).OrderByDescending(x => x.DateCreated).FirstOrDefaultAsync();

            return treatmentPlan;
        }
    }
}