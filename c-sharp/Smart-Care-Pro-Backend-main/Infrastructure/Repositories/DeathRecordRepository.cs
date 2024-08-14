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
    public class DeathRecordRepository : Repository<DeathRecord>, IDeathRecordRepository
    {
        /// <summary>
        /// Implementation of IDeathRecordRepository interface.
        /// </summary>
        public DeathRecordRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get death record by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Returns a death record if the key is matched.</returns>
        public async Task<DeathRecord> GetDeathRecordByKey(Guid key)
        {
            try
            {
                var deathRecord = await LoadWithChildAsync<DeathRecord>(d => d.InteractionId == key && d.IsDeleted == false, d => d.District, d => d.District.Provinces, c => c.Client, cd => cd.DeathCause);

                if (deathRecord != null)
                {
                    deathRecord.DeathCause = context.DeathCauses.Include(x => x.ICDDiagnosis).Include(x => x.ICPC2Description).ThenInclude(x => x.AnatomicAxis).Include(x => x.ICPC2Description.PathologyAxis).Where(x => x.DeathRecordId == key).ToList();
                    deathRecord.ClinicianName = await context.UserAccounts.Where(x => x.Oid == deathRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    deathRecord.FacilityName = await context.Facilities.Where(x => x.Oid == deathRecord.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    deathRecord.EncounterDate = await context.Encounters.Where(x => x.Oid == deathRecord.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }
                //  return await LoadListWithChildAsync<DeathCause>(d => d.IsDeleted == false && d.DeathRecordId == deathRecordId, x => x.ICPC2Description, x => x.ICPC2Description.AnatomicAxis, x => x.ICPC2Description.PathologyAxis, x => x.ICDDiagnosis);

                return deathRecord;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a death record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a death record if the ClientID is matched.</returns>
        public async Task<DeathRecord> GetDeathRecordByClient(Guid clientId)
        {
            try
            {
                var deathRecord = await LoadWithChildAsync<DeathRecord>(d => d.ClientId == clientId && d.IsDeleted == false, d => d.Client, d => d.District, d => d.District.Provinces);
               
                if (deathRecord != null)
                {
                    deathRecord.ClinicianName = await context.UserAccounts.Where(x => x.Oid == deathRecord.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    deathRecord.FacilityName = await context.Facilities.Where(x => x.Oid == deathRecord.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    deathRecord.EncounterDate = await context.Encounters.Where(x => x.Oid == deathRecord.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                }

                return deathRecord;
            }
            catch (Exception)
            {

                throw;
            }
        }

        ///// <summary>
        ///// The method is used to get a death record by ClientID.
        ///// </summary>
        ///// <param name="ClientID"></param>
        ///// <returns>Returns a death record if the ClientID is matched.</returns>
        //public async Task<IEnumerable<DeathRecord>> GetDeathRecordByClient(Guid ClientID)
        //{
        //    try
        //    {
        //        return await QueryAsync(b => b.IsDeleted == false && b.ClientID == ClientID);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public async Task<IEnumerable<DeathRecord>> GetDeathRecords()
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

        public async Task<IEnumerable<DeathRecord>> GetDeathRecordByEncounterID(Guid encounterId)
        {
            try
            {
                return await QueryAsync(d => d.IsDeleted == false && d.EncounterId == encounterId);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}