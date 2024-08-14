using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Biplob Roy
 * Date created : 29.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class PelvicAndVaginalExaminationRepository : Repository<PelvicAndVaginalExamination>, IPelvicAndVaginalExaminationRepository
    {
        /// <summary>
        /// Implementation of IPelvicAndVaginalExaminationRepository interface.
        /// </summary>
        public PelvicAndVaginalExaminationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a PelvicAndVaginalExamination by key.
        /// </summary>
        /// <param name="key">Primary key of the table PelvicAndVaginalExaminations.</param>
        /// <returns>Returns a PelvicAndVaginalExamination if the key is matched.</returns>
        public async Task<PelvicAndVaginalExamination> GetPelvicAndVaginalExaminationByKey(Guid key)
        {
            try
            {
                var pelvicAndVaginalExamination = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (pelvicAndVaginalExamination != null)
                    pelvicAndVaginalExamination.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return pelvicAndVaginalExamination;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PelvicAndVaginalExamination.
        /// </summary>
        /// <returns>Returns a list of all PelvicAndVaginalExaminations.</returns>
        public async Task<IEnumerable<PelvicAndVaginalExamination>> GetPelvicAndVaginalExaminations()
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
        /// The method is used to get a PelvicAndVaginalExamination by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a PelvicAndVaginalExamination if the ClientID is matched.</returns>
        public async Task<IEnumerable<PelvicAndVaginalExamination>> GetPelvicAndVaginalExaminationByClient(Guid clientId)
        {
            try
            {
                return await context.PelvicAndVaginalExaminations.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
           .Join(
               context.Encounters.AsNoTracking(),
               pelvicAndVaginalExamination => pelvicAndVaginalExamination.EncounterId,
               encounter => encounter.Oid,
               (pelvicAndVaginalExamination, encounter) => new PelvicAndVaginalExamination
               {
                   EncounterId = pelvicAndVaginalExamination.EncounterId,
                   EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                   CreatedIn = pelvicAndVaginalExamination.CreatedIn,
                   CreatedBy = pelvicAndVaginalExamination.CreatedBy,
                   ClientId = pelvicAndVaginalExamination.ClientId,
                   DateModified = pelvicAndVaginalExamination.DateModified,
                   DateCreated = pelvicAndVaginalExamination.DateCreated,
                   InteractionId = pelvicAndVaginalExamination.InteractionId,
                   EncounterType = pelvicAndVaginalExamination.EncounterType,
                   Bleeding = pelvicAndVaginalExamination.Bleeding,
                   EpisiotomySuture = pelvicAndVaginalExamination.EpisiotomySuture,
                   IsDeleted = pelvicAndVaginalExamination.IsDeleted,
                   IsSynced = pelvicAndVaginalExamination.IsSynced,
                   Lochia = pelvicAndVaginalExamination.Lochia,
                   ModifiedBy = pelvicAndVaginalExamination.ModifiedBy,
                   ModifiedIn = pelvicAndVaginalExamination.ModifiedIn,
                   Perineum = pelvicAndVaginalExamination.Perineum,
                   UterusContracted = pelvicAndVaginalExamination.UterusContracted,
                   Vulva = pelvicAndVaginalExamination.Vulva,

               }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<PelvicAndVaginalExamination>> GetPelvicAndVaginalExaminationByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var pelvicAndVaginalExaminationAsQuerable = context.PelvicAndVaginalExaminations.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
       .Join(
           context.Encounters.AsNoTracking(),
           pelvicAndVaginalExamination => pelvicAndVaginalExamination.EncounterId,
           encounter => encounter.Oid,
           (pelvicAndVaginalExamination, encounter) => new PelvicAndVaginalExamination
           {
               EncounterId = pelvicAndVaginalExamination.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               CreatedIn = pelvicAndVaginalExamination.CreatedIn,
               CreatedBy = pelvicAndVaginalExamination.CreatedBy,
               ClientId = pelvicAndVaginalExamination.ClientId,
               DateModified = pelvicAndVaginalExamination.DateModified,
               DateCreated = pelvicAndVaginalExamination.DateCreated,
               InteractionId = pelvicAndVaginalExamination.InteractionId,
               EncounterType = pelvicAndVaginalExamination.EncounterType,
               Bleeding = pelvicAndVaginalExamination.Bleeding,
               EpisiotomySuture = pelvicAndVaginalExamination.EpisiotomySuture,
               IsDeleted = pelvicAndVaginalExamination.IsDeleted,
               IsSynced = pelvicAndVaginalExamination.IsSynced,
               Lochia = pelvicAndVaginalExamination.Lochia,
               ModifiedBy = pelvicAndVaginalExamination.ModifiedBy,
               ModifiedIn = pelvicAndVaginalExamination.ModifiedIn,
               Perineum = pelvicAndVaginalExamination.Perineum,
               UterusContracted = pelvicAndVaginalExamination.UterusContracted,
               Vulva = pelvicAndVaginalExamination.Vulva,


           }).AsQueryable();

                if (encounterType == null)
                    return await pelvicAndVaginalExaminationAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await pelvicAndVaginalExaminationAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetPelvicAndVaginalExaminationByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.PelvicAndVaginalExaminations.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.PelvicAndVaginalExaminations.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get the list of PelvicAndVaginalExamination by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all PelvicAndVaginalExamination by EncounterID.</returns>
        public async Task<IEnumerable<PelvicAndVaginalExamination>> GetPelvicAndVaginalExaminationByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.PelvicAndVaginalExaminations.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
           .Join(
               context.Encounters.AsNoTracking(),
               pelvicAndVaginalExamination => pelvicAndVaginalExamination.EncounterId,
               encounter => encounter.Oid,
               (pelvicAndVaginalExamination, encounter) => new PelvicAndVaginalExamination
               {
                   EncounterId = pelvicAndVaginalExamination.EncounterId,
                   EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                   CreatedIn = pelvicAndVaginalExamination.CreatedIn,
                   CreatedBy = pelvicAndVaginalExamination.CreatedBy,
                   ClientId = pelvicAndVaginalExamination.ClientId,
                   DateModified = pelvicAndVaginalExamination.DateModified,
                   DateCreated = pelvicAndVaginalExamination.DateCreated,
                   InteractionId = pelvicAndVaginalExamination.InteractionId,
                   EncounterType = pelvicAndVaginalExamination.EncounterType,
                   Bleeding = pelvicAndVaginalExamination.Bleeding,
                   EpisiotomySuture = pelvicAndVaginalExamination.EpisiotomySuture,
                   IsDeleted = pelvicAndVaginalExamination.IsDeleted,
                   IsSynced = pelvicAndVaginalExamination.IsSynced,
                   Lochia = pelvicAndVaginalExamination.Lochia,
                   ModifiedBy = pelvicAndVaginalExamination.ModifiedBy,
                   ModifiedIn = pelvicAndVaginalExamination.ModifiedIn,
                   Perineum = pelvicAndVaginalExamination.Perineum,
                   UterusContracted = pelvicAndVaginalExamination.UterusContracted,
                   Vulva = pelvicAndVaginalExamination.Vulva,

               }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}