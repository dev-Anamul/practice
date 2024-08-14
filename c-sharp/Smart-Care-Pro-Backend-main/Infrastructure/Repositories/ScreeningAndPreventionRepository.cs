using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Biplob Roy
 * Date created : 18.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class ScreeningAndPreventionRepository : Repository<ScreeningAndPrevention>, IScreeningAndPreventionRepository
    {
        public ScreeningAndPreventionRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a ScreeningAndPrevention if the ClientID is matched.</returns>
        public async Task<IEnumerable<ScreeningAndPrevention>> GetScreeningAndPreventionByClient(Guid ClientID)
        {
            try
            {
                return await context.ScreeningAndPreventions.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == ClientID)
                      .Join(
                          context.Encounters.AsNoTracking(),
                          screeningAndPrevention => screeningAndPrevention.EncounterId,
                          encounter => encounter.Oid,
                          (screeningAndPrevention, encounter) => new ScreeningAndPrevention
                          {
                              EncounterId = screeningAndPrevention.EncounterId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              IsSynced = screeningAndPrevention.IsSynced,
                              AmeniaDose = screeningAndPrevention.AmeniaDose,
                              ClientId = screeningAndPrevention.ClientId,
                              CreatedBy = screeningAndPrevention.CreatedBy,
                              CreatedIn = screeningAndPrevention.CreatedIn,
                              DateCreated = screeningAndPrevention.DateCreated,
                              DateModified = screeningAndPrevention.DateModified,
                              EncounterType = screeningAndPrevention.EncounterType,
                              HepatitisBDose = screeningAndPrevention.HepatitisBDose,
                              InteractionId = screeningAndPrevention.InteractionId,
                              IsDeleted = screeningAndPrevention.IsDeleted,
                              MalariaDose = screeningAndPrevention.MalariaDose,
                              MalariaDoseNo = screeningAndPrevention.MalariaDoseNo,
                              ModifiedBy = screeningAndPrevention.ModifiedBy,
                              ModifiedIn = screeningAndPrevention.ModifiedIn,
                              SyphilisDose = screeningAndPrevention.SyphilisDose,
                              TetanusDose = screeningAndPrevention.TetanusDose,
                              TetanusDoseNo = screeningAndPrevention.TetanusDoseNo

                          }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<ScreeningAndPrevention>> GetScreeningAndPreventionByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var screeningAndPreventionAsQuerable = context.ScreeningAndPreventions.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                   .Join(
                          context.Encounters.AsNoTracking(),
                          screeningAndPrevention => screeningAndPrevention.EncounterId,
                          encounter => encounter.Oid,
                          (screeningAndPrevention, encounter) => new ScreeningAndPrevention
                          {
                              EncounterId = screeningAndPrevention.EncounterId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              IsSynced = screeningAndPrevention.IsSynced,
                              AmeniaDose = screeningAndPrevention.AmeniaDose,
                              ClientId = screeningAndPrevention.ClientId,
                              CreatedBy = screeningAndPrevention.CreatedBy,
                              CreatedIn = screeningAndPrevention.CreatedIn,
                              DateCreated = screeningAndPrevention.DateCreated,
                              DateModified = screeningAndPrevention.DateModified,
                              EncounterType = screeningAndPrevention.EncounterType,
                              HepatitisBDose = screeningAndPrevention.HepatitisBDose,
                              InteractionId = screeningAndPrevention.InteractionId,
                              IsDeleted = screeningAndPrevention.IsDeleted,
                              MalariaDose = screeningAndPrevention.MalariaDose,
                              MalariaDoseNo = screeningAndPrevention.MalariaDoseNo,
                              ModifiedBy = screeningAndPrevention.ModifiedBy,
                              ModifiedIn = screeningAndPrevention.ModifiedIn,
                              SyphilisDose = screeningAndPrevention.SyphilisDose,
                              TetanusDose = screeningAndPrevention.TetanusDose,
                              TetanusDoseNo = screeningAndPrevention.TetanusDoseNo

                          }).AsQueryable();

                if (encounterType == null)
                    return await screeningAndPreventionAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await screeningAndPreventionAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
 
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetScreeningAndPreventionByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.ScreeningAndPreventions.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.ScreeningAndPreventions.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get the list of ScreeningAndPrevention by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ScreeningAndPrevention by EncounterID.</returns>
        public async Task<IEnumerable<ScreeningAndPrevention>> GetScreeningAndPreventionByEncounter(Guid EncounterID)
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false && c.EncounterId == EncounterID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a ScreeningAndPrevention by key.
        /// </summary>
        /// <param name="key">Primary key of the table ScreeningAndPreventions.</param>
        /// <returns>Returns a ScreeningAndPrevention if the key is matched.</returns>
        public async Task<ScreeningAndPrevention> GetScreeningAndPreventionByKey(Guid key)
        {
            try
            {
                var screeningAndPrevention = await FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);

                if (screeningAndPrevention != null)
                    screeningAndPrevention.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return screeningAndPrevention;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ScreeningAndPreventions.
        /// </summary>
        /// <returns>Returns a list of all ScreeningAndPrevention.</returns>
        public async Task<IEnumerable<ScreeningAndPrevention>> GetScreeningAndPreventions()
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
    }
}