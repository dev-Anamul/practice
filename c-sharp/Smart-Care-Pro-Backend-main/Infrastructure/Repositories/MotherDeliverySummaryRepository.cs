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
    public class MotherDeliverySummaryRepository : Repository<MotherDeliverySummary>, IMotherDeliverySummaryRepository
    {
        public MotherDeliverySummaryRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// The method is used to a get mother delivery summary by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Returns a mother delivery summary if the key is matched.</returns>
        public async Task<MotherDeliverySummary> GetMotherDeliverySummaryByKey(Guid key)
        {
            try
            {
                var motherDeliverySummary = await FirstOrDefaultAsync(m => m.InteractionId == key && m.IsDeleted == false);

                if (motherDeliverySummary != null)
                    motherDeliverySummary.EncounterDate = await context.Encounters.Select(e => e.OPDVisitDate ?? e.IPDAdmissionDate ?? e.DateCreated).FirstOrDefaultAsync();

                return motherDeliverySummary;

                /// return await FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get a mother delivery summary by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a mother delivery summary if the ClientID is matched.</returns>
        public async Task<IEnumerable<MotherDeliverySummary>> GetMotherDeliverySummaryByClient(Guid clientId)
        {
            try
            {
                var motherDeliverySummary = context.MotherDeliverySummaries.Where(m => m.IsDeleted == false && m.ClientId == clientId).AsNoTracking()
                    .Join(context.Encounters.AsNoTracking(),
                    motherDeliverySummary => motherDeliverySummary.EncounterId,
                    encounter => encounter.Oid,
                    (motherDeliverySummary,encounter) => new MotherDeliverySummary
                    {
                        BirthType = motherDeliverySummary.BirthType,
                        ClientId = motherDeliverySummary.ClientId,
                        CreatedBy = motherDeliverySummary.CreatedBy,
                        CreatedIn = motherDeliverySummary.CreatedIn,
                        DateCreated = motherDeliverySummary.DateCreated,
                        DateModified = motherDeliverySummary.DateModified,
                        DeliveredBy = motherDeliverySummary.DeliveredBy,
                        DeliveredByName = motherDeliverySummary.DeliveredByName,
                        DeliveredType = motherDeliverySummary.DeliveredType,
                        DeliveryLocation = motherDeliverySummary.DeliveryLocation,
                        DurationOfRupture = motherDeliverySummary.DurationOfRupture,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        EncounterId =motherDeliverySummary.EncounterId,
                        EncounterType = motherDeliverySummary.EncounterType,
                        GestationalPeriod = motherDeliverySummary.GestationalPeriod,
                        InteractionId = motherDeliverySummary.InteractionId,
                        IsDeleted =motherDeliverySummary.IsDeleted,
                        IsSynced = motherDeliverySummary.IsSynced,
                        LaborDuration = motherDeliverySummary.LaborDuration,
                        ModifiedBy = motherDeliverySummary.ModifiedBy,
                        ModifiedIn = motherDeliverySummary.ModifiedIn,
                        Number = motherDeliverySummary.Number,
                        Other = motherDeliverySummary.Other,
                        VaginalWashes = motherDeliverySummary.VaginalWashes,
                    }
                    ).AsQueryable();
                
                return await motherDeliverySummary.OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of mother delivery summaries.
        /// </summary>
        /// <returns>Returns a list of all mother delivery summaries.</returns>
        public async Task<IEnumerable<MotherDeliverySummary>> GetMotherDeliverySummaries()
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

        public async Task<IEnumerable<MotherDeliverySummary>> GetMotherDeliverySummaryByEncounter(Guid encounterId)
        {
            try
            {
                //    var motherDeliverySummary = context.MotherDeliverySummaries.Where(m => m.IsDeleted == false && m.EncounterId == encounterId).AsNoTracking()
                //        .Join(context.Encounters.AsNoTracking(),
                //        motherDeliverySummary => motherDeliverySummary.EncounterId,
                //        encounter => encounter.Oid,
                //        (motherDeliverySummary, encounter) => new MotherDeliverySummary
                //        {
                //            BirthType = motherDeliverySummary.BirthType,
                //            ClientId = motherDeliverySummary.ClientId,
                //            CreatedBy = motherDeliverySummary.CreatedBy,
                //            CreatedIn = motherDeliverySummary.CreatedIn,
                //            DateCreated = motherDeliverySummary.DateCreated,
                //            DateModified = motherDeliverySummary.DateModified,
                //            DeliveredBy = motherDeliverySummary.DeliveredBy,
                //            DeliveredByName = motherDeliverySummary.DeliveredByName,
                //            DeliveredType = motherDeliverySummary.DeliveredType,
                //            DeliveryLocation = motherDeliverySummary.DeliveryLocation,
                //            DurationOfRupture = motherDeliverySummary.DurationOfRupture,
                //            EncounterDate = motherDeliverySummary.EncounterDate,
                //            EncounterId = motherDeliverySummary.EncounterId,
                //            EncounterType = motherDeliverySummary.EncounterType,
                //            GestationalPeriod = motherDeliverySummary.GestationalPeriod,
                //            InteractionId = motherDeliverySummary.InteractionId,
                //            IsDeleted = motherDeliverySummary.IsDeleted,
                //            IsSynced = motherDeliverySummary.IsSynced,
                //            LaborDuration = motherDeliverySummary.LaborDuration,
                //            ModifiedBy = motherDeliverySummary.ModifiedBy,
                //            ModifiedIn = motherDeliverySummary.ModifiedIn,
                //            Number = motherDeliverySummary.Number,
                //            Other = motherDeliverySummary.Other,
                //            VaginalWashes = motherDeliverySummary.VaginalWashes
                //        }
                //        ).AsQueryable();

                //    return await motherDeliverySummary.OrderByDescending(x => x.EncounterDate).ToListAsync();
                return await QueryAsync(b => b.IsDeleted == false && b.EncounterId == encounterId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}