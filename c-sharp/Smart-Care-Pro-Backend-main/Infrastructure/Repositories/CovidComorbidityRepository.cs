using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

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
    public class CovidComorbidityRepository : Repository<CovidComorbidity>, ICovidComorbidityRepository
    {
        public CovidComorbidityRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of covidComorbidity.
        /// </summary>
        /// <returns>Returns a list of all covid comorbidities.</returns>
        public async Task<IEnumerable<CovidComorbidity>> GetCovidComorbidities()
        {
            try
            {
                return await context.CovidComorbidities.AsNoTracking().Where(x => x.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        covidComorbidity => covidComorbidity.EncounterId,
                        encounter => encounter.Oid,
                        (covidComorbidity, encounter) => new CovidComorbidity
                        {
                            EncounterId = covidComorbidity.EncounterId,
                            EncounterType = covidComorbidity.EncounterType,
                            CreatedIn = covidComorbidity.CreatedIn,
                            DateCreated = covidComorbidity.DateCreated,
                            CreatedBy = covidComorbidity.CreatedBy,
                            ModifiedIn = covidComorbidity.ModifiedIn,
                            DateModified = covidComorbidity.DateModified,
                            ModifiedBy = covidComorbidity.ModifiedBy,
                            IsDeleted = covidComorbidity.IsDeleted,
                            IsSynced = covidComorbidity.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            // Additional properties from CovidComorbidity entity
                            InteractionId = covidComorbidity.InteractionId,
                            CovidComorbidityConditions = covidComorbidity.CovidComorbidityConditions,
                            CovidId = covidComorbidity.CovidId,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == covidComorbidity.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == covidComorbidity.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        }
                    )
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get Covid Comorbidity to by CovidId.
        /// </summary>
        /// <param name="CovidId">CovidId of Covid comorbidity to.</param>
        /// <returns>Returns Covid comorbidity to if the covidId is matched.</returns>
        public async Task<IEnumerable<CovidComorbidity>> GetCovidComorbidityByCovid(Guid CovidID)
        {
            try
            {

                return await context.CovidComorbidities.AsNoTracking().Where(x => x.IsDeleted == false && x.CovidId == CovidID)
                  .Join(
                      context.Encounters.AsNoTracking(),
                      covidcomorbidity => covidcomorbidity.EncounterId,
                      encounter => encounter.Oid,
                      (covidcomorbidity, encounter) => new CovidComorbidity
                      {
                          EncounterId = covidcomorbidity.EncounterId,
                          EncounterType = covidcomorbidity.EncounterType,
                          CreatedIn = covidcomorbidity.CreatedIn,
                          DateCreated = covidcomorbidity.DateCreated,
                          CreatedBy = covidcomorbidity.CreatedBy,
                          ModifiedIn = covidcomorbidity.ModifiedIn,
                          DateModified = covidcomorbidity.DateModified,
                          ModifiedBy = covidcomorbidity.ModifiedBy,
                          IsDeleted = covidcomorbidity.IsDeleted,
                          IsSynced = covidcomorbidity.IsSynced,
                          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                          InteractionId = covidcomorbidity.InteractionId,
                          CovidComorbidityConditions = covidcomorbidity.CovidComorbidityConditions,
                          CovidId = covidcomorbidity.CovidId,
                          ClinicianName = context.UserAccounts.Where(x => x.Oid == covidcomorbidity.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                          FacilityName = context.Facilities.Where(x => x.Oid == covidcomorbidity.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                      }
                  )
                  .OrderByDescending(x => x.EncounterDate)
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
        public async Task<CovidComorbidity> GetCovidComobidityByKey(Guid key)
        {
            try
            {
                var covidComorbidity = await context.CovidComorbidities.AsNoTracking().FirstOrDefaultAsync(x => x.IsDeleted == false && x.InteractionId == key);

                if (covidComorbidity != null)
                {
                    covidComorbidity.ClinicianName = await context.UserAccounts.Where(x => x.Oid == covidComorbidity.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    covidComorbidity.FacilityName = await context.Facilities.Where(x => x.Oid == covidComorbidity.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    covidComorbidity.EncounterDate = await context.Encounters.Where(x => x.Oid == covidComorbidity.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }
                return covidComorbidity;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
