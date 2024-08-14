using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Infrastructure.Repositories
{
    public class PrEPRepository : Repository<Plan>, IPrEPRepository
    {
        public PrEPRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get PrEP by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Returns a PrEP if the key is matched.</returns>
        public async Task<Plan> GetPrEPByKey(Guid key)
        {
            try
            {
                var plans = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (plans != null)
                    plans.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return plans;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a PrEP by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a PrEP if the ClientID is matched.</returns>
        public async Task<IEnumerable<Plan>> GetPrEPClient(Guid clientId)
        {
            try
            {
                return await context.Plans.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
               .Join(
                   context.Encounters.AsNoTracking(),
                   plan => plan.EncounterId,
                   encounter => encounter.Oid,
                   (plan, encounter) => new Plan
                   {
                       EncounterId = plan.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       ClientId = plan.ClientId,
                       Plans = plan.Plans,
                       CreatedBy = plan.CreatedBy,
                       CreatedIn = plan.CreatedIn,
                       DateCreated = plan.DateCreated,
                       DateModified = plan.DateModified,
                       EncounterType = plan.EncounterType,
                       HasAcuteHIVInfectionSymptoms = plan.HasAcuteHIVInfectionSymptoms,
                       HasGreaterFiftyCreatinineClearance = plan.HasGreaterFiftyCreatinineClearance,
                       InteractionId = plan.InteractionId,
                       IsAbleToAdhereDailyPrEP = plan.IsAbleToAdhereDailyPrEP,
                       IsDeleted = plan.IsDeleted,
                       IsSynced = plan.IsSynced,
                       IsPotentialHIVExposureMoreThanSixWeeksOld = plan.IsPotentialHIVExposureMoreThanSixWeeksOld,
                       IsUrinalysisNormal = plan.IsUrinalysisNormal,
                       ModifiedBy = plan.ModifiedBy,
                       ModifiedIn = plan.ModifiedIn,
                       Note = plan.Note,
                       StartDate = plan.StartDate,
                       StopDate = plan.StopDate,
                       StoppingReasonId = plan.StoppingReasonId,
                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<Plan>> GetPrEPClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var planAsQuerable = context.Plans.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
               .Join(
                   context.Encounters.AsNoTracking(),
                   plan => plan.EncounterId,
                   encounter => encounter.Oid,
                   (plan, encounter) => new Plan
                   {
                       EncounterId = plan.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       ClientId = plan.ClientId,
                       Plans = plan.Plans,
                       CreatedBy = plan.CreatedBy,
                       CreatedIn = plan.CreatedIn,
                       DateCreated = plan.DateCreated,
                       DateModified = plan.DateModified,
                       EncounterType = plan.EncounterType,
                       HasAcuteHIVInfectionSymptoms = plan.HasAcuteHIVInfectionSymptoms,
                       HasGreaterFiftyCreatinineClearance = plan.HasGreaterFiftyCreatinineClearance,
                       InteractionId = plan.InteractionId,
                       IsAbleToAdhereDailyPrEP = plan.IsAbleToAdhereDailyPrEP,
                       IsDeleted = plan.IsDeleted,
                       IsSynced = plan.IsSynced,
                       IsPotentialHIVExposureMoreThanSixWeeksOld = plan.IsPotentialHIVExposureMoreThanSixWeeksOld,
                       IsUrinalysisNormal = plan.IsUrinalysisNormal,
                       ModifiedBy = plan.ModifiedBy,
                       ModifiedIn = plan.ModifiedIn,
                       Note = plan.Note,
                       StartDate = plan.StartDate,
                       StopDate = plan.StopDate,
                       StoppingReasonId = plan.StoppingReasonId,
                   }).AsQueryable();

                if (encounterType == null)
                    return await planAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await planAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetPrEPClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.Plans.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.Plans.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public async Task<IEnumerable<Plan>> GetPrEPs()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Plan>> GetPrEPByEncounter(Guid encounterId)
        {
            try
            {
                return await context.Plans.Include(x => x.StoppingReason).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId)
                   .Join(
                       context.Encounters.AsNoTracking(),
                       plan => plan.EncounterId,
                       encounter => encounter.Oid,
                       (plan, encounter) => new Plan
                       {
                           EncounterId = plan.EncounterId,
                           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                           ClientId = plan.ClientId,
                           Plans = plan.Plans,
                           CreatedBy = plan.CreatedBy,
                           CreatedIn = plan.CreatedIn,
                           DateCreated = plan.DateCreated,
                           DateModified = plan.DateModified,
                           EncounterType = plan.EncounterType,
                           HasAcuteHIVInfectionSymptoms = plan.HasAcuteHIVInfectionSymptoms,
                           HasGreaterFiftyCreatinineClearance = plan.HasGreaterFiftyCreatinineClearance,
                           InteractionId = plan.InteractionId,
                           IsAbleToAdhereDailyPrEP = plan.IsAbleToAdhereDailyPrEP,
                           IsDeleted = plan.IsDeleted,
                           IsSynced = plan.IsSynced,
                           IsPotentialHIVExposureMoreThanSixWeeksOld = plan.IsPotentialHIVExposureMoreThanSixWeeksOld,
                           IsUrinalysisNormal = plan.IsUrinalysisNormal,
                           ModifiedBy = plan.ModifiedBy,
                           ModifiedIn = plan.ModifiedIn,
                           Note = plan.Note,
                           StartDate = plan.StartDate,
                           StopDate = plan.StopDate,
                           StoppingReasonId = plan.StoppingReasonId,
                           StoppingReason = plan.StoppingReason,

                       }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Plan> GetPrEPEncounterId(Guid encounterId)
        {
            try
            {
                return await context.Plans.Include(x => x.StoppingReason).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId)
                   .Join(
                       context.Encounters.AsNoTracking(),
                       plan => plan.EncounterId,
                       encounter => encounter.Oid,
                       (plan, encounter) => new Plan
                       {
                           EncounterId = plan.EncounterId,
                           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                           ClientId = plan.ClientId,
                           Plans = plan.Plans,
                           CreatedBy = plan.CreatedBy,
                           CreatedIn = plan.CreatedIn,
                           DateCreated = plan.DateCreated,
                           DateModified = plan.DateModified,
                           EncounterType = plan.EncounterType,
                           HasAcuteHIVInfectionSymptoms = plan.HasAcuteHIVInfectionSymptoms,
                           HasGreaterFiftyCreatinineClearance = plan.HasGreaterFiftyCreatinineClearance,
                           InteractionId = plan.InteractionId,
                           IsAbleToAdhereDailyPrEP = plan.IsAbleToAdhereDailyPrEP,
                           IsDeleted = plan.IsDeleted,
                           IsSynced = plan.IsSynced,
                           IsPotentialHIVExposureMoreThanSixWeeksOld = plan.IsPotentialHIVExposureMoreThanSixWeeksOld,
                           IsUrinalysisNormal = plan.IsUrinalysisNormal,
                           ModifiedBy = plan.ModifiedBy,
                           ModifiedIn = plan.ModifiedIn,
                           Note = plan.Note,
                           StartDate = plan.StartDate,
                           StopDate = plan.StopDate,
                           StoppingReasonId = plan.StoppingReasonId,
                           StoppingReason = plan.StoppingReason,

                       }).OrderByDescending(x => x.EncounterDate).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}