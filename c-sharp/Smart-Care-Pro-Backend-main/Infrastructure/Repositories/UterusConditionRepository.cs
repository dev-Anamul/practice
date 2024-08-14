using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IUseTBIdentificationMethodRepository interface.
    /// </summary>
    public class UterusConditionRepository : Repository<UterusCondition>, IUterusConditionRepository
    {
        public UterusConditionRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get UterusCondition by key.
        /// </summary>
        /// <param name="key">Primary key of the table UterusConditions.</param>
        /// <returns>Returns a UterusCondition if the key is matched.</returns>
        public async Task<UterusCondition> GetUterusConditionByKey(Guid key)
        {
            try
            {
                var uterusCondition = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (uterusCondition != null)
                    uterusCondition.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return uterusCondition;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of UterusConditions.
        /// </summary>
        /// <returns>Returns a list of all Medical treatments.</returns>
        public async Task<IEnumerable<UterusCondition>> GetUterusConditions()
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

        /// <summary>
        /// The method is used to get the list of UterusCondition by EncounterId.
        /// </summary>
        /// <param name="EncounterId">Primary key of the table UterusConditions.</param>
        /// <returns>Returns a list of all UterusCondition by EncounterId.</returns>
        public async Task<IEnumerable<UterusCondition>> GetUterusConditionByEncounter(Guid EncounterId)
        {
            try
            {
                return await context.UterusConditions.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterId)
               .Join(
                      context.Encounters.AsNoTracking(),
                      uterusCondition => uterusCondition.EncounterId,
                      encounter => encounter.Oid,
                      (uterusCondition, encounter) => new UterusCondition
                      {
                          EncounterId = uterusCondition.EncounterId,
                          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                          DateModified = uterusCondition.DateModified,
                          InteractionId = uterusCondition.InteractionId,
                          EncounterType = uterusCondition.EncounterType,
                          ConditionOfUterus = uterusCondition.ConditionOfUterus,
                          CreatedBy = uterusCondition.CreatedBy,
                          CreatedIn = uterusCondition.CreatedIn,
                          DateCreated = uterusCondition.DateCreated,
                          DeliveryId = uterusCondition.DeliveryId,
                          IsDeleted = uterusCondition.IsDeleted,
                          IsSynced = uterusCondition.IsSynced,
                          ModifiedBy = uterusCondition.ModifiedBy,
                          ModifiedIn = uterusCondition.ModifiedIn,
                          Other = uterusCondition.Other,
                      }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of UterusCondition by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all UterusCondition by DeliveryId.</returns>
        public async Task<IEnumerable<UterusCondition>> GetUterusConditionByDelivery(Guid deliveryId)
        {
            try
            {
                return await context.UterusConditions.AsNoTracking().Where(p => p.IsDeleted == false && p.DeliveryId == deliveryId)
               .Join(
                      context.Encounters.AsNoTracking(),
                      uterusCondition => uterusCondition.EncounterId,
                      encounter => encounter.Oid,
                      (uterusCondition, encounter) => new UterusCondition
                      {
                          EncounterId = uterusCondition.EncounterId,
                          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                          DateModified = uterusCondition.DateModified,
                          InteractionId = uterusCondition.InteractionId,
                          EncounterType = uterusCondition.EncounterType,
                          ConditionOfUterus = uterusCondition.ConditionOfUterus,
                          CreatedBy = uterusCondition.CreatedBy,
                          CreatedIn = uterusCondition.CreatedIn,
                          DateCreated = uterusCondition.DateCreated,
                          DeliveryId = uterusCondition.DeliveryId,
                          IsDeleted = uterusCondition.IsDeleted,
                          IsSynced = uterusCondition.IsSynced,
                          ModifiedBy = uterusCondition.ModifiedBy,
                          ModifiedIn = uterusCondition.ModifiedIn,
                          Other = uterusCondition.Other,
                      }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}