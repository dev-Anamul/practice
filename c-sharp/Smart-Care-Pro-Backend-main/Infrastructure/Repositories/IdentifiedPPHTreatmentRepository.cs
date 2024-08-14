using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class IdentifiedPPHTreatmentRepository : Repository<IdentifiedPPHTreatment>, IIdentifiedPPHTreatmentRepository
    {
        /// <summary>
        /// Implementation of IIdentifiedPPHTreatmentRepository interface.
        /// </summary>
        public IdentifiedPPHTreatmentRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a IdentifiedPPHTreatment by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPPHTreatments.</param>
        /// <returns>Returns a IdentifiedPPHTreatment if the key is matched.</returns>
        public async Task<IdentifiedPPHTreatment> GetIdentifiedPPHTreatmentByKey(Guid key)
        {
            try
            {
                var identifiedPPHTreatment = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedPPHTreatment != null)
                {
                    identifiedPPHTreatment.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedPPHTreatment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedPPHTreatment.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedPPHTreatment.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedPPHTreatment.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedPPHTreatment.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return identifiedPPHTreatment;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPPHTreatment.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedPPHTreatments.</returns>
        public async Task<IEnumerable<IdentifiedPPHTreatment>> GetIdentifiedPPHTreatments()
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
        /// The method is used to get a IdentifiedPPHTreatment by PPHTreatmentsId.
        /// </summary>
        /// <param name="pphTreatmentsId"></param>
        /// <returns>Returns a IdentifiedPPHTreatment if the PPHTreatmentsId is matched.</returns>
        public async Task<IEnumerable<IdentifiedPPHTreatment>> GetIdentifiedPPHTreatmentByPPHTreatments(Guid pphTreatmentsId)
        {
            try
            {
                return await context.IdentifiedPPHTreatments.Where(p => p.IsDeleted == false && p.PPHTreatmentsId == pphTreatmentsId).AsNoTracking()
               .Join(
                   context.Encounters.AsNoTracking(),
                 identifiedPPHTreatment => identifiedPPHTreatment.EncounterId,
                   encounter => encounter.Oid,
                   (identifiedPPHTreatment, encounter) => new IdentifiedPPHTreatment
                   {
                       EncounterId = identifiedPPHTreatment.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       PPHTreatmentsId = identifiedPPHTreatment.PPHTreatmentsId,
                       ModifiedBy = identifiedPPHTreatment.ModifiedBy,
                       ModifiedIn = identifiedPPHTreatment.ModifiedIn,
                       CreatedBy = identifiedPPHTreatment.CreatedBy,
                       CreatedIn = identifiedPPHTreatment.CreatedIn,
                       DateCreated = identifiedPPHTreatment.DateCreated,
                       DateModified = identifiedPPHTreatment.DateModified,
                       EncounterType = identifiedPPHTreatment.EncounterType,
                       InteractionId = identifiedPPHTreatment.InteractionId,
                       IsDeleted = identifiedPPHTreatment.IsDeleted,
                       IsSynced = identifiedPPHTreatment.IsSynced,
                       TreatmentsOfPPH = identifiedPPHTreatment.TreatmentsOfPPH,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedPPHTreatment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == identifiedPPHTreatment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPPHTreatment by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedPPHTreatment by EncounterID.</returns>
        public async Task<IEnumerable<IdentifiedPPHTreatment>> GetIdentifiedPPHTreatmentByEncounter(Guid encounterId)
        {
            try
            {
                return await context.IdentifiedPPHTreatments.Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
               .Join(
                   context.Encounters.AsNoTracking(),
                 identifiedPPHTreatment => identifiedPPHTreatment.EncounterId,
                   encounter => encounter.Oid,
                   (identifiedPPHTreatment, encounter) => new IdentifiedPPHTreatment
                   {
                       EncounterId = identifiedPPHTreatment.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       PPHTreatmentsId = identifiedPPHTreatment.PPHTreatmentsId,
                       ModifiedBy = identifiedPPHTreatment.ModifiedBy,
                       ModifiedIn = identifiedPPHTreatment.ModifiedIn,
                       CreatedBy = identifiedPPHTreatment.CreatedBy,
                       CreatedIn = identifiedPPHTreatment.CreatedIn,
                       DateCreated = identifiedPPHTreatment.DateCreated,
                       DateModified = identifiedPPHTreatment.DateModified,
                       EncounterType = identifiedPPHTreatment.EncounterType,
                       InteractionId = identifiedPPHTreatment.InteractionId,
                       IsDeleted = identifiedPPHTreatment.IsDeleted,
                       IsSynced = identifiedPPHTreatment.IsSynced,
                       TreatmentsOfPPH = identifiedPPHTreatment.TreatmentsOfPPH,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedPPHTreatment.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == identifiedPPHTreatment.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}