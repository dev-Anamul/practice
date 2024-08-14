using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Rezwana
 * Date created : 25.12.2022
 * Modified by  : Rezwana
 * Last modified: 27.12.2022
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class ContraceptiveHistoryRepository : Repository<ContraceptiveHistory>, IContraceptiveHistoryRepository
    {
        /// <summary>
        /// Implementation of IContraceptiveHistoryRepository interface.
        /// </summary>
        public ContraceptiveHistoryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a contraceptive history by key.
        /// </summary>
        /// <param name="key">Primary key of the table ContraceptiveHistories.</param>
        /// <returns>Returns a contraceptive histories if the key is matched.</returns>
        public async Task<ContraceptiveHistory> GetContraceptiveHistoryByKey(Guid key)
        {
            try
            {
                var contraceptiveHistory = await context.ContraceptiveHistories.AsNoTracking().FirstOrDefaultAsync(c => c.InteractionId == key && c.IsDeleted == false);

                if (contraceptiveHistory != null)
                {
                    contraceptiveHistory.ClinicianName = await context.UserAccounts.Where(x => x.Oid == contraceptiveHistory.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    contraceptiveHistory.FacilityName = await context.Facilities.Where(x => x.Oid == contraceptiveHistory.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    contraceptiveHistory.EncounterDate = await context.Encounters.Where(x => x.Oid == contraceptiveHistory.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }
                return contraceptiveHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get a contraceptive history by key.
        /// </summary>
        /// <param name="key">Primary key of the table ContraceptiveHistories.</param>
        /// <returns>Returns a contraceptive histories if the key is matched.</returns>
        public async Task<IEnumerable<ContraceptiveHistory>> GetContraceptiveHistoryByGynObsHistoryId(Guid gynObsHistoryId)
        {
            try
            {
                var contraceptiveHistory = await context.ContraceptiveHistories.AsNoTracking().Where(c => c.GynObsHistoryId == gynObsHistoryId && c.IsDeleted == false).ToListAsync();


                return contraceptiveHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// The method is used to get the list of contraceptive histories.
        /// </summary>
        /// <returns>Returns a list of all contraceptive histories.</returns>
        public async Task<IEnumerable<ContraceptiveHistory>> GetContraceptiveHistories()
        {
            try
            {
                return await context.ContraceptiveHistories
                      .AsNoTracking().Where(x => x.IsDeleted == false)
                      .Join(
                            context.Encounters.AsNoTracking(),
                            contraceptiveHistory => contraceptiveHistory.EncounterId,
                  encounter => encounter.Oid,
                            (contraceptiveHistory, encounter) => new ContraceptiveHistory
                            {
                                InteractionId = contraceptiveHistory.InteractionId,
                                ContraceptiveId = contraceptiveHistory.ContraceptiveId,
                                GynObsHistoryId = contraceptiveHistory.GynObsHistoryId,
                                UsedFor = contraceptiveHistory.UsedFor,
                                // Properties from EncounterBaseModel
                                EncounterId = contraceptiveHistory.EncounterId,
                                EncounterType = contraceptiveHistory.EncounterType,
                                CreatedIn = contraceptiveHistory.CreatedIn,
                                DateCreated = contraceptiveHistory.DateCreated,
                                CreatedBy = contraceptiveHistory.CreatedBy,
                                ModifiedIn = contraceptiveHistory.ModifiedIn,
                                DateModified = contraceptiveHistory.DateModified,
                                ModifiedBy = contraceptiveHistory.ModifiedBy,
                                IsDeleted = contraceptiveHistory.IsDeleted,
                                IsSynced = contraceptiveHistory.IsSynced,
                                EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,

                            })
                      .OrderByDescending(x => x.EncounterDate)
                      .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}