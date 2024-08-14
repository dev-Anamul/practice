using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Rezwana
 * Date created : 19.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IExposureRepository interface.
    /// </summary>
    public class ExposureRepository : Repository<Exposure>, IExposureRepository
    {
        public ExposureRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an exposure by key.
        /// </summary>
        /// <param name="key">Primary key of the table Exposures.</param>
        /// <returns>Returns an exposure if the key is matched.</returns>
        public async Task<Exposure> GetExposureByKey(Guid key)
        {
            try
            {
                var exposure = await FirstOrDefaultAsync(e => e.Oid == key && e.IsDeleted == false);

                if (exposure != null)
                {
                    exposure.ClinicianName = await context.UserAccounts.Where(x => x.Oid == exposure.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    exposure.FacilityName = await context.Facilities.Where(x => x.Oid == exposure.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    exposure.EncounterDate = await context.Encounters.Where(x => x.Oid == exposure.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return exposure;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of exposures.
        /// </summary>
        /// <returns>Returns a list of all exposures.</returns>
        public async Task<IEnumerable<Exposure>> GetExposures()
        {
            try
            {
                return await QueryAsync(e => e.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Exposure>> GetExposureByID(Guid ChiefComplaintID)
        {
            try
            {
                return await context.Exposures.Where(p => p.IsDeleted == false && p.ChiefComplaintId == ChiefComplaintID).AsNoTracking()
           .Join(
               context.Encounters.AsNoTracking(),
              exposure => exposure.EncounterId,
               encounter => encounter.Oid,
               (exposure, encounter) => new Exposure
               {
                   EncounterId = exposure.EncounterId,
                   EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                   ChiefComplaintId = exposure.ChiefComplaintId,
                   ModifiedIn = exposure.ModifiedIn,
                   CreatedBy = exposure.CreatedBy,
                   CreatedIn = exposure.CreatedIn,
                   DateCreated = exposure.DateCreated,
                   DateModified = exposure.DateModified,
                   EncounterType = exposure.EncounterType,
                   ExposureType = exposure.ExposureType,
                   ExposureTypeId = exposure.ExposureTypeId,
                   IsDeleted = exposure.IsDeleted,
                   IsSynced = exposure.IsSynced,
                   ModifiedBy = exposure.ModifiedBy,
                   Oid = exposure.Oid,
                   ClinicianName = context.UserAccounts.Where(x => x.Oid == exposure.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                   FacilityName = context.Facilities.Where(x => x.Oid == exposure.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


               }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}