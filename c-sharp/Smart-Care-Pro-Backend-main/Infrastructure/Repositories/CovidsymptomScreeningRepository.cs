using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

/*
 * Created by    : Shakil
 * Date created  : 18.02.2023
 * Modified by   : Shakil
 * Last modified : 05.06.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Repositories
{
    public class CovidsymptomScreeningRepository : Repository<CovidSymptomScreening>, ICovidSymptomScreeningRepository
    {
        public CovidsymptomScreeningRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a covid symptom screening by key.
        /// </summary>
        /// <param name="key">Primary key of the table CovidsymptomScreenings.</param>
        /// <returns>Returns a covid if the key is matched.</returns>
        public async Task<CovidSymptomScreening> GetCovidSymptomScreeningByKey(Guid key)
        {
            try
            {
                var covidSymptomScreening = await context.CovidSymptomScreenings.AsNoTracking()
                    .FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);

                if (covidSymptomScreening != null)
                {
                    covidSymptomScreening.ClinicianName = await context.UserAccounts.Where(x => x.Oid == covidSymptomScreening.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    covidSymptomScreening.FacilityName = await context.Facilities.Where(x => x.Oid == covidSymptomScreening.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    covidSymptomScreening.EncounterDate = await context.Encounters.Where(x => x.Oid == covidSymptomScreening.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                }
                return covidSymptomScreening;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get Covid Semptom screening to by CovidId.
        /// </summary>
        /// <param name="HTSID">CovidId of Covid semptom screening to.</param>
        /// <returns>Returns Covid semptom screening to if the covidId is matched.</returns>
        public async Task<IEnumerable<CovidSymptomScreening>> GetCovidSymptomScreeenByCovid(Guid CovidId)
        {
            try
            {
                return await context.CovidSymptomScreenings.AsNoTracking()
                  .Where(h => h.IsDeleted == false && h.CovidId == CovidId)
                  .Join(
                        context.Encounters.AsNoTracking(),
                        covidSymptomScreenings => covidSymptomScreenings.EncounterId,
                        encounter => encounter.Oid,
                        (covidSymptomScreenings, encounter) => new CovidSymptomScreening
                        {
                            // Include properties from EncounterBaseModel
                            EncounterId = covidSymptomScreenings.EncounterId,
                            EncounterType = covidSymptomScreenings.EncounterType,
                            CreatedIn = covidSymptomScreenings.CreatedIn,
                            DateCreated = covidSymptomScreenings.DateCreated,
                            CreatedBy = covidSymptomScreenings.CreatedBy,
                            ModifiedIn = covidSymptomScreenings.ModifiedIn,
                            DateModified = covidSymptomScreenings.DateModified,
                            ModifiedBy = covidSymptomScreenings.ModifiedBy,
                            IsDeleted = covidSymptomScreenings.IsDeleted,
                            IsSynced = covidSymptomScreenings.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            // Include properties from CovidSymptomScreening entity
                            InteractionId = covidSymptomScreenings.InteractionId,
                            CovidSymptomId = covidSymptomScreenings.CovidSymptomId,
                            CovidSymptom = covidSymptomScreenings.CovidSymptom,  // Navigation property
                            CovidId = covidSymptomScreenings.CovidId,
                            Covid = covidSymptomScreenings.Covid,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == covidSymptomScreenings.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == covidSymptomScreenings.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        })
                  .OrderByDescending(x => x.EncounterDate)
                  .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of covidsymptomScreenings.
        /// </summary>
        /// <returns>Returns a list of all covidsymptomScreenings.</returns>
        public async Task<IEnumerable<CovidSymptomScreening>> GetCovidSymptomScreenings()
        {
            try
            {
                return await context.CovidSymptomScreenings.AsNoTracking()
                  .Where(h => h.IsDeleted == false)
                  .Join(
                        context.Encounters.AsNoTracking(),
                        covidSymptomScreenings => covidSymptomScreenings.EncounterId,
                        encounter => encounter.Oid,
                        (covidSymptomScreenings, encounter) => new CovidSymptomScreening
                        {
                            // Include properties from EncounterBaseModel
                            EncounterId = covidSymptomScreenings.EncounterId,
                            EncounterType = covidSymptomScreenings.EncounterType,
                            CreatedIn = covidSymptomScreenings.CreatedIn,
                            DateCreated = covidSymptomScreenings.DateCreated,
                            CreatedBy = covidSymptomScreenings.CreatedBy,
                            ModifiedIn = covidSymptomScreenings.ModifiedIn,
                            DateModified = covidSymptomScreenings.DateModified,
                            ModifiedBy = covidSymptomScreenings.ModifiedBy,
                            IsDeleted = covidSymptomScreenings.IsDeleted,
                            IsSynced = covidSymptomScreenings.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,

                            InteractionId = covidSymptomScreenings.InteractionId,
                            CovidSymptomId = covidSymptomScreenings.CovidSymptomId,
                            CovidSymptom = covidSymptomScreenings.CovidSymptom,  // Navigation property
                            CovidId = covidSymptomScreenings.CovidId,
                            Covid = covidSymptomScreenings.Covid,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == covidSymptomScreenings.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == covidSymptomScreenings.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        })
                      .OrderByDescending(x => x.EncounterDate)
                  .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}