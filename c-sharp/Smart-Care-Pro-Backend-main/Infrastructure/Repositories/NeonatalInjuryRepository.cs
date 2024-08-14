using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

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
    public class NeonatalInjuryRepository : Repository<NeonatalInjury>, INeonatalInjuryRepository
    {
        /// <summary>
        /// Implementation of INeonatalInjuryRepository interface.
        /// </summary>
        public NeonatalInjuryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a NeonatalInjury by key.
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalInjuries.</param>
        /// <returns>Returns a NeonatalInjury if the key is matched.</returns>
        public async Task<NeonatalInjury> GetNeonatalInjuryByKey(Guid key)
        {
            try
            {
                var neonatalInjury = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (neonatalInjury != null)
                    neonatalInjury.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return neonatalInjury;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of NeonatalInjury.
        /// </summary>
        /// <returns>Returns a list of all NeonatalInjuries.</returns>
        public async Task<IEnumerable<NeonatalInjury>> GetNeonatalInjuries()
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
        /// The method is used to get a NeonatalInjury by NeonatalId.
        /// </summary>
        /// <param name="neonatalId"></param>
        /// <returns>Returns a NeonatalInjury if the NeonatalId is matched.</returns>
        public async Task<IEnumerable<NeonatalInjury>> GetNeonatalInjuryByNeonatal(Guid neonatalId)
        {
            try
            {
                return await context.NeonatalInjuries.AsNoTracking().Where(p => p.IsDeleted == false && p.NeonatalId == neonatalId)
      .Join(
          context.Encounters.AsNoTracking(),
          neonatalDeath => neonatalDeath.EncounterId,
          encounter => encounter.Oid,
          (neonatalDeath, encounter) => new NeonatalInjury
          {
              EncounterId = neonatalDeath.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              InteractionId = neonatalDeath.InteractionId,
              Other = neonatalDeath.Other,
              CreatedBy = neonatalDeath.CreatedBy,
              CreatedIn = neonatalDeath.CreatedIn,
              DateCreated = neonatalDeath.DateCreated,
              DateModified = neonatalDeath.DateModified,
              EncounterType = neonatalDeath.EncounterType,
              Injuries = neonatalDeath.Injuries,
              IsDeleted = neonatalDeath.IsDeleted,
              IsSynced = neonatalDeath.IsSynced,
              ModifiedBy = neonatalDeath.ModifiedBy,
              ModifiedIn = neonatalDeath.ModifiedIn,
              NeonatalId = neonatalDeath.NeonatalId,
              InjuriesList = context.NeonatalInjuries.Where(x => x.NeonatalId == neonatalDeath.InteractionId).Select(x=>x.Injuries.Value).ToArray() 

          }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of NeonatalInjury by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all NeonatalInjury by EncounterID.</returns>
        public async Task<IEnumerable<NeonatalInjury>> GetNeonatalInjuryByEncounter(Guid encounterId)
        {
            try
            {
                return await context.NeonatalInjuries.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId)
      .Join(
          context.Encounters.AsNoTracking(),
          neonatalDeath => neonatalDeath.EncounterId,
          encounter => encounter.Oid,
          (neonatalDeath, encounter) => new NeonatalInjury
          {
              EncounterId = neonatalDeath.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              InteractionId = neonatalDeath.InteractionId,
              Other = neonatalDeath.Other,
              CreatedBy = neonatalDeath.CreatedBy,
              CreatedIn = neonatalDeath.CreatedIn,
              DateCreated = neonatalDeath.DateCreated,
              DateModified = neonatalDeath.DateModified,
              EncounterType = neonatalDeath.EncounterType,
              Injuries = neonatalDeath.Injuries,
              IsDeleted = neonatalDeath.IsDeleted,
              IsSynced = neonatalDeath.IsSynced,
              ModifiedBy = neonatalDeath.ModifiedBy,
              ModifiedIn = neonatalDeath.ModifiedIn,
              NeonatalId = neonatalDeath.NeonatalId,

          }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}