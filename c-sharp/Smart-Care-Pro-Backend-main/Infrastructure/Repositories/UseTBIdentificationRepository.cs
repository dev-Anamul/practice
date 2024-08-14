using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Lion
 * Date created : 30.03.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IUseTBIdentificationMethodRepository interface.
    /// </summary>
    public class UseTBIdentificationMethodRepository : Repository<UsedTBIdentificationMethod>, IUseTBIdentificationMethodRepository
    {
        /// <summary>
        /// Implementation of IUseTBIdentificationRepository interface.
        /// </summary>
        public UseTBIdentificationMethodRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a UsedTBIdentificationMethod by key.
        /// </summary>
        /// <param name="key">Primary key of the table UsedTBIdentificationMethods.</param>
        /// <returns>Returns a UsedTBIdentificationMethod if the key is matched.</returns>
        public async Task<UsedTBIdentificationMethod> GetUsedTBIdentificationMethodByKey(Guid key)
        {
            try
            {
                var usedTBIdentificationMethod = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (usedTBIdentificationMethod != null)
                    usedTBIdentificationMethod.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return usedTBIdentificationMethod;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of UsedTBIdentificationMethod.
        /// </summary>
        /// <returns>Returns a list of all UsedTBIdentificationMethods.</returns>
        public async Task<IEnumerable<UsedTBIdentificationMethod>> GetUsedTBIdentificationMethods()
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
        /// The method is used to get the list of UsedTBIdentificationMethod by EncounterId.
        /// </summary>
        /// <param name="EncounterId">EncounterId of a Client.</param>
        /// <returns>Returns a list of all UsedTBIdentificationMethod by EncounterId.</returns>
        public async Task<IEnumerable<UsedTBIdentificationMethod>> GetUsedTBIdentificationMethodByEncounter(Guid EncounterId)
        {
            try
            {
                return await context.UsedTBIdentificationMethods.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterId)
                 .Join(
                        context.Encounters.AsNoTracking(),
                        usedTBIdentificationMethod => usedTBIdentificationMethod.EncounterId,
                        encounter => encounter.Oid,
                        (usedTBIdentificationMethod, encounter) => new UsedTBIdentificationMethod
                        {
                            EncounterId = usedTBIdentificationMethod.EncounterId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ModifiedIn = usedTBIdentificationMethod.ModifiedIn,
                            ModifiedBy = usedTBIdentificationMethod.ModifiedBy,
                            IsSynced = usedTBIdentificationMethod.IsSynced,
                            IsDeleted = usedTBIdentificationMethod.IsDeleted,
                            CreatedBy = usedTBIdentificationMethod.CreatedBy,
                            CreatedIn = usedTBIdentificationMethod.CreatedIn,
                            DateCreated = usedTBIdentificationMethod.DateCreated,
                            DateModified = usedTBIdentificationMethod.DateModified,
                            EncounterType = usedTBIdentificationMethod.EncounterType,
                            InteractionId = usedTBIdentificationMethod.InteractionId,
                            TBHistoryId = usedTBIdentificationMethod.TBHistoryId,
                            TBIdentificationMethodId = usedTBIdentificationMethod.TBIdentificationMethodId,


                        }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}