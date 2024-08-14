using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Bithy
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class TakenARTDrugRepository : Repository<TakenARTDrug>, ITakenARTDrugRepository
    {
        public TakenARTDrugRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get takenARTDrug by key.
        /// </summary>
        /// <param name="key">Primary key of the table TakenARTDrugs.</param>
        /// <returns>Returns a takenARTDrug if the key is matched.</returns>
        public async Task<TakenARTDrug> GetTakenARTDrugByKey(Guid key)
        {
            try
            {
                var takenARTDrug = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (takenARTDrug != null)
                    takenARTDrug.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return takenARTDrug;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of takenARTDrugs.
        /// </summary>
        /// <returns>Returns a list of all takenARTDrugs.</returns>
        public async Task<IEnumerable<TakenARTDrug>> GetTakenARTDrugs()
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
        /// The method is used to a get takenARTDrug by encounterID.
        /// </summary>
        /// <param name="key">Primary key of the table TakenARTDrugs.</param>
        /// <returns>Returns a takenARTDrug if the key is matched.</returns>
        public async Task<IEnumerable<TakenARTDrug>> GetTakenARTDrugByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.TakenARTDrugs.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
         .Join(
             context.Encounters.AsNoTracking(),
             takenARTDrug => takenARTDrug.EncounterId,
             encounter => encounter.Oid,
             (takenARTDrug, encounter) => new TakenARTDrug
             {
                 EncounterId = takenARTDrug.EncounterId,
                 EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                 DateCreated = takenARTDrug.DateCreated,
                 ARTDrugId = takenARTDrug.ARTDrugId,
                 CreatedIn = takenARTDrug.CreatedIn,
                 CreatedBy = takenARTDrug.CreatedBy,
                 DateModified = takenARTDrug.DateModified,
                 EncounterType = takenARTDrug.EncounterType,
                 EndDate = takenARTDrug.EndDate,
                 InteractionId = takenARTDrug.InteractionId,
                 IsDeleted = takenARTDrug.IsDeleted,
                 IsSynced = takenARTDrug.IsSynced,
                 ModifiedBy = takenARTDrug.ModifiedBy,
                 ModifiedIn = takenARTDrug.ModifiedIn,
                 PriorExposureId = takenARTDrug.PriorExposureId,
                 StartDate = takenARTDrug.StartDate,
                 StoppingReason = takenARTDrug.StoppingReason,

             }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}