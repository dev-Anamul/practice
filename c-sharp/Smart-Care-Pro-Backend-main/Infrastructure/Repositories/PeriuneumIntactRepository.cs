using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Biplob Roy
 * Date created : 02.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class PeriuneumIntactRepository : Repository<PerineumIntact>, IPeriuneumIntactRepository
    {
        public PeriuneumIntactRepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// The method is used to a get PeriuneumIntact by key.
        /// </summary>
        /// <param name="key">Primary key of the table PeriuneumIntacts.</param>
        /// <returns>Returns a PeriuneumIntact if the key is matched.</returns>
        public async Task<PerineumIntact> GetPeriuneumIntactByKey(Guid key)
        {
            try
            {
                var perineumIntact = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (perineumIntact != null)
                    perineumIntact.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return perineumIntact;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PeriuneumIntacts.
        /// </summary>
        /// <returns>Returns a list of all Medical treatments.</returns>
        public async Task<IEnumerable<PerineumIntact>> GetPeriuneumIntacts()
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
        /// The method is used to get the list of PeriuneumIntact by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all PeriuneumIntact by EncounterID.</returns>
        public async Task<IEnumerable<PerineumIntact>> GetPeriuneumIntactByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.PeriuneumIntacts.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
            .Join(
                context.Encounters.AsNoTracking(),
                perineumIntact => perineumIntact.EncounterId,
                encounter => encounter.Oid,
                (perineumIntact, encounter) => new PerineumIntact
                {
                    EncounterId = perineumIntact.EncounterId,
                    EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                    ModifiedBy = perineumIntact.ModifiedBy,
                    InteractionId = perineumIntact.InteractionId,
                    IsSynced = perineumIntact.IsSynced,
                    CreatedBy = perineumIntact.CreatedBy,
                    CreatedIn = perineumIntact.CreatedIn,
                    DateCreated = perineumIntact.DateCreated,
                    DateModified = perineumIntact.DateModified,
                    DeliveryId = perineumIntact.DeliveryId,
                    EncounterType = perineumIntact.EncounterType,
                    IsDeleted = perineumIntact.IsDeleted,
                    IsPerineumIntact = perineumIntact.IsPerineumIntact,
                    ModifiedIn = perineumIntact.ModifiedIn,
                    MotherDeliveryComment = perineumIntact.MotherDeliveryComment,
                    TearDetails = perineumIntact.TearDetails,

                }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PeriuneumIntact by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all PeriuneumIntact by DeliveryId.</returns>
        public async Task<IEnumerable<PerineumIntact>> GetPeriuneumIntactByDelivery(Guid DeliveryId)
        {
            try
            {
                return await context.PeriuneumIntacts.AsNoTracking().Where(p => p.IsDeleted == false && p.DeliveryId == DeliveryId)
            .Join(
                context.Encounters.AsNoTracking(),
                perineumIntact => perineumIntact.EncounterId,
                encounter => encounter.Oid,
                (perineumIntact, encounter) => new PerineumIntact
                {
                    EncounterId = perineumIntact.EncounterId,
                    EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                    ModifiedBy = perineumIntact.ModifiedBy,
                    InteractionId = perineumIntact.InteractionId,
                    IsSynced = perineumIntact.IsSynced,
                    CreatedBy = perineumIntact.CreatedBy,
                    CreatedIn = perineumIntact.CreatedIn,
                    DateCreated = perineumIntact.DateCreated,
                    DateModified = perineumIntact.DateModified,
                    DeliveryId = perineumIntact.DeliveryId,
                    EncounterType = perineumIntact.EncounterType,
                    IsDeleted = perineumIntact.IsDeleted,
                    IsPerineumIntact = perineumIntact.IsPerineumIntact,
                    ModifiedIn = perineumIntact.ModifiedIn,
                    MotherDeliveryComment = perineumIntact.MotherDeliveryComment,
                    TearDetails = perineumIntact.TearDetails,

                }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}