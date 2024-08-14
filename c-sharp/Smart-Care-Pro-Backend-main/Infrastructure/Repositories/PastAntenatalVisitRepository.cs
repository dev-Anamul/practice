using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Biplob Roy
 * Date created : 19.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class PastAntenatalVisitRepository : Repository<PastAntenatalVisit>, IPastAntenatalVisitRepository
    {
        public PastAntenatalVisitRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a PastAntenatalVisit if the ClientID is matched.</returns>
        public async Task<IEnumerable<PastAntenatalVisit>> GetPastAntenatalVisitByClient(Guid clientId)
        {
            try
            {
                return await context.PastAntenatalVisits.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
              .Join(
               context.Encounters.AsNoTracking(),
               pastAntenatalVisit => pastAntenatalVisit.EncounterId,
               encounter => encounter.Oid,
               (pastAntenatalVisit, encounter) => new PastAntenatalVisit
               {
                   EncounterId = pastAntenatalVisit.EncounterId,
                   EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                   ClientId = pastAntenatalVisit.ClientId,
                   CreatedBy = pastAntenatalVisit.CreatedBy,
                   CreatedIn = pastAntenatalVisit.CreatedIn,
                   DateCreated = pastAntenatalVisit.DateCreated,
                   DateModified = pastAntenatalVisit.DateModified,
                   EncounterType = pastAntenatalVisit.EncounterType,
                   Findings = pastAntenatalVisit.Findings,
                   InteractionId = pastAntenatalVisit.InteractionId,
                   IsAdmitted = pastAntenatalVisit.IsAdmitted,
                   IsDeleted = pastAntenatalVisit.IsDeleted,
                   IsSynced = pastAntenatalVisit.IsSynced,
                   ModifiedBy = pastAntenatalVisit.ModifiedBy,
                   ModifiedIn = pastAntenatalVisit.ModifiedIn,
                   VisitDate = pastAntenatalVisit.VisitDate,
                   VisitNo = pastAntenatalVisit.VisitNo,
                   ClinicianName = context.UserAccounts.Where(x => x.Oid == pastAntenatalVisit.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                   FacilityName = context.Facilities.Where(x => x.Oid == pastAntenatalVisit.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

               }).OrderByDescending(p => p.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a birth record by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="encounterType"></param>
        /// <returns>Returns a PastAntenatalVisit if the clientId is matched.</returns>
        public async Task<IEnumerable<PastAntenatalVisit>> GetPastAntenatalVisitByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var pastAntenatalVisitAsQuerable = context.PastAntenatalVisits.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
       .Join(
           context.Encounters.AsNoTracking(),
           pastAntenatalVisit => pastAntenatalVisit.EncounterId,
           encounter => encounter.Oid,
           (pastAntenatalVisit, encounter) => new PastAntenatalVisit
           {
               EncounterId = pastAntenatalVisit.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               ClientId = pastAntenatalVisit.ClientId,
               CreatedBy = pastAntenatalVisit.CreatedBy,
               CreatedIn = pastAntenatalVisit.CreatedIn,
               DateCreated = pastAntenatalVisit.DateCreated,
               DateModified = pastAntenatalVisit.DateModified,
               EncounterType = pastAntenatalVisit.EncounterType,
               Findings = pastAntenatalVisit.Findings,
               InteractionId = pastAntenatalVisit.InteractionId,
               IsAdmitted = pastAntenatalVisit.IsAdmitted,
               IsDeleted = pastAntenatalVisit.IsDeleted,
               IsSynced = pastAntenatalVisit.IsSynced,
               ModifiedBy = pastAntenatalVisit.ModifiedBy,
               ModifiedIn = pastAntenatalVisit.ModifiedIn,
               VisitDate = pastAntenatalVisit.VisitDate,
               VisitNo = pastAntenatalVisit.VisitNo,
               ClinicianName = context.UserAccounts.Where(x => x.Oid == pastAntenatalVisit.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
               FacilityName = context.Facilities.Where(x => x.Oid == pastAntenatalVisit.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

           }).AsQueryable();

                if (encounterType == null)
                    return await pastAntenatalVisitAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

                else
                    return await pastAntenatalVisitAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PastAntenatalVisit by EncounterID.
        /// </summary>
        /// <param name="clientID"
        /// <returns>Returns a list of all PastAntenatalVisit by EncounterID.</returns>
        public int GetPastAntenatalVisitByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.PastAntenatalVisits.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();

            else
                return context.PastAntenatalVisits.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }


        /// <summary>
        /// The method is used to get the list of PastAntenatalVisit by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all PastAntenatalVisit by EncounterID.</returns>
        public async Task<IEnumerable<PastAntenatalVisit>> GetPastAntenatalVisitByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.PastAntenatalVisits.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
        .Join(
            context.Encounters.AsNoTracking(),
            pastAntenatalVisit => pastAntenatalVisit.EncounterId,
            encounter => encounter.Oid,
            (pastAntenatalVisit, encounter) => new PastAntenatalVisit
            {
                EncounterId = pastAntenatalVisit.EncounterId,
                EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                ClientId = pastAntenatalVisit.ClientId,
                CreatedBy = pastAntenatalVisit.CreatedBy,
                CreatedIn = pastAntenatalVisit.CreatedIn,
                DateCreated = pastAntenatalVisit.DateCreated,
                DateModified = pastAntenatalVisit.DateModified,
                EncounterType = pastAntenatalVisit.EncounterType,
                Findings = pastAntenatalVisit.Findings,
                InteractionId = pastAntenatalVisit.InteractionId,
                IsAdmitted = pastAntenatalVisit.IsAdmitted,
                IsDeleted = pastAntenatalVisit.IsDeleted,
                IsSynced = pastAntenatalVisit.IsSynced,
                ModifiedBy = pastAntenatalVisit.ModifiedBy,
                ModifiedIn = pastAntenatalVisit.ModifiedIn,
                VisitDate = pastAntenatalVisit.VisitDate,
                VisitNo = pastAntenatalVisit.VisitNo,

            }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a PastAntenatalVisit by key.
        /// </summary>
        /// <param name="key">Primary key of the table PastAntenatalVisits.</param>
        /// <returns>Returns a PastAntenatalVisit if the key is matched.</returns>
        public async Task<PastAntenatalVisit> GetPastAntenatalVisitByKey(Guid key)
        {
            try
            {
                var pastAntenatalVisit = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (pastAntenatalVisit != null)
                    pastAntenatalVisit.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return pastAntenatalVisit;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PastAntenatalVisits.
        /// </summary>
        /// <returns>Returns a list of all PastAntenatalVisit.</returns>
        public async Task<IEnumerable<PastAntenatalVisit>> GetPastAntenatalVisits()
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