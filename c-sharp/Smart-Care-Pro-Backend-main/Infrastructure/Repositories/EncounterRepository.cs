using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Lion
 * Date created : 12.09.2022
 * Modified by  : Bella
 * Last modified: 11.11.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IOPDVisitRepository interface.
    /// </summary>
    public class EncounterRepository : Repository<Encounter>, IEncounterRepository
    {
        public EncounterRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an opd visit by key.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisits.</param>
        /// <returns>Returns an opd visit if the key is matched.</returns>
        public async Task<Encounter> GetEncounterByKey(Guid key)
        {
            try
            {
                return await LoadWithChildAsync<Encounter>(o => o.Oid == key && o.IsDeleted == false, c => c.Client, b => b.Bed, w => w.Bed.Ward, f => f.Bed.Ward.Firm, d => d.Bed.Ward.Firm.Department);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Encounter> GetEncounterByClient(Guid key)
        {
            try
            {
                var data = await context.Encounters.Where(x => x.ClientId == key && x.IsDeleted == false && x.BedId != null).OrderByDescending(x => x.DateCreated).ThenByDescending(x => x.IPDAdmissionDate).FirstOrDefaultAsync();
                return await GetEncounterByKey(data.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the IPD Admission if client is not discharge.
        /// </summary>
        public async Task<Encounter> GetIPDAdmissionByClient(Guid clientId)
        {
            try
            {
                 var data = await context.Encounters.Where(x => x.ClientId == clientId && x.IsDeleted == false && x.IPDAdmissionDate != null && x.IPDDischargeDate == null).Include(x => x.Client).Include(x => x.Client.HomeLanguage).Include(x => x.Client.Occupation).Include(x => x.Client.EducationLevel)
                    .Include(x => x.Client.Country).Include(x => x.Client.District).AsNoTracking().Include(x=>x.Bed).Include(x => x.Bed.Ward).Include(x => x.Bed.Ward.Firm)
                    .Include(x => x.Bed.Ward.Firm.Department).OrderByDescending(x => x.DateCreated).ThenByDescending(x => x.IPDAdmissionDate).FirstOrDefaultAsync();

                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Encounter>> ReadEncounterListByClient(Guid clientId)
        {
            try
            {
                var encounters = await context.Encounters
                    .Include(x => x.Client).Include(x => x.Bed).ThenInclude(b => b.Ward).ThenInclude(w => w.Firm).ThenInclude(f => f.Department)
                    .Where(x => x.ClientId == clientId && x.IsDeleted == false)
                    .OrderByDescending(x => x.DateCreated).ThenByDescending(x => x.IPDAdmissionDate)
                    .ToListAsync();

                return encounters;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of Admissions of Client.
        /// </summary>
        /// <returns>Returns a list of all Admissions of Client.</returns>
        public async Task<IEnumerable<Encounter>> ReadAdmissionsByClient(Guid clientId)
        {
            try
            {
                var encounters = await context.Encounters
                    .Include(x => x.Client).Include(x => x.Bed).ThenInclude(b => b.Ward).ThenInclude(w => w.Firm).ThenInclude(f => f.Department)
                    .Where(x => x.ClientId == clientId && x.IsDeleted == false && x.IPDAdmissionDate != null)
                    .OrderByDescending(x => x.DateCreated).ThenByDescending(x => x.IPDAdmissionDate)
                    .ToListAsync();

                return encounters;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of opd visits.
        /// </summary>
        /// <returns>Returns a list of all opd visits.</returns>
        public async Task<IEnumerable<Encounter>> GetEncounters()
        {
            try
            {
                return await QueryAsync(o => o.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Encounter> GetEncounterByDate(DateTime? historicalDate)
        {
            try
            {
                var historicalVisit = await context.Encounters.Where(x => x.OPDVisitDate == historicalDate && x.IsDeleted == false).FirstOrDefaultAsync();

                if (historicalVisit == null)
                {
                    return historicalVisit;
                }

                return await GetEncounterByKey(historicalVisit.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}