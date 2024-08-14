using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ExposerRiskRepository : Repository<ExposureRisk>, IExposerRiskRepository
    {
        public ExposerRiskRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of covidComorbidity.
        /// </summary>
        /// <returns>Returns a list of all covid comorbidities.</returns>
        public async Task<IEnumerable<ExposureRisk>> GetExposerRiskes()
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
        /// The method is used to get Covid ExposureRisk to by CovidId.
        /// </summary>
        /// <param name="CovidId">CovidId of Covid ExposureRisk to.</param>
        /// <returns>Returns Covid ExposureRisk to if the covidId is matched.</returns>
        public async Task<IEnumerable<ExposureRisk>> GetExposureRiskByCovid(Guid CovidId)
        {
            try
            {
                return await context.ExposureRisks.Where(p => p.IsDeleted == false && p.CovidId == CovidId).AsNoTracking()
             //.Join(
             //    context.Encounters.AsNoTracking(),
             //   exposureRisk => exposureRisk.EncounterId,
             //    encounter => encounter.Oid,
             //    (exposureRisk, encounter) => new ExposureRisk
             //    {
             //        EncounterId = exposureRisk.EncounterId,
             //        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             //        CreatedBy = exposureRisk.CreatedBy,
             //        CreatedIn = exposureRisk.CreatedIn,
             //        DateCreated = exposureRisk.DateCreated,
             //        CovidId = exposureRisk.CovidId,
             //        DateModified = exposureRisk.DateModified,
             //        EncounterType = exposureRisk.EncounterType,
             //        ExposureRisks = exposureRisk.ExposureRisks,
             //        InteractionId = exposureRisk.InteractionId,
             //        IsDeleted = exposureRisk.IsDeleted,
             //        IsSynced = exposureRisk.IsSynced,
             //        ModifiedBy = exposureRisk.ModifiedBy,
             //        ModifiedIn = exposureRisk.ModifiedIn,

             //    }).OrderByDescending(x => x.EncounterDate)
                 .ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to a getcovid comobi by key.
        /// </summary>
        /// <param name="key">Primary key of the table CovidComobidity.</param>
        /// <returns>Returns a birth history if the key is matched.</returns>
        public async Task<ExposureRisk> GetExposerRiskByKey(Guid key)
        {
            try
            {
                var exposureRisk = await FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);
                if (exposureRisk != null)
                    exposureRisk.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return exposureRisk;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}