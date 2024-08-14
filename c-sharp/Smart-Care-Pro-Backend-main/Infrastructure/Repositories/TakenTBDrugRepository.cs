using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Rezwana
 * Date created : 01.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ITakenTBDrugRepository interface.
    /// </summary>
    public class TakenTBDrugRepository : Repository<TakenTBDrug>, ITakenTBDrugRepository
    {
        public TakenTBDrugRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a TakenTBDrug by key.
        /// </summary>
        /// <param name="key">Primary key of the table TakenTBDrugs.</param>
        /// <returns>Returns a TakenTBDrug if the key is matched.</returns>
        public async Task<TakenTBDrug> GetTakenTBDrugByKey(Guid key)
        {
            try
            {
                var takenTBDrug = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (takenTBDrug != null)
                    takenTBDrug.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return takenTBDrug;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of TakenTBDrugs.
        /// </summary>
        /// <returns>Returns a list of all TakenTBDrugs.</returns>
        public async Task<IEnumerable<TakenTBDrug>> GetTakenTBDrugs()
        {
            try
            {
                return await QueryAsync(t => t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a TakenTBDrug by EncounterID.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a TakenTBDrug if the EncounterID is matched.</returns>
        public async Task<IEnumerable<TakenTBDrug>> GetTakenTBDrugByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.TakenTBDrugs.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
         .Join(
             context.Encounters.AsNoTracking(),
             takenTBDrug => takenTBDrug.EncounterId,
             encounter => encounter.Oid,
             (takenTBDrug, encounter) => new TakenTBDrug
             {
                 EncounterId = takenTBDrug.EncounterId,
                 EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                 IsDeleted = takenTBDrug.IsDeleted,
                 ModifiedIn = takenTBDrug.ModifiedIn,
                 ModifiedBy = takenTBDrug.ModifiedBy,
                 IsSynced = takenTBDrug.IsSynced,
                 InteractionId = takenTBDrug.InteractionId,
                 EncounterType = takenTBDrug.EncounterType,
                 DateModified = takenTBDrug.DateModified,
                 CreatedBy = takenTBDrug.CreatedBy,
                 CreatedIn = takenTBDrug.CreatedIn,
                 DateCreated = takenTBDrug.DateCreated,
                 TBDrugId = takenTBDrug.TBDrugId,
                 TBHistoryId = takenTBDrug.TBHistoryId,

             }).OrderByDescending(x => x.EncounterDate).ToListAsync();
        
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}