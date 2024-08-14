using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class IdentifiedCordStumpRepository : Repository<IdentifiedCordStump>, IIdentifiedCordStumpRepository
    {
        /// <summary>
        /// Implementation of IIdentifiedCordStumpRepository interface.
        /// </summary>
        public IdentifiedCordStumpRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a IdentifiedCordStump by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedCordStumps.</param>
        /// <returns>Returns a IdentifiedCordStump if the key is matched.</returns>
        public async Task<IdentifiedCordStump> GetIdentifiedCordStumpByKey(Guid key)
        {
            try
            {
                var identifiedCordStump = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedCordStump != null)
                {
                    identifiedCordStump.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedCordStump.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedCordStump.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedCordStump.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedCordStump.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedCordStump.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                 }

                return identifiedCordStump;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedCordStump.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedCordStumps.</returns>
        public async Task<IEnumerable<IdentifiedCordStump>> GetIdentifiedCordStumps()
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
        /// The method is used to get the list of IdentifiedCordStump by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedCordStump by EncounterID.</returns>
        public async Task<IEnumerable<IdentifiedCordStump>> GetIdentifiedCordStumpByEncounter(Guid encounterId)
        {
            try
            {
                return await context.IdentifiedCordStumps.Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
               .Join(
                   context.Encounters.AsNoTracking(),
                identifiedCordStumps => identifiedCordStumps.EncounterId,
                   encounter => encounter.Oid,
                   (identifiedCordStumps, encounter) => new IdentifiedCordStump
                   {
                       EncounterId = identifiedCordStumps.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       AssessmentId = identifiedCordStumps.AssessmentId,
                       InteractionId = identifiedCordStumps.InteractionId,
                       EncounterType = identifiedCordStumps.EncounterType,
                       DateModified = identifiedCordStumps.DateModified,
                       DateCreated = identifiedCordStumps.DateCreated,
                       CreatedBy = identifiedCordStumps.CreatedBy,
                       CordStumpCondition = identifiedCordStumps.CordStumpCondition,
                       CreatedIn = identifiedCordStumps.CreatedIn,
                       IsDeleted = identifiedCordStumps.IsDeleted,
                       IsSynced = identifiedCordStumps.IsSynced,
                       ModifiedBy = identifiedCordStumps.ModifiedBy,
                       ModifiedIn = identifiedCordStumps.ModifiedIn,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedCordStumps.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == identifiedCordStumps.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedCordStump by AssessmentID.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedCordStump by AssessmentID.</returns>
        public async Task<IEnumerable<IdentifiedCordStump>> ReadIdentifiedCordStumpByAssessment(Guid assessmentId)
        {
            try
            {
                return await context.IdentifiedCordStumps.Where(p => p.IsDeleted == false && p.AssessmentId == assessmentId).AsNoTracking()
               .Join(
                   context.Encounters.AsNoTracking(),
                identifiedCordStumps => identifiedCordStumps.EncounterId,
                   encounter => encounter.Oid,
                   (identifiedCordStumps, encounter) => new IdentifiedCordStump
                   {
                       EncounterId = identifiedCordStumps.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       AssessmentId = identifiedCordStumps.AssessmentId,
                       InteractionId = identifiedCordStumps.InteractionId,
                       EncounterType = identifiedCordStumps.EncounterType,
                       DateModified = identifiedCordStumps.DateModified,
                       DateCreated = identifiedCordStumps.DateCreated,
                       CreatedBy = identifiedCordStumps.CreatedBy,
                       CordStumpCondition = identifiedCordStumps.CordStumpCondition,
                       CreatedIn = identifiedCordStumps.CreatedIn,
                       IsDeleted = identifiedCordStumps.IsDeleted,
                       IsSynced = identifiedCordStumps.IsSynced,
                       ModifiedBy = identifiedCordStumps.ModifiedBy,
                       ModifiedIn = identifiedCordStumps.ModifiedIn,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedCordStumps.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == identifiedCordStumps.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}