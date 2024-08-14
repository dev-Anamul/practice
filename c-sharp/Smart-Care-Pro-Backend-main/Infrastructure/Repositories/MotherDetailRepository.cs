using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class MotherDetailRepository : Repository<MotherDetail>, IMotherDetailRepository
    {
        public MotherDetailRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a MotherDetail if the clientId is matched.</returns>
        public async Task<IEnumerable<MotherDetail>> GetMotherDetailByClient(Guid clientId)
        {
            try
            {
                var motherDetails = context.MotherDetails.Where(m => m.IsDeleted == false && m.ClientId == clientId).AsNoTracking()
                    .Join(context.Encounters.AsNoTracking(),
                    motherDetails => motherDetails.EncounterId,
                    encounter => encounter.Oid,
                    (motherDetails, encounter) => new MotherDetail
                    {
                        ClientId = motherDetails.ClientId,
                        CreatedBy = motherDetails.CreatedBy,
                        CreatedIn = motherDetails.CreatedIn,
                        DateCreated = motherDetails.DateCreated,
                        DateModified = motherDetails.DateModified,
                        DateofDelivary = motherDetails.DateofDelivary,
                        DeliveryMethod = motherDetails.DeliveryMethod,
                        EarlyTerminationReason = motherDetails.EarlyTerminationReason,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        EncounterId = motherDetails.EncounterId,
                        EncounterType = motherDetails.EncounterType,
                        InteractionId = motherDetails.InteractionId,
                        IsDeleted = motherDetails.IsDeleted,
                        IsSynced = motherDetails.IsSynced,
                        MetarnalComplication = motherDetails.MetarnalComplication,
                        MetarnalOutcome = motherDetails.MetarnalOutcome,
                        ModifiedBy = motherDetails.ModifiedBy,
                        ModifiedIn = motherDetails.ModifiedIn,
                        Notes = motherDetails.Notes,
                        PregnancyConclusion = motherDetails.PregnancyConclusion,
                        PregnancyDuration = motherDetails.PregnancyDuration,
                        PregnancyNo = motherDetails.PregnancyNo,
                        PueperiumOutcome = motherDetails.PueperiumOutcome,

                    }).AsQueryable();

                return await motherDetails.OrderByDescending(x => x.EncounterDate).ToListAsync();
                /// return await QueryAsync(b => b.IsDeleted == false && b.ClientId == clientId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<MotherDetail>> GetMotherDetailByClient(Guid clientId, int page, int pageSize)
        {
            try
            {
                var motherDetails = context.MotherDetails.Where(m => m.IsDeleted == false && m.ClientId == clientId).AsNoTracking()
                    .Join(context.Encounters.AsNoTracking(),
                    motherDetails => motherDetails.EncounterId,
                    encounter => encounter.Oid,
                    (motherDetails, encounter) => new MotherDetail
                    {
                        ClientId = motherDetails.ClientId,
                        CreatedBy = motherDetails.CreatedBy,
                        CreatedIn = motherDetails.CreatedIn,
                        DateCreated = motherDetails.DateCreated,
                        DateModified = motherDetails.DateModified,
                        DateofDelivary = motherDetails.DateofDelivary,
                        DeliveryMethod = motherDetails.DeliveryMethod,
                        EarlyTerminationReason = motherDetails.EarlyTerminationReason,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        EncounterId = motherDetails.EncounterId,
                        EncounterType = motherDetails.EncounterType,
                        InteractionId = motherDetails.InteractionId,
                        IsDeleted = motherDetails.IsDeleted,
                        IsSynced = motherDetails.IsSynced,
                        MetarnalComplication = motherDetails.MetarnalComplication,
                        MetarnalOutcome = motherDetails.MetarnalOutcome,
                        ModifiedBy = motherDetails.ModifiedBy,
                        ModifiedIn = motherDetails.ModifiedIn,
                        Notes = motherDetails.Notes,
                        PregnancyConclusion = motherDetails.PregnancyConclusion,
                        PregnancyDuration = motherDetails.PregnancyDuration,
                        PregnancyNo = motherDetails.PregnancyNo,
                        PueperiumOutcome = motherDetails.PueperiumOutcome,

                    }).AsQueryable();

                return await motherDetails.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                /// return await QueryAsync(b => b.IsDeleted == false && b.ClientId == clientId);
            }
            catch (Exception)
            {
                throw;
            }
        }
      
        public int GetMotherDetailByClientTotalCount(Guid clientID)
        {
            return context.MotherDetails.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
        }
        /// <summary>
        /// The method is used to get the list of Mother Detail by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all MotherDetail by EncounterID.</returns>
        public async Task<IEnumerable<MotherDetail>> GetMotherDetailByEncounter(Guid encounterId)
        {
            try
            {
                var motherDetails = context.MotherDetails.Where(m => m.IsDeleted == false && m.EncounterId == encounterId).AsNoTracking()
                    .Join(context.Encounters.AsNoTracking(),
                    motherDetails => motherDetails.EncounterId,
                    encounter => encounter.Oid,
                    (motherDetails, encounter) => new MotherDetail
                    {
                        ClientId = motherDetails.ClientId,
                        CreatedBy = motherDetails.CreatedBy,
                        CreatedIn = motherDetails.CreatedIn,
                        DateCreated = motherDetails.DateCreated,
                        DateModified = motherDetails.DateModified,
                        DateofDelivary = motherDetails.DateofDelivary,
                        DeliveryMethod = motherDetails.DeliveryMethod,
                        EarlyTerminationReason = motherDetails.EarlyTerminationReason,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        EncounterId = motherDetails.EncounterId,
                        EncounterType = motherDetails.EncounterType,
                        InteractionId = motherDetails.InteractionId,
                        IsDeleted = motherDetails.IsDeleted,
                        IsSynced = motherDetails.IsSynced,
                        MetarnalComplication = motherDetails.MetarnalComplication,
                        MetarnalOutcome = motherDetails.MetarnalOutcome,
                        ModifiedBy = motherDetails.ModifiedBy,
                        ModifiedIn = motherDetails.ModifiedIn,
                        Notes = motherDetails.Notes,
                        PregnancyConclusion = motherDetails.PregnancyConclusion,
                        PregnancyDuration = motherDetails.PregnancyDuration,
                        PregnancyNo = motherDetails.PregnancyNo,
                        PueperiumOutcome = motherDetails.PueperiumOutcome,

                    }).AsQueryable();

                return await motherDetails.OrderByDescending(x => x.EncounterDate).ToListAsync();

                /// return await QueryAsync(c => c.IsDeleted == false && c.EncounterId == encounterId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a MotherDetail by key.
        /// </summary>
        /// <param name="key">Primary key of the table MotherDetails.</param>
        /// <returns>Returns a MotherDetail if the key is matched.</returns>
        public async Task<MotherDetail> GetMotherDetailByKey(Guid key)
        {
            try
            {
                var motherDetail = await FirstOrDefaultAsync(m => m.InteractionId == key && m.IsDeleted == false);

                if (motherDetail != null)
                    motherDetail.EncounterDate = await context.Encounters.Select(e => e.OPDVisitDate ?? e.IPDAdmissionDate ?? e.DateCreated).FirstOrDefaultAsync();

                return motherDetail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of MotherDetails.
        /// </summary>
        /// <returns>Returns a list of all MotherDetail.</returns>
        public async Task<IEnumerable<MotherDetail>> GetMotherDetails()
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