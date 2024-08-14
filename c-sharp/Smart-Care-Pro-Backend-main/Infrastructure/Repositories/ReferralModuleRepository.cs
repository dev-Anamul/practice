using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

namespace Infrastructure.Repositories
{
    public class ReferralModuleRepository : Repository<ReferralModule>, IReferralModuleRepository
    {
        public ReferralModuleRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// The method is used to a get referral module by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Returns a referral module if the key is matched.</returns>
        public async Task<ReferralModule> GetReferralModuleByKey(Guid key)
        {
            try
            {
                var referralModule = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (referralModule != null)
                    referralModule.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return referralModule;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a referral module by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a referral module if the ClientID is matched.</returns>
        public async Task<IEnumerable<ReferralModule>> GetReferralModuleByClient(Guid clientId)
        {
            try
            {
                return await context.ReferralModules.Include(x => x.ReceivingFacility).Include(z => z.ServicePoint).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                  .Join(
                      context.Encounters.AsNoTracking(),
                      referralModule => referralModule.EncounterId,
                      encounter => encounter.Oid,
                      (referralModule, encounter) => new ReferralModule
                      {
                          EncounterId = referralModule.EncounterId,
                          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                          ClientId = referralModule.ClientId,
                          Comments = referralModule.Comments,
                          CreatedBy = referralModule.CreatedBy,
                          CreatedIn = referralModule.CreatedIn,
                          DateCreated = referralModule.DateCreated,
                          DateModified = referralModule.DateModified,
                          EncounterType = referralModule.EncounterType,
                          InteractionId = referralModule.InteractionId,
                          IsDeleted = referralModule.IsDeleted,
                          IsProceededFacility = referralModule.IsProceededFacility,
                          IsSynced = referralModule.IsSynced,
                          ModifiedBy = referralModule.ModifiedBy,
                          ModifiedIn = referralModule.ModifiedIn,
                          OtherFacility = referralModule.OtherFacility,
                          OtherFacilityType = referralModule.OtherFacilityType,
                          ReasonForReferral = referralModule.ReasonForReferral,
                          ReceivingFacilityId = referralModule.ReceivingFacilityId,
                          ReceivingFacility = referralModule.ReceivingFacility,
                          ReferralType = referralModule.ReferralType,
                          ReferredFacility = referralModule.ReferredFacility,
                          ReferredFacilityId = referralModule.ReferredFacilityId,
                          ServicePointId = referralModule.ServicePointId,
                          ServicePoint = referralModule.ServicePoint,

                      }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<ReferralModule>> GetReferralModuleByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var referralModuleAsQuerable = context.ReferralModules.Include(x => x.ReceivingFacility).Include(z => z.ServicePoint).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                  .Join(
                      context.Encounters.AsNoTracking(),
                      referralModule => referralModule.EncounterId,
                      encounter => encounter.Oid,
                      (referralModule, encounter) => new ReferralModule
                      {
                          EncounterId = referralModule.EncounterId,
                          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                          ClientId = referralModule.ClientId,
                          Comments = referralModule.Comments,
                          CreatedBy = referralModule.CreatedBy,
                          CreatedIn = referralModule.CreatedIn,
                          DateCreated = referralModule.DateCreated,
                          DateModified = referralModule.DateModified,
                          EncounterType = referralModule.EncounterType,
                          InteractionId = referralModule.InteractionId,
                          IsDeleted = referralModule.IsDeleted,
                          IsProceededFacility = referralModule.IsProceededFacility,
                          IsSynced = referralModule.IsSynced,
                          ModifiedBy = referralModule.ModifiedBy,
                          ModifiedIn = referralModule.ModifiedIn,
                          OtherFacility = referralModule.OtherFacility,
                          OtherFacilityType = referralModule.OtherFacilityType,
                          ReasonForReferral = referralModule.ReasonForReferral,
                          ReceivingFacilityId = referralModule.ReceivingFacilityId,
                          ReceivingFacility = referralModule.ReceivingFacility,
                          ReferralType = referralModule.ReferralType,
                          ReferredFacility = referralModule.ReferredFacility,
                          ReferredFacilityId = referralModule.ReferredFacilityId,
                          ServicePointId = referralModule.ServicePointId,
                          ServicePoint = referralModule.ServicePoint,

                      }).AsQueryable();

                if (encounterType == null)
                    return await referralModuleAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await referralModuleAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetReferralModuleByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.ReferralModules.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.ReferralModules.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get the list of referral modules.
        /// </summary>
        /// <returns>Returns a list of all referral modules.</returns>
        public async Task<IEnumerable<ReferralModule>> GetReferralModules()
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

        public async Task<IEnumerable<ReferralModule>> GetReferralModuleByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.ReferralModules.Include(x => x.ReceivingFacility).Include(z => z.ServicePoint).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
                  .Join(
                      context.Encounters.AsNoTracking(),
                      referralModule => referralModule.EncounterId,
                      encounter => encounter.Oid,
                      (referralModule, encounter) => new ReferralModule
                      {
                          EncounterId = referralModule.EncounterId,
                          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                          ClientId = referralModule.ClientId,
                          Comments = referralModule.Comments,
                          CreatedBy = referralModule.CreatedBy,
                          CreatedIn = referralModule.CreatedIn,
                          DateCreated = referralModule.DateCreated,
                          DateModified = referralModule.DateModified,
                          EncounterType = referralModule.EncounterType,
                          InteractionId = referralModule.InteractionId,
                          IsDeleted = referralModule.IsDeleted,
                          IsProceededFacility = referralModule.IsProceededFacility,
                          IsSynced = referralModule.IsSynced,
                          ModifiedBy = referralModule.ModifiedBy,
                          ModifiedIn = referralModule.ModifiedIn,
                          OtherFacility = referralModule.OtherFacility,
                          OtherFacilityType = referralModule.OtherFacilityType,
                          ReasonForReferral = referralModule.ReasonForReferral,
                          ReceivingFacilityId = referralModule.ReceivingFacilityId,
                          ReceivingFacility = referralModule.ReceivingFacility,
                          ReferralType = referralModule.ReferralType,
                          ReferredFacility = referralModule.ReferredFacility,
                          ReferredFacilityId = referralModule.ReferredFacilityId,
                          ServicePointId = referralModule.ServicePointId,
                          ServicePoint = referralModule.ServicePoint,

                      }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}