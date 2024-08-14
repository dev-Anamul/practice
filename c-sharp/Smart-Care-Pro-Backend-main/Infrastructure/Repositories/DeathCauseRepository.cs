using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DeathCauseRepository : Repository<DeathCause>, IDeathCauseRepository
    {
        public DeathCauseRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a death cause by key.
        /// </summary>
        /// <param name="key">Primary key of the table DeathCauses.</param>
        /// <returns>Returns a death cause if the key is matched.</returns>
        public async Task<DeathCause> GetDeathCauseByKey(Guid key)
        {
            try
            {
                var deathCause = await FirstOrDefaultAsync(d => d.InteractionId == key && d.IsDeleted == false);

                if (deathCause != null)
                {
                    deathCause.ClinicianName = await context.UserAccounts.Where(x => x.Oid == deathCause.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    deathCause.FacilityName = await context.Facilities.Where(x => x.Oid == deathCause.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    deathCause.EncounterDate = await context.Encounters.Where(x => x.Oid == deathCause.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                }
                return deathCause;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of death causes.
        /// </summary>
        /// <returns>Returns a list of all death causes.</returns>
        public async Task<IEnumerable<DeathCause>> GetDeathCauses()
        {
            try
            {
                return await QueryAsync(d => d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a death cause by deathRecordId.
        /// </summary>
        /// <param name="deathRecordId"></param>
        /// <returns>Returns a death cause if the deathRecordId is matched.</returns>
        public async Task<IEnumerable<DeathCause>> GetDeathCauseByDeathRecordID(Guid deathRecordId)
        {
            try
            {
                return await LoadListWithChildAsync<DeathCause>(d => d.IsDeleted == false && d.DeathRecordId == deathRecordId, x => x.ICPC2Description, x => x.ICPC2Description.AnatomicAxis, x => x.ICPC2Description.PathologyAxis, x => x.ICDDiagnosis);
            }
            catch (Exception)
            {
                throw;
            }

        }



    }
}