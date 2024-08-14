using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

/*
 * Created by   : Brian
 * Date created : 08.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class OptedCircumcisionReasonRepository : Repository<OptedCircumcisionReason>, IOptedCircumcisionReasonRepository
    {
        public OptedCircumcisionReasonRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get OptedCircumcisionReason by key.
        /// </summary>
        /// <param name="key">Primary key of the table OptedCircumcisionReasons.</param>
        /// <returns>Returns a OptedCircumcisionReason if the key is matched.</returns>
        public async Task<OptedCircumcisionReason> GetOptedCircumcisionReasonByKey(Guid key)
        {
            try
            {
                var optedCircumcisionReason = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (optedCircumcisionReason != null)
                    optedCircumcisionReason.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return optedCircumcisionReason;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of Opted Circumcision Reason.
        /// </summary>
        /// <returns>Returns a list of all Opted Circumcision Reason.</returns>
        public async Task<IEnumerable<OptedCircumcisionReason>> GetOptedCircumcisionReasons()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to a get OptedCircumcisionReason by key.
        /// </summary>
        /// <param name="circumcisionReasonId">Primary key of the table OptedCircumcisionReasons.</param>
        /// <returns>Returns a OptedCircumcisionReason if the CircumcisionReasonId is matched.</returns>
        public async Task<IEnumerable<OptedCircumcisionReason>> GetOptedCircumcisionReasonByCircumcisionReason(int circumcisionReasonId)
        {
            try
            {
                return await context.OptedCircumcisionReasons.AsNoTracking().Where(p => p.IsDeleted == false && p.CircumcisionReasonId == circumcisionReasonId)
       .Join(
           context.Encounters.AsNoTracking(),
           optedCircumcisionReasons => optedCircumcisionReasons.EncounterId,
           encounter => encounter.Oid,
           (optedCircumcisionReasons, encounter) => new OptedCircumcisionReason
           {
               EncounterId = optedCircumcisionReasons.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               EncounterType = optedCircumcisionReasons.EncounterType,
               DateModified = optedCircumcisionReasons.DateModified,
               CircumcisionReason = optedCircumcisionReasons.CircumcisionReason,
               CircumcisionReasonId = optedCircumcisionReasons.CircumcisionReasonId,
               CreatedBy = optedCircumcisionReasons.CreatedBy,
               CreatedIn = optedCircumcisionReasons.CreatedIn,
               DateCreated = optedCircumcisionReasons.DateCreated,
               InteractionId = optedCircumcisionReasons.InteractionId,
               IsDeleted = optedCircumcisionReasons.IsDeleted,
               IsSynced = optedCircumcisionReasons.IsSynced,
               ModifiedBy = optedCircumcisionReasons.ModifiedBy,
               ModifiedIn = optedCircumcisionReasons.ModifiedIn,
               VMMCServiceId = optedCircumcisionReasons.VMMCServiceId,

           }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to a get OptedCircumcisionReason by VMMCServiceId.
        /// </summary>
        /// <param name="vmmcServiceId">Primary key of the table OptedCircumcisionReasons.</param>
        /// <returns>Returns a OptedCircumcisionReason if the VMMCServiceId is matched.</returns>
        public async Task<IEnumerable<OptedCircumcisionReason>> GetCircumcisionReasonByVMMCService(Guid vmmcServiceId)
        {
            try
            {

                return await context.OptedCircumcisionReasons.AsNoTracking().Where(p => p.IsDeleted == false && p.VMMCServiceId == vmmcServiceId)
       .Join(
           context.Encounters.AsNoTracking(),
           optedCircumcisionReasons => optedCircumcisionReasons.EncounterId,
           encounter => encounter.Oid,
           (optedCircumcisionReasons, encounter) => new OptedCircumcisionReason
           {
               EncounterId = optedCircumcisionReasons.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               EncounterType = optedCircumcisionReasons.EncounterType,
               DateModified = optedCircumcisionReasons.DateModified,
               CircumcisionReason = optedCircumcisionReasons.CircumcisionReason,
               CircumcisionReasonId = optedCircumcisionReasons.CircumcisionReasonId,
               CreatedBy = optedCircumcisionReasons.CreatedBy,
               CreatedIn = optedCircumcisionReasons.CreatedIn,
               DateCreated = optedCircumcisionReasons.DateCreated,
               InteractionId = optedCircumcisionReasons.InteractionId,
               IsDeleted = optedCircumcisionReasons.IsDeleted,
               IsSynced = optedCircumcisionReasons.IsSynced,
               ModifiedBy = optedCircumcisionReasons.ModifiedBy,
               ModifiedIn = optedCircumcisionReasons.ModifiedIn,
               VMMCServiceId = optedCircumcisionReasons.VMMCServiceId,

           }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}