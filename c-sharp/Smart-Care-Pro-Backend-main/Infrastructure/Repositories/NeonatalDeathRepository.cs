using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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
    public class NeonatalDeathRepository : Repository<NeonatalDeath>, INeonatalDeathRepository
    {
        /// <summary>
        /// Implementation of INeonatalDeathRepository interface.
        /// </summary>
        public NeonatalDeathRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a NeonatalDeath by key.
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalDeaths.</param>
        /// <returns>Returns a NeonatalDeath if the key is matched.</returns>
        public async Task<NeonatalDeath> GetNeonatalDeathByKey(Guid key)
        {
            try
            {
                var neonatalDeath = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (neonatalDeath != null)
                    neonatalDeath.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return neonatalDeath;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of NeonatalDeath.
        /// </summary>
        /// <returns>Returns a list of all NeonatalDeaths.</returns>
        public async Task<IEnumerable<NeonatalDeath>> GetNeonatalDeaths()
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
        /// The method is used to get a NeonatalDeath by CauseOfNeonatalDeathId.
        /// </summary>
        /// <param name="causeOfNeonatalDeathId"></param>
        /// <returns>Returns a NeonatalDeath if the CauseOfNeonatalDeathID is matched.</returns>
        public async Task<IEnumerable<NeonatalDeath>> GetNeonatalDeathByCauseOfNeonatalDeath(int causeOfNeonatalDeathId)
        {
            try
            {
                return await context.NeonatalDeaths.AsNoTracking().Where(p => p.IsDeleted == false && p.CauseOfNeonatalDeathId == causeOfNeonatalDeathId)
      .Join(
          context.Encounters.AsNoTracking(),
          neonatalDeath => neonatalDeath.EncounterId,
          encounter => encounter.Oid,
          (neonatalDeath, encounter) => new NeonatalDeath
          {
              EncounterId = neonatalDeath.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              AgeAtTimeOfDeath = neonatalDeath.AgeAtTimeOfDeath,
              CauseOfNeonatalDeathId = neonatalDeath.CauseOfNeonatalDeathId,
              Comments = neonatalDeath.Comments,
              CreatedBy = neonatalDeath.CreatedBy,
              CreatedIn = neonatalDeath.CreatedIn,
              DateCreated = neonatalDeath.DateCreated,
              DateModified = neonatalDeath.DateModified,
              EncounterType = neonatalDeath.EncounterType,
              InteractionId = neonatalDeath.InteractionId,
              IsDeleted = neonatalDeath.IsDeleted,
              IsSynced = neonatalDeath.IsSynced,
              ModifiedBy = neonatalDeath.ModifiedBy,
              ModifiedIn = neonatalDeath.ModifiedIn,
              NeonatalId = neonatalDeath.NeonatalId,
              Other = neonatalDeath.Other,
              TimeOfDeath = neonatalDeath.TimeOfDeath,
              TimeUnit = neonatalDeath.TimeUnit,

          }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a NeonatalDeath by NeonatalId.
        /// </summary>
        /// <param name="neonatalId"></param>
        /// <returns>Returns a NeonatalDeath if the NeonatalId is matched.</returns>
        public async Task<IEnumerable<NeonatalDeath>> GetNeonatalDeathByNeonatal(Guid neonatalId)
        {
            try
            {
                return await context.NeonatalDeaths.Include(x => x.CauseOfNeonatalDeath).AsNoTracking().Where(p => p.IsDeleted == false && p.NeonatalId == neonatalId)
    .Join(
        context.Encounters.AsNoTracking(),
        neonatalDeath => neonatalDeath.EncounterId,
        encounter => encounter.Oid,
        (neonatalDeath, encounter) => new NeonatalDeath
        {
            EncounterId = neonatalDeath.EncounterId,
            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
            AgeAtTimeOfDeath = neonatalDeath.AgeAtTimeOfDeath,
            CauseOfNeonatalDeathId = neonatalDeath.CauseOfNeonatalDeathId,
            CauseOfNeonatalDeath = neonatalDeath.CauseOfNeonatalDeath,
            Comments = neonatalDeath.Comments,
            CreatedBy = neonatalDeath.CreatedBy,
            CreatedIn = neonatalDeath.CreatedIn,
            DateCreated = neonatalDeath.DateCreated,
            DateModified = neonatalDeath.DateModified,
            EncounterType = neonatalDeath.EncounterType,
            InteractionId = neonatalDeath.InteractionId,
            IsDeleted = neonatalDeath.IsDeleted,
            IsSynced = neonatalDeath.IsSynced,
            ModifiedBy = neonatalDeath.ModifiedBy,
            ModifiedIn = neonatalDeath.ModifiedIn,
            NeonatalId = neonatalDeath.NeonatalId,
            Other = neonatalDeath.Other,
            TimeOfDeath = neonatalDeath.TimeOfDeath,
            TimeUnit = neonatalDeath.TimeUnit,

        }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of NeonatalDeath by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all NeonatalDeath by EncounterID.</returns>
        public async Task<IEnumerable<NeonatalDeath>> GetNeonatalDeathByEncounter(Guid encounterId)
        {
            try
            {
                return await context.NeonatalDeaths.Include(x => x.CauseOfNeonatalDeath).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId)
    .Join(
        context.Encounters.AsNoTracking(),
        neonatalDeath => neonatalDeath.EncounterId,
        encounter => encounter.Oid,
        (neonatalDeath, encounter) => new NeonatalDeath
        {
            EncounterId = neonatalDeath.EncounterId,
            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
            AgeAtTimeOfDeath = neonatalDeath.AgeAtTimeOfDeath,
            CauseOfNeonatalDeathId = neonatalDeath.CauseOfNeonatalDeathId,
            CauseOfNeonatalDeath = neonatalDeath.CauseOfNeonatalDeath,
            Comments = neonatalDeath.Comments,
            CreatedBy = neonatalDeath.CreatedBy,
            CreatedIn = neonatalDeath.CreatedIn,
            DateCreated = neonatalDeath.DateCreated,
            DateModified = neonatalDeath.DateModified,
            EncounterType = neonatalDeath.EncounterType,
            InteractionId = neonatalDeath.InteractionId,
            IsDeleted = neonatalDeath.IsDeleted,
            IsSynced = neonatalDeath.IsSynced,
            ModifiedBy = neonatalDeath.ModifiedBy,
            ModifiedIn = neonatalDeath.ModifiedIn,
            NeonatalId = neonatalDeath.NeonatalId,
            Other = neonatalDeath.Other,
            TimeOfDeath = neonatalDeath.TimeOfDeath,
            TimeUnit = neonatalDeath.TimeUnit,

        }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}