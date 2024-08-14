using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 13.08.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class HIVRiskScreeningRepository : Repository<HIVRiskScreening>, IHIVRiskScreeningRepository
    {
        public HIVRiskScreeningRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get HIVRiskScreening by key.
        /// </summary>
        /// <param name="key">Primary key of the table HIVRiskScreening.</param>
        /// <returns>Returns a HIVRiskScreening if the key is matched.</returns>
        public async Task<HIVRiskScreening> GetHIVRiskScreeningByKey(Guid key)
        {
            try
            {
                var hIVRiskScreening = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (hIVRiskScreening != null)
                    hIVRiskScreening.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return hIVRiskScreening;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a HIVRiskScreening by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a HIVRiskScreening if the ClientID is matched.</returns>
        public async Task<IEnumerable<HIVRiskScreening>> GetHIVRiskScreeningByClient(Guid clientId)
        {
            try
            {
                return await context.HIVRiskScreenings.Include(x => x.Question).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
                  .Join(
                      context.Encounters.AsNoTracking(),
                  hIVRiskScreening => hIVRiskScreening.EncounterId,
                      encounter => encounter.Oid,
                      (hIVRiskScreening, encounter) => new HIVRiskScreening
                      {
                          EncounterId = hIVRiskScreening.EncounterId,
                          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                          Answer = hIVRiskScreening.Answer,
                          ClientId = hIVRiskScreening.ClientId,
                          CreatedBy = hIVRiskScreening.CreatedBy,
                          CreatedIn = hIVRiskScreening.CreatedIn,
                          DateCreated = hIVRiskScreening.DateCreated,
                          DateModified = hIVRiskScreening.DateModified,
                          EncounterType = hIVRiskScreening.EncounterType,
                          InteractionId = hIVRiskScreening.InteractionId,
                          IsDeleted = hIVRiskScreening.IsDeleted,
                          IsSynced = hIVRiskScreening.IsSynced,
                          ModifiedBy = hIVRiskScreening.ModifiedBy,
                          ModifiedIn = hIVRiskScreening.ModifiedIn,
                          QuestionId = hIVRiskScreening.QuestionId,
                          Question = hIVRiskScreening.Question,


                      }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of HIVRiskScreenings.
        /// </summary>
        /// <returns>Returns a list of all HIVRiskScreenings.</returns>
        public async Task<IEnumerable<HIVRiskScreening>> GetHIVRiskScreenings()
        {
            try
            {
                return await QueryAsync(h => h.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a HIVRiskScreening by encounterId
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a HIVRiskScreening if the encounterId is matched.</returns>
        public async Task<IEnumerable<HIVRiskScreening>> GetHIVRiskScreeningByEncounter(Guid encounterId)
        {
            try
            {
                return await context.HIVRiskScreenings.Include(x => x.Question).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
                 .Join(
                     context.Encounters.AsNoTracking(),
                 hIVRiskScreening => hIVRiskScreening.EncounterId,
                     encounter => encounter.Oid,
                     (hIVRiskScreening, encounter) => new HIVRiskScreening
                     {
                         EncounterId = hIVRiskScreening.EncounterId,
                         EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                         Answer = hIVRiskScreening.Answer,
                         ClientId = hIVRiskScreening.ClientId,
                         CreatedBy = hIVRiskScreening.CreatedBy,
                         CreatedIn = hIVRiskScreening.CreatedIn,
                         DateCreated = hIVRiskScreening.DateCreated,
                         DateModified = hIVRiskScreening.DateModified,
                         EncounterType = hIVRiskScreening.EncounterType,
                         InteractionId = hIVRiskScreening.InteractionId,
                         IsDeleted = hIVRiskScreening.IsDeleted,
                         IsSynced = hIVRiskScreening.IsSynced,
                         ModifiedBy = hIVRiskScreening.ModifiedBy,
                         ModifiedIn = hIVRiskScreening.ModifiedIn,
                         QuestionId = hIVRiskScreening.QuestionId,
                         Question = hIVRiskScreening.Question,


                     }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<HIVRiskScreening>> GetHIVRiskScreeningByEncounterIdEncounterType(Guid encounterId, EncounterType encounterType)
        {
            try
            {
                return await context.HIVRiskScreenings.Include(x => x.Question).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterType == encounterType && p.EncounterId == encounterId).AsNoTracking()
                 .Join(
                     context.Encounters.AsNoTracking(),
                 hIVRiskScreening => hIVRiskScreening.EncounterId,
                     encounter => encounter.Oid,
                     (hIVRiskScreening, encounter) => new HIVRiskScreening
                     {
                         EncounterId = hIVRiskScreening.EncounterId,
                         EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                         Answer = hIVRiskScreening.Answer,
                         ClientId = hIVRiskScreening.ClientId,
                         CreatedBy = hIVRiskScreening.CreatedBy,
                         CreatedIn = hIVRiskScreening.CreatedIn,
                         DateCreated = hIVRiskScreening.DateCreated,
                         DateModified = hIVRiskScreening.DateModified,
                         EncounterType = hIVRiskScreening.EncounterType,
                         InteractionId = hIVRiskScreening.InteractionId,
                         IsDeleted = hIVRiskScreening.IsDeleted,
                         IsSynced = hIVRiskScreening.IsSynced,
                         ModifiedBy = hIVRiskScreening.ModifiedBy,
                         ModifiedIn = hIVRiskScreening.ModifiedIn,
                         QuestionId = hIVRiskScreening.QuestionId,
                         Question = hIVRiskScreening.Question,


                     }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}