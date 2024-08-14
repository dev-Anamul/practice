using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Biplob Roy
 * Date created : 02.05.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class DischargeMetricRepository : Repository<DischargeMetric>, IDischargeMetricRepository
    {
        /// <summary>
        /// Implementation of IDischargeMetricRepository interface.
        /// </summary>
        public DischargeMetricRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a DischargeMetric by key.
        /// </summary>
        /// <param name="key">Primary key of the table DischargeMetrics.</param>
        /// <returns>Returns a DischargeMetric if the key is matched.</returns>
        public async Task<DischargeMetric> GetDischargeMetricByKey(Guid key)
        {
            try
            {
                var dischargeMetric = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (dischargeMetric == null)
                {
                    dischargeMetric.ClinicianName = await context.UserAccounts.Where(x => x.Oid == dischargeMetric.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    dischargeMetric.FacilityName = await context.Facilities.Where(x => x.Oid == dischargeMetric.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    dischargeMetric.EncounterDate = await context.Encounters.Where(x => x.Oid == dischargeMetric.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                }

                return dischargeMetric;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of DischargeMetric.
        /// </summary>
        /// <returns>Returns a list of all DischargeMetrics.</returns>
        public async Task<IEnumerable<DischargeMetric>> GetDischargeMetrics()
        {
            try
            {
                return await QueryAsync(n => n.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a DischargeMetric by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a DischargeMetric if the ClientID is matched.</returns>
        public async Task<IEnumerable<DischargeMetric>> GetDischargeMetricByClient(Guid clientId)
        {
            try
            {
                return await context.DischargeMetrics.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
    .Join(
        context.Encounters.AsNoTracking(),
        dischargeMetric => dischargeMetric.EncounterId,
        encounter => encounter.Oid,
        (dischargeMetric, encounter) => new DischargeMetric
        {
            EncounterId = dischargeMetric.EncounterId,
            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
            ClientId = dischargeMetric.ClientId,
            DateCreated = dischargeMetric.DateCreated,
            CreatedBy = dischargeMetric.CreatedBy,
            CreatedIn = dischargeMetric.CreatedIn,
            ChestCircumference = dischargeMetric.ChestCircumference,
            EncounterType = dischargeMetric.EncounterType,
            DateModified = dischargeMetric.DateModified,
            ApgarScore = dischargeMetric.ApgarScore,
            BodyLength = dischargeMetric.BodyLength,
            HeadCircumference = dischargeMetric.HeadCircumference,
            InteractionId = dischargeMetric.InteractionId,
            IsDeleted = dischargeMetric.IsDeleted,
            IsSynced = dischargeMetric.IsSynced,
            ModifiedBy = dischargeMetric.ModifiedBy,
            ModifiedIn = dischargeMetric.ModifiedIn,
            PerinatalProblems = dischargeMetric.PerinatalProblems,
            ClinicianName = context.UserAccounts.Where(x => x.Oid == dischargeMetric.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
            FacilityName = context.Facilities.Where(x => x.Oid == dischargeMetric.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

        }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of DischargeMetric by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all DischargeMetric by EncounterID.</returns>
        public async Task<IEnumerable<DischargeMetric>> GetDischargeMetricByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.DischargeMetrics.Where(p => p.IsDeleted == false && p.EncounterId == EncounterID).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         dischargeMetric => dischargeMetric.EncounterId,
         encounter => encounter.Oid,
         (dischargeMetric, encounter) => new DischargeMetric
         {
             EncounterId = dischargeMetric.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             ClientId = dischargeMetric.ClientId,
             DateCreated = dischargeMetric.DateCreated,
             CreatedBy = dischargeMetric.CreatedBy,
             CreatedIn = dischargeMetric.CreatedIn,
             ChestCircumference = dischargeMetric.ChestCircumference,
             EncounterType = dischargeMetric.EncounterType,
             DateModified = dischargeMetric.DateModified,
             ApgarScore = dischargeMetric.ApgarScore,
             BodyLength = dischargeMetric.BodyLength,
             HeadCircumference = dischargeMetric.HeadCircumference,
             InteractionId = dischargeMetric.InteractionId,
             IsDeleted = dischargeMetric.IsDeleted,
             IsSynced = dischargeMetric.IsSynced,
             ModifiedBy = dischargeMetric.ModifiedBy,
             ModifiedIn = dischargeMetric.ModifiedIn,
             PerinatalProblems = dischargeMetric.PerinatalProblems,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == dischargeMetric.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == dischargeMetric.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

         }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}