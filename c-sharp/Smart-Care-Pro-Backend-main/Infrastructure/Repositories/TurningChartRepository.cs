using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;

/*
 * Created by    : Shakil
 * Date created  : 07.02.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Repositories
{
    public class TurningChartRepository : Repository<TurningChart>, ITurningChartRepository
    {
        public TurningChartRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a nursing plan by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a nursing plan if the ClientID is matched.</returns>
        public async Task<IEnumerable<TurningChart>> GetTurningChartByClient(Guid clientId)
        {
            try
            {
                return await context.TurningCharts.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                 .Join(
                        context.Encounters.AsNoTracking(),
                        turningChart => turningChart.EncounterId,
                        encounter => encounter.Oid,
                        (turningChart, encounter) => new TurningChart
                        {
                            EncounterId = turningChart.EncounterId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            InteractionId = turningChart.InteractionId,
                            EncounterType = turningChart.EncounterType,
                            DateModified = turningChart.DateModified,
                            DateCreated = turningChart.DateCreated,
                            ClientId = clientId,
                            CreatedIn = turningChart.CreatedIn,
                            CreatedBy = turningChart.CreatedBy,
                            Comments = turningChart.Comments,
                            IsDeleted = turningChart.IsDeleted,
                            IsSynced = turningChart.IsSynced,
                            ModifiedBy = turningChart.ModifiedBy,
                            ModifiedIn = turningChart.ModifiedIn,
                            RecordDate = turningChart.RecordDate,
                            TurningTime = turningChart.TurningTime,
                        }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<TurningChart>> GetTurningChartByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var turningChartsAsQuerable = context.TurningCharts.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                 .Join(
                        context.Encounters.AsNoTracking(),
                        turningChart => turningChart.EncounterId,
                        encounter => encounter.Oid,
                        (turningChart, encounter) => new TurningChart
                        {
                            EncounterId = turningChart.EncounterId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            InteractionId = turningChart.InteractionId,
                            EncounterType = turningChart.EncounterType,
                            DateModified = turningChart.DateModified,
                            DateCreated = turningChart.DateCreated,
                            ClientId = clientId,
                            CreatedIn = turningChart.CreatedIn,
                            CreatedBy = turningChart.CreatedBy,
                            Comments = turningChart.Comments,
                            IsDeleted = turningChart.IsDeleted,
                            IsSynced = turningChart.IsSynced,
                            ModifiedBy = turningChart.ModifiedBy,
                            ModifiedIn = turningChart.ModifiedIn,
                            RecordDate = turningChart.RecordDate,
                            TurningTime = turningChart.TurningTime,
                        }).AsQueryable();

                if (encounterType == null)
                    return await turningChartsAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await turningChartsAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetTurningChartByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.TurningCharts.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.TurningCharts.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get a birth record by Encounter.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a birth record if the OPD EncounterID is matched.</returns>
        public async Task<IEnumerable<TurningChart>> GetTurningChartByEncounterId(Guid EncounterID)
        {
            try
            {
                return await context.TurningCharts.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
                 .Join(
                        context.Encounters.AsNoTracking(),
                        turningChart => turningChart.EncounterId,
                        encounter => encounter.Oid,
                        (turningChart, encounter) => new TurningChart
                        {
                            EncounterId = turningChart.EncounterId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            InteractionId = turningChart.InteractionId,
                            EncounterType = turningChart.EncounterType,
                            DateModified = turningChart.DateModified,
                            DateCreated = turningChart.DateCreated,
                            ClientId = turningChart.ClientId,
                            CreatedIn = turningChart.CreatedIn,
                            CreatedBy = turningChart.CreatedBy,
                            Comments = turningChart.Comments,
                            IsDeleted = turningChart.IsDeleted,
                            IsSynced = turningChart.IsSynced,
                            ModifiedBy = turningChart.ModifiedBy,
                            ModifiedIn = turningChart.ModifiedIn,
                            RecordDate = turningChart.RecordDate,
                            TurningTime = turningChart.TurningTime,
                        }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a birth record by key.
        /// </summary>
        /// <param name="key">Primary key of the table TurningCharts.</param>
        /// <returns>Returns a birth record if the key is matched.</returns>
        public async Task<TurningChart> GetTurningChartByKey(Guid key)
        {
            try
            {
                var turningChart = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (turningChart != null)
                    turningChart.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return turningChart;
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
        public async Task<IEnumerable<TurningChart>> GetTurningCharts()
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