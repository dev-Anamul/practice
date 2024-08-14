using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
*Created by: Stephan
* Date created: 29.04.2023
* Modified by: Stephan
* Last modified: 13.08.2023
* Reviewed by:
*Date reviewed:
*/
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IWHOClinicalStageRepository class.
    /// </summary>
    public class WHOConditionRepository : Repository<WHOCondition>, IWHOConditionRepository
    {
        public WHOConditionRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a who condition by key.
        /// </summary>
        /// <param name="key">Primary key of the table WHOConditions.</param>
        /// <returns>Returns a who condition if the key is matched.</returns>
        public async Task<WHOCondition> GetWHOConditionByKey(Guid key)
        {
            try
            {
                var wHOCondition = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (wHOCondition != null)
                    wHOCondition.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return wHOCondition;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of who conditions.
        /// </summary>
        /// <returns>Returns a list of all who conditions.</returns>
        public async Task<IEnumerable<WHOCondition>> GetWHOConditions()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Encounter by key.
        /// </summary>
        /// <param name="EncounterId">EncounterId of the table Encounter.</param>
        /// <returns>Returns a Encounter if the EncounterId is matched.</returns>
        public async Task<IEnumerable<WHOCondition>> GetWHOConditionsByEncounterId(Guid EncounterId)
        {
            try
            {
                return await context.WHOConditions.Include(x => x.WHOStagesCondition).ThenInclude(x => x.WHOClinicalStage).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterId)
                      .Join(
                             context.Encounters.AsNoTracking(),
                             wHOCondition => wHOCondition.EncounterId,
                             encounter => encounter.Oid,
                             (wHOCondition, encounter) => new WHOCondition
                             {
                                 EncounterId = wHOCondition.EncounterId,
                                 EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                 ClientId = wHOCondition.ClientId,
                                 WHOStagesCondition = wHOCondition.WHOStagesCondition,
                                 WHOStagesConditionId = wHOCondition.WHOStagesConditionId,
                                 CreatedBy = wHOCondition.CreatedBy,
                                 CreatedIn = wHOCondition.CreatedIn,
                                 DateCreated = wHOCondition.DateCreated,
                                 DateModified = wHOCondition.DateModified,
                                 EncounterType = wHOCondition.EncounterType,
                                 InteractionId = wHOCondition.InteractionId,
                                 IsDeleted = wHOCondition.IsDeleted,
                                 IsSynced = wHOCondition.IsSynced,
                                 ModifiedBy = wHOCondition.ModifiedBy,
                                 ModifiedIn = wHOCondition.ModifiedIn,

                             }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Encounter by key.
        /// </summary>
        /// <param name="ClientId">Primary ClientId of the table Client.</param>
        /// <returns>Returns a Client if the ClientId is matched.</returns>
        public async Task<IEnumerable<WHOCondition>> GetWHOConditionsByClient(Guid clientId)
        {
            try
            {
                return await context.WHOConditions.Include(x => x.WHOStagesCondition).ThenInclude(x => x.WHOClinicalStage).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                      .Join(
                             context.Encounters.AsNoTracking(),
                             wHOCondition => wHOCondition.EncounterId,
                             encounter => encounter.Oid,
                             (wHOCondition, encounter) => new WHOCondition
                             {
                                 EncounterId = wHOCondition.EncounterId,
                                 EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                                 ClientId = wHOCondition.ClientId,
                                 WHOStagesCondition = wHOCondition.WHOStagesCondition,
                                 WHOStagesConditionId = wHOCondition.WHOStagesConditionId,
                                 CreatedBy = wHOCondition.CreatedBy,
                                 CreatedIn = wHOCondition.CreatedIn,
                                 DateCreated = wHOCondition.DateCreated,
                                 DateModified = wHOCondition.DateModified,
                                 EncounterType = wHOCondition.EncounterType,
                                 InteractionId = wHOCondition.InteractionId,
                                 IsDeleted = wHOCondition.IsDeleted,
                                 IsSynced = wHOCondition.IsSynced,
                                 ModifiedBy = wHOCondition.ModifiedBy,
                                 ModifiedIn = wHOCondition.ModifiedIn,

                             }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<WHOCondition>> GetWHOConditionsByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var wHOConditionAsQuerable = context.WHOConditions.Include(x => x.WHOStagesCondition).ThenInclude(x => x.WHOClinicalStage).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                   .Join(
                          context.Encounters.AsNoTracking(),
                          wHOCondition => wHOCondition.EncounterId,
                          encounter => encounter.Oid,
                          (wHOCondition, encounter) => new WHOCondition
                          {
                              EncounterId = wHOCondition.EncounterId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClientId = wHOCondition.ClientId,
                              WHOStagesCondition = wHOCondition.WHOStagesCondition,
                              WHOStagesConditionId = wHOCondition.WHOStagesConditionId,
                              CreatedBy = wHOCondition.CreatedBy,
                              CreatedIn = wHOCondition.CreatedIn,
                              DateCreated = wHOCondition.DateCreated,
                              DateModified = wHOCondition.DateModified,
                              EncounterType = wHOCondition.EncounterType,
                              InteractionId = wHOCondition.InteractionId,
                              IsDeleted = wHOCondition.IsDeleted,
                              IsSynced = wHOCondition.IsSynced,
                              ModifiedBy = wHOCondition.ModifiedBy,
                              ModifiedIn = wHOCondition.ModifiedIn,

                          }).AsQueryable();

                if (encounterType == null)
                    return await wHOConditionAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await wHOConditionAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetWHOConditionsByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.WHOConditions.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.WHOConditions.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
    }
}