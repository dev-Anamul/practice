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
    public class IdentifiedEyesAssessmentRepository : Repository<IdentifiedEyesAssessment>, IIdentifiedEyesAssessmentRepository
    {
        /// <summary>
        /// Implementation of IIdentifiedEyesAssessmentRepository interface.
        /// </summary>
        public IdentifiedEyesAssessmentRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a IdentifiedEyesAssessment by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedEyesAssessments.</param>
        /// <returns>Returns a IdentifiedEyesAssessment if the key is matched.</returns>
        public async Task<IdentifiedEyesAssessment> GetIdentifiedEyesAssessmentByKey(Guid key)
        {
            try
            {
                var identifiedEyesAssessment = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedEyesAssessment != null)
                {
                    identifiedEyesAssessment.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedEyesAssessment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedEyesAssessment.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedEyesAssessment.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedEyesAssessment.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedEyesAssessment.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return identifiedEyesAssessment;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedEyesAssessment.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedEyesAssessments.</returns>
        public async Task<IEnumerable<IdentifiedEyesAssessment>> GetIdentifiedEyesAssessments()
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
        /// The method is used to get the list of IdentifiedEyesAssessment by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedEyesAssessment by EncounterID.</returns>
        public async Task<IEnumerable<IdentifiedEyesAssessment>> GetIdentifiedEyesAssessmentByEncounter(Guid encounterId)
        {
            try
            {
                return await context.IdentifiedEyesAssessments.Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
               .Join(
                   context.Encounters.AsNoTracking(),
                identifiedEyesAssessment => identifiedEyesAssessment.EncounterId,
                   encounter => encounter.Oid,
                   (identifiedEyesAssessment, encounter) => new IdentifiedEyesAssessment
                   {
                       EncounterId = identifiedEyesAssessment.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       DateModified = identifiedEyesAssessment.DateModified,
                       InteractionId = identifiedEyesAssessment.InteractionId,
                       EncounterType = identifiedEyesAssessment.EncounterType,
                       DateCreated = identifiedEyesAssessment.DateCreated,
                       AssessmentId = identifiedEyesAssessment.AssessmentId,
                       CreatedBy = identifiedEyesAssessment.CreatedBy,
                       CreatedIn = identifiedEyesAssessment.CreatedIn,
                       EyesCondition = identifiedEyesAssessment.EyesCondition,
                       IsDeleted = identifiedEyesAssessment.IsDeleted,
                       IsSynced = identifiedEyesAssessment.IsSynced,
                       ModifiedBy = identifiedEyesAssessment.ModifiedBy,
                       ModifiedIn = identifiedEyesAssessment.ModifiedIn,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedEyesAssessment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == identifiedEyesAssessment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedEyesAssessment by AssessmentID.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedEyesAssessment by AssessmentID.</returns>
        public async Task<IEnumerable<IdentifiedEyesAssessment>> ReadIdentifiedEyesAssessmentByAssessment(Guid assessmentId)
        {
            try
            {
                return await context.IdentifiedEyesAssessments.Where(p => p.IsDeleted == false && p.AssessmentId == assessmentId).AsNoTracking()
               .Join(
                   context.Encounters.AsNoTracking(),
                identifiedEyesAssessment => identifiedEyesAssessment.EncounterId,
                   encounter => encounter.Oid,
                   (identifiedEyesAssessment, encounter) => new IdentifiedEyesAssessment
                   {
                       EncounterId = identifiedEyesAssessment.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       DateModified = identifiedEyesAssessment.DateModified,
                       InteractionId = identifiedEyesAssessment.InteractionId,
                       EncounterType = identifiedEyesAssessment.EncounterType,
                       DateCreated = identifiedEyesAssessment.DateCreated,
                       AssessmentId = identifiedEyesAssessment.AssessmentId,
                       CreatedBy = identifiedEyesAssessment.CreatedBy,
                       CreatedIn = identifiedEyesAssessment.CreatedIn,
                       EyesCondition = identifiedEyesAssessment.EyesCondition,
                       IsDeleted = identifiedEyesAssessment.IsDeleted,
                       IsSynced = identifiedEyesAssessment.IsSynced,
                       ModifiedBy = identifiedEyesAssessment.ModifiedBy,
                       ModifiedIn = identifiedEyesAssessment.ModifiedIn,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedEyesAssessment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == identifiedEyesAssessment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}