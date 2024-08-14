using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  : Stephan
 * Last modified: 16.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IVeginalPositionRepository interface.
    /// </summary>
    public class VaginalPositionRepository : Repository<VaginalPosition>, IVaginalPositionRepository
    {
        public VaginalPositionRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by ClientId.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns>Returns a VeginalPosition if the ClientId is matched.</returns>
        public async Task<IEnumerable<VaginalPosition>> GetVaginalPositionByClient(Guid clientId)
        {
            try
            {
                return await context.VaginalPositions.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
                    .Join(
                           context.Encounters.AsNoTracking(),
                           vaginalPosition => vaginalPosition.EncounterId,
                           encounter => encounter.Oid,
                           (vaginalPosition, encounter) => new VaginalPosition
                           {
                               EncounterId = vaginalPosition.EncounterId,
                               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                               ClientId = vaginalPosition.ClientId,
                               ModifiedIn = vaginalPosition.ModifiedIn,
                               ModifiedBy = vaginalPosition.ModifiedBy,
                               IsSynced = vaginalPosition.IsSynced,
                               IsDeleted = vaginalPosition.IsDeleted,
                               CreatedBy = vaginalPosition.CreatedBy,
                               CreatedIn = vaginalPosition.CreatedIn,
                               DateCreated = vaginalPosition.DateCreated,
                               DateModified = vaginalPosition.DateModified,
                               EncounterType = vaginalPosition.EncounterType,
                               InteractionId = vaginalPosition.InteractionId,
                               Note = vaginalPosition.Note,
                               Position = vaginalPosition.Position,

                           }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of VeginalPosition by EncounterId.
        /// </summary>
        /// <returns>Returns a list of all VeginalPosition by EncounterId.</returns>
        public async Task<IEnumerable<VaginalPosition>> GetVaginalPositionByEncounter(Guid encounterId)
        {
            try
            {
                return await context.VaginalPositions.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId)
                 .Join(
                        context.Encounters.AsNoTracking(),
                        vaginalPosition => vaginalPosition.EncounterId,
                        encounter => encounter.Oid,
                        (vaginalPosition, encounter) => new VaginalPosition
                        {
                            EncounterId = vaginalPosition.EncounterId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClientId = vaginalPosition.ClientId,
                            ModifiedIn = vaginalPosition.ModifiedIn,
                            ModifiedBy = vaginalPosition.ModifiedBy,
                            IsSynced = vaginalPosition.IsSynced,
                            IsDeleted = vaginalPosition.IsDeleted,
                            CreatedBy = vaginalPosition.CreatedBy,
                            CreatedIn = vaginalPosition.CreatedIn,
                            DateCreated = vaginalPosition.DateCreated,
                            DateModified = vaginalPosition.DateModified,
                            EncounterType = vaginalPosition.EncounterType,
                            InteractionId = vaginalPosition.InteractionId,
                            Note = vaginalPosition.Note,
                            Position = vaginalPosition.Position,

                        }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a VeginalPosition by key.
        /// </summary>
        /// <param name="key">Primary key of the table VeginalPositions.</param>
        /// <returns>Returns a VeginalPosition if the key is matched.</returns>
        public async Task<VaginalPosition> GetVaginalPositionByKey(Guid key)
        {
            try
            {
                var vaginalPosition = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (vaginalPosition != null)
                    vaginalPosition.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return vaginalPosition;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of VeginalPositions.
        /// </summary>
        /// <returns>Returns a list of all VeginalPosition.</returns>
        public async Task<IEnumerable<VaginalPosition>> GetVaginalPositions()
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