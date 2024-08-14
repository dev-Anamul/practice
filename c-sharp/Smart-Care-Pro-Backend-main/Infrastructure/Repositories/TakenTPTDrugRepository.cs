using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Bithy
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class TakenTPTDrugRepository : Repository<TakenTPTDrug>, ITakenTPTDrugRepository
    {
        public TakenTPTDrugRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get takenTPTDrug by key.
        /// </summary>
        /// <param name="key">Primary key of the table TakenTPTDrugs.</param>
        /// <returns>Returns a takenTPTDrug if the key is matched.</returns>
        public async Task<TakenTPTDrug> GetTakenTPTDrugByKey(Guid key)
        {
            try
            {
                var takenTPTDrug = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (takenTPTDrug != null)
                    takenTPTDrug.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return takenTPTDrug;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of takenTPTDrug.
        /// </summary>
        /// <returns>Returns a list of all takenTPTDrugs.</returns>
        public async Task<IEnumerable<TakenTPTDrug>> GetTakenTPTDrugs()
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

        /// <summary>
        /// The method is used to a get takenTPTDrug by encounterID.
        /// </summary>
        /// <param name="key">Primary key of the table TakenTPTDrugs.</param>
        /// <returns>Returns a takenTPTDrug if the key is matched.</returns>
        public async Task<IEnumerable<TakenTPTDrug>> GetTakenTPTDrugByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.TakenTPTDrugs.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
      .Join(
          context.Encounters.AsNoTracking(),
          takenTPTDrug => takenTPTDrug.EncounterId,
          encounter => encounter.Oid,
          (takenTPTDrug, encounter) => new TakenTPTDrug
          {
              EncounterId = takenTPTDrug.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              CreatedBy = takenTPTDrug.CreatedBy,
              CreatedIn = takenTPTDrug.CreatedIn,
              DateCreated = takenTPTDrug.DateCreated,
              DateModified = takenTPTDrug.DateModified,
              EncounterType = takenTPTDrug.EncounterType,
              InteractionId = takenTPTDrug.InteractionId,
              IsDeleted = takenTPTDrug.IsDeleted,
              IsSynced = takenTPTDrug.IsSynced,
              ModifiedBy = takenTPTDrug.ModifiedBy,
              ModifiedIn = takenTPTDrug.ModifiedIn,

          }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}